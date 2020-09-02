import { EonetEventsRequest, EonetEventsResponse, EonetFilters } from './models/EventListScreen';
import { Dispatch } from 'redux';
import { EventListApi } from './eventListApi';

export type EventListActions = ReturnType<
    typeof requestEventList | typeof receiveEventList
>;

export function requestEventList(filters: EonetFilters) {
    return <const>{
        type: 'REQUEST_EVENT_LIST',
        filters: filters
    };
}

export function receiveEventList(response: EonetEventsResponse) {
    return <const>{
        type: 'RECEIVE_EVENT_LIST',
        response
    };
}

export const fetchEventListEffect = (request: EonetEventsRequest, filters: EonetFilters) => async (dispatch: Dispatch) => {
    dispatch(requestEventList(filters));
    const response : EonetEventsResponse = await EventListApi.GetEvents(request);
    dispatch(receiveEventList(response));
};
