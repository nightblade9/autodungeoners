import React from 'react';
import ReactDOM from 'react-dom';
import { MemoryRouter } from 'react-router-dom';
import { Identity } from './Identity';

it('Identity renders without crashing and shows Unauthenticated', async () => {
  const div = document.createElement('div');
  ReactDOM.render(
    <MemoryRouter>
      <Identity token={null} />
    </MemoryRouter>, div);
  await new Promise(resolve => setTimeout(resolve, 1000));
  expect(div.textContent.includes("Unauthenticated"));
});