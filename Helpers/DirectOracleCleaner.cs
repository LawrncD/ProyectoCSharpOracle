using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;

namespace MiProyectoCSharp.Helpers
{
    /// <summary>
    /// Alternativa A: Si el ConfederationCleaner falla, usa este método
    /// Ejecuta limpieza SQL PURA directamente en Oracle sin pasar datos por C#
    /// </summary>
    public static class DirectOracleCleaner
    {
        public static void CleanConfederationsDirect()
        {
            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("DirectOracleCleaner: Ejecutando limpieza SQL PURA en Oracle");
            Console.WriteLine(new string('=', 60));

            try
            {
                using var conn = DbConnectionHelper.GetConnection();
                var cmd = conn.CreateCommand();

                // 1. Configurar sesión
                Console.WriteLine("\n[1] Configurando sesión...");
                cmd.CommandText = "ALTER SESSION SET NLS_CHARACTERSET='UTF8'";
                cmd.ExecuteNonQuery();

                // 2. Verificar charset
                Console.WriteLine("[2] Verificando charset de base de datos...");
                cmd.CommandText = "SELECT VALUE FROM nls_database_parameters WHERE PARAMETER='NLS_CHARACTERSET'";
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.WriteLine($"    NLS_CHARACTERSET = {reader[0]}");
                    }
                }

                // 3. Leer antes
                Console.WriteLine("\n[3] Confederaciones ANTES:");
                cmd.CommandText = "SELECT ID_CONFEDERACION, NOMBRE, LENGTHB(NOMBRE) as BYTES FROM Confederacion ORDER BY ID_CONFEDERACION";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"    ID={reader[0]}, NOMBRE='{reader[1]}', BYTES={reader[2]}");
                    }
                }

                // 4. LIMPIAR
                Console.WriteLine("\n[4] Eliminando toda data...");
                cmd.CommandText = "DELETE FROM Confederacion";
                int del = cmd.ExecuteNonQuery();
                Console.WriteLine($"    Filas eliminadas: {del}");
                
                cmd.CommandText = "COMMIT";
                cmd.ExecuteNonQuery();
                Console.WriteLine("    COMMIT ejecutado");

                // 5. Insertar LIMPIO - INSERTS SEPARADOS
                Console.WriteLine("\n[5] Insertando confederaciones (ASCII PURO)...");
                
                var inserts = new List<(int, string)>
                {
                    (1, "CONMEBOL"),
                    (2, "CONCACAF"),
                    (3, "UEFA"),
                    (4, "AFC"),
                    (5, "CAF"),
                    (6, "OFC")
                };

                foreach (var (id, name) in inserts)
                {
                    cmd.CommandText = $"INSERT INTO Confederacion (ID_CONFEDERACION, NOMBRE) VALUES ({id}, '{name}')";
                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine($"    Insertado: ID={id}, NOMBRE={name}");
                }

                cmd.CommandText = "COMMIT";
                cmd.ExecuteNonQuery();
                Console.WriteLine("    COMMIT ejecutado");

                // 6. Verificar DESPUÉS
                Console.WriteLine("\n[6] Confederaciones DESPUÉS:");
                cmd.CommandText = "SELECT ID_CONFEDERACION, NOMBRE, LENGTHB(NOMBRE) as BYTES FROM Confederacion ORDER BY ID_CONFEDERACION";
                int countAfter = 0;
                bool hasCorruption = false;
                
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        countAfter++;
                        string nombre = reader[1].ToString();
                        int bytes = (int)reader[2];
                        
                        Console.WriteLine($"    ID={reader[0]}, NOMBRE='{nombre}', BYTES={bytes}");
                        
                        if (nombre.Length > 15 || nombre.Contains("ã") || nombre.Contains("Ã"))
                        {
                            Console.WriteLine($"      ⚠️ POSIBLE CORRUPCIÓN DETECTADA");
                            hasCorruption = true;
                        }
                    }
                }

                if (countAfter != 6)
                {
                    Console.WriteLine($"\n    ⚠️ ADVERTENCIA: Se esperaban 6 confederaciones, pero hay {countAfter}");
                }

                if (hasCorruption)
                {
                    Console.WriteLine("\n    ✗ TODAVÍA HAY CORRUPCIÓN - El problema es a nivel de Oracle");
                }
                else if (countAfter == 6)
                {
                    Console.WriteLine("\n    ✓✓✓ ÉXITO: 6 confederaciones limpias cargadas correctamente");
                }

                conn.Close();
                Console.WriteLine(new string('=', 60) + "\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ ERROR: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}\n");
            }
        }
    }
}
