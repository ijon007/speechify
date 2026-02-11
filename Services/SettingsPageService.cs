using System.Linq;
using WinFormTest;

namespace WinFormTest.Services;

public class SettingsPageService
{
    private readonly DatabaseService? databaseService;
    private readonly GlobalHotkeyManager? hotkeyManager;
    private readonly SpeechRecognitionService? speechService;
    private readonly SpeechOverlayForm? overlayForm;
    private readonly DictionaryPageService? dictionaryPageService;
    private readonly SnippetsPageService? snippetsPageService;
    private readonly TranscriptionCorrectionService? transcriptionCorrectionService;
    private readonly string username;
    private readonly Form parentForm;
    private readonly Panel panelSettingsPage;
    
    private Panel? panelSettingsContent;
    private Label? lblSettingsTitle;
    private Label? lblHotkeySectionTitle;
    private Label? lblCurrentHotkey;
    private Label? lblCurrentHotkeyValue;
    private Button? btnChangeHotkey;
    private Label? lblHotkeyDescription;
    private Label? lblMicrophoneSectionTitle;
    private Label? lblMicrophoneDevice;
    private ComboBox? cmbMicrophoneDevice;
    private Label? lblOverlaySectionTitle;
    private Label? lblOverlayPosition;
    private ComboBox? cmbOverlayPosition;
    private Label? lblApplicationSectionTitle;
    private CheckBox? chkStartMinimized;
    private CheckBox? chkMinimizeToTray;
    private Label? lblDataSectionTitle;
    private Button? btnClearSpeechHistory;
    private Button? btnClearDictionary;
    private Button? btnClearSnippets;
    private Button? btnExportData;

    public SettingsPageService(
        DatabaseService? databaseService,
        GlobalHotkeyManager? hotkeyManager,
        SpeechRecognitionService? speechService,
        SpeechOverlayForm? overlayForm,
        DictionaryPageService? dictionaryPageService,
        SnippetsPageService? snippetsPageService,
        TranscriptionCorrectionService? transcriptionCorrectionService,
        string username,
        Form parentForm,
        Panel panelSettingsPage)
    {
        this.databaseService = databaseService;
        this.hotkeyManager = hotkeyManager;
        this.speechService = speechService;
        this.overlayForm = overlayForm;
        this.dictionaryPageService = dictionaryPageService;
        this.snippetsPageService = snippetsPageService;
        this.transcriptionCorrectionService = transcriptionCorrectionService;
        this.username = username;
        this.parentForm = parentForm;
        this.panelSettingsPage = panelSettingsPage;
    }

