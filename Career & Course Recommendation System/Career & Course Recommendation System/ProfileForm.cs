using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Career___Course_Recommendation_System
{
    public partial class ProfileForm : Form
    {
        private readonly int userId;

        private TextBox txtName;
        private TextBox txtEmail;
        private TextBox txtStudentId;
        private TextBox txtDepartment;
        private TextBox txtCGPA;
        private TextBox txtMath;
        private TextBox txtProgramming;
        private TextBox txtStatistics;

        private Button btnSave;
        private Button btnBack;

        public ProfileForm(int userId)
        {
            this.userId = userId;

            InitializeComponent();
            CreateProfileUI();
            LoadProfile();
        }

        private void CreateProfileUI()
        {
            Text = "CareerPath - My Profile";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(850, 720);
            BackColor = Color.FromArgb(245, 247, 250);

            Controls.Clear();

            Label title = new Label
            {
                Text = "My Profile",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 65, 120),
                Location = new Point(50, 30),
                AutoSize = true
            };

            Controls.Add(title);

            txtName = AddField("Full Name", 90);
            txtEmail = AddField("Email", 145);
            txtStudentId = AddField("Student ID", 200);
            txtDepartment = AddField("Department", 255);
            txtCGPA = AddField("CGPA", 310);
            txtMath = AddField("Mathematics", 365);
            txtProgramming = AddField("Programming", 420);
            txtStatistics = AddField("Statistics", 475);

            btnSave = new Button
            {
                Text = "Save Changes",
                Location = new Point(220, 570),
                Size = new Size(190, 50),
                BackColor = Color.FromArgb(45, 95, 160),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnBack = new Button
            {
                Text = "Back",
                Location = new Point(430, 570),
                Size = new Size(140, 50),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(45, 95, 160),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnSave.Click += BtnSave_Click;
            btnBack.Click += BtnBack_Click;

            Controls.Add(btnSave);
            Controls.Add(btnBack);
        }

        private TextBox AddField(string labelText, int y)
        {
            Label label = new Label
            {
                Text = labelText,
                Location = new Point(80, y + 8),
                Size = new Size(150, 30),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            TextBox textbox = new TextBox
            {
                Location = new Point(240, y),
                Size = new Size(450, 32),
                Font = new Font("Segoe UI", 10)
            };

            Controls.Add(label);
            Controls.Add(textbox);

            return textbox;
        }

        private void LoadProfile()
        {
            try
            {
                using SqlConnection connection = DatabaseManager.GetConnection();
                connection.Open();

                string query = @"
                    SELECT
                        u.FullName,
                        u.Email,
                        s.StudentIdNumber,
                        s.Department,
                        s.CGPA,
                        s.MathematicsScore,
                        s.ProgrammingScore,
                        s.StatisticsScore
                    FROM Users u
                    LEFT JOIN Students s
                        ON u.UserId = s.UserId
                    WHERE u.UserId = @UserId";

                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                using SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    txtName.Text = reader["FullName"]?.ToString() ?? "";
                    txtEmail.Text = reader["Email"]?.ToString() ?? "";

                    txtStudentId.Text =
                        reader["StudentIdNumber"] == DBNull.Value
                        ? ""
                        : reader["StudentIdNumber"].ToString();

                    txtDepartment.Text =
                        reader["Department"] == DBNull.Value
                        ? ""
                        : reader["Department"].ToString();

                    txtCGPA.Text =
                        reader["CGPA"] == DBNull.Value
                        ? ""
                        : reader["CGPA"].ToString();

                    txtMath.Text =
                        reader["MathematicsScore"] == DBNull.Value
                        ? ""
                        : reader["MathematicsScore"].ToString();

                    txtProgramming.Text =
                        reader["ProgrammingScore"] == DBNull.Value
                        ? ""
                        : reader["ProgrammingScore"].ToString();

                    txtStatistics.Text =
                        reader["StatisticsScore"] == DBNull.Value
                        ? ""
                        : reader["StatisticsScore"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not load profile.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (!double.TryParse(txtCGPA.Text.Trim(), out double cgpa))
            {
                MessageBox.Show("Please enter a valid CGPA.");
                return;
            }

            if (cgpa < 0 || cgpa > 4)
            {
                MessageBox.Show("CGPA must be between 0 and 4.");
                return;
            }

            if (!int.TryParse(txtMath.Text.Trim(), out int math) ||
                !int.TryParse(txtProgramming.Text.Trim(), out int programming) ||
                !int.TryParse(txtStatistics.Text.Trim(), out int statistics))
            {
                MessageBox.Show("Subject scores must be valid numbers.");
                return;
            }

            try
            {
                using SqlConnection connection = DatabaseManager.GetConnection();
                connection.Open();

                using SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    string userQuery = @"
                        UPDATE Users
                        SET FullName = @FullName,
                            Email = @Email
                        WHERE UserId = @UserId";

                    using (SqlCommand command =
                           new SqlCommand(userQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue("@FullName", txtName.Text.Trim());
                        command.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        command.Parameters.AddWithValue("@UserId", userId);

                        command.ExecuteNonQuery();
                    }

                    string studentQuery = @"
                        UPDATE Students
                        SET StudentIdNumber = @StudentIdNumber,
                            Department = @Department,
                            CGPA = @CGPA,
                            MathematicsScore = @Math,
                            ProgrammingScore = @Programming,
                            StatisticsScore = @Statistics
                        WHERE UserId = @UserId";

                    using (SqlCommand command =
                           new SqlCommand(studentQuery, connection, transaction))
                    {
                        command.Parameters.AddWithValue(
                            "@StudentIdNumber",
                            txtStudentId.Text.Trim());

                        command.Parameters.AddWithValue(
                            "@Department",
                            txtDepartment.Text.Trim());

                        command.Parameters.AddWithValue("@CGPA", cgpa);
                        command.Parameters.AddWithValue("@Math", math);
                        command.Parameters.AddWithValue("@Programming", programming);
                        command.Parameters.AddWithValue("@Statistics", statistics);
                        command.Parameters.AddWithValue("@UserId", userId);

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    MessageBox.Show(
                        "Profile updated successfully!",
                        "Success",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not update profile.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnBack_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}