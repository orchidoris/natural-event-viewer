import { EventListScreenStore } from '../EventListScreen/models/EventListScreen';
import { EonetEvent } from '../shared/models/EonetEvent';
import { GlobalState } from '../shared/models/GlobalState';
import { ThunkDispatch } from 'redux-thunk';

// The top-level state object
export interface ApplicationState {
    eventListScreen: EventListScreenStore;
    eventScreen: EonetEvent;
    global: GlobalState;
}

// This type can be used as a hint on action creators so that its 'dispatch' and 'getState' params are
// correctly typed to match your store.
export interface AppThunkAction<TAction> {
    (dispatch: (action: TAction | AppThunkAction<TAction>) => void, getState: () => ApplicationState): void;
}

export type AsyncDispatch = ThunkDispatch<ApplicationState, null, any>;
