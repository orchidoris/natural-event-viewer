import { ApiClient } from '../shared/apiClient';
import { EonetEvent } from '../shared/models/EonetEvent';

const apiBase = '/api';

export class EventApi {
    static GetEventById(id: string) {
        return ApiClient.get<EonetEvent>(`${apiBase}/event/GetEvent?id=${id}`);
    }
}

