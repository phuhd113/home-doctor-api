using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HomeDoctor.Data.Models
{
	public class License
	{
		public int LicenseId { get; set; }
		[Required]
		[Column(TypeName = "nvarchar(50)")]
		public string Name { get; set; }
		[Required]
		public int Days { get; set; }
		[Required]
		[Column(TypeName = "Money")]
		public decimal Price { get; set; }
		[Column(TypeName = "nvarchar(255)")]
		public string Description { get; set; }
		[Required]
		[Column(TypeName = "varchar(10)")]
		public string Status { get; set; }
		[Column(TypeName = "datetime")]
		public DateTime? DateActive { get; set; }
		[Column(TypeName = "datetime")]
		public DateTime? DateCancel { get; set; }
		public int? FromBy { get; set; }
		

	}
}
