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
                    // --- CONFEDERACION (prioridad: estas son lookups importantes) ---
                    "UPDATE Confederacion SET nombre = 'CONMEBOL' WHERE nombre LIKE '%CONMEBOL%' OR nombre LIKE '%CONMEBOL%'",
                    "UPDATE Confederacion SET nombre = 'CONCACAF' WHERE nombre LIKE '%CONCACAF%' OR nombre LIKE '%CONCACAF%'",
                    "UPDATE Confederacion SET nombre = 'UEFA' WHERE nombre LIKE '%UEFA%' OR nombre LIKE '%UEFA%'",
                    "UPDATE Confederacion SET nombre = 'AFC' WHERE nombre LIKE '%AFC%' OR nombre LIKE '%AFC%'",
                    "UPDATE Confederacion SET nombre = 'CAF' WHERE nombre LIKE '%CAF%' OR nombre LIKE '%CAF%'",
                    "UPDATE Confederacion SET nombre = 'OFC' WHERE nombre LIKE '%OFC%' OR nombre LIKE '%OFC%'",
                    
                    // --- EQUIPO - nombre ---
                    "UPDATE Equipo SET nombre = 'España' WHERE nombre LIKE '%Espa%' AND nombre NOT LIKE '%España%'",
                    "UPDATE Equipo SET nombre = 'México' WHERE nombre LIKE '%xico%' AND nombre NOT LIKE '%México%'",
                    "UPDATE Equipo SET nombre = 'Canadá' WHERE nombre LIKE '%Canad%' AND nombre NOT LIKE '%Canadá%'",
                    "UPDATE Equipo SET nombre = 'Japón' WHERE nombre LIKE '%Jap%n%' AND nombre NOT LIKE '%Japón%'",
                    "UPDATE Equipo SET nombre = 'Costa Rica' WHERE nombre LIKE '%Costa%'",
                    
                    // --- EQUIPO - pais ---
                    "UPDATE Equipo SET pais = 'España' WHERE pais LIKE '%Espa%' AND pais NOT LIKE '%España%'",
                    "UPDATE Equipo SET pais = 'México' WHERE pais LIKE '%xico%' AND pais NOT LIKE '%México%'",
                    "UPDATE Equipo SET pais = 'Canadá' WHERE pais LIKE '%Canad%' AND pais NOT LIKE '%Canadá%'",
                    "UPDATE Equipo SET pais = 'Japón' WHERE pais LIKE '%Jap%n%' AND pais NOT LIKE '%Japón%'",
                    "UPDATE Equipo SET pais = 'Costa Rica' WHERE pais LIKE '%Costa%'",

                    // --- DIRECTORTECNICO - nacionalidad ---
                    "UPDATE DirectorTecnico SET nacionalidad = 'España' WHERE nacionalidad LIKE '%Espa%' AND nacionalidad NOT LIKE '%España%'",
                    "UPDATE DirectorTecnico SET nacionalidad = 'México' WHERE nacionalidad LIKE '%xico%' AND nacionalidad NOT LIKE '%México%'",
                    "UPDATE DirectorTecnico SET nacionalidad = 'Canadá' WHERE nacionalidad LIKE '%Canad%' AND nacionalidad NOT LIKE '%Canadá%'",
                    "UPDATE DirectorTecnico SET nacionalidad = 'Japón' WHERE nacionalidad LIKE '%Jap%n%' AND nacionalidad NOT LIKE '%Japón%'",

                    // --- PAISANFITRION ---
                    "UPDATE PaisAnfitrion SET nombre = 'México' WHERE nombre LIKE '%xico%' AND nombre NOT LIKE '%México%'",
                    "UPDATE PaisAnfitrion SET nombre = 'Canadá' WHERE nombre LIKE '%Canad%' AND nombre NOT LIKE '%Canadá%'",
                    "UPDATE PaisAnfitrion SET nombre = 'Japón' WHERE nombre LIKE '%Jap%n%' AND nombre NOT LIKE '%Japón%'",

                    // --- CIUDAD ---
                    "UPDATE Ciudad SET nombre = 'México DF' WHERE nombre LIKE '%xico%' AND nombre LIKE '%DF%'",
                    "UPDATE Ciudad SET nombre = 'Monterrey' WHERE nombre LIKE '%Monterrey%'",
                    "UPDATE Ciudad SET nombre = 'Guadalajara' WHERE nombre LIKE '%Guadalajara%'",
                    "UPDATE Ciudad SET nombre = 'Vancouver' WHERE nombre LIKE '%Vancouver%'",
                    "UPDATE Ciudad SET nombre = 'Toronto' WHERE nombre LIKE '%Toronto%'",

                    // --- BITACORA ---
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
