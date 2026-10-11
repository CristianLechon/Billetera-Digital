import { createRoot } from 'react-dom/client'
import { AppRoutes } from './router'

createRoot(document.getElementById('root')).render(
    <>
        <AppRoutes />
    </>,
)
