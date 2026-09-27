FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY KinoPoshuk.sln .
COPY KinoPoshuk.csproj .
COPY KinoPoshuk.BLL/KinoPoshuk.BLL.csproj KinoPoshuk.BLL/
COPY KinoPoshuk.DAL/KinoPoshuk.DAL.csproj KinoPoshuk.DAL/
COPY KinoPoshuk.Tests/KinoPoshuk.Tests.csproj KinoPoshuk.Tests/
RUN dotnet restore KinoPoshuk.csproj

COPY . .
RUN dotnet publish KinoPoshuk.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080
ENTRYPOINT ["dotnet", "KinoPoshuk.dll"]
