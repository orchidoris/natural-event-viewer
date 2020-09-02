import * as React from 'react';
import { FC } from 'react';
import * as iconStyles from './icons.module.css';
import svgProps from './svgProps';

const SearchIcon: FC<React.SVGProps<SVGSVGElement>> = (props) => (
    <svg {...svgProps(props, iconStyles.iconSearch)}>
        <title>Search</title>
        <g id="Events" stroke="none" strokeWidth="1" fillRule="evenodd">
            <g id="FS_DESCRIPTION" transform="translate(-19.000000, -24.000000)" fillRule="nonzero">
                <g id="FOLDER_STRUCTURE" transform="translate(-1.000000, 0.000000)">
                    <g id="MENU">
                        <g id="search-alternate" transform="translate(20.000000, 24.000000)">
                            <path
                                d="M15.5893333,14.4126667 L11.2366667,10.06 C13.2081036,7.46847635 12.8344556,3.79261779 10.3818776,1.65079158 C7.92929963,-0.491034632 4.23660561,-0.366295741 1.93415494,1.93615494 C-0.368295741,4.23860561 -0.493034632,7.93129963 1.64879158,10.3838776 C3.79061779,12.8364556 7.46647635,13.2101036 10.058,11.2386667 L14.4106667,15.5913333 C14.7384898,15.9111057 15.2615102,15.9111057 15.5893333,15.5913333 C15.914506,15.2657263 15.914506,14.7382737 15.5893333,14.4126667 Z M1.83333333,6.33333333 C1.83333333,3.84805196 3.84805196,1.83333333 6.33333333,1.83333333 C8.81861471,1.83333333 10.8333333,3.84805196 10.8333333,6.33333333 C10.8333333,8.81861471 8.81861471,10.8333333 6.33333333,10.8333333 C3.84927029,10.8303945 1.83627219,8.81739638 1.83333333,6.33333333 Z"
                                id="Shape"
                            ></path>
                        </g>
                    </g>
                </g>
            </g>
        </g>
    </svg>
);

export default SearchIcon;
