# Use the official .NET Core SDK as a parent image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project file and restore any dependencies (use .csproj for the project name)
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the application code
COPY . .

# Restore any tools (e.g., dotnet-ef)
RUN dotnet tool restore

# Run the EF Core migration to create the database schema
RUN dotnet ef migrations add InitialCreate --context Athenticate.Database.DataContext
RUN dotnet ef database update --context Athenticate.Database.DataContext

# Publish the application
RUN dotnet publish -c Release -o out

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Expose the port your application will run on
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "Athenticate.dll"]
