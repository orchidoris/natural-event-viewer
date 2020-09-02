import * as React from 'react';
import { createBrowserHistory } from 'history';
import { Link } from 'react-router-dom';

export interface MenuItemProps {
    className?: string;
    title: string;
    containerElement?: React.ReactElement<any>;
}

export class MenuItem extends React.Component<MenuItemProps> {
    public render() {
        var { className, title, containerElement } = this.props;

        var buttonProps = {
            className: className,
            title: title,
        };

        return containerElement
            ? React.cloneElement(containerElement, buttonProps)
            : React.createElement('a', buttonProps);
    }
}