    public void Initialize()
    {
        const int paddingLeft = 40;
        const int paddingTop = 60;
        const int paddingRight = 40;
        const int paddingBottom = 50;
        const int sectionSpacing = 44;
        const int labelGap = 10;
        const int afterSectionTitle = 34;
        const int afterSubLabel = 28;
        const int afterControl = 10;
        int y = 0;

        panelSettingsPage.BackColor = Color.White;
        panelSettingsPage.Dock = DockStyle.Fill;

        // Scrollable content panel - use padding so content isn't clipped at edges
        panelSettingsContent = new Panel();
        panelSettingsContent.AutoScroll = true;
        panelSettingsContent.BackColor = Color.White;
        panelSettingsContent.Padding = new Padding(0, 4, 0, 0);
        panelSettingsContent.Location = new Point(paddingLeft, paddingTop);
        panelSettingsContent.Size = new Size(panelSettingsPage.ClientSize.Width - paddingLeft - paddingRight,
            Math.Max(0, panelSettingsPage.ClientSize.Height - paddingTop - paddingBottom));
        panelSettingsContent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        panelSettingsContent.Name = "panelSettingsContent";

        // Title
        lblSettingsTitle = new Label();
        lblSettingsTitle.Text = "Settings";
        lblSettingsTitle.Font = UIFonts.Title;
        lblSettingsTitle.ForeColor = UIColors.DarkText;
        lblSettingsTitle.Location = new Point(0, y);
        lblSettingsTitle.AutoSize = true;
        lblSettingsTitle.Name = "lblSettingsTitle";
        panelSettingsContent.Controls.Add(lblSettingsTitle);
        y += Math.Max(48, lblSettingsTitle.PreferredHeight + 16);

        // Hotkey section
        lblHotkeySectionTitle = new Label();
        lblHotkeySectionTitle.Text = "Hotkey";
        lblHotkeySectionTitle.Font = UIFonts.Heading;
        lblHotkeySectionTitle.ForeColor = UIColors.DarkText;
        lblHotkeySectionTitle.Location = new Point(0, y);
        lblHotkeySectionTitle.AutoSize = true;
        panelSettingsContent.Controls.Add(lblHotkeySectionTitle);
        y += Math.Max(afterSectionTitle, lblHotkeySectionTitle.PreferredHeight + 8);

        lblCurrentHotkey = new Label();
        lblCurrentHotkey.Text = "Current hotkey";
        lblCurrentHotkey.Font = UIFonts.Medium;
        lblCurrentHotkey.ForeColor = UIColors.SecondaryText;
        lblCurrentHotkey.Location = new Point(0, y);
        lblCurrentHotkey.AutoSize = true;
        panelSettingsContent.Controls.Add(lblCurrentHotkey);
        y += afterSubLabel;

        lblCurrentHotkeyValue = new Label();
        lblCurrentHotkeyValue.Font = UIFonts.Body;
        lblCurrentHotkeyValue.ForeColor = UIColors.DarkText;
        lblCurrentHotkeyValue.Location = new Point(0, y);
        lblCurrentHotkeyValue.AutoSize = true;
        RefreshHotkeyDisplay();
        panelSettingsContent.Controls.Add(lblCurrentHotkeyValue!);
        y += 28 + afterControl;

        btnChangeHotkey = new Button();
        btnChangeHotkey.Text = "Change hotkey";
        btnChangeHotkey.Font = UIFonts.Body;
        btnChangeHotkey.FlatStyle = FlatStyle.Flat;
        btnChangeHotkey.FlatAppearance.BorderSize = 0;
        btnChangeHotkey.BackColor = UIColors.DarkPrimary;
        btnChangeHotkey.ForeColor = Color.White;
        btnChangeHotkey.Cursor = Cursors.Hand;
        btnChangeHotkey.Size = UILayout.StandardButtonSize;
        btnChangeHotkey.Location = new Point(0, y);
        btnChangeHotkey.Click += BtnChangeHotkey_Click;
        panelSettingsContent.Controls.Add(btnChangeHotkey);
        y += 44;

        lblHotkeyDescription = new Label();
        lblHotkeyDescription.Text = "Press the hotkey to start or stop voice input. The overlay shows the current shortcut.";
        lblHotkeyDescription.Font = UIFonts.Small;
        lblHotkeyDescription.ForeColor = UIColors.SecondaryText;
        lblHotkeyDescription.Location = new Point(0, y);
        int descMaxWidth = Math.Max(200, panelSettingsContent.ClientSize.Width - 80);
        lblHotkeyDescription.MaximumSize = new Size(descMaxWidth, 0);
        lblHotkeyDescription.AutoSize = true;
        panelSettingsContent.Controls.Add(lblHotkeyDescription);
        y += Math.Max(lblHotkeyDescription.PreferredHeight + 20, 56) + sectionSpacing;

        // Microphone section
        lblMicrophoneSectionTitle = new Label();
        lblMicrophoneSectionTitle.Text = "Microphone";
        lblMicrophoneSectionTitle.Font = UIFonts.Heading;
        lblMicrophoneSectionTitle.ForeColor = UIColors.DarkText;
        lblMicrophoneSectionTitle.Location = new Point(0, y);
        lblMicrophoneSectionTitle.AutoSize = true;
        panelSettingsContent.Controls.Add(lblMicrophoneSectionTitle);
        y += afterSectionTitle;

        lblMicrophoneDevice = new Label();
        lblMicrophoneDevice.Text = "Device";
        lblMicrophoneDevice.Font = UIFonts.Medium;
        lblMicrophoneDevice.ForeColor = UIColors.SecondaryText;
        lblMicrophoneDevice.Location = new Point(0, y);
        lblMicrophoneDevice.AutoSize = true;
        panelSettingsContent.Controls.Add(lblMicrophoneDevice);
        y += afterSubLabel;

        cmbMicrophoneDevice = new ComboBox();
        cmbMicrophoneDevice.Font = UIFonts.Body;
        cmbMicrophoneDevice.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbMicrophoneDevice.Location = new Point(0, y);
        cmbMicrophoneDevice.Width = 320;
        cmbMicrophoneDevice.Height = 28;
        var mics = SpeechRecognitionService.GetAvailableMicrophones();
        cmbMicrophoneDevice.Items.Add("(Default)");
        foreach (var (num, name) in mics)
            cmbMicrophoneDevice.Items.Add($"{num}: {name}");
        string? savedMic = databaseService?.GetUserMicrophonePreference(username);
        if (!string.IsNullOrEmpty(savedMic) && int.TryParse(savedMic, out int micNum))
        {
            int idx = mics.FindIndex(x => x.deviceNumber == micNum);
            if (idx >= 0) cmbMicrophoneDevice.SelectedIndex = idx + 1;
            else cmbMicrophoneDevice.SelectedIndex = 0;
        }
        else
            cmbMicrophoneDevice.SelectedIndex = 0;
        cmbMicrophoneDevice.SelectedIndexChanged += CmbMicrophoneDevice_SelectedIndexChanged;
        panelSettingsContent.Controls.Add(cmbMicrophoneDevice);
        y += 32 + sectionSpacing;

        // Overlay section
        lblOverlaySectionTitle = new Label();
        lblOverlaySectionTitle.Text = "Overlay position";
        lblOverlaySectionTitle.Font = UIFonts.Heading;
        lblOverlaySectionTitle.ForeColor = UIColors.DarkText;
        lblOverlaySectionTitle.Location = new Point(0, y);
        lblOverlaySectionTitle.AutoSize = true;
        panelSettingsContent.Controls.Add(lblOverlaySectionTitle);
        y += afterSectionTitle;

        lblOverlayPosition = new Label();
        lblOverlayPosition.Text = "Position";
        lblOverlayPosition.Font = UIFonts.Medium;
        lblOverlayPosition.ForeColor = UIColors.SecondaryText;
        lblOverlayPosition.Location = new Point(0, y);
        lblOverlayPosition.AutoSize = true;
        panelSettingsContent.Controls.Add(lblOverlayPosition);
        y += afterSubLabel;

        cmbOverlayPosition = new ComboBox();
        cmbOverlayPosition.Font = UIFonts.Body;
        cmbOverlayPosition.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbOverlayPosition.Location = new Point(0, y);
        cmbOverlayPosition.Width = 220;
        cmbOverlayPosition.Height = 28;
        cmbOverlayPosition.Items.AddRange(new object[] { "Bottom center", "Bottom right", "Bottom left", "Top center", "Top left", "Top right" });
        string overlayPos = databaseService?.GetUserOverlayPosition(username) ?? "bottom_center";
        string[] values = { "bottom_center", "bottom_right", "bottom_left", "top_center", "top_left", "top_right" };
        int overlayIdx = Array.IndexOf(values, overlayPos);
        cmbOverlayPosition.SelectedIndex = overlayIdx >= 0 ? overlayIdx : 0;
        cmbOverlayPosition.SelectedIndexChanged += CmbOverlayPosition_SelectedIndexChanged;
        panelSettingsContent.Controls.Add(cmbOverlayPosition);
        y += 32 + sectionSpacing;

        // Application section
        lblApplicationSectionTitle = new Label();
        lblApplicationSectionTitle.Text = "Application";
        lblApplicationSectionTitle.Font = UIFonts.Heading;
        lblApplicationSectionTitle.ForeColor = UIColors.DarkText;
        lblApplicationSectionTitle.Location = new Point(0, y);
        lblApplicationSectionTitle.AutoSize = true;
        panelSettingsContent.Controls.Add(lblApplicationSectionTitle);
        y += afterSectionTitle;

        chkStartMinimized = new CheckBox();
        chkStartMinimized.Text = "Start minimized";
        chkStartMinimized.Font = UIFonts.Body;
        chkStartMinimized.ForeColor = UIColors.DarkText;
        chkStartMinimized.Location = new Point(0, y);
        chkStartMinimized.AutoSize = true;
        var (startMin, minTray) = databaseService?.GetUserApplicationPreferences(username) ?? (false, false);
        chkStartMinimized.Checked = startMin;
        chkStartMinimized.CheckedChanged += ChkStartMinimized_CheckedChanged;
        panelSettingsContent.Controls.Add(chkStartMinimized);
        y += 32;

        chkMinimizeToTray = new CheckBox();
        chkMinimizeToTray.Text = "Minimize to system tray";
        chkMinimizeToTray.Font = UIFonts.Body;
        chkMinimizeToTray.ForeColor = UIColors.DarkText;
        chkMinimizeToTray.Location = new Point(0, y);
        chkMinimizeToTray.AutoSize = true;
        chkMinimizeToTray.Checked = minTray;
        chkMinimizeToTray.CheckedChanged += ChkMinimizeToTray_CheckedChanged;
        panelSettingsContent.Controls.Add(chkMinimizeToTray);
        y += 44 + sectionSpacing;

        // Data section
        lblDataSectionTitle = new Label();
        lblDataSectionTitle.Text = "Data";
        lblDataSectionTitle.Font = UIFonts.Heading;
        lblDataSectionTitle.ForeColor = UIColors.DarkText;
        lblDataSectionTitle.Location = new Point(0, y);
        lblDataSectionTitle.AutoSize = true;
        panelSettingsContent.Controls.Add(lblDataSectionTitle);
        y += afterSectionTitle + labelGap;

        btnClearSpeechHistory = new Button();
        btnClearSpeechHistory.Text = "Clear speech history";
        btnClearSpeechHistory.Font = UIFonts.Body;
        btnClearSpeechHistory.FlatStyle = FlatStyle.Flat;
        btnClearSpeechHistory.FlatAppearance.BorderSize = 0;
        btnClearSpeechHistory.BackColor = UIColors.LightButtonBackground;
        btnClearSpeechHistory.ForeColor = UIColors.DarkText;
        btnClearSpeechHistory.Cursor = Cursors.Hand;
        btnClearSpeechHistory.Size = new Size(160, 35);
        btnClearSpeechHistory.Location = new Point(0, y);
        btnClearSpeechHistory.Click += BtnClearSpeechHistory_Click;
        panelSettingsContent.Controls.Add(btnClearSpeechHistory);
        y += 42;

        btnClearDictionary = new Button();
        btnClearDictionary.Text = "Clear dictionary";
        btnClearDictionary.Font = UIFonts.Body;
        btnClearDictionary.FlatStyle = FlatStyle.Flat;
        btnClearDictionary.FlatAppearance.BorderSize = 0;
        btnClearDictionary.BackColor = UIColors.LightButtonBackground;
        btnClearDictionary.ForeColor = UIColors.DarkText;
        btnClearDictionary.Cursor = Cursors.Hand;
        btnClearDictionary.Size = new Size(160, 35);
        btnClearDictionary.Location = new Point(0, y);
        btnClearDictionary.Click += BtnClearDictionary_Click;
        panelSettingsContent.Controls.Add(btnClearDictionary);
        y += 42;

        btnClearSnippets = new Button();
        btnClearSnippets.Text = "Clear snippets";
        btnClearSnippets.Font = UIFonts.Body;
        btnClearSnippets.FlatStyle = FlatStyle.Flat;
        btnClearSnippets.FlatAppearance.BorderSize = 0;
        btnClearSnippets.BackColor = UIColors.LightButtonBackground;
        btnClearSnippets.ForeColor = UIColors.DarkText;
        btnClearSnippets.Cursor = Cursors.Hand;
        btnClearSnippets.Size = new Size(160, 35);
        btnClearSnippets.Location = new Point(0, y);
        btnClearSnippets.Click += BtnClearSnippets_Click;
        panelSettingsContent.Controls.Add(btnClearSnippets);
        y += 42;

        btnExportData = new Button();
        btnExportData.Text = "Export data";
        btnExportData.Font = UIFonts.Body;
        btnExportData.FlatStyle = FlatStyle.Flat;
        btnExportData.FlatAppearance.BorderSize = 0;
        btnExportData.BackColor = UIColors.LightButtonBackground;
        btnExportData.ForeColor = UIColors.DarkText;
        btnExportData.Cursor = Cursors.Hand;
        btnExportData.Size = new Size(160, 35);
        btnExportData.Location = new Point(0, y);
        btnExportData.Click += BtnExportData_Click;
        panelSettingsContent.Controls.Add(btnExportData);

        panelSettingsPage.Controls.Add(panelSettingsContent);

        panelSettingsPage.Paint += (s, e) =>
        {
            UIStylingService.DrawRoundedBorder(panelSettingsPage, e.Graphics, 10, Color.FromArgb(200, 200, 200));
            if (panelSettingsContent != null)
                UIStylingService.HideScrollbar(panelSettingsContent);
        };

        panelSettingsPage.Resize += PanelSettingsPage_Resize;
    }

