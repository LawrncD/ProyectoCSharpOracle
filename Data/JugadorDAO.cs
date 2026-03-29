using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using MiProyectoCSharp.Models;
using MiProyectoCSharp.Helpers;

namespace MiProyectoCSharp.Data
{
    public class JugadorDAO
    {
        public List<Jugador> ObtenerTodos()
        {
            var lista = new List<Jugador>();
            using var connection = DbConnectionHelper.GetConnection();
            string sql = "SELECT id_jugador, nombre, fecha_nacimiento, posicion, peso, estatura, valor_mercado, id_equipo FROM Jugador ORDER BY id_jugador DESC";
            using var cmd = new OracleCommand(sql, connection);
            cmd.BindByName = true;
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Jugador
                {
                    IdJugador = Convert.ToInt32(reader["id_jugador"]),
                    Nombre = reader["nombre"].ToString() ?? "",
                    FechaNacimiento = Convert.ToDateTime(reader["fecha_nacimiento"]),
                    Posicion = reader["posicion"]?.ToString(),
                    Peso = Convert.ToDecimal(reader["peso"]),
                    Estatura = Convert.ToDecimal(reader["estatura"]),
                    ValorMercado = Convert.ToDecimal(reader["valor_mercado"]),  
                    IdEquipo = Convert.ToInt32(reader["id_equipo"])
                });
            }
            return lista;
        }

        public bool Insertar(Jugador jugador)
        {
            using var connection = DbConnectionHelper.GetConnection();
            string sql = "INSERT INTO Jugador (nombre, fecha_nacimiento, posicion, peso, estatura, valor_mercado, id_equipo) VALUES (:nombre, :fecha, :pos, :peso, :estatura, :valor, :equipo)";
            using var cmd = new OracleCommand(sql, connection);
            cmd.BindByName = true;
            cmd.Parameters.Add(new OracleParameter("nombre", jugador.Nombre));   
            cmd.Parameters.Add(new OracleParameter("fecha", jugador.FechaNacimiento));
            cmd.Parameters.Add(new OracleParameter("pos", jugador.Posicion ?? "Delantero"));
            cmd.Parameters.Add(new OracleParameter("peso", jugador.Peso));
            cmd.Parameters.Add(new OracleParameter("estatura", jugador.Estatura));
            cmd.Parameters.Add(new OracleParameter("valor", jugador.ValorMercado));
            cmd.Parameters.Add(new OracleParameter("equipo", jugador.IdEquipo));

            return cmd.ExecuteNonQuery() > 0;
        }

        // 1. Determinar los datos del jugador más costoso por confederación  
        public DataTable ObtenerJugadorMasCostosoPorConfederacion()
        {
            var dt = new DataTable();
            using var connection = DbConnectionHelper.GetConnection();
            string sql = @"
                SELECT c.nombre AS confederacion, j.nombre AS jugador, j.valor_mercado
                FROM Jugador j
                JOIN Equipo e ON j.id_equipo = e.id_equipo
                JOIN Confederacion c ON e.id_confederacion = c.id_confederacion 
                WHERE j.valor_mercado = (
                    SELECT MAX(j2.valor_mercado)
                    FROM Jugador j2
                    JOIN Equipo e2 ON j2.id_equipo = e2.id_equipo
                    WHERE e2.id_confederacion = e.id_confederacion
                )";
            using var cmd = new OracleCommand(sql, connection);
            cmd.BindByName = true;
            using var da = new OracleDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        // 2. Determinar la cantidad de jugadores por equipo que tienen menos de 21 años.
        public DataTable ObtenerCantidadJugadoresMenoresDe21PorEquipo()
        {
            var dt = new DataTable();
            using var connection = DbConnectionHelper.GetConnection();
            string sql = @"
                SELECT e.nombre AS equipo, COUNT(j.id_jugador) AS menores_de_21 
                FROM Equipo e
                LEFT JOIN Jugador j ON e.id_equipo = j.id_equipo AND (MONTHS_BETWEEN(SYSDATE, j.fecha_nacimiento) / 12) < 21
                GROUP BY e.nombre
                ORDER BY e.nombre";
            using var cmd = new OracleCommand(sql, connection);
            cmd.BindByName = true;
            using var da = new OracleDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        // Reporte: Listar los jugadores cuyo peso, estatura y equipo están dentro de lo solicitado.
        public DataTable ObtenerJugadoresFiltrados(decimal pesoMin, decimal pesoMax, decimal estMin, decimal estMax, int? idEquipo)
        {
            var dt = new DataTable();
            using var connection = DbConnectionHelper.GetConnection();
            string sql = "SELECT j.nombre, j.peso, j.estatura, e.nombre AS equipo FROM Jugador j JOIN Equipo e ON j.id_equipo = e.id_equipo WHERE j.peso BETWEEN :pesoMin AND :pesoMax AND j.estatura BETWEEN :estMin AND :estMax";

            if (idEquipo.HasValue && idEquipo.Value > 0)
            {
                sql += " AND j.id_equipo = :idEq";
            }

            using var cmd = new OracleCommand(sql, connection);
            cmd.BindByName = true;
            cmd.Parameters.Add(new OracleParameter("pesoMin", pesoMin));        
            cmd.Parameters.Add(new OracleParameter("pesoMax", pesoMax));        
            cmd.Parameters.Add(new OracleParameter("estMin", estMin));
            cmd.Parameters.Add(new OracleParameter("estMax", estMax));
            if (idEquipo.HasValue && idEquipo.Value > 0)
            {
                cmd.Parameters.Add(new OracleParameter("idEq", idEquipo.Value));
            }

            using var da = new OracleDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }
    }
}
