namespace WinFormTest;

partial class Form1
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
    lblTitle = new Label();
    lblUsername = new Label();
    txtUsername = new TextBox();
    underlineUsername = new Panel();
    lblPassword = new Label();
    txtPassword = new TextBox();
    underlinePassword = new Panel();
    btnLogin = new Button();
    btnClose = new Button();
    btnMinimize = new Button();
    SuspendLayout();
    // 
    // lblTitle
    // 
    lblTitle.AutoSize = true;
    lblTitle.Font = new Font("Poppins", 24F, FontStyle.Regular, GraphicsUnit.Point);
    lblTitle.ForeColor = Color.FromArgb(45, 45, 48);
    lblTitle.Location = new Point(0, 80);
    lblTitle.Name = "lblTitle";
    lblTitle.Size = new Size(100, 45);
    lblTitle.TabIndex = 0;
    lblTitle.Text = "Welcome";
    lblTitle.TextAlign = ContentAlignment.MiddleCenter;
    // 
    // lblUsername
    // 
    lblUsername.AutoSize = true;
    lblUsername.Font = new Font("Poppins", 9F, FontStyle.Regular, GraphicsUnit.Point);
    lblUsername.ForeColor = Color.FromArgb(100, 100, 100);
    lblUsername.Location = new Point(0, 180);
    lblUsername.Name = "lblUsername";
    lblUsername.Size = new Size(100, 15);
    lblUsername.TabIndex = 1;
    lblUsername.Text = "Username";
    lblUsername.TextAlign = ContentAlignment.MiddleLeft;
    // 
    // txtUsername
    // 
    txtUsername.BackColor = Color.White;
    txtUsername.BorderStyle = BorderStyle.None;
    txtUsername.Font = new Font("Poppins", 11F, FontStyle.Regular, GraphicsUnit.Point);
    txtUsername.ForeColor = Color.FromArgb(45, 45, 48);
    txtUsername.Location = new Point(0, 200);
    txtUsername.Name = "txtUsername";
    txtUsername.Size = new Size(300, 20);
    txtUsername.TabIndex = 2;
    // 
    // underlineUsername
    // 
    underlineUsername.BackColor = Color.FromArgb(220, 220, 220);
    underlineUsername.Location = new Point(0, 220);
    underlineUsername.Name = "underlineUsername";
    underlineUsername.Size = new Size(300, 1);
    underlineUsername.TabIndex = 6;
    // 
    // lblPassword
    // 
    lblPassword.AutoSize = true;
    lblPassword.Font = new Font("Poppins", 9F, FontStyle.Regular, GraphicsUnit.Point);
    lblPassword.ForeColor = Color.FromArgb(100, 100, 100);
    lblPassword.Location = new Point(0, 250);
    lblPassword.Name = "lblPassword";
    lblPassword.Size = new Size(100, 15);
    lblPassword.TabIndex = 3;
    lblPassword.Text = "Password";
    lblPassword.TextAlign = ContentAlignment.MiddleLeft;
    // 
    // txtPassword
    // 
    txtPassword.BackColor = Color.White;
    txtPassword.BorderStyle = BorderStyle.None;
    txtPassword.Font = new Font("Poppins", 11F, FontStyle.Regular, GraphicsUnit.Point);
    txtPassword.ForeColor = Color.FromArgb(45, 45, 48);
    txtPassword.Location = new Point(0, 270);
    txtPassword.Name = "txtPassword";
    txtPassword.PasswordChar = '•';
    txtPassword.Size = new Size(300, 20);
    txtPassword.TabIndex = 4;
    // 
    // underlinePassword
    // 
    underlinePassword.BackColor = Color.FromArgb(220, 220, 220);
    underlinePassword.Location = new Point(0, 290);
    underlinePassword.Name = "underlinePassword";
    underlinePassword.Size = new Size(300, 1);
    underlinePassword.TabIndex = 7;
    // 
    // btnLogin
    // 
    btnLogin.BackColor = Color.FromArgb(45, 45, 48);
    btnLogin.FlatAppearance.BorderSize = 0;
    btnLogin.FlatStyle = FlatStyle.Flat;
    btnLogin.Font = new Font("Poppins", 11F, FontStyle.Regular, GraphicsUnit.Point);
    btnLogin.ForeColor = Color.White;
    btnLogin.Location = new Point(0, 320);
    btnLogin.Name = "btnLogin";
    btnLogin.Size = new Size(300, 42);
    btnLogin.TabIndex = 5;
    btnLogin.Text = "Sign In";
    btnLogin.UseVisualStyleBackColor = false;
    btnLogin.Click += btnLogin_Click;
    btnLogin.MouseEnter += btnLogin_MouseEnter;
    btnLogin.MouseLeave += btnLogin_MouseLeave;
    // 
    // btnClose
    // 
    btnClose.BackColor = Color.Transparent;
    btnClose.FlatAppearance.BorderSize = 0;
    btnClose.FlatStyle = FlatStyle.Flat;
    btnClose.Font = new Font("Poppins", 12F, FontStyle.Regular, GraphicsUnit.Point);
    btnClose.ForeColor = Color.FromArgb(100, 100, 100);
    btnClose.Location = new Point(370, 0);
    btnClose.Name = "btnClose";
    btnClose.Size = new Size(30, 30);
    btnClose.TabIndex = 8;
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
    btnMinimize.Font = new Font("Poppins", 14F, FontStyle.Regular, GraphicsUnit.Point);
    btnMinimize.ForeColor = Color.FromArgb(100, 100, 100);
    btnMinimize.Location = new Point(340, 0);
    btnMinimize.Name = "btnMinimize";
    btnMinimize.Size = new Size(30, 30);
    btnMinimize.TabIndex = 9;
    btnMinimize.Text = "−";
    btnMinimize.UseVisualStyleBackColor = false;
    btnMinimize.Click += btnMinimize_Click;
    btnMinimize.MouseEnter += btnMinimize_MouseEnter;
    btnMinimize.MouseLeave += btnMinimize_MouseLeave;
    // 
    // Form1
    // 
    AutoScaleMode = AutoScaleMode.Font;
    BackColor = Color.White;
    ClientSize = new Size(400, 500);
    Controls.Add(btnMinimize);
    Controls.Add(btnClose);
    Controls.Add(btnLogin);
    Controls.Add(underlinePassword);
    Controls.Add(txtPassword);
    Controls.Add(lblPassword);
    Controls.Add(underlineUsername);
    Controls.Add(txtUsername);
    Controls.Add(lblUsername);
    Controls.Add(lblTitle);
    FormBorderStyle = FormBorderStyle.None;
    Name = "Form1";
    StartPosition = FormStartPosition.CenterScreen;
    Text = "My Application";
    ResumeLayout(false);
    PerformLayout();
  }

  private Label lblTitle;
  private Label lblUsername;
  private TextBox txtUsername;
  private Panel underlineUsername;
  private Label lblPassword;
  private TextBox txtPassword;
  private Panel underlinePassword;
  private Button btnLogin;
  private Button btnClose;
  private Button btnMinimize;

  #endregion
}
