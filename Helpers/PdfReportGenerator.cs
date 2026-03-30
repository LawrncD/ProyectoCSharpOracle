using System;
using System.IO;
using System.Data;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;

namespace MiProyectoCSharp.Helpers
{
    /// <summary>
    /// Utilidad encargada de la generación de reportes en formato PDF a partir de datos estructurados.
    /// </summary>
    public class PdfReportGenerator
    {
        /// <summary>
        /// Genera un documento PDF estructurado en base a un conjunto de datos y lo guarda en la ruta especificada.
        /// </summary>
        /// <param name="dt">La tabla de datos (<see cref="DataTable"/>) que contiene la información a reportar.</param>
        /// <param name="titulo">El título principal que se mostrará en el encabezado del documento.</param>
        /// <param name="rutaDestino">La ruta del sistema de archivos local o red donde se guardará el archivo PDF resultante.</param>
        public static void GenerarReporteDesdeDataTable(DataTable dt, string titulo, string rutaDestino)
        {
            try
            {
                using var writer = new PdfWriter(rutaDestino);
                using var pdf = new PdfDocument(writer);
                using var document = new Document(pdf);

                var fontRojo = ColorConstants.DARK_GRAY;
                document.Add(new Paragraph(titulo)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(18)
                    .SetFontColor(fontRojo));
                document.Add(new Paragraph("\n"));

                if (dt.Columns.Count > 0)
                {
                    Table table = new Table(UnitValue.CreatePercentArray(dt.Columns.Count)).UseAllAvailableWidth();

                    foreach (DataColumn column in dt.Columns)
                    {
                        var cell = new Cell().Add(new Paragraph(column.ColumnName.ToUpper()))
                            .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                            .SetTextAlignment(TextAlignment.CENTER);
                        table.AddHeaderCell(cell);
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (var item in row.ItemArray)
                        {
                            table.AddCell(new Cell().Add(new Paragraph(item?.ToString() ?? "")));
                        }
                    }

                    document.Add(table);
                }
                else
                {
                    document.Add(new Paragraph("No hay datos para mostrar en este reporte."));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al generar el PDF: " + ex.Message);
            }
        }
    }
}
