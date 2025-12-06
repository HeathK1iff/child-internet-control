import React from 'react';
import './Footer.css';

const Footer = ({ text }) => {
  return (
    <footer className="footer">
      <p>{text}</p>
    </footer>
  );
};

export default Footer;