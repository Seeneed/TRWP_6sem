# Сборка приложения (self-contained, single-file, linux-x64)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish TDWA07-01.csproj -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -o /app/publish

# Финальный образ (только runtime-deps, без полного .NET runtime)
FROM mcr.microsoft.com/dotnet/runtime-deps:9.0
WORKDIR /app
COPY --from=build /app/publish/ .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["./TDWA07-01"]