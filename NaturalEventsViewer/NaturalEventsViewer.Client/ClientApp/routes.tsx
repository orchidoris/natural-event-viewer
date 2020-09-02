import * as React from 'react';
import { Route, Switch, Redirect } from 'react-router-dom';
import { Layout } from './Layout/components/Layout/Layout';
import EventListScreen from './EventListScreen/components/EventListScreen/EventListScreen';
import EventListMapScreen from './EventListScreen/components/EventListScreen/EventListMapScreen';
import EventScreen from './EventScreen/components/EventScreen/EventScreen';

export const routes = (
    <Layout>
        <Switch>
            <Redirect exact from="/" to="events" />
            <Route exact path="/events" component={EventListScreen} />
            <Route exact path="/eventsMap" component={EventListMapScreen} />
            <Route exact path="/event/:eventId" component={EventScreen} />
        </Switch>
    </Layout>
);
