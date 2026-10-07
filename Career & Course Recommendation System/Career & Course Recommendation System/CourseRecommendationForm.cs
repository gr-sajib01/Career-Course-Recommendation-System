using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Career___Course_Recommendation_System
{
    public partial class CourseRecommendationForm : Form
    {
        private DataGridView courseGrid;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblProfile;
        private Button btnRefresh;
        private Button btnBack;

        public CourseRecommendationForm()
        {
            InitializeComponent();
            CreateCourseRecommendationUI();
            LoadCourseRecommendations();
        }

        private void CreateCourseRecommendationUI()
        {
            Text = "CareerPath - Course Recommendation";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1100, 700);
            MinimumSize = new Size(1000, 600);
            BackColor = Color.FromArgb(245, 247, 250);

            // Title
            lblTitle = new Label
            {
                Text = "Course Recommendation",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(20, 45, 80),
                Location = new Point(45, 35),
                AutoSize = true
            };

            // Subtitle
            lblSubtitle = new Label
            {
                Text = "Explore courses based on your academic profile.",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(90, 105, 125),
                Location = new Point(47, 78),
                AutoSize = true
            };

            // Profile
            lblProfile = new Label
            {
                Text = "Loading your profile...",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(35, 90, 160),
                Location = new Point(65, 125),
                AutoSize = true
            };

            // Grid
            courseGrid = new DataGridView
            {
                Location = new Point(55, 170),
                Size = new Size(990, 350),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false
            };

            courseGrid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(35, 85, 150),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter
            };

            courseGrid.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 9),
                SelectionBackColor = Color.FromArgb(35, 125, 200),
                SelectionForeColor = Color.White
            };

            // Refresh button
            btnRefresh = new Button
            {
                Text = "Refresh Recommendations",
                Location = new Point(55, 550),
                Size = new Size(220, 45),
                BackColor = Color.FromArgb(40, 90, 160),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += BtnRefresh_Click;

            // Back button
            btnBack = new Button
            {
                Text = "Back to Dashboard",
                Location = new Point(295, 550),
                Size = new Size(190, 45),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(40, 90, 160),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            btnBack.FlatAppearance.BorderColor = Color.FromArgb(40, 90, 160);
            btnBack.Click += BtnBack_Click;

            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);
            Controls.Add(lblProfile);
            Controls.Add(courseGrid);
            Controls.Add(btnRefresh);
            Controls.Add(btnBack);
        }

        private void LoadCourseRecommendations()
        {
            try
            {
                using (SqlConnection connection = DatabaseManager.GetConnection())
                {
                    connection.Open();

                    // Get latest student's academic profile
                    string studentQuery = @"
                        SELECT TOP 1
                            CGPA,
                            MathematicsScore,
                            ProgrammingScore,
                            StatisticsScore
                        FROM Students
                        ORDER BY StudentId DESC";

                    double cgpa = 0;
                    int mathematics = 0;
                    int programming = 0;
                    int statistics = 0;

                    using (SqlCommand command =
                        new SqlCommand(studentQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cgpa = reader["CGPA"] == DBNull.Value
                                    ? 0
                                    : Convert.ToDouble(reader["CGPA"]);

                                mathematics = reader["MathematicsScore"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(reader["MathematicsScore"]);

                                programming = reader["ProgrammingScore"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(reader["ProgrammingScore"]);

                                statistics = reader["StatisticsScore"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(reader["StatisticsScore"]);
                            }
                        }
                    }

                    lblProfile.Text =
                        $"Your profile: CGPA {cgpa:F2} | Programming {programming} | " +
                        $"Mathematics {mathematics} | Statistics {statistics}";

                    DataTable table = new DataTable();

                    table.Columns.Add("Course");
                    table.Columns.Add("Description");
                    table.Columns.Add("Difficulty");
                    table.Columns.Add("Duration");
                    table.Columns.Add("Match Score");
                    table.Columns.Add("Recommendation");

                    string courseQuery = @"
                        SELECT
                            CourseId,
                            CourseName,
                            Description,
                            DifficultyLevel,
                            Duration
                        FROM Courses
                        ORDER BY CourseId";

                    using (SqlCommand command =
                        new SqlCommand(courseQuery, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string courseName =
                                    reader["CourseName"].ToString() ?? "";

                                string description =
                                    reader["Description"].ToString() ?? "";

                                string difficulty =
                                    reader["DifficultyLevel"].ToString() ?? "";

                                string duration =
                                    reader["Duration"].ToString() ?? "";

                                int score = CalculateCourseScore(
                                    courseName,
                                    difficulty,
                                    cgpa,
                                    mathematics,
                                    programming,
                                    statistics
                                );

                                string recommendation;

                                if (score >= 80)
                                    recommendation = "Highly Recommended";
                                else if (score >= 60)
                                    recommendation = "Recommended";
                                else if (score >= 40)
                                    recommendation = "Possible Option";
                                else
                                    recommendation = "Not Recommended";

                                table.Rows.Add(
                                    courseName,
                                    description,
                                    difficulty,
                                    duration,
                                    score,
                                    recommendation
                                );
                            }
                        }
                    }

                    // Sort by Match Score
                    DataView view = table.DefaultView;
                    view.Sort = "Match Score DESC";

                    courseGrid.DataSource = view;

                    if (courseGrid.Columns.Count >= 6)
                    {
                        courseGrid.Columns[0].HeaderText = "Course";
                        courseGrid.Columns[1].HeaderText = "Description";
                        courseGrid.Columns[2].HeaderText = "Difficulty";
                        courseGrid.Columns[3].HeaderText = "Duration";
                        courseGrid.Columns[4].HeaderText = "Match Score";
                        courseGrid.Columns[5].HeaderText = "Recommendation";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load course recommendations.\n\n" +
                    ex.Message,
                    "Course Recommendation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private int CalculateCourseScore(
            string courseName,
            string difficulty,
            double cgpa,
            int mathematics,
            int programming,
            int statistics)
        {
            // CGPA contribution: maximum 30
            double cgpaScore = Math.Min(cgpa / 4.0, 1.0) * 30;

            int subjectScore = 0;

            string name = courseName.ToLower();

            if (name.Contains("programming") ||
                name.Contains("web"))
            {
                subjectScore =
                    (int)(programming * 0.45 +
                          mathematics * 0.20 +
                          statistics * 0.05);
            }
            else if (name.Contains("database"))
            {
                subjectScore =
                    (int)(programming * 0.30 +
                          mathematics * 0.20 +
                          statistics * 0.10);
            }
            else if (name.Contains("data") ||
                     name.Contains("statistics") ||
                     name.Contains("machine"))
            {
                subjectScore =
                    (int)(statistics * 0.30 +
                          mathematics * 0.30 +
                          programming * 0.10);
            }
            else if (name.Contains("cyber"))
            {
                subjectScore =
                    (int)(programming * 0.30 +
                          mathematics * 0.20 +
                          statistics * 0.10);
            }
            else
            {
                subjectScore =
                    (int)(programming * 0.20 +
                          mathematics * 0.20 +
                          statistics * 0.20);
            }

            double difficultyBonus = 0;

            if (difficulty.Equals("Beginner",
                StringComparison.OrdinalIgnoreCase))
            {
                difficultyBonus = 10;
            }
            else if (difficulty.Equals("Intermediate",
                StringComparison.OrdinalIgnoreCase))
            {
                difficultyBonus = 7;
            }
            else if (difficulty.Equals("Advanced",
                StringComparison.OrdinalIgnoreCase))
            {
                difficultyBonus = cgpa >= 3.0 ? 10 : 2;
            }

            int finalScore =
                (int)Math.Round(cgpaScore + subjectScore + difficultyBonus);

            return Math.Max(0, Math.Min(100, finalScore));
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            LoadCourseRecommendations();
        }

        private void BtnBack_Click(object? sender, EventArgs e)
        {
            Close();
        }
    }
}