# crypto-price
Displays real-time prices of major cryptocurrencies. Uses React for frontend, .NET Core and SignalR for backend.

Backend service subscribes to Bitfinex and Binance WebSockets to retrieve prices.

Start backend service first before opening web page. If not started, "Cannot connect to server" is shown.

Console app "CryptoPrice.TestConsole" included for testing price subscription to data sources.

To run:
1. Web Service
- In terminal window, go to "backend" directory then run `dotnet run ./CryptoPrice.Api.csproj`
2. Web UI
- In terminal window, go to "frontend" directory then run `npm install`
- Run `npm start`
- Navigate to http://localhost:3000/

![Screenshot](https://github.com/joriguzman/crypto-price/blob/main/screenshot.PNG)
