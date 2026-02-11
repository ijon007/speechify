# Textify Database

The app uses **SQL Server** (Microsoft.Data.SqlClient). All data is stored in a database named **WinFormTest** on `localhost\SQLEXPRESS` with Windows Authentication.

## Connection

- **Connection string** (see `Constants.cs` → `DatabaseConstants.ConnectionString`):
  - `Data Source=localhost\SQLEXPRESS;Initial Catalog=WinFormTest;Integrated Security=True;TrustServerCertificate=True;`
- **Access**: `DatabaseService` holds the connection string and runs all queries. The login form (`Form1`) also uses it for authentication only.

## Tables

### Users

Used only for login. No create/update is done in app code; accounts are expected to exist in the DB.

| Column     | Type         | Description        |
|-----------|--------------|--------------------|
| username  | (string)     | Login name         |
| userPass  | (string)     | Password (plain)   |

- **Usage**: `Form1.AuthenticateUser` runs `SELECT COUNT(*) FROM Users WHERE username = @u AND userPass = @p`.

---

### Speeches

Stores speech-to-text results per user.

| Column     | Type        | Description                    |
|-----------|-------------|--------------------------------|
| Id        | int         | Primary key                    |
| Username  | (string)    | Owner                          |
| SpeechText| (string)    | Transcribed text               |
| CreatedAt | datetime    | When the speech was saved      |
| Duration  | int? (ms)   | Optional recording duration    |

- **Operations**: Insert via `SaveSpeech`; list via `GetSpeeches` (by username, ordered by `CreatedAt DESC`); stats via `GetConsecutiveWeeksStreak`, `GetTotalWords`, `GetAverageWPM`; delete all for user via `ClearAllSpeechHistory`.

---

### Dictionary

User-defined words (e.g. for recognition/correction).

| Column   | Type     | Description     |
|----------|----------|-----------------|
| Id       | int      | Primary key     |
| Username | (string) | Owner           |
| Word     | (string) | Dictionary word |
| CreatedAt| datetime | Created         |
| UpdatedAt| datetime | Last updated    |

- **Constraints**: One row per (Username, Word). Updates and deletes check `Id` + `Username`.
- **Operations**: `AddDictionaryEntry`, `GetDictionaryEntries`, `UpdateDictionaryEntry`, `DeleteDictionaryEntry`, `ClearAllDictionary`.

---

### Snippets

Shortcut → replacement text per user.

| Column     | Type     | Description      |
|------------|----------|------------------|
| Id         | int      | Primary key      |
| Username   | (string) | Owner            |
| Shortcut   | (string) | Trigger phrase   |
| Replacement| (string) | Text to insert   |
| CreatedAt  | datetime | Created          |
| UpdatedAt  | datetime | Last updated     |

- **Constraints**: One row per (Username, Shortcut). Updates/deletes check `Id` + `Username`.
- **Operations**: `AddSnippet`, `GetSnippets`, `UpdateSnippet`, `DeleteSnippet`, `ClearAllSnippets`.

---

### UserSettings

One logical row per user; stores style, hotkey, microphone, overlay, and app behavior. Columns are added over time; missing columns are created by `EnsureSettingsColumnsExist()` (MicrophoneDeviceId, OverlayPosition, StartMinimized, MinimizeToTray). Hotkey columns (`HotkeyModifiers`, `HotkeyKeyCode`) are not auto-created; a migration script is required if they are missing.

| Column           | Type        | Description                          |
|------------------|-------------|--------------------------------------|
| Username         | (string)    | User (key)                           |
| StylePreference  | (string)    | e.g. formal, casual, very_casual     |
| CreatedAt        | datetime    | Created                              |
| UpdatedAt        | datetime    | Last updated                         |
| HotkeyModifiers  | (string)    | e.g. "Ctrl+Win" (optional migration) |
| HotkeyKeyCode    | int?        | Virtual key code (optional migration)|
| MicrophoneDeviceId | NVARCHAR(100) | Preferred mic device ID            |
| OverlayPosition  | NVARCHAR(50)  | e.g. bottom_center (default)      |
| StartMinimized   | BIT           | Launch minimized                    |
| MinimizeToTray   | BIT           | Minimize to system tray             |

- **Operations**: Get/set style, hotkey, microphone, overlay position, and app preferences via the corresponding `Get*` / `Save*` methods. `UserSettingsExists` and `EnsureUserSettingsExists` create a row with default style when needed.

## Data flow

- **Login**: `Form1` → `DatabaseConstants.ConnectionString` → `Users` (read-only check).
- **After login**: `DashboardForm(username)` receives the username; all further DB access goes through `DatabaseService` and is scoped by that username (Speeches, Dictionary, Snippets, UserSettings).
- **Errors**: DB failures are logged with `Debug.WriteLine` and often swallowed so the UI keeps working; some methods (e.g. add/update dictionary or snippet) throw for business-rule or DB errors.

## Schema evolution

- **UserSettings**: New columns (MicrophoneDeviceId, OverlayPosition, StartMinimized, MinimizeToTray) are added at runtime via `EnsureSettingsColumnsExist()` if they do not exist. Hotkey columns are **not** auto-created; the code references `database_migration.sql` when they are missing.
- **Other tables**: No automatic migrations; schema changes (e.g. new tables or columns for Speeches, Dictionary, Snippets, Users) must be applied manually or via a separate migration script.

## Export

- `ExportUserData(username)` returns JSON containing the username, export date, and all speeches, dictionary entries, and snippets for that user (from the same `DatabaseService` getters). It does not include UserSettings or Users.
