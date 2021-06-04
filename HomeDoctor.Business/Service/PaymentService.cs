using HomeDoctor.Business.IService;
using HomeDoctor.Business.Repositories;
using HomeDoctor.Business.UnitOfWork;
using HomeDoctor.Business.ViewModel.RequestModel;
using HomeDoctor.Business.VnPay;
using HomeDoctor.Data.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HomeDoctor.Business.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepositoryBase<Contract> _repoContract;
        private IConfiguration _configuration { get; }

        public PaymentService(IUnitOfWork uow, IConfiguration configuration)
        {
            _uow = uow;
            _configuration = configuration;
            _repoContract = _uow.GetRepository<Contract>();
        }

        public async Task<string> PayContract(OderPaymentRequest orderInfor)
        {
            string vnp_TmnCode = _configuration["VnPay:vnp_TmnCode"]; //Ma website
            string vnp_HashSecret = _configuration["VnPay:vnp_HashSecret"]; //Chuoi bi mat
            string vnp_Returnurl = "https://vnpay.vn/"; //URL nhan ket qua tra ve 
            string vnp_Url = _configuration["VnPay:vnp_Url"]; ;

            if (string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret))
            {               
                return "vnp_TmpCode and vnp_HashSecret not found";
            }
            OrderInfo order = new OrderInfo();
            order.OrderId = DateTime.Now.Ticks;
            order.Amount = Convert.ToInt64(orderInfor.Amount);
            order.OrderDescription = orderInfor.OrderDescription;
            order.CreatedDate = DateTime.Now;
            //string locale = cboLanguage.SelectedItem.Value;
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", "2.0.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString());           
            vnpay.AddRequestData("vnp_BankCode", order.BankCode);
            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
            vnpay.AddRequestData("vnp_Locale", "vn");

            vnpay.AddRequestData("vnp_OrderInfo", order.OrderDescription);
            vnpay.AddRequestData("vnp_OrderType", "Thanh toán hợp đồng tại hệ thống HOME DOCTOR"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString());

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);            
            return paymentUrl;
        }

        public async Task<string> CheckPaymentStatus(int contractId,string urlRespone)
        {
            if (!string.IsNullOrEmpty(urlRespone)) {
                Uri uri = new Uri(urlRespone);
                var vnpayData = HttpUtility.ParseQueryString(uri.Query);
                string returnContent = string.Empty;
                if (vnpayData.Count > 0)
                {
                    string vnp_HashSecret = _configuration["VnPay:vnp_HashSecret"]; //Secret key
                    VnPayLibrary vnpay = new VnPayLibrary();


                    foreach (string s in vnpayData)
                    {
                        //get all querystring data
                        if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                        {
                            vnpay.AddResponseData(s, vnpayData[s]);
                        }
                    }
                    //Lay danh sach tham so tra ve tu VNPAY
                    //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
                    //vnp_TransactionNo: Ma GD tai he thong VNPAY
                    //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                    //vnp_SecureHash: SHA256 cua du lieu tra ve

                    long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                    long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                    string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                    string vnp_SecureHash = vnpayData["vnp_SecureHash"];
                    bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

                    if (checkSignature)
                    {
                        //Cap nhat ket qua GD
                        //Yeu cau: Truy van vao CSDL cua  Merchant => lay ra duoc OrderInfo
                        //Giả sử OrderInfo lấy ra được như giả lập bên dưới
                        OrderInfo order = new OrderInfo();
                        order.OrderId = orderId;
                        order.vnp_TransactionNo = vnpayTranId;
                        order.Status = 0; //0: Cho thanh toan,1: da thanh toan,2: GD loi
                                          //Kiem tra tinh trang Order
                        if (order != null)
                        {
                            if (order.Status == 0)
                            {
                                if (vnp_ResponseCode == "00")
                                {
                                    //Thanh toan thanh cong                                    
                                    order.Status = 1;
                                }
                                else
                                {
                                    //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                                    //  displayMsg.InnerText = "Có lỗi xảy ra trong quá trình xử lý.Mã lỗi: " + vnp_ResponseCode;                                    
                                    order.Status = 2;
                                }

                                //Thêm code Thực hiện cập nhật vào Database 
                                //Update Database
                                var contract = await _repoContract.GetById(contractId);
                                if(contract != null)
                                {
                                    contract.TransactionNo = vnpayTranId.ToString();
                                    if(await _repoContract.Update(contract))
                                    {
                                        await _uow.CommitAsync();
                                    }
                                }

                                returnContent = "{\"RspCode\":\"00\",\"Message\":\"Confirm Success\"}";
                            }
                            else
                            {
                                returnContent = "{\"RspCode\":\"02\",\"Message\":\"Order already confirmed\"}";
                            }
                        }
                        else
                        {
                            returnContent = "{\"RspCode\":\"01\",\"Message\":\"Order not found\"}";
                        }
                    }
                    else
                    {
                        returnContent = "{\"RspCode\":\"97\",\"Message\":\"Invalid signature\"}";
                    }
                }
                else
                {
                    returnContent = "{\"RspCode\":\"99\",\"Message\":\"Input data required\"}";
                }
                return returnContent;
            }
            return null;
        }
    }
}
