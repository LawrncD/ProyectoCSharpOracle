#nullable disable
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using MiProyectoCSharp.Helpers;
using MiProyectoCSharp.Enums;
using MiProyectoCSharp.Data;

namespace MiProyectoCSharp.UI
{
    public class FrmPrincipal : Form
    {
        private MenuStrip menuStrip1;
        private StatusStrip statusStrip1;
        private ToolStrip toolStrip1;
        private ToolStripStatusLabel lblUsuario;
        private ToolStripStatusLabel lblRol;
        private ToolStripStatusLabel lblReloj;
        private ToolStripStatusLabel lblEstadoDB;
        private System.Windows.Forms.Timer timerReloj;

        // ToolStrip Buttons
        private ToolStripButton btnTSUsuarios;
        private ToolStripButton btnTSEquipos;
        private ToolStripButton btnTSJugadores;
        private ToolStripButton btnTSConsultas;
        private ToolStripButton btnTSReportes;
        private ToolStripButton btnTSSalir;

        public FrmPrincipal()
        {
            InitializeComponent();
            ConfigurarDiseñoCorporativo();
        }

        private void InitializeComponent()
        {
            this.Text = "ERP Sistema de Gestión Mundialista"; 
            this.IsMdiContainer = true;
            this.WindowState = FormWindowState.Maximized;
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            this.BackColor = Color.FromArgb(245, 245, 240); // Todo beige

            // ================== MENU STRIP (Arriba) ==================        
            menuStrip1 = new MenuStrip();
            menuStrip1.BackColor = Color.FromArgb(140, 135, 125); 
            menuStrip1.ForeColor = Color.White;
            menuStrip1.Font = new Font("Segoe UI", 10f, FontStyle.Regular);        
            menuStrip1.Padding = new Padding(5, 8, 5, 8);

            var menuArchivo = new ToolStripMenuItem("SISTEMA");
            var itemSalir = new ToolStripMenuItem("Desconectar y Salir");       
            itemSalir.Click += ItemSalir_Click;
            menuArchivo.DropDownItems.Add(itemSalir);

            var menuGestion = new ToolStripMenuItem("GESTIÓN");
            var itemUsuarios = new ToolStripMenuItem("Módulo de Usuarios");
            itemUsuarios.Click += ItemUsuarios_Click;
            var itemEquipos = new ToolStripMenuItem("Módulo de Equipos");
            itemEquipos.Click += ItemEquipos_Click;
            var itemJugadores = new ToolStripMenuItem("Plantillas y Jugadores");
            itemJugadores.Click += ItemJugadores_Click;
            menuGestion.DropDownItems.Add(itemUsuarios);
            menuGestion.DropDownItems.Add(itemEquipos);
            menuGestion.DropDownItems.Add(itemJugadores);

            var menuReportes = new ToolStripMenuItem("INTELIGENCIA DE NEGOCIO (BI)");
            var itemConsultas = new ToolStripMenuItem("Centro de Consultas");
            itemConsultas.Click += ItemConsultas_Click;
            var itemReportes = new ToolStripMenuItem("Motor de Reportes");
            itemReportes.Click += ItemReportes_Click;
            menuReportes.DropDownItems.Add(itemConsultas);
            menuReportes.DropDownItems.Add(itemReportes);

            menuStrip1.Items.Add(menuArchivo);
            menuStrip1.Items.Add(menuGestion);
            menuStrip1.Items.Add(menuReportes);

            this.Controls.Add(menuStrip1);
            this.MainMenuStrip = menuStrip1;

            // ================== TOOL STRIP (Módulo de Acceso Rápido) ==================
            // Full Beige, No Colors
            toolStrip1 = new ToolStrip();
            toolStrip1.BackColor = Color.FromArgb(230, 225, 215);
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.Padding = new Padding(10, 5, 10, 5);

            btnTSUsuarios = new ToolStripButton("Gestión Usuarios") { Font = new Font("Segoe UI", 9f, FontStyle.Regular), ForeColor = Color.FromArgb(60, 60, 60) };
            btnTSUsuarios.Click += ItemUsuarios_Click;

            btnTSEquipos = new ToolStripButton("Gestión Equipos") { Font = new Font("Segoe UI", 9f, FontStyle.Regular), ForeColor = Color.FromArgb(60, 60, 60) };
            btnTSEquipos.Click += ItemEquipos_Click;

            btnTSJugadores = new ToolStripButton("Gestión Jugadores") { Font = new Font("Segoe UI", 9f, FontStyle.Regular), ForeColor = Color.FromArgb(60, 60, 60) };
            btnTSJugadores.Click += ItemJugadores_Click;

            btnTSConsultas = new ToolStripButton("Consultas") { Font = new Font("Segoe UI", 9f, FontStyle.Regular), ForeColor = Color.FromArgb(60, 60, 60) };
            btnTSConsultas.Click += ItemConsultas_Click;

            btnTSReportes = new ToolStripButton("Reportes PDF") { Font = new Font("Segoe UI", 9f, FontStyle.Regular), ForeColor = Color.FromArgb(60, 60, 60) };
            btnTSReportes.Click += ItemReportes_Click;

            btnTSSalir = new ToolStripButton("Cerrar Sesión") { Font = new Font("Segoe UI", 9f, FontStyle.Regular), ForeColor = Color.FromArgb(80, 80, 80), Alignment = ToolStripItemAlignment.Right };
            btnTSSalir.Click += ItemSalir_Click;

            toolStrip1.Items.Add(btnTSUsuarios);
            toolStrip1.Items.Add(new ToolStripSeparator());
            toolStrip1.Items.Add(btnTSEquipos);
            toolStrip1.Items.Add(new ToolStripSeparator());
            toolStrip1.Items.Add(btnTSJugadores);
            toolStrip1.Items.Add(new ToolStripSeparator());
            toolStrip1.Items.Add(btnTSConsultas);
            toolStrip1.Items.Add(new ToolStripSeparator());
            toolStrip1.Items.Add(btnTSReportes);
            toolStrip1.Items.Add(btnTSSalir);

            this.Controls.Add(toolStrip1);

            // ================== STATUS STRIP (Abajo) ==================
            statusStrip1 = new StatusStrip();
            statusStrip1.BackColor = Color.FromArgb(140, 135, 125); 
            statusStrip1.ForeColor = Color.White;
            statusStrip1.Font = new Font("Segoe UI", 9f);

            lblUsuario = new ToolStripStatusLabel();
            lblRol = new ToolStripStatusLabel();
            lblEstadoDB = new ToolStripStatusLabel("Estado BD: Conectado") { ForeColor = Color.FromArgb(240, 240, 240) };
            lblReloj = new ToolStripStatusLabel() { Spring = true, TextAlign = ContentAlignment.MiddleRight };

            statusStrip1.Items.Add(new ToolStripStatusLabel("Usuario Activo: "));
            statusStrip1.Items.Add(lblUsuario);
            statusStrip1.Items.Add(new ToolStripStatusLabel(" | Rol: "));
            statusStrip1.Items.Add(lblRol);
            statusStrip1.Items.Add(new ToolStripStatusLabel(" | "));
            statusStrip1.Items.Add(lblEstadoDB);
            statusStrip1.Items.Add(lblReloj);

            this.Controls.Add(statusStrip1);

            // Timer para el reloj
            timerReloj = new System.Windows.Forms.Timer { Interval = 1000 };
            timerReloj.Tick += (s, e) => { lblReloj.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"); };
            timerReloj.Start();
        }

        private void ConfigurarDiseñoCorporativo()
        {
            var currentUser = SessionManager.Instance.UsuarioActivo;
            if (currentUser != null)
            {
                lblUsuario.Text = currentUser.NombreUsuario;
                lblRol.Text = currentUser.Tipo.ToString();

                AplicarReglasDeNegocio(currentUser.Tipo);
            }
        }

        private void AplicarReglasDeNegocio(TipoUsuario rol)
        {
            if (rol == TipoUsuario.Tradicional)
            {
                btnTSUsuarios.Visible = false;
                menuStrip1.Items[1].Visible = false; 
            }
            else if (rol == TipoUsuario.Esporadico)
            {
                btnTSUsuarios.Visible = false;
                btnTSEquipos.Visible = false;
                btnTSJugadores.Visible = false;
                menuStrip1.Items[1].Visible = false; 
            }
        }

        private void AbrirFormulario<T>() where T : Form, new()
        {
            var frm = this.MdiChildren.FirstOrDefault(x => x is T);
            if (frm != null)
            {
                frm.BringToFront();
                return;
            }

            frm = new T();
            frm.MdiParent = this;
            frm.Show();
        }

        private void ItemUsuarios_Click(object? sender, EventArgs e) => AbrirFormulario<FrmUsuarios>();
        private void ItemEquipos_Click(object? sender, EventArgs e) => AbrirFormulario<FrmEquipos>();
        private void ItemJugadores_Click(object? sender, EventArgs e) => AbrirFormulario<FrmJugadores>();
        private void ItemConsultas_Click(object? sender, EventArgs e) => AbrirFormulario<FrmConsultas>();
        private void ItemReportes_Click(object? sender, EventArgs e) => AbrirFormulario<FrmReportes>();
        
        private void ItemSalir_Click(object? sender, EventArgs e)
        {
            var bDao = new BitacoraDAO();
            bDao.RegistrarSalida(SessionManager.Instance.BitacoraActivaId ?? 0);
            Application.Exit();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            ItemSalir_Click(this, EventArgs.Empty);
            base.OnFormClosing(e);
        }
    }
}


