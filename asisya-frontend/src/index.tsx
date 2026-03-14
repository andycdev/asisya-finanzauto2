import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App'; // Importa el App que me mostraste

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);