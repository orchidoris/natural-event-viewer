import * as styles from '../LeftPanel/LeftPanel.module.less';
import * as React from 'react';

export default function LeftPanel(p: React.HTMLProps<HTMLDivElement>) {
    return <div className={styles.container}>{p.children}</div>;
}
