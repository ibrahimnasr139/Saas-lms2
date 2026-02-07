# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy solution file
COPY *.slnx ./

# Copy all project files
COPY Api/Api.csproj ./Api/
COPY Application/Application.csproj ./Application/
COPY Infrastructure/Infrastructure.csproj ./Infrastructure/
COPY Domain/Domain.csproj ./Domain/

# Restore packages for Api project
WORKDIR /app/Api
RUN dotnet restore

# Copy everything else
WORKDIR /app
COPY . ./

# Build and publish
WORKDIR /app/Api
RUN dotnet publish -c Release -o /app/out

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/out .

# Expose port
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Run the application
ENTRYPOINT ["dotnet", "Api.dll"]