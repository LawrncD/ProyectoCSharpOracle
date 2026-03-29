-- =====================================================
-- CREACIÓN DE LA BASE DE DATOS MUNDIAL 2026 (Adaptado para Oracle)
-- =====================================================

-- Puesto que ya estamos en el esquema/usuario 'copamundial', creamos las estructuras directamente.
-- Limpiar estructuras anteriores si existen
BEGIN
   FOR cur_rec IN (SELECT object_name, object_type FROM user_objects WHERE object_type IN ('TABLE', 'VIEW', 'PROCEDURE', 'FUNCTION', 'TRIGGER', 'SEQUENCE')) LOOP
      BEGIN
         IF cur_rec.object_type = 'TABLE' THEN
            EXECUTE IMMEDIATE 'DROP TABLE ' || cur_rec.object_name || ' CASCADE CONSTRAINTS';
         ELSIF cur_rec.object_type = 'SEQUENCE' THEN
            EXECUTE IMMEDIATE 'DROP SEQUENCE ' || cur_rec.object_name;
         ELSIF cur_rec.object_type = 'VIEW' THEN
            EXECUTE IMMEDIATE 'DROP VIEW ' || cur_rec.object_name;
         ELSIF cur_rec.object_type IN ('PROCEDURE', 'FUNCTION', 'PACKAGE') THEN
            EXECUTE IMMEDIATE 'DROP ' || cur_rec.object_type || ' ' || cur_rec.object_name;
         END IF;
      EXCEPTION
         WHEN OTHERS THEN NULL;
      END;
   END LOOP;
END;
/

-- =====================================================
-- TABLAS PRINCIPALES
-- =====================================================

CREATE TABLE Usuario (
    id_usuario NUMBER PRIMARY KEY,
    nombre_usuario VARCHAR2(50) UNIQUE NOT NULL,
    contrasena_hash VARCHAR2(255) NOT NULL,
    tipo_usuario VARCHAR2(20) CHECK (tipo_usuario IN ('Administrador', 'Tradicional', 'Esporadico')) NOT NULL,
    fecha_creacion DATE DEFAULT SYSDATE
);
CREATE SEQUENCE seq_usuario START WITH 1 INCREMENT BY 1;
CREATE OR REPLACE TRIGGER trg_usuario_ai BEFORE INSERT ON Usuario FOR EACH ROW
BEGIN
  IF :NEW.id_usuario IS NULL THEN SELECT seq_usuario.NEXTVAL INTO :NEW.id_usuario FROM dual; END IF;
END;
/

CREATE TABLE Confederacion (
    id_confederacion NUMBER PRIMARY KEY,
    nombre VARCHAR2(100) UNIQUE NOT NULL,
    siglas VARCHAR2(10) NOT NULL
);
CREATE SEQUENCE seq_confederacion START WITH 1 INCREMENT BY 1;
CREATE OR REPLACE TRIGGER trg_confederacion_ai BEFORE INSERT ON Confederacion FOR EACH ROW
BEGIN
  IF :NEW.id_confederacion IS NULL THEN SELECT seq_confederacion.NEXTVAL INTO :NEW.id_confederacion FROM dual; END IF;
END;
/

CREATE TABLE PaisAnfitrion (
    id_pais_anfitrion NUMBER PRIMARY KEY,
    nombre VARCHAR2(100) UNIQUE NOT NULL
);
CREATE SEQUENCE seq_paisanfitrion START WITH 1 INCREMENT BY 1;
CREATE OR REPLACE TRIGGER trg_paisanfitrion_ai BEFORE INSERT ON PaisAnfitrion FOR EACH ROW
BEGIN
  IF :NEW.id_pais_anfitrion IS NULL THEN SELECT seq_paisanfitrion.NEXTVAL INTO :NEW.id_pais_anfitrion FROM dual; END IF;
END;
/

