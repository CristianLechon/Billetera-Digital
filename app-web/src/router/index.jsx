import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { LoginView } from '../views/LoginView.jsx';
import { TransaccionView } from '../views/TransaccionView.jsx';
export function AppRoutes() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Navigate to="/login" replace />} />
                <Route path="/login" element={<LoginView />} />
                <Route path="/transaccion" element={<TransaccionView />} />
            </Routes>
        </BrowserRouter>
    );
}