#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
ADD ["Data", "/Data/"]
COPY ["HomeDoctor.Api/HomeDoctor.Api.csproj", "HomeDoctor.Api/"]
COPY ["HomeDoctor.Data/HomeDoctor.Data.csproj", "HomeDoctor.Data/"]
COPY ["HomeDoctor.Business/HomeDoctor.Business.csproj", "HomeDoctor.Business/"]
RUN dotnet restore "HomeDoctor.Api/HomeDoctor.Api.csproj"
COPY . .
WORKDIR "/src/HomeDoctor.Api"
RUN dotnet build "HomeDoctor.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeDoctor.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeDoctor.Api.dll"]