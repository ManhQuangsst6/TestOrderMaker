import React from 'react';
import PropTypes from 'prop-types';
import { Select } from 'antd';

const SelectBase = ({ options, value, onChange, placeholder, disabled, style }) => {
    return (
        <Select
            value={value}
            onChange={onChange}
            placeholder={placeholder}
            disabled={disabled}
            style={style}
        >
            {options.map((option) => (
                <Select.Option key={option.value} value={option.value}>
                    {option.label}
                </Select.Option>
            ))}
        </Select>
    );
};
SelectBase.propTypes = {
    options: PropTypes.arrayOf(
        PropTypes.shape({
            value: PropTypes.any.isRequired,
            label: PropTypes.string.isRequired,
        })
    ).isRequired,
    value: PropTypes.any,
    onChange: PropTypes.func.isRequired,
    placeholder: PropTypes.string,
    disabled: PropTypes.bool,
    style: PropTypes.object,
};

export default SelectBase;
