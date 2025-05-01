# Use the official .NET runtime image to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GooglePlaySubscriptionChecker.csproj", "./"]
RUN dotnet restore "./GooglePlaySubscriptionChecker.csproj"
COPY . .
RUN dotnet publish "GooglePlaySubscriptionChecker.csproj" -c Release -o /app/publish

# Final stage: copy the published app and start it
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GooglePlaySubscriptionChecker.dll"]
