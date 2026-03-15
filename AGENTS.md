1. Problema y Objetivos
Problema: La falta de un sistema centralizado genera desorganización en las citas y pérdida de trazabilidad sobre la asistencia de los clientes
. Objetivo: Proveer una plataforma donde los clientes reserven sin fricciones y los barberos gestionen su agenda y cumplimiento mediante una interfaz segura
.
2. Requerimientos Funcionales
Módulo de Cliente (Público):
Visualización de horarios disponibles.
Formulario de reserva: Nombre, teléfono, correo, selección de barbero, fecha y hora.
No se requiere login para esta acción.
Módulo de Barbero (Privado):
Login seguro: Autenticación mediante JWT (JSON Web Tokens).
Dashboard de citas: Lista de citas asignadas.
Control de asistencia: Botón para marcar "Asistió/Servicio Prestado" o "No Asistió".
Módulo de Administración/Datos:
Persistencia en base de datos local SQLite.
3. Stack Tecnológico
Frontend: Angular.js (Framework SPA).
Backend: C# .NET Framework (Web API).
Base de Datos: SQLite (Single binary, zero dependencies)
.
Seguridad: JWT para la gestión de sesiones de barberos.

--------------------------------------------------------------------------------
Estándares de Codificación (AGENTS.md)
Para asegurar la calidad del proyecto, se definen las siguientes reglas que deben ser validadas por herramientas de revisión como Gentleman Guardian Angel (GGA)
.
Frontend: Angular.js (Best Practices)
REQUIRE el uso de controladores delgados; la lógica de negocio debe residir en Servicios.
REQUIRE nombres descriptivos y semánticos para todas las funciones y variables
.
REJECT if se manipula el DOM directamente desde el controlador; usa directivas.
PREFER la desestructuración de objetos para mejorar la legibilidad.
Backend: C# .NET & SOLID (Best Practices)
REQUIRE el cumplimiento de los Principios SOLID
:
S: Cada clase debe tener una única responsabilidad (ej. AppointmentService solo gestiona citas).
O: Las clases deben estar abiertas a la extensión pero cerradas a la modificación.
L: Las subclases deben poder sustituir a las clases base.
I: Segregación de interfaces para no obligar a implementar métodos no usados.
D: Inyección de Dependencias para desacoplar el acceso a SQLite de la lógica de negocio.
REJECT if existen claves secretas o strings de conexión hardcodeadas; usa archivos de configuración.
REQUIRE el manejo global de excepciones para devolver códigos de estado HTTP correctos.
REJECT if se utiliza any o tipos dinámicos donde se pueda definir un modelo/interfaz claro
.

--------------------------------------------------------------------------------
4. Criterios de Aceptación
Un usuario anónimo puede completar una reserva en menos de 3 pasos.
El barbero no puede acceder a su agenda sin un token JWT válido.
Cada cambio de estado de la cita (Asistió/No asistió) se persiste inmediatamente en SQLite.
El backend debe pasar las validaciones de GGA sin advertencias de violación de principios SOLID
.
5. Riesgos y Dependencias
Seguridad: El manejo de JWT debe incluir expiración para evitar sesiones eternas.
Concurrencia: SQLite debe manejarse con cuidado para evitar bloqueos durante escrituras simultáneas.
Siguiente paso recomendado: Ejecuta el comando /sdd-ff barber-booking en tu orquestador de Agent Teams Lite para generar automáticamente la propuesta técnica, el diseño de la base de datos y la lista de tareas detallada basada en este PRD
. A medida que desarrolles, recuerda que la IA no reemplaza tus fundamentos; actúa como supervisor técnico para validar que el código de .NET realmente cumpla con los principios SOLID