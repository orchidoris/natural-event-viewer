import { EonetEventsRequest, EonetEventsResponse, EonetFilters, EonetEventOrderAttributeType } from './models/EventListScreen';
import { Dispatch } from 'redux';
import { EventListApi } from './eventListApi';
import { EonetEventStatus } from '../shared/models/EonetEvent';
import { GlobalState } from '../shared/models/GlobalState';

const globalState = ((window as any).initialState.global as GlobalState);
export const cleanFilters = (): EonetFilters => ({
    sources: globalState.sources,
    categories: globalState.categories.map(c => c.id),
    statuses: Object.values(EonetEventStatus),
    daysPrior: globalState.maxDaysPrior,
    order: [ EonetEventOrderAttributeType.LastDate ],
    orderAttributesDirection: Object.values(EonetEventOrderAttributeType).map(t => ({
        attributeType: t,
        isDescending: t == EonetEventOrderAttributeType.LastDate ? true : false })),
    titleSearch: ''
});

export type EventListActions = ReturnType<
    typeof requestEventList | typeof receiveEventList | typeof updateFilters
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

export function updateFilters(filters: EonetFilters) {
    return <const>{
        type: 'UPDATE_FILTERS',
        filters: filters
    };
}

export const updateFiltersEffect = (filters: EonetFilters) => async (dispatch: Dispatch) => {
    dispatch(updateFilters(filters));
}

export const fetchEventListEffect = (filters: EonetFilters | null = null) => async (dispatch: Dispatch) => {
    const newFilters: EonetFilters = filters || cleanFilters();

    const request: EonetEventsRequest =  {
        days: newFilters.daysPrior,
        sources: newFilters.sources,
        status: newFilters.statuses.length == 1 ? newFilters.statuses[0] : null,
        categories: newFilters.categories,
        titleSearch: newFilters.titleSearch,
        ordering: newFilters.order.map(attrType => ({
            attributeType: attrType,
            isDescending: newFilters.orderAttributesDirection.some(oa => oa.attributeType === attrType && oa.isDescending)
        }))
    };

    dispatch(requestEventList(newFilters));
    const response : EonetEventsResponse = await EventListApi.GetEvents(request);
    dispatch(receiveEventList(response));
};
