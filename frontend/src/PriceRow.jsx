import { useEffect, useRef } from 'react';

const PriceRow = ({price}) => {
  const { exchange, symbol, lastPrice, priceChangePercent, volume } = price;
  const prevPriceRef = useRef(0);
  const prevStyleRef = useRef('');

  useEffect(() => {
    prevPriceRef.current = lastPrice;
    prevStyleRef.current = priceStyle;
  });
  const previousPrice = prevPriceRef.current;

  let priceStyle;
  if (lastPrice < previousPrice) {
    priceStyle = 'red';
  } else if (lastPrice > previousPrice) {
    priceStyle = 'green';
  } else {
    priceStyle = prevStyleRef.current;
  }

  const getPriceChangeStyle = () => {
    return priceChangePercent > 0 ? 'green' :
      priceChangePercent < 0 ? 'red' : null;
  }

  return (
    <tr id={price.key}>
      <td>{exchange}</td>
      <td>{symbol.toUpperCase()}</td>
      <td className={priceStyle}>{lastPrice.toLocaleString()}</td>
      <td className={getPriceChangeStyle()}>{(priceChangePercent * 100).toFixed(2) + '%'}</td>
      <td>{volume.toLocaleString()}</td>
    </tr>
  );
}

export default PriceRow;