using System;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using MiProyectoCSharp.Data;
using MiProyectoCSharp.Domain.Entities;

namespace MiProyectoCSharp.UI
{
    public class FrmEquipos : Form
    {
        private DataGridView dgvEquipos;
        private TextBox txtNombre;
        private TextBox txtPais;
        private ComboBox cmbConfederacion;
        private NumericUpDown numValor;
        private Button btnAgregar;
        
        private EquipoDAO equipoDAO = new EquipoDAO();
        private ConfederacionDAO confedDAO = new ConfederacionDAO();

        public FrmEquipos()
        {
            InitializeComponent();
            CargarConfederaciones();
            CargarEquipos();
        }

        private void InitializeComponent()
        {
            this.Text = "Gesti�n de Selecciones"; 
            this.Size = new Size(850, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            // Beige background
            this.BackColor = Color.FromArgb(245, 245, 240);
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = Color.FromArgb(140, 135, 125) };
            var lblTitulo = new Label
            {
                Text = "EQUIPOS", 
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semilight", 14),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0)
            };
            pnlHeader.Controls.Add(lblTitulo);
            this.Controls.Add(pnlHeader);

            var pnlBody = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };
            this.Controls.Add(pnlBody);

            var gbForm = new GroupBox
            {
                Text = "Registro",
                Dock = DockStyle.Top,
                Height = 120,
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                ForeColor = Color.FromArgb(60, 60, 60)
            };
            pnlBody.Controls.Add(gbForm);

            gbForm.Controls.Add(new Label { Text = "Nombre:", Location = new Point(20, 35), AutoSize = true});
            txtNombre = new TextBox { Location = new Point(90, 32), Width = 150, BorderStyle = BorderStyle.FixedSingle };
            gbForm.Controls.Add(txtNombre);

            gbForm.Controls.Add(new Label { Text = "Pa�s:", Location = new Point(265, 35), AutoSize = true});
            txtPais = new TextBox { Location = new Point(310, 32), Width = 150, BorderStyle = BorderStyle.FixedSingle };
            gbForm.Controls.Add(txtPais);

            gbForm.Controls.Add(new Label { Text = "Confederaci�n:", Location = new Point(10, 75), AutoSize = true});
            cmbConfederacion = new ComboBox { 
                Location = new Point(120, 72), 
                Width = 140, 
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            gbForm.Controls.Add(cmbConfederacion);

            gbForm.Controls.Add(new Label { Text = "Valor:", Location = new Point(280, 75), AutoSize = true});
            numValor = new NumericUpDown { Location = new Point(330, 72), Width = 130, Minimum = 0, Maximum = 99999999999M, DecimalPlaces = 2, BorderStyle = BorderStyle.FixedSingle }; 
            gbForm.Controls.Add(numValor);

            btnAgregar = new Button
            {
                Text = "REGISTRAR",
                Location = new Point(490, 70),
                Size = new Size(180, 30),
                BackColor = Color.FromArgb(100, 95, 85),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAgregar.FlatAppearance.BorderSize = 0;
            btnAgregar.Click += BtnAgregar_Click;
            gbForm.Controls.Add(btnAgregar);

            var pnlSpace = new Panel { Dock = DockStyle.Top, Height = 15 };     
            pnlBody.Controls.Add(pnlSpace);

            dgvEquipos = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,        
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,     
                BackgroundColor = Color.FromArgb(250, 250, 245),
                BorderStyle = BorderStyle.FixedSingle,
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false,
                GridColor = Color.FromArgb(210, 210, 200),
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.White }
            };
            dgvEquipos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(180, 175, 165);
            dgvEquipos.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(40, 40, 40);   
            dgvEquipos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            dgvEquipos.ColumnHeadersHeight = 35;
            dgvEquipos.DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 240);
            dgvEquipos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 195, 185);
            dgvEquipos.DefaultCellStyle.SelectionForeColor = Color.Black;

            pnlBody.Controls.Add(dgvEquipos);
            pnlBody.Controls.SetChildIndex(gbForm, 2);
            pnlBody.Controls.SetChildIndex(pnlSpace, 1);
            pnlBody.Controls.SetChildIndex(dgvEquipos, 0);
        }

        private void CargarConfederaciones()
        {
            try
            {

                
                DataTable dt = confedDAO.ObtenerTodas();
                cmbConfederacion.DisplayMember = "NOMBRE"; 
                cmbConfederacion.ValueMember = "ID_CONFEDERACION"; 
                cmbConfederacion.DataSource = dt;
            }
            catch {}
        }

        private void CargarEquipos()
        {
            try
            {
                dgvEquipos.DataSource = equipoDAO.ObtenerTodos();
                dgvEquipos.ClearSelection();
            }
            catch {}
        }

        private void BtnAgregar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtPais.Text))
            {
                MessageBox.Show("Debe escribir el nombre y el pa�s.", "Atenci�n", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cmbConfederacion.SelectedValue == null) return;

            var nuevo = new Equipo
            {
                Nombre = txtNombre.Text.Trim(),
                Pais = txtPais.Text.Trim(),
                IdConfederacion = Convert.ToInt32(cmbConfederacion.SelectedValue),
                ValorTotalEquipo = numValor.Value
            };

            try
            {
                if (equipoDAO.Insertar(nuevo))
                {
                    txtNombre.Clear();
                    txtPais.Clear();
                    if(cmbConfederacion.Items.Count > 0) cmbConfederacion.SelectedIndex = 0;
                    numValor.Value = 0;
                    CargarEquipos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo registrar el equipo:\n" + ex.ToString());
            }
        }
    }
}
