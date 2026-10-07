using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Career___Course_Recommendation_System
{
    public partial class InterestForm : Form
    {
        private readonly int userId;

        private CheckedListBox interestList;
        private Label lblSummary;
        private Button btnSave;
        private Button btnBack;

        public InterestForm(int userId)
        {
            this.userId = userId;

            InitializeComponent();
            CreateInterestUI();
            LoadInterests();
            LoadStudentInterests();
        }

        private void CreateInterestUI()
        {
            Text = "CareerPath - Interest Analysis";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(850, 650);
            BackColor = Color.FromArgb(245, 247, 250);

            Controls.Clear();

            Label lblTitle = new Label
            {
                Text = "Interest Analysis",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 65, 120),
                Location = new Point(50, 35),
                AutoSize = true
            };

            Label lblSubtitle = new Label
            {
                Text = "Select the areas you are interested in.",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(80, 95, 115),
                Location = new Point(53, 80),
                AutoSize = true
            };

            interestList = new CheckedListBox
            {
                Location = new Point(55, 125),
                Size = new Size(500, 350),
                Font = new Font("Segoe UI", 11),
                CheckOnClick = true
            };

            lblSummary = new Label
            {
                Text = "Selected Interests (0):",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(25, 65, 120),
                Location = new Point(590, 130),
                Size = new Size(220, 300)
            };

            interestList.ItemCheck += InterestList_ItemCheck;

            btnSave = new Button
            {
                Text = "Save Interests",
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
            Controls.Add(interestList);
            Controls.Add(lblSummary);
            Controls.Add(btnSave);
            Controls.Add(btnBack);
        }

        private void LoadInterests()
        {
            try
            {
                using (SqlConnection connection =
                    DatabaseManager.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT InterestId, InterestName
                        FROM Interests
                        ORDER BY InterestName";

                    using (SqlCommand command =
                        new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader =
                            command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                interestList.Items.Add(
                                    new InterestItem
                                    {
                                        InterestId =
                                            Convert.ToInt32(
                                                reader["InterestId"]),

                                        InterestName =
                                            reader["InterestName"]
                                            .ToString() ?? ""
                                    });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not load interests.\n\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LoadStudentInterests()
        {
            try
            {
                using (SqlConnection connection =
                    DatabaseManager.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT si.InterestId
                        FROM StudentInterests si
                        INNER JOIN Students s
                            ON si.StudentId = s.StudentId
                        WHERE s.UserId = @UserId";

                    using (SqlCommand command =
                        new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@UserId",
                            userId);

                        using (SqlDataReader reader =
                            command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int interestId =
                                    Convert.ToInt32(
                                        reader["InterestId"]);

                                for (int i = 0;
                                     i < interestList.Items.Count;
                                     i++)
                                {
                                    InterestItem item =
                                        (InterestItem)
                                        interestList.Items[i];

                                    if (item.InterestId ==
                                        interestId)
                                    {
                                        interestList.SetItemChecked(
                                            i,
                                            true);
                                    }
                                }
                            }
                        }
                    }
                }

                UpdateSummary();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not load saved interests.\n\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(
            object? sender,
            EventArgs e)
        {
            try
            {
                using (SqlConnection connection =
                    DatabaseManager.GetConnection())
                {
                    connection.Open();

                    using (SqlTransaction transaction =
                        connection.BeginTransaction())
                    {
                        try
                        {
                            int studentId;

                            string studentQuery = @"
                                SELECT StudentId
                                FROM Students
                                WHERE UserId = @UserId";

                            using (SqlCommand command =
                                new SqlCommand(
                                    studentQuery,
                                    connection,
                                    transaction))
                            {
                                command.Parameters.AddWithValue(
                                    "@UserId",
                                    userId);

                                object? result =
                                    command.ExecuteScalar();

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

                                studentId =
                                    Convert.ToInt32(result);
                            }

                            // Remove previous interests
                            string deleteQuery = @"
                                DELETE FROM StudentInterests
                                WHERE StudentId = @StudentId";

                            using (SqlCommand command =
                                new SqlCommand(
                                    deleteQuery,
                                    connection,
                                    transaction))
                            {
                                command.Parameters.AddWithValue(
                                    "@StudentId",
                                    studentId);

                                command.ExecuteNonQuery();
                            }

                            // Save new interests
                            string insertQuery = @"
                                INSERT INTO StudentInterests
                                (StudentId, InterestId)
                                VALUES
                                (@StudentId, @InterestId)";

                            foreach (
                                object checkedItem
                                in interestList.CheckedItems)
                            {
                                InterestItem interest =
                                    (InterestItem)checkedItem;

                                using (SqlCommand command =
                                    new SqlCommand(
                                        insertQuery,
                                        connection,
                                        transaction))
                                {
                                    command.Parameters.AddWithValue(
                                        "@StudentId",
                                        studentId);

                                    command.Parameters.AddWithValue(
                                        "@InterestId",
                                        interest.InterestId);

                                    command.ExecuteNonQuery();
                                }
                            }

                            transaction.Commit();

                            UpdateSummary();

                            MessageBox.Show(
                                "Interests saved successfully!",
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Could not save interests.\n\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void InterestList_ItemCheck(object? sender, ItemCheckEventArgs e)
        {
            if (interestList == null || !interestList.IsHandleCreated)
                return;

            interestList.BeginInvoke(new Action(UpdateSummary));
        }

        private void UpdateSummary()
        {
            if (interestList == null ||
                lblSummary == null)
                return;

            string text = "";
            int count = 0;

            foreach (
                object item
                in interestList.CheckedItems)
            {
                InterestItem interest =
                    (InterestItem)item;

                text += "• " +
                        interest.InterestName +
                        "\n";

                count++;
            }

            lblSummary.Text =
                "Selected Interests (" +
                count +
                "):\n\n" +
                (count == 0
                    ? "No interests selected."
                    : text);
        }

        private void BtnBack_Click(
            object? sender,
            EventArgs e)
        {
            Close();
        }

        private class InterestItem
        {
            public int InterestId { get; set; }

            public string InterestName { get; set; } = "";

            public override string ToString()
            {
                return InterestName;
            }
        }
    }
}