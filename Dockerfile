FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Presentation/LibraryApp.API/LibraryApp.API.csproj", "Presentation/LibraryApp.API/"]
COPY ["src/Core/LibraryApp.Application/LibraryApp.Application.csproj", "Core/LibraryApp.Application/"]
COPY ["src/Core/LibraryApp.Domain/LibraryApp.Domain.csproj", "Core/LibraryApp.Domain/"]
COPY ["src/Infrastructure/LibraryApp.DAL/LibraryApp.DAL.csproj", "Infrastructure/LibraryApp.DAL/"]
RUN dotnet restore "Presentation/LibraryApp.API/LibraryApp.API.csproj"
COPY ./src .
WORKDIR "/src/Presentation/LibraryApp.API"
RUN dotnet publish "LibraryApp.API.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/publish .
CMD ["dotnet", "LibraryApp.API.dll"]

EXPOSE 5000
EXPOSE 5001