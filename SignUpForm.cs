using Microsoft.Data.SqlClient;

namespace WinFormTest;

public partial class SignUpForm : Form
{
  private readonly string _connectionString;

  public SignUpForm(string connectionString)
  {
    _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    InitializeComponent();
    CenterControls();
  }

  private static void EnsureUsersTableExists(SqlConnection connection)
  {
    string createSql = @"
      IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Users' AND schema_id = SCHEMA_ID('dbo'))
      CREATE TABLE dbo.Users ( username NVARCHAR(255) NOT NULL, userPass NVARCHAR(255) NOT NULL );";
    using var cmd = new SqlCommand(createSql, connection);
    cmd.ExecuteNonQuery();
  }

  private void CenterControls()
  {
    int w = ClientSize.Width;
    foreach (Control c in new Control[] { lblTitle, lblUsername, txtUsername, underlineUsername, lblPassword, txtPassword, underlinePassword, lblConfirm, txtConfirm, underlineConfirm, btnCreate })
      c.Left = (w - c.Width) / 2;
  }

  private void btnCreate_Click(object? sender, EventArgs e)
  {
    string username = txtUsername.Text?.Trim() ?? "";
    string password = txtPassword.Text ?? "";
    string confirm = txtConfirm.Text ?? "";

    if (string.IsNullOrWhiteSpace(username))
    {
      MessageBox.Show("Please enter a username.", "Sign Up", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      return;
    }
    if (string.IsNullOrEmpty(password))
    {
      MessageBox.Show("Please enter a password.", "Sign Up", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      return;
    }
    if (password != confirm)
    {
      MessageBox.Show("Passwords do not match.", "Sign Up", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      return;
    }

    try
    {
      using var connection = new SqlConnection(_connectionString);
      connection.Open();

      EnsureUsersTableExists(connection);

      string checkQuery = "SELECT COUNT(*) FROM dbo.Users WHERE username = @u";
      using (var checkCmd = new SqlCommand(checkQuery, connection))
      {
        checkCmd.Parameters.AddWithValue("@u", username);
        int existing = (int)checkCmd.ExecuteScalar();
        if (existing > 0)
        {
          MessageBox.Show("Username already exists.", "Sign Up", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          return;
        }
      }

      string insertQuery = "INSERT INTO dbo.Users (username, userPass) VALUES (@u, @p)";
      using (var insertCmd = new SqlCommand(insertQuery, connection))
      {
        insertCmd.Parameters.AddWithValue("@u", username);
        insertCmd.Parameters.AddWithValue("@p", password);
        insertCmd.ExecuteNonQuery();
      }

      MessageBox.Show("Account created. You can now sign in.", "Sign Up", MessageBoxButtons.OK, MessageBoxIcon.Information);
      DialogResult = DialogResult.OK;
      Close();
    }
    catch (SqlException ex)
    {
      MessageBox.Show($"Error: {ex.Message}", "Sign Up", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
  }
}
