import { ReactNode } from 'react';
import styles from './styles.module.css';

interface IParams {
    className?: string;
    disabled?: boolean;
    children?: ReactNode;
    onClick?: () => void;
}

const Button = ({
    className,
    disabled,
    children,
    onClick
}: IParams) => {
    return (
        <button
            onClick={onClick}
            disabled={disabled}
            className={`${className} ${styles.Button}`}
        >
            {children}
        </button>
    );
};

export default Button;
