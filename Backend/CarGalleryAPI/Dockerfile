FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY CarGalleryAPI/CarGalleryAPI.csproj CarGalleryAPI/
RUN dotnet restore CarGalleryAPI/CarGalleryAPI.csproj
COPY CarGalleryAPI/ CarGalleryAPI/
WORKDIR /src/CarGalleryAPI
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "CarGalleryAPI.dll"]
