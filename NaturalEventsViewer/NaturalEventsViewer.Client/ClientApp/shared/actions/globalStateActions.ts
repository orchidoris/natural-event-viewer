import { AppThunkAction, ApplicationState } from '../../store';
import { ThunkAction } from 'redux-thunk';
import { EonetCategory } from '../models/EonetEvent';

export const RECEIVE_SOURCES = 'RECEIVE_SOURCES';
export const RECEIVE_CATEGORIES = 'RECEIVE_CATEGORIES';

export interface ReceiveSources {
    type: typeof RECEIVE_SOURCES;
    sources: string[];
}

export interface ReceiveCategories {
    type: typeof RECEIVE_CATEGORIES;
    categories: EonetCategory[];
}

export type GlobalStateActions = ReceiveSources | ReceiveCategories;

export const actions = {
    fetchSources: (): AppThunkAction<ReceiveSources> => async (dispatch, getState) => {
        const sources : string[] = ((window as any).initialState as ApplicationState).global.sources; //const events = await SourceApi.GetSources();
        dispatch({ type: RECEIVE_SOURCES, sources });
    },
    fetchCategories: (): AppThunkAction<ReceiveCategories> => async (dispatch, getState) => {
        const categories : EonetCategory[] = ((window as any).initialState as ApplicationState).global.categories; // await CategoryApi.GetCategories();
        dispatch({ type: RECEIVE_CATEGORIES, categories });
    },
    fetchGlobalState: (): ThunkAction<Promise<void>, ApplicationState, {}, GlobalStateActions> => async (dispatch, getState) => {
        await dispatch(actions.fetchSources());
        await dispatch(actions.fetchCategories());
    },
};
