import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import styles from '../styles/Transaccion.module.css'; // Importamos los estilos
import { obtenerSaldoFachada, obtenerMovimientosFachada, registrarTransaccionFachada } from '../clients/api.js';

export function Transaccion() {
    const formatearFecha = (fecha) => fecha.toISOString().split('T')[0];
    const hoy = new Date();
    const hace30Dias = new Date();
    hace30Dias.setDate(hoy.getDate() - 30);

    const [saldo, setSaldo] = useState(0);
    const [numeroCuenta, setNumeroCuenta] = useState('');
    const [movimientos, setMovimientos] = useState([]);

    const [fechaInicio, setFechaInicio] = useState(formatearFecha(hace30Dias));
    const [fechaFin, setFechaFin] = useState(formatearFecha(hoy));

    const [valor, setValor] = useState('');
    const [tipoTransaccion, setTipoTransaccion] = useState(1);
    const [mensaje, setMensaje] = useState('');

    const navigate = useNavigate();

    useEffect(() => {
        cargarDatosBasicos();
        buscarHistorial();
    }, []);

    const cargarDatosBasicos = async () => {
        try {
            if (!localStorage.getItem('token')) {
                navigate('/login');
                return;
            }
            const dataSaldo = await obtenerSaldoFachada();
            setSaldo(dataSaldo.saldo);
            setNumeroCuenta(dataSaldo.numeroCuenta);
        } catch (error) {
            if (error.response?.status === 401) {
                localStorage.removeItem('token');
                navigate('/login');
            }
        }
    };

    const buscarHistorial = async () => {
        try {
            const dataMovimientos = await obtenerMovimientosFachada(fechaInicio, fechaFin);
            setMovimientos(dataMovimientos);
        } catch (error) {
            console.error("Error al cargar movimientos:", error);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setMensaje('');

        if (Number(valor) <= 0) {
            setMensaje('El valor debe ser mayor a 0');
            return;
        }

        try {
            await registrarTransaccionFachada({
                valor: Number(valor),
                tipoTransaccion: Number(tipoTransaccion)
            });

            setMensaje('Transacción exitosa');
            setValor('');
            cargarDatosBasicos();
            buscarHistorial();

        } catch (error) {
            setMensaje(error.response?.data?.error || 'Error al procesar la transacción');
        }
    };

    const cerrarSesion = () => {
        localStorage.removeItem('token');
        navigate('/');
    };

    return (
        <div className={styles.container}>
            <div className={styles.wrapper}>

                {/* Cabecera */}
                <div className={styles.header}>
                    <h2>N&uacute;mero de Cuenta: {numeroCuenta}</h2>
                    <button className={`${styles.btn} ${styles.btnDanger}`} onClick={cerrarSesion}>
                        Cerrar Sesión
                    </button>
                </div>

                {/* Saldo (Tarjeta 1) */}
                <div className={styles.glassCard}>
                    <h3>Saldo Disponible</h3>
                    <p className={styles.saldoTitle}>${saldo.toFixed(2)}</p>
                </div>

                {/* Formulario (Tarjeta 2) */}
                <div className={styles.glassCard}>
                    <h3>Nueva Transacción</h3>
                    <form onSubmit={handleSubmit} className={styles.formRow}>
                        <select
                            className={styles.inputField}
                            value={tipoTransaccion}
                            onChange={(e) => setTipoTransaccion(Number(e.target.value))}
                        >
                            <option value={1}>Depósito (Crédito)</option>
                            <option value={0}>Retiro (Débito)</option>
                        </select>

                        <input
                            className={styles.inputField}
                            type="number"
                            step="0.01"
                            value={valor}
                            onChange={(e) => setValor(e.target.value)}
                            placeholder="Monto"
                            required
                        />

                        <button className={styles.btn} type="submit">Procesar</button>
                    </form>

                    {mensaje && (
                        <p className={mensaje.includes('Error') || mensaje.includes('insuficiente') || mensaje.includes('mayor a 0') ? styles.textError : styles.textSuccess}>
                            {mensaje}
                        </p>
                    )}
                </div>

                {/* Historial (Tarjeta 3) */}
                <div className={styles.glassCard}>
                    <h3>Historial de Movimientos</h3>

                    <div className={styles.filterRow}>
                        <input
                            className={styles.inputField}
                            type="date"
                            value={fechaInicio}
                            onChange={(e) => setFechaInicio(e.target.value)}
                            title="Fecha Inicio"
                        />
                        <input
                            className={styles.inputField}
                            type="date"
                            value={fechaFin}
                            onChange={(e) => setFechaFin(e.target.value)}
                            title="Fecha Fin"
                        />
                        <button className={styles.btn} onClick={buscarHistorial}>Buscar</button>
                    </div>

                    <div className={styles.tableWrapper}>
                        <table className={styles.dataTable}>
                            <thead>
                                <tr>
                                    <th>Fecha</th>
                                    <th>Tipo</th>
                                    <th>Valor</th>
                                    <th>Saldo Resultante</th>
                                </tr>
                            </thead>
                            <tbody>
                                {movimientos.length === 0 ? (
                                    <tr>
                                        <td colSpan="4" style={{ textAlign: 'center' }}>No hay movimientos en este rango de fechas</td>
                                    </tr>
                                ) : (
                                    movimientos.map((mov, index) => (
                                        <tr key={index}>
                                            <td>{new Date(mov.fecha).toLocaleString()}</td>
                                            <td className={mov.tipoTransaccion === 1 ? styles.textCredit : styles.textDebit}>
                                                {mov.tipoTransaccion === 1 ? 'Crédito' : 'Débito'}
                                            </td>
                                            <td>${mov.valor.toFixed(2)}</td>
                                            <td>${mov.saldoDisponible.toFixed(2)}</td>
                                        </tr>
                                    ))
                                )}
                            </tbody>
                        </table>
                    </div>
                </div>

            </div>
        </div>
    )
}