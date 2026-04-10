-- Script para corregir definitivamente todos los caracteres corruptos en la base de datos
-- Ejecutar como: sqlplus copamundial/copamundial@XE < FixEncoding.sql

-- TABLA: Equipo
UPDATE Equipo SET nombre = 'España' WHERE nombre LIKE '%Espa%' AND nombre NOT LIKE '%España%';
UPDATE Equipo SET nombre = 'México' WHERE nombre LIKE '%M%xico%' AND nombre NOT LIKE '%México%';
UPDATE Equipo SET nombre = 'Canadá' WHERE nombre LIKE '%Canad%' AND nombre NOT LIKE '%Canadá%';
UPDATE Equipo SET nombre = 'Japón' WHERE nombre LIKE '%Jap%n%' AND nombre NOT LIKE '%Japón%';
UPDATE Equipo SET nombre = 'Costa Rica' WHERE nombre LIKE '%Costa Rica%' AND LENGTH(nombre) > 15;
UPDATE Equipo SET nombre = 'República Dominicana' WHERE nombre LIKE '%Rep%blica%' AND nombre NOT LIKE '%República Dominicana%';

UPDATE Equipo SET pais = 'España' WHERE pais LIKE '%Espa%' AND pais NOT LIKE '%España%';
UPDATE Equipo SET pais = 'México' WHERE pais LIKE '%M%xico%' AND pais NOT LIKE '%México%';
UPDATE Equipo SET pais = 'Canadá' WHERE pais LIKE '%Canad%' AND pais NOT LIKE '%Canadá%';
UPDATE Equipo SET pais = 'Japón' WHERE pais LIKE '%Jap%n%' AND pais NOT LIKE '%Japón%';
UPDATE Equipo SET pais = 'Costa Rica' WHERE pais LIKE '%Costa Rica%' AND LENGTH(pais) > 15;

-- TABLA: DirectorTecnico
UPDATE DirectorTecnico SET nacionalidad = 'España' WHERE nacionalidad LIKE '%Espa%' AND nacionalidad NOT LIKE '%España%';
UPDATE DirectorTecnico SET nacionalidad = 'México' WHERE nacionalidad LIKE '%M%xico%' AND nacionalidad NOT LIKE '%México%';
UPDATE DirectorTecnico SET nacionalidad = 'Canadá' WHERE nacionalidad LIKE '%Canad%' AND nacionalidad NOT LIKE '%Canadá%';
UPDATE DirectorTecnico SET nacionalidad = 'Japón' WHERE nacionalidad LIKE '%Jap%n%' AND nacionalidad NOT LIKE '%Japón%';
UPDATE DirectorTecnico SET nacionalidad = 'Costa Rica' WHERE nacionalidad LIKE '%Costa Rica%' AND LENGTH(nacionalidad) > 15;

-- TABLA: PaisAnfitrion
UPDATE PaisAnfitrion SET nombre = 'México' WHERE nombre LIKE '%M%xico%' AND nombre NOT LIKE '%México%';
UPDATE PaisAnfitrion SET nombre = 'Canadá' WHERE nombre LIKE '%Canad%' AND nombre NOT LIKE '%Canadá%';
UPDATE PaisAnfitrion SET nombre = 'Japón' WHERE nombre LIKE '%Jap%n%' AND nombre NOT LIKE '%Japón%';

-- TABLA: Ciudad
UPDATE Ciudad SET nombre = 'México DF' WHERE nombre LIKE '%Mexico%' OR nombre LIKE '%M%xico%';
UPDATE Ciudad SET nombre = 'Monterrey' WHERE nombre LIKE '%Monterrey%';
UPDATE Ciudad SET nombre = 'Guadalajara' WHERE nombre LIKE '%Guadalajara%';
UPDATE Ciudad SET nombre = 'Vancouver' WHERE nombre LIKE '%Vancouver%';
UPDATE Ciudad SET nombre = 'Toronto' WHERE nombre LIKE '%Toronto%';
UPDATE Ciudad SET nombre = 'Tokio' WHERE nombre LIKE '%Tokio%' OR nombre LIKE '%Tokyo%';
UPDATE Ciudad SET nombre = 'Osaka' WHERE nombre LIKE '%Osaka%';

-- TABLA: Confederacion (si hay corrupción)
UPDATE Confederacion SET nombre = 'CONMEBOL' WHERE nombre LIKE '%CONMEBOL%';
UPDATE Confederacion SET nombre = 'CONCACAF' WHERE nombre LIKE '%CONCACAF%';
UPDATE Confederacion SET nombre = 'UEFA' WHERE nombre LIKE '%UEFA%';
UPDATE Confederacion SET nombre = 'AFC' WHERE nombre LIKE '%AFC%';
UPDATE Confederacion SET nombre = 'CAF' WHERE nombre LIKE '%CAF%';
UPDATE Confederacion SET nombre = 'OFC' WHERE nombre LIKE '%OFC%';

-- TABLA: Bitacora
UPDATE Bitacora SET accion = 'Inicio de Sesión' WHERE accion LIKE '%nicio de Sesi%' OR accion LIKE '%Inicio de Sesion%';
UPDATE Bitacora SET accion = 'Gestión de Usuarios' WHERE accion LIKE '%Gesti%n%Usuarios%';
UPDATE Bitacora SET accion = 'Creación de Equipo' WHERE accion LIKE '%Creaci%n%Equipo%';
UPDATE Bitacora SET accion = 'Modificación de Dato' WHERE accion LIKE '%Modificaci%n%';
UPDATE Bitacora SET accion = 'Eliminación' WHERE accion LIKE '%Eliminaci%n%';

COMMIT;
