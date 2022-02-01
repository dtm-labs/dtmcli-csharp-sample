FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 9090

FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build
WORKDIR /src
COPY ./DtmSample .
RUN dotnet restore "DtmSample.sln"
WORKDIR /src/DtmSample
RUN dotnet build "DtmSample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DtmSample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV TZ=Asia/Shanghai

ENTRYPOINT ["dotnet", "DtmSample.dll"]
