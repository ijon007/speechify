# WinFormTest - Application Flow Documentation

## Overview

WinFormTest is a Windows Forms application that provides speech-to-text functionality with global hotkey support. The application allows users to speak into their microphone and automatically inject the recognized text into any active application window.

## Architecture Components

- **Form1.cs** - Login form with user authentication
- **DashboardForm.cs** - Main dashboard displaying speech history
- **DashboardForm.Designer.cs** - UI component declarations and initialization
- **SpeechRecognitionService.cs** - Handles Vosk offline speech recognition
- **TextInjectionService.cs** - Injects recognized text into active windows
- **GlobalHotkeyManager.cs** - Manages global Ctrl+Win hotkey detection
- **DatabaseService.cs** - Handles SQL Server database operations
- **SpeechOverlayForm.cs** - Visual overlay during speech recognition
- **WindowsApiHelper.cs** - Windows API interop functions

**📋 Component Code Locations:** See [COMPONENT_MAP.md](./COMPONENT_MAP.md) for detailed mapping of where each UI component's code is located.

---

## 1. Application Startup Flow

```
Program.Main()
    ↓
ApplicationConfiguration.Initialize()
    ↓
Application.Run(new Form1())
    ↓
Form1 Constructor
    ├─ InitializeComponent()
    ├─ SetupCustomControls() [Centers UI controls]
    ├─ SetupWindowControls() [Enables window dragging]
    └─ Load custom icon from assets/cp-black.ico
```

**Key Points:**
- Application starts with `Form1` (login form)
- Window is draggable from the top 40px area
- Custom icon is loaded if available

---

## 2. Authentication Flow

```
User enters username/password
    ↓
btnLogin_Click()
    ↓
Validate input (non-empty)
    ↓
TestConnection() [Tests SQL Server connection]
    ↓
AuthenticateUser(username, password)
    ├─ Opens SQL connection
    ├─ Executes: SELECT COUNT(*) FROM Users WHERE username=@u AND userPass=@p
    └─ Returns true if count > 0
    ↓
If authenticated:
    ├─ Form1.Hide()
    ├─ Create DashboardForm(username)
    ├─ DashboardForm.Show()
    └─ DashboardForm.FormClosed → Form1.Close()
```

**Database Query:**
```sql
SELECT COUNT(*) FROM Users 
WHERE username = @u AND userPass = @p
```

**Connection String:**
```
Data Source=localhost\SQLEXPRESS;
Initial Catalog=WinFormTest;
Integrated Security=True;
TrustServerCertificate=True;
```

---

## 3. Dashboard Initialization Flow

```
DashboardForm Constructor
    ├─ Store username
    ├─ InitializeComponent()
    ├─ SetupWindowControls() [Enables dragging]
    ├─ Load custom icon
    ├─ Create DatabaseService instance
    ├─ LoadDashboardData() [Sets welcome message, loads history]
    ├─ InitializeLoadingUI() [Shows loading panel]
    └─ InitializeSpeechServicesAsync() [Async initialization]
```

### 3.1 Loading UI

```
InitializeLoadingUI()
    ├─ Create loadingPanel (Dock.Fill, dark background)
    ├─ Create loadingLabel ("Loading speech recognition model...")
    └─ Display loading panel over dashboard
```

### 3.2 Dashboard UI Initialization

```
InitializePages() [DashboardForm.cs:537]
    ├─ CreatePlaceholderPage(panelDictionaryPage, "Dictionary")
    ├─ CreatePlaceholderPage(panelSnippetsPage, "Snippets")
    ├─ CreatePlaceholderPage(panelStylePage, "Style")
    ├─ CreatePlaceholderPage(panelSettingsPage, "Settings")
    ├─ Hide all pages except Home
    └─ Set Home as default active page
```

**Page Structure:**
- All pages are `Panel` controls docked to fill `panelMain`
- Only one page is visible at a time (visibility toggled)
- Home page contains: Welcome message, stats, speech history
- Other pages currently have placeholder content

**Sidebar Navigation:**
- Located in `panelSidebar` (250px wide, left docked)
- Contains: Logo, Home, Dictionary, Snippets, Style, Settings labels
- Navigation labels handle click, mouse enter, and mouse leave events
- Active state managed via `SetActiveNavItem()` method

