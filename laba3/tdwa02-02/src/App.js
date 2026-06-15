import React, { useState } from 'react';
import './App.css';

function App() {
  const [inputs, setInputs] = useState({ op: '', x: '', y: '' });
  const [result, setResult] = useState('Результаты запросов будут здесь...');

  const API_URL = 'https://localhost:20443/NGINX-test';
  
  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setInputs(prev => ({ ...prev, [name]: value }));
  };
  
  const performFetch = async (method, body = null) => {
    try {
      const options = { method };
      if (body) {
        options.headers = { 'Content-Type': 'application/json' };
        options.body = JSON.stringify(body);
      }
      
      const response = await fetch(API_URL, options);
      const status = response.status;
      const statusText = response.statusText;
      let responseBody;

      try {
        responseBody = await response.clone().json();
        responseBody = JSON.stringify(responseBody, null, 2);
      } catch (e) {
        responseBody = await response.text();
      }
      
      setResult(`Статус: ${status} ${statusText}\n\nТело ответа:\n${responseBody}`);
    } catch (error) {
      setResult(`Ошибка сети: ${error.message}`);
    }
  };

  const handlePost = () => {
    const { op, x, y } = inputs;
    performFetch('POST', { op, x: parseFloat(x), y: parseFloat(y) });
  };
  
  const handlePut = () => {
    const { op, x, y } = inputs;
    performFetch('PUT', { op, x: parseFloat(x), y: parseFloat(y) });
  };

  return (
    <div className="App">
      <h1>SPA Калькулятор (React)</h1>
      <div className="container">
        <div className="card">
          <h2>Просмотр и удаление</h2>
          <button onClick={() => performFetch('GET')}>GET (Получить)</button>
          <button onClick={() => performFetch('DELETE')}>DELETE (Удалить)</button>
        </div>

        <div className="card">
          <h2>Создание и обновление (POST/PUT)</h2>
          <div className="form-group">
            <label>Операция (op):</label>
            <input type="text" name="op" value={inputs.op} onChange={handleInputChange} placeholder="add, sub, mul, div" />
          </div>
          <div className="form-group">
            <label>Число X:</label>
            <input type="number" name="x" value={inputs.x} onChange={handleInputChange} placeholder="10" />
          </div>
          <div className="form-group">
            <label>Число Y:</label>
            <input type="number" name="y" value={inputs.y} onChange={handleInputChange} placeholder="5" />
          </div>
          <button onClick={handlePost}>POST (Создать)</button>
          <button onClick={handlePut}>PUT (Обновить)</button>
        </div>
      </div>
      
      <div className="result-container">
        <h2>Результат:</h2>
        <pre>{result}</pre>
      </div>
    </div>
  );
}

export default App;