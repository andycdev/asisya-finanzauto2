# 🚀 Asisya Products API & SPA - Prueba Técnica 2026

Solución integral para la gestión masiva de productos y categorías, diseñada bajo principios de arquitectura limpia, seguridad con JWT y procesamiento eficiente de datos (100k registros) utilizando **.NET 8** para el Backend y **React** para el Frontend.

---

## 🏗️ Decisiones Arquitectónicas

Se implementó una **Arquitectura en Capas** siguiendo principios **SOLID** para garantizar la escalabilidad y mantenibilidad del sistema:

1.  **Capa de API (Controllers):** Definición de contratos REST y manejo de peticiones HTTP.
2.  **Capa de Negocio (Services):** Lógica principal, incluyendo el motor de generación aleatoria de 100,000 productos.
3.  **Capa de Persistencia (Data):** Uso de **Entity Framework Core** con **PostgreSQL**.
4.  **Seguridad (JWT):** Implementación de autenticación basada en tokens para proteger rutas críticas.
5.  **Patrón DTO:** Mapeo explícito para desacoplar las entidades de la base de datos de las respuestas de la API.

### ⚡ Estrategia de Carga Masiva (100,000 registros)

Para cumplir con el requerimiento de eficiencia en el endpoint `POST /Product`:

- Se optimizó el contexto de datos (EF Core) para inserciones en lote (**Batching**).
- Uso de colecciones eficientes en memoria antes de la persistencia para minimizar el impacto en el servidor.
- Configuración de índices en PostgreSQL para mantener el rendimiento de las consultas paginadas tras la carga masiva.

---

## 🛠️ Stack Tecnológico

- **Backend:** .NET 8 (C#), ASP.NET Core Web API, EF Core.
- **Frontend:** React 18, TypeScript, React Router, Context API, Axios.
- **Base de Datos:** PostgreSQL 15.
- **DevOps:** Docker, Docker Compose, GitHub Actions.

---

## 📦 Guía de Instalación y Despliegue (Docker)

Sigue estos pasos para poner en marcha el ecosistema completo de forma automatizada.

### 1. Clonar el Repositorio

```
git clone https://github.com/andycdev/asisya-finanzauto2.git
```

### 2. Levantar la infraestructura completa

Orquesta la Base de Datos, la API y el Frontend con un solo comando:

```
docker-compose up --build
```

### 3. 🔍 Verificación de Servicios

| Servicio               | URL Local                                                      |
| ---------------------- | -------------------------------------------------------------- |
| **Frontend (React)**   | [http://localhost:3000](http://localhost:3000)                 |
| **Backend (API)**      | [http://localhost:5000](http://localhost:5000)                 |
| **Base de Datos (PG)** | Puerto `5432`                                                  |

# 🔐 Requerimientos de Seguridad e Interfaz

- **Login:** Acceso mediante credenciales para obtener el Token JWT.
- **Persistencia:** El Token se almacena en `localStorage` y se gestiona globalmente mediante el `AuthContext`.
- **Interceptores:** Se utiliza un interceptor de Axios para adjuntar el token automáticamente a cada petición saliente.
- **Protección de Rutas:** Uso de un componente `ProtectedRoute` para evitar el acceso de usuarios no autenticados a la gestión de productos.
- **Formularios:** Implementación de validaciones en el frontend para asegurar la integridad de los datos (precios positivos, campos obligatorios, etc.).

# 🤖 CI/CD (GitHub Actions)

El proyecto incluye un pipeline de Integración Continua en `.github/workflows/ci.yaml` que valida:

- **Build Backend:** Compilación del proyecto .NET 8.
- **Build Frontend:** Validación de TypeScript y construcción del paquete de producción de React.
- **Docker Validation:** Prueba de construcción de imágenes para garantizar la portabilidad.

# ☁️ Escalabilidad Horizontal (Propuesta Cloud)

Para escalar esta solución en un entorno de producción real:

- **Stateless API:** Gracias al uso de JWT, el backend puede escalar horizontalmente en un clúster de Kubernetes detrás de un balanceador de carga.
- **Caché Distribuida:** Implementación de Redis para almacenar resultados de búsqueda frecuentes y reducir la carga sobre la base de datos.
- **Procesamiento Asíncrono:** Para cargas de millones de registros, se propone el uso de un Worker Service y colas de mensajes como RabbitMQ o Azure Service Bus.

---

**Desarrollado por:** Andres Barrera  
**Proyecto:** Prueba Técnica ASISYA 2026  
**Fecha:** Marzo 2026
