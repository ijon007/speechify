using Microsoft.Data.SqlClient;
using System.Runtime.InteropServices;

namespace WinFormTest;

public partial class DashboardForm : Form
{
  private string username;
  private SpeechRecognitionService? speechService;
  private GlobalHotkeyManager? hotkeyManager;
  private TextInjectionService? textInjectionService;
  private SpeechOverlayForm? overlayForm;
  private string? recognizedText;
  private DatabaseService? databaseService;
  private Panel? loadingPanel;
  private Label? loadingLabel;
  private Panel? activePagePanel;
  private Label? activeNavItem;
  private DateTime? recordingStartTime;

  public DashboardForm(string username)
  {
    this.username = username;
    InitializeComponent();
    SetupRoundedCornersForPages();
    SetupWindowControls();
    
    // Prevent focus rectangles on buttons
    btnClose.TabStop = false;
    btnMinimize.TabStop = false;
    
    // Set custom icon from assets folder
    string iconPath = Path.Combine(Application.StartupPath, "assets", "cp-black.ico");
    if (File.Exists(iconPath))
    {
      this.Icon = new Icon(iconPath);
      // Load logo into PictureBox
      try
      {
        using (Icon icon = new Icon(iconPath, 32, 32))
        {
          picLogo.Image = icon.ToBitmap();
        }
      }
      catch
      {
        // If loading fails, try loading as image directly
        try
        {
          picLogo.Image = Image.FromFile(iconPath);
        }
        catch
        {
          // If both methods fail, leave PictureBox empty
        }
      }
    }
    
    databaseService = new DatabaseService();
    InitializePages();
    LoadDashboardData();
    InitializeLoadingUI();
    InitializeSpeechServicesAsync();
  }

  private void ApplyRoundedCorners(Panel panel, int radius)
  {
    if (panel.Width <= 0 || panel.Height <= 0) return;
    
    System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
    path.AddArc(0, 0, radius * 2, radius * 2, 180, 90);
    path.AddArc(panel.Width - radius * 2, 0, radius * 2, radius * 2, 270, 90);
    path.AddArc(panel.Width - radius * 2, panel.Height - radius * 2, radius * 2, radius * 2, 0, 90);
    path.AddArc(0, panel.Height - radius * 2, radius * 2, radius * 2, 90, 90);
    path.CloseAllFigures();
    panel.Region = new System.Drawing.Region(path);
  }

  private void PanelPage_Paint(object? sender, PaintEventArgs e)
  {
    if (sender is Panel panel)
    {
      DrawRoundedBorder(panel, e.Graphics, 10, Color.FromArgb(200, 200, 200));
    }
  }

  private void DrawRoundedBorder(Panel panel, Graphics g, int radius, Color borderColor)
  {
    using (Pen pen = new Pen(borderColor, 1))
    {
      // Draw rounded rectangle border
      System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
      Rectangle rect = new Rectangle(0, 0, panel.Width - 1, panel.Height - 1);
      
      path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
      path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
      path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
      path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
      path.CloseAllFigures();
      
      g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
      g.DrawPath(pen, path);
    }
  }

  private void PanelPage_Resize(object? sender, EventArgs e)
  {
    if (sender is Panel panel)
    {
      ApplyRoundedCorners(panel, 10);
      
      // Ensure panelSpeechHistory stays within homePage border bounds
      if (panel == panelHomePage && panelSpeechHistory != null)
      {
        int paddingLeft = 40;
        int paddingRight = 40;
        int paddingTop = 60;
        int paddingBottom = 50;
        int speechHistoryTop = 220; // Y position of panelSpeechHistory
        
        // Calculate available width and height accounting for padding
        int availableWidth = panel.ClientSize.Width - paddingLeft - paddingRight;
        // Available height = total height - top padding - space above speech history - bottom padding
        int availableHeight = panel.ClientSize.Height - paddingTop - (speechHistoryTop - paddingTop) - paddingBottom;
        
        // Update panelSpeechHistory size to stay within bounds
        panelSpeechHistory.Width = availableWidth;
        panelSpeechHistory.Height = Math.Max(0, availableHeight);
      }
    }
  }

