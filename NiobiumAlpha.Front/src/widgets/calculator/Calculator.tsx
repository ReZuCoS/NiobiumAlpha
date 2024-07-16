import Button from './button/Button';
import styles from './styles.module.css';
import { ErrorObject } from '../../shared/lib/(cs)fetch';
import { apiCalculate, apiGetProviders } from '../../shared/api';
import React, { useState, useRef, ChangeEvent, useEffect } from 'react';

const buttonsArray = [
    "C", "←", "(", ")",
    "7", "8", "9", "/",
    "4", "5", "6", "*",
    "1", "2", "3", "-",
    "0", ".", "=", "+",
]

const Calculator: React.FC = () => {
    const [input, setInput] = useState<string>('');
    const container = useRef<HTMLDivElement>(null);
    const [core, setCore] = useState<string>(null);
    const [loading, setLoading] = useState<boolean>(true);
    const [errors, setErrors] = useState<ErrorObject[]>([]);
    const [providers, setProviders] = useState<string[]>([]);

    const handleChange = ({ target }: ChangeEvent<HTMLInputElement>) => {
        const { value } = target;

        if (/\d\(/.test(value)) {
            return;
        }

        if (/^[\d\/*\-+\(\)\s]*$/.test(value)) {
            setInput(value);
        }
    };

    const handleButtonClick = (value: string) => {
        setInput((prev) => prev + value);

        if (container.current) {
            container.current.focus();
        }
    };

    const handleClear = () => {
        setInput('');
        setErrors([]);
    };

    const handleBackspace = () => {
        setInput((prev) => prev.slice(0, -1));
    };

    const handleCalculate = async () => {
        setLoading(true);

        const res = await apiCalculate(input, core);

        res.errors
            ? setErrors(res.errors)
            : setInput(res.result ?? input);

        setLoading(false)
    };

    const getButtonAction = (char: string) => {
        switch (char) {
            case "C":
                return handleClear();
            
            case "←":
                return handleBackspace();
            
            case "=":
                return handleCalculate();    

            default:
                return handleButtonClick(char);
        }
    }

    useEffect(() => {
        (async () => {
            const result = await apiGetProviders();

            if (result.errors) {
                alert('Providers receiving error!');
            }
                
            setProviders(result.providers);
            setCore(result.providers[0]);

            setLoading(false);
        })();
    }, [])

    useEffect(() => {
        setErrors([]);
    }, [input])

    return (
        <div ref={container} className={styles.Calculator}>
            <div className={styles.TabButtonContainer}>
                {providers.map(p => (
                    <button
                        key={p}
                        className={`
                            ${styles.TabButton}
                            ${core === p ? styles.TabButtonActive : ''}
                        `}
                        onClick={() => setCore(p)}
                    >{p.replace('CalculationService', '')}</button>
                ))}
            </div>

            <input
                type="text"
                value={input}
                disabled={loading}
                onChange={handleChange}
                className={styles.Display}
            />
            
            <div className={styles.ErrorsContainer}>
                {errors.map(e =>
                    <div className={styles.ErrorMessage}>{e.message}</div>
                )}
            </div>

            <div className={styles.ButtonsContainer}>
                {buttonsArray.map((b, i) => (
                    <Button
                        disabled={loading}
                        key={`calc_btn_${i}`}
                        onClick={() => getButtonAction(b)}
                    >{b}</Button>
                ))}
            </div>
        </div>
    );
};

export default Calculator;
