import { Reducer } from 'redux';
import { EventActions } from './eventActions';
import { EonetEvent, EonetEventStatus, EonetGeometryType, EonetGeometryPoint } from '../shared/models/EonetEvent';

const initialState: EonetEvent = {
    id: '#',
    title: 'Loading...',
    description: null,
    link: 'https://eonet.sci.gsfc.nasa.gov/',
    closed: null,
    categories: [ { id: 'Category Id', title: 'Loading...'} ],
    sources: [ { id: 'SRC', url: 'https://eonet.sci.gsfc.nasa.gov/'} ],
    geometry: [ { date:'2020-01-01', type: EonetGeometryType.Point, coordinates: [0, 0] } as EonetGeometryPoint ],
    status: EonetEventStatus.Open,
    lastGeometryDate: 'Unknown'
};

const notFoundState: EonetEvent = {
    id: 'ID',
    title: 'Not Found',
    description: null,
    link: 'https://eonet.sci.gsfc.nasa.gov/',
    closed: null,
    categories: [ { id: 'Category Id', title: 'Not Found'} ],
    sources: [ { id: 'SRC', url: 'https://eonet.sci.gsfc.nasa.gov/'} ],
    geometry: [ { date:'2020-01-01', type: EonetGeometryType.Point, coordinates: [0, 0] } as EonetGeometryPoint ],
    status: EonetEventStatus.Open,
    lastGeometryDate: 'Unknown'
};

const reducer: Reducer<EonetEvent, EventActions> = (state: EonetEvent = initialState, action: EventActions) => {
    switch (action.type) {
        case 'RECEIVE_EVENT': {
            return action.event || notFoundState;
        }
        case 'REQUEST_EVENT': {
            return initialState;
        }
    }

    return state;
};

export default reducer;