using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Career___Course_Recommendation_System
{
    public partial class SkillsForm : Form
    {
        private readonly int userId;

        private CheckedListBox skillsList;
        private Label lblTitle;
        private Label lblSummary;
        private Button btnSave;
        private Button btnBack;

        public SkillsForm(int userId)
        {
            this.userId = userId;

            InitializeComponent();
            CreateSkillsUI();
            LoadSkills();
            LoadStudentSkills();
        }

        private void CreateSkillsUI()
        {
            Text = "CareerPath - Skills Analysis";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(850, 650);
            BackColor = Color.FromArgb(245, 247, 250);

            Controls.Clear();

            lblTitle = new Label
            {
                Text = "Skills Analysis",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 65, 120),
                Location = new Point(50, 35),
                AutoSize = true
            };

            Label lblSubtitle = new Label
            {
                Text = "Select the skills you currently have.",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(80, 95, 115),
                Location = new Point(53, 80),
                AutoSize = true
            };

            skillsList = new CheckedListBox
            {
                Location = new Point(55, 125),
                Size = new Size(500, 350),
                Font = new Font("Segoe UI", 11),
                CheckOnClick = true,
                BorderStyle = BorderStyle.FixedSingle
            };

            lblSummary = new Label
            {
                Text = "Selected skills: 0",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 65, 120),
                Location = new Point(590, 130),
                Size = new Size(200, 250)
            };

            skillsList.ItemCheck += SkillsList_ItemCheck;

            btnSave = new Button
            {
                Text = "Save Skills",
                Location = new Point(55, 510),
                Size = new Size(190, 50),
                BackColor = Color.FromArgb(45, 95, 160),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnBack = new Button
            {
                Text = "Back",
                Location = new Point(265, 510),
                Size = new Size(150, 50),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(45, 95, 160),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnSave.Click += BtnSave_Click;
            btnBack.Click += BtnBack_Click;

            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);
            Controls.Add(skillsList);
            Controls.Add(lblSummary);
            Controls.Add(btnSave);
            Controls.Add(btnBack);
        }

        private void LoadSkills()
        {
            try
            {
                using SqlConnection connection = DatabaseManager.GetConnection();
                connection.Open();

                string query = @"
                    SELECT SkillId, SkillName
                    FROM Skills
                    ORDER BY SkillName";

                using SqlCommand command = new SqlCommand(query, connection);
                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    skillsList.Items.Add(
                        new SkillItem
                        {
                            SkillId = Convert.ToInt32(reader["SkillId"]),
                            SkillName = reader["SkillName"].ToString()
                        }
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not load skills.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadStudentSkills()
        {
            try
            {
                using SqlConnection connection = DatabaseManager.GetConnection();
                connection.Open();

                string query = @"
                    SELECT ss.SkillId
                    FROM StudentSkills ss
                    INNER JOIN Students s
                        ON ss.StudentId = s.StudentId
                    WHERE s.UserId = @UserId";

                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int selectedSkillId = Convert.ToInt32(reader["SkillId"]);

                    for (int i = 0; i < skillsList.Items.Count; i++)
                    {
                        SkillItem item = (SkillItem)skillsList.Items[i];

                        if (item.SkillId == selectedSkillId)
                        {
                            skillsList.SetItemChecked(i, true);
                        }
                    }
                }

                UpdateSummary();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not load your saved skills.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            try
            {
                using SqlConnection connection = DatabaseManager.GetConnection();
                connection.Open();

                using SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    int studentId;

                    string studentQuery = @"
                        SELECT StudentId
                        FROM Students
                        WHERE UserId = @UserId";

                    using (SqlCommand studentCommand =
                           new SqlCommand(studentQuery, connection, transaction))
                    {
                        studentCommand.Parameters.AddWithValue("@UserId", userId);

                        object result = studentCommand.ExecuteScalar();

                        if (result == null)
                        {
                            MessageBox.Show(
                                "Student profile was not found.",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                            transaction.Rollback();
                            return;
                        }

                        studentId = Convert.ToInt32(result);
                    }

                    string deleteQuery = @"
                        DELETE FROM StudentSkills
                        WHERE StudentId = @StudentId";

                    using (SqlCommand deleteCommand =
                           new SqlCommand(deleteQuery, connection, transaction))
                    {
                        deleteCommand.Parameters.AddWithValue("@StudentId", studentId);
                        deleteCommand.ExecuteNonQuery();
                    }

                    string insertQuery = @"
                        INSERT INTO StudentSkills (StudentId, SkillId, skillLevel)
                        VALUES (@StudentId, @SkillId, 2)";

                    foreach (object checkedItem in skillsList.CheckedItems)
                    {
                        SkillItem skill = (SkillItem)checkedItem;

                        using SqlCommand insertCommand =
                            new SqlCommand(insertQuery, connection, transaction);

                        insertCommand.Parameters.AddWithValue("@StudentId", studentId);
                        insertCommand.Parameters.AddWithValue("@SkillId", skill.SkillId);

                        insertCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();

                    UpdateSummary();

                    MessageBox.Show(
                        "Skills saved successfully!",
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
                    "Could not save skills.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void SkillsList_ItemCheck(object? sender, ItemCheckEventArgs e)
        {
            if (skillsList == null || !skillsList.IsHandleCreated)
                return;

            skillsList.BeginInvoke(new Action(UpdateSummary));
        }

        private void UpdateSummary()
        {
            if (skillsList == null || lblSummary == null)
                return;

            lblSummary.Text = "Selected Skills:\n\n";

            int count = 0;

            foreach (object item in skillsList.CheckedItems)
            {
                SkillItem skill = (SkillItem)item;

                lblSummary.Text += "• " + skill.SkillName + "\n";
                count++;
            }

            if (count == 0)
                lblSummary.Text += "No skills selected.";

            lblSummary.Text =
                "Selected Skills (" + count + "):\n\n" +
                lblSummary.Text.Replace("Selected Skills:\n\n", "");
        }

        private void BtnBack_Click(object? sender, EventArgs e)
        {
            Close();
        }

        private class SkillItem
        {
            public int SkillId { get; set; }
            public string SkillName { get; set; } = "";

            public override string ToString()
            {
                return SkillName;
            }
        }
    }
}