  private void SetupRoundedCornersForPages()
  {
    // Apply rounded corners to all page panels
    Panel[] pagePanels = { panelHomePage, panelDictionaryPage, panelSnippetsPage, panelStylePage, panelSettingsPage };
    
    foreach (Panel panel in pagePanels)
    {
      panel.Resize += PanelPage_Resize;
      panel.Paint += PanelPage_Paint;
      ApplyRoundedCorners(panel, 10);
    }
  }

  private void InitializeLoadingUI()
  {
    // Create loading panel
    loadingPanel = new Panel();
    loadingPanel.Dock = DockStyle.Fill;
    loadingPanel.BackColor = Color.FromArgb(45, 45, 48);
    loadingPanel.Visible = true;
    loadingPanel.BringToFront();

    // Create loading label
    loadingLabel = new Label();
    loadingLabel.Text = "Loading speech recognition model...\nPlease wait...";
    loadingLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
    loadingLabel.ForeColor = Color.White;
    loadingLabel.AutoSize = false;
    loadingLabel.TextAlign = ContentAlignment.MiddleCenter;
    loadingLabel.Dock = DockStyle.Fill;
    loadingLabel.Padding = new Padding(20);

    loadingPanel.Controls.Add(loadingLabel);
    this.Controls.Add(loadingPanel);
    loadingPanel.BringToFront();
  }

