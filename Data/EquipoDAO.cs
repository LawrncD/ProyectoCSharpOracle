using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using MiProyectoCSharp.Models;
using MiProyectoCSharp.Helpers;

namespace MiProyectoCSharp.Data
{
    public class EquipoDAO
    {
        public List<Equipo> ObtenerTodos()
        {
            var lista = new List<Equipo>();
            using var connection = DbConnectionHelper.GetConnection();
            string sql = "SELECT id_equipo, nombre, pais, id_confederacion, valor_total_equipo FROM Equipo";
            using var cmd = new OracleCommand(sql, connection);
            cmd.BindByName = true;
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                lista.Add(new Equipo
                {
                    IdEquipo = Convert.ToInt32(reader["id_equipo"]),
                    Nombre = reader["nombre"].ToString() ?? "",
                    Pais = reader["pais"].ToString() ?? "",
                    IdConfederacion = Convert.ToInt32(reader["id_confederacion"]),
                    ValorTotalEquipo = Convert.ToDecimal(reader["valor_total_equipo"])
                });
            }
            return lista;
        }

        public bool Insertar(Equipo equipo)
        {
            using var connection = DbConnectionHelper.GetConnection();
            string sql = "INSERT INTO Equipo (nombre, pais, id_confederacion, valor_total_equipo) VALUES (:nombre, :pais, :idConf, :valor)";
            using var cmd = new OracleCommand(sql, connection);
            cmd.BindByName = true;
            cmd.Parameters.Add(new OracleParameter("nombre", equipo.Nombre));
            cmd.Parameters.Add(new OracleParameter("pais", equipo.Pais));
            cmd.Parameters.Add(new OracleParameter("idConf", equipo.IdConfederacion));
            cmd.Parameters.Add(new OracleParameter("valor", equipo.ValorTotalEquipo));

            return cmd.ExecuteNonQuery() > 0;
        }

        // Determinar el equipo más costoso de los que van a jugar en cada país (México, USA, Canadá)
        // SQL: Encontramos los partidos en estadios de un pais, tomamos los equipos locales y visitantes,
        // agrupamos y sacamos el maximo valor.
        public DataTable ObtenerEquipoMasCostosoPorPais()
        {
            var dt = new DataTable();
            using var connection = DbConnectionHelper.GetConnection();
            string sql = @"
                WITH EquiposEnPais AS (
                    SELECT DISTINCT e.id_equipo, e.nombre, e.valor_total_equipo, pa.nombre as pais_anfitrion
                    FROM Partido p
                    JOIN Estadio est ON p.id_estadio = est.id_estadio
                    JOIN Ciudad c ON est.id_ciudad = c.id_ciudad
                    JOIN PaisAnfitrion pa ON c.id_pais_anfitrion = pa.id_pais_anfitrion
                    JOIN Equipo e ON e.id_equipo = p.id_equipo_local OR e.id_equipo = p.id_equipo_visitante
                )
                SELECT ep.pais_anfitrion, ep.nombre AS equipo, ep.valor_total_equipo
                FROM EquiposEnPais ep
                WHERE ep.valor_total_equipo = (
                    SELECT MAX(e2.valor_total_equipo) 
                    FROM EquiposEnPais e2 
                    WHERE e2.pais_anfitrion = ep.pais_anfitrion
                )";
                
            using var cmd = new OracleCommand(sql, connection);
            cmd.BindByName = true;
            using var da = new OracleDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }
    }
}
