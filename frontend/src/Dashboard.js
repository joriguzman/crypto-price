import React, { useState, useEffect } from 'react';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import PriceList from './PriceList';
import ConnectionStatus from './ConnectionStatus';

function Dashboard({apiUrl}) {
  const [connection, setConnection] = useState(null);
  const [prices, setPrices] = useState({});
  const [connectionError, setConnectionError] = useState(false);

  const startStreaming = () => {
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
      .build();

    newConnection.start()
      .then(() => {
        console.log('Connected to URL: ', apiUrl);

        newConnection.on('UpdatePrice', price => {
          updatePrice(price);
        });

        newConnection.onclose(() => {
          setConnectionError(true);
        });
      })
      .then(() => newConnection.invoke('StreamPrices'))
      .catch(e => {
        console.error('Connection failed: ', e);
        setConnectionError(true);
      });

    setConnection(newConnection);
  }, []);

  return (
    <div className='container'>
      <div className='card'>
        <div className='card-header'>
          <div className='card-header-title'>
            Live Cryptocurrency Prices &nbsp;&nbsp;
            <Button handleClick={startStreaming} text="Start streaing"></Button>&nbsp;&nbsp;
            <Button handleClick={stopStreaming} text="Stop streaming"></Button>
          </div>
        </div>
        <div className='card-content'>
          <PriceList prices={Object.values(prices)} />
        </div>
        <ConnectionStatus connectionError={connectionError}/>
      </div>
    </div>
  );
}

const Button = ({handleClick, text}) => {
  return (
    <button className='button is-primary is-small' onClick={handleClick}>
      {text}
    </button>
  );
}

export default Dashboard;