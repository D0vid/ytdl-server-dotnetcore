FROM microsoft/dotnet:2.0.0-sdk
WORKDIR /app
RUN mkdir Temp
######################################################################################################################
################################################# INSTALLING FFMPEG ##################################################
RUN apt-get update ; apt-get install -y git build-essential gcc make yasm autoconf automake cmake libtool checkinstall libmp3lame-dev pkg-config libunwind-dev zlib1g-dev libssl-dev

RUN apt-get update \
    && apt-get clean \
    && apt-get install -y --no-install-recommends libc6-dev libgdiplus wget software-properties-common

#RUN RUN apt-add-repository ppa:git-core/ppa && apt-get update && apt-get install -y git

RUN wget https://www.ffmpeg.org/releases/ffmpeg-4.0.2.tar.gz
RUN tar -xzf ffmpeg-4.0.2.tar.gz; rm -r ffmpeg-4.0.2.tar.gz
RUN cd ./ffmpeg-4.0.2; ./configure --enable-gpl --enable-libmp3lame --enable-decoder=mjpeg,png --enable-encoder=png --enable-openssl --enable-nonfree


RUN cd ./ffmpeg-4.0.2; make
RUN  cd ./ffmpeg-4.0.2; make install
######################################################################################################################
######################################################################################################################
COPY . .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet dds-ytdl-server.dll