import * as React from 'react';
import * as styles from './Layout.module.css';
import { Menu } from '../menu/Menu';

export class Layout extends React.Component<{}, {}> {
    public render() {
        return (
            <div key="app" className={styles.container}>
                <Menu />
                <div key="content" className={styles.content}>
                    {this.props.children}
                </div>
            </div>
        );
    }
}
