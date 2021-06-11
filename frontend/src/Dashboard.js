import React, { useState, useEffect } from 'react';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

function Dashboard({apiUrl}) {
  const [connection, setConnection] = useState(null);
  const [prices, setPrices] = useState({});

  const streamPrices = () => {
    connection.invoke('StreamPrices')
      .catch(error => console.error('StreamPrices error: ', error));
  };

  const stopStreaming = () => {
    connection.invoke('StopStreaming')
      .catch(error => console.error('StopStreaming error: ', error));
  };

  const updatePrice = price => {
    let newPrices = prices;
    const key = price.key;
    newPrices[key] = price;
    setPrices(oldPrices => ({...oldPrices, ...newPrices}));
  }

  useEffect(() => {
    const newConnection = new HubConnectionBuilder()
      .withUrl(apiUrl)
      .configureLogging(LogLevel.Debug)
      // .withAutomaticReconnect()
      .build();

    newConnection.start()
      .then(() => {
        console.log('Connected!');

        newConnection.on('UpdatePrice', price => {
          console.log('UpdatePrice: ', JSON.stringify(price));
          updatePrice(price);
        });
      })
      .catch(e => console.log('Connection failed: ', e));

      setConnection(newConnection);
  }, []);

  return (
    <div>
      <Header message={'Ticker prices'} />
      <Button handleClick={streamPrices} text="Stream prices"></Button>&nbsp;&nbsp;
      <Button handleClick={stopStreaming} text="Stop streaming"></Button>
      <div>
        <PriceList prices={Object.values(prices)} />
      </div>
    </div>
  );
}

const Button = ({handleClick, text}) => {
  return (
    <button onClick={handleClick}>
      {text}
    </button>
  );
}

const Header = ({message}) => {
    return (
        <h3>{message}</h3>
    );
}

const PriceList = ({prices}) => {
  return (
    <div>
      <table>
        <thead>
          <tr>
          <th>Exchange</th>
          <th>Symbol </th>
          <th>Last Price</th>
          <th>24h Change %</th>
          </tr>
        </thead>
        <tbody>
          {prices.map(price => <PriceRow key={price.key} price={price} />)}
        </tbody>
      </table>
    </div>
  );
}

const PriceRow = ({price}) => {
  const { exchange, symbol, lastPrice, priceChangePercent } = price;
  return (
    <tr id={price.key}>
      <td>{exchange}</td>
      <td>{symbol.toUpperCase()}</td>
      <td>{lastPrice.toFixed(2)}</td>
      <td>{(priceChangePercent * 100).toFixed(2) + '%'}</td>
    </tr>
  );
}

export default Dashboard;