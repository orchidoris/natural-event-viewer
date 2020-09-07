import { EonetEvent, EonetCategory } from '../../shared/models/EonetEvent';
import { EonetEventStatus } from '../../shared/models/EonetEvent';

export type EonetEventsResponse = {
    title: string;
    description: string;
    link: string;
    events: EonetEvent[];
    titleSearch: string;
    totalCount: number;
}

export type EonetEventsRequest = {
    days?: number;
    status?: EonetEventStatus | null;
    sources?: string[];
    categories?: string[];
    titleSearch?: string;
    ordering?: EonetEventOrderAttribute[];
}

export type EonetFilters = {
    daysPrior: number;
    sources: string[],
    categories: string[],
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
