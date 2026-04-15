La aplicación web está estructurada en **9 módulos principales**, y el acceso a cada una de estas áreas está estrictamente controlado por **cinco roles definidos: Administrador, Coordinador, Tutor, Juez y Estudiante**. 

A continuación, se presenta un desglose de las partes más importantes del sitio web y los accesos correspondientes para cada rol:

**1. Módulos de Autenticación, Usuarios y Perfil**
*   **Descripción:** Estas áreas controlan el ingreso seguro al sistema mediante usuario y contraseña, la creación de nuevas cuentas y la actualización de los datos personales.
*   **Accesos:**
    *   **Administrador:** Es el único rol con autorización para crear nuevos usuarios y el único que puede modificar los correos electrónicos registrados.
    *   **Coordinador:** Tiene permiso para editar la información de los usuarios ya existentes, pero no puede crear cuentas nuevas.
    *   **Todos los roles:** Tienen acceso al módulo de "Perfil" para consultar y modificar sus datos básicos personales y cambiar su propia contraseña. Los roles que no son administradores ni coordinadores no pueden acceder al módulo de "Usuarios".

**2. Módulos de Actores (Estudiantes, Tutores y Jueces)**
*   **Descripción:** Son tres módulos independientes que sirven para registrar, consultar, actualizar y eliminar de la base de datos a todos los participantes de las ferias, ya sean los alumnos, los profesores guías (tutores) o los evaluadores (jueces).
*   **Accesos:** 
    *   El uso de estos tres módulos es **exclusivo para el Administrador y el Coordinador**.

**3. Módulo de Eventos**
*   **Descripción:** Centraliza la planificación de todas las ferias académicas (Feria Científica, Expo-Técnica, etc.). En esta sección se registra la información fundamental como el nombre del evento, la descripción y las fechas de inicio y fin.
*   **Accesos:** 
    *   Únicamente el **Administrador y el Coordinador** pueden visualizar este módulo para crear, consultar, modificar o eliminar eventos.

**4. Módulo de Inscripciones**
*   **Descripción:** Es el área donde se formaliza la participación estudiantil en los eventos que están en proceso, seleccionando el tipo de participación y vinculando al alumno con su proyecto.
*   **Accesos:**
    *   **Estudiantes:** Pueden ingresar para registrar directamente su propia inscripción a las ferias.
    *   **Administrador y Coordinador:** Tienen permisos para crear inscripciones, realizar consultas y son los encargados de **asignar el tutor** al proyecto, acción que da por aprobada la inscripción.
    *   **Juez:** Su acceso está limitado únicamente a visualizar las inscripciones realizadas, sin poder modificarlas.

**5. Módulo de Resultados**
*   **Descripción:** Es la sección destinada a documentar oficialmente los logros de los alumnos. Aquí se ingresan y almacenan las calificaciones, las observaciones sobre los proyectos y las posiciones de los ganadores.
*   **Accesos:**
    *   **Juez:** Es el usuario central de este módulo, ya que tiene los permisos operativos para registrar las calificaciones finales y observaciones de las presentaciones.
    *   **Administrador y Coordinador:** También están autorizados para ingresar y gestionar las calificaciones y definir a los ganadores.
    *   **Todos los roles (incluyendo Estudiantes y Tutores):** Una vez que los resultados son publicados, cualquier rol puede ingresar al módulo únicamente en modo de consulta para visualizar las calificaciones y posiciones.

**6. Módulo de Reportes**
*   **Descripción:** Genera documentos e informes consolidados sobre las ferias. Permite exportar datos históricos, el historial de un estudiante en particular, la lista completa de participaciones por evento y el resumen de los resultados.
*   **Accesos:** 
    *   Esta herramienta de análisis está **disponible exclusivamente para el Administrador y el Coordinador**; los roles sin permiso no pueden acceder a esta funcionalidad.