**Navigation Flow:**
```
User clicks navigation label
    ↓
navItem_Click() [DashboardForm.cs:621]
    ↓
SwitchPage(targetPanel, activeNavLabel) [DashboardForm.cs:584]
    ├─ Hide all pages
    ├─ Show target page
    └─ Update active navigation styling
```

### 3.3 Speech Services Initialization (Async)

```
InitializeSpeechServicesAsync()
    ├─ Create SpeechRecognitionService
    │   ├─ Subscribe to SpeechRecognized event
    │   └─ Subscribe to SpeechPartialResult event
    ├─ Create TextInjectionService
    ├─ Create GlobalHotkeyManager
    │   ├─ Subscribe to HotkeyPressed event
    │   ├─ Subscribe to HotkeyReleased event
    │   └─ InstallKeyboardHook() [Low-level keyboard hook]
    ├─ Create SpeechOverlayForm (initially hidden)
    └─ await speechService.InitializeAsync()
        └─ Load Vosk model from models/vosk-model-en-us-0.22/
            └─ Hide loading panel when complete
```

**Model Loading:**
- Path: `Application.StartupPath/models/vosk-model-en-us-0.22/`
- Format: 16kHz, Mono, 16-bit
- Loaded asynchronously to prevent UI blocking

---

## 3.5 Dashboard Page Navigation Flow

### Page Components
All pages are `Panel` controls declared in `DashboardForm.Designer.cs`:
- `panelHomePage` - Main dashboard with speech history
- `panelDictionaryPage` - Dictionary page (placeholder)
- `panelSnippetsPage` - Snippets page (placeholder)
- `panelStylePage` - Style page (placeholder)
- `panelSettingsPage` - Settings page (placeholder)

### Sidebar Components
Navigation labels declared in `DashboardForm.Designer.cs`:
- `lblNavHome` - Home navigation item
- `lblNavDictionary` - Dictionary navigation item
- `lblNavSnippets` - Snippets navigation item
- `lblNavStyle` - Style navigation item
- `lblNavSettings` - Settings navigation item

### Navigation Event Flow
```
User clicks sidebar navigation label
    ↓
navItem_Click() [DashboardForm.cs:621-646]
    ├─ Identifies clicked label
    ├─ Maps to corresponding panel
    └─ Calls SwitchPage(panel, label)
        ↓
SwitchPage() [DashboardForm.cs:584-599]
    ├─ Hides all page panels
    ├─ Shows target page panel
    ├─ Updates activePagePanel field
    └─ Calls SetActiveNavItem(label)
        ↓
SetActiveNavItem() [DashboardForm.cs:601-619]
    ├─ Resets all nav items to inactive (gray)
    └─ Sets clicked item to active (black, light gray bg)
```

### Hover Effects
- **MouseEnter:** `navItem_MouseEnter()` [DashboardForm.cs:375-382]
  - Changes background to light gray
  - Changes text to black
- **MouseLeave:** `navItem_MouseLeave()` [DashboardForm.cs:384-395]
  - Restores inactive state (unless item is active)
  - Preserves active state styling

---

## 4. Speech Recognition Flow

### 4.1 Hotkey Detection

```
User presses Ctrl + Win keys
    ↓
GlobalHotkeyManager.HookCallback() [Low-level keyboard hook]
    ├─ Detects Ctrl key (0xA2 or 0xA3)
    ├─ Detects Win key (0x5B or 0x5C)
    └─ When both pressed simultaneously:
        ├─ Set isPressed = true
        └─ Invoke HotkeyPressed event (via SynchronizationContext)
```

**Hook Implementation:**
- Uses `SetWindowsHookEx` with `WH_KEYBOARD_LL` (low-level keyboard hook)
- Checks `GetAsyncKeyState` to verify both keys are pressed
- Posts events to UI thread via `SynchronizationContext`

### 4.2 Start Listening

