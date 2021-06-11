import './App.css';
import Dashboard from './Dashboard';

function App() {
  return (
    <div className="App">
      <Dashboard apiUrl="https://localhost:5001/hubs/prices" />
    </div>
  );
}

export default App;
