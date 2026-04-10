using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;

namespace MiProyectoCSharp.Helpers
{
    public static class ConfederationCleaner
    {
        private static List<string> logs = new List<string>();

        public static void CleanAndRecreateConfederations()
        {
            logs.Clear();
            LogMessage("========================================");
            LogMessage("=== ConfederationCleaner v2 START ===");
            LogMessage("========================================");

            try
            {
                using var conn = DbConnectionHelper.GetConnection();
                
                // PASO 1: SET SESSION PROPERTIES
                LogMessage("\n[PASO 1] Configurando sesión Oracle...");
                var cmd = conn.CreateCommand();
                ExecuteCommandSilent(cmd, "ALTER SESSION SET NLS_CHARACTERSET='UTF8'");
                ExecuteCommandSilent(cmd, "ALTER SESSION SET NLS_NCHARSET='UTF8'");
                ExecuteCommandSilent(cmd, "ALTER SESSION SET NLS_LANG='es_ES.UTF8'");
                LogMessage("✓ Sesión configurada a UTF8");

                // PASO 2: LEER DATOS ANTES DE LIMPIAR
                LogMessage("\n[PASO 2] Leyendo confederaciones actuales (ANTES)...");
                int countBefore = ReadAndLogConfederations(cmd, "BEFORE");

                // PASO 3: BORRAR TODOS LOS DATOS
                LogMessage("\n[PASO 3] Borrando confederaciones corruptas...");
                cmd.CommandText = "DELETE FROM Confederacion";
                int deleted = cmd.ExecuteNonQuery();
                LogMessage($"✓ Eliminadas {deleted} filas");
                
                cmd.CommandText = "COMMIT";
                cmd.ExecuteNonQuery();
                LogMessage("✓ COMMIT ejecutado");

                // PASO 4: INSERTAR CONFEDERACIONES LIMPIAS
                LogMessage("\n[PASO 4] Insertando confederaciones limpias (ASCII ONLY)...");
                Dictionary<int, string> confederations = new Dictionary<int, string>
                {
                    { 1, "CONMEBOL" },
                    { 2, "CONCACAF" },
                    { 3, "UEFA" },
                    { 4, "AFC" },
                    { 5, "CAF" },
                    { 6, "OFC" }
                };

                foreach (var conf in confederations)
                {
                    try
                    {
                        // Validar que es ASCII puro
                        byte[] bytes = Encoding.ASCII.GetBytes(conf.Value);
                        string validatedText = Encoding.ASCII.GetString(bytes);

                        cmd.CommandText = $"INSERT INTO Confederacion (ID_CONFEDERACION, NOMBRE) VALUES ({conf.Key}, '{validatedText}')";
                        int rowsInserted = cmd.ExecuteNonQuery();
                        
                        cmd.CommandText = "COMMIT";
                        cmd.ExecuteNonQuery();
                        
                        LogMessage($"✓ Insertado ID={conf.Key}, NOMBRE='{validatedText}'");
                    }
                    catch (Exception ex)
                    {
                        LogMessage($"✗ ERROR insertando {conf.Key}: {ex.Message}");
                    }
                }

                // PASO 5: VERIFICAR DESPUÉS DE LIMPIAR
                LogMessage("\n[PASO 5] Leyendo confederaciones después de limpiar (DESPUÉS)...");
                int countAfter = ReadAndLogConfederations(cmd, "AFTER");

                // PASO 6: VALIDAR INTEGRIDAD
                LogMessage("\n[PASO 6] Validando integridad de datos...");
                ValidateConfederationData(cmd);

                conn.Close();
                
                LogMessage("\n========================================");
                LogMessage("=== ConfederationCleaner END (SUCCESS) ===");
                LogMessage("========================================\n");

                // IMPRIMIR LOGS
                foreach (var log in logs)
                {
                    System.Diagnostics.Debug.WriteLine(log);
                    Console.WriteLine(log);
                }

                // GUARDAR LOGS EN ARCHIVO
                if (!System.IO.Directory.Exists("LogsYDebug"))
                    System.IO.Directory.CreateDirectory("LogsYDebug");
                
                string logFile = System.IO.Path.Combine("LogsYDebug", "ConfederationCleaner_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");
                System.IO.File.WriteAllLines(logFile, logs);
                LogMessage($"✓ Logs guardados en: {logFile}");
            }
            catch (Exception ex)
            {
                LogMessage($"\n✗ ERROR CRÍTICO: {ex.Message}");
                LogMessage($"StackTrace: {ex.StackTrace}");
                
                foreach (var log in logs)
                {
                    System.Diagnostics.Debug.WriteLine(log);
                    Console.WriteLine(log);
                }
            }
        }

        private static int ReadAndLogConfederations(OracleCommand cmd, string label)
        {
            cmd.CommandText = "SELECT ID_CONFEDERACION, NOMBRE FROM Confederacion ORDER BY ID_CONFEDERACION";
            int count = 0;
            
            using (var reader = cmd.ExecuteReader())
            {
                LogMessage($"\n  [{label}] Confederaciones en BD:");
                while (reader.Read())
                {
                    var id = reader.GetValue(0);
                    var nombre = reader.GetValue(1);
                    string nombreStr = nombre?.ToString() ?? "[NULL]";
                    int bytes = nombre != null ? Encoding.UTF8.GetByteCount(nombreStr) : 0;
                    
                    count++;
                    LogMessage($"    → ID={id}, NOMBRE='{nombreStr}', BytesUTF8={bytes}, Len={nombreStr.Length}");
                    
                    // Detectar corrupción
                    if (nombreStr.Contains("ã") || nombreStr.Contains("Ã") || 
                        nombreStr.Contains("é") || nombreStr.Contains("É") ||
                        nombreStr.Contains("ó") || nombreStr.Contains("Ó") ||
                        nombreStr.Contains("ñ") || nombreStr.Contains("Ñ"))
                    {
                        LogMessage($"      ⚠️ ADVERTENCIA: Posible corrupción detectada");
                    }
                }
            }
            
            LogMessage($"  [{label}] Total: {count} filas");
            return count;
        }

        private static void ValidateConfederationData(OracleCommand cmd)
        {
            string[] expectedConfeds = { "CONMEBOL", "CONCACAF", "UEFA", "AFC", "CAF", "OFC" };
            cmd.CommandText = "SELECT NOMBRE FROM Confederacion ORDER BY ID_CONFEDERACION";

            var actualConfeds = new List<string>();
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    actualConfeds.Add(reader[0].ToString());
                }
            }

            LogMessage("\n  Validación:");
            bool allClean = true;
            for (int i = 0; i < expectedConfeds.Length && i < actualConfeds.Count; i++)
            {
                bool match = expectedConfeds[i] == actualConfeds[i];
                if (match)
                    LogMessage($"    ✓ {expectedConfeds[i]} == {actualConfeds[i]}");
                else
                {
                    LogMessage($"    ✗ MISMATCH: Esperado '{expectedConfeds[i]}' pero obtuve '{actualConfeds[i]}'");
                    allClean = false;
                }
            }

            if (allClean)
                LogMessage("\n  ✓✓✓ TODAS LAS CONFEDERACIONES SON LIMPIAS ✓✓✓");
            else
                LogMessage("\n  ✗✗✗ TODAVÍA HAY CORRUPCIÓN DETECTADA ✗✗✗");
        }

        private static void ExecuteCommandSilent(OracleCommand cmd, string sql)
        {
            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch { }
        }

        private static void LogMessage(string msg)
        {
            logs.Add($"[{DateTime.Now:HH:mm:ss.fff}] {msg}");
        }
    }
}
