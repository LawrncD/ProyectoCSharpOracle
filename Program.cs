using System;
using System.Windows.Forms;
using System.IO;
using MiProyectoCSharp.UI;
using MiProyectoCSharp.Helpers;

namespace MiProyectoCSharp
{
    internal static class Program
    {
        private static bool ejecutarLimpieza = true;
        private static Form? shellForm;

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            
            // Ejecutar limpieza ANTES de Application.Run()
            EjecutarLimpieza();

            // Form shell INVISIBLE que mantiene Application.Run() activo
            shellForm = new Form 
            { 
                Visible = false,
                ShowInTaskbar = false,
                WindowState = FormWindowState.Minimized
            };

            // Mostrar primera sesión de login
            MostrarLogin();

            // ÚNICO Application.Run() en toda la vida de la aplicación
            Application.Run(shellForm);
        }

        private static void MostrarLogin()
        {
            using (var login = new FrmLogin())
            {
                DialogResult loginResult = login.ShowDialog();

                if (loginResult == DialogResult.OK)
                {
                    // Login OK → Mostrar Principal
                    // Cuando se cierre Principal, vuelve a MostrarLogin() automáticamente
                    MostrarPrincipal();
                }
                else
                {
                    // Login cancelado → Cerrar app
                    shellForm?.Close();
                }
            }
        }

        private static void MostrarPrincipal()
        {
            using (var principal = new FrmPrincipal())
            {
                // ShowDialog es síncrono - espera hasta que se cierre
                DialogResult result = principal.ShowDialog();
                
                // Cuando Principal se cierra, vuelve al login
                // Llamada recursiva (o iterativa si prefieres)
                MostrarLogin();
            }
        }

        private static void EjecutarLimpieza()
        {
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
        }
    }
}