CREATE TABLE Grupo (
    id_grupo NUMBER PRIMARY KEY,
    nombre_grupo VARCHAR2(1) NOT NULL CHECK (nombre_grupo IN ('A','B','C','D','E','F','G','H','I','J','K','L'))
);
CREATE SEQUENCE seq_grupo START WITH 1 INCREMENT BY 1;
CREATE OR REPLACE TRIGGER trg_grupo_ai BEFORE INSERT ON Grupo FOR EACH ROW
BEGIN
  IF :NEW.id_grupo IS NULL THEN SELECT seq_grupo.NEXTVAL INTO :NEW.id_grupo FROM dual; END IF;
END;
/

-- =====================================================
-- TABLAS CON DEPENDENCIAS
-- =====================================================

CREATE TABLE Equipo (
    id_equipo NUMBER PRIMARY KEY,
    nombre VARCHAR2(100) UNIQUE NOT NULL,
    pais VARCHAR2(100) NOT NULL,
    valor_total_equipo NUMBER(15,2) DEFAULT 0,
    id_confederacion NUMBER NOT NULL,
    CONSTRAINT fk_equipo_conf FOREIGN KEY (id_confederacion) REFERENCES Confederacion(id_confederacion)
);
CREATE SEQUENCE seq_equipo START WITH 1 INCREMENT BY 1;
CREATE OR REPLACE TRIGGER trg_equipo_ai BEFORE INSERT ON Equipo FOR EACH ROW
BEGIN
  IF :NEW.id_equipo IS NULL THEN SELECT seq_equipo.NEXTVAL INTO :NEW.id_equipo FROM dual; END IF;
END;
/

CREATE TABLE DirectorTecnico (
    id_dt NUMBER PRIMARY KEY,
    nombre VARCHAR2(200) NOT NULL,
    nacionalidad VARCHAR2(100) NOT NULL,
    fecha_nacimiento DATE NOT NULL,
    id_equipo NUMBER UNIQUE NOT NULL,
    CONSTRAINT fk_dt_equipo FOREIGN KEY (id_equipo) REFERENCES Equipo(id_equipo)
);
CREATE SEQUENCE seq_dt START WITH 1 INCREMENT BY 1;
CREATE OR REPLACE TRIGGER trg_dt_ai BEFORE INSERT ON DirectorTecnico FOR EACH ROW
BEGIN
  IF :NEW.id_dt IS NULL THEN SELECT seq_dt.NEXTVAL INTO :NEW.id_dt FROM dual; END IF;
END;
/

CREATE TABLE Jugador (
    id_jugador NUMBER PRIMARY KEY,
    nombre VARCHAR2(200) NOT NULL,
    fecha_nacimiento DATE NOT NULL,
    posicion VARCHAR2(50) NOT NULL,
    peso NUMBER(5,2) NOT NULL CHECK (peso > 0 AND peso < 200),
    estatura NUMBER(3,2) NOT NULL CHECK (estatura > 0 AND estatura < 2.50),
    valor_mercado NUMBER(15,2) NOT NULL CHECK (valor_mercado >= 0),
    id_equipo NUMBER NOT NULL,
    CONSTRAINT fk_jugador_equipo FOREIGN KEY (id_equipo) REFERENCES Equipo(id_equipo)
);
CREATE SEQUENCE seq_jugador START WITH 1 INCREMENT BY 1;
CREATE OR REPLACE TRIGGER trg_jugador_ai BEFORE INSERT ON Jugador FOR EACH ROW
BEGIN
  IF :NEW.id_jugador IS NULL THEN SELECT seq_jugador.NEXTVAL INTO :NEW.id_jugador FROM dual; END IF;
END;
/

-- Triggers para actualizar valor_total_equipo (Adaptación MySQL a Oracle)
CREATE OR REPLACE TRIGGER trg_actualizar_valor_equipo_i
AFTER INSERT ON Jugador
FOR EACH ROW
BEGIN
    UPDATE Equipo 
    SET valor_total_equipo = valor_total_equipo + :NEW.valor_mercado
    WHERE id_equipo = :NEW.id_equipo;