```
HotkeyManager_HotkeyPressed()
    ├─ Check if model is loaded
    ├─ Reset recognizedText = null
    ├─ Save foreground window handle (WindowsApiHelper.GetForegroundWindow())
    ├─ textInjectionService.SaveForegroundWindow(activeWindow)
    ├─ speechService.StartListening()
    │   ├─ Create WaveInEvent (16kHz, Mono, 16-bit)
    │   ├─ Subscribe to DataAvailable event
    │   ├─ Reset Vosk recognizer
    │   └─ StartRecording()
    ├─ overlayForm.SetState(Listening) [Shows animated waves]
    └─ overlayForm.Show() [Displays overlay at bottom-center of screen]
```

### 4.3 Audio Processing

```
Microphone captures audio
    ↓
WaveIn_DataAvailable() [Called for each audio chunk]
    ├─ Process audio through Vosk recognizer
    ├─ recognizer.AcceptWaveform(buffer, bytesRecorded)
    │   ├─ If true: Finalized result (sentence complete)
    │   │   └─ recognizer.Result() → ProcessFinalResult()
    │   │       ├─ Parse JSON: {"text": "recognized text"}
    │   │       └─ Invoke SpeechRecognized event
    │   └─ If false: Partial result (words in progress)
    │       └─ recognizer.PartialResult() → ProcessPartialResult()
    │           ├─ Parse JSON: {"partial": "partial text"}
    │           └─ Invoke SpeechPartialResult event
```

**Event Handlers:**

```
SpeechService_SpeechRecognized()
    ├─ Accumulate text: recognizedText += " " + text
    └─ overlayForm.SetRecognizedText(recognizedText)

SpeechService_SpeechPartialResult()
    ├─ Display preview: recognizedText + " " + partialText
    └─ overlayForm.SetRecognizedText(displayText)
    [Note: Partial results are NOT accumulated, only displayed]
```

### 4.4 Stop Listening

```
User releases Ctrl + Win keys
    ↓
GlobalHotkeyManager.HookCallback()
    ├─ Detects key release
    ├─ Checks if both keys are no longer pressed
    └─ Invoke HotkeyReleased event
        ↓
HotkeyManager_HotkeyReleased() [Async]
    ├─ speechService.StopListening()
    │   ├─ StopRecording()
    │   ├─ Get final result: recognizer.FinalResult()
    │   └─ ProcessFinalResult()
    ├─ await Task.Delay(1000) [Wait for pending recognition events]
    ├─ If recognizedText is not empty:
    │   ├─ textInjectionService.InjectText(recognizedText)
    │   ├─ SaveSpeechToDatabase(recognizedText)
    │   └─ AddSpeechToHistory(recognizedText)
    ├─ overlayForm.Hide()
    └─ Reset recognizedText = null
```

**Delay Reason:**
- Recognition events can fire asynchronously after `StopListening()`
- 1000ms delay ensures all pending events are processed

---

## 5. Text Injection Flow

```
TextInjectionService.InjectText(text)
    ├─ Restore focus to original window
    │   └─ WindowsApiHelper.SetForegroundWindow(originalForegroundWindow)
    │   └─ Thread.Sleep(150) [Give window time to receive focus]
    └─ InjectViaClipboard(text)
        ├─ Save current clipboard content
        ├─ Clipboard.Clear()
        ├─ Thread.Sleep(10)
        ├─ Clipboard.SetText(text)
        ├─ Verify clipboard was set correctly
        ├─ Restore window focus again
        ├─ Thread.Sleep(50)
        ├─ SendKeys.SendWait("^v") [Send Ctrl+V to paste]
        ├─ Thread.Sleep(50)
        └─ Restore original clipboard after 200ms delay
            [Async task to restore clipboard content]
```

**Fallback:**
- If clipboard method fails, just set clipboard as fallback
- Original clipboard is restored after injection

---

## 6. Database Operations Flow

### 6.1 Save Speech

```
SaveSpeechToDatabase(text)
    ↓
DatabaseService.SaveSpeech(username, text)
    ├─ Open SQL connection
    ├─ Execute: INSERT INTO Speeches (Username, SpeechText, CreatedAt) 
    │           VALUES (@username, @speechText, @createdAt)
    └─ Close connection
    [Errors are logged but don't throw exceptions]
```

### 6.2 Load Speech History

