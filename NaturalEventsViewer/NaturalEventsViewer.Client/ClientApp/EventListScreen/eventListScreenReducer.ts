import { Reducer } from 'redux';
import { EventListActions } from './eventListActions';
import { EventListScreenStore, EonetEventOrderAttributeType } from './models/EventListScreen';
import { EonetEventStatus } from '../shared/models/EonetEvent';
import { GlobalState } from '../shared/models/GlobalState';

const globalState = ((window as any).initialState.global as GlobalState);

const initialState: EventListScreenStore = {
    eventsResponse: {
        title: 'Initial',
        description: 'Natural events from EONET.',
        link: 'https://eonet.sci.gsfc.nasa.gov/api/v3/events',
        titleSearch: '',
        events: []
    },
    filters: {
        sources: globalState.sources,
        categories: globalState.categories,
        statuses: Object.values(EonetEventStatus),
        daysPrior: 180,
        order: [ EonetEventOrderAttributeType.LastDate ],
        orderAttributesDirection: Object.values(EonetEventOrderAttributeType).map(t => ({
            attributeType: t,
            isDescending: t == EonetEventOrderAttributeType.LastDate ? true : false })),
        titleSearch: ''
    }
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
            return {
                ...state,
                filters: action.filters
            };
        }
    }

    return state;
};

export default reducer;
