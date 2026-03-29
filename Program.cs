using System;
using System.Windows.Forms;
using MiProyectoCSharp.UI;

namespace MiProyectoCSharp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            
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
