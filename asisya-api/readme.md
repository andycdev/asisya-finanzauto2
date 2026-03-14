## Escalabilidad Horizontal en Entorno Cloud

La API fue diseñada para permitir **escalado horizontal**, aumentando la capacidad mediante múltiples instancias sin afectar la consistencia ni la disponibilidad.

### Estrategia propuesta:

1. **Contenerización**
   - La aplicación se empaqueta en un **Docker container**, lo que permite desplegar múltiples instancias idénticas.
   - `docker-compose` se puede usar para entornos locales de prueba y despliegues simples.

2. **Orquestación**
   - En la nube, se puede usar **Kubernetes (K8s)** o **AWS ECS / Azure AKS / Google GKE** para gestionar múltiples instancias.
   - El orquestador balancea la carga entre los pods/containers y reinicia instancias en caso de fallo.

3. **Base de datos**
   - Se recomienda una base de datos relacional escalable verticalmente, con **read replicas** para distribuir la carga de lectura.
   - Para operaciones de escritura masiva, se pueden usar **colas (RabbitMQ, AWS SQS, Kafka)** para procesar datos de manera asíncrona.

4. **Caching**
   - Implementar **caché distribuido** (Redis, Memcached) para reducir consultas frecuentes y acelerar la API.

5. **Balanceo de carga**
   - Un **load balancer** (ELB, ALB, NGINX) distribuye las solicitudes HTTP entre las instancias de la API.
   - Permite tolerancia a fallos y escalado automático según la demanda.

6. **Escalado automático**
   - Configurar **autoscaling** en la nube según métricas de CPU, memoria o tráfico.
   - Permite añadir o quitar instancias automáticamente según la carga del sistema.

**Resumen:**  
Con esta estrategia, la API puede manejar grandes volúmenes de tráfico y crecimiento del negocio sin degradar la experiencia del usuario ni comprometer la disponibilidad.