import PriceRow from './PriceRow';

const PriceList = ({prices}) => {
  const priceRows = prices.map(price =>
    <PriceRow key={price.key} price={price} />
  );

  return (
    <div id='price-list'>
      <table className='table is-bordered is-striped is-hoverable'>
        <thead>
          <tr>
            <th>Exchange</th>
            <th>Symbol</th>
            <th>Last Price</th>
            <th>24h Change %</th>
            <th>USD Volume</th>
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