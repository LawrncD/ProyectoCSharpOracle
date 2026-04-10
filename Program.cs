using System;
using System.Windows.Forms;
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
            
            // Limpiar encoding corrupto en la base de datos (una sola vez al inicio)
            EncodingFixer.FixAllBrokenEncodings();
            
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