    private void RefreshHotkeyDisplay()
    {
        if (lblCurrentHotkeyValue == null) return;
        var (ctrl, alt, shift, win, keyCode) = databaseService?.GetUserHotkeyPreference(username) ?? (true, false, false, true, null);
        var parts = new List<string>();
        if (ctrl) parts.Add("Ctrl");
        if (alt) parts.Add("Alt");
        if (shift) parts.Add("Shift");
        if (win) parts.Add("Win");
        if (keyCode.HasValue && keyCode.Value != 0)
        {
            if (keyCode.Value == 0x20) parts.Add("Space");
            else parts.Add($"Key {keyCode.Value}");
        }
        lblCurrentHotkeyValue.Text = parts.Count > 0 ? string.Join(" + ", parts) : "Ctrl + Win";
    }

    public void SetControls(
        Panel? panelSettingsContent,
        Label? lblSettingsTitle,
        Label? lblHotkeySectionTitle,
        Label? lblCurrentHotkey,
        Label? lblCurrentHotkeyValue,
        Button? btnChangeHotkey,
        Label? lblHotkeyDescription,
        Label? lblMicrophoneSectionTitle,
        Label? lblMicrophoneDevice,
        ComboBox? cmbMicrophoneDevice,
        Label? lblOverlaySectionTitle,
        Label? lblOverlayPosition,
        ComboBox? cmbOverlayPosition,
        Label? lblApplicationSectionTitle,
        CheckBox? chkStartMinimized,
        CheckBox? chkMinimizeToTray,
        Label? lblDataSectionTitle,
        Button? btnClearSpeechHistory,
        Button? btnClearDictionary,
        Button? btnClearSnippets,
        Button? btnExportData)
    {
        this.panelSettingsContent = panelSettingsContent;
        this.lblSettingsTitle = lblSettingsTitle;
        this.lblHotkeySectionTitle = lblHotkeySectionTitle;
        this.lblCurrentHotkey = lblCurrentHotkey;
        this.lblCurrentHotkeyValue = lblCurrentHotkeyValue;
        this.btnChangeHotkey = btnChangeHotkey;
        this.lblHotkeyDescription = lblHotkeyDescription;
        this.lblMicrophoneSectionTitle = lblMicrophoneSectionTitle;
        this.lblMicrophoneDevice = lblMicrophoneDevice;
        this.cmbMicrophoneDevice = cmbMicrophoneDevice;
        this.lblOverlaySectionTitle = lblOverlaySectionTitle;
        this.lblOverlayPosition = lblOverlayPosition;
        this.cmbOverlayPosition = cmbOverlayPosition;
        this.lblApplicationSectionTitle = lblApplicationSectionTitle;
        this.chkStartMinimized = chkStartMinimized;
        this.chkMinimizeToTray = chkMinimizeToTray;
        this.lblDataSectionTitle = lblDataSectionTitle;
        this.btnClearSpeechHistory = btnClearSpeechHistory;
        this.btnClearDictionary = btnClearDictionary;
        this.btnClearSnippets = btnClearSnippets;
        this.btnExportData = btnExportData;
        
        // Wire up event handlers if controls are provided
        if (btnChangeHotkey != null)
        {
            btnChangeHotkey.Click += BtnChangeHotkey_Click;
        }
        if (cmbMicrophoneDevice != null)
        {
            cmbMicrophoneDevice.SelectedIndexChanged += CmbMicrophoneDevice_SelectedIndexChanged;
        }
        if (cmbOverlayPosition != null)
        {
            cmbOverlayPosition.SelectedIndexChanged += CmbOverlayPosition_SelectedIndexChanged;
        }
        if (chkStartMinimized != null)
        {
            chkStartMinimized.CheckedChanged += ChkStartMinimized_CheckedChanged;
        }
        if (chkMinimizeToTray != null)
        {
            chkMinimizeToTray.CheckedChanged += ChkMinimizeToTray_CheckedChanged;
        }
        if (btnClearSpeechHistory != null)
        {
            btnClearSpeechHistory.Click += BtnClearSpeechHistory_Click;
        }
        if (btnClearDictionary != null)
        {
            btnClearDictionary.Click += BtnClearDictionary_Click;
        }
        if (btnClearSnippets != null)
        {
            btnClearSnippets.Click += BtnClearSnippets_Click;
        }
        if (btnExportData != null)
        {
            btnExportData.Click += BtnExportData_Click;
        }
    }

