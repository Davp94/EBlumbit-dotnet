FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["EBlumbit.csproj", "."]
RUN dotnet restore "EBlumbit.csproj"

COPY . .
RUN dotnet publish "EBlumbit.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5071
EXPOSE 5071

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "EBlumbit.dll"]
