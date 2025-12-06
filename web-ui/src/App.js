import React, { useState } from 'react';
import './App.css';
import Header from './components/Header';
import ListContainer from './components/ListContainer';
import Footer from './components/Footer';

function App() {
  const [onlineDevices, setOnlineDevices] = useState(0);
  const [offlineDevices, setOfflineDevices] = useState(0);

  const handleDeviceUpdate = (online, offline) => {
    setOnlineDevices(online);
    setOfflineDevices(offline);
  };

  return (
    <div className="container">
      <Header 
        title="Internet Monitor"
      />
      
      <ListContainer onDeviceUpdate={handleDeviceUpdate} />
      
      <Footer 
        text="Internet Device Monitor &copy; 2026"
      />
    </div>
  );
}

export default App;