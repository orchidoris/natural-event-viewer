import * as React from 'react';
import { FC } from 'react';
import * as iconStyles from './icons.module.css';
import svgProps from './svgProps';

const ListIcon: FC<React.SVGProps<SVGSVGElement>> = (props) => (
    <svg {...svgProps(props, iconStyles.mainNavigationIcon)} viewBox={'0 0 16 16'}>
        <g fillRule="evenodd" stroke="none" strokeWidth="1">
            <path
                d="M15 9.333h-2.333A.667.667 0 0012 10a.667.667 0 01-.667.667H4.667A.667.667 0 014 10a.667.667 0 00-.667-.667H1a1 1 0 00-1 1v4.334C0 15.403.597 16 1.333 16h13.334c.736 0 1.333-.597 1.333-1.333v-4.334a1 1 0 00-1-1zM6.333 8a.667.667 0 000 1.333h3.334a.667.667 0 000-1.333H6.333zM1.667 7.333a.667.667 0 00.666-.666h11.334a.667.667 0 101.333 0c0-.737-.597-1.334-1.333-1.334H2.333C1.597 5.333 1 5.93 1 6.667c0 .368.298.666.667.666zM1.667 4.667A.667.667 0 002.333 4h11.334A.667.667 0 1015 4c0-.736-.597-1.333-1.333-1.333H2.333C1.597 2.667 1 3.264 1 4c0 .368.298.667.667.667zM1.667 2a.667.667 0 00.666-.667h11.334a.667.667 0 101.333 0C15 .597 14.403 0 13.667 0H2.333C1.597 0 1 .597 1 1.333c0 .369.298.667.667.667z"
                transform="translate(-20 -78) translate(8 78) translate(12)"
            ></path>
        </g>
    </svg>
);

export default ListIcon;
