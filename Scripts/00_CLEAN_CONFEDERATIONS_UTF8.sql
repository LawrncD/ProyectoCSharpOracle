-- SCRIPT DE LIMPIEZA EXHAUSTIVA DE CONFEDERACIONES
-- Este script limpia y recrea la tabla de confederaciones garantizando UTF-8
-- Ejecutar COMO USUARIO: copamundial
-- Ejecutar DIRECTAMENTE en SQL*Plus o SQLDeveloper

-- Paso 1: Verificar el charset de la base de datos
SELECT * FROM nls_database_parameters WHERE parameter LIKE '%CHARACTERSET%';

-- Paso 2: Configurar sesión para UTF-8
ALTER SESSION SET NLS_CHARACTERSET='UTF8';
ALTER SESSION SET NLS_NCHARSET='UTF8';
ALTER SESSION SET NLS_LANG='es_ES.UTF8';

-- Paso 3: Verificar el contenido ACTUAL corrupto
COLUMN ID_CONFEDERACION FORMAT 9999
COLUMN NOMBRE FORMAT A50
SELECT ID_CONFEDERACION, NOMBRE, LENGTHB(NOMBRE) as BYTES, LENGTH(NOMBRE) as CHARS
FROM Confederacion
ORDER BY ID_CONFEDERACION;

-- Paso 4: ELIMINAR TODOS LOS DATOS CORRUPTO
DELETE FROM Confederacion;
COMMIT;

-- Paso 5: Insertar confederaciones LIMPIAS - UNA POR UNA
INSERT INTO Confederacion (ID_CONFEDERACION, NOMBRE) 
VALUES (1, 'CONMEBOL');
COMMIT;

INSERT INTO Confederacion (ID_CONFEDERACION, NOMBRE) 
VALUES (2, 'CONCACAF');
COMMIT;

INSERT INTO Confederacion (ID_CONFEDERACION, NOMBRE) 
VALUES (3, 'UEFA');
COMMIT;

INSERT INTO Confederacion (ID_CONFEDERACION, NOMBRE) 
VALUES (4, 'AFC');
COMMIT;

INSERT INTO Confederacion (ID_CONFEDERACION, NOMBRE) 
VALUES (5, 'CAF');
COMMIT;

INSERT INTO Confederacion (ID_CONFEDERACION, NOMBRE) 
VALUES (6, 'OFC');
COMMIT;

-- Paso 6: VERIFICAR que se guardaron CORRECTAMENTE
COLUMN ID_CONFEDERACION FORMAT 9999
COLUMN NOMBRE FORMAT A50
SELECT ID_CONFEDERACION, NOMBRE, LENGTHB(NOMBRE) as BYTES, LENGTH(NOMBRE) as CHARS
FROM Confederacion
ORDER BY ID_CONFEDERACION;

-- Resultado esperado:
-- ID_CONFEDERACION | NOMBRE      | BYTES | CHARS
-- 1                | CONMEBOL    | 8     | 8
-- 2                | CONCACAF    | 8     | 8
-- 3                | UEFA        | 4     | 4
-- 4                | AFC         | 3     | 3
-- 5                | CAF         | 3     | 3
-- 6                | OFC         | 3     | 3

-- Si ves algo como: "confederacióN" o "confederaciÃ³n" es que FALLÓ

-- Paso 7: COMMIT FINAL
COMMIT;
