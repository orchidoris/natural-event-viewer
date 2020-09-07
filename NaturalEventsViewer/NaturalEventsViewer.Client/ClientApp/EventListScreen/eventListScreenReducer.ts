import { Reducer } from 'redux';
import { EventListActions, cleanFilters } from './eventListActions';
import { EventListScreenStore } from './models/EventListScreen';

const localStorageFiltersKey = 'event-list-filters';
const savedFiltersString = window.localStorage.getItem(localStorageFiltersKey);

const initialState: EventListScreenStore = {
    eventsResponse: {
        title: 'Initial',
        description: 'Natural events from EONET.',
        link: 'https://eonet.sci.gsfc.nasa.gov/api/v3/events',
        titleSearch: '',
        events: [],
        totalCount: 0
    },
    filters: savedFiltersString ? JSON.parse(savedFiltersString) : cleanFilters()
};

const reducer: Reducer<EventListScreenStore, EventListActions> = (state: EventListScreenStore = initialState, action: EventListActions) => {
    switch (action.type) {
        case 'RECEIVE_EVENT_LIST': {
            return {
                ...state,
                eventsResponse: action.response
            };
        }
        case 'REQUEST_EVENT_LIST': {
            window.localStorage.setItem(localStorageFiltersKey, JSON.stringify(action.filters));

            return {
                ...state,
                filters: action.filters
            };
        }
        case 'UPDATE_FILTERS': {
            return {
                ...state,
                filters: action.filters
            };
        }
    }

    return state;
};

export default reducer;
