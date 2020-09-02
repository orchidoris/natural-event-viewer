import * as React from 'react';
import * as styles from './Button.module.less';
import { ButtonHTMLAttributes } from 'react';

export interface Props {
    isPrimary?: boolean;
    isLink?: boolean;
    disabled?: boolean;
}
//TODO: delete isLink if not need
export default class Button extends React.Component<Props & ButtonHTMLAttributes<HTMLButtonElement>> {
    public render(): JSX.Element {
        const { isLink, isPrimary, disabled, ...props } = this.props;

        return (
            <button
                {...props}
                className={
                    `${styles.container} ${this.props.className} ` +
                    `${isPrimary ? styles.primary : ''} ${isLink ? styles.link : ''} ` +
                    `${disabled ? styles.disabled : ''}`
                }
            >
                {props.children}
            </button>
        );
    }
}
