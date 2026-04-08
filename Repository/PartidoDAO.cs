

using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using MiProyectoCSharp.Helpers;

namespace MiProyectoCSharp.Repository
{
    public class PartidoDAO
    {
        // Consulta 2: Listar los partidos que se llevarán a cabo en un estadio cualquiera que el usuario elija.
        public DataTable ObtenerPartidosPorEstadio(int idEstadio)
        {
            var dt = new DataTable();
            using var connection = DbConnectionHelper.GetConnection();
            string sql = @"
                SELECT p.fecha_hora, eq_loc.nombre AS local, eq_vis.nombre AS visitante, g.nombre_grupo AS grupo
                FROM Partido p
                JOIN Equipo eq_loc ON p.id_equipo_local = eq_loc.id_equipo
                JOIN Equipo eq_vis ON p.id_equipo_visitante = eq_vis.id_equipo
                JOIN Grupo g ON p.id_grupo = g.id_grupo
                WHERE p.id_estadio = :idEstadio
                ORDER BY p.fecha_hora";
                
            using var cmd = new OracleCommand(sql, connection);
            cmd.Parameters.Add(new OracleParameter("idEstadio", idEstadio));
            using var da = new OracleDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        // Reporte 4: Listar los países que se jugarán en cada país anfitrión
        public DataTable ObtenerPaisesVisitantesPorPaisAnfitrion()
        {
            var dt = new DataTable();
            using var connection = DbConnectionHelper.GetConnection();
            string sql = @"
                SELECT DISTINCT pa.nombre AS pais_anfitrion, e.pais AS pais_visitante
                FROM Partido p
                JOIN Estadio est ON p.id_estadio = est.id_estadio
                JOIN Ciudad c ON est.id_ciudad = c.id_ciudad
                JOIN PaisAnfitrion pa ON c.id_pais_anfitrion = pa.id_pais_anfitrion
                JOIN Equipo e ON e.id_equipo = p.id_equipo_local OR e.id_equipo = p.id_equipo_visitante
                ORDER BY pa.nombre, e.pais";
                
            using var cmd = new OracleCommand(sql, connection);
            using var da = new OracleDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }
    }
}
