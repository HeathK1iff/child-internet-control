import React, { useState, useEffect } from 'react';
import ListItem from './ListItem';
import './ListContainer.css';

const ListContainer = ({ onDeviceUpdate }) => {
  const [devices, setDevices] = useState([]);
  const [allDevices, setAllDevices] = useState([]);
  const [selectedDevice, setSelectedDevice] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [visibleCount, setVisibleCount] = useState(8);
  const [hasMore, setHasMore] = useState(false);

  // Calculate statistics
  const onlineDevices = devices.filter(device => device.hasInternet).length;
  const offlineDevices = devices.length - onlineDevices;

  // Notify parent component about device changes
  useEffect(() => {
    if (onDeviceUpdate) {
      onDeviceUpdate(onlineDevices, offlineDevices);
    }
  }, [onlineDevices, offlineDevices, onDeviceUpdate]);

  console.log(process.env.REACT_APP_API_URL);
  

  // Load devices from default URL on component mount
  useEffect(() => {
    loadDevicesFromUrl(process.env.REACT_APP_API_URL);
  }, []);

  // Update visible devices when allDevices or visibleCount changes
  useEffect(() => {
    if (allDevices.length > 0) {
      setDevices(allDevices.slice(0, visibleCount));
      setHasMore(visibleCount < allDevices.length);
    }
  }, [allDevices, visibleCount]);

  const loadDevicesFromUrl = async (url) => {
    setLoading(true);
    setError(null);
    
    try {
      console.log(`Loading devices from: ${url}`);
      const response = await fetch(`${url}/api/v1/devices`, {
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
          'X-Auth': process.env.REACT_APP_AUTH_KEY
        }
      });
      
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      
      const data = await response.json();
      
      // Transform API data to our device format
      const transformedDevices = transformApiDataToDevices(data);
      setAllDevices(transformedDevices);
      setVisibleCount(8); // Reset to initial view count
      
    } catch (err) {
      setError(`Failed to load devices from ${url}. Error: ${err.message}`);
      console.error('Error loading devices:', err);
      
    } finally {
      setLoading(false);
    }
  };

  const transformApiDataToDevices = (apiData) => {
    // Colors for devices
    const colors = ['#3498db', '#2ecc71', '#9b59b6', '#e74c3c', '#f39c12', '#1abc9c', '#e67e22', '#34495e'];
    
    // Check if data is array or object
    const dataArray = Array.isArray(apiData.Payload) ? apiData.Payload : [apiData.Payload];
    
    return dataArray.map((item, index) => {
      const colorIndex = index % colors.length;
      
      return {
        id: index + 1,
        deviceName: item.name,
        macAddress: item.mac,
        color: colors[colorIndex],
        hasInternet: item.hasInternet !== undefined ? item.hasInternet : false
      };
    });
  };

   
  const handleCheckboxChange = async (deviceId, hasInternet) => {
    const updatedDevices = devices.map(device =>
      device.id === deviceId ? { ...device, hasInternet } : device
    );
    setDevices(updatedDevices);
    
     var selected = devices.find(a => a.id == deviceId);

     const endpoint = hasInternet ? 'activate' : 'deactivate';

     const response = await fetch(
        `${process.env.REACT_APP_API_URL}/api/v1/devices/${endpoint}?mac=${encodeURIComponent(selected.macAddress)}`,
        {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json',
            'X-Auth': process.env.REACT_APP_AUTH_KEY
          }
        }
      );

    
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }


    // Also update in allDevices
    const updatedAllDevices = allDevices.map(device =>
      device.id === deviceId ? { ...device, hasInternet } : device
    );
    setAllDevices(updatedAllDevices);
  };

  const handleDeviceClick = (deviceId, isActive) => {
    setSelectedDevice(isActive ? deviceId : null);
  };

  const handleLoadMore = () => {
    // Increase visible count by 8, or show all if less than 8 remain
    const newCount = Math.min(visibleCount + 8, allDevices.length);
    setVisibleCount(newCount);
  };

  const handleShowAll = () => {
    setVisibleCount(allDevices.length);
  };

  if (loading) {
    return (
      <main className="list-container">
        <div className="centered-list loading-state">
          <div className="loader">
            <div className="spinner"></div>
            <p>Loading devices from router...</p>
          </div>
        </div>
      </main>
    );
  }

  return (
    <main className="list-container">
      <div className="centered-list">
        <div className="list-header-section">
          <div className="device-stats">
            <div className="stat-item">
              <span className="stat-label">Total:</span>
              <span className="stat-value total">{allDevices.length}</span>
            </div>
            <div className="stat-item">
              <span className="stat-label">Online:</span>
              <span className="stat-value online">{onlineDevices}</span>
            </div>
            <div className="stat-item">
              <span className="stat-label">Offline:</span>
              <span className="stat-value offline">{offlineDevices}</span>
            </div>
            <div className="stat-item">
              <span className="stat-label">Showing:</span>
              <span className="stat-value showing">{devices.length} of {allDevices.length}</span>
            </div>
          </div>
        </div>

          <>
            <ul className="list-items">
              {devices.map((device) => (
                <ListItem
                  key={device.id}
                  id={device.id}
                  deviceName={device.deviceName}
                  macAddress={device.macAddress}
                  color={device.color}
                  hasInternet={device.hasInternet}
                  onCheckboxChange={(hasInternet) => handleCheckboxChange(device.id, hasInternet)}
                  onItemClick={(isActive) => handleDeviceClick(device.id, isActive)}
                  alignCheckboxRight={true}
                />
              ))}
            </ul>

            {hasMore && (
              <div className="load-more-section">
                <button className="load-more-btn" onClick={handleLoadMore}>
                  <i className="fas fa-chevron-down"></i> Load More ({allDevices.length - visibleCount} remaining)
                </button>
                <button className="show-all-btn" onClick={handleShowAll}>
                  <i className="fas fa-expand"></i> Show All ({allDevices.length} devices)
                </button>
              </div>
            )}
          </>
      </div>
    </main>
  );
};

export default ListContainer;