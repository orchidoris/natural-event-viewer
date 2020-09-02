import './shared/global.module.css';
import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import { BrowserRouter } from 'react-router-dom';
import configureStore from './store/configureStore';
import { ApplicationState } from './store';
import { routes } from './routes';

// Create browser history to use in the Redux store
const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href')!;

// Get the application-wide store instance, prepopulating with state from the server where available.
const initialState = (window as any).initialState as ApplicationState;
const store = configureStore(initialState);

function renderApp() {
    // This code starts up the React app when it runs in a browser. It sets up the routing configuration
    // and injects the app into a DOM element.

    ReactDOM.render(
        <Provider store={store}>
            <BrowserRouter basename={baseUrl}>{routes}</BrowserRouter>
        </Provider>,
        document.getElementById('react-app')
    );
}

renderApp();

// // Allow Hot Module Replacement
// if (module.hot) {
//     module.hot.accept('./routes', () => {
//         routes = require<typeof RoutesModule>('./routes').routes;
//         renderApp();
//     });
// }
