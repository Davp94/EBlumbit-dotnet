FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5071

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["EBlumbit.csproj", "."]
COPY . .
WORKDIR "/src/."
RUN dotnet build "./Eblumbit.csproj" -o /app/build


FROM base AS final
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT [ "dotnet", "Eblumbit.dll" ]