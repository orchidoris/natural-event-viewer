import { ApiClient } from '../shared/apiClient';
import { EonetEventsRequest, EonetEventsResponse } from './models/EventListScreen';

const apiBase = '/api';

export class EventListApi {
    static GetEvents(request?: EonetEventsRequest) {
        if (!request) request = { };
        return ApiClient.post<EonetEventsRequest, EonetEventsResponse>(`${apiBase}/event/GetEventList`, request);
    }
}
