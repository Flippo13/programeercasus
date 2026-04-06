FROM mcr.microsoft.com/dotnet/runtime:9.0
WORKDIR /App

COPY . . / 

RUN dotnet restore
RUN dotnet publish -o out 

FROM mcr.microsoft.com/dotnet/runtime:9.0
WORKDIR /App
COPY --from=build /App/out . 
ENTRYPOINT  ["dotnet", "LumenAPI.dll"]