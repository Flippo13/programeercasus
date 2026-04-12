# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /lumen-backend

# Copy only the project file and restore dependencies
COPY ["Lumen.csproj", "./"]
RUN dotnet restore

# Copy the rest of the source files
COPY . .

# Publish only the Lumen project (not the solution)
RUN dotnet publish Lumen.csproj -c Release -o out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /lumen-backend

# Copy the published output
COPY --from=build /lumen-backend/out .

# Set the entry point
ENTRYPOINT ["dotnet", "Lumen.dll"]