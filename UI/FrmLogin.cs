using System;
using System.Drawing;
using System.Windows.Forms;
using MiProyectoCSharp.Data;
using MiProyectoCSharp.Helpers;

namespace MiProyectoCSharp.UI
{
    public class FrmLogin : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Label lblError;

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Login - Copa Mundial";
            this.Size = new Size(350, 250);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            Label lblUser = new Label() { Text = "Usuario:", Location = new Point(30, 30), AutoSize = true };
            txtUsername = new TextBox() { Location = new Point(120, 28), Width = 180 };

            Label lblPass = new Label() { Text = "Contraseña:", Location = new Point(30, 70), AutoSize = true };
            txtPassword = new TextBox() { Location = new Point(120, 68), Width = 180, PasswordChar = '*' };

            btnLogin = new Button() { Text = "Ingresar", Location = new Point(120, 110), Width = 100 };
            btnLogin.Click += BtnLogin_Click;

            lblError = new Label() { ForeColor = Color.Red, Location = new Point(30, 150), AutoSize = true, MaximumSize = new Size(280, 0) };

            this.Controls.Add(lblUser);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPass);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(lblError);
            
            this.AcceptButton = btnLogin;
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            try
            {
                var dao = new UsuarioDAO();
                var usuario = dao.ValidarCredenciales(txtUsername.Text, txtPassword.Text);

                if (usuario != null)
                {
                    // Registro en bitácora
                    var bDao = new BitacoraDAO();
                    int bitacoraId = bDao.RegistrarIngreso(usuario.IdUsuario);

                    SessionManager.Instance.IniciarSesion(usuario, bitacoraId);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    lblError.Text = "Credenciales incorrectas.";
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error BD: " + ex.Message;
            }
        }
    }
}
