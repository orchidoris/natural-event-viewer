import { EonetEvent } from '../shared/models/EonetEvent';
import { Dispatch } from 'redux';
import { EventApi } from './eventApi';

export type EventActions = ReturnType<
    typeof requestEvent | typeof receiveEvent
>;

export function requestEvent(id: string) {
    return <const>{
        type: 'REQUEST_EVENT',
        id
    };
}

export function receiveEvent(event: EonetEvent) {
    return <const>{
        type: 'RECEIVE_EVENT',
        event
    };
}

export const fetchEventEffect = (id: string) => async (dispatch: Dispatch) => {
    dispatch(requestEvent(id));
    const response : EonetEvent = await EventApi.GetEventById(id);
    dispatch(receiveEvent(response));
};