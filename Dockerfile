FROM mcr.microsoft.com/dotnet/sdk as build-env
WORKDIR /app
COPY DiscordRadio /app
RUN dotnet restore
RUN dotnet publish --nologo --configuration Release

FROM mcr.microsoft.com/dotnet/runtime
WORKDIR /app
COPY --from=build-env /app/DiscordRadio/bin/Release/net6.0/publish /app
RUN apt update \
    && apt install ffmpeg -y \
    && apt install libopus-dev -y

COPY config.json /app
CMD dotnet DiscordRadio.dll
