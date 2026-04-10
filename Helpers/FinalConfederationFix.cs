using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;

namespace MiProyectoCSharp.Helpers
{
    /// <summary>
    /// SOLUCIÓN FINAL v2: Reemplaza confederaciones SIN violar Foreign Keys
    /// MEJORADÍSIMO CON VALIDACIÓN EXHAUSTIVA Y MÚLTIPLES INTENTOS
    /// </summary>
    public static class FinalConfederationFix
    {
        public static void FixConfederations()
        {
            Console.WriteLine("\n" + new string('=', 80));
            Console.WriteLine("FinalConfederationFix v2: SOLUCIÓN EXHAUSTIVA SIN FK VIOLATIONS");
            Console.WriteLine(new string('=', 80) + "\n");

            try
            {
                // Mapeo CORRECTO de confederaciones
                var cleanNames = new Dictionary<int, string>
                {
                    { 1, "UEFA" },
                    { 2, "CONMEBOL" },
                    { 3, "CONCACAF" },
                    { 4, "CAF" },
                    { 5, "AFC" },
                    { 6, "OFC" }
                };

                // INTENTO 1: UPDATE directo
                Console.WriteLine("[INTENTO 1] UPDATE directo en sesión nueva...");
                if (TryUpdateWithNewConnection(cleanNames))
                {
                    Console.WriteLine("✓ ÉXITO en Intento 1\n");
                    return;
                }

                Console.WriteLine("✗ Falló Intento 1, probando Intento 2...\n");

                // INTENTO 2: UPDATE con conexión persistente + COMMIT explícito
                Console.WriteLine("[INTENTO 2] UPDATE con COMMIT explícito múltiple...");
                if (TryUpdateWithExplicitCommits(cleanNames))
                {
                    Console.WriteLine("✓ ÉXITO en Intento 2\n");
                    return;
                }

                Console.WriteLine("✗ Falló Intento 2, probando Intento 3...\n");

                // INTENTO 3: UPDATE uno por uno con validación entre cada uno
                Console.WriteLine("[INTENTO 3] UPDATE individual con validación...");
                if (TryUpdateIndividualWithValidation(cleanNames))
                {
                    Console.WriteLine("✓ ÉXITO en Intento 3\n");
                    return;
                }

                Console.WriteLine("✗ Falló Intento 3 - PROBLEMA CRÍTICO EN ORACLE");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ ERROR NO CAPTURADO: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}\n");
            }
        }

        private static bool TryUpdateWithNewConnection(Dictionary<int, string> cleanNames)
        {
            try
            {
                using var conn = DbConnectionHelper.GetConnection();
                var cmd = conn.CreateCommand();

                // Verificar antes
                Console.WriteLine("  [Antes]");
                VerifyConfederations(cmd);

                // UPDATE todos
                Console.WriteLine("  [Actualizando...]");
                foreach (var (id, name) in cleanNames)
                {
                    cmd.CommandText = $"UPDATE Confederacion SET NOMBRE = '{name}' WHERE ID_CONFEDERACION = {id}";
                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine($"    ID={id}: '{name}' → {rows} rows updated");
                }

                // COMMIT
                cmd.CommandText = "COMMIT";
                cmd.ExecuteNonQuery();
                Console.WriteLine("  [COMMIT ejecutado]");

                // Verificar después
                Console.WriteLine("  [Después]");
                bool success = VerifyConfederations(cmd);

                conn.Close();
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ✗ Error: {ex.Message}");
                return false;
            }
        }

        private static bool TryUpdateWithExplicitCommits(Dictionary<int, string> cleanNames)
        {
            try
            {
                using var conn = DbConnectionHelper.GetConnection();
                var cmd = conn.CreateCommand();

                Console.WriteLine("  [Antes]");
                VerifyConfederations(cmd);

                Console.WriteLine("  [Actualizando con COMMIT individual...]");
                foreach (var (id, name) in cleanNames)
                {
                    cmd.CommandText = $"UPDATE Confederacion SET NOMBRE = '{name}' WHERE ID_CONFEDERACION = {id}";
                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine($"    ID={id}: '{name}'");

                    // COMMIT separado para cada uno
                    cmd.CommandText = "COMMIT";
                    cmd.ExecuteNonQuery();
                    Console.WriteLine($"      → COMMIT");
                }

                Console.WriteLine("  [Después]");
                bool success = VerifyConfederations(cmd);

                conn.Close();
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ✗ Error: {ex.Message}");
                return false;
            }
        }

        private static bool TryUpdateIndividualWithValidation(Dictionary<int, string> cleanNames)
        {
            try
            {
                // Abrir UNA sola conexión para todo
                using var conn = DbConnectionHelper.GetConnection();

                Console.WriteLine("  [Antes]");
                var cmd = conn.CreateCommand();
                VerifyConfederations(cmd);

                Console.WriteLine("  [Actualizando y validando cada una...]");

                foreach (var (id, name) in cleanNames)
                {
                    // UPDATE
                    cmd.CommandText = $"UPDATE Confederacion SET NOMBRE = '{name}' WHERE ID_CONFEDERACION = {id}";
                    cmd.ExecuteNonQuery();

                    // COMMIT
                    cmd.CommandText = "COMMIT";
                    cmd.ExecuteNonQuery();

                    // VERIFICAR
                    cmd.CommandText = $"SELECT NOMBRE FROM Confederacion WHERE ID_CONFEDERACION = {id}";
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string actualName = reader[0].ToString();
                            if (actualName == name)
                            {
                                Console.WriteLine($"    ✓ ID={id}: '{actualName}' VERIFICADO");
                            }
                            else
                            {
                                Console.WriteLine($"    ✗ ID={id}: Esperado '{name}' pero obtuve '{actualName}'");
                                return false;
                            }
                        }
                    }
                }

                Console.WriteLine("  [Después]");
                bool success = VerifyConfederations(cmd);

                conn.Close();
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ✗ Error: {ex.Message}");
                return false;
            }
        }

        private static bool VerifyConfederations(OracleCommand cmd)
        {
            cmd.CommandText = "SELECT ID_CONFEDERACION, NOMBRE FROM Confederacion ORDER BY ID_CONFEDERACION";
            bool allClean = true;

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = int.Parse(reader[0].ToString());
                    string nombre = reader[1].ToString();
                    int len = nombre.Length;

                    // Detectar si es limpio
                    bool isClean = (len <= 15) && !nombre.Contains("├") && !nombre.Contains("├") &&
                                  !nombre.Contains("Ã") && !nombre.Contains("ã");

                    if (isClean)
                    {
                        Console.WriteLine($"    ✓ ID={id}: '{nombre}' ({len} chars)");
                    }
                    else
                    {
                        Console.WriteLine($"    ✗ ID={id}: '{nombre}' ({len} chars) - PROBLEMA DETECTADO");
                        allClean = false;
                    }
                }
            }

            return allClean;
        }
    }
}
