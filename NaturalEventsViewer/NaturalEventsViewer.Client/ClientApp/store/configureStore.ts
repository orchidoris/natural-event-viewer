import { createStore, applyMiddleware, combineReducers, ReducersMapObject, AnyAction } from 'redux';

import { composeWithDevTools } from 'redux-devtools-extension';

import thunk from 'redux-thunk';
import Reducers from '../shared/reducers/rootReducer';
import { ApplicationState } from './index';

export default function configureStore(initialState?: ApplicationState) {
    const allReducers = buildRootReducer(Reducers);

    const store = createStore(allReducers, initialState, composeWithDevTools(applyMiddleware(thunk)));

    return store;
}

export type AppStore = ReturnType<typeof configureStore>;

function buildRootReducer(allReducers: any) {
    return combineReducers<ApplicationState>(allReducers);
}
