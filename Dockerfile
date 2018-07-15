# NOTE: I cannot follow why the other directory is also being carried along for the ride, but I haven't touched it, so it should still work ¯\_(ツ)_/¯

#Builder
FROM microsoft/dotnet:2.1-sdk AS builder
WORKDIR /app

COPY /UnityDocsBotConsole/*.csproj .
RUN dotnet restore

COPY /UnityDocsBotConsole .
COPY /UnityDocsBot /UnityDocsBot
RUN dotnet publish -c Release -o out

# Build the runtime image
FROM microsoft/dotnet:2.1-runtime-alpine
WORKDIR /app

COPY --from=builder /app/out .
COPY --from=builder /UnityDocsBot /UnityDocsBot

ENTRYPOINT [ "dotnet", "UnityDocsBotConsole.dll" ]