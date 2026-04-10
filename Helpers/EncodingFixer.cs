using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace MiProyectoCSharp.Helpers
{
    public static class EncodingFixer
    {
        public static void FixAllBrokenEncodings()
        {
            try
            {
                using var conn = DbConnectionHelper.GetConnection();
                var cmd = conn.CreateCommand();

                // Arrays de UPDATE statements que limpian todo
                string[] updateStatements = new string[]
                {
                    // Equipo - nombre
                    "UPDATE Equipo SET nombre = 'España' WHERE nombre LIKE '%Espa%' AND nombre NOT LIKE '%España%'",
                    "UPDATE Equipo SET nombre = 'México' WHERE nombre LIKE '%xico%' AND nombre NOT LIKE '%México%'",
                    "UPDATE Equipo SET nombre = 'Canadá' WHERE nombre LIKE '%Canad%' AND nombre NOT LIKE '%Canadá%'",
                    "UPDATE Equipo SET nombre = 'Japón' WHERE nombre LIKE '%Jap%n%' AND nombre NOT LIKE '%Japón%'",
                    "UPDATE Equipo SET nombre = 'Costa Rica' WHERE nombre LIKE '%Costa%'",
                    
                    // Equipo - pais
                    "UPDATE Equipo SET pais = 'España' WHERE pais LIKE '%Espa%' AND pais NOT LIKE '%España%'",
                    "UPDATE Equipo SET pais = 'México' WHERE pais LIKE '%xico%' AND pais NOT LIKE '%México%'",
                    "UPDATE Equipo SET pais = 'Canadá' WHERE pais LIKE '%Canad%' AND pais NOT LIKE '%Canadá%'",
                    "UPDATE Equipo SET pais = 'Japón' WHERE pais LIKE '%Jap%n%' AND pais NOT LIKE '%Japón%'",
                    "UPDATE Equipo SET pais = 'Costa Rica' WHERE pais LIKE '%Costa%'",

                    // DirectorTecnico - nacionalidad
                    "UPDATE DirectorTecnico SET nacionalidad = 'España' WHERE nacionalidad LIKE '%Espa%' AND nacionalidad NOT LIKE '%España%'",
                    "UPDATE DirectorTecnico SET nacionalidad = 'México' WHERE nacionalidad LIKE '%xico%' AND nacionalidad NOT LIKE '%México%'",
                    "UPDATE DirectorTecnico SET nacionalidad = 'Canadá' WHERE nacionalidad LIKE '%Canad%' AND nacionalidad NOT LIKE '%Canadá%'",
                    "UPDATE DirectorTecnico SET nacionalidad = 'Japón' WHERE nacionalidad LIKE '%Jap%n%' AND nacionalidad NOT LIKE '%Japón%'",

                    // PaisAnfitrion
                    "UPDATE PaisAnfitrion SET nombre = 'México' WHERE nombre LIKE '%xico%' AND nombre NOT LIKE '%México%'",
                    "UPDATE PaisAnfitrion SET nombre = 'Canadá' WHERE nombre LIKE '%Canad%' AND nombre NOT LIKE '%Canadá%'",
                    "UPDATE PaisAnfitrion SET nombre = 'Japón' WHERE nombre LIKE '%Jap%n%' AND nombre NOT LIKE '%Japón%'",

                    // Ciudad
                    "UPDATE Ciudad SET nombre = 'México DF' WHERE nombre LIKE '%xico%' AND nombre LIKE '%DF%'",
                    "UPDATE Ciudad SET nombre = 'Monterrey' WHERE nombre LIKE '%Monterrey%'",
                    "UPDATE Ciudad SET nombre = 'Guadalajara' WHERE nombre LIKE '%Guadalajara%'",
                    "UPDATE Ciudad SET nombre = 'Vancouver' WHERE nombre LIKE '%Vancouver%'",
                    "UPDATE Ciudad SET nombre = 'Toronto' WHERE nombre LIKE '%Toronto%'",

                    // Bitacora
                    "UPDATE Bitacora SET accion = 'Inicio de Sesión' WHERE accion LIKE '%nicio%'",
                    "UPDATE Bitacora SET accion = 'Gestión' WHERE accion LIKE '%Gesti%n%'"
                };

                foreach (var sql in updateStatements)
                {
                    try
                    {
                        cmd.CommandText = sql;
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            System.Diagnostics.Debug.WriteLine($"Fixed {rows} rows: {sql}");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error in {sql}: {ex.Message}");
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EncodingFixer Error: {ex.Message}");
            }
        }
    }
}
