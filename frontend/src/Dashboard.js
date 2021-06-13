import React, { useState, useEffect } from 'react';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import PriceList from './PriceList';
import ConnectionStatus from './ConnectionStatus';

function Dashboard({apiUrl}) {
  const [prices, setPrices] = useState({});
  const [connectionError, setConnectionError] = useState(false);

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
  }, []);

  return (
    <div className='container'>
      <div className='card'>
        <div className='card-header'>
          <div className='card-header-title'>
            Live Cryptocurrency Prices
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

export default Dashboard;