```
LoadDashboardData()
    ↓
LoadSpeechHistory()
    ↓
DatabaseService.GetSpeeches(username, limit=50)
    ├─ Open SQL connection
    ├─ Execute: SELECT TOP 50 Id, CreatedAt, SpeechText 
    │           FROM Speeches 
    │           WHERE Username = @username 
    │           ORDER BY CreatedAt DESC
    ├─ Read results and format time as "hh:mm tt"
    └─ Return List<(id, time, text)>
        ↓
For each speech entry:
    ├─ Create timestamp label
    ├─ Create text label (with word wrapping)
    ├─ Create copy button (📋)
    └─ Add to panelSpeechHistory
```

### 6.3 Add Speech to History (UI Update)

```
AddSpeechToHistory(text)
    ├─ Get current time: DateTime.Now.ToString("hh:mm tt")
    ├─ Generate temporary ID from DateTime.Ticks
    ├─ Move existing controls down
    ├─ CreateSpeechRow(tempId, time, text, yOffset=10)
    └─ Scroll to top
```

**Copy Functionality:**
- Each speech entry has a copy button (📋)
- Clicking copies text to clipboard
- Button temporarily shows "✓" for 1 second

---

## 7. Speech Overlay Form Flow

```
SpeechOverlayForm
    ├─ Position: Bottom-center of primary screen
    ├─ Rounded corners (10px radius)
    ├─ States:
    │   ├─ Idle: Shows hotkey hint
    │   ├─ Listening: Animated voice waves
    │   └─ Recognizing: Shows recognized text
    └─ Animation:
        ├─ Dot animation timer (500ms interval)
        └─ Wave heights animation (4 waves, varying heights)
```

**State Transitions:**
```
Idle → Listening (when hotkey pressed)
    ├─ StartDotAnimation()
    └─ Draw animated waves

Listening → Recognizing (when text recognized)
    ├─ StopDotAnimation()
    └─ Display recognized text

Recognizing → Hidden (when hotkey released)
    └─ Hide overlay
```

---

## 8. Event Flow Diagram

### 8.1 Complete Speech Recognition Cycle

```
[User Action] Press Ctrl+Win
    ↓
[GlobalHotkeyManager] HookCallback detects keys
    ↓
[Event] HotkeyPressed
    ↓
[DashboardForm] HotkeyManager_HotkeyPressed
    ├─ Save foreground window
    ├─ Start listening
    └─ Show overlay
        ↓
[SpeechRecognitionService] StartListening
    ├─ Initialize NAudio WaveInEvent
    └─ Start recording
        ↓
[Audio Stream] WaveIn_DataAvailable (continuous)
    ├─ Process through Vosk
    ├─ Partial results → SpeechPartialResult event
    └─ Final results → SpeechRecognized event
        ↓
[DashboardForm] Event handlers
    ├─ SpeechPartialResult → Update overlay preview
    └─ SpeechRecognized → Accumulate text
        ↓
[User Action] Release Ctrl+Win
    ↓
[GlobalHotkeyManager] HookCallback detects release
    ↓
[Event] HotkeyReleased
    ↓
[DashboardForm] HotkeyManager_HotkeyReleased
    ├─ Stop listening
    ├─ Wait 1000ms for pending events
    ├─ Inject text
    ├─ Save to database
    ├─ Update UI history
    └─ Hide overlay
```

### 8.2 Text Injection Sequence

```
[Hotkey Released]
    ↓
[TextInjectionService] InjectText
    ├─ Restore window focus
    ├─ Save clipboard
    ├─ Set clipboard to recognized text
    ├─ Verify clipboard
    ├─ Restore focus again
    ├─ Send Ctrl+V
    └─ Restore original clipboard (async, 200ms delay)
```

---

## 9. Component Dependencies

```
Program
    └─ Form1 (Login)
        └─ DashboardForm
            ├─ DatabaseService
            ├─ SpeechRecognitionService
            │   └─ Vosk Model (external file)
            ├─ TextInjectionService
            │   └─ WindowsApiHelper
            ├─ GlobalHotkeyManager
            │   └─ WindowsApiHelper
            └─ SpeechOverlayForm
```

