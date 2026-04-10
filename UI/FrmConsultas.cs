#nullable disable
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using MiProyectoCSharp.Data;
using MiProyectoCSharp.Helpers;
using Oracle.ManagedDataAccess.Client;

namespace MiProyectoCSharp.UI
{
    public class FrmConsultas : Form
    {
        private ComboBox cmbConsultas;
        private Label lblEstadio;
        private ComboBox cmbEstadios; // Lo usaremos para el punto 2
        private Button btnEjecutar;
        private DataGridView dgvResultados;

        public FrmConsultas()
        {
            InitializeComponent();
            CargarEstadios();
        }

        private void InitializeComponent()
        {
            this.Text = "Consultas de la Copa Mundial"; // Mucho mejor el titulo :)
            this.Size = new Size(850, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(245, 245, 240);

            // Título general
            var lblTitulo = new Label {
                Text = "Seleccione la consulta deseada:", 
                Location = new Point(20, 20), 
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            cmbConsultas = new ComboBox { 
                Location = new Point(250, 18), 
                Width = 350, 
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            // Las 4 consultas obligatorias
            cmbConsultas.Items.Add("1. Jugador más costoso por confederación");  
            cmbConsultas.Items.Add("2. Partidos en un estadio específico");
            cmbConsultas.Items.Add("3. Equipo más costoso por país (MEX, USA, CAN)");
            cmbConsultas.Items.Add("4. Cantidad de jugadores < 21 años por equipo");
            cmbConsultas.SelectedIndex = 0;
            cmbConsultas.SelectedIndexChanged += CmbConsultas_SelectedIndexChanged;

            lblEstadio = new Label { 
                Text = "Estadio:", 
                Location = new Point(20, 55), 
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Visible = false 
            };

            cmbEstadios = new ComboBox { 
                Location = new Point(100, 52), 
                Width = 250, 
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10),
                Visible = false
            };

            btnEjecutar = new Button { 
                Text = "Ejecutar", 
                Location = new Point(620, 16), 
                Width = 100, 
                Height = 30,
                BackColor = Color.FromArgb(100, 95, 85),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
            };
            btnEjecutar.Cursor = Cursors.Hand;
            btnEjecutar.Click += BtnEjecutar_Click;

            dgvResultados = new DataGridView
            {
                Location = new Point(20, 100),
                Size = new Size(790, 440),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,     
                AllowUserToAddRows = false,
                ReadOnly = true,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(250, 250, 245) }
            };
            dgvResultados.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(140, 135, 125);
            dgvResultados.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvResultados.ColumnHeadersHeight = 35;
            dgvResultados.EnableHeadersVisualStyles = false;

            this.Controls.Add(lblTitulo);
            this.Controls.Add(cmbConsultas);
            this.Controls.Add(lblEstadio);
            this.Controls.Add(cmbEstadios);
            this.Controls.Add(btnEjecutar);
            this.Controls.Add(dgvResultados);
        }

        private void CargarEstadios()
        {
            // Pequeña consulta a pulso para llenar el combobox de estadios rápidamente
            try
            {
                var dt = new DataTable();
                using var conexion = DbConnectionHelper.GetConnection();
                string query = "SELECT id_estadio, nombre FROM Estadio ORDER BY nombre";
                using var cmd = new OracleCommand(query, conexion);
                using var adapter = new OracleDataAdapter(cmd);
                adapter.Fill(dt);
                
                cmbEstadios.DisplayMember = "nombre";
                cmbEstadios.ValueMember = "id_estadio";
                cmbEstadios.DataSource = dt;
            }
            catch { /* Ignorar fallos de carga en inicio silenciados */ }
        }

        private void CmbConsultas_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Ocultamos o mostramos el combo de estadios dependiendo de la opcion elegida
            bool esPunto2 = (cmbConsultas.SelectedIndex == 1);
            lblEstadio.Visible = esPunto2;
            cmbEstadios.Visible = esPunto2;
        }

        private void BtnEjecutar_Click(object? sender, EventArgs e)
        {
            try
            {
                int index = cmbConsultas.SelectedIndex;
                switch (index)
                {
                    case 0:
                        var jDao = new JugadorDAO();
                        dgvResultados.DataSource = jDao.ObtenerJugadorMasCostosoPorConfederacion();
                        break;
                    case 1:
                        if (cmbEstadios.SelectedValue != null)
                        {
                            var pDao = new PartidoDAO();
                            int idEstadio = Convert.ToInt32(cmbEstadios.SelectedValue);
                            dgvResultados.DataSource = pDao.ObtenerPartidosPorEstadio(idEstadio);
                        }
                        else
                        {
                            MessageBox.Show("Seleccione un estadio primero.");
                        }
                        break;
                    case 2:
                        var eDao = new EquipoDAO();
                        dgvResultados.DataSource = eDao.ObtenerEquipoMasCostosoPorPais();
                        break;
                    case 3:
                        var jDao2 = new JugadorDAO();
                        dgvResultados.DataSource = jDao2.ObtenerCantidadJugadoresMenoresDe21PorEquipo();
                        break;
                }
                dgvResultados.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ejecutar consulta: " + ex.Message, "Error en BD", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}


