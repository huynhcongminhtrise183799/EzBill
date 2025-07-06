# Stage 1 - Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["EzBill.csproj", "./"]
RUN dotnet restore "EzBill.csproj"

COPY . .
RUN dotnet build "EzBill.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 2 - Publish
FROM build AS publish
RUN dotnet publish "EzBill.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 3 - Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EzBill.dll"]
