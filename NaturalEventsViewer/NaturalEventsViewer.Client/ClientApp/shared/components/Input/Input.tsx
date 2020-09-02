import * as React from 'react';
import * as styles from './Input.module.less';

interface Props {
    isValid?: boolean;
    isSmall?: boolean;
}

export default class Input extends React.Component<React.InputHTMLAttributes<HTMLInputElement> & Props> {
    public render(): JSX.Element {
        const { isValid = true, isSmall, ...props } = this.props;

        let classList: string[] = [styles.container];
        if (props.className) {
            classList.push(props.className);
        }
        if (!isValid) {
            classList.push(styles.invalid);
        }
        if (isSmall) {
            classList.push(styles.small);
        }

        return <input {...props} className={classList.join(' ')}></input>;
    }
}
