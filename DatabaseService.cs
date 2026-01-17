using Microsoft.Data.SqlClient;

namespace WinFormTest;

public class DatabaseService
{
  private string connectionString;

  public DatabaseService()
  {
    connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=WinFormTest;Integrated Security=True;TrustServerCertificate=True;";
  }

  public void SaveSpeech(string username, string speechText)
  {
    if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(speechText))
      return;

    try
    {
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        
        string query = "INSERT INTO Speeches (Username, SpeechText, CreatedAt) VALUES (@username, @speechText, @createdAt)";
        
        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@username", username);
          command.Parameters.AddWithValue("@speechText", speechText);
          command.Parameters.AddWithValue("@createdAt", DateTime.Now);
          
          command.ExecuteNonQuery();
        }
      }
    }
    catch (Exception ex)
    {
      // Log error but don't throw - we don't want to break the app if DB fails
      System.Diagnostics.Debug.WriteLine($"Failed to save speech to database: {ex.Message}");
    }
  }

  public List<(int id, string time, string text)> GetSpeeches(string username, int limit = 50)
  {
    var speeches = new List<(int id, string time, string text)>();

    try
    {
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        
        string query = @"
          SELECT TOP (@limit) Id, CreatedAt, SpeechText 
          FROM Speeches 
          WHERE Username = @username 
          ORDER BY CreatedAt DESC";
        
        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@username", username);
          command.Parameters.AddWithValue("@limit", limit);
          
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              int id = reader.GetInt32(0);
              DateTime createdAt = reader.GetDateTime(1);
              string text = reader.GetString(2);
              string time = createdAt.ToString("hh:mm tt");
              
              speeches.Add((id, time, text));
            }
          }
        }
      }
    }
    catch (Exception ex)
    {
      System.Diagnostics.Debug.WriteLine($"Failed to load speeches from database: {ex.Message}");
    }

    return speeches;
  }
}
