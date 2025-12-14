ARG AUTH_KEY;

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ./DeviceControlService.Application/DeviceControlService.Application.csproj ./DeviceControlService.Application/DeviceControlService.Application.csproj
COPY ./DeviceControlService.Domain/DeviceControlService.Domain.csproj ./DeviceControlService.Domain/DeviceControlService.Domain.csproj
COPY ./DeviceControlService.Infrastructure/DeviceControlService.Infrastructure.csproj ./DeviceControlService.Infrastructure/DeviceControlService.Infrastructure.csproj
RUN dotnet restore ./DeviceControlService.Application/DeviceControlService.Application.csproj

FROM build AS publish 
WORKDIR /src
COPY . .
RUN dotnet publish ./DeviceControlService.Application/DeviceControlService.Application.csproj -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 as release
ARG AUTH_KEY
WORKDIR /app
ENV Auth__Key=${AUTH_KEY}
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DeviceControlService.Application.dll"]



