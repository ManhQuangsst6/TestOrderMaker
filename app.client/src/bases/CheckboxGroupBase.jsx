import React from 'react';
import PropTypes from 'prop-types';
import { Checkbox } from 'antd';

const CheckboxGroupBase = ({
    options = [],
    value = [],
    onChange,
    style = {},
    disabled = false,
}) => {
    return (
        <Checkbox.Group
            options={options}
            value={value}
            onChange={onChange}
            style={{ ...style }}
            disabled={disabled}
        />
    );
};
CheckboxGroupBase.propTypes = {
    options: PropTypes.array,
    value: PropTypes.array,
    onChange: PropTypes.func,
    style: PropTypes.object,
    disabled: PropTypes.bool,
};

export default CheckboxGroupBase;
