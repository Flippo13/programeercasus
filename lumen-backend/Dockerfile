FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /lumen-backend

COPY . ./ 

RUN dotnet restore
RUN dotnet publish -o out 
RUN ls -l /lumen-backend/out  # Debug: List files in the output directory
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /lumen-backend
COPY --from=build /lumen-backend/out . 
ENTRYPOINT  ["dotnet", "Lumen.dll"]