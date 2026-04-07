FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /lumen-backend

COPY . ./ 

RUN dotnet restore
RUN dotnet publish -o out 

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /lumen-backend
COPY --from=build /lumen-backend/out . 
ENTRYPOINT  ["dotnet", "LumenAPI.dll"]