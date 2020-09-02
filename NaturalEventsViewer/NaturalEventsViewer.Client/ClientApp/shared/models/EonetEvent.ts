export type EonetEvent = {
    id: string;
    title: string;
    description: string | null;
    link: string;
    closed: string | null; // TODO: Use Date type instead
    lastGeometryDate: string; // TODO: Use Date type instead
    status: EonetEventStatus;
    categories: EonetCategory[];
    sources: EonetSource[];
    geometry: EonetGeometry[];
}

export enum EonetEventStatus {
    Open = 'Open',
    Closed = 'Closed'
}

export type EonetCategory = {
    id: string;
    title: string;
}

export type EonetSource = {
    id: string;
    url: string;
}

export type EonetGeometry = {
    date: string;
    type: EonetGeometryType;
}

export enum EonetGeometryType {
    Point = 'Point',
    Polygon = 'Polygon'
}

export type EonetGeometryPoint = EonetGeometry & {
    coordinates: number[];
}

export type EonetGeometryPolygon = {
    coordinates: number[][][];
}