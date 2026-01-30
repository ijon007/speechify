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
  private string overlayPosition = "bottom_center";

  public SpeechOverlayForm()
  {
    InitializeComponent();
    // Enable double buffering for smooth rendering
    SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.ResizeRedraw, true);
    // Enable double buffering for the panel as well
    if (panelMain != null)
    {
      typeof(Panel).InvokeMember("DoubleBuffered",
        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
        null, panelMain, new object[] { true });
    }
    SetupOverlay();
    MakeRounded();
  }

  private void MakeRounded()
  {
    // Create pill shape using Region
    System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
    // For thin pills, use smaller radius (half the height, but max 5)
    int radius = Math.Min(this.Height / 2, 5);
    path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
    path.AddArc(this.Width - radius * 2, 0, radius * 2, radius * 2, 270, 90);
    path.AddArc(this.Width - radius * 2, this.Height - radius * 2, radius * 2, radius * 2, 0, 90);
    path.AddArc(0, this.Height - radius * 2, radius * 2, radius * 2, 90, 90);
    path.CloseAllFigures();
    this.Region = new Region(path);
  }

  public void SetOverlayPosition(string position)
  {
    overlayPosition = position;
    UpdateOverlayPosition();
  }

  private void SetupOverlay()
  {
    ResizeFormForState(OverlayState.Idle);
    UpdateOverlayPosition();
    SetState(OverlayState.Idle);
  }

  private void UpdateOverlayPosition()
  {
    Screen? primaryScreen = Screen.PrimaryScreen;
    if (primaryScreen == null) return;

    int x = 0, y = 0;
    const int margin = 20;

    switch (overlayPosition)
    {
      case "top_center":
        x = (primaryScreen.WorkingArea.Width - this.Width) / 2;
        y = primaryScreen.WorkingArea.Top + margin;
        break;
      case "top_left":
        x = primaryScreen.WorkingArea.Left + margin;
        y = primaryScreen.WorkingArea.Top + margin;
        break;
      case "top_right":
        x = primaryScreen.WorkingArea.Right - this.Width - margin;
        y = primaryScreen.WorkingArea.Top + margin;
        break;
      case "bottom_left":
        x = primaryScreen.WorkingArea.Left + margin;
        y = primaryScreen.WorkingArea.Bottom - this.Height - margin;
        break;
      case "bottom_right":
        x = primaryScreen.WorkingArea.Right - this.Width - margin;
        y = primaryScreen.WorkingArea.Bottom - this.Height - margin;
        break;
      case "bottom_center":
      default:
        x = (primaryScreen.WorkingArea.Width - this.Width) / 2;
        y = primaryScreen.WorkingArea.Bottom - this.Height - margin;
        break;
    }

    this.Location = new Point(x, y);
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
        ResizeFormForState(state);
        panelMain.Invalidate();
        break;

      case OverlayState.Listening:
        ResizeFormForState(state);
        StartDotAnimation();
        panelMain.Invalidate();
        break;

      case OverlayState.Recognizing:
        StopDotAnimation();
        ResizeFormForState(state);
        // Hide text label - text is not shown in the bubble
        if (lblMainText != null)
        {
          lblMainText.Visible = false;
        }
        panelMain.Invalidate();
        break;
    }
  }

  private void ResizeFormForState(OverlayState state)
  {
    int width = 60;
    int height;
    
    if (state == OverlayState.Idle)
    {
      height = 10; // Very thin pill for idle state
    }
    else if (state == OverlayState.Listening)
    {
      height = 24; // Slightly taller to accommodate centered animation
    }
    else // Recognizing
    {
      height = 20; // Small pill height
    }
    
    this.Size = new Size(width, height);
    MakeRounded(); // Recreate rounded region for new size
    UpdateOverlayPosition(); // Reposition after resize
  }

  public void SetRecognizedText(string text)
  {
    // Don't show text inside the bubble - keep it as a small pill
    // Text display is handled elsewhere if needed
    if (lblMainText != null)
    {
      lblMainText.Visible = false;
    }
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
    
    // Thin border around entire pill - white outer, black inner
    int r = Math.Min(panelMain.Height / 2, 5);
    float borderWidth = 1f; // Thin border
    
    // White border (outer) - covers entire area including all edges
    using (var outer = new System.Drawing.Drawing2D.GraphicsPath())
    {
      // Top-left corner
      outer.AddArc(0, 0, r * 2, r * 2, 180, 90);
      // Top edge
      outer.AddLine(r, 0, panelMain.Width - r, 0);
      // Top-right corner
      outer.AddArc(panelMain.Width - r * 2, 0, r * 2, r * 2, 270, 90);
      // Right edge
      outer.AddLine(panelMain.Width, r, panelMain.Width, panelMain.Height - r);
      // Bottom-right corner
      outer.AddArc(panelMain.Width - r * 2, panelMain.Height - r * 2, r * 2, r * 2, 0, 90);
      // Bottom edge
      outer.AddLine(panelMain.Width - r, panelMain.Height, r, panelMain.Height);
      // Bottom-left corner
      outer.AddArc(0, panelMain.Height - r * 2, r * 2, r * 2, 90, 90);
      // Left edge
      outer.AddLine(0, panelMain.Height - r, 0, r);
      outer.CloseAllFigures();
      g.FillPath(new SolidBrush(Color.White), outer);
    }
    
    // Black fill (inner) - creates thin border effect, offset by borderWidth on all sides
    using (var inner = new System.Drawing.Drawing2D.GraphicsPath())
    {
      float offset = borderWidth;
      float innerWidth = panelMain.Width - (offset * 2);
      float innerHeight = panelMain.Height - (offset * 2);
      int innerR = Math.Max(0, r - (int)borderWidth);
      
      // Top-left corner
      inner.AddArc(offset, offset, innerR * 2, innerR * 2, 180, 90);
      // Top edge
      inner.AddLine(offset + innerR, offset, offset + innerWidth - innerR, offset);
      // Top-right corner
      inner.AddArc(offset + innerWidth - innerR * 2, offset, innerR * 2, innerR * 2, 270, 90);
      // Right edge
      inner.AddLine(offset + innerWidth, offset + innerR, offset + innerWidth, offset + innerHeight - innerR);
      // Bottom-right corner
      inner.AddArc(offset + innerWidth - innerR * 2, offset + innerHeight - innerR * 2, innerR * 2, innerR * 2, 0, 90);
      // Bottom edge
      inner.AddLine(offset + innerWidth - innerR, offset + innerHeight, offset + innerR, offset + innerHeight);
      // Bottom-left corner
      inner.AddArc(offset, offset + innerHeight - innerR * 2, innerR * 2, innerR * 2, 90, 90);
      // Left edge
      inner.AddLine(offset, offset + innerHeight - innerR, offset, offset + innerR);
      inner.CloseAllFigures();
      g.FillPath(new SolidBrush(Color.Black), inner);
    }
    
    // Draw waves when listening
    if (currentState == OverlayState.Listening)
    {
      int centerY = panelMain.Height / 2;
      int centerX = panelMain.Width / 2;
      int spacing = 6;
      int startX = centerX - (waveHeights.Length - 1) * spacing / 2;
      
      using (var pen = new Pen(Color.White, 2))
      {
        for (int i = 0; i < waveHeights.Length; i++)
        {
          int h = Math.Min(waveHeights[i], panelMain.Height - 4);
          g.DrawLine(pen, startX + i * spacing, centerY - h / 2, startX + i * spacing, centerY + h / 2);
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
