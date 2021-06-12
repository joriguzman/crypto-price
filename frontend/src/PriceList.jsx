import PriceRow from './PriceRow';

const PriceList = ({prices}) => {
  const priceRows = prices
    .filter(price => price.key === 'Binance:BTCUSDT')
    .map(price => <PriceRow key={price.key} price={price} />);

  return (
    <div>
      <table>
        <thead>
          <tr>
          <th>Exchange</th>
          <th>Symbol</th>
          <th>Last Price</th>
          <th>24h Change %</th>
          </tr>
        </thead>
        <tbody>
          {priceRows}
        </tbody>
      </table>
    </div>
  );
}

export default PriceList;