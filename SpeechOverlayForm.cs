namespace WinFormTest;

public partial class SpeechOverlayForm : Form
{
  public enum OverlayState
  {
    Idle,
    Listening,
    Recognizing
  }

  private OverlayState currentState = OverlayState.Idle;
  private System.Windows.Forms.Timer? dotAnimationTimer;
  private int dotCount = 0;
  private int[] waveHeights = new int[4] { 3, 6, 4, 5 };

  public SpeechOverlayForm()
  {
    InitializeComponent();
    SetupOverlay();
    MakeRounded();
  }

  private void MakeRounded()
  {
    // Create pill shape using Region
    System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
    int radius = 10;
    path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
    path.AddArc(this.Width - radius * 2, 0, radius * 2, radius * 2, 270, 90);
    path.AddArc(this.Width - radius * 2, this.Height - radius * 2, radius * 2, radius * 2, 0, 90);
    path.AddArc(0, this.Height - radius * 2, radius * 2, radius * 2, 90, 90);
    path.CloseAllFigures();
    this.Region = new Region(path);
  }

  private void SetupOverlay()
  {
    // Position at bottom-center of screen
    Screen? primaryScreen = Screen.PrimaryScreen;
    if (primaryScreen != null)
    {
      int x = (primaryScreen.WorkingArea.Width - this.Width) / 2;
      int y = primaryScreen.WorkingArea.Bottom - this.Height - 20;
      this.Location = new Point(x, y);
    }

    SetState(OverlayState.Idle);
  }

  public void SetState(OverlayState state)
  {
    currentState = state;

    switch (state)
    {
      case OverlayState.Idle:
        HighlightHotkey();
        lblDots.Text = "";
        StopDotAnimation();
        panelMain.Invalidate();
        break;

      case OverlayState.Listening:
        StartDotAnimation();
        panelMain.Invalidate();
        break;

      case OverlayState.Recognizing:
        StopDotAnimation();
        panelMain.Invalidate();
        break;
    }
  }

  public void SetRecognizedText(string text)
  {
    lblMainText.Text = text;
    SetState(OverlayState.Recognizing);
  }

  private void HighlightHotkey()
  {
    // The hotkey text "Ctrl + Win" will be highlighted in the designer
    // For now, we'll keep it simple
  }

  private void StartDotAnimation()
  {
    dotAnimationTimer = new System.Windows.Forms.Timer();
    dotAnimationTimer.Interval = 500;
    dotAnimationTimer.Tick += DotAnimationTimer_Tick;
    dotAnimationTimer.Start();
    dotCount = 0;
    UpdateDots();
  }

  private void StopDotAnimation()
  {
    if (dotAnimationTimer != null)
    {
      dotAnimationTimer.Stop();
      dotAnimationTimer.Dispose();
      dotAnimationTimer = null;
    }
    lblDots.Text = "";
    // Reset wave heights
    for (int i = 0; i < waveHeights.Length; i++)
    {
      waveHeights[i] = 3;
    }
    panelMain.Invalidate();
  }

  private void DotAnimationTimer_Tick(object? sender, EventArgs e)
  {
    UpdateDots();
  }

  private void UpdateDots()
  {
    dotCount = (dotCount + 1) % 4;
    // Animate wave heights
    for (int i = 0; i < waveHeights.Length; i++)
    {
      int phase = (dotCount + i) % 4;
      waveHeights[i] = phase switch
      {
        0 => 3,
        1 => 8,
        2 => 5,
        3 => 6,
        _ => 4
      };
    }
    panelMain.Invalidate();
  }

  private void panelMain_Paint(object? sender, PaintEventArgs e)
  {
    Graphics g = e.Graphics;
    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
    
    // Draw white border
    using (Pen borderPen = new Pen(Color.White, 1))
    {
      int radius = 10;
      g.DrawArc(borderPen, 0, 0, radius * 2, radius * 2, 180, 90);
      g.DrawArc(borderPen, panelMain.Width - radius * 2, 0, radius * 2, radius * 2, 270, 90);
      g.DrawArc(borderPen, panelMain.Width - radius * 2, panelMain.Height - radius * 2, radius * 2, radius * 2, 0, 90);
      g.DrawArc(borderPen, 0, panelMain.Height - radius * 2, radius * 2, radius * 2, 90, 90);
      g.DrawLine(borderPen, radius, 0, panelMain.Width - radius, 0);
      g.DrawLine(borderPen, radius, panelMain.Height, panelMain.Width - radius, panelMain.Height);
      g.DrawLine(borderPen, 0, radius, 0, panelMain.Height - radius);
      g.DrawLine(borderPen, panelMain.Width, radius, panelMain.Width, panelMain.Height - radius);
    }
    
    // Draw voice waves animation when listening
    if (currentState == OverlayState.Listening)
    {
      using (Pen wavePen = new Pen(Color.White, 2))
      {
        int centerY = panelMain.Height / 2;
        int spacing = 8;
        int numWaves = waveHeights.Length;
        // Calculate total width needed: (numWaves - 1) * spacing for gaps
        int totalWaveWidth = (numWaves - 1) * spacing;
        // Center the waves: (panel width - total wave width) / 2
        int startX = (panelMain.Width - totalWaveWidth) / 2;
        
        for (int i = 0; i < waveHeights.Length; i++)
        {
          int x = startX + (i * spacing);
          int height = waveHeights[i];
          int topY = centerY - height / 2;
          int bottomY = centerY + height / 2;
          
          g.DrawLine(wavePen, x, topY, x, bottomY);
        }
      }
    }
  }

  protected override void OnFormClosing(FormClosingEventArgs e)
  {
    StopDotAnimation();
    base.OnFormClosing(e);
  }
}