END;
/
CREATE OR REPLACE TRIGGER trg_actualizar_valor_equipo_u
AFTER UPDATE ON Jugador
FOR EACH ROW
BEGIN
    IF :NEW.id_equipo != :OLD.id_equipo THEN
        UPDATE Equipo SET valor_total_equipo = valor_total_equipo + :NEW.valor_mercado WHERE id_equipo = :NEW.id_equipo;
        UPDATE Equipo SET valor_total_equipo = valor_total_equipo - :OLD.valor_mercado WHERE id_equipo = :OLD.id_equipo;
    ELSIF :NEW.valor_mercado != :OLD.valor_mercado THEN
        UPDATE Equipo SET valor_total_equipo = valor_total_equipo - :OLD.valor_mercado + :NEW.valor_mercado WHERE id_equipo = :NEW.id_equipo;
    END IF;
END;
/
CREATE OR REPLACE TRIGGER trg_actualizar_valor_equipo_d
AFTER DELETE ON Jugador
FOR EACH ROW
BEGIN
    UPDATE Equipo SET valor_total_equipo = valor_total_equipo - :OLD.valor_mercado WHERE id_equipo = :OLD.id_equipo;
END;
/

CREATE TABLE Ciudad (
    id_ciudad NUMBER PRIMARY KEY,
    nombre VARCHAR2(100) NOT NULL,
    id_pais_anfitrion NUMBER NOT NULL,
    CONSTRAINT fk_ciudad_pais FOREIGN KEY (id_pais_anfitrion) REFERENCES PaisAnfitrion(id_pais_anfitrion),
    CONSTRAINT unq_ciudad_pais UNIQUE (nombre, id_pais_anfitrion)
);
CREATE SEQUENCE seq_ciudad START WITH 1 INCREMENT BY 1;
CREATE OR REPLACE TRIGGER trg_ciudad_ai BEFORE INSERT ON Ciudad FOR EACH ROW
BEGIN
  IF :NEW.id_ciudad IS NULL THEN SELECT seq_ciudad.NEXTVAL INTO :NEW.id_ciudad FROM dual; END IF;
END;
/

CREATE TABLE Estadio (
    id_estadio NUMBER PRIMARY KEY,
    nombre VARCHAR2(100) UNIQUE NOT NULL,
    capacidad NUMBER(7) NOT NULL CHECK (capacidad > 0),
    id_ciudad NUMBER NOT NULL,
    CONSTRAINT fk_estadio_ciud FOREIGN KEY (id_ciudad) REFERENCES Ciudad(id_ciudad)
);
CREATE SEQUENCE seq_estadio START WITH 1 INCREMENT BY 1;
CREATE OR REPLACE TRIGGER trg_estadio_ai BEFORE INSERT ON Estadio FOR EACH ROW
BEGIN
  IF :NEW.id_estadio IS NULL THEN SELECT seq_estadio.NEXTVAL INTO :NEW.id_estadio FROM dual; END IF;
END;
/

CREATE TABLE Equipo_Grupo (
    id_equipo_grupo NUMBER PRIMARY KEY,
    id_equipo NUMBER NOT NULL,
    id_grupo NUMBER NOT NULL,
    CONSTRAINT fk_eqgr_eq FOREIGN KEY (id_equipo) REFERENCES Equipo(id_equipo),
    CONSTRAINT fk_eqgr_gr FOREIGN KEY (id_grupo) REFERENCES Grupo(id_grupo),
    CONSTRAINT unq_equipo_grupo UNIQUE (id_equipo, id_grupo)
);
CREATE SEQUENCE seq_eq_gr START WITH 1 INCREMENT BY 1;
CREATE OR REPLACE TRIGGER trg_eq_gr_ai BEFORE INSERT ON Equipo_Grupo FOR EACH ROW
BEGIN
  IF :NEW.id_equipo_grupo IS NULL THEN SELECT seq_eq_gr.NEXTVAL INTO :NEW.id_equipo_grupo FROM dual; END IF;
