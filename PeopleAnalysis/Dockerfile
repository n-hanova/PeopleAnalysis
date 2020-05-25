#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["PeopleAnalysis/PeopleAnalysis.csproj", "PeopleAnalysis/"]
COPY ["CommonCoreLibrary/CommonCoreLibrary.csproj", "CommonCoreLibrary/"]
RUN dotnet restore "PeopleAnalysis/PeopleAnalysis.csproj"
COPY . .
WORKDIR "/src/PeopleAnalysis"
RUN dotnet build "PeopleAnalysis.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PeopleAnalysis.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY wait /wait
RUN chmod +x /wait
COPY /cert /root/.dotnet/https
ENTRYPOINT /wait && dotnet PeopleAnalysis.dll