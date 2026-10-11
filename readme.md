# 🚀 Billetera Digital - Arquitectura de Microservicios

Este proyecto es un sistema de Billetera Digital desarrollado bajo una arquitectura de microservicios. Permite la autenticación segura de usuarios y la gestión del historial de transacciones (depósitos y retiros) con validación de saldos en tiempo real.

## Link de Repositorio GitHub

[Billetera Digital](https://github.com/alexander7475/Billetera-Digital.git)

## 🛠️ Tecnologías Utilizadas
* **Backend:** C# .NET Core (APIs RESTful).
* **Base de Datos:** SQL Server.
* **ORM:** Entity Framework Core (EF).
* **Seguridad:** Autenticación por JWT (JSON Web Tokens) con validación asimétrica (RSA).
* **Frontend:** React (Vite), CSS Modules, Axios, React Router.

---

## 📋 Requisitos Previos
Antes de levantar el proyecto, asegúrase de tener instalado localmente:
* [.NET SDK](https://dotnet.microsoft.com/download)
* [Node.js](https://nodejs.org/)
* **SQL Server** en ejecución. (Asegurarse que las credenciales de conexión coincidan con el archivo `appsettings.json` de cada microservicio).

---

## ⚙️ Configuración y Base de Datos
**No es necesario ejecutar scripts manuales en SQL Server.** El proyecto cuenta con **Migraciones Automáticas** implementadas en el inicio de la aplicación. 

Al ejecutar los microservicios por primera vez, Entity Framework Core se encargará automáticamente de:
1. Crear las bases de datos correspondientes de manera aislada (`auth` y `wallet`).
2. Generar toda la estructura de tablas.
3. Poblar las tablas con datos de prueba iniciales (Seed Data).

---

## 🚀 Instrucciones para Levantar el Proyecto

El sistema está dividido en tres partes fundamentales que deben ejecutarse en paralelo. Te recomiendo abrir 3 pestañas o ventanas diferentes en tu terminal.

### 1. Levantar el Microservicio de Autenticación
Este servicio se encargará de validar las credenciales de los usuarios y emitir los tokens JWT.
1. Abre una terminal y navega hasta la carpeta raíz del proyecto de Autenticación.
2. Ejecuta el siguiente comando:
   ```bash
   dotnet run
   ```
   *(El servicio quedará escuchando en el puerto `http://localhost:9090`)*

### 2. Levantar el Microservicio de Transacciones
Este servicio protege sus rutas validando el JWT y procesa los movimientos financieros (depósitos/retiros).
1. Abre una nueva terminal y navega hasta la carpeta raíz del proyecto de Transacciones.
2. Ejecuta el siguiente comando:
   ```bash
   dotnet run
   ```
   *(El servicio quedará escuchando en el puerto `http://localhost:8090`)*

### 3. Levantar el Frontend (React)
Esta es la interfaz gráfica para que el usuario final interactúe con su billetera.
1. Abre una tercera terminal y navega hasta la carpeta raíz del proyecto frontend.
2. Instala las dependencias la primera vez ejecutando:
   ```bash
   npm install
   ```
3. Inicia el servidor de desarrollo ejecutando:
   ```bash
   npm run dev
   ```
   *(La aplicación web estará disponible en `http://localhost:5173`)*

---

## 🧪 Datos de Prueba
Una vez levantados los 3 servicios (los dos backends y el frontend), ingresa a la URL del sitio web (`http://localhost:5173`) y prueba el sistema con las credenciales que se han insertado mediante las migraciones de prueba:

* **Usuario / Identificación:** `Labcom` / 1700000001
* **Contraseña:** `labcom,2015`

* **Usuario / Identificación:** `Labcom1` / 1700000002
* **Contraseña:** `admin`