**External Dependencies:**
- **Vosk** (v0.3.38) - Speech recognition engine
- **NAudio** (v2.2.1) - Audio capture
- **Microsoft.Data.SqlClient** (v5.2.2) - Database connectivity
- **SQL Server** - Database backend
- **Vosk Model** - Language model files (vosk-model-en-us-0.22)

---

## 10. Key Design Patterns

### 10.1 Event-Driven Architecture
- Services communicate via events (SpeechRecognized, HotkeyPressed, etc.)
- Loose coupling between components

### 10.2 Async/Await Pattern
- Model loading is asynchronous to prevent UI blocking
- Text injection uses async tasks for clipboard restoration

### 10.3 Service Pattern
- Separate services for distinct responsibilities:
  - SpeechRecognitionService
  - TextInjectionService
  - DatabaseService
  - GlobalHotkeyManager

### 10.4 Thread Safety
- SpeechRecognitionService uses `lockObject` for thread-safe audio processing
- SynchronizationContext ensures UI thread execution for events

---

## 11. Error Handling

### 11.1 Database Errors
- Database operations catch exceptions and log to Debug
- Application continues even if database fails

### 11.2 Speech Recognition Errors
- Audio processing errors are logged but don't stop recognition
- Model loading errors show MessageBox to user

### 11.3 Text Injection Errors
- Clipboard errors are caught and ignored
- Fallback to simple clipboard set if injection fails

### 11.4 Hotkey Errors
- Keyboard hook errors are handled gracefully
- Application continues if hotkey registration fails

---

## 12. Data Flow Summary

```
User Input (Speech)
    ↓
Microphone (NAudio)
    ↓
Audio Stream (16kHz, Mono)
    ↓
Vosk Recognizer
    ↓
JSON Results
    ├─ Partial: {"partial": "text"}
    └─ Final: {"text": "recognized text"}
    ↓
Event Handlers
    ↓
Accumulated Text String
    ↓
Text Injection (Clipboard + Ctrl+V)
    ↓
Active Application Window
    ↓
Database (SQL Server)
    ↓
UI History Display
```

---

## 13. Configuration Points

### 13.1 Database Connection
- **Form1.cs** (line 8): Login form connection string
- **DatabaseService.cs** (line 11): Service connection string

### 13.2 Vosk Model Path
- **SpeechRecognitionService.cs** (line 46): `models/vosk-model-en-us-0.22`

### 13.3 Audio Format
- **SpeechRecognitionService.cs** (line 72): 16kHz, Mono, 16-bit

### 13.4 Hotkey Combination
- **GlobalHotkeyManager.cs**: Ctrl (0xA2/0xA3) + Win (0x5B/0x5C)

### 13.5 Recognition Delay
- **DashboardForm.cs** (line 175): 1000ms delay after stopping

---

## 14. UI Features

### 14.1 Window Dragging
- Both Form1 and DashboardForm support dragging from top 40px
- Custom window controls (close, minimize)

### 14.2 Speech History
- Scrollable panel with speech entries
- Each entry shows: timestamp, text, copy button
- New entries added at top
- Word wrapping for long text

### 14.3 Loading States
- Loading panel shown during model initialization
- Loading text updates during async operations

### 14.4 Visual Feedback
- Overlay form shows recognition state
- Animated waves during listening
- Text preview during recognition

---

## 15. Cleanup and Disposal

```
DashboardForm.OnFormClosing()
    ├─ speechService?.Dispose()
    │   ├─ StopListening()
    │   ├─ Dispose WaveInEvent
    │   ├─ Dispose VoskRecognizer
    │   └─ Dispose VoskModel
    ├─ hotkeyManager?.Dispose()
    │   ├─ UnregisterHotkey()
    │   └─ UninstallKeyboardHook()
    └─ overlayForm?.Close()
```

**Resource Management:**
- All services implement `IDisposable`
- Proper cleanup prevents resource leaks
- Keyboard hook is uninstalled on exit

---

## Notes

- The application uses Windows Forms with .NET 10.0
- All speech recognition happens offline using Vosk
- Global hotkey works system-wide (not just when app is focused)
- Text injection preserves original clipboard content
- Database operations are non-blocking (errors don't crash app)
- UI updates are thread-safe via SynchronizationContext
