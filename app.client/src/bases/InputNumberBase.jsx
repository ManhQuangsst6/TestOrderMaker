import React from 'react';
import { Input, Button, Space } from 'antd';
import PropTypes from 'prop-types';

const PaddedNumberInput = ({
    value = '',
    onChange,
    length = 4,
    min = 0,
    max = 9999,
}) => {
    const getPaddedValue = (num) => {
        return String(num).padStart(length, '0');
    };

    const handleChange = (e) => {
        const raw = e.target.value.replace(/[^\d]/g, '');
        if (raw === '') {
            onChange('');
        } else {
            const number = Math.min(parseInt(raw, 10), max);
            onChange(getPaddedValue(number));
        }
    };

    const increase = () => {
        const number = parseInt(value, 10) || 0;
        const next = Math.min(number + 1, max);
        onChange(getPaddedValue(next));
    };

    const decrease = () => {
        const number = parseInt(value, 10) || 0;
        const next = Math.max(number - 1, min);
        onChange(getPaddedValue(next));
    };

    return (
        <Space>
            <Button size="small" onClick={decrease}>-</Button>
            <Input
                size="small"
                value={value}
                onChange={handleChange}
                style={{ width: 80, textAlign: 'center' }}
            />
            <Button size="small" onClick={increase}>+</Button>
        </Space>
    );
};

PaddedNumberInput.propTypes = {
    value: PropTypes.string,
    onChange: PropTypes.func.isRequired,
    length: PropTypes.number,
    min: PropTypes.number,
    max: PropTypes.number,
};

export default PaddedNumberInput;
