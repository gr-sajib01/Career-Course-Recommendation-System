using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Career___Course_Recommendation_System
{
    public partial class RegistrationForm : Form
    {
        private TextBox txtFullName = null!;
        private TextBox txtEmail = null!;
        private TextBox txtPassword = null!;
        private TextBox txtStudentId = null!;
        private TextBox txtDepartment = null!;
        private TextBox txtCGPA = null!;
        private TextBox txtMath = null!;
        private TextBox txtProgramming = null!;
        private TextBox txtStatistics = null!;

        private Button btnRegister = null!;
        private Button btnBack = null!;

        public RegistrationForm()
        {
            InitializeComponent();
            CreateRegistrationUI();
        }

        private void CreateRegistrationUI()
        {
            Text = "CareerPath - Student Registration";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(650, 700);
            BackColor = Color.FromArgb(245, 247, 250);

            Label lblTitle = new Label
            {
                Text = "Student Registration",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 45, 80),
                Location = new Point(45, 30),
                AutoSize = true
            };

            Label lblSubtitle = new Label
            {
                Text = "Create your CareerPath student profile",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(90, 105, 125),
                Location = new Point(48, 72),
                AutoSize = true
            };

            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);

            AddLabel("Full Name", 55, 115);
            txtFullName = AddTextBox(200, 110);

            AddLabel("Email", 55, 155);
            txtEmail = AddTextBox(200, 150);

            AddLabel("Password", 55, 195);
            txtPassword = AddTextBox(200, 190);
            txtPassword.UseSystemPasswordChar = true;

            AddLabel("Student ID", 55, 245);
            txtStudentId = AddTextBox(200, 240);

            AddLabel("Department", 55, 285);
            txtDepartment = AddTextBox(200, 280);

            AddLabel("CGPA", 55, 325);
            txtCGPA = AddTextBox(200, 320);

            AddLabel("Mathematics", 55, 365);
            txtMath = AddTextBox(200, 360);

            AddLabel("Programming", 55, 405);
            txtProgramming = AddTextBox(200, 400);

            AddLabel("Statistics", 55, 445);
            txtStatistics = AddTextBox(200, 440);

            btnRegister = new Button
            {
                Text = "Register",
                Location = new Point(200, 500),
                Size = new Size(150, 45),
                BackColor = Color.FromArgb(40, 90, 160),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.Click += BtnRegister_Click;
            Controls.Add(btnRegister);

            btnBack = new Button
            {
                Text = "Back",
                Location = new Point(370, 500),
                Size = new Size(120, 45),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(40, 90, 160),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnBack.FlatAppearance.BorderColor =
                Color.FromArgb(40, 90, 160);

            btnBack.Click += BtnBack_Click;
            Controls.Add(btnBack);
        }

        private void AddLabel(string text, int x, int y)
        {
            Label label = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 50, 70),
                Location = new Point(x, y),
                Size = new Size(130, 25)
            };

            Controls.Add(label);
        }

        private TextBox AddTextBox(int x, int y)
        {
            TextBox textBox = new TextBox
            {
                Location = new Point(x, y),
                Size = new Size(300, 28),
                Font = new Font("Segoe UI", 10)
            };

            Controls.Add(textBox);

            return textBox;
        }

        // SHA-256 password hashing
        private string HashPassword(string password)
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

        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtStudentId.Text) ||
                string.IsNullOrWhiteSpace(txtDepartment.Text) ||
                string.IsNullOrWhiteSpace(txtCGPA.Text) ||
                string.IsNullOrWhiteSpace(txtMath.Text) ||
                string.IsNullOrWhiteSpace(txtProgramming.Text) ||
                string.IsNullOrWhiteSpace(txtStatistics.Text))
            {
                MessageBox.Show(
                    "Please fill in all fields.",
                    "Registration",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!decimal.TryParse(txtCGPA.Text, out decimal cgpa) ||
                cgpa < 0 || cgpa > 4)
            {
                MessageBox.Show(
                    "CGPA must be between 0.00 and 4.00.",
                    "Registration",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (!int.TryParse(txtMath.Text, out int math) ||
                !int.TryParse(txtProgramming.Text, out int programming) ||
                !int.TryParse(txtStatistics.Text, out int statistics) ||
                math < 0 || math > 100 ||
                programming < 0 || programming > 100 ||
                statistics < 0 || statistics > 100)
            {
                MessageBox.Show(
                    "Marks must be between 0 and 100.",
                    "Registration",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            try
            {
                using (SqlConnection connection =
                    DatabaseManager.GetConnection())
                {
                    connection.Open();

                    // Check duplicate email
                    string checkEmailQuery = @"
                        SELECT COUNT(*)
                        FROM Users
                        WHERE Email = @Email";

                    using (SqlCommand checkCommand =
                        new SqlCommand(checkEmailQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue(
                            "@Email", txtEmail.Text.Trim());

                        int count =
                            Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show(
                                "This email is already registered.",
                                "Registration",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }

                    // Check duplicate Student ID
                    string checkStudentIdQuery = @"
                        SELECT COUNT(*)
                        FROM Students
                        WHERE StudentIdNumber = @StudentIdNumber";

                    using (SqlCommand checkStudentCommand =
                        new SqlCommand(checkStudentIdQuery, connection))
                    {
                        checkStudentCommand.Parameters.AddWithValue(
                            "@StudentIdNumber",
                            txtStudentId.Text.Trim());

                        int count =
                            Convert.ToInt32(
                                checkStudentCommand.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show(
                                "This Student ID is already registered.",
                                "Registration",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            return;
                        }
                    }

                    // HASH PASSWORD
                    string passwordHash =
                        HashPassword(txtPassword.Text);

                    // Insert into Users
                    string userQuery = @"
                        INSERT INTO Users
                        (
                            FullName,
                            Email,
                            PasswordHash,
                            [Role]
                        )
                        OUTPUT INSERTED.UserId
                        VALUES
                        (
                            @FullName,
                            @Email,
                            @PasswordHash,
                            'Student'
                        )";

                    int userId;

                    using (SqlCommand userCommand =
                        new SqlCommand(userQuery, connection))
                    {
                        userCommand.Parameters.AddWithValue(
                            "@FullName",
                            txtFullName.Text.Trim());

                        userCommand.Parameters.AddWithValue(
                            "@Email",
                            txtEmail.Text.Trim());

                        userCommand.Parameters.AddWithValue(
                            "@PasswordHash",
                            passwordHash);

                        userId =
                            Convert.ToInt32(
                                userCommand.ExecuteScalar());
                    }

                    // Insert into Students
                    string studentQuery = @"
                        INSERT INTO Students
                        (
                            UserId,
                            StudentIdNumber,
                            Department,
                            CGPA,
                            MathematicsScore,
                            ProgrammingScore,
                            StatisticsScore
                        )
                        VALUES
                        (
                            @UserId,
                            @StudentIdNumber,
                            @Department,
                            @CGPA,
                            @MathematicsScore,
                            @ProgrammingScore,
                            @StatisticsScore
                        )";

                    using (SqlCommand studentCommand =
                        new SqlCommand(studentQuery, connection))
                    {
                        studentCommand.Parameters.AddWithValue(
                            "@UserId", userId);

                        studentCommand.Parameters.AddWithValue(
                            "@StudentIdNumber",
                            txtStudentId.Text.Trim());

                        studentCommand.Parameters.AddWithValue(
                            "@Department",
                            txtDepartment.Text.Trim());

                        studentCommand.Parameters.AddWithValue(
                            "@CGPA", cgpa);

                        studentCommand.Parameters.AddWithValue(
                            "@MathematicsScore", math);

                        studentCommand.Parameters.AddWithValue(
                            "@ProgrammingScore", programming);

                        studentCommand.Parameters.AddWithValue(
                            "@StatisticsScore", statistics);

                        studentCommand.ExecuteNonQuery();
                    }

                    MessageBox.Show(
                        "Registration successful!",
                        "CareerPath",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Registration failed.\n\n" + ex.Message,
                    "Registration Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            txtFullName.Clear();
            txtEmail.Clear();
            txtPassword.Clear();
            txtStudentId.Clear();
            txtDepartment.Clear();
            txtCGPA.Clear();
            txtMath.Clear();
            txtProgramming.Clear();
            txtStatistics.Clear();

            txtFullName.Focus();
        }

        private void BtnBack_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}