namespace WinFormTest;

partial class DashboardForm
{
  /// <summary>
  ///  Required designer variable.
  /// </summary>
  private System.ComponentModel.IContainer components = null;

  /// <summary>
  ///  Clean up any resources being used.
  /// </summary>
  /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
  protected override void Dispose(bool disposing)
  {
    if (disposing && (components != null))
    {
      components.Dispose();
    }
    base.Dispose(disposing);
  }

  #region Windows Form Designer generated code

  /// <summary>
  ///  Required method for Designer support - do not modify
  ///  the contents of this method with the code editor.
  /// </summary>
  private void InitializeComponent()
  {
    components = new System.ComponentModel.Container();
    panelSidebar = new Panel();
    picLogo = new PictureBox();
    lblLogo = new Label();
    lblNavHome = new Label();
    lblNavDictionary = new Label();
    lblNavSnippets = new Label();
    lblNavStyle = new Label();
    lblNavSettings = new Label();
    panelMain = new Panel();
    panelHomePage = new Panel();
    lblWelcome = new Label();
    lblStatWeeks = new Label();
    lblStatWords = new Label();
    lblStatWPM = new Label();
    lblToday = new Label();
    panelSpeechHistory = new NoScrollbarPanel();
    panelDictionaryPage = new Panel();
    panelSnippetsPage = new Panel();
    panelStylePage = new Panel();
    panelSettingsPage = new Panel();
    btnClose = new Button();
    btnMinimize = new Button();
    panelTopRibbon = new Panel();
    panelSidebar.SuspendLayout();
    panelMain.SuspendLayout();
    SuspendLayout();
    // 
    // panelSidebar
    // 
    panelSidebar.BackColor = Color.White;
    panelSidebar.Controls.Add(lblNavSettings);
    panelSidebar.Controls.Add(lblNavStyle);
    panelSidebar.Controls.Add(lblNavSnippets);
    panelSidebar.Controls.Add(lblNavDictionary);
    panelSidebar.Controls.Add(lblNavHome);
    panelSidebar.Controls.Add(picLogo);
    panelSidebar.Controls.Add(lblLogo);
    panelSidebar.Dock = DockStyle.Left;
    panelSidebar.Location = new Point(0, 40);
    panelSidebar.Name = "panelSidebar";
    panelSidebar.Size = new Size(200, 760);
    panelSidebar.TabIndex = 0;
    panelSidebar.Padding = new Padding(0, 0, 0, 0);
    // 
    // picLogo
    // 
    picLogo.Location = new Point(22, 11);
    picLogo.Name = "picLogo";
    picLogo.Size = new Size(20, 20);
    picLogo.SizeMode = PictureBoxSizeMode.StretchImage;
    picLogo.TabIndex = 0;
    picLogo.TabStop = false;
    // 
    // lblLogo
    // 
    lblLogo.AutoSize = true;
    lblLogo.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
    lblLogo.ForeColor = Color.FromArgb(45, 45, 48);
    lblLogo.Location = new Point(40, 10);
    lblLogo.Name = "lblLogo";
    lblLogo.Size = new Size(45, 21);
    lblLogo.TabIndex = 1;
    lblLogo.Text = "Textify";
    // 
    // lblNavHome
    // 
    lblNavHome.AutoSize = false;
    lblNavHome.Cursor = Cursors.Hand;
    lblNavHome.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
    lblNavHome.ForeColor = Color.FromArgb(45, 45, 48);
    lblNavHome.Location = new Point(10, 40);
    lblNavHome.Name = "lblNavHome";
    lblNavHome.Padding = new Padding(10, 9, 10, 8);
    lblNavHome.Size = new Size(180, 35);
    lblNavHome.TabIndex = 2;
    lblNavHome.Text = "🏠 Home";
    lblNavHome.TextAlign = ContentAlignment.MiddleLeft;
    lblNavHome.BackColor = Color.FromArgb(245, 245, 245);
    lblNavHome.Click += navItem_Click;
    lblNavHome.MouseEnter += navItem_MouseEnter;
    lblNavHome.MouseLeave += navItem_MouseLeave;
    // 
    // lblNavDictionary
    // 
    lblNavDictionary.AutoSize = false;
    lblNavDictionary.Cursor = Cursors.Hand;
    lblNavDictionary.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
    lblNavDictionary.ForeColor = Color.FromArgb(100, 100, 100);
    lblNavDictionary.Location = new Point(10, 80);
    lblNavDictionary.Name = "lblNavDictionary";
    lblNavDictionary.Padding = new Padding(10, 9, 10, 8);
    lblNavDictionary.Size = new Size(180, 35);
    lblNavDictionary.TabIndex = 3;
    lblNavDictionary.Text = "📚 Dictionary";
    lblNavDictionary.TextAlign = ContentAlignment.MiddleLeft;
    lblNavDictionary.Click += navItem_Click;
    lblNavDictionary.MouseEnter += navItem_MouseEnter;
    lblNavDictionary.MouseLeave += navItem_MouseLeave;
    // 
    // lblNavSnippets
    // 
    lblNavSnippets.AutoSize = false;
    lblNavSnippets.Cursor = Cursors.Hand;
    lblNavSnippets.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
    lblNavSnippets.ForeColor = Color.FromArgb(100, 100, 100);
    lblNavSnippets.Location = new Point(10, 120);
    lblNavSnippets.Name = "lblNavSnippets";
    lblNavSnippets.Padding = new Padding(10, 9, 10, 8);
    lblNavSnippets.Size = new Size(180, 35);
    lblNavSnippets.TabIndex = 4;
    lblNavSnippets.Text = "📝 Snippets";
    lblNavSnippets.TextAlign = ContentAlignment.MiddleLeft;
    lblNavSnippets.Click += navItem_Click;
    lblNavSnippets.MouseEnter += navItem_MouseEnter;
    lblNavSnippets.MouseLeave += navItem_MouseLeave;
    // 
    // lblNavStyle
    // 
    lblNavStyle.AutoSize = false;
    lblNavStyle.Cursor = Cursors.Hand;
    lblNavStyle.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
    lblNavStyle.ForeColor = Color.FromArgb(100, 100, 100);
    lblNavStyle.Location = new Point(10, 160);
    lblNavStyle.Name = "lblNavStyle";
    lblNavStyle.Padding = new Padding(10, 9, 10, 8);
    lblNavStyle.Size = new Size(180, 35);
    lblNavStyle.TabIndex = 5;
    lblNavStyle.Text = "🎨 Style";
    lblNavStyle.TextAlign = ContentAlignment.MiddleLeft;
    lblNavStyle.Click += navItem_Click;
    lblNavStyle.MouseEnter += navItem_MouseEnter;
    lblNavStyle.MouseLeave += navItem_MouseLeave;
    // 
    // lblNavSettings
    // 
    lblNavSettings.AutoSize = false;
    lblNavSettings.Cursor = Cursors.Hand;
    lblNavSettings.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
    lblNavSettings.ForeColor = Color.FromArgb(100, 100, 100);
    lblNavSettings.Location = new Point(10, 200);
    lblNavSettings.Name = "lblNavSettings";
    lblNavSettings.Padding = new Padding(10, 9, 10, 8);
    lblNavSettings.Size = new Size(180, 35);
    lblNavSettings.TabIndex = 6;
    lblNavSettings.Text = "⚙️ Settings";
    lblNavSettings.TextAlign = ContentAlignment.MiddleLeft;
    lblNavSettings.Click += navItem_Click;
    lblNavSettings.MouseEnter += navItem_MouseEnter;
    lblNavSettings.MouseLeave += navItem_MouseLeave;
    // 
    // panelMain
    // 
    panelMain.BackColor = Color.White;
    panelMain.Controls.Add(panelSettingsPage);
    panelMain.Controls.Add(panelStylePage);
    panelMain.Controls.Add(panelSnippetsPage);
    panelMain.Controls.Add(panelDictionaryPage);
    panelMain.Controls.Add(panelHomePage);
    panelMain.Dock = DockStyle.Fill;
    panelMain.Location = new Point(250, 40);
    panelMain.Name = "panelMain";
    panelMain.Size = new Size(950, 760);
    panelMain.TabIndex = 1;
    panelMain.Padding = new Padding(0, 0, 10, 10);
    // 
    // panelHomePage
    // 
    panelHomePage.BackColor = Color.White;
    panelHomePage.Controls.Add(panelSpeechHistory);
    panelHomePage.Controls.Add(lblToday);
    panelHomePage.Controls.Add(lblStatWPM);
    panelHomePage.Controls.Add(lblStatWords);
    panelHomePage.Controls.Add(lblStatWeeks);
    panelHomePage.Controls.Add(lblWelcome);
    panelHomePage.Dock = DockStyle.Fill;
    panelHomePage.Location = new Point(0, 0);
    panelHomePage.Name = "panelHomePage";
    panelHomePage.Padding = new Padding(40, 60, 40, 50);
    panelHomePage.Size = new Size(950, 800);
    panelHomePage.TabIndex = 0;
    // 
    // lblWelcome
    // 
    lblWelcome.AutoSize = true;
    lblWelcome.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point);
    lblWelcome.ForeColor = Color.FromArgb(45, 45, 48);
    lblWelcome.Location = new Point(40, 60);
    lblWelcome.Name = "lblWelcome";
    lblWelcome.Size = new Size(300, 45);
    lblWelcome.TabIndex = 0;
    lblWelcome.Text = "Welcome back!";
    // 
    // lblStatWeeks
    // 
    lblStatWeeks.AutoSize = true;
    lblStatWeeks.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
    lblStatWeeks.ForeColor = Color.FromArgb(100, 100, 100);
    lblStatWeeks.Location = new Point(40, 120);
    lblStatWeeks.Name = "lblStatWeeks";
    lblStatWeeks.Size = new Size(70, 19);
    lblStatWeeks.TabIndex = 1;
    lblStatWeeks.Text = "🔥 5 weeks";
    // 
    // lblStatWords
    // 
    lblStatWords.AutoSize = true;
    lblStatWords.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
    lblStatWords.ForeColor = Color.FromArgb(100, 100, 100);
    lblStatWords.Location = new Point(130, 120);
    lblStatWords.Name = "lblStatWords";
    lblStatWords.Size = new Size(100, 19);
    lblStatWords.TabIndex = 2;
    lblStatWords.Text = "🚀 20.2K words";
    // 
    // lblStatWPM
    // 
    lblStatWPM.AutoSize = true;
    lblStatWPM.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
    lblStatWPM.ForeColor = Color.FromArgb(100, 100, 100);
    lblStatWPM.Location = new Point(250, 120);
    lblStatWPM.Name = "lblStatWPM";
    lblStatWPM.Size = new Size(80, 19);
    lblStatWPM.TabIndex = 3;
    lblStatWPM.Text = "🏆 111 WPM";
    // 
    // lblToday
    // 
    lblToday.AutoSize = true;
    lblToday.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
    lblToday.ForeColor = Color.FromArgb(45, 45, 48);
    lblToday.Location = new Point(40, 180);
    lblToday.Name = "lblToday";
    lblToday.Size = new Size(60, 21);
    lblToday.TabIndex = 4;
    lblToday.Text = "TODAY";
    // 
    // panelSpeechHistory
    // 
    panelSpeechHistory.AutoScroll = true;
    panelSpeechHistory.BackColor = Color.White;
    panelSpeechHistory.Location = new Point(40, 220);
    panelSpeechHistory.Name = "panelSpeechHistory";
    panelSpeechHistory.Size = new Size(870, 520);
    panelSpeechHistory.TabIndex = 5;
    panelSpeechHistory.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    panelSpeechHistory.Padding = new Padding(0, 0, 0, 10);
    // 
    // panelDictionaryPage
    // 
    panelDictionaryPage.BackColor = Color.White;
    panelDictionaryPage.Dock = DockStyle.Fill;
    panelDictionaryPage.Location = new Point(0, 0);
    panelDictionaryPage.Name = "panelDictionaryPage";
    panelDictionaryPage.Size = new Size(950, 800);
    panelDictionaryPage.TabIndex = 1;
    // 
    // panelSnippetsPage
    // 
    panelSnippetsPage.BackColor = Color.White;
    panelSnippetsPage.Dock = DockStyle.Fill;
    panelSnippetsPage.Location = new Point(0, 0);
    panelSnippetsPage.Name = "panelSnippetsPage";
    panelSnippetsPage.Size = new Size(950, 800);
    panelSnippetsPage.TabIndex = 2;
    // 
    // panelStylePage
    // 
    panelStylePage.BackColor = Color.White;
    panelStylePage.Dock = DockStyle.Fill;
    panelStylePage.Location = new Point(0, 0);
    panelStylePage.Name = "panelStylePage";
    panelStylePage.Size = new Size(950, 800);
    panelStylePage.TabIndex = 3;
    // 
    // panelSettingsPage
    // 
    panelSettingsPage.BackColor = Color.White;
    panelSettingsPage.Dock = DockStyle.Fill;
    panelSettingsPage.Location = new Point(0, 0);
    panelSettingsPage.Name = "panelSettingsPage";
    panelSettingsPage.Size = new Size(950, 800);
    panelSettingsPage.TabIndex = 4;
    // 
    // btnClose
    // 
    btnClose.BackColor = Color.Transparent;
    btnClose.FlatAppearance.BorderSize = 0;
    btnClose.FlatStyle = FlatStyle.Flat;
    btnClose.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
    btnClose.ForeColor = Color.FromArgb(100, 100, 100);
    btnClose.Location = new Point(1170, 0);
    btnClose.Name = "btnClose";
    btnClose.Size = new Size(30, 30);
    btnClose.TabIndex = 2;
    btnClose.TabStop = false;
    btnClose.Text = "×";
    btnClose.UseVisualStyleBackColor = false;
    btnClose.Click += btnClose_Click;
    btnClose.MouseEnter += btnClose_MouseEnter;
    btnClose.MouseLeave += btnClose_MouseLeave;
    // 
    // btnMinimize
    // 
    btnMinimize.BackColor = Color.Transparent;
    btnMinimize.FlatAppearance.BorderSize = 0;
    btnMinimize.FlatStyle = FlatStyle.Flat;
    btnMinimize.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
    btnMinimize.ForeColor = Color.FromArgb(100, 100, 100);
    btnMinimize.Location = new Point(1140, 0);
    btnMinimize.Name = "btnMinimize";
    btnMinimize.Size = new Size(30, 30);
    btnMinimize.TabIndex = 3;
    btnMinimize.TabStop = false;
    btnMinimize.Text = "−";
    btnMinimize.UseVisualStyleBackColor = false;
    btnMinimize.Click += btnMinimize_Click;
    btnMinimize.MouseEnter += btnMinimize_MouseEnter;
    btnMinimize.MouseLeave += btnMinimize_MouseLeave;
    // 
    // panelTopRibbon
    // 
    panelTopRibbon.BackColor = Color.White;
    panelTopRibbon.Dock = DockStyle.Top;
    panelTopRibbon.Location = new Point(0, 0);
    panelTopRibbon.Name = "panelTopRibbon";
    panelTopRibbon.Size = new Size(1200, 30);
    panelTopRibbon.TabIndex = 4;
    // 
    // DashboardForm
    // 
    AutoScaleMode = AutoScaleMode.Font;
    BackColor = Color.White;
    ClientSize = new Size(1200, 800);
    Controls.Add(panelMain);
    Controls.Add(panelSidebar);
    Controls.Add(panelTopRibbon);
    Controls.Add(btnMinimize);
    Controls.Add(btnClose);
    FormBorderStyle = FormBorderStyle.None;
    Name = "DashboardForm";
    StartPosition = FormStartPosition.CenterScreen;
    Text = "Dashboard";
    panelSidebar.ResumeLayout(false);
    panelSidebar.PerformLayout();
    panelMain.ResumeLayout(false);
    panelMain.PerformLayout();
    ResumeLayout(false);
  }

  private Panel panelSidebar;
  private PictureBox picLogo;
  private Label lblLogo;
  private Label lblNavHome;
  private Label lblNavDictionary;
  private Label lblNavSnippets;
  private Label lblNavStyle;
  private Label lblNavSettings;
  private Panel panelMain;
  private Panel panelHomePage;
  private Label lblWelcome;
  private Label lblStatWeeks;
  private Label lblStatWords;
  private Label lblStatWPM;
  private Label lblToday;
  private NoScrollbarPanel panelSpeechHistory;
  private Panel panelDictionaryPage;
  private Panel panelSnippetsPage;
  private Panel panelStylePage;
  private Panel panelSettingsPage;
  private Button btnClose;
  private Button btnMinimize;
  private Panel panelTopRibbon;

  #endregion
}
