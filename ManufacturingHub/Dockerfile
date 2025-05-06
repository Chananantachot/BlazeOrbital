# Stage 1: Build the Blazor WebAssembly project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY . ./
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Define output for the build artifacts
FROM alpine AS output
WORKDIR /output
COPY --from=build /app/publish/wwwroot /output/wwwroot
