import { useState } from 'react';

function useInput(initialValue = '') {
    const [value, setValue] = useState(initialValue);

    const onChange = (e) => {
        setValue(e.target.value);
    };

    return {
        value,
        onChange,
        setValue, // optional: in case you want to manually update it
    };
}

export default useInput;