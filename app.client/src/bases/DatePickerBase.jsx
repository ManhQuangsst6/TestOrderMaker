import React from 'react';
import PropTypes from 'prop-types';
import { DatePicker } from 'antd';

const DatePickerBase = ({
    value,
    onChange,
    placeholder = '',
    format = 'DD/MM/YYYY',
    style = {},
    disabled = false,
    allowClear = true,
    ...rest
}) => {
    return (
        <DatePicker
            size='small'
            value={value}
            onChange={onChange}
            placeholder={placeholder}
            format={format}
            style={{ width: '100%', ...style }}
            disabled={disabled}
            allowClear={allowClear}
            {...rest}
        />
    );
};
DatePickerBase.propTypes = {
    value: PropTypes.any,
    onChange: PropTypes.func,
    placeholder: PropTypes.string,
    format: PropTypes.string,
    style: PropTypes.object,
    disabled: PropTypes.bool,
    allowClear: PropTypes.bool,
};

export default DatePickerBase;