END;
/

CREATE TABLE Partido (
    id_partido NUMBER PRIMARY KEY,
    fecha_hora TIMESTAMP NOT NULL,
    id_estadio NUMBER NOT NULL,
    id_grupo NUMBER NOT NULL,
    id_equipo_local NUMBER NOT NULL,
    id_equipo_visitante NUMBER NOT NULL,
    goles_local NUMBER DEFAULT 0,
    goles_visitante NUMBER DEFAULT 0,
    CONSTRAINT fk_par_est FOREIGN KEY (id_estadio) REFERENCES Estadio(id_estadio),
    CONSTRAINT fk_par_gr FOREIGN KEY (id_grupo) REFERENCES Grupo(id_grupo),
    CONSTRAINT fk_par_eql FOREIGN KEY (id_equipo_local) REFERENCES Equipo(id_equipo),
    CONSTRAINT fk_par_eqv FOREIGN KEY (id_equipo_visitante) REFERENCES Equipo(id_equipo),
    CONSTRAINT chk_eqs_dif CHECK (id_equipo_local != id_equipo_visitante),
    CONSTRAINT chk_goles_ok CHECK (goles_local >= 0 AND goles_visitante >= 0)
);
CREATE SEQUENCE seq_partido START WITH 1 INCREMENT BY 1;
CREATE OR REPLACE TRIGGER trg_partido_ai BEFORE INSERT ON Partido FOR EACH ROW
BEGIN
  IF :NEW.id_partido IS NULL THEN SELECT seq_partido.NEXTVAL INTO :NEW.id_partido FROM dual; END IF;
END;
/

CREATE TABLE Bitacora (
    id_registro NUMBER PRIMARY KEY,
    id_usuario NUMBER NOT NULL,
    fecha_hora_ingreso TIMESTAMP DEFAULT SYSTIMESTAMP,
    fecha_hora_salida TIMESTAMP NULL,
    CONSTRAINT fk_bit_user FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario)
);
CREATE SEQUENCE seq_bitacora START WITH 1 INCREMENT BY 1;
CREATE OR REPLACE TRIGGER trg_bitacora_ai BEFORE INSERT ON Bitacora FOR EACH ROW
BEGIN
  IF :NEW.id_registro IS NULL THEN SELECT seq_bitacora.NEXTVAL INTO :NEW.id_registro FROM dual; END IF;
END;
/

-- =====================================================
-- ÍNDICES
-- =====================================================
CREATE INDEX idx_jugador_equipo ON Jugador(id_equipo);
CREATE INDEX idx_jugador_valor ON Jugador(valor_mercado);
CREATE INDEX idx_jugador_edad ON Jugador(fecha_nacimiento);
CREATE INDEX idx_partido_fecha ON Partido(fecha_hora);
CREATE INDEX idx_partido_estadio ON Partido(id_estadio);
CREATE INDEX idx_bitacora_fechas ON Bitacora(fecha_hora_ingreso, fecha_hora_salida);
CREATE INDEX idx_bitacora_usuario ON Bitacora(id_usuario);

-- =====================================================
-- VISTAS
-- =====================================================
CREATE OR REPLACE VIEW vw_jugadores_completo AS
SELECT j.*, e.nombre AS equipo_nombre, e.pais AS equipo_pais, c.nombre AS confederacion, c.siglas AS confederacion_siglas
FROM Jugador j
INNER JOIN Equipo e ON j.id_equipo = e.id_equipo
INNER JOIN Confederacion c ON e.id_confederacion = c.id_confederacion;

CREATE OR REPLACE VIEW vw_partidos_completo AS
SELECT p.id_partido, p.fecha_hora, el.nombre AS equipo_local, ev.nombre AS equipo_visitante,
       est.nombre AS estadio, ci.nombre AS ciudad, pa.nombre AS pais_anfitrion, g.nombre_grupo AS grupo,
       p.goles_local, p.goles_visitante
