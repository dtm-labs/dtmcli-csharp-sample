#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DtmTccSample/DtmTccSample.csproj", "DtmTccSample/"]
RUN dotnet restore "DtmTccSample/DtmTccSample.csproj"
COPY . .
WORKDIR "/src/DtmTccSample"
RUN dotnet build "DtmTccSample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DtmTccSample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DtmTccSample.dll"]