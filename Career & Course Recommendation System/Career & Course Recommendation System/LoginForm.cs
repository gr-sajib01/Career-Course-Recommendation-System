using Microsoft.Data.SqlClient;
using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Career___Course_Recommendation_System
{
    public partial class LoginForm : Form
    {
        private TextBox txtEmail;
        private TextBox txtPassword;
        private CheckBox chkShowPassword;
        private Button btnLogin;
        private Button btnRegister;

        public LoginForm()
        {
            InitializeComponent();

            BuildLoginForm();
        }

        private void BuildLoginForm()
        {
            // Form settings
            this.Text = "CareerPath - Login";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(1100, 700);
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Remove controls created by the designer
            this.Controls.Clear();

            // Left information panel
            // -----------------------------

            Panel leftPanel = new Panel();
            leftPanel.Size = new Size(450, 620);
            leftPanel.Location = new Point(40, 40);
            leftPanel.BackColor = Color.FromArgb(35, 75, 135);

            this.Controls.Add(leftPanel);

            Label lblLogo = new Label();
            lblLogo.Text = "CAREERPATH";
            lblLogo.Font = new Font("Segoe UI", 28, FontStyle.Bold);
            lblLogo.ForeColor = Color.White;
            lblLogo.AutoSize = true;
            lblLogo.Location = new Point(55, 70);

            leftPanel.Controls.Add(lblLogo);

            Label lblTagline = new Label();
            lblTagline.Text = "Smart Career & Course Recommendation";
            lblTagline.Font = new Font("Segoe UI", 11);
            lblTagline.ForeColor = Color.FromArgb(220, 230, 242);
            lblTagline.AutoSize = true;
            lblTagline.Location = new Point(58, 120);

            leftPanel.Controls.Add(lblTagline);

            Label lblHeading = new Label();
            lblHeading.Text = "Build your career.\r\nShape your future.";
            lblHeading.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            lblHeading.ForeColor = Color.White;
            lblHeading.AutoSize = true;
            lblHeading.Location = new Point(55, 190);

            leftPanel.Controls.Add(lblHeading);

            Label lblDescription = new Label();
            lblDescription.Text =
                "CareerPath helps students discover suitable\r\n" +
                "career opportunities and courses based on\r\n" +
                "their academic performance, interests and skills.";

            lblDescription.Font = new Font("Segoe UI", 11);
            lblDescription.ForeColor = Color.FromArgb(220, 230, 242);
            lblDescription.AutoSize = true;
            lblDescription.Location = new Point(58, 305);

            leftPanel.Controls.Add(lblDescription);

            Label lblFeatures = new Label();
            lblFeatures.Text =
                "✓ Career Recommendations\r\n\r\n" +
                "✓ Course Recommendations\r\n\r\n" +
                "✓ Skills & Interest Analysis";

            lblFeatures.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblFeatures.ForeColor = Color.White;
            lblFeatures.AutoSize = true;
            lblFeatures.Location = new Point(58, 415);

            leftPanel.Controls.Add(lblFeatures);

            // Right login panel
            // -----------------------------

            Panel loginPanel = new Panel();
            loginPanel.Size = new Size(530, 620);
            loginPanel.Location = new Point(510, 40);
            loginPanel.BackColor = Color.White;

            this.Controls.Add(loginPanel);

            Label lblWelcome = new Label();
            lblWelcome.Text = "Welcome Back";
            lblWelcome.Font = new Font("Segoe UI", 26, FontStyle.Bold);
            lblWelcome.ForeColor = Color.FromArgb(35, 45, 60);
            lblWelcome.AutoSize = true;
            lblWelcome.Location = new Point(70, 70);

            loginPanel.Controls.Add(lblWelcome);

            Label lblSubtitle = new Label();
            lblSubtitle.Text = "Sign in to continue to CareerPath";
            lblSubtitle.Font = new Font("Segoe UI", 10);
            lblSubtitle.ForeColor = Color.Gray;
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point(73, 120);

            loginPanel.Controls.Add(lblSubtitle);


            // Email

            Label lblEmail = new Label();
            lblEmail.Text = "Email Address";
            lblEmail.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblEmail.ForeColor = Color.FromArgb(40, 40, 40);
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(73, 175);

            loginPanel.Controls.Add(lblEmail);

            txtEmail = new TextBox();
            txtEmail.Font = new Font("Segoe UI", 12);
            txtEmail.Size = new Size(380, 38);
            txtEmail.Location = new Point(70, 205);
            txtEmail.BorderStyle = BorderStyle.FixedSingle;

            loginPanel.Controls.Add(txtEmail);


            // Password

            Label lblPassword = new Label();
            lblPassword.Text = "Password";
            lblPassword.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblPassword.ForeColor = Color.FromArgb(40, 40, 40);
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(73, 270);

            loginPanel.Controls.Add(lblPassword);

            txtPassword = new TextBox();
            txtPassword.Font = new Font("Segoe UI", 12);
            txtPassword.Size = new Size(380, 38);
            txtPassword.Location = new Point(70, 300);
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.UseSystemPasswordChar = true;

            loginPanel.Controls.Add(txtPassword);


            // Show password

            chkShowPassword = new CheckBox();
            chkShowPassword.Text = "Show password";
            chkShowPassword.Font = new Font("Segoe UI", 9);
            chkShowPassword.ForeColor = Color.FromArgb(70, 70, 70);
            chkShowPassword.AutoSize = true;
            chkShowPassword.Location = new Point(72, 350);

            chkShowPassword.CheckedChanged += ChkShowPassword_CheckedChanged;

            loginPanel.Controls.Add(chkShowPassword);


            // Login button

            btnLogin = new Button();
            btnLogin.Text = "SIGN IN";
            btnLogin.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.BackColor = Color.FromArgb(35, 85, 155);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Size = new Size(380, 48);
            btnLogin.Location = new Point(70, 395);
            btnLogin.Cursor = Cursors.Hand;

            btnLogin.Click += BtnLogin_Click;

            loginPanel.Controls.Add(btnLogin);


            // Register section

            Label lblRegister = new Label();
            lblRegister.Text = "Don't have an account?";
            lblRegister.Font = new Font("Segoe UI", 9);
            lblRegister.ForeColor = Color.Gray;
            lblRegister.AutoSize = true;
            lblRegister.Location = new Point(145, 480);

            loginPanel.Controls.Add(lblRegister);

            btnRegister = new Button();
            btnRegister.Text = "Create an account";
            btnRegister.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnRegister.ForeColor = Color.FromArgb(35, 85, 155);
            btnRegister.BackColor = Color.White;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Size = new Size(190, 35);
            btnRegister.Location = new Point(170, 510);
            btnRegister.Cursor = Cursors.Hand;

            btnRegister.Click += BtnRegister_Click;

            loginPanel.Controls.Add(btnRegister);


            // Footer

            Label lblFooter = new Label();
            lblFooter.Text = "© 2026 CareerPath";
            lblFooter.Font = new Font("Segoe UI", 8);
            lblFooter.ForeColor = Color.Gray;
            lblFooter.AutoSize = true;
            lblFooter.Location = new Point(215, 570);

            loginPanel.Controls.Add(lblFooter);
        }


        // Show / Hide password
        // -----------------------------

        private void ChkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
        }


        // Login
        // -----------------------------

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show(
                    "Please enter your email address.",
                    "CareerPath",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(
                    "Please enter your password.",
                    "CareerPath",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtPassword.Focus();
                return;
            }

            string passwordHash = CreatePasswordHash(password);

            try
            {
                using (SqlConnection connection = DatabaseManager.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT UserId, FullName, Role
                        FROM Users
                        WHERE Email = @Email
                        AND PasswordHash = @PasswordHash";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@PasswordHash", passwordHash);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = Convert.ToInt32(reader["UserId"]);
                                string name = reader["FullName"].ToString();
                                string role = reader["Role"].ToString();

                                MessageBox.Show(
                                    "Welcome, " + name + "!",
                                    "Login Successful",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                                DashboardForm dashboard = new DashboardForm(name, userId);

                                this.Hide();

                                dashboard.FormClosed += (s, args) =>
                                {
                                    this.Close();
                                };

                                dashboard.Show();
                            }
                            else
                            {
                                MessageBox.Show(
                                    "Invalid email or password.",
                                    "Login Failed",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to connect to the database.\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        // Register
        // -----------------------------

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            RegistrationForm registrationForm =
                new RegistrationForm();

            registrationForm.ShowDialog();
        }


        // Password hashing
        // -----------------------------

        private string CreatePasswordHash(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder result = new StringBuilder();

                foreach (byte b in hash)
                {
                    result.Append(b.ToString("x2"));
                }

                return result.ToString();
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}