
import * as React from 'react';
import { useState, useEffect, FC } from 'react';
import { withRouter, RouteComponentProps } from 'react-router';
import { History } from 'history';
import { connect } from 'react-redux';
import { bindActionCreators, Dispatch } from 'redux';
import { ApplicationState } from '../../../store';
import { SourcesStaticData } from '../../../shared/staticData/sources';

import { fetchEventEffect } from '../../eventActions';
import * as styles from './EventScreen.module.less';
import { EonetEvent, EonetCategory, EonetEventStatus, EonetGeometryType, EonetGeometryPoint } from '../../../shared/models/EonetEvent';
import Map from '../../../shared/components/Map/Map';

const mapStateToProps = (state: ApplicationState) => ({
    event: state.eventScreen
});

const mapDispatchToProps = (dispatch: Dispatch) =>
    bindActionCreators(
        {
            fetchEventEffect
        },
        dispatch
    );

export type Props = ReturnType<typeof mapDispatchToProps> &
    ReturnType<typeof mapStateToProps> &
    RouteComponentProps<{
        eventId: string;
    }>;

export const EventScreen: FC<Props> = (p) => {
    useEffect(() => {
        p.fetchEventEffect(p.match.params.eventId);
    }, []);

    const sourceNameDictionary = Object.assign({}, ...SourcesStaticData.sources.map((s: any) => ({[s.id]: s.title})));

    function getCategoryIconImgClass(categoryId: string) {
        switch(categoryId) {
            case 'volcanoes': return styles.volcanoesCategoryImg;
            case 'landslides': return styles.landslidesCategoryImg;
            case 'severeStorms': return styles.severeStormsCategoryImg;
            case 'wildfires': return styles.wildfiresCategoryImg;
            case 'seaLakeIce': return styles.waterIceCategoryImg;
        }

        return styles.defaultCategoryImg;
    }

    function getCategoryMapPinColor(categoryId: string) {
        switch(categoryId) {
            case 'volcanoes': return '#4cbeff';
            case 'landslides': return '#ff6e2e';
            case 'severeStorms': return '#ff5233';
            case 'wildfires': return '#ffa725';
            case 'seaLakeIce': return '#3c9bd4';
        }

        return styles.defaultCategoryImg;
    }

    var geometry = p.event?.geometry[0];
    var geometryPoint = geometry.type == EonetGeometryType.Point ? (p.event.geometry[0] as EonetGeometryPoint) : null;
    var lat = geometryPoint?.coordinates[1];
    var lng = geometryPoint?.coordinates[0];

    return (
        <div className={styles.container}>
            <div className={styles.infoContainer}>
                <div className={styles.categoryBlock}>
                    <div className={getCategoryIconImgClass(p.event.categories[0].id) + (p.event.closed ? ' ' + styles.categoryClosed : '')}/>
                    <div className={styles.categoryName}>{p.event.categories[0].title}</div>
                </div>
                <div className={styles.eventDetails}>
                    <div className={styles.id}>{p.event.id}</div>
                    <div className={styles.name}>{p.event.title}</div>
                    {p.event.description ? <div className={styles.listItemDetailsDescription}>{p.event.description}</div> : ''}
                    {(p.event.geometry[0] ?? {}).type == EonetGeometryType.Point ?
                        <div className={styles.locationBlock}>
                            <div className={styles.locationImg} />
                            <div className={styles.locationNum}>{(p.event.geometry[0] as EonetGeometryPoint).coordinates.join(', ')}</div>
                        </div>: ''}
                    <div className={styles.sourceBlock}>
                        <span className={styles.sourceText}>Sources:</span>
                        {p.event.sources.map((s) => 
                            <span key={s.id} className={styles.sourceItem} title={sourceNameDictionary[s.id]}>{s.id}<a className={styles.goOutLink} href={s.url}></a></span>)}
                    </div>
                    <div className={styles.sourceBlock}>
                        <span className={styles.sourceText}>Last date:</span>
                        <span className={styles.sourceItem}>{new Date(Date.parse(p.event.lastGeometryDate || '')).toLocaleDateString()}</span>
                    </div>
                    <div className={styles.statusBlock}>
                        <span className={styles.sourceText}>Status:</span>
                        <span className={p.event.status == EonetEventStatus.Open ? styles.statusOpen : styles.statusClosed}>
                            {p.event.status == EonetEventStatus.Open ? 'Open' : `Closed at ${new Date(Date.parse(p.event.closed || '')).toLocaleDateString()}`}
                        </span>
                    </div>
                </div>
            </div>
            {(lat && lng && p.event.title != 'Loading...' && p.event.title != 'Not Found' ? (
                <div className={styles.mapContainer}>
                    <Map zoom={5} points={[{
                        id: p.event.id,
                        isClosed: p.event.status == EonetEventStatus.Closed,
                        lat: lat,
                        lng: lng,
                        categoryClass: getCategoryIconImgClass(p.event.categories[0].id),
                        text: p.event.title,
                        categoryId: p.event.categories[0].id
                    }]} />
                </div> ) : '')}
        </div>
    );
}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(EventScreen));
