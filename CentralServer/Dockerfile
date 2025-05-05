# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application
COPY . ./

# Build the application
RUN dotnet publish -c Release -o /app/out

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out .

# Set environment variables for HTTPS
ENV ASPNETCORE_URLS=https://+:443;http://+:8080
ENV ASPNETCORE_HTTPS_PORT=443

# Copy HTTPS certificate
COPY certificate.pfx /app/
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/certificate.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=PfxPassword
# Expose port 44423 for HTTP traffic
EXPOSE 8080
EXPOSE 443
# Define the entry point for the container
ENTRYPOINT ["dotnet", "BlazeOrbital.CentralServer.dll"]
