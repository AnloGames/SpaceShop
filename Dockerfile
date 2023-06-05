FROM mcr.microsoft.com/dotnet/sdk:6.0

WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o release

WORKDIR /app/release

ENV Urls http://0.0.0.0:5000

CMD ["./SpaceShop"]