    public CheckBox? GetMinimizeToTrayCheckbox()
    {
        return chkMinimizeToTray;
    }

    private void PanelSettingsPage_Resize(object? sender, EventArgs e)
    {
        if (panelSettingsPage != null && panelSettingsContent != null)
        {
            int paddingLeft = 40;
            int paddingRight = 40;
            int paddingTop = 60;
            int paddingBottom = 50;
            int availableHeight = panelSettingsPage.ClientSize.Height - paddingTop - paddingBottom;
            int availableWidth = panelSettingsPage.ClientSize.Width - paddingLeft - paddingRight;
            
            panelSettingsContent.Height = Math.Max(0, availableHeight);
            panelSettingsContent.Top = paddingTop;
            panelSettingsContent.Left = paddingLeft;
            panelSettingsContent.Width = Math.Max(0, availableWidth);
            
            UIStylingService.HideScrollbar(panelSettingsContent);
        }
        
        if (lblHotkeyDescription != null && panelSettingsContent != null)
        {
            int maxWidth = panelSettingsContent.ClientSize.Width - 80;
            lblHotkeyDescription.MaximumSize = new Size(maxWidth, 0);
        }
    }

    private void BtnChangeHotkey_Click(object? sender, EventArgs e)
    {
        MessageBox.Show("Hotkey is currently fixed to Ctrl + Win. Custom hotkey selection may be added in a future update.",
            "Hotkey", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void CmbMicrophoneDevice_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cmbMicrophoneDevice == null || databaseService == null) return;
        string? deviceId = null;
        if (cmbMicrophoneDevice.SelectedIndex > 0)
        {
            var mics = SpeechRecognitionService.GetAvailableMicrophones();
            int idx = cmbMicrophoneDevice.SelectedIndex - 1;
            if (idx >= 0 && idx < mics.Count)
                deviceId = mics[idx].deviceNumber.ToString();
        }
        try
        {
            databaseService.SaveUserMicrophonePreference(username, deviceId);
            if (speechService != null && deviceId != null && int.TryParse(deviceId, out int num))
                speechService.SetMicrophoneDevice(num);
            else if (speechService != null)
                speechService.SetMicrophoneDevice(null);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to save microphone: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void CmbOverlayPosition_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cmbOverlayPosition == null || databaseService == null) return;
        string[] values = { "bottom_center", "bottom_right", "bottom_left", "top_center", "top_left", "top_right" };
        int idx = cmbOverlayPosition.SelectedIndex;
        if (idx < 0 || idx >= values.Length) return;
        string position = values[idx];
        try
        {
            databaseService.SaveUserOverlayPosition(username, position);
            overlayForm?.SetOverlayPosition(position);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to save overlay position: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void ChkStartMinimized_CheckedChanged(object? sender, EventArgs e)
    {
        if (chkStartMinimized == null || chkMinimizeToTray == null || databaseService == null) return;
        try
        {
            databaseService.SaveUserApplicationPreferences(username, chkStartMinimized.Checked, chkMinimizeToTray.Checked);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to save preference: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void ChkMinimizeToTray_CheckedChanged(object? sender, EventArgs e)
    {
        if (chkStartMinimized == null || chkMinimizeToTray == null || databaseService == null) return;
        try
        {
            databaseService.SaveUserApplicationPreferences(username, chkStartMinimized.Checked, chkMinimizeToTray.Checked);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to save preference: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
    
    private void BtnClearSpeechHistory_Click(object? sender, EventArgs e)
    {
        if (databaseService == null)
            return;

        DialogResult result = MessageBox.Show(
            "Are you sure you want to clear all speech history? This action cannot be undone.",
            "Confirm Clear",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result == DialogResult.Yes)
        {
            try
            {
                databaseService.ClearAllSpeechHistory(username);
                MessageBox.Show("Speech history cleared successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to clear speech history: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void BtnClearDictionary_Click(object? sender, EventArgs e)
    {
        if (databaseService == null)
            return;

        DialogResult result = MessageBox.Show(
            "Are you sure you want to clear all dictionary entries? This action cannot be undone.",
            "Confirm Clear",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result == DialogResult.Yes)
        {
            try
            {
                databaseService.ClearAllDictionary(username);
                dictionaryPageService?.RefreshDictionaryList();
                transcriptionCorrectionService?.InvalidateDictionaryCache(username);
                MessageBox.Show("Dictionary cleared successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to clear dictionary: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void BtnClearSnippets_Click(object? sender, EventArgs e)
    {
        if (databaseService == null)
            return;

        DialogResult result = MessageBox.Show(
            "Are you sure you want to clear all snippets? This action cannot be undone.",
            "Confirm Clear",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result == DialogResult.Yes)
        {
            try
            {
                databaseService.ClearAllSnippets(username);
                snippetsPageService?.RefreshSnippetsList();
                transcriptionCorrectionService?.InvalidateSnippetsCache(username);
                MessageBox.Show("Snippets cleared successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to clear snippets: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void BtnExportData_Click(object? sender, EventArgs e)
    {
        if (databaseService == null)
            return;

        try
        {
            string jsonData = databaseService.ExportUserData(username);
            
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            saveDialog.FileName = $"WinFormTest_Export_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveDialog.FileName, jsonData);
                MessageBox.Show($"Data exported successfully to:\n{saveDialog.FileName}", "Export Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to export data: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
