import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import styles from '../styles/Login.module.css';
import { obtenertokenFachada } from '../clients/api.js';

export function Login() {
    const [usuario, setUsuario] = useState('');
    const [password, setPassword] = useState('');
    const [errorMsg, setErrorMsg] = useState('');

    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setErrorMsg('');
        try {
            const data = await obtenertokenFachada({ usuario, password });
            //console.log("Token guardado:", data);

            // Si todo sale bien, lo mandamos a la ruta de transacciones
            navigate('/transaccion');
        } catch (err) {
            console.error(err);
            // Mostrar mensaje de error al usuario
            setErrorMsg('Credenciales incorrectas o error en el servidor');
        }
    }

    return (
        <div className={styles.container}>
            <div className={styles.App}>
                <h1>Billetera Digital</h1>
            </div>

            <form className={styles.formulario} onSubmit={handleSubmit}>
                <img
                    className={styles['app-img']}
                    src="https://img.magnific.com/vector-gratis/billetera-letrero-neon-dinero_1262-16224.jpg"
                    alt="Logo"
                />

                {errorMsg && <p style={{ color: '#ef4444', margin: '0', textAlign: 'center' }}>{errorMsg}</p>}

                <p className={styles.inputGroup} typeof="Usuario">
                    <input
                        className={styles.inputField}
                        value={usuario}
                        onChange={(e) => setUsuario(e.target.value)}
                        type="text"
                        placeholder="Ingresa tu usuario"
                        required
                    />
                </p>

                <p className={styles.inputGroup} typeof="Password">
                    <input
                        className={styles.inputField}
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        type="password"
                        placeholder="***********"
                        required
                    />
                </p>

                <div className={styles['boton-app']}>
                    <button className={styles.btn} type="submit">Ingresar</button>
                </div>
            </form>
        </div>
    )
}
