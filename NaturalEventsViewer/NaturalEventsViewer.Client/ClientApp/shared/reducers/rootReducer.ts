import eventListScreenReducer from '../../EventListScreen/eventListScreenReducer';
import eventScreenReducer from '../../EventScreen/eventScreenReducer';
import globalStateReducer from '../reducers/globalStateReducer';

const emptyReducer = (state: any, action: any) => {
    return state || null;
};

export default {
    eventListScreen: eventListScreenReducer,
    global: globalStateReducer,
    eventScreen: eventScreenReducer
};
