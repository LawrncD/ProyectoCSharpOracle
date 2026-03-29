using System;
using System.Drawing;
using System.Windows.Forms;
using MiProyectoCSharp.Data;

namespace MiProyectoCSharp.UI
{
    public class FrmConsultas : Form
    {
        private ComboBox cmbConsultas;
        private Button btnEjecutar;
        private DataGridView dgvResultados;

        public FrmConsultas()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Consultas y Reportes";
            this.Size = new Size(800, 600);

            Label lbl = new Label() { Text = "Seleccione Consulta:", Location = new Point(20, 20), AutoSize = true };
            
            cmbConsultas = new ComboBox() { Location = new Point(150, 20), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbConsultas.Items.Add("Jugador más costoso por confederación");
            cmbConsultas.Items.Add("Equipo más costoso por país (MEX, USA, CAN)");
            cmbConsultas.Items.Add("Cantidad de jugadores < 21 años por equipo");
            cmbConsultas.Items.Add("Países jugando en cada País Anfitrión");
            cmbConsultas.SelectedIndex = 0;

            btnEjecutar = new Button() { Text = "Ejecutar", Location = new Point(470, 18), Width = 100 };
            btnEjecutar.Click += BtnEjecutar_Click;

            dgvResultados = new DataGridView()
            {
                Location = new Point(20, 60),
                Size = new Size(740, 480),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true
            };

            this.Controls.Add(lbl);
            this.Controls.Add(cmbConsultas);
            this.Controls.Add(btnEjecutar);
            this.Controls.Add(dgvResultados);
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
                        var eDao = new EquipoDAO();
                        dgvResultados.DataSource = eDao.ObtenerEquipoMasCostosoPorPais();
                        break;
                    case 2:
                        var jDao2 = new JugadorDAO();
                        dgvResultados.DataSource = jDao2.ObtenerCantidadJugadoresMenoresDe21PorEquipo();
                        break;
                    case 3:
                        var pDao = new PartidoDAO();
                        dgvResultados.DataSource = pDao.ObtenerPaisesVisitantesPorPaisAnfitrion();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ejecutar consulta: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
