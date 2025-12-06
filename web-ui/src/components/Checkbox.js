import React, { useState, useEffect } from 'react';
import './Checkbox.css';

const Checkbox = ({ 
  id, 
  checked = false, 
  onChange, 
  label, 
  disabled = false,
  color = '#3498db',
  size = 'medium'
}) => {
  const [isChecked, setIsChecked] = useState(checked);
  const [isFocused, setIsFocused] = useState(false);

  // Update internal state when prop changes
  useEffect(() => {
    setIsChecked(checked);
  }, [checked]);

  const handleChange = (e) => {
    if (disabled) return;
    
    const newChecked = !isChecked;
    setIsChecked(newChecked);
    
    if (onChange) {
      onChange(newChecked, e);
    }
  };

  const handleKeyPress = (e) => {
    if (e.key === 'Enter' || e.key === ' ') {
      e.preventDefault();
      handleChange(e);
    }
  };

  const handleFocus = () => setIsFocused(true);
  const handleBlur = () => setIsFocused(false);

  const checkboxId = id || `checkbox-${Math.random().toString(36).substr(2, 9)}`;

  return (
    <div className={`checkbox-wrapper ${disabled ? 'disabled' : ''} size-${size}`}>
      <input
        type="checkbox"
        id={checkboxId}
        checked={isChecked}
        onChange={handleChange}
        disabled={disabled}
        className="checkbox-input"
        onKeyPress={handleKeyPress}
        onFocus={handleFocus}
        onBlur={handleBlur}
      />
      <label 
        htmlFor={checkboxId} 
        className="checkbox-label"
        style={isFocused ? { '--focus-color': `${color}40` } : null}
      >
        <span 
          className="checkbox-custom"
          style={{
            '--checkbox-color': color,
            '--checkbox-border': isChecked ? color : '#bdc3c7',
            '--checkbox-bg': isChecked ? color : 'white'
          }}
        >
          {isChecked && (
            <svg 
              className="checkmark" 
              viewBox="0 0 12 10"
              style={{ '--checkmark-color': 'white' }}
            >
              <path 
                d="M1 5L4 8L11 1" 
                stroke="var(--checkmark-color)" 
                strokeWidth="2" 
                fill="none" 
                strokeLinecap="round" 
                strokeLinejoin="round"
              />
            </svg>
          )}
        </span>
        {label && <span className="checkbox-text">{label}</span>}
      </label>
    </div>
  );
};

export default Checkbox;