FROM Partido p
INNER JOIN Equipo el ON p.id_equipo_local = el.id_equipo
INNER JOIN Equipo ev ON p.id_equipo_visitante = ev.id_equipo
INNER JOIN Estadio est ON p.id_estadio = est.id_estadio
INNER JOIN Ciudad ci ON est.id_ciudad = ci.id_ciudad
INNER JOIN PaisAnfitrion pa ON ci.id_pais_anfitrion = pa.id_pais_anfitrion
INNER JOIN Grupo g ON p.id_grupo = g.id_grupo;

-- =====================================================
-- INSERTS LIMITADOS Y COMPATIBLES CON FECHAS ORACLE
-- =====================================================
-- Insertar Usuario Administrador
INSERT INTO Usuario (nombre_usuario, contrasena_hash, tipo_usuario) VALUES 
('admin', '$2a$11$68/vD./uU/8wT2m7JkG0/OSw58k1gS9u4C9sYh89r.7x/7pXk0pTe', 'Administrador');

INSERT INTO Confederacion (nombre, siglas) VALUES ('Unión de Federaciones Europeas de Fútbol', 'UEFA');
INSERT INTO Confederacion (nombre, siglas) VALUES ('Confederación Sudamericana de Fútbol', 'CONMEBOL');
INSERT INTO Confederacion (nombre, siglas) VALUES ('Confederación de Fútbol de Norte, Centroamérica y el Caribe', 'CONCACAF');
INSERT INTO Confederacion (nombre, siglas) VALUES ('Confederación Africana de Fútbol', 'CAF');
INSERT INTO Confederacion (nombre, siglas) VALUES ('Confederación Asiática de Fútbol', 'AFC');
INSERT INTO Confederacion (nombre, siglas) VALUES ('Confederación de Fútbol de Oceanía', 'OFC');

INSERT INTO PaisAnfitrion (nombre) VALUES ('México');
INSERT INTO PaisAnfitrion (nombre) VALUES ('Estados Unidos');
INSERT INTO PaisAnfitrion (nombre) VALUES ('Canadá');

INSERT INTO Grupo (nombre_grupo) VALUES ('A');
INSERT INTO Grupo (nombre_grupo) VALUES ('B');
INSERT INTO Grupo (nombre_grupo) VALUES ('C');
INSERT INTO Grupo (nombre_grupo) VALUES ('D');
INSERT INTO Grupo (nombre_grupo) VALUES ('E');
INSERT INTO Grupo (nombre_grupo) VALUES ('F');
INSERT INTO Grupo (nombre_grupo) VALUES ('G');
INSERT INTO Grupo (nombre_grupo) VALUES ('H');
INSERT INTO Grupo (nombre_grupo) VALUES ('I');
INSERT INTO Grupo (nombre_grupo) VALUES ('J');
INSERT INTO Grupo (nombre_grupo) VALUES ('K');
INSERT INTO Grupo (nombre_grupo) VALUES ('L');

INSERT INTO Ciudad (nombre, id_pais_anfitrion) VALUES ('Ciudad de México', 1);
INSERT INTO Ciudad (nombre, id_pais_anfitrion) VALUES ('Guadalajara', 1);
INSERT INTO Ciudad (nombre, id_pais_anfitrion) VALUES ('Monterrey', 1);
INSERT INTO Ciudad (nombre, id_pais_anfitrion) VALUES ('Los Ángeles', 2);
INSERT INTO Ciudad (nombre, id_pais_anfitrion) VALUES ('Nueva York', 2);
INSERT INTO Ciudad (nombre, id_pais_anfitrion) VALUES ('Miami', 2);
INSERT INTO Ciudad (nombre, id_pais_anfitrion) VALUES ('Toronto', 3);
INSERT INTO Ciudad (nombre, id_pais_anfitrion) VALUES ('Vancouver', 3);
INSERT INTO Ciudad (nombre, id_pais_anfitrion) VALUES ('Montreal', 3);

