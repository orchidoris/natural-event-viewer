import { Reducer } from 'redux';
import { GlobalState } from '../models/GlobalState';
import { GlobalStateActions, RECEIVE_SOURCES, RECEIVE_CATEGORIES, ReceiveSources, ReceiveCategories } from '../actions/globalStateActions';

const initialState = { sources: [], categories: [] };

const reducer: Reducer<GlobalState> = (state: GlobalState = initialState, action: /*GlobalStateActions*/ any) => {
    switch (action.type) {
        case RECEIVE_SOURCES: {
            return {
                ...state,
                sources: (action as ReceiveSources).sources,
            };
        }
        case RECEIVE_CATEGORIES: {
            return {
                ...state,
                categories: (action as ReceiveCategories).categories,
            };
        }
    }

    return state;
};

export default reducer;