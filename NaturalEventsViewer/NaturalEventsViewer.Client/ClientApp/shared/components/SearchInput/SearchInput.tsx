import * as React from 'react';
import Input from '../Input/Input';
import SearchIcon from '../../../Icons/SearchIcon';
import * as styles from './SearchInput.module.less';
import { useState } from 'react';

type Props = {
    placeholder?: string;
    onChange: (newValue: string) => void;
    defaultValue?: string;
};

export const SearchInput = (props: Props) => {
    const [value, setValue] = useState(props.defaultValue || '');

    return (
        <div className={styles.container}>
            <SearchIcon className={styles.icon} />
            <Input
                placeholder={props.placeholder || 'Search'}
                type="search"
                onChange={(e) => {
                    setValue(e.target.value);
                    props.onChange(e.target.value);
                }}
                value={value}
                className={styles.textInput}
            ></Input>
        </div>
    );
};
