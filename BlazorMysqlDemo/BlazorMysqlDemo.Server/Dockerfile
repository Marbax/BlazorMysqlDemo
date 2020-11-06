#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BlazorMysqlDemo.Server/BlazorMysqlDemo.Server.csproj", "BlazorMysqlDemo.Server/"]
COPY ["BlazorMysqlDemo.DataLibrary/BlazorMysqlDemo.DataLibrary.csproj", "BlazorMysqlDemo.DataLibrary/"]
RUN dotnet restore "BlazorMysqlDemo.Server/BlazorMysqlDemo.Server.csproj"
COPY . .
WORKDIR "/src/BlazorMysqlDemo.Server"
RUN dotnet build "BlazorMysqlDemo.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlazorMysqlDemo.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorMysqlDemo.Server.dll"]