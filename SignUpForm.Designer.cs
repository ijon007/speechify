namespace WinFormTest;

partial class SignUpForm
{
  private System.ComponentModel.IContainer components = null;

  protected override void Dispose(bool disposing)
  {
    if (disposing && (components != null))
      components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    lblTitle = new Label();
    lblUsername = new Label();
    txtUsername = new TextBox();
    underlineUsername = new Panel();
    lblPassword = new Label();
    txtPassword = new TextBox();
    underlinePassword = new Panel();
    lblConfirm = new Label();
    txtConfirm = new TextBox();
    underlineConfirm = new Panel();
    btnCreate = new Button();
    SuspendLayout();
    //
    // lblTitle
    //
    lblTitle.AutoSize = true;
    lblTitle.Font = new Font("Poppins", 18F, FontStyle.Regular, GraphicsUnit.Point);
    lblTitle.ForeColor = Color.FromArgb(45, 45, 48);
    lblTitle.Location = new Point(0, 24);
    lblTitle.Name = "lblTitle";
    lblTitle.Size = new Size(200, 34);
    lblTitle.TabIndex = 0;
    lblTitle.Text = "Create account";
    lblTitle.TextAlign = ContentAlignment.MiddleCenter;
    //
    // lblUsername
    //
    lblUsername.AutoSize = true;
    lblUsername.Font = new Font("Poppins", 9F, FontStyle.Regular, GraphicsUnit.Point);
    lblUsername.ForeColor = Color.FromArgb(100, 100, 100);
    lblUsername.Location = new Point(0, 80);
    lblUsername.Name = "lblUsername";
    lblUsername.Size = new Size(100, 15);
    lblUsername.TabIndex = 1;
    lblUsername.Text = "Username";
    //
    // txtUsername
    //
    txtUsername.BackColor = Color.White;
    txtUsername.BorderStyle = BorderStyle.None;
    txtUsername.Font = new Font("Poppins", 11F, FontStyle.Regular, GraphicsUnit.Point);
    txtUsername.ForeColor = Color.FromArgb(45, 45, 48);
    txtUsername.Location = new Point(0, 100);
    txtUsername.Name = "txtUsername";
    txtUsername.Size = new Size(280, 20);
    txtUsername.TabIndex = 2;
    //
    // underlineUsername
    //
    underlineUsername.BackColor = Color.FromArgb(220, 220, 220);
    underlineUsername.Location = new Point(0, 120);
    underlineUsername.Name = "underlineUsername";
    underlineUsername.Size = new Size(280, 1);
    underlineUsername.TabIndex = 3;
    //
    // lblPassword
    //
    lblPassword.AutoSize = true;
    lblPassword.Font = new Font("Poppins", 9F, FontStyle.Regular, GraphicsUnit.Point);
    lblPassword.ForeColor = Color.FromArgb(100, 100, 100);
    lblPassword.Location = new Point(0, 140);
    lblPassword.Name = "lblPassword";
    lblPassword.Size = new Size(100, 15);
    lblPassword.TabIndex = 4;
    lblPassword.Text = "Password";
    //
    // txtPassword
    //
    txtPassword.BackColor = Color.White;
    txtPassword.BorderStyle = BorderStyle.None;
    txtPassword.Font = new Font("Poppins", 11F, FontStyle.Regular, GraphicsUnit.Point);
    txtPassword.ForeColor = Color.FromArgb(45, 45, 48);
    txtPassword.Location = new Point(0, 160);
    txtPassword.Name = "txtPassword";
    txtPassword.PasswordChar = '•';
    txtPassword.Size = new Size(280, 20);
    txtPassword.TabIndex = 5;
    //
    // underlinePassword
    //
    underlinePassword.BackColor = Color.FromArgb(220, 220, 220);
    underlinePassword.Location = new Point(0, 180);
    underlinePassword.Name = "underlinePassword";
    underlinePassword.Size = new Size(280, 1);
    underlinePassword.TabIndex = 6;
    //
    // lblConfirm
    //
    lblConfirm.AutoSize = true;
    lblConfirm.Font = new Font("Poppins", 9F, FontStyle.Regular, GraphicsUnit.Point);
    lblConfirm.ForeColor = Color.FromArgb(100, 100, 100);
    lblConfirm.Location = new Point(0, 200);
    lblConfirm.Name = "lblConfirm";
    lblConfirm.Size = new Size(100, 15);
    lblConfirm.TabIndex = 7;
    lblConfirm.Text = "Confirm password";
    //
    // txtConfirm
    //
    txtConfirm.BackColor = Color.White;
    txtConfirm.BorderStyle = BorderStyle.None;
    txtConfirm.Font = new Font("Poppins", 11F, FontStyle.Regular, GraphicsUnit.Point);
    txtConfirm.ForeColor = Color.FromArgb(45, 45, 48);
    txtConfirm.Location = new Point(0, 220);
    txtConfirm.Name = "txtConfirm";
    txtConfirm.PasswordChar = '•';
    txtConfirm.Size = new Size(280, 20);
    txtConfirm.TabIndex = 8;
    //
    // underlineConfirm
    //
    underlineConfirm.BackColor = Color.FromArgb(220, 220, 220);
    underlineConfirm.Location = new Point(0, 240);
    underlineConfirm.Name = "underlineConfirm";
    underlineConfirm.Size = new Size(280, 1);
    underlineConfirm.TabIndex = 9;
    //
    // btnCreate
    //
    btnCreate.BackColor = Color.FromArgb(45, 45, 48);
    btnCreate.FlatAppearance.BorderSize = 0;
    btnCreate.FlatStyle = FlatStyle.Flat;
    btnCreate.Font = new Font("Poppins", 11F, FontStyle.Regular, GraphicsUnit.Point);
    btnCreate.ForeColor = Color.White;
    btnCreate.Location = new Point(0, 268);
    btnCreate.Name = "btnCreate";
    btnCreate.Size = new Size(280, 42);
    btnCreate.TabIndex = 10;
    btnCreate.Text = "Create account";
    btnCreate.UseVisualStyleBackColor = false;
    btnCreate.Click += btnCreate_Click;
    //
    // SignUpForm
    //
    AutoScaleMode = AutoScaleMode.Font;
    BackColor = Color.White;
    ClientSize = new Size(320, 340);
    Controls.Add(btnCreate);
    Controls.Add(underlineConfirm);
    Controls.Add(txtConfirm);
    Controls.Add(lblConfirm);
    Controls.Add(underlinePassword);
    Controls.Add(txtPassword);
    Controls.Add(lblPassword);
    Controls.Add(underlineUsername);
    Controls.Add(txtUsername);
    Controls.Add(lblUsername);
    Controls.Add(lblTitle);
    FormBorderStyle = FormBorderStyle.FixedDialog;
    MaximizeBox = false;
    MinimizeBox = false;
    Name = "SignUpForm";
    StartPosition = FormStartPosition.CenterParent;
    Text = "Sign Up";
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
  private Label lblConfirm;
  private TextBox txtConfirm;
  private Panel underlineConfirm;
  private Button btnCreate;
}
