# ============================
# Stage 1: Restore + Build
# ============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files
COPY EzBill/EzBill.csproj ./EzBill/
COPY EzBill.Application/EzBill.Application.csproj ./EzBill.Application/
COPY EzBill.Domain/EzBill.Domain.csproj ./EzBill.Domain/
COPY EzBill.Infrastructure/EzBill.Infrastructure.csproj ./EzBill.Infrastructure/

# Restore
RUN dotnet restore ./EzBill/EzBill.csproj

# Copy full source
COPY . .

# Publish
RUN dotnet publish EzBill/EzBill.csproj -c Release -o /app/publish

# ============================
# Stage 2: Runtime
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "EzBill.dll"]
