import axios from 'axios';

const URL = 'http://localhost:9090/auth/token';
const TRANSACCIONES_URL = 'http://localhost:8090/transacciones';

const obtenertoken = async ({ usuario, password }) => {
    const response = await axios.post(URL, {
        user: usuario,
        password: password
    });
    const data = response.data;
    const token = data.accessToken;
    localStorage.setItem("token", token);
    localStorage.setItem("expira", data.expiresAt);
    return token;
}

export const obtenertokenFachada = async ({ usuario, password }) => {
    return await obtenertoken({ usuario, password });
}

const getToken = () => {
    return localStorage.getItem("token");
};

// Transacciones

const obtenerSaldo = async () => {
    const TOKEN = getToken();
    const data = await axios.get(`${TRANSACCIONES_URL}/saldo`, { headers: { Authorization: `Bearer ${TOKEN}` } }).then(r => r.data);
    return data;
}

export const obtenerSaldoFachada = async () => {
    return await obtenerSaldo();
}

const obtenerMovimientos = async (fechaInicio, fechaFin) => {
    const TOKEN = getToken();
    let url = `${TRANSACCIONES_URL}/movimientos`;
    if (fechaInicio && fechaFin) {
        url += `?fechaInicio=${fechaInicio}&fechaFin=${fechaFin}`;
    }

    const response = await axios.get(url, { headers: { Authorization: `Bearer ${TOKEN}` } });
    return response.data;
}

export const obtenerMovimientosFachada = async (fechaInicio, fechaFin) => {
    return await obtenerMovimientos(fechaInicio, fechaFin);
}

const registrarTransaccion = async (body) => {
    const TOKEN = getToken();
    const data = await axios.post(`${TRANSACCIONES_URL}/registrar`, body, { headers: { Authorization: `Bearer ${TOKEN}` } });
    return data.data;
}

export const registrarTransaccionFachada = async (body) => {
    return await registrarTransaccion(body);
}
