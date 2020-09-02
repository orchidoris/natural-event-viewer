import * as iconStyles from './icons.module.css';

export default function (props: any, iconClassName?: string) {
    return {
        ...props,
        className: `${iconClassName ? `${iconClassName} ` : ''}${iconStyles.svgIcon} ${props.className ? props.className : ''}`,
    };
}
