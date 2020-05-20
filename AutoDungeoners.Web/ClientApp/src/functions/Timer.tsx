import { useState, useEffect } from 'react';

type args = {
  intervalSeconds: number,
  callback: () => void
};

export const Timer = ({intervalSeconds, callback} : args) => {
  const [seconds] = useState(0);

  useEffect(() => {
    let interval = setInterval(callback, intervalSeconds * 1000);
    return () => clearInterval(interval);
  }, [seconds, intervalSeconds, callback]);
  
  return null; // no visual display
}

export default Timer;