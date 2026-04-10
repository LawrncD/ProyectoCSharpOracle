#nullable disable
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MiProyectoCSharp.Data;
using MiProyectoCSharp.Helpers;

namespace MiProyectoCSharp.UI
{
    public class FrmReportes : Form
    {
        private Button btnReporteTotalConfederacion;
        private Button btnReportePaisesAnfitriones;
        private ComboBox cmbConfederacion; // Para el punto 3

        private ConfederacionDAO confedDAO = new ConfederacionDAO();

        public FrmReportes()
        {
            InitializeComponent();
            CargarConfederaciones();
            
            // Refrescar confederaciones cuando el formulario se muestre
            this.Shown += (s, e) => 
            {
                CargarConfederaciones(); // Asegura que cargue datos FRESCOS desde BD
            };
        }

        private void InitializeComponent()
        {
            this.Text = "Generar Reportes PDF de la Copa Mundial";
            this.Size = new Size(500, 350);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(245, 245, 240);

            var lblConf = new Label { 
                Text = "Confederación:", 
                Location = new Point(50, 43), 
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
            };

            cmbConfederacion = new ComboBox { 
                Location = new Point(170, 40), 
                Width = 260, 
                DropDownStyle = ComboBoxStyle.DropDownList 
            };

            btnReporteTotalConfederacion = new Button
            {
                Text = "Reporte: Valor Total Equipos x Confederación",
                Location = new Point(50, 80),
                Size = new Size(380, 40),
                BackColor = Color.FromArgb(100, 95, 85),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnReporteTotalConfederacion.Click += BtnReporteTotalConfederacion_Click;

            btnReportePaisesAnfitriones = new Button
            {
                Text = "Reporte: Países Visitantes a Anfitriones",
                Location = new Point(50, 140),
                Size = new Size(380, 40),
                BackColor = Color.FromArgb(140, 135, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnReportePaisesAnfitriones.Click += BtnReportePaisesAnfitriones_Click;

            this.Controls.Add(lblConf);
            this.Controls.Add(cmbConfederacion);
            this.Controls.Add(btnReporteTotalConfederacion);
            this.Controls.Add(btnReportePaisesAnfitriones);
        }

        private void CargarConfederaciones()
        {
            try
            {
                var dt = confedDAO.ObtenerTodas();
                cmbConfederacion.DisplayMember = "NOMBRE";
                cmbConfederacion.ValueMember = "ID_CONFEDERACION";
                cmbConfederacion.DataSource = dt;
            }
            catch { /* Ignoramos el error inicial */ }
        }

        private void ExportarAPdf(DataTable dt, string titulo, string nombreArchivo)
        {
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar a PDF en esta consulta.", "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var sfd = new SaveFileDialog();
            sfd.Filter = "PDF Files | *.pdf";
            sfd.FileName = nombreArchivo;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                PdfReportGenerator.GenerarReporteDesdeDataTable(dt, titulo, sfd.FileName, abrirAlTerminar: true);
            }
        }

        private void BtnReportePaisesAnfitriones_Click(object? sender, EventArgs e)
        {
            try {
                var pDao = new PartidoDAO();
                var dt = pDao.ObtenerPaisesVisitantesPorPaisAnfitrion();
                ExportarAPdf(dt, "Países Jugando por País Anfitrión", "Reporte_Anfitriones.pdf");
            } catch (Exception ex) {
                MessageBox.Show("Ocurrió un error al obtener la info: " + ex.Message);
            }
        }

        private void BtnReporteTotalConfederacion_Click(object? sender, EventArgs e)
        {
            try {
                if (cmbConfederacion.SelectedValue == null) 
                {
                    MessageBox.Show("Selecciona una confederación primero.");
                    return;
                }

                int idConf = Convert.ToInt32(cmbConfederacion.SelectedValue);
                // Ahora sí usamos datos reales de la BD, no más mocks jeje
                var dt = confedDAO.ObtenerValorEquiposPorConfederacion(idConf);
                
                string nombreConf = cmbConfederacion.Text;
                ExportarAPdf(dt, $"Valor Total Equipos de la confederación: {nombreConf}", $"Reporte_{nombreConf}.pdf");
            } catch (Exception ex) {
                 MessageBox.Show("Ocurrió un error al obtener la info: " + ex.Message);
            }
        }
    }
}




