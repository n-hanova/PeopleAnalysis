#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["AnalyticAPI/AnalyticAPI.csproj", "AnalyticAPI/"]
COPY ["CommonCoreLibrary/CommonCoreLibrary.csproj", "CommonCoreLibrary/"]
COPY ["PeopleAnalysisML.Model/PeopleAnalysisML.Model.csproj", "PeopleAnalysisML.Model/"]
RUN dotnet restore "AnalyticAPI/AnalyticAPI.csproj"
COPY . .
WORKDIR "/src/AnalyticAPI"
RUN dotnet build "AnalyticAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AnalyticAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY wait /wait
RUN chmod +x /wait
COPY /cert /root/.dotnet/https
COPY PeopleAnalysisML.Model/MLModel.zip /ml/
ENTRYPOINT /wait && dotnet AnalyticAPI.dll