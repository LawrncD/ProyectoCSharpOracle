using System;
using System.Windows.Forms;
using System.IO;
using MiProyectoCSharp.UI;
using MiProyectoCSharp.Helpers;

namespace MiProyectoCSharp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            
            // REDIRIGIR Console a archivo para ver qué está pasando
            string logFile = "LogsYDebug/STARTUP_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log";
            
            if (!Directory.Exists("LogsYDebug"))
                Directory.CreateDirectory("LogsYDebug");

            using (StreamWriter sw = new StreamWriter(logFile, false))
            {
                sw.WriteLine(new string('*', 80));
                sw.WriteLine("INICIANDO LIMPIEZA DE CONFEDERACIONES");
                sw.WriteLine(new string('*', 80));
                sw.WriteLine($"Tiempo: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                sw.WriteLine();

                try
                {
                    sw.WriteLine("[EJECUTANDO] FinalConfederationFix.FixConfederations()");
                    sw.Flush();
                    
                    FinalConfederationFix.FixConfederations();
                    
                    sw.WriteLine("[OK] FinalConfederationFix completado");
                    sw.Flush();
                }
                catch (Exception ex)
                {
                    sw.WriteLine($"[ERROR] {ex.Message}");
                    sw.WriteLine($"[STACK] {ex.StackTrace}");
                    sw.Flush();
                }

                sw.WriteLine();
                sw.WriteLine("[ESTADO FINAL DE CONFEDERACIONES]");
                try
                {
                    using var conn = DbConnectionHelper.GetConnection();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT ID_CONFEDERACION, NOMBRE FROM Confederacion ORDER BY ID_CONFEDERACION";
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sw.WriteLine($"  ID={reader[0]}, NOMBRE='{reader[1]}', LEN={reader[1].ToString().Length}");
                        }
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    sw.WriteLine($"  [ERROR al leer] {ex.Message}");
                }

                sw.WriteLine();
                sw.WriteLine(new string('*', 80));
                sw.WriteLine("INICIANDO APLICACIÓN");
                sw.WriteLine(new string('*', 80));
                sw.Flush();
            }

            // Iniciar con FrmLogin
            var login = new FrmLogin();
            if (login.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new FrmPrincipal());
            }
            else
            {
                Application.Exit();
            }
        }
    }
}







