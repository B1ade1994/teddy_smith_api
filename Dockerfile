FROM mcr.microsoft.com/dotnet/sdk:9.0 AS development
WORKDIR /app

RUN dotnet tool install --global dotnet-ef --version 9.0.0
ENV PATH="${PATH}:/root/.dotnet/tools"

COPY ["teddy_smith_api.csproj", "./"]
RUN dotnet restore "teddy_smith_api.csproj"

COPY . .

EXPOSE 8080

CMD ["dotnet", "watch", "run", "--no-launch-profile"]
