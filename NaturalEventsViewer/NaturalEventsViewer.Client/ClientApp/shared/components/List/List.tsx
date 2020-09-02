import React, { FC } from 'react';
import * as styles from './List.module.less';

type Props = {
    label?: string;
    children: React.ReactChild[];
    className?: string;
};

export const List: FC<Props> = (props) => {
    return (
        <div className={`${styles.container} ${props.className || ''}`}>
            {props.label == null || <span className={styles.label}>{props.label}</span>}
            <ul className={styles.list}>
                {props.children.map((child, index) => {
                    return <ListItem key={index}>{child}</ListItem>;
                })}
            </ul>
        </div>
    );
};

export const ListItem: FC = (props) => {
    return <li className={styles.item}>{props.children}</li>;
};
