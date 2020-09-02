import { EonetEvent, EonetCategory } from '../../shared/models/EonetEvent';
import { EonetEventStatus } from '../../shared/models/EonetEvent';

export type EonetEventsResponse = {
    title: string;
    description: string;
    link: string;
    events: EonetEvent[];
    titleSearch: string;
}

export type EonetEventsRequest = {
    limit?: number;
    days?: number;
    sources?: string[];
    status?: EonetEventStatus | null;
    categories?: string[];
    titleSearch?: string;
    ordering?: EonetEventOrderAttribute[];
}

export type EonetFilters = {
    daysPrior: number;
    sources: string[],
    categories: EonetCategory[],
    statuses: EonetEventStatus[],
    order: EonetEventOrderAttributeType[],
    orderAttributesDirection: EonetEventOrderAttribute[],
    titleSearch: string
}

export enum EonetEventOrderAttributeType {
    Id = "Id",
    Title = "Title",
    Category = "Category",
    Source = "Source",
    LastDate = "LastDate",
    Status = "Status",
    ClosedDate = "ClosedDate"
}

export type EonetEventOrderAttribute = {
    attributeType: EonetEventOrderAttributeType;
    isDescending: boolean;
}

export type EventListScreenStore = {
    eventsResponse: EonetEventsResponse,
    filters: EonetFilters
}
