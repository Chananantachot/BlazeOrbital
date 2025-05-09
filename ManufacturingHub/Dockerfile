
# Stage 1: Build the Blazor WebAssembly project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

RUN apt-get update && apt-get install -y python3 python3-pip python3-venv

# Ensure python is available in PATH
RUN ln -s /usr/bin/python3 /usr/bin/python

WORKDIR /app

COPY . ./
RUN dotnet workload restore
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Define output for the build artifacts
FROM nginx:alpine

COPY --from=build /app/publish/wwwroot /usr/share/nginx/html

# Copy SSL certs and nginx config
COPY ./nginx.conf /etc/nginx/nginx.conf
COPY ./nginx.crt /etc/nginx/ssl/nginx.crt
COPY ./nginx.key /etc/nginx/ssl/nginx.key

EXPOSE 80
EXPOSE 443
CMD ["nginx", "-g", "daemon off;"]