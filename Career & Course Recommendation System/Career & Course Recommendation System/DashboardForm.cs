using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Career___Course_Recommendation_System
{
    public partial class DashboardForm : Form
    {
        private readonly string userName;
        private readonly int userId;

        private Panel sidebar;
        private Panel contentPanel;

        private Label lblWelcome;
        private Label lblStudentInfo;

        private Label lblCareerCount;
        private Label lblCourseCount;
        private Label lblSkillCount;
        private Label lblInterestCount;

        private Button btnDashboard;
        private Button btnCareer;
        private Button btnCourse;
        private Button btnSkills;
        private Button btnInterest;
        private Button btnProfile;
        private Button btnLogout;

        public DashboardForm(string userName, int userId)
        {
            this.userName = userName;
            this.userId = userId;

            CreateDashboard();
            LoadDashboardData();
        }

        // CREATE DASHBOARD
        // =========================================================

        private void CreateDashboard()
        {

            // FORM
            // -----------------------------------------------------

            Text = "CareerPath - Dashboard";
            StartPosition = FormStartPosition.CenterScreen;

            Size = new Size(1200, 750);
            MinimumSize = new Size(1000, 650);

            BackColor = Color.FromArgb(245, 247, 250);

            Font = new Font(
                "Segoe UI",
                10F
            );

            // CONTENT PANEL
            // -----------------------------------------------------

            contentPanel = new Panel();

            contentPanel.Left = 240;
            contentPanel.Top = 0;

            contentPanel.Width =
                ClientSize.Width - 240;

            contentPanel.Height =
                ClientSize.Height;

            contentPanel.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left |
                AnchorStyles.Right;

            contentPanel.BackColor =
                Color.FromArgb(245, 247, 250);

            Controls.Add(contentPanel);

            // SIDEBAR
            // -----------------------------------------------------

            sidebar = new Panel();

            sidebar.Left = 0;
            sidebar.Top = 0;

            sidebar.Width = 240;
            sidebar.Height = ClientSize.Height;

            sidebar.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Bottom |
                AnchorStyles.Left;

            sidebar.BackColor =
                Color.FromArgb(37, 78, 139);

            Controls.Add(sidebar);


            // SIDEBAR LOGO
            // -----------------------------------------------------

            Label lblLogo = new Label();

            lblLogo.Text = "CAREERPATH";

            lblLogo.ForeColor = Color.White;

            lblLogo.Font = new Font(
                "Segoe UI",
                20F,
                FontStyle.Bold
            );

            lblLogo.AutoSize = true;

            lblLogo.Location =
                new Point(30, 35);

            sidebar.Controls.Add(lblLogo);

            // SIDEBAR SUBTITLE
            // -----------------------------------------------------

            Label lblLogoSubtitle = new Label();

            lblLogoSubtitle.Text =
                "Smart Career & Course";

            lblLogoSubtitle.ForeColor =
                Color.FromArgb(220, 230, 245);

            lblLogoSubtitle.Font =
                new Font("Segoe UI", 9F);

            lblLogoSubtitle.AutoSize = true;

            lblLogoSubtitle.Location =
                new Point(30, 70);

            sidebar.Controls.Add(lblLogoSubtitle);

            // SIDEBAR BUTTONS
            // -----------------------------------------------------

            btnDashboard =
                CreateSidebarButton(
                    "Dashboard",
                    125
                );

            btnCareer =
                CreateSidebarButton(
                    "Career Recommendation",
                    180
                );

            btnCourse =
                CreateSidebarButton(
                    "Course Recommendation",
                    235
                );

            btnSkills =
                CreateSidebarButton(
                    "Skills Analysis",
                    290
                );

            btnInterest =
                CreateSidebarButton(
                    "Interest Analysis",
                    345
                );

            btnProfile =
                CreateSidebarButton(
                    "My Profile",
                    400
                );

            btnLogout =
                CreateSidebarButton(
                    "Logout",
                    480
                );

            sidebar.Controls.Add(btnDashboard);
            sidebar.Controls.Add(btnCareer);
            sidebar.Controls.Add(btnCourse);
            sidebar.Controls.Add(btnSkills);
            sidebar.Controls.Add(btnInterest);
            sidebar.Controls.Add(btnProfile);
            sidebar.Controls.Add(btnLogout);

            // BUTTON EVENTS
            // -----------------------------------------------------

            btnDashboard.Click += DashboardButton_Click;
            btnCareer.Click += CareerButton_Click;
            btnCourse.Click += CourseButton_Click;
            btnSkills.Click += SkillsButton_Click;
            btnInterest.Click += InterestButton_Click;
            btnProfile.Click += ProfileButton_Click;
            btnLogout.Click += LogoutButton_Click;

            // HEADER
            // -----------------------------------------------------

            Panel headerPanel = new Panel();

            headerPanel.Left = 0;
            headerPanel.Top = 0;

            headerPanel.Width =
                contentPanel.ClientSize.Width;

            headerPanel.Height = 100;

            headerPanel.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;

            headerPanel.BackColor = Color.White;

            contentPanel.Controls.Add(headerPanel);

            // WELCOME
            // -----------------------------------------------------

            lblWelcome = new Label();

            lblWelcome.Text =
                "Welcome back, " + userName + "!";

            lblWelcome.Font =
                new Font(
                    "Segoe UI",
                    22F,
                    FontStyle.Bold
                );

            lblWelcome.ForeColor =
                Color.FromArgb(30, 45, 65);

            lblWelcome.AutoSize = true;

            lblWelcome.Location =
                new Point(35, 25);

            headerPanel.Controls.Add(lblWelcome);


            // HEADER DESCRIPTION
            // -----------------------------------------------------

            lblStudentInfo = new Label();

            lblStudentInfo.Text =
                "Continue building your career journey.";

            lblStudentInfo.Font =
                new Font(
                    "Segoe UI",
                    10F
                );

            lblStudentInfo.ForeColor =
                Color.FromArgb(110, 120, 135);

            lblStudentInfo.AutoSize = true;

            lblStudentInfo.Location =
                new Point(38, 62);

            headerPanel.Controls.Add(lblStudentInfo);

            // DASHBOARD TITLE
            // -----------------------------------------------------

            Label lblDashboardTitle = new Label();

            lblDashboardTitle.Text =
                "Dashboard";

            lblDashboardTitle.Font =
                new Font(
                    "Segoe UI",
                    18F,
                    FontStyle.Bold
                );

            lblDashboardTitle.ForeColor =
                Color.FromArgb(35, 45, 60);

            lblDashboardTitle.AutoSize = true;

            lblDashboardTitle.Location =
                new Point(35, 125);

            contentPanel.Controls.Add(
                lblDashboardTitle
            );

 
            // DASHBOARD DESCRIPTION
            // -----------------------------------------------------

            Label lblDescription = new Label();

            lblDescription.Text =
                "Explore careers, courses, skills and interests.";

            lblDescription.Font =
                new Font(
                    "Segoe UI",
                    10F
                );

            lblDescription.ForeColor =
                Color.FromArgb(110, 120, 135);

            lblDescription.AutoSize = true;

            lblDescription.Location =
                new Point(37, 155);

            contentPanel.Controls.Add(
                lblDescription
            );

            // STATISTICS CARDS
            // -----------------------------------------------------

            Panel careerCard =
                CreateStatCard(
                    "Career Options",
                    "0",
                    Color.FromArgb(37, 78, 139),
                    new Point(35, 195)
                );

            Panel courseCard =
                CreateStatCard(
                    "Available Courses",
                    "0",
                    Color.FromArgb(46, 125, 100),
                    new Point(265, 195)
                );

            Panel skillCard =
                CreateStatCard(
                    "Skills",
                    "0",
                    Color.FromArgb(112, 78, 150),
                    new Point(495, 195)
                );

            Panel interestCard =
                CreateStatCard(
                    "Interests",
                    "0",
                    Color.FromArgb(190, 120, 50),
                    new Point(725, 195)
                );

            contentPanel.Controls.Add(careerCard);
            contentPanel.Controls.Add(courseCard);
            contentPanel.Controls.Add(skillCard);
            contentPanel.Controls.Add(interestCard);

            // Save the number labels
            lblCareerCount =
                (Label)careerCard.Tag;

            lblCourseCount =
                (Label)courseCard.Tag;

            lblSkillCount =
                (Label)skillCard.Tag;

            lblInterestCount =
                (Label)interestCard.Tag;

            // QUICK ACTION TITLE
            // -----------------------------------------------------

            Label lblQuickActions = new Label();

            lblQuickActions.Text =
                "Quick Actions";

            lblQuickActions.Font =
                new Font(
                    "Segoe UI",
                    17F,
                    FontStyle.Bold
                );

            lblQuickActions.ForeColor =
                Color.FromArgb(35, 45, 60);

            lblQuickActions.AutoSize = true;

            lblQuickActions.Location =
                new Point(35, 360);

            contentPanel.Controls.Add(
                lblQuickActions
            );


            // CAREER ACTION CARD
            // -----------------------------------------------------

            Panel careerAction =
                CreateActionCard(
                    "Career Recommendation",
                    "Find careers that match your skills and interests.",
                    "Explore Careers",
                    new Point(35, 400),
                    true
                );

            contentPanel.Controls.Add(
                careerAction
            );

            // COURSE ACTION CARD
            // -----------------------------------------------------

            Panel courseAction =
                CreateActionCard(
                    "Course Recommendation",
                    "Discover courses that can help build your career.",
                    "Explore Courses",
                    new Point(480, 400),
                    false
                );

            contentPanel.Controls.Add(
                courseAction
            );

            // FOOTER
            // -----------------------------------------------------

            Label lblFooter = new Label();

            lblFooter.Text =
                "CareerPath © 2026 | Smart Career & Course Recommendation System";

            lblFooter.Font =
                new Font(
                    "Segoe UI",
                    8.5F
                );

            lblFooter.ForeColor =
                Color.FromArgb(140, 145, 150);

            lblFooter.AutoSize = true;

            lblFooter.Location =
                new Point(35, 650);

            contentPanel.Controls.Add(
                lblFooter
            );
        }

        // SIDEBAR BUTTON
        // =========================================================

        private Button CreateSidebarButton(
            string text,
            int top
        )
        {
            Button button = new Button();

            button.Text = text;

            button.Left = 15;
            button.Top = top;

            button.Width = 210;
            button.Height = 42;

            button.FlatStyle =
                FlatStyle.Flat;

            button.FlatAppearance.BorderSize = 0;

            button.BackColor =
                Color.FromArgb(37, 78, 139);

            button.ForeColor = Color.White;

            button.Font =
                new Font(
                    "Segoe UI",
                    9.5F,
                    FontStyle.Bold
                );

            button.TextAlign =
                ContentAlignment.MiddleLeft;

            button.Padding =
                new Padding(15, 0, 0, 0);

            button.Cursor =
                Cursors.Hand;

            return button;
        }

        // STATISTICS CARD
        // =========================================================

        private Panel CreateStatCard(
            string title,
            string value,
            Color accentColor,
            Point location
        )
        {
            Panel card = new Panel();

            card.Width = 205;
            card.Height = 135;

            card.Location = location;

            card.BackColor = Color.White;

            // Accent bar
            Panel accent = new Panel();

            accent.Width = 6;
            accent.Height = 135;

            accent.Left = 0;
            accent.Top = 0;

            accent.BackColor = accentColor;

            card.Controls.Add(accent);

            // Title
            Label lblTitle = new Label();

            lblTitle.Text = title;

            lblTitle.Font =
                new Font(
                    "Segoe UI",
                    9.5F
                );

            lblTitle.ForeColor =
                Color.FromArgb(110, 120, 135);

            lblTitle.AutoSize = true;

            lblTitle.Location =
                new Point(25, 22);

            card.Controls.Add(lblTitle);

            // Number
            Label lblValue = new Label();

            lblValue.Text = value;

            lblValue.Font =
                new Font(
                    "Segoe UI",
                    27F,
                    FontStyle.Bold
                );

            lblValue.ForeColor =
                accentColor;

            lblValue.AutoSize = true;

            lblValue.Location =
                new Point(25, 52);

            card.Controls.Add(lblValue);

            // Save number label
            card.Tag = lblValue;

            return card;
        }

        // ACTION CARD
        // =========================================================

        private Panel CreateActionCard(
            string title,
            string description,
            string buttonText,
            Point location,
            bool career
        )
        {
            Panel card = new Panel();

            card.Width = 400;
            card.Height = 170;

            card.Location = location;

            card.BackColor = Color.White;

            // TITLE
            // -----------------------------------------------------

            Label lblTitle = new Label();

            lblTitle.Text = title;

            lblTitle.Font =
                new Font(
                    "Segoe UI",
                    13F,
                    FontStyle.Bold
                );

            lblTitle.ForeColor =
                Color.FromArgb(35, 45, 60);

            lblTitle.AutoSize = true;

            lblTitle.Location =
                new Point(25, 20);

            card.Controls.Add(lblTitle);

            // DESCRIPTION
            // -----------------------------------------------------

            Label lblDescription = new Label();

            lblDescription.Text = description;

            lblDescription.Font =
                new Font(
                    "Segoe UI",
                    9F
                );

            lblDescription.ForeColor =
                Color.FromArgb(110, 120, 135);

            lblDescription.AutoSize = true;

            lblDescription.MaximumSize =
                new Size(350, 45);

            lblDescription.Location =
                new Point(25, 55);

            card.Controls.Add(lblDescription);

            // BUTTON
            // -----------------------------------------------------

            Button button = new Button();

            button.Text = buttonText;

            button.Width = 145;
            button.Height = 38;

            button.Left = 25;
            button.Top = 110;

            button.BackColor =
                Color.FromArgb(37, 78, 139);

            button.ForeColor = Color.White;

            button.FlatStyle =
                FlatStyle.Flat;

            button.FlatAppearance.BorderSize = 0;

            button.Font =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold
                );

            button.Cursor =
                Cursors.Hand;

            if (career)
            {
                button.Click +=
                    CareerButton_Click;
            }
            else
            {
                button.Click +=
                    CourseButton_Click;
            }

            card.Controls.Add(button);

            return card;
        }

        // LOAD DATABASE DATA
        // =========================================================

        private void LoadDashboardData()
        {
            try
            {
                lblCareerCount.Text =
                    GetCount("Careers").ToString();

                lblCourseCount.Text =
                    GetCount("Courses").ToString();

                lblSkillCount.Text =
                    GetCount("Skills").ToString();

                lblInterestCount.Text =
                    GetCount("Interests").ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load dashboard data.\n\n" +
                    ex.Message,
                    "Dashboard Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // GET TABLE COUNT
        // =========================================================

        private int GetCount(string tableName)
        {
            using (SqlConnection connection =
                   DatabaseManager.GetConnection())
            {
                connection.Open();

                string query =
                    "SELECT COUNT(*) FROM " + tableName;

                using (SqlCommand command =
                       new SqlCommand(
                           query,
                           connection
                       ))
                {
                    return Convert.ToInt32(
                        command.ExecuteScalar()
                    );
                }
            }
        }

        // DASHBOARD
        // =========================================================

        private void DashboardButton_Click(
            object sender,
            EventArgs e
        )
        {
            LoadDashboardData();
        }

        // CAREER
        // =========================================================

        private void CareerButton_Click(object sender, EventArgs e)
        {
            CareerRecommendationForm careerForm =
                new CareerRecommendationForm();

            careerForm.ShowDialog();
        }

        // COURSE
        // =========================================================

        private void CourseButton_Click(object? sender, EventArgs e)
        {
            CourseRecommendationForm courseForm =
                new CourseRecommendationForm();

            courseForm.ShowDialog();
        }

        // SKILLS
        // =========================================================

        private void SkillsButton_Click(
            object sender,
            EventArgs e)
        {
            SkillsForm skillsForm = new SkillsForm(userId);
            skillsForm.ShowDialog();
        }

        // INTEREST
        // =========================================================

        private void InterestButton_Click(
           object sender,
           EventArgs e)
        {
            InterestForm interestForm = new InterestForm(userId);
            interestForm.ShowDialog();
        }

        // PROFILE
        // =========================================================

        private void ProfileButton_Click(
            object sender,
            EventArgs e
        )
        {
            ProfileForm profileForm = new ProfileForm(userId);
            profileForm.ShowDialog();
        }

        // LOGOUT
        // =========================================================

        private void LogoutButton_Click(
            object sender,
            EventArgs e
        )
        {
            DialogResult result =
                MessageBox.Show(
                    "Are you sure you want to logout?",
                    "Logout",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

            if (result == DialogResult.Yes)
            {
                Close();
            }
        }
    }
}