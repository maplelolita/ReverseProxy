FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["ReverseProxy/ReverseProxy.csproj", "ReverseProxy/"]
RUN dotnet restore "ReverseProxy/ReverseProxy.csproj"
COPY . .
WORKDIR "/src/ReverseProxy"
RUN dotnet build "ReverseProxy.csproj" -c Release -o /app/build 

FROM build AS publish
RUN dotnet publish "ReverseProxy.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReverseProxy.dll"]
