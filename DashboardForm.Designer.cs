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
    lblLogo = new Label();
    lblNavHome = new Label();
    lblNavDictionary = new Label();
    lblNavSnippets = new Label();
    lblNavStyle = new Label();
    lblNavNotes = new Label();
    lblNavSettings = new Label();
    lblNavHelp = new Label();
    panelMain = new Panel();
    lblWelcome = new Label();
    lblStatWeeks = new Label();
    lblStatWords = new Label();
    lblStatWPM = new Label();
    lblToday = new Label();
    panelSpeechHistory = new Panel();
    btnClose = new Button();
    btnMinimize = new Button();
    panelSidebar.SuspendLayout();
    panelMain.SuspendLayout();
    SuspendLayout();
    // 
    // panelSidebar
    // 
    panelSidebar.BackColor = Color.White;
    panelSidebar.Controls.Add(lblNavHelp);
    panelSidebar.Controls.Add(lblNavSettings);
    panelSidebar.Controls.Add(lblNavNotes);
    panelSidebar.Controls.Add(lblNavStyle);
    panelSidebar.Controls.Add(lblNavSnippets);
    panelSidebar.Controls.Add(lblNavDictionary);
    panelSidebar.Controls.Add(lblNavHome);
    panelSidebar.Controls.Add(lblLogo);
    panelSidebar.Dock = DockStyle.Left;
    panelSidebar.Location = new Point(0, 0);
    panelSidebar.Name = "panelSidebar";
    panelSidebar.Size = new Size(250, 800);
    panelSidebar.TabIndex = 0;
    // 
    // lblLogo
    // 
    lblLogo.AutoSize = true;
    lblLogo.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point);
    lblLogo.ForeColor = Color.FromArgb(45, 45, 48);
    lblLogo.Location = new Point(20, 30);
    lblLogo.Name = "lblLogo";
    lblLogo.Size = new Size(60, 30);
    lblLogo.TabIndex = 0;
    lblLogo.Text = "Flow";
    // 
    // lblNavHome
    // 
    lblNavHome.AutoSize = true;
    lblNavHome.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
    lblNavHome.ForeColor = Color.FromArgb(45, 45, 48);
    lblNavHome.Location = new Point(20, 100);
    lblNavHome.Name = "lblNavHome";
    lblNavHome.Padding = new Padding(10, 8, 10, 8);
    lblNavHome.Size = new Size(60, 35);
    lblNavHome.TabIndex = 1;
    lblNavHome.Text = "Home";
    lblNavHome.BackColor = Color.FromArgb(245, 245, 245);
    // 
    // lblNavDictionary
    // 
    lblNavDictionary.AutoSize = true;
    lblNavDictionary.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
    lblNavDictionary.ForeColor = Color.FromArgb(100, 100, 100);
    lblNavDictionary.Location = new Point(20, 150);
    lblNavDictionary.Name = "lblNavDictionary";
    lblNavDictionary.Padding = new Padding(10, 8, 10, 8);
    lblNavDictionary.Size = new Size(90, 35);
    lblNavDictionary.TabIndex = 2;
    lblNavDictionary.Text = "Dictionary";
    lblNavDictionary.MouseEnter += navItem_MouseEnter;
    lblNavDictionary.MouseLeave += navItem_MouseLeave;
    // 
    // lblNavSnippets
    // 
    lblNavSnippets.AutoSize = true;
    lblNavSnippets.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
    lblNavSnippets.ForeColor = Color.FromArgb(100, 100, 100);
    lblNavSnippets.Location = new Point(20, 200);
    lblNavSnippets.Name = "lblNavSnippets";
    lblNavSnippets.Padding = new Padding(10, 8, 10, 8);
    lblNavSnippets.Size = new Size(80, 35);
    lblNavSnippets.TabIndex = 3;
    lblNavSnippets.Text = "Snippets";
    lblNavSnippets.MouseEnter += navItem_MouseEnter;
    lblNavSnippets.MouseLeave += navItem_MouseLeave;
    // 
    // lblNavStyle
    // 
    lblNavStyle.AutoSize = true;
    lblNavStyle.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
    lblNavStyle.ForeColor = Color.FromArgb(100, 100, 100);
    lblNavStyle.Location = new Point(20, 250);
    lblNavStyle.Name = "lblNavStyle";
    lblNavStyle.Padding = new Padding(10, 8, 10, 8);
    lblNavStyle.Size = new Size(50, 35);
    lblNavStyle.TabIndex = 4;
    lblNavStyle.Text = "Style";
    lblNavStyle.MouseEnter += navItem_MouseEnter;
    lblNavStyle.MouseLeave += navItem_MouseLeave;
    // 
    // lblNavNotes
    // 
    lblNavNotes.AutoSize = true;
    lblNavNotes.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
    lblNavNotes.ForeColor = Color.FromArgb(100, 100, 100);
    lblNavNotes.Location = new Point(20, 300);
    lblNavNotes.Name = "lblNavNotes";
    lblNavNotes.Padding = new Padding(10, 8, 10, 8);
    lblNavNotes.Size = new Size(60, 35);
    lblNavNotes.TabIndex = 5;
    lblNavNotes.Text = "Notes";
    lblNavNotes.MouseEnter += navItem_MouseEnter;
    lblNavNotes.MouseLeave += navItem_MouseLeave;
    // 
    // lblNavSettings
    // 
    lblNavSettings.AutoSize = true;
    lblNavSettings.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
    lblNavSettings.ForeColor = Color.FromArgb(100, 100, 100);
    lblNavSettings.Location = new Point(20, 700);
    lblNavSettings.Name = "lblNavSettings";
    lblNavSettings.Padding = new Padding(10, 8, 10, 8);
    lblNavSettings.Size = new Size(75, 35);
    lblNavSettings.TabIndex = 6;
    lblNavSettings.Text = "Settings";
    lblNavSettings.MouseEnter += navItem_MouseEnter;
    lblNavSettings.MouseLeave += navItem_MouseLeave;
    // 
    // lblNavHelp
    // 
    lblNavHelp.AutoSize = true;
    lblNavHelp.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
    lblNavHelp.ForeColor = Color.FromArgb(100, 100, 100);
    lblNavHelp.Location = new Point(20, 750);
    lblNavHelp.Name = "lblNavHelp";
    lblNavHelp.Padding = new Padding(10, 8, 10, 8);
    lblNavHelp.Size = new Size(50, 35);
    lblNavHelp.TabIndex = 7;
    lblNavHelp.Text = "Help";
    lblNavHelp.MouseEnter += navItem_MouseEnter;
    lblNavHelp.MouseLeave += navItem_MouseLeave;
    // 
    // panelMain
    // 
    panelMain.BackColor = Color.White;
    panelMain.Controls.Add(panelSpeechHistory);
    panelMain.Controls.Add(lblToday);
    panelMain.Controls.Add(lblStatWPM);
    panelMain.Controls.Add(lblStatWords);
    panelMain.Controls.Add(lblStatWeeks);
    panelMain.Controls.Add(lblWelcome);
    panelMain.Dock = DockStyle.Fill;
    panelMain.Location = new Point(250, 0);
    panelMain.Name = "panelMain";
    panelMain.Padding = new Padding(40, 60, 40, 40);
    panelMain.Size = new Size(950, 800);
    panelMain.TabIndex = 1;
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
    panelSpeechHistory.Size = new Size(870, 540);
    panelSpeechHistory.TabIndex = 5;
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
    btnMinimize.Text = "−";
    btnMinimize.UseVisualStyleBackColor = false;
    btnMinimize.Click += btnMinimize_Click;
    btnMinimize.MouseEnter += btnMinimize_MouseEnter;
    btnMinimize.MouseLeave += btnMinimize_MouseLeave;
    // 
    // DashboardForm
    // 
    AutoScaleMode = AutoScaleMode.Font;
    BackColor = Color.White;
    ClientSize = new Size(1200, 800);
    Controls.Add(btnMinimize);
    Controls.Add(btnClose);
    Controls.Add(panelMain);
    Controls.Add(panelSidebar);
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
  private Label lblLogo;
  private Label lblNavHome;
  private Label lblNavDictionary;
  private Label lblNavSnippets;
  private Label lblNavStyle;
  private Label lblNavNotes;
  private Label lblNavSettings;
  private Label lblNavHelp;
  private Panel panelMain;
  private Label lblWelcome;
  private Label lblStatWeeks;
  private Label lblStatWords;
  private Label lblStatWPM;
  private Label lblToday;
  private Panel panelSpeechHistory;
  private Button btnClose;
  private Button btnMinimize;

  #endregion
}
