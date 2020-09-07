import { EonetCategory } from './EonetEvent';

export type GlobalState = {
    categories: EonetCategory[];
    sources: string[];
    maxDaysPrior: number;
}