import React, { useState } from 'react';
import * as styles from './Map.module.css';
import { Link } from 'react-router-dom';

export type Props = {
    id: string;
    text: string,
    isClosed: boolean;
    categoryClass: string,
    categoryId: string
}

const Marker = (props: any) => {
    const { text, id, categoryClass, categoryId, isClosed } = props;

    function getCategoryMapPinColor(categoryId: string) {
        switch(categoryId) {
            case 'volcanoes': return '#4cbeff';
            case 'landslides': return '#ff6e2e';
            case 'severeStorms': return '#ff5233';
            case 'wildfires': return '#ffa725';
            case 'seaLakeIce': return '#3c9bd4';
        }

        return '#ff6e2e';
    }

    function getCategoryMapPinPulseClass(categoryId: string) {
        switch(categoryId) {
            case 'volcanoes': return styles.pulseVolcanoes;
            case 'landslides': return styles.pulseLandslides;
            case 'severeStorms': return styles.pulseSevereStorms;
            case 'wildfires': return styles.pulseWildfires;
            case 'seaLakeIce': return styles.pulseSeaLakeIce;
        }

        return '';
    }

    return (
        <div className={isClosed ? styles.dimmed : ''}>
            <Link to={`/event/${id}`} title={text} className={categoryClass + ' ' + styles.categoryLogo + ' ' + styles.bounceLogo}></Link>
            <div className={styles.pin + ' ' + styles.bounce} title={text} style={{ backgroundColor: getCategoryMapPinColor(categoryId)}} />
            { !isClosed ? <div className={styles.pulse + ' ' + getCategoryMapPinPulseClass(categoryId)} /> : ''}
            
        </div>
    );
};

export default Marker;