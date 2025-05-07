# Stage 1: Build the Blazor WebAssembly project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

RUN apt-get update && apt-get install -y python3 python3-pip python3-venv

# Ensure python is available in PATH
RUN ln -s /usr/bin/python3 /usr/bin/python

WORKDIR /app

COPY . ./
# BEFORE dotnet publish step
RUN dotnet workload restore

RUN dotnet publish -c Release -o /app/publish

# Stage 2: Define output for the build artifacts
FROM alpine AS output
WORKDIR /output
COPY --from=build /app/publish/wwwroot /output/wwwroot
