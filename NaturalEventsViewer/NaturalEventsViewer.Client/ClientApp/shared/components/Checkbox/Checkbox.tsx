import * as React from 'react';
import * as styles from './Checkbox.module.less';

interface Props {
    label: string;
}

export default class Checkbox extends React.Component<React.InputHTMLAttributes<HTMLInputElement> & Props> {
    public render(): JSX.Element {
        const { label, className, ...props } = this.props;
        return (
            <label className={styles.container + ' ' + className}>
                <input name="checkbox" {...props} type="checkbox" className={styles.input} />
                <span>{label}</span>
            </label>
        );
    }
}
