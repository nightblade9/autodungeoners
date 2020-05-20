import React from 'react';
import { MemoryRouter } from 'react-router-dom';
import ReactDOM from 'react-dom';
// import { render } from '@testing-library/react';
import App from './App';

// test('renders learn react link', () => {
//   const { getByText } = render(<App />);
//   const linkElement = getByText(/learn react/i);
//   expect(linkElement).toBeInTheDocument();
// });

it('App renders without crashing', async () => {
  const div = document.createElement('div');
  // TODO, use that test render thing instead of ReactDOM?
  ReactDOM.render(
    <MemoryRouter>
      <App />
    </MemoryRouter>, div);
  await new Promise(resolve => setTimeout(resolve, 1000));
});
