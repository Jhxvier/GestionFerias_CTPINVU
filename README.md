# Gestión Ferias CTP INVU

Este proyecto es un sistema web desarrollado para la gestión integral de ferias en el Colegio Técnico Profesional INVU (CTP INVU). Permite la administración de eventos, estudiantes, centros educativos, categorías de proyectos, jueces y resultados, facilitando el proceso de registro, evaluación y seguimiento de las actividades.

~/.dotnet/dotnet run

## Tecnologías Utilizadas

El sistema está construido utilizando un stack moderno de tecnologías .NET y web:

- **Backend:** ASP.NET Core 8.0 MVC (Model-View-Controller)
- **Base de Datos:** MySQL
- **ORM:** Entity Framework Core 8 (Pomelo.EntityFrameworkCore.MySql)
- **Gestión de Estado:** ASP.NET Core Session Management

## Arquitectura del Proyecto

La aplicación sigue el patrón arquitectónico **MVC (Modelo-Vista-Controlador)**, estructurando el código de la siguiente manera:

- **Models:** Contiene las entidades de dominio que representan las tablas en la base de datos (ej., `Estudiante`, `Evento`, `Juece`).
- **Views:** Vistas Razor (`.cshtml`) encargadas de la presentación de la interfaz de usuario.
- **Controllers:** Manejan las peticiones HTTP, coordinan la lógica de negocio junto con el `DbContext`, y retornan las vistas correspondientes.
- **Data:** Contiene la configuración de Entity Framework (`AppDbContext.cs`), definiendo los `DbSet` para cada entidad y administrando la conexión. Configurado en `Program.cs`.

## Estructura de la Base de Datos

La base de datos `BD_FeriasCTPINVU` gestiona las siguientes entidades principales representadas en el `AppDbContext`:

1.  **Personas e Identidades:**
    *   `Usuarios` y `Roles`, junto con su tabla intermedia `UsuarioRole`.
    *   `Personas`: Información general de individuos.
    *   `Estudiantes`: Entidad específica para los alumnos participantes.
    *   `Jueces`: Entidad para los evaluadores.
    *   `Tutores`: Profesores o guías de los proyectos.

2.  **Organización y Eventos:**
    *   `CentrosEducativos` y `CentroTelefonos`: Detalles de las instituciones participantes.
    *   `Eventos`: Ferias o actividades principales.
    *   `Categorias` y `Subcategorias`: Clasificación de los proyectos presentados.

3.  **Proceso de Participación y Resultados:**
    *   `Inscripciones` y `InscripcionIntegrantes`: Registro de proyectos y sus respectivos creadores.
    *   `ResultadosEventos` y `ResultadosGanadores`: Almacenamiento de evaluaciones y triunfadores de cada evento.

## Autenticación y Sesiones

El sistema incluye una capa de manejo de sesiones para controlar el ciclo de vida del usuario (configurado con un `IdleTimeout` de 30 minutos) que gestiona el estado de autenticación de diferentes tipos de usuarios (ej. administradores, jueces) en sus apartados de trabajo.

## Instalación y Ejecución

Al ejecutar el proyecto por primera vez, **el sistema aplica de manera automática las migraciones pendientes** y crea la base de datos si esta no existe. 

Para ejecutar el proyecto:

1. Asegúrate de tener un servidor MySQL en ejecución.
2. Verifica o actualiza la cadena de conexión `DefaultConnection` en el archivo `appsettings.json` o `appsettings.Development.json`.
3. Compila y ejecuta el proyecto (ya sea desde Visual Studio, Rider, o usando el CLI con `dotnet run`). La base de datos `BD_FeriasCTPINVU` se generará y configurará automáticamente.
