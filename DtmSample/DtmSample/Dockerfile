#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 9090

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DtmSample/DtmSample.csproj", "DtmSample/"]
RUN dotnet restore "DtmSample/DtmSample.csproj"
COPY . .
WORKDIR "/src/DtmSample"
RUN dotnet build "DtmSample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DtmSample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DtmSample.dll"]
