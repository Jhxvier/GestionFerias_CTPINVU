-- Script para insertar 30 estudiantes en la base de datos
START TRANSACTION;

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20438074200', 'Carlos', 'Sanchez', 'Masculino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'carlos.sanchez1@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '7', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20691051801', 'Ana', 'Ramirez', 'Femenino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'ana.ramirez2@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '9', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20121056002', 'Luis', 'Flores', 'Femenino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'luis.flores3@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '10', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20129039203', 'Maria', 'Castillo', 'Femenino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'maria.castillo4@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '11', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20281061004', 'Jose', 'Molina', 'Femenino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'jose.molina5@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '8', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20847018005', 'Fernanda', 'Guzman', 'Femenino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'fernanda.guzman6@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '7', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20651039306', 'David', 'Mendoza', 'Femenino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'david.mendoza7@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '7', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20847099307', 'Laura', 'Navarro', 'Masculino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'laura.navarro8@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '10', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20255039608', 'Miguel', 'Mendez', 'Femenino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'miguel.mendez9@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '10', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20472080809', 'Sofia', 'Perez', 'Masculino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'sofia.perez10@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '7', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20922094310', 'Jorge', 'Sanchez', 'Femenino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'jorge.sanchez11@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '11', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20744038311', 'Camila', 'Ramirez', 'Femenino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'camila.ramirez12@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '8', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20555033912', 'Daniel', 'Flores', 'Masculino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'daniel.flores13@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '8', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20819079913', 'Valentina', 'Castillo', 'Masculino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'valentina.castillo14@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '7', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20603065714', 'Alejandro', 'Molina', 'Masculino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'alejandro.molina15@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '7', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20765071415', 'Valeria', 'Guzman', 'Femenino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'valeria.guzman16@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '11', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20379084416', 'Andres', 'Mendoza', 'Masculino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'andres.mendoza17@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '11', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20588042217', 'Lucia', 'Navarro', 'Masculino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'lucia.navarro18@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '7', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20338061318', 'Diego', 'Mendez', 'Masculino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'diego.mendez19@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '11', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20257028519', 'Isabella', 'Perez', 'Femenino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'isabella.perez20@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '7', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20585020820', 'Javier', 'Sanchez', 'Masculino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'javier.sanchez21@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '11', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20193042721', 'Mariana', 'Ramirez', 'Masculino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'mariana.ramirez22@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '11', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20215055522', 'Ricardo', 'Flores', 'Femenino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'ricardo.flores23@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '7', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20563023723', 'Daniela', 'Castillo', 'Masculino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'daniela.castillo24@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '11', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20923059324', 'Gabriel', 'Molina', 'Masculino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'gabriel.molina25@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '11', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20882074025', 'Gabriela', 'Guzman', 'Masculino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'gabriela.guzman26@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '9', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20899034126', 'Fernando', 'Mendoza', 'Femenino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'fernando.mendoza27@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '7', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20464075227', 'Andrea', 'Navarro', 'Femenino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'andrea.navarro28@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '7', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20162038628', 'Manuel', 'Mendez', 'Masculino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'manuel.mendez29@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '10', NOW());

INSERT INTO personas (documento, nombres, apellidos, sexo, fecha_creacion) VALUES ('20296060029', 'Natalia', 'Perez', 'Masculino', NOW());
SET @p_id = LAST_INSERT_ID();
INSERT INTO usuarios (persona_id, correo, password_hash, requiere_cambio_clave, estado, fecha_creacion) VALUES (@p_id, 'natalia.perez30@estudiante.cr', '03ac674216f3e15c761ee1a5e255f067953623c8b388b4459e13f978d7c846f4', 0, 'Activo', NOW());
SET @u_id = LAST_INSERT_ID();
INSERT INTO estudiantes (estudiante_id, grado, fecha_creacion) VALUES (@u_id, '8', NOW());

COMMIT;
