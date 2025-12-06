import React, { useState, useEffect } from 'react';
import Checkbox from './Checkbox';
import './ListItem.css';

const ListItem = ({ 
  id,
  deviceName, 
  macAddress, 
  color, 
  hasInternet = false,
  onCheckboxChange,
  onItemClick,
  alignCheckboxRight = true
}) => {
  const [isActive, setIsActive] = useState(false);
  const [hasInternetChecked, setHasInternetChecked] = useState(hasInternet);
  const [borderColor, setBorderColor] = useState(color);

  useEffect(() => {
    if (borderColor !== color) {
      const timer = setTimeout(() => {
        setBorderColor(color);
      }, 300);
      return () => clearTimeout(timer);
    }
  }, [borderColor, color]);

  const handleItemClick = () => {
    const newActiveState = !isActive;
    setIsActive(newActiveState);
    setBorderColor('#2ecc71');
    
    if (onItemClick) {
      onItemClick(newActiveState);
    }
  };

  const handleCheckboxChange = (checked) => {
    setHasInternetChecked(checked);
    
    if (onCheckboxChange) {
      onCheckboxChange(checked);
    }
  };

  const handleKeyPress = (e) => {
    if (e.key === 'Enter' || e.key === ' ') {
      e.preventDefault();
      handleItemClick();
    }
  };

  const itemStyle = {
    borderLeft: `4px solid ${borderColor}`
  };

  const checkboxId = `device-${id}-checkbox`;

  return (
    <li 
      className={`list-item ${isActive ? 'active' : ''} ${alignCheckboxRight ? 'checkbox-right' : ''}`}
      onClick={handleItemClick}
      style={itemStyle}
      role="button"
      tabIndex={0}
      onKeyPress={handleKeyPress}
      aria-labelledby={`${checkboxId}-label`}
    >
      <div className="item-number" style={{ backgroundColor: color }}>
        {id}
      </div>
      
      <div className="item-content">
        <div className="device-info">
          <div className="device-name" id={`${checkboxId}-label`}>
            {deviceName}
          </div>
          <div className="mac-address">
            {formatMacAddress(macAddress)}
          </div>
        </div>
      </div>
      
      <div className="item-checkbox-container">
        <div className="checkbox-label-wrapper">
          <span className="checkbox-title">Has Internet</span>
          <Checkbox
            id={checkboxId}
            checked={hasInternetChecked}
            onChange={handleCheckboxChange}
            color={color}
            size="medium"
            label=""
          />
        </div>
      </div>
    </li>
  );
};

// Helper function to format MAC address
const formatMacAddress = (mac) => {
  if (!mac) return '00:00:00:00:00:00';
  
  // Remove any existing separators and convert to uppercase
  const cleanMac = mac.replace(/[^a-fA-F0-9]/g, '').toUpperCase();
  
  // Format as XX:XX:XX:XX:XX:XX
  if (cleanMac.length === 12) {
    return cleanMac.match(/.{2}/g).join(':');
  }
  
  return mac; // Return original if can't format
};



export default ListItem;