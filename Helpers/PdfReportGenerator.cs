using System;
using System.IO;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Font;
using iText.IO.Font.Constants;

namespace MiProyectoCSharp.Helpers
{
    public static class PdfReportGenerator
    {
        public static void GenerarReporteDesdeDataTable(DataTable data, string titulo, string rutaDestino, bool abrirAlTerminar = true)
        {
            try
            {
                // Asegurar que el archivo no existe
                if (File.Exists(rutaDestino))
                    File.Delete(rutaDestino);

                using (var writer = new PdfWriter(rutaDestino))
                using (var pdf = new PdfDocument(writer))
                using (var document = new Document(pdf))
                {
                    var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                    document.SetFont(font);
                    
                    var title = new Paragraph(titulo)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(16)
                        .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD));
                    
                    document.Add(title);
                    document.Add(new Paragraph(" "));

                    Table table = new Table(data.Columns.Count).UseAllAvailableWidth();
                    var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                    foreach (DataColumn column in data.Columns)
                    {
                        table.AddHeaderCell(new Cell().Add(new Paragraph(column.ColumnName).SetFont(boldFont)));
                    }

                    foreach (DataRow row in data.Rows)
                    {
                        foreach (var item in row.ItemArray)
                        {
                            string value = item?.ToString() ?? "";
                            if (!string.IsNullOrEmpty(value))
                                value = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(value));
                            table.AddCell(new Cell().Add(new Paragraph(value)));
                        }
                    }

                    document.Add(table);
                }

                System.Threading.Thread.Sleep(500);
                
                if (!File.Exists(rutaDestino))
                    throw new FileNotFoundException($"El archivo PDF no se creó: {rutaDestino}");

                var fileInfo = new FileInfo(rutaDestino);
                if (fileInfo.Length == 0)
                    throw new InvalidOperationException("El archivo PDF está vacío");

                MessageBox.Show("¡Reporte generado exitosamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (abrirAlTerminar)
                {
                    try
                    {
                        var psi = new ProcessStartInfo
                        {
                            FileName = rutaDestino,
                            UseShellExecute = true
                        };
                        Process.Start(psi);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"No se pudo abrir el PDF automáticamente: {ex.Message}\n\nPDF guardado en: {rutaDestino}", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar PDF: {ex.Message}\n\nStack: {ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}