  private async void InitializeSpeechServicesAsync()
  {
    try
    {
      // Initialize speech recognition service (model loading happens async)
      speechService = new SpeechRecognitionService();
      speechService.SpeechRecognized += SpeechService_SpeechRecognized;
      speechService.SpeechPartialResult += SpeechService_SpeechPartialResult;

      // Initialize text injection service
      textInjectionService = new TextInjectionService();

      // Initialize hotkey manager
      hotkeyManager = new GlobalHotkeyManager(this.Handle);
      hotkeyManager.HotkeyPressed += HotkeyManager_HotkeyPressed;
      hotkeyManager.HotkeyReleased += HotkeyManager_HotkeyReleased;
      hotkeyManager.InstallKeyboardHook();

      // Initialize overlay form
      overlayForm = new SpeechOverlayForm();
      overlayForm.Hide();

      // Load model asynchronously
      UpdateLoadingText("Loading Vosk model...");
      await speechService.InitializeAsync();
      
      // Hide loading panel once model is loaded
      if (loadingPanel != null)
      {
        loadingPanel.Visible = false;
        loadingPanel.Dispose();
        loadingPanel = null;
      }
    }
    catch (Exception ex)
    {
      if (loadingPanel != null)
      {
        loadingPanel.Visible = false;
        loadingPanel.Dispose();
        loadingPanel = null;
      }
      MessageBox.Show($"Failed to initialize speech services: {ex.Message}", "Error", 
        MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
  }

  private void UpdateLoadingText(string text)
  {
    if (loadingLabel != null && loadingLabel.InvokeRequired)
    {
      loadingLabel.Invoke(new Action(() => loadingLabel.Text = text));
    }
    else if (loadingLabel != null)
    {
      loadingLabel.Text = text;
    }
  }

  protected override void WndProc(ref Message m)
  {
    // Handle hotkey messages
    if (m.Msg == WindowsApiHelper.WM_HOTKEY)
    {
      hotkeyManager?.ProcessMessage(m.Msg, m.WParam, m.LParam);
    }
    // Prevent focus border from showing on borderless form
    else if (m.Msg == WindowsApiHelper.WM_NCACTIVATE)
    {
      m.Result = new IntPtr(1); // Always return true to prevent focus border
      return;
    }
    base.WndProc(ref m);
  }
  
  protected override bool ShowFocusCues => false;

  private void HotkeyManager_HotkeyPressed(object? sender, EventArgs e)
  {
    if (speechService == null || overlayForm == null || textInjectionService == null)
      return;

    // Check if model is loaded
    if (!speechService.IsModelLoaded)
    {
      MessageBox.Show("Speech recognition model is still loading. Please wait...", "Loading",
        MessageBoxButtons.OK, MessageBoxIcon.Information);
      return;
    }

    try
    {
      recognizedText = null;
      recordingStartTime = DateTime.Now;
      
      // Save the foreground window BEFORE showing overlay
      // This ensures we capture the window that was active when user pressed hotkey
      IntPtr activeWindow = WindowsApiHelper.GetForegroundWindow();
      textInjectionService.SaveForegroundWindow(activeWindow);
      
      speechService.StartListening();
      overlayForm.SetState(SpeechOverlayForm.OverlayState.Listening);
      overlayForm.Show();
    }
    catch (Exception ex)
    {
      MessageBox.Show($"Failed to start speech recognition: {ex.Message}", "Error",
        MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
  }

  private async void HotkeyManager_HotkeyReleased(object? sender, EventArgs e)
  {
    if (speechService == null || overlayForm == null || textInjectionService == null)
      return;

    try
    {
      speechService.StopListening();
      
      // Wait a short time to allow pending recognition events to complete
      // Recognition events can fire asynchronously after StopListening()
      // Increased delay to 1000ms to give more time for recognition processing
      await Task.Delay(1000);
      
      // Calculate duration if we have a start time
      int? duration = null;
      if (recordingStartTime.HasValue)
      {
        duration = (int)(DateTime.Now - recordingStartTime.Value).TotalMilliseconds;
      }
      
      // Inject recognized text if available
      if (!string.IsNullOrEmpty(recognizedText))
      {
        textInjectionService.InjectText(recognizedText);
        SaveSpeechToDatabase(recognizedText, duration);
        AddSpeechToHistory(recognizedText);
      }

      overlayForm.Hide();
      recognizedText = null;
      recordingStartTime = null;
    }
    catch (Exception ex)
    {
      MessageBox.Show($"Failed to stop speech recognition: {ex.Message}", "Error",
        MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
  }

  private void SpeechService_SpeechPartialResult(object? sender, string text)
  {
    // Partial results are for real-time display only - don't accumulate
    // Show current accumulated text + partial result for preview
    if (overlayForm != null && !string.IsNullOrWhiteSpace(text))
    {
      string displayText = string.IsNullOrEmpty(recognizedText) 
        ? text 
        : recognizedText + " " + text;
      overlayForm.SetRecognizedText(displayText);
    }
  }

  private void SpeechService_SpeechRecognized(object? sender, string text)
  {
    if (overlayForm == null)
      return;

    // Accumulate final recognized text (append with space if there's existing text)
    if (string.IsNullOrEmpty(recognizedText))
    {
      recognizedText = text;
    }
    else
    {
      recognizedText = recognizedText + " " + text;
    }
    
    overlayForm.SetRecognizedText(recognizedText);
  }

  private void SetupWindowControls()
  {
    // Enable dragging from top ribbon panel (acts like native title bar)
    panelTopRibbon.MouseDown += PanelTopRibbon_MouseDown;
    
    // Ensure buttons stay on top and are clickable
    btnClose.BringToFront();
    btnMinimize.BringToFront();
  }
  
  private void PanelTopRibbon_MouseDown(object? sender, MouseEventArgs e)
  {
    if (e.Button == MouseButtons.Left)
    {
      // Don't drag if clicking directly on buttons
      if (sender == btnClose || sender == btnMinimize)
        return;
      
      // Convert to form coordinates to check button positions
      Control? sourceControl = sender as Control;
      if (sourceControl != null)
      {
        Point formPoint = sourceControl.PointToScreen(e.Location);
        formPoint = this.PointToClient(formPoint);
        
        // Allow dragging everywhere except button area (x >= 1140)
        if (formPoint.X < 1140)
        {
          WindowsApiHelper.ReleaseCapture();
          WindowsApiHelper.SendMessage(this.Handle, WindowsApiHelper.WM_NCLBUTTONDOWN, 
            WindowsApiHelper.HT_CAPTION, 0);
        }
      }
    }
  }

  private void DashboardForm_MouseDown(object? sender, MouseEventArgs e)
  {
    // This method is kept for backward compatibility but is no longer the primary handler
    // The top ribbon panel now handles its own dragging via PanelTopRibbon_MouseDown
  }

  private void btnClose_Click(object? sender, EventArgs e)
  {
    // Remove focus from button before closing
    this.Focus();
    this.Close();
  }

  private void btnClose_MouseEnter(object? sender, EventArgs e)
  {
    btnClose.ForeColor = Color.White;
    btnClose.BackColor = Color.FromArgb(232, 17, 35);
  }

  private void btnClose_MouseLeave(object? sender, EventArgs e)
  {
    btnClose.ForeColor = Color.FromArgb(100, 100, 100);
    btnClose.BackColor = Color.Transparent;
  }

  private void btnMinimize_Click(object? sender, EventArgs e)
  {
    // Remove focus from button before minimizing
    this.Focus();
    this.WindowState = FormWindowState.Minimized;
  }

  private void btnMinimize_MouseEnter(object? sender, EventArgs e)
  {
    btnMinimize.ForeColor = Color.FromArgb(45, 45, 48);
    btnMinimize.BackColor = Color.FromArgb(240, 240, 240);
  }

  private void btnMinimize_MouseLeave(object? sender, EventArgs e)
  {
    btnMinimize.ForeColor = Color.FromArgb(100, 100, 100);
    btnMinimize.BackColor = Color.Transparent;
  }

  private void LoadDashboardData()
  {
    // Set welcome message with username
    lblWelcome.Text = $"Welcome back, {username}!";
    
    // Load sample speech entries
    LoadSpeechHistory();
    
    // Load and display statistics
    RefreshStats();
  }

  private void RefreshStats()
  {
    if (databaseService == null)
      return;

    try
    {
      // Get consecutive weeks streak
      int weeksStreak = databaseService.GetConsecutiveWeeksStreak(username);
      lblStatWeeks.Text = weeksStreak == 1 ? "🔥 1 week" : $"🔥 {weeksStreak} weeks";

      // Get total words
      int totalWords = databaseService.GetTotalWords(username);
      if (totalWords >= 1000)
      {
        double wordsK = totalWords / 1000.0;
        lblStatWords.Text = $"🚀 {wordsK:F1}K words";
      }
      else
      {
        lblStatWords.Text = $"🚀 {totalWords} words";
      }

      // Get average WPM
      int averageWPM = databaseService.GetAverageWPM(username);
      lblStatWPM.Text = $"🏆 {averageWPM} WPM";
    }
    catch (Exception ex)
    {
      System.Diagnostics.Debug.WriteLine($"Failed to refresh stats: {ex.Message}");
      // Set default values on error
      lblStatWeeks.Text = "🔥 0 weeks";
      lblStatWords.Text = "🚀 0 words";
      lblStatWPM.Text = "🏆 0 WPM";
    }
  }

  private void LoadSpeechHistory()
  {
    if (databaseService == null)
      return;

    var speeches = databaseService.GetSpeeches(username);

    int yOffset = 10;
    const int spacingBetweenItems = 15; // Reduced spacing between transcription items
    const int fixedPanelHeight = 70; // Fixed height for each item (smaller)
    
    foreach (var speech in speeches)
    {
      CreateSpeechRow(speech.id, speech.time, speech.text, yOffset);
      
      // Calculate next position: fixed panel height + spacing between items
      yOffset += fixedPanelHeight + spacingBetweenItems;
    }
  }

  // Sidebar navigation hover effects
  private void navItem_MouseEnter(object? sender, EventArgs e)
  {
    if (sender is Label label)
    {
      label.BackColor = Color.FromArgb(245, 245, 245);
      label.ForeColor = Color.Black;
    }
  }

  private void navItem_MouseLeave(object? sender, EventArgs e)
  {
    if (sender is Label label)
    {
      // Only restore gray if it's not the active navigation item
      if (activeNavItem == null || label != activeNavItem)
      {
        label.BackColor = Color.Transparent;
        label.ForeColor = Color.FromArgb(100, 100, 100);
      }
    }
  }

  private void SaveSpeechToDatabase(string text, int? duration = null)
  {
    if (databaseService != null && !string.IsNullOrWhiteSpace(text))
    {
      databaseService.SaveSpeech(username, text, duration);
    }
  }

  private string TruncateText(string text, Font font, int maxWidth, int maxHeight)
  {
    if (string.IsNullOrEmpty(text))
      return text;

    // Create a temporary label to measure text accurately
    using (Label tempLabel = new Label())
    {
      tempLabel.Font = font;
      tempLabel.Text = text;
      tempLabel.AutoSize = true;
      tempLabel.MaximumSize = new Size(maxWidth, 0);
      
      // If text fits within maxHeight, return as is
      if (tempLabel.Height <= maxHeight)
        return text;

      // Text is too long, need to truncate
      string ellipsis = "...";
      
      // Binary search approach: find the maximum length that fits
      int minLength = 0;
      int maxLength = text.Length;
      
      while (minLength < maxLength)
      {
        int midLength = (minLength + maxLength + 1) / 2;
        string testText = text.Substring(0, midLength) + ellipsis;
        tempLabel.Text = testText;
        
        if (tempLabel.Height <= maxHeight)
        {
          minLength = midLength;
        }
        else
        {
          maxLength = midLength - 1;
        }
      }
      
      if (minLength == 0)
        return ellipsis;
      
      return text.Substring(0, minLength) + ellipsis;
    }
  }

  private void CreateSpeechRow(int id, string time, string text, int yOffset)
  {
    // Calculate available width accounting for padding and scrollbar
    int availableWidth = panelSpeechHistory.ClientSize.Width - 20; // 10px padding on each side
    
    // Fixed panel height for smaller items
    const int fixedPanelHeight = 70;

    // Create container panel for this speech entry
    ClippingPanel entryPanel = new ClippingPanel();
    entryPanel.BackColor = Color.White;
    entryPanel.Location = new Point(10, yOffset);
    entryPanel.Size = new Size(availableWidth, fixedPanelHeight);
    entryPanel.Name = $"panelEntry_{id}";
    entryPanel.Paint += EntryPanel_Paint;
    entryPanel.AutoSize = false;
    entryPanel.AutoSizeMode = AutoSizeMode.GrowOnly;

    // Create timestamp label
    Label lblTime = new Label();
    lblTime.Text = time;
    lblTime.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
    lblTime.ForeColor = Color.FromArgb(100, 100, 100);
    lblTime.BackColor = Color.White;
    lblTime.Location = new Point(10, 10);
    lblTime.AutoSize = true;
    lblTime.Name = $"lblTime_{id}";

    // Create text label - ensure it doesn't overlap with button
    // Button is at Width - 50, button width is 30, so text max width = Width - 50 - 30 - 10 (spacing) - 10 (left padding)
    int textMaxWidth = entryPanel.Width - 100;
    // Calculate available height: panel height (70) - timestamp top (10) - timestamp height (~15) - text top margin (5) - bottom padding (10)
    int textMaxHeight = fixedPanelHeight - 30 - 10; // ~30px for 2 lines max
    Font textFont = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
    
    // Truncate text if needed
    string displayText = TruncateText(text, textFont, textMaxWidth, textMaxHeight);
    
    Label lblText = new Label();
    lblText.Text = displayText;
    lblText.Font = textFont;
    lblText.ForeColor = Color.FromArgb(45, 45, 48);
    lblText.BackColor = Color.White;
    lblText.Location = new Point(10, 30);
    lblText.AutoSize = false;
    lblText.Size = new Size(textMaxWidth, textMaxHeight);
    lblText.Name = $"lblText_{id}";
    lblText.UseCompatibleTextRendering = false;
    lblText.TextAlign = ContentAlignment.TopLeft;
    // Ensure text wraps and doesn't overflow
    lblText.MaximumSize = new Size(textMaxWidth, textMaxHeight);
    // Clip the label to its bounds
    System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
    path.AddRectangle(new Rectangle(0, 0, textMaxWidth, textMaxHeight));
    lblText.Region = new System.Drawing.Region(path);

    // Create copy button
    Button btnCopy = new Button();
    btnCopy.Text = "📋";
    btnCopy.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
    btnCopy.Size = new Size(30, 30);
    btnCopy.FlatStyle = FlatStyle.Flat;
    btnCopy.FlatAppearance.BorderSize = 0;
    btnCopy.BackColor = Color.FromArgb(245, 245, 245);
    btnCopy.ForeColor = Color.FromArgb(100, 100, 100);
    btnCopy.Cursor = Cursors.Hand;
    btnCopy.Name = $"btnCopy_{id}";
    
    // Position button next to the text (right side)
    int buttonX = entryPanel.Width - 50;
    int buttonY = 30;
    btnCopy.Location = new Point(buttonX, buttonY);
    
    // Store the text in Tag for easy access
    btnCopy.Tag = text;
    btnCopy.Click += BtnCopy_Click;
    btnCopy.MouseEnter += BtnCopy_MouseEnter;
    btnCopy.MouseLeave += BtnCopy_MouseLeave;

    // Add controls to entry panel
    entryPanel.Controls.Add(lblTime);
    entryPanel.Controls.Add(lblText);
    entryPanel.Controls.Add(btnCopy);

    // Add entry panel to speech history panel
    panelSpeechHistory.Controls.Add(entryPanel);
  }

  private void EntryPanel_Paint(object? sender, PaintEventArgs e)
  {
    if (sender is Panel panel)
    {
      // Draw only bottom border with no rounding
      using (Pen pen = new Pen(Color.FromArgb(200, 200, 200), 1))
      {
        e.Graphics.DrawLine(pen, 0, panel.Height - 1, panel.Width, panel.Height - 1);
      }
    }
  }

  private void BtnCopy_Click(object? sender, EventArgs e)
  {
    if (sender is Button btn && btn.Tag is string text)
    {
      try
      {
        Clipboard.SetText(text);
        // Optional: Show brief feedback
        btn.Text = "✓";
        Task.Delay(1000).ContinueWith(_ =>
        {
          if (btn.InvokeRequired)
          {
            btn.Invoke(new Action(() => btn.Text = "📋"));
          }
          else
          {
            btn.Text = "📋";
          }
        });
      }
      catch
      {
        // Ignore clipboard errors
      }
    }
  }

  private void BtnCopy_MouseEnter(object? sender, EventArgs e)
  {
    if (sender is Button btn)
    {
      btn.BackColor = Color.FromArgb(230, 230, 230);
    }
  }

  private void BtnCopy_MouseLeave(object? sender, EventArgs e)
  {
    if (sender is Button btn)
    {
      btn.BackColor = Color.FromArgb(245, 245, 245);
    }
  }

  public void AddSpeechToHistory(string text)
  {
    if (string.IsNullOrEmpty(text))
      return;

    // Get current time
    string time = DateTime.Now.ToString("hh:mm tt");
    
    // Use a temporary ID for new entries (they'll get real IDs from DB on next load)
    int tempId = (int)DateTime.Now.Ticks;

    // Calculate position (add at top)
    int yOffset = 10;
    const int spacingBetweenItems = 15; // Reduced spacing between transcription items
    const int fixedPanelHeight = 70; // Fixed height for each item

    // Calculate the height of the new item to move existing controls down
    int newItemHeight = fixedPanelHeight + spacingBetweenItems;
    
    // Move existing controls down by the height of the new item
    foreach (Control control in panelSpeechHistory.Controls)
    {
      control.Location = new Point(control.Location.X, control.Location.Y + newItemHeight);
    }

    // Create the new speech row
    CreateSpeechRow(tempId, time, text, yOffset);

    // Scroll to top
    panelSpeechHistory.AutoScrollPosition = new Point(0, 0);
    
    // Refresh statistics after adding new speech
    RefreshStats();
  }

  private void InitializePages()
  {
    // Set up placeholder content for other pages
    CreatePlaceholderPage(panelDictionaryPage, "Dictionary");
    CreatePlaceholderPage(panelSnippetsPage, "Snippets");
    CreatePlaceholderPage(panelStylePage, "Style");
    CreatePlaceholderPage(panelSettingsPage, "Settings");
    
    // Initially hide all pages except Home
    panelDictionaryPage.Visible = false;
    panelSnippetsPage.Visible = false;
    panelStylePage.Visible = false;
    panelSettingsPage.Visible = false;
    panelHomePage.Visible = true;
    
    // Set Home as default active page
    activePagePanel = panelHomePage;
    activeNavItem = lblNavHome;
    SetActiveNavItem(lblNavHome);
  }

  private void CreatePlaceholderPage(Panel panel, string title)
  {
    panel.BackColor = Color.White;
    panel.Dock = DockStyle.Fill;
    panel.Padding = new Padding(40, 60, 40, 40);
    
    Label lblTitle = new Label();
    lblTitle.Text = title;
    lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point);
    lblTitle.ForeColor = Color.FromArgb(45, 45, 48);
    lblTitle.Location = new Point(40, 60);
    lblTitle.AutoSize = true;
    lblTitle.Name = $"lbl{title}Title";
    
    Label lblPlaceholder = new Label();
    lblPlaceholder.Text = $"This is the {title} page.\nContent will be added here.";
    lblPlaceholder.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
    lblPlaceholder.ForeColor = Color.FromArgb(100, 100, 100);
    lblPlaceholder.Location = new Point(40, 120);
    lblPlaceholder.AutoSize = true;
    lblPlaceholder.Name = $"lbl{title}Placeholder";
    
    panel.Controls.Add(lblTitle);
    panel.Controls.Add(lblPlaceholder);
  }

  private void SwitchPage(Panel targetPage, Label activeNavLabel)
  {
    // Hide all page panels
    panelHomePage.Visible = false;
    panelDictionaryPage.Visible = false;
    panelSnippetsPage.Visible = false;
    panelStylePage.Visible = false;
    panelSettingsPage.Visible = false;
    
    // Show target page
    targetPage.Visible = true;
    activePagePanel = targetPage;
    
    // Update active navigation item
    SetActiveNavItem(activeNavLabel);
  }

  private void SetActiveNavItem(Label navItem)
  {
    // Reset all navigation items to inactive state
    lblNavHome.ForeColor = Color.FromArgb(100, 100, 100);
    lblNavHome.BackColor = Color.Transparent;
    lblNavDictionary.ForeColor = Color.FromArgb(100, 100, 100);
    lblNavDictionary.BackColor = Color.Transparent;
    lblNavSnippets.ForeColor = Color.FromArgb(100, 100, 100);
    lblNavSnippets.BackColor = Color.Transparent;
    lblNavStyle.ForeColor = Color.FromArgb(100, 100, 100);
    lblNavStyle.BackColor = Color.Transparent;
    lblNavSettings.ForeColor = Color.FromArgb(100, 100, 100);
    lblNavSettings.BackColor = Color.Transparent;
    
    // Set active navigation item styling
    navItem.ForeColor = Color.Black;
    navItem.BackColor = Color.FromArgb(245, 245, 245);
    activeNavItem = navItem;
  }

  private void navItem_Click(object? sender, EventArgs e)
  {
    if (sender is Label label)
    {
      if (label == lblNavHome)
      {
        SwitchPage(panelHomePage, lblNavHome);
      }
      else if (label == lblNavDictionary)
      {
        SwitchPage(panelDictionaryPage, lblNavDictionary);
      }
      else if (label == lblNavSnippets)
      {
        SwitchPage(panelSnippetsPage, lblNavSnippets);
      }
      else if (label == lblNavStyle)
      {
        SwitchPage(panelStylePage, lblNavStyle);
      }
      else if (label == lblNavSettings)
      {
        SwitchPage(panelSettingsPage, lblNavSettings);
      }
    }
  }

  protected override void OnFormClosing(FormClosingEventArgs e)
  {
    // Clean up services
    speechService?.Dispose();
    hotkeyManager?.Dispose();
    overlayForm?.Close();
    
    base.OnFormClosing(e);
  }
}
