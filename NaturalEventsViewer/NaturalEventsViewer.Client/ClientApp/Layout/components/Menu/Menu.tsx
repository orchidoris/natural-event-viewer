import * as React from 'react';
import { NavLink } from 'react-router-dom';
import { MenuItem } from './MenuItem';
import * as styles from './Menu.module.less';
import ListIcon from '../../../Icons/ListIcon';
import MapIcon from '../../../Icons/MapIcon';

export class Menu extends React.Component<{}> {
    public render() {
        return (
            <div key="menu" className={styles.container}>
                <img src="https://eonet.sci.gsfc.nasa.gov/img/nasa-logo.svg" width="32" height="32" alt="Natural Events Viewer" className={styles.logo} />
                <MenuItem
                    className={styles.icon}
                    title="Detailed Events List"
                    containerElement={<NavLink to="/events/list" activeClassName={styles.isActive} children={<ListIcon />} />}
                />
                <MenuItem
                    className={styles.icon}
                    title="Events on Map"
                    containerElement={<NavLink to="/events/map" activeClassName={styles.isActive} children={<MapIcon />} />}
                />
            </div>
        );
    }
}
