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
                Console.WriteLine($"[PDF] Generando reporte: {rutaDestino}");
                
                // Validar datos
                if (data == null || data.Rows.Count == 0)
                {
                    throw new InvalidOperationException("No hay datos para generar el PDF");
                }

                // Validar ruta
                string directorioDestino = Path.GetDirectoryName(rutaDestino);
                if (string.IsNullOrEmpty(directorioDestino))
                {
                    directorioDestino = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    rutaDestino = Path.Combine(directorioDestino, rutaDestino);
                }

                // Crear directorio si no existe
                if (!Directory.Exists(directorioDestino))
                {
                    Directory.CreateDirectory(directorioDestino);
                    Console.WriteLine($"[PDF] Directorio creado: {directorioDestino}");
                }

                // Eliminar archivo anterior si existe
                if (File.Exists(rutaDestino))
                {
                    try
                    {
                        File.Delete(rutaDestino);
                        System.Threading.Thread.Sleep(100);
                    }
                    catch { }
                }

                // Crear PDF con WriterProperties para mejor stability
                Console.WriteLine($"[PDF] Creando PdfWriter...");
                
                PdfWriter writer = null;
                PdfDocument pdfDoc = null;
                Document document = null;
                
                try
                {
                    // Usar WriterProperties para mejor control
                    var properties = new WriterProperties();
                    writer = new PdfWriter(rutaDestino, properties);
                    Console.WriteLine($"[PDF] PdfWriter creado");
                    
                    pdfDoc = new PdfDocument(writer);
                    Console.WriteLine($"[PDF] PdfDocument creado");
                    
                    document = new Document(pdfDoc);
                    Console.WriteLine($"[PDF] Document creado");

                    // Agregar título
                    var titleFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                    var title = new Paragraph(titulo)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontSize(16)
                        .SetFont(titleFont);
                    document.Add(title);
                    document.Add(new Paragraph("\n")); // Espacio
                    Console.WriteLine($"[PDF] Título agregado");

                    // Crear tabla
                    int numColumnas = data.Columns.Count;
                    Console.WriteLine($"[PDF] Creando tabla con {numColumnas} columnas");
                    
                    Table table = new Table(numColumnas).UseAllAvailableWidth();
                    var headerFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                    // Agregar encabezados
                    foreach (DataColumn column in data.Columns)
                    {
                        string headerText = column.ColumnName ?? "";
                        Paragraph cellParagraph = new Paragraph(headerText).SetFont(headerFont);
                        Cell headerCell = new Cell().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.MAGENTA);
                        headerCell.Add(cellParagraph);
                        table.AddHeaderCell(headerCell);
                    }
                    Console.WriteLine($"[PDF] {numColumnas} encabezados agregados");

                    // Agregar filas de datos
                    var regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                    int rowCount = 0;
                    foreach (DataRow row in data.Rows)
                    {
                        foreach (var item in row.ItemArray)
                        {
                            string cellValue = item?.ToString() ?? "";
                            
                            // Validar encoding
                            try
                            {
                                if (!string.IsNullOrEmpty(cellValue))
                                {
                                    byte[] bytes = Encoding.UTF8.GetBytes(cellValue);
                                    cellValue = Encoding.UTF8.GetString(bytes);
                                }
                            }
                            catch
                            {
                                // Si hay problema con encoding, usar el valor original
                            }

                            Paragraph cellParagraph = new Paragraph(cellValue).SetFont(regularFont);
                            table.AddCell(new Cell().Add(cellParagraph));
                        }
                        rowCount++;
                    }
                    Console.WriteLine($"[PDF] {rowCount} filas agregadas a tabla");

                    // Agregar tabla al documento
                    document.Add(table);
                    Console.WriteLine($"[PDF] Tabla agregada al documento");

                    // Cerrar documento
                    document.Close();
                    pdfDoc.Close();
                    writer.Close();
                    Console.WriteLine($"[PDF] Documento cerrado");
                }
                catch (Exception ex)
                {
                    // Limpiar recursos si hay error
                    document?.Close();
                    pdfDoc?.Close();
                    writer?.Close();
                    
                    throw new Exception($"Error durante creación de PDF: {ex.Message}", ex);
                }

                // Esperar a que se escriba
                System.Threading.Thread.Sleep(500);

                // Validar que se creó
                if (!File.Exists(rutaDestino))
                {
                    throw new FileNotFoundException($"El archivo PDF no se creó en: {rutaDestino}");
                }

                var fileInfo = new FileInfo(rutaDestino);
                if (fileInfo.Length == 0)
                {
                    throw new InvalidOperationException($"El archivo PDF está vacío: {rutaDestino}");
                }

                Console.WriteLine($"[PDF] ✓ PDF generado exitosamente: {rutaDestino} ({fileInfo.Length} bytes)");
                MessageBox.Show($"¡Reporte generado exitosamente!\n\nGuardado en:\n{rutaDestino}", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Intentar abrir
                if (abrirAlTerminar)
                {
                    try
                    {
                        System.Threading.Thread.Sleep(500);
                        var psi = new ProcessStartInfo
                        {
                            FileName = rutaDestino,
                            UseShellExecute = true,
                            ErrorDialog = false
                        };
                        Process.Start(psi);
                        Console.WriteLine($"[PDF] Abriendo PDF automáticamente");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[PDF] No se pudo abrir automáticamente: {ex.Message}");
                        MessageBox.Show($"PDF guardado en:\n{rutaDestino}\n\nNo se pudo abrir automáticamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PDF] ✗ ERROR: {ex.Message}");
                Console.WriteLine($"[PDF] Stack: {ex.StackTrace}");
                MessageBox.Show($"Error al generar PDF:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}



