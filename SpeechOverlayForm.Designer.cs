namespace WinFormTest;

partial class SpeechOverlayForm
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
    panelMain = new Panel();
    lblMainText = new Label();
    lblDots = new Label();
    panelMain.SuspendLayout();
    SuspendLayout();
    // 
    // panelMain
    // 
    panelMain.BackColor = Color.Black;
    panelMain.BorderStyle = BorderStyle.None;
    panelMain.Controls.Add(lblDots);
    panelMain.Controls.Add(lblMainText);
    panelMain.Dock = DockStyle.Fill;
    panelMain.Location = new Point(0, 0);
    panelMain.Name = "panelMain";
    panelMain.Padding = new Padding(0);
    panelMain.Size = new Size(60, 20);
    panelMain.TabIndex = 0;
    panelMain.Paint += panelMain_Paint;
    // 
    // lblMainText
    // 
    lblMainText.AutoSize = true;
    lblMainText.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
    lblMainText.ForeColor = Color.White;
    lblMainText.Location = new Point(20, 20);
    lblMainText.Name = "lblMainText";
    lblMainText.Size = new Size(310, 20);
    lblMainText.TabIndex = 0;
    lblMainText.Text = "Click or hold Ctrl + Win to start dictating";
    lblMainText.Visible = false;
    // 
    // lblDots
    // 
    lblDots.AutoSize = true;
    lblDots.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
    lblDots.ForeColor = Color.FromArgb(200, 200, 200);
    lblDots.Location = new Point(20, 50);
    lblDots.Name = "lblDots";
    lblDots.Size = new Size(20, 25);
    lblDots.TabIndex = 1;
    lblDots.Text = "";
    lblDots.Visible = false;
    // 
    // SpeechOverlayForm
    // 
    AutoScaleMode = AutoScaleMode.Font;
    BackColor = Color.Black;
    ClientSize = new Size(60, 20);
    Controls.Add(panelMain);
    FormBorderStyle = FormBorderStyle.None;
    StartPosition = FormStartPosition.Manual;
    TopMost = true;
    ShowInTaskbar = false;
    panelMain.ResumeLayout(false);
    panelMain.PerformLayout();
    ResumeLayout(false);
  }

  private Panel panelMain;
  private Label lblMainText;
  private Label lblDots;

  #endregion
}
