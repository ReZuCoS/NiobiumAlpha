FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY "NiobiumAlpha.Api.csproj" .
RUN dotnet restore "NiobiumAlpha.Api.csproj"

COPY . .

RUN dotnet publish "NiobiumAlpha.Api.csproj" -c Development -o /publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 as final
WORKDIR /src
COPY --from=build /publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "NiobiumAlpha.Api.dll"]
