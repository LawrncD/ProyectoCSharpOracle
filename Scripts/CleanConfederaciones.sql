-- Script para limpiar y recrear las confederaciones correctamente
-- Primero eliminamos las confederaciones corruptas
DELETE FROM Confederacion WHERE ID_CONFEDERACION > 0;

-- Ahora insertamos las 6 confederaciones principales limpias en UTF-8
INSERT INTO Confederacion (ID_CONFEDERACION, Nombre) VALUES (1, 'CONMEBOL');
INSERT INTO Confederacion (ID_CONFEDERACION, Nombre) VALUES (2, 'CONCACAF');
INSERT INTO Confederacion (ID_CONFEDERACION, Nombre) VALUES (3, 'UEFA');
INSERT INTO Confederacion (ID_CONFEDERACION, Nombre) VALUES (4, 'AFC');
INSERT INTO Confederacion (ID_CONFEDERACION, Nombre) VALUES (5, 'CAF');
INSERT INTO Confederacion (ID_CONFEDERACION, Nombre) VALUES (6, 'OFC');

COMMIT;

-- Verificar los datos insertados
SELECT * FROM Confederacion;
