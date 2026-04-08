using System;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using MiProyectoCSharp.Data;
using MiProyectoCSharp.Domain.Entities;

namespace MiProyectoCSharp.UI
{
    public class FrmJugadores : Form
    {
        private DataGridView dgvJugadores;
        private TextBox txtNombre;
        private ComboBox cmbEquipos;
        private DateTimePicker dtpFechaNacimiento;
        private NumericUpDown numPeso;
        private NumericUpDown numEstatura;
        private NumericUpDown numValor;
        private ComboBox cmbPosicion;
        private Button btnAgregar;
        
        private JugadorDAO jugadorDAO = new JugadorDAO();
        private EquipoDAO equipoDAO = new EquipoDAO();

        public FrmJugadores()
        {
            InitializeComponent();
            CargarEquipos();
            CargarJugadores();
        }

        private void InitializeComponent()
        {
            this.Text = "Gesti�n de Jugadores"; 
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            // Beige background
            this.BackColor = Color.FromArgb(245, 245, 240);
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 55, BackColor = Color.FromArgb(140, 135, 125) };
            var lblTitulo = new Label
            {
                Text = "JUGADORES", 
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
                Text = "Registro de Jugador",
                Dock = DockStyle.Top,
                Height = 150,
                Font = new Font("Segoe UI", 10f, FontStyle.Regular),
                ForeColor = Color.FromArgb(60, 60, 60)
            };
            pnlBody.Controls.Add(gbForm);

            // Row 1
            gbForm.Controls.Add(new Label { Text = "Nombre:", Location = new Point(20, 35), AutoSize = true});
            txtNombre = new TextBox { Location = new Point(85, 32), Width = 150, BorderStyle = BorderStyle.FixedSingle };
            gbForm.Controls.Add(txtNombre);

            gbForm.Controls.Add(new Label { Text = "Equipo:", Location = new Point(255, 35), AutoSize = true});
            cmbEquipos = new ComboBox { 
                Location = new Point(310, 32), 
                Width = 150, 
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            gbForm.Controls.Add(cmbEquipos);

            gbForm.Controls.Add(new Label { Text = "Posici�n:", Location = new Point(480, 35), AutoSize = true});
            cmbPosicion = new ComboBox { 
                Location = new Point(545, 32), 
                Width = 120, 
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbPosicion.Items.AddRange(new string[] { "Portero", "Defensa", "Mediocampista", "Delantero" });
            cmbPosicion.SelectedIndex = 3;
            gbForm.Controls.Add(cmbPosicion);

            gbForm.Controls.Add(new Label { Text = "Nacimiento:", Location = new Point(680, 35), AutoSize = true});
            dtpFechaNacimiento = new DateTimePicker { Location = new Point(765, 32), Width = 110, Format = DateTimePickerFormat.Short };
            gbForm.Controls.Add(dtpFechaNacimiento);

            // Row 2
            gbForm.Controls.Add(new Label { Text = "Peso(kg):", Location = new Point(20, 75), AutoSize = true});
            numPeso = new NumericUpDown { Location = new Point(85, 72), Width = 80, Minimum = 40, Maximum = 150, DecimalPlaces = 2, BorderStyle = BorderStyle.FixedSingle, Value = 70.00M }; 
            gbForm.Controls.Add(numPeso);

            gbForm.Controls.Add(new Label { Text = "Altura(m):", Location = new Point(190, 75), AutoSize = true});
            numEstatura = new NumericUpDown { Location = new Point(265, 72), Width = 80, Minimum = 1.40M, Maximum = 2.50M, DecimalPlaces = 2, Increment = 0.01M, BorderStyle = BorderStyle.FixedSingle, Value = 1.75M }; 
            gbForm.Controls.Add(numEstatura);

            gbForm.Controls.Add(new Label { Text = "Valor ($):", Location = new Point(370, 75), AutoSize = true});
            numValor = new NumericUpDown { Location = new Point(440, 72), Width = 110, Minimum = 0, Maximum = 99999999999M, DecimalPlaces = 2, BorderStyle = BorderStyle.FixedSingle }; 
            gbForm.Controls.Add(numValor);

            btnAgregar = new Button
            {
                Text = "REGISTRAR",
                Location = new Point(680, 70),
                Size = new Size(195, 30),
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

            dgvJugadores = new DataGridView
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
            dgvJugadores.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(180, 175, 165);
            dgvJugadores.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(40, 40, 40);   
            dgvJugadores.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            dgvJugadores.ColumnHeadersHeight = 35;
            dgvJugadores.DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 240);
            dgvJugadores.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 195, 185);
            dgvJugadores.DefaultCellStyle.SelectionForeColor = Color.Black;

            pnlBody.Controls.Add(dgvJugadores);
            pnlBody.Controls.SetChildIndex(gbForm, 2);
            pnlBody.Controls.SetChildIndex(pnlSpace, 1);
            pnlBody.Controls.SetChildIndex(dgvJugadores, 0);
        }

        private void CargarEquipos()
        {
            try
            {
                var listaEquipos = equipoDAO.ObtenerTodos();
                var dt = new DataTable();
                dt.Columns.Add("id_equipo", typeof(int));
                dt.Columns.Add("nombre", typeof(string));
                foreach(var eq in listaEquipos) {
                    dt.Rows.Add(eq.IdEquipo, eq.Nombre);
                }

                cmbEquipos.DisplayMember = "NOMBRE"; 
                cmbEquipos.ValueMember = "ID_EQUIPO"; 
                cmbEquipos.DataSource = dt;
            }
            catch {}
        }

        private void CargarJugadores()
        {
            try
            {
                dgvJugadores.DataSource = jugadorDAO.ObtenerTodos();
                dgvJugadores.ClearSelection();
            }
            catch {}
        }

        private void BtnAgregar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("Debe escribir el nombre del jugador.", "Atenci�n", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cmbEquipos.SelectedValue == null) 
            {
                MessageBox.Show("Debe seleccionar un equipo primero. Si no hay equipos, por favor registre uno.", "Atenci�n", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var nuevo = new Jugador
            {
                Nombre = txtNombre.Text.Trim(),
                FechaNacimiento = dtpFechaNacimiento.Value,
                Posicion = cmbPosicion.Text,
                Peso = numPeso.Value,
                Estatura = numEstatura.Value,
                ValorMercado = numValor.Value,
                IdEquipo = Convert.ToInt32(cmbEquipos.SelectedValue)
            };

            try
            {
                if (jugadorDAO.Insertar(nuevo))
                {
                    txtNombre.Clear();
                    numValor.Value = 0;
                    CargarJugadores();
                    // Alerta s�per corporativa amigable
                    var lblConfirm = new Label { Text = "? Guardado con �xito", ForeColor = Color.Green, AutoSize = true, Location = new Point(btnAgregar.Right + 10, btnAgregar.Top + 5) };
                    btnAgregar.Parent.Controls.Add(lblConfirm);
                    var t = new System.Windows.Forms.Timer { Interval = 2000 };
                    t.Tick += (s, ev) => { t.Stop(); lblConfirm.Dispose(); };
                    t.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurri� un error en base de datos:\n" + ex.ToString());
            }
        }
    }
}
