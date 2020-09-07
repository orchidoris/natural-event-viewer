import * as React from 'react';
import { Route, Switch, Redirect } from 'react-router-dom';
import { Layout } from './Layout/components/Layout/Layout';
import EventListScreen from './EventListScreen/components/EventListScreen/EventListScreen';
import EventScreen from './EventScreen/components/EventScreen/EventScreen';

export const routes = (
    <Layout>
        <Switch>
            <Redirect exact from="/" to="events/map" />
            <Route exact path="/events/:mode" component={EventListScreen} />
            <Route exact path="/event/:eventId" component={EventScreen} />
        </Switch>
    </Layout>
);