INSERT INTO Estadio (nombre, capacidad, id_ciudad) VALUES ('Estadio Azteca', 87523, 1);
INSERT INTO Estadio (nombre, capacidad, id_ciudad) VALUES ('Estadio Akron', 49850, 2);
INSERT INTO Estadio (nombre, capacidad, id_ciudad) VALUES ('Estadio BBVA', 53500, 3);
INSERT INTO Estadio (nombre, capacidad, id_ciudad) VALUES ('Rose Bowl', 92542, 4);
INSERT INTO Estadio (nombre, capacidad, id_ciudad) VALUES ('MetLife Stadium', 82500, 5);
INSERT INTO Estadio (nombre, capacidad, id_ciudad) VALUES ('Hard Rock Stadium', 65326, 6);
INSERT INTO Estadio (nombre, capacidad, id_ciudad) VALUES ('BMO Field', 30991, 7);
INSERT INTO Estadio (nombre, capacidad, id_ciudad) VALUES ('BC Place', 54500, 8);
INSERT INTO Estadio (nombre, capacidad, id_ciudad) VALUES ('Stade Olympique', 56040, 9);

INSERT INTO Equipo (nombre, pais, id_confederacion) VALUES ('Brasil', 'Brasil', 2);
INSERT INTO Equipo (nombre, pais, id_confederacion) VALUES ('Argentina', 'Argentina', 2);
INSERT INTO Equipo (nombre, pais, id_confederacion) VALUES ('Francia', 'Francia', 1);
INSERT INTO Equipo (nombre, pais, id_confederacion) VALUES ('España', 'España', 1);
INSERT INTO Equipo (nombre, pais, id_confederacion) VALUES ('México', 'México', 3);
INSERT INTO Equipo (nombre, pais, id_confederacion) VALUES ('Estados Unidos', 'Estados Unidos', 3);
INSERT INTO Equipo (nombre, pais, id_confederacion) VALUES ('Canadá', 'Canadá', 3);
INSERT INTO Equipo (nombre, pais, id_confederacion) VALUES ('Japón', 'Japón', 5);

INSERT INTO DirectorTecnico (nombre, nacionalidad, fecha_nacimiento, id_equipo) VALUES ('Tite', 'Brasileña', TO_DATE('1961-05-25', 'YYYY-MM-DD'), 1);
INSERT INTO DirectorTecnico (nombre, nacionalidad, fecha_nacimiento, id_equipo) VALUES ('Lionel Scaloni', 'Argentina', TO_DATE('1978-05-16', 'YYYY-MM-DD'), 2);
INSERT INTO DirectorTecnico (nombre, nacionalidad, fecha_nacimiento, id_equipo) VALUES ('Didier Deschamps', 'Francesa', TO_DATE('1968-10-15', 'YYYY-MM-DD'), 3);
INSERT INTO DirectorTecnico (nombre, nacionalidad, fecha_nacimiento, id_equipo) VALUES ('Luis de la Fuente', 'Española', TO_DATE('1961-06-21', 'YYYY-MM-DD'), 4);
INSERT INTO DirectorTecnico (nombre, nacionalidad, fecha_nacimiento, id_equipo) VALUES ('Jaime Lozano', 'Mexicana', TO_DATE('1978-09-29', 'YYYY-MM-DD'), 5);
INSERT INTO DirectorTecnico (nombre, nacionalidad, fecha_nacimiento, id_equipo) VALUES ('Gregg Berhalter', 'Estadounidense', TO_DATE('1973-08-01', 'YYYY-MM-DD'), 6);
INSERT INTO DirectorTecnico (nombre, nacionalidad, fecha_nacimiento, id_equipo) VALUES ('John Herdman', 'Inglesa', TO_DATE('1975-07-19', 'YYYY-MM-DD'), 7);
INSERT INTO DirectorTecnico (nombre, nacionalidad, fecha_nacimiento, id_equipo) VALUES ('Hajime Moriyasu', 'Japonesa', TO_DATE('1968-08-23', 'YYYY-MM-DD'), 8);

