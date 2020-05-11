import { useState, useEffect } from 'react';

export const Timer = ({intervalSeconds, callback}) => {
  const [seconds] = useState(0);

  useEffect(() => {
    let interval = null;
    interval = setInterval(() => {
        callback();
        }, intervalSeconds * 1000);
    return () => clearInterval(interval);
  }, [seconds, intervalSeconds, callback]);
  
  return null; // no visual display
}

export default Timer;