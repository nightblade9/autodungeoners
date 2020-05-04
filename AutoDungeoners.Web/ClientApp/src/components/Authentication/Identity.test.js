import React from 'react';
import ReactDOM from 'react-dom';
import { MemoryRouter } from 'react-router-dom';
import { Identity } from './Identity';

it('Identity renders without crashing', async () => {
  const div = document.createElement('div');
  ReactDOM.render(
    <MemoryRouter>
      <Identity />
    </MemoryRouter>, div);
  await new Promise(resolve => setTimeout(resolve, 1000));
});
