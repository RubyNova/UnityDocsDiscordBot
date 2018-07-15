FROM microsoft/dotnet:latest
COPY ./UnityDocsBotConsole /app
ADD ./UnityDocsBot /UnityDocsBot
RUN cd app && dotnet restore
WORKDIR /app
ENTRYPOINT ["dotnet", "run"]