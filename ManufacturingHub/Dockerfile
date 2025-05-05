# Stage 1: Build the Blazor WebAssembly project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY . ./
RUN dotnet publish -c Release -o /app/publish


# Stage 2: Serve with Nginx
FROM nginx:alpine
# Copy Blazor files
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html

# Copy SSL certs and nginx config
COPY nginx.conf /etc/nginx/nginx.conf
COPY nginx.crt /etc/nginx/ssl/nginx.crt
COPY nginx.key /etc/nginx/ssl/nginx.key

EXPOSE 80
EXPOSE 443
CMD ["nginx", "-g", "daemon off;"]

# Stage 1: Build the application
# FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
# WORKDIR /app

# # Copy csproj and restore dependencies
# COPY *.csproj ./
# RUN dotnet restore

# # Copy the rest of the application
# COPY . ./

# # Build the application
# RUN dotnet publish -c Release -o /app/publish

# # Stage 2: Create the runtime image
# FROM mcr.microsoft.com/dotnet/aspnet:9.0
# WORKDIR /app
# COPY --from=build /app/publish ./

# ENV ASPNETCORE_URLS=https://+:443;http://+:8080
# ENV ASPNETCORE_HTTPS_PORT=443

# # Copy HTTPS certificate
# COPY certificate.pfx /app/
# ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/certificate.pfx
# ENV ASPNETCORE_Kestrel__Certificates__Default__Password=PfxPassword

# EXPOSE 8080
# EXPOSE 443

# # Define the entry point for the container
# ENTRYPOINT ["dotnet", "BlazeOrbital.ManufacturingHub.dll"] 