INSERT INTO Jugador (nombre, fecha_nacimiento, posicion, peso, estatura, valor_mercado, id_equipo) VALUES ('Neymar Jr', TO_DATE('1992-02-05', 'YYYY-MM-DD'), 'Delantero', 68.5, 1.75, 90000000, 1);
INSERT INTO Jugador (nombre, fecha_nacimiento, posicion, peso, estatura, valor_mercado, id_equipo) VALUES ('Vinicius Jr', TO_DATE('2000-07-12', 'YYYY-MM-DD'), 'Delantero', 73.0, 1.76, 150000000, 1);
INSERT INTO Jugador (nombre, fecha_nacimiento, posicion, peso, estatura, valor_mercado, id_equipo) VALUES ('Lionel Messi', TO_DATE('1987-06-24', 'YYYY-MM-DD'), 'Delantero', 72.0, 1.70, 50000000, 2);
INSERT INTO Jugador (nombre, fecha_nacimiento, posicion, peso, estatura, valor_mercado, id_equipo) VALUES ('Enzo Fernández', TO_DATE('2001-01-17', 'YYYY-MM-DD'), 'Centrocampista', 78.0, 1.78, 75000000, 2);
INSERT INTO Jugador (nombre, fecha_nacimiento, posicion, peso, estatura, valor_mercado, id_equipo) VALUES ('Kylian Mbappé', TO_DATE('1998-12-20', 'YYYY-MM-DD'), 'Delantero', 73.0, 1.78, 180000000, 3);
INSERT INTO Jugador (nombre, fecha_nacimiento, posicion, peso, estatura, valor_mercado, id_equipo) VALUES ('Eduardo Camavinga', TO_DATE('2002-11-10', 'YYYY-MM-DD'), 'Centrocampista', 68.0, 1.82, 85000000, 3);
INSERT INTO Jugador (nombre, fecha_nacimiento, posicion, peso, estatura, valor_mercado, id_equipo) VALUES ('Pedri', TO_DATE('2002-11-25', 'YYYY-MM-DD'), 'Centrocampista', 60.0, 1.74, 90000000, 4);
INSERT INTO Jugador (nombre, fecha_nacimiento, posicion, peso, estatura, valor_mercado, id_equipo) VALUES ('Gavi', TO_DATE('2004-08-05', 'YYYY-MM-DD'), 'Centrocampista', 70.0, 1.73, 80000000, 4);
INSERT INTO Jugador (nombre, fecha_nacimiento, posicion, peso, estatura, valor_mercado, id_equipo) VALUES ('Santiago Giménez', TO_DATE('2001-04-18', 'YYYY-MM-DD'), 'Delantero', 76.0, 1.82, 40000000, 5);
INSERT INTO Jugador (nombre, fecha_nacimiento, posicion, peso, estatura, valor_mercado, id_equipo) VALUES ('Edson Álvarez', TO_DATE('1997-10-24', 'YYYY-MM-DD'), 'Defensa', 75.0, 1.87, 35000000, 5);

INSERT INTO Equipo_Grupo (id_equipo, id_grupo) VALUES (1, 1);
INSERT INTO Equipo_Grupo (id_equipo, id_grupo) VALUES (2, 1);
INSERT INTO Equipo_Grupo (id_equipo, id_grupo) VALUES (3, 2);
INSERT INTO Equipo_Grupo (id_equipo, id_grupo) VALUES (4, 2);

INSERT INTO Partido (fecha_hora, id_estadio, id_grupo, id_equipo_local, id_equipo_visitante) VALUES (TO_TIMESTAMP('2026-06-14 15:00:00', 'YYYY-MM-DD HH24:MI:SS'), 1, 1, 1, 2);

COMMIT;
EXIT;
