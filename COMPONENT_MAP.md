# Component Code Location Map - DashboardForm

This document maps where each component's code is located in the DashboardForm application.

## Table of Contents
1. [Sidebar Components](#sidebar-components)
2. [Page Components](#page-components)
3. [Home Page Components](#home-page-components)
4. [Navigation Logic](#navigation-logic)
5. [Window Controls](#window-controls)

---

## Sidebar Components

### Sidebar Panel
**Location:** `DashboardForm.Designer.cs`
- **Panel Declaration:** Line 361 - `private Panel panelSidebar;`
- **Panel Initialization:** Lines 58-72
  - BackColor: White
  - Dock: Left
  - Size: 250x800
  - Contains: Logo, navigation labels

### Logo Components
**Location:** `DashboardForm.Designer.cs`
- **PictureBox (picLogo):** Lines 74-81
  - Location: (20, 30)
  - Size: 32x32
- **Label (lblLogo):** Lines 83-92
  - Text: "Textify"
  - Location: (60, 30)
  - Font: Segoe UI, 16pt, Bold

**Logo Loading Logic:** `DashboardForm.cs` Lines 30-55
- Loads icon from `assets/cp-black.ico`
- Sets form icon and PictureBox image

### Navigation Labels (Sidebar Items)
**Location:** `DashboardForm.Designer.cs`

All navigation labels are declared at:
- Lines 364-368 (field declarations)

#### Home Navigation
- **Label Declaration:** Line 364 - `private Label lblNavHome;`
- **Initialization:** Lines 94-109
  - Text: "Home"
  - Location: (20, 100)
  - Event Handlers:
    - `Click += navItem_Click` (Line 107)
    - `MouseEnter += navItem_MouseEnter` (Line 108)
    - `MouseLeave += navItem_MouseLeave` (Line 109)

#### Dictionary Navigation
- **Label Declaration:** Line 365 - `private Label lblNavDictionary;`
- **Initialization:** Lines 111-125
  - Text: "Dictionary"
  - Location: (20, 150)
  - Same event handlers as Home

#### Snippets Navigation
- **Label Declaration:** Line 366 - `private Label lblNavSnippets;`
- **Initialization:** Lines 127-141
  - Text: "Snippets"
  - Location: (20, 200)
  - Same event handlers as Home

#### Style Navigation
- **Label Declaration:** Line 367 - `private Label lblNavStyle;`
- **Initialization:** Lines 143-157
  - Text: "Style"
  - Location: (20, 250)
  - Same event handlers as Home

#### Settings Navigation
- **Label Declaration:** Line 368 - `private Label lblNavSettings;`
- **Initialization:** Lines 159-173
  - Text: "Settings"
  - Location: (20, 300)
  - Same event handlers as Home

### Sidebar Event Handlers
**Location:** `DashboardForm.cs`

#### Navigation Click Handler
- **Method:** `navItem_Click()` - Lines 621-646
- **Logic:** Determines which navigation item was clicked and calls `SwitchPage()` with appropriate panel and label

#### Hover Effects
- **MouseEnter:** `navItem_MouseEnter()` - Lines 375-382
  - Changes background to light gray (245, 245, 245)
  - Changes text color to black
- **MouseLeave:** `navItem_MouseLeave()` - Lines 384-395
  - Restores inactive state (transparent background, gray text)
  - Preserves active state if item is currently active

#### Active State Management
- **Method:** `SetActiveNavItem()` - Lines 601-619
  - Resets all navigation items to inactive state
  - Sets the active item to black text with light gray background
  - Updates `activeNavItem` field (Line 18)

---

## Page Components

### Main Container Panel
**Location:** `DashboardForm.Designer.cs`
- **Panel Declaration:** Line 369 - `private Panel panelMain;`
- **Initialization:** Lines 175-187
  - BackColor: White
  - Dock: Fill
  - Location: (250, 0) - Starts after sidebar
  - Size: 950x800
  - Contains all page panels

### Page Panel Declarations
**Location:** `DashboardForm.Designer.cs` Lines 370-380
- `panelHomePage` (Line 370)
- `panelDictionaryPage` (Line 377)
- `panelSnippetsPage` (Line 378)
- `panelStylePage` (Line 379)
- `panelSettingsPage` (Line 380)

### Page Initialization
**Location:** `DashboardForm.cs`

#### InitializePages() Method
- **Location:** Lines 537-556
- **Called from:** Constructor (Line 58)
- **Responsibilities:**
  1. Creates placeholder content for Dictionary, Snippets, Style, Settings pages
  2. Hides all pages except Home
  3. Sets Home as default active page
  4. Sets active navigation item

#### CreatePlaceholderPage() Method
- **Location:** Lines 558-582
- **Parameters:** Panel and title string
- **Creates:**
  - Title label (24pt, Bold)
  - Placeholder text label (11pt, Regular)
- **Used for:** Dictionary, Snippets, Style, Settings pages

#### SwitchPage() Method
- **Location:** Lines 584-599
- **Parameters:** Target panel and active navigation label
- **Logic:**
  1. Hides all page panels
  2. Shows target page panel
  3. Updates `activePagePanel` field (Line 17)
  4. Calls `SetActiveNavItem()` to update sidebar styling

### Individual Page Panels

#### Home Page (panelHomePage)
**Location:** `DashboardForm.Designer.cs` Lines 189-203
- **Initialization:** Lines 191-203
  - BackColor: White
  - Dock: Fill
  - Padding: (40, 60, 40, 40)
  - Contains: Welcome label, stats labels, "TODAY" label, speech history panel

**Content Setup:** `DashboardForm.cs`
- **LoadDashboardData()** - Lines 337-344
  - Sets welcome message with username
  - Calls `LoadSpeechHistory()`

#### Dictionary Page (panelDictionaryPage)
**Location:** `DashboardForm.Designer.cs` Lines 269-276
- **Initialization:** Lines 271-276
  - Basic panel setup (White, Dock Fill)
- **Content:** Created dynamically in `CreatePlaceholderPage()` - Line 540

#### Snippets Page (panelSnippetsPage)
**Location:** `DashboardForm.Designer.cs` Lines 278-285
- **Initialization:** Lines 280-285
  - Basic panel setup (White, Dock Fill)
- **Content:** Created dynamically in `CreatePlaceholderPage()` - Line 541

#### Style Page (panelStylePage)
**Location:** `DashboardForm.Designer.cs` Lines 287-294
- **Initialization:** Lines 289-294
  - Basic panel setup (White, Dock Fill)
- **Content:** Created dynamically in `CreatePlaceholderPage()` - Line 542

#### Settings Page (panelSettingsPage)
**Location:** `DashboardForm.Designer.cs` Lines 296-303
- **Initialization:** Lines 298-303
  - Basic panel setup (White, Dock Fill)
- **Content:** Created dynamically in `CreatePlaceholderPage()` - Line 543

---

## Home Page Components

### Welcome Label
**Location:** `DashboardForm.Designer.cs` Lines 205-214
- **Declaration:** Line 371 - `private Label lblWelcome;`
- **Initialization:** Lines 207-214
  - Text: "Welcome back!" (updated dynamically)
  - Font: Segoe UI, 24pt, Bold
  - Location: (40, 60)
- **Dynamic Update:** `DashboardForm.cs` Line 340
  - `lblWelcome.Text = $"Welcome back, {username}!";`

### Statistics Labels
**Location:** `DashboardForm.Designer.cs`

#### Weeks Stat (lblStatWeeks)
- **Declaration:** Line 372 - `private Label lblStatWeeks;`
- **Initialization:** Lines 216-225
  - Text: "🔥 5 weeks"
  - Location: (40, 120)
  - Font: Segoe UI, 10pt, Regular

#### Words Stat (lblStatWords)
- **Declaration:** Line 373 - `private Label lblStatWords;`
- **Initialization:** Lines 227-236
  - Text: "🚀 20.2K words"
  - Location: (130, 120)
  - Font: Segoe UI, 10pt, Regular

#### WPM Stat (lblStatWPM)
- **Declaration:** Line 374 - `private Label lblStatWPM;`
- **Initialization:** Lines 238-247
  - Text: "🏆 111 WPM"
  - Location: (250, 120)
  - Font: Segoe UI, 10pt, Regular

### Today Label
**Location:** `DashboardForm.Designer.cs` Lines 249-258
- **Declaration:** Line 375 - `private Label lblToday;`
- **Initialization:** Lines 251-258
  - Text: "TODAY"
  - Location: (40, 180)
  - Font: Segoe UI, 12pt, Bold

### Speech History Panel
**Location:** `DashboardForm.Designer.cs` Lines 260-267
- **Declaration:** Line 376 - `private Panel panelSpeechHistory;`
- **Initialization:** Lines 262-267
  - AutoScroll: true
  - BackColor: White
  - Location: (40, 220)
  - Size: 870x540

#### Speech History Logic
**Location:** `DashboardForm.cs`

##### LoadSpeechHistory() Method
- **Location:** Lines 346-372
- **Called from:** `LoadDashboardData()` - Line 343
- **Process:**
  1. Gets speeches from database via `DatabaseService.GetSpeeches()`
  2. Calculates spacing (40px between items)
  3. For each speech entry:
     - Measures text height for layout
     - Calls `CreateSpeechRow()` to create UI elements
     - Calculates next Y position

##### CreateSpeechRow() Method
- **Location:** Lines 405-452
- **Parameters:** id, time, text, yOffset
- **Creates:**
  1. Timestamp label (9pt, gray)
  2. Text label (11pt, dark gray, word-wrapped)
  3. Copy button (📋 emoji, 30x30)
- **Adds to:** `panelSpeechHistory.Controls`

##### AddSpeechToHistory() Method
- **Location:** Lines 498-535
- **Called from:** `HotkeyManager_HotkeyReleased()` - Line 217
- **Process:**
  1. Gets current time
  2. Generates temporary ID
  3. Moves existing controls down
  4. Creates new speech row at top
  5. Scrolls to top

##### Copy Button Handlers
- **Click:** `BtnCopy_Click()` - Lines 454-480
  - Copies text to clipboard
  - Shows "✓" for 1 second
- **MouseEnter:** `BtnCopy_MouseEnter()` - Lines 482-488
  - Darker background on hover
- **MouseLeave:** `BtnCopy_MouseLeave()` - Lines 490-496
  - Restores light background

---

## Navigation Logic

### Page Switching Flow
```
User clicks navigation label
    ↓
navItem_Click() [DashboardForm.cs:621]
    ↓
Identifies which label was clicked
    ↓
Calls SwitchPage(panel, label)
    ↓
SwitchPage() [DashboardForm.cs:584]
    ├─ Hides all pages
    ├─ Shows target page
    ├─ Updates activePagePanel field
    └─ Calls SetActiveNavItem()
        ↓
SetActiveNavItem() [DashboardForm.cs:601]
    ├─ Resets all nav items to inactive
    └─ Sets clicked item to active
```

### Active State Tracking
**Fields:** `DashboardForm.cs` Lines 17-18
- `activePagePanel` - Currently visible page panel
- `activeNavItem` - Currently active navigation label

---

## Window Controls

### Close Button
**Location:** `DashboardForm.Designer.cs` Lines 305-321
- **Declaration:** Line 381 - `private Button btnClose;`
- **Initialization:** Lines 307-321
  - Text: "×"
  - Location: (1170, 0)
  - Size: 30x30
- **Event Handlers:** `DashboardForm.cs`
  - `Click`: `btnClose_Click()` - Lines 299-304
  - `MouseEnter`: `btnClose_MouseEnter()` - Lines 306-310
  - `MouseLeave`: `btnClose_MouseLeave()` - Lines 312-316

### Minimize Button
**Location:** `DashboardForm.Designer.cs` Lines 323-339
- **Declaration:** Line 382 - `private Button btnMinimize;`
- **Initialization:** Lines 325-339
  - Text: "−"
  - Location: (1140, 0)
  - Size: 30x30
- **Event Handlers:** `DashboardForm.cs`
  - `Click`: `btnMinimize_Click()` - Lines 318-323
  - `MouseEnter`: `btnMinimize_MouseEnter()` - Lines 325-329
  - `MouseLeave`: `btnMinimize_MouseLeave()` - Lines 331-335

### Window Dragging
**Location:** `DashboardForm.cs`
- **Setup:** `SetupWindowControls()` - Lines 261-269
  - Attaches mouse down handlers to form, sidebar, and main panel
- **Handler:** `DashboardForm_MouseDown()` - Lines 271-297
  - Allows dragging from top 40px area
  - Excludes button areas (x >= 1140)

---

## Code Organization Summary

### DashboardForm.Designer.cs
**Purpose:** UI component declarations and initialization
- All control declarations (Panels, Labels, Buttons, PictureBox)
- `InitializeComponent()` method with all UI setup
- Control properties (size, location, colors, fonts)
- Event handler wiring

### DashboardForm.cs
**Purpose:** Business logic and event handlers
- Constructor and initialization logic
- Page management (InitializePages, SwitchPage, CreatePlaceholderPage)
- Navigation logic (navItem_Click, SetActiveNavItem)
- Speech history management (LoadSpeechHistory, CreateSpeechRow, AddSpeechToHistory)
- Window controls (close, minimize, dragging)
- Sidebar hover effects (navItem_MouseEnter, navItem_MouseLeave)
- Speech recognition integration (not covered in this document)

---

## Quick Reference: Where to Find Code

| Component | Designer File | Code File |
|-----------|--------------|-----------|
| Sidebar Panel | Designer.cs:361, 58-72 | - |
| Logo PictureBox | Designer.cs:362, 74-81 | DashboardForm.cs:30-55 |
| Logo Label | Designer.cs:363, 83-92 | - |
| Nav Home | Designer.cs:364, 94-109 | DashboardForm.cs:621-646 |
| Nav Dictionary | Designer.cs:365, 111-125 | DashboardForm.cs:621-646 |
| Nav Snippets | Designer.cs:366, 127-141 | DashboardForm.cs:621-646 |
| Nav Style | Designer.cs:367, 143-157 | DashboardForm.cs:621-646 |
| Nav Settings | Designer.cs:368, 159-173 | DashboardForm.cs:621-646 |
| Main Panel | Designer.cs:369, 175-187 | - |
| Home Page | Designer.cs:370, 189-203 | DashboardForm.cs:337-535 |
| Dictionary Page | Designer.cs:377, 269-276 | DashboardForm.cs:540, 558-582 |
| Snippets Page | Designer.cs:378, 278-285 | DashboardForm.cs:541, 558-582 |
| Style Page | Designer.cs:379, 287-294 | DashboardForm.cs:542, 558-582 |
| Settings Page | Designer.cs:380, 296-303 | DashboardForm.cs:543, 558-582 |
| Welcome Label | Designer.cs:371, 205-214 | DashboardForm.cs:340 |
| Stats Labels | Designer.cs:372-374, 216-247 | - |
| Today Label | Designer.cs:375, 249-258 | - |
| Speech History | Designer.cs:376, 260-267 | DashboardForm.cs:346-535 |
| Close Button | Designer.cs:381, 305-321 | DashboardForm.cs:299-316 |
| Minimize Button | Designer.cs:382, 323-339 | DashboardForm.cs:318-335 |

---

## Notes

1. **Page Content:** Currently, Dictionary, Snippets, Style, and Settings pages only have placeholder content created in `CreatePlaceholderPage()`. To add real functionality, modify the `InitializePages()` method or add content directly to those panels.

2. **Speech History:** The speech history is dynamically created. Each entry consists of three controls (timestamp label, text label, copy button) added to `panelSpeechHistory`.

3. **Navigation:** All navigation items share the same event handlers (`navItem_Click`, `navItem_MouseEnter`, `navItem_MouseLeave`). The click handler determines which item was clicked by comparing the sender.

4. **Active State:** The active navigation item is tracked via the `activeNavItem` field and styled differently (black text, light gray background).

5. **Page Visibility:** Only one page is visible at a time. The `SwitchPage()` method hides all pages before showing the target page.
