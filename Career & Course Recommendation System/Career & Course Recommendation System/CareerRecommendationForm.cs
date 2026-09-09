using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace Career___Course_Recommendation_System
{
    public partial class CareerRecommendationForm : Form
    {

        // CONTROLS
        // --------------------------------

        private DataGridView careerGrid;
        private Label lblResult;
        private Button btnRefresh;
        private Button btnBack;

        // Logged-in user's ID
        private int userId;


        // CONSTRUCTOR
        // --------------------------------

        public CareerRecommendationForm()
        {
            InitializeComponent();

            // Default user
            userId = 0;

            CreateCareerRecommendationUI();
            LoadCareerRecommendations();
        }


        // Constructor with UserId
        public CareerRecommendationForm(int userId)
        {
            InitializeComponent();

            this.userId = userId;

            CreateCareerRecommendationUI();
            LoadCareerRecommendations();
        }


        // CREATE UI
        // --------------------------------

        private void CreateCareerRecommendationUI()
        {
            // FORM
            // ------------------------------

            Text = "CareerPath - Career Recommendation";
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1100, 700);
            MinimumSize = new Size(900, 600);
            BackColor = Color.FromArgb(245, 247, 250);


            // TITLE
            // ------------------------------

            Label lblTitle = new Label();

            lblTitle.Text = "Career Recommendation";
            lblTitle.Font = new Font(
                "Segoe UI",
                24,
                FontStyle.Bold
            );

            lblTitle.ForeColor = Color.FromArgb(30, 45, 80);

            lblTitle.Location = new Point(45, 30);
            lblTitle.AutoSize = true;

            Controls.Add(lblTitle);


            // SUBTITLE
            // ------------------------------

            Label lblSubtitle = new Label();

            lblSubtitle.Text =
                "Explore career options based on your academic profile.";

            lblSubtitle.Font = new Font(
                "Segoe UI",
                11
            );

            lblSubtitle.ForeColor =
                Color.FromArgb(90, 105, 125);

            lblSubtitle.Location =
                new Point(48, 72);

            lblSubtitle.AutoSize = true;

            Controls.Add(lblSubtitle);


            // PROFILE RESULT
            // ------------------------------

            lblResult = new Label();

            lblResult.Text =
                "Checking your academic profile...";

            lblResult.Font = new Font(
                "Segoe UI",
                12,
                FontStyle.Bold
            );

            lblResult.ForeColor =
                Color.FromArgb(40, 90, 160);

            lblResult.Location =
                new Point(70, 145);

            lblResult.AutoSize = true;

            Controls.Add(lblResult);

            // CAREER GRID
            // ------------------------------

            careerGrid = new DataGridView();

            careerGrid.Location =
                new Point(65, 195);

            careerGrid.Size =
                new Size(1000, 350);

            careerGrid.AutoGenerateColumns = true;

            careerGrid.AllowUserToAddRows = false;

            careerGrid.AllowUserToDeleteRows = false;

            careerGrid.ReadOnly = true;

            careerGrid.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            careerGrid.MultiSelect = false;

            careerGrid.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            careerGrid.BackgroundColor = Color.White;

            careerGrid.BorderStyle =
                BorderStyle.FixedSingle;

            careerGrid.ColumnHeadersDefaultCellStyle.Font =
                new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold
                );

            careerGrid.DefaultCellStyle.Font =
                new Font(
                    "Segoe UI",
                    9
                );

            careerGrid.RowTemplate.Height = 30;

            Controls.Add(careerGrid);


            // REFRESH BUTTON
            // ------------------------------

            btnRefresh = new Button();

            btnRefresh.Text =
                "Refresh Recommendations";

            btnRefresh.Location =
                new Point(65, 575);

            btnRefresh.Size =
                new Size(220, 45);

            btnRefresh.BackColor =
                Color.FromArgb(40, 90, 160);

            btnRefresh.ForeColor =
                Color.White;

            btnRefresh.FlatStyle =
                FlatStyle.Flat;

            btnRefresh.FlatAppearance.BorderSize = 0;

            btnRefresh.Font =
                new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold
                );

            btnRefresh.Cursor =
                Cursors.Hand;

            btnRefresh.Click +=
                BtnRefresh_Click;

            Controls.Add(btnRefresh);


            // BACK BUTTON
            // ------------------------------

            btnBack = new Button();

            btnBack.Text =
                "Back to Dashboard";

            btnBack.Location =
                new Point(305, 575);

            btnBack.Size =
                new Size(185, 45);

            btnBack.BackColor =
                Color.White;

            btnBack.ForeColor =
                Color.FromArgb(40, 90, 160);

            btnBack.FlatStyle =
                FlatStyle.Flat;

            btnBack.FlatAppearance.BorderColor =
                Color.FromArgb(40, 90, 160);

            btnBack.Font =
                new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold
                );

            btnBack.Cursor =
                Cursors.Hand;

            btnBack.Click +=
                BtnBack_Click;

            Controls.Add(btnBack);
        }


        // LOAD CAREER RECOMMENDATIONS
        // --------------------------------

        private void LoadCareerRecommendations()
        {
            try
            {
                using (SqlConnection connection =
                       DatabaseManager.GetConnection())
                {
                    connection.Open();


               
                    // STUDENT DATA
                    // ------------------------------

                    double cgpa = 0;

                    int mathematics = 0;

                    int programming = 0;

                    int statistics = 0;


                    string studentQuery;


                    if (userId > 0)
                    {
                        studentQuery = @"
                            SELECT TOP 1
                                CGPA,
                                MathematicsScore,
                                ProgrammingScore,
                                StatisticsScore
                            FROM Students
                            WHERE UserId = @UserId
                            ORDER BY StudentId DESC";
                    }
                    else
                    {
                        studentQuery = @"
                            SELECT TOP 1
                                CGPA,
                                MathematicsScore,
                                ProgrammingScore,
                                StatisticsScore
                            FROM Students
                            ORDER BY StudentId DESC";
                    }


                    using (SqlCommand command =
                           new SqlCommand(
                               studentQuery,
                               connection))
                    {
                        if (userId > 0)
                        {
                            command.Parameters.AddWithValue(
                                "@UserId",
                                userId
                            );
                        }


                        using (SqlDataReader reader =
                               command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cgpa =
                                    reader["CGPA"] == DBNull.Value
                                    ? 0
                                    : Convert.ToDouble(
                                        reader["CGPA"]
                                      );


                                mathematics =
                                    reader["MathematicsScore"]
                                    == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(
                                        reader["MathematicsScore"]
                                      );


                                programming =
                                    reader["ProgrammingScore"]
                                    == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(
                                        reader["ProgrammingScore"]
                                      );


                                statistics =
                                    reader["StatisticsScore"]
                                    == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(
                                        reader["StatisticsScore"]
                                      );
                            }
                        }
                    }


            
                    // SHOW PROFILE
                    // ------------------------------

                    lblResult.Text =
                        $"Your profile: CGPA {cgpa:0.00} | " +
                        $"Programming {programming} | " +
                        $"Mathematics {mathematics} | " +
                        $"Statistics {statistics}";


      
                    // GET CAREERS
                    // ------------------------------

                    string careerQuery = @"
                        SELECT
                            CareerId,
                            CareerName,
                            Description,
                            MinimumCGPA,
                            AverageSalary
                        FROM Careers
                        ORDER BY MinimumCGPA DESC";


                    DataTable table =
                        new DataTable();


                    using (SqlCommand command =
                           new SqlCommand(
                               careerQuery,
                               connection))
                    {
                        using (SqlDataAdapter adapter =
                               new SqlDataAdapter(command))
                        {
                            adapter.Fill(table);
                        }
                    }


               
                    // ADD MATCH SCORE
                    // ------------------------------

                    table.Columns.Add(
                        "Match Score",
                        typeof(int)
                    );


                    table.Columns.Add(
                        "Recommendation",
                        typeof(string)
                    );


                    foreach (DataRow row in table.Rows)
                    {
                        double minimumCgpa =
                            Convert.ToDouble(
                                row["MinimumCGPA"]
                            );


                        string careerName =
                            row["CareerName"]?.ToString()
                            ?? "";


                        int score =
                            CalculateMatchScore(
                                careerName,
                                minimumCgpa,
                                cgpa,
                                mathematics,
                                programming,
                                statistics
                            );


                        row["Match Score"] =
                            score;


                        if (score >= 80)
                        {
                            row["Recommendation"] =
                                "Highly Recommended";
                        }
                        else if (score >= 60)
                        {
                            row["Recommendation"] =
                                "Recommended";
                        }
                        else if (score >= 40)
                        {
                            row["Recommendation"] =
                                "Possible Option";
                        }
                        else
                        {
                            row["Recommendation"] =
                                "Needs Improvement";
                        }
                    }


                  
                    // SORT
                    // ------------------------------

                    table.DefaultView.Sort =
                        "[Match Score] DESC";


                    careerGrid.DataSource =
                        table.DefaultView;


                    // Hide CareerId

                    if (careerGrid.Columns.Contains(
                        "CareerId"))
                    {
                        careerGrid.Columns[
                            "CareerId"
                        ].Visible = false;
                    }


                    // COLUMN NAMES
                    // ------------------------------

                    if (careerGrid.Columns.Contains(
                        "CareerName"))
                    {
                        careerGrid.Columns[
                            "CareerName"
                        ].HeaderText = "Career";
                    }


                    if (careerGrid.Columns.Contains(
                        "Description"))
                    {
                        careerGrid.Columns[
                            "Description"
                        ].HeaderText = "Description";
                    }


                    if (careerGrid.Columns.Contains(
                        "MinimumCGPA"))
                    {
                        careerGrid.Columns[
                            "MinimumCGPA"
                        ].HeaderText =
                            "Minimum CGPA";
                    }


                    if (careerGrid.Columns.Contains(
                        "AverageSalary"))
                    {
                        careerGrid.Columns[
                            "AverageSalary"
                        ].HeaderText =
                            "Average Salary";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load career recommendations.\n\n"
                    + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        // MATCH SCORE
        // --------------------------------

        private int CalculateMatchScore(
            string careerName,
            double minimumCgpa,
            double cgpa,
            int mathematics,
            int programming,
            int statistics)
        {
            int score = 0;


            // CGPA = 40%
            // ------------------------------

            if (cgpa >= minimumCgpa)
            {
                score += 40;
            }
            else if (
                cgpa >= minimumCgpa - 0.25)
            {
                score += 25;
            }
            else if (
                cgpa >= minimumCgpa - 0.50)
            {
                score += 15;
            }


            // SKILLS = 60%
            // ------------------------------

            if (careerName.Contains(
                "Software Engineer",
                StringComparison.OrdinalIgnoreCase))
            {
                score +=
                    (int)(programming * 0.60);
            }

            else if (careerName.Contains(
                "Web Developer",
                StringComparison.OrdinalIgnoreCase))
            {
                score +=
                    (int)(programming * 0.60);
            }

            else if (careerName.Contains(
                "Data Analyst",
                StringComparison.OrdinalIgnoreCase))
            {
                score +=
                    (int)(
                        ((statistics + mathematics)
                        / 2.0) * 0.60
                    );
            }

            else if (careerName.Contains(
                "Database Administrator",
                StringComparison.OrdinalIgnoreCase))
            {
                score +=
                    (int)(
                        ((programming + mathematics)
                        / 2.0) * 0.60
                    );
            }

            else if (careerName.Contains(
                "Cyber Security",
                StringComparison.OrdinalIgnoreCase))
            {
                score +=
                    (int)(
                        ((programming + mathematics)
                        / 2.0) * 0.60
                    );
            }

            else
            {
                score +=
                    (int)(
                        ((programming +
                          mathematics +
                          statistics) / 3.0)
                        * 0.60
                    );
            }


            if (score > 100)
            {
                score = 100;
            }


            return score;
        }


        // REFRESH
        // --------------------------------

        private void BtnRefresh_Click(
            object sender,
            EventArgs e)
        {
            LoadCareerRecommendations();
        }



        // BACK
        // --------------------------------

        private void BtnBack_Click(
            object sender,
            EventArgs e)
        {
            Close();
        }
    }
}