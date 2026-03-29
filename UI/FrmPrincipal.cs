using System;
using System.Drawing;
using System.Windows.Forms;
using MiProyectoCSharp.Helpers;
using MiProyectoCSharp.Enums;
using MiProyectoCSharp.Data;

namespace MiProyectoCSharp.UI
{
    public class FrmPrincipal : Form
    {
        private MenuStrip menuStrip1;

        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Copa Mundial - Principal";
            this.IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;

            menuStrip1 = new MenuStrip();

            var menuArchivo = new ToolStripMenuItem("Archivo");
            var itemSalir = new ToolStripMenuItem("Salir");
            itemSalir.Click += ItemSalir_Click;
            menuArchivo.DropDownItems.Add(itemSalir);

            var menuGestion = new ToolStripMenuItem("Gestión");
            var itemUsuarios = new ToolStripMenuItem("Usuarios");
            var itemEquipos = new ToolStripMenuItem("Equipos/Jugadores");
            menuGestion.DropDownItems.Add(itemUsuarios);
            menuGestion.DropDownItems.Add(itemEquipos);

            var menuReportes = new ToolStripMenuItem("Consultas y Reportes");
            var itemConsultas = new ToolStripMenuItem("Abrir Consultas");
            itemConsultas.Click += ItemConsultas_Click;
            menuReportes.DropDownItems.Add(itemConsultas);

            var itemReportes = new ToolStripMenuItem("Generar Reportes PDF");
            itemReportes.Click += ItemReportes_Click;
            menuReportes.DropDownItems.Add(itemReportes);

            menuStrip1.Items.Add(menuArchivo);
            menuStrip1.Items.Add(menuGestion);
            menuStrip1.Items.Add(menuReportes);

            this.Controls.Add(menuStrip1);
            this.MainMenuStrip = menuStrip1;
            
            this.Load += FrmPrincipal_Load;
            this.FormClosing += FrmPrincipal_FormClosing;
        }

        private void FrmPrincipal_Load(object? sender, EventArgs e)
        {
            if (!SessionManager.Instance.HaySesionActiva) return;

            var tipo = SessionManager.Instance.UsuarioActivo!.Tipo;
            this.Text = $"Copa Mundial - Usuario: {SessionManager.Instance.UsuarioActivo.NombreUsuario} ({tipo})";

            // Control de Acceso (RBAC)
            var menuGestion = (ToolStripMenuItem)menuStrip1.Items[1];
            if (tipo == TipoUsuario.Esporadico)
            {
                menuGestion.Visible = false;
            }
            else if (tipo == TipoUsuario.Tradicional)
            {
                menuGestion.DropDownItems[0].Visible = false; // Solo Administrador ve Usuarios
            }
        }

        private void ItemConsultas_Click(object? sender, EventArgs e)
        {
            var frm = new FrmConsultas();
            frm.MdiParent = this;
            frm.Show();
        }

        private void ItemReportes_Click(object? sender, EventArgs e)
        {
            var frm = new FrmReportes();
            frm.ShowDialog();
        }

        private void ItemSalir_Click(object? sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmPrincipal_FormClosing(object? sender, FormClosingEventArgs e)
        {
            // Registrar salida en la Bitácora
            if (SessionManager.Instance.HaySesionActiva && SessionManager.Instance.BitacoraActivaId.HasValue)
            {
                var bDao = new BitacoraDAO();
                bDao.RegistrarSalida(SessionManager.Instance.BitacoraActivaId.Value);
            }
        }
    }
}
