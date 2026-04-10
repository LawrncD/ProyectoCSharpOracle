#nullable disable
using System;
using System.Windows.Forms;
using System.Drawing;
using MiProyectoCSharp.Data;
using MiProyectoCSharp.Models;
using MiProyectoCSharp.Enums;

namespace MiProyectoCSharp.UI
{
    public class FrmUsuarios : Form
    {
        private DataGridView dgvUsuarios;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private ComboBox cmbTipo;
        private Button btnAgregar;
        private Button btnEliminar;
        
        private UsuarioDAO usuarioDAO = new UsuarioDAO();

        public FrmUsuarios()
        {
            InitializeComponent();
            CargarUsuarios();
        }

        private void InitializeComponent()
        {
            this.Text = "Gestión de Usuarios - Copa Mundial";
            this.Size = new Size(700, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(243, 244, 246);
            this.Font = new Font("Segoe UI", 9.5f, FontStyle.Regular);

            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(100, 95, 85) };
            var lblTitulo = new Label 
            { 
                Text = "ðŸ‘¥ GESTIá“N DE USUARIOS", 
                ForeColor = Color.White, 
                Font = new Font("Segoe UI", 14, FontStyle.Bold), 
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
                Text = "Crear Nuevo Usuario", 
                Dock = DockStyle.Top, 
                Height = 120, 
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(60, 60, 60)
            };
            pnlBody.Controls.Add(gbForm);

            var regFont = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            
            gbForm.Controls.Add(new Label { Text = "Usuario:", Location = new Point(20, 35), AutoSize = true, Font = regFont });
            txtUsername = new TextBox { Location = new Point(90, 32), Width = 160, Font = regFont };
            gbForm.Controls.Add(txtUsername);

            gbForm.Controls.Add(new Label { Text = "Clave:", Location = new Point(275, 35), AutoSize = true, Font = regFont });
            txtPassword = new TextBox { Location = new Point(330, 32), Width = 150, UseSystemPasswordChar = true, Font = regFont };
            gbForm.Controls.Add(txtPassword);

            gbForm.Controls.Add(new Label { Text = "Rol:", Location = new Point(20, 75), AutoSize = true, Font = regFont });
            cmbTipo = new ComboBox { Location = new Point(90, 72), Width = 160, DropDownStyle = ComboBoxStyle.DropDownList, Font = regFont };
            cmbTipo.DataSource = Enum.GetValues(typeof(TipoUsuario));
            gbForm.Controls.Add(cmbTipo);

            btnAgregar = new Button 
            { 
                Text = "âž• Guardar", 
                Location = new Point(275, 70), 
                Size = new Size(205, 32),
                BackColor = Color.FromArgb(140, 135, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnAgregar.FlatAppearance.BorderSize = 0;
            btnAgregar.Click += BtnAgregar_Click;
            gbForm.Controls.Add(btnAgregar);

            var pnlMid = new Panel { Dock = DockStyle.Top, Height = 55 };
            btnEliminar = new Button 
            { 
                Text = "ðŸ—‘ï¸ Eliminar Usuario Seleccionado", 
                Location = new Point(0, 15), 
                Size = new Size(250, 32),
                BackColor = Color.FromArgb(140, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnEliminar.FlatAppearance.BorderSize = 0;
            btnEliminar.Click += BtnEliminar_Click;
            pnlMid.Controls.Add(btnEliminar);
            pnlBody.Controls.Add(pnlMid);

            dgvUsuarios = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false,
                AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(240, 248, 255) }
            };
            dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(140, 135, 125);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvUsuarios.ColumnHeadersHeight = 35;
            dgvUsuarios.DefaultCellStyle.Padding = new Padding(5);
            dgvUsuarios.RowTemplate.Height = 30;

            pnlBody.Controls.Add(dgvUsuarios);
            pnlBody.Controls.SetChildIndex(gbForm, 2);
            pnlBody.Controls.SetChildIndex(pnlMid, 1);
            pnlBody.Controls.SetChildIndex(dgvUsuarios, 0);
        }

        private void CargarUsuarios()
        {
            try
            {
                dgvUsuarios.DataSource = usuarioDAO.ObtenerTodos();
                if (dgvUsuarios.Columns["ContrasenaHash"] != null)
                    dgvUsuarios.Columns["ContrasenaHash"].Visible = false;
                dgvUsuarios.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAgregar_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Ambos campos (usuario y clave) son obligatorios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nuevo = new Usuario { NombreUsuario = txtUsername.Text.Trim(), ContrasenaHash = txtPassword.Text, Tipo = (TipoUsuario)cmbTipo.SelectedItem! };
            try
            {
                if (usuarioDAO.Insertar(nuevo))
                {
                    MessageBox.Show("Usuario creado correctamente.", "á‰xito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUsername.Clear(); txtPassword.Clear(); CargarUsuarios();
                }
            }
            catch (Exception ex) { MessageBox.Show("Error BD: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void BtnEliminar_Click(object? sender, EventArgs e)
        {
            if (dgvUsuarios.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una fila.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int id = Convert.ToInt32(dgvUsuarios.SelectedRows[0].Cells["IdUsuario"].Value);
            string user = dgvUsuarios.SelectedRows[0].Cells["NombreUsuario"].Value.ToString() ?? "";
            
            if (user.ToLower() == "admin") { MessageBox.Show("No se puede borrar el admin.", "Seguridad", MessageBoxButtons.OK, MessageBoxIcon.Stop); return; }
            if (MessageBox.Show($"Â¿Borrar a '{user}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try { if (usuarioDAO.Eliminar(id)) { MessageBox.Show("Borrado."); CargarUsuarios(); } }
                catch (Exception ex) { MessageBox.Show("No se eliminó (Â¿Tiene registros de bitácora?). " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }
    }
}


