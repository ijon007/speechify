using Microsoft.Data.SqlClient;
using System.Linq;

namespace WinFormTest;

public class DatabaseService
{
  private string connectionString;

  public DatabaseService()
  {
    connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=WinFormTest;Integrated Security=True;TrustServerCertificate=True;";
  }

  public void SaveSpeech(string username, string speechText, int? duration = null)
  {
    if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(speechText))
      return;

    try
    {
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        
        string query = "INSERT INTO Speeches (Username, SpeechText, CreatedAt, Duration) VALUES (@username, @speechText, @createdAt, @duration)";
        
        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@username", username);
          command.Parameters.AddWithValue("@speechText", speechText);
          command.Parameters.AddWithValue("@createdAt", DateTime.Now);
          
          if (duration.HasValue)
            command.Parameters.AddWithValue("@duration", duration.Value);
          else
            command.Parameters.AddWithValue("@duration", DBNull.Value);
          
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

  public int GetConsecutiveWeeksStreak(string username)
  {
    try
    {
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        
        string query = @"
          SELECT DISTINCT CAST(CreatedAt AS DATE) as SpeechDate
          FROM Speeches
          WHERE Username = @username
          ORDER BY SpeechDate DESC";
        
        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@username", username);
          
          var dates = new List<DateTime>();
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              dates.Add(reader.GetDateTime(0));
            }
          }
          
          if (dates.Count == 0)
            return 0;
          
          // Group dates by week (Monday as start of week)
          var weeks = dates
            .Select(d => GetWeekStart(d))
            .Distinct()
            .OrderByDescending(d => d)
            .ToList();
          
          if (weeks.Count == 0)
            return 0;
          
          // Count consecutive weeks starting from the most recent
          int streak = 1;
          DateTime currentWeek = weeks[0];
          
          for (int i = 1; i < weeks.Count; i++)
          {
            DateTime previousWeek = weeks[i];
            DateTime expectedPreviousWeek = currentWeek.AddDays(-7);
            
            // Check if weeks are consecutive (within 1 day tolerance for edge cases)
            if (Math.Abs((previousWeek - expectedPreviousWeek).TotalDays) <= 1)
            {
              streak++;
              currentWeek = previousWeek;
            }
            else
            {
              break; // Streak broken
            }
          }
          
          return streak;
        }
      }
    }
    catch (Exception ex)
    {
      System.Diagnostics.Debug.WriteLine($"Failed to get consecutive weeks streak: {ex.Message}");
      return 0;
    }
  }

  private DateTime GetWeekStart(DateTime date)
  {
    int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
    return date.AddDays(-1 * diff).Date;
  }

  public int GetTotalWords(string username)
  {
    try
    {
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        
        string query = @"
          SELECT SpeechText
          FROM Speeches
          WHERE Username = @username AND SpeechText IS NOT NULL";
        
        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@username", username);
          
          int totalWords = 0;
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              string text = reader.GetString(0);
              if (!string.IsNullOrWhiteSpace(text))
              {
                // Count words by splitting on whitespace
                totalWords += text.Split(new[] { ' ', '\t', '\n', '\r' }, 
                  StringSplitOptions.RemoveEmptyEntries).Length;
              }
            }
          }
          
          return totalWords;
        }
      }
    }
    catch (Exception ex)
    {
      System.Diagnostics.Debug.WriteLine($"Failed to get total words: {ex.Message}");
      return 0;
    }
  }

  public int GetAverageWPM(string username)
  {
    try
    {
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        connection.Open();
        
        // Include records with NULL duration - we'll estimate them
        string query = @"
          SELECT SpeechText, Duration
          FROM Speeches
          WHERE Username = @username 
            AND SpeechText IS NOT NULL";
        
        using (SqlCommand command = new SqlCommand(query, connection))
        {
          command.Parameters.AddWithValue("@username", username);
          
          var wpmValues = new List<double>();
          const double averageSpeakingRateWPM = 150.0; // Average speaking rate for estimation
          
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              string text = reader.GetString(0);
              
              if (string.IsNullOrWhiteSpace(text))
                continue;
              
              // Count words
              int wordCount = text.Split(new[] { ' ', '\t', '\n', '\r' }, 
                StringSplitOptions.RemoveEmptyEntries).Length;
              
              if (wordCount == 0)
                continue;
              
              // Get duration (may be NULL)
              int? durationMs = reader.IsDBNull(1) ? null : reader.GetInt32(1);
              
              double durationMinutes;
              
              if (durationMs.HasValue && durationMs.Value > 0)
              {
                // Use actual recorded duration
                durationMinutes = durationMs.Value / 60000.0;
              }
              else
              {
                // Estimate duration based on average speaking rate (150 WPM)
                // durationMinutes = wordCount / averageSpeakingRateWPMj
                durationMinutes = wordCount / averageSpeakingRateWPM;
              }
              
              // Calculate WPM: (words / duration_in_minutes)
              double wpm = wordCount / durationMinutes;
              wpmValues.Add(wpm);
            }
          }
          
          if (wpmValues.Count == 0)
            return 0;
          
          // Return average WPM rounded to nearest integer
          return (int)Math.Round(wpmValues.Average());
        }
      }
    }
    catch (Exception ex)
    {
      System.Diagnostics.Debug.WriteLine($"Failed to get average WPM: {ex.Message}");
      return 0;
    }
  }
}
