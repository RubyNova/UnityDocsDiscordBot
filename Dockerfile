# NOTE: AH FIXED ET! - RubyNovarinos

#Builder
FROM microsoft/dotnet:2.1-sdk AS builder
WORKDIR /app
COPY /UnityDocsBot /UnityDocsBot
COPY /UnityDocsBotConsole/*.csproj .
RUN dotnet restore

COPY /UnityDocsBotConsole .
RUN dotnet publish -c Release -o out

# Build the runtime image
FROM microsoft/dotnet:2.1-runtime-alpine
WORKDIR /app

COPY --from=builder /app/out .
COPY --from=builder /UnityDocsBot /UnityDocsBot

ENTRYPOINT [ "dotnet", "UnityDocsBotConsole.dll" ]
