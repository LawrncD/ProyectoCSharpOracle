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

        public FrmReportes()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Generar Reportes PDF";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterParent;

            btnReporteTotalConfederacion = new Button()
            {
                Text = "Reporte: Valor Total Equipos x Confederación",
                Location = new Point(50, 50),
                Size = new Size(280, 40)
            };
            btnReporteTotalConfederacion.Click += BtnReporteTotalConfederacion_Click;

            btnReportePaisesAnfitriones = new Button()
            {
                Text = "Reporte: Países Visitantes a Anfitriones",
                Location = new Point(50, 110),
                Size = new Size(280, 40)
            };
            btnReportePaisesAnfitriones.Click += BtnReportePaisesAnfitriones_Click;

            this.Controls.Add(btnReporteTotalConfederacion);
            this.Controls.Add(btnReportePaisesAnfitriones);
        }

        private void ExportarAPdf(DataTable dt, string titulo, string nombreArchivo)
        {
            using var sfd = new SaveFileDialog();
            sfd.Filter = "PDF Files | *.pdf";
            sfd.FileName = nombreArchivo;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    PdfReportGenerator.GenerarReporteDesdeDataTable(dt, titulo, sfd.FileName);
                    MessageBox.Show("Reporte generado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnReportePaisesAnfitriones_Click(object? sender, EventArgs e)
        {
            var pDao = new PartidoDAO();
            var dt = pDao.ObtenerPaisesVisitantesPorPaisAnfitrion();
            ExportarAPdf(dt, "Países Jugando por País Anfitrión", "Reporte_Anfitriones.pdf");
        }

        private void BtnReporteTotalConfederacion_Click(object? sender, EventArgs e)
        {
            // Solo para motivos del ejemplo de UI
            var dt = new DataTable();
            dt.Columns.Add("Equipo");
            dt.Columns.Add("Confederación");
            dt.Columns.Add("Valor_Total");
            dt.Rows.Add("Brasil", "CONMEBOL", "1500000.00");
            dt.Rows.Add("Francia", "UEFA", "2500000.00");
            
            ExportarAPdf(dt, "Valor Total Equipos por Confederación", "Reporte_Confederaciones.pdf");
        }
    }
}
