using System.Speech.Recognition;

namespace WinFormTest;

public class SpeechRecognitionService : IDisposable
{
  private SpeechRecognitionEngine? recognitionEngine;
  private bool isListening = false;

  public event EventHandler<string>? SpeechRecognized;
  public event EventHandler<string>? SpeechRejected;

  public bool IsListening => isListening;

  public SpeechRecognitionService()
  {
    try
    {
      recognitionEngine = new SpeechRecognitionEngine();
      
      // Load default dictation grammar
      var dictationGrammar = new DictationGrammar();
      recognitionEngine.LoadGrammar(dictationGrammar);

      // Set recognition confidence threshold
      recognitionEngine.SpeechRecognized += RecognitionEngine_SpeechRecognized;
      recognitionEngine.SpeechRecognitionRejected += RecognitionEngine_SpeechRecognitionRejected;
    }
    catch (Exception ex)
    {
      throw new Exception($"Failed to initialize speech recognition: {ex.Message}", ex);
    }
  }

  public void StartListening()
  {
    if (recognitionEngine == null || isListening)
      return;

    try
    {
      recognitionEngine.SetInputToDefaultAudioDevice();
      recognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
      isListening = true;
    }
    catch (Exception ex)
    {
      throw new Exception($"Failed to start listening: {ex.Message}", ex);
    }
  }

  public void StopListening()
  {
    if (recognitionEngine == null || !isListening)
      return;

    try
    {
      recognitionEngine.RecognizeAsyncStop();
      isListening = false;
    }
    catch
    {
      // Ignore errors when stopping
    }
  }

  private void RecognitionEngine_SpeechRecognized(object? sender, SpeechRecognizedEventArgs e)
  {
    // Lowered threshold from 0.7 to 0.3 to accept more recognition results
    // Even with lower confidence, we want to capture speech input
    if (e.Result.Confidence >= 0.3 && !string.IsNullOrWhiteSpace(e.Result.Text))
    {
      SpeechRecognized?.Invoke(this, e.Result.Text);
    }
    else
    {
      SpeechRejected?.Invoke(this, e.Result.Text);
    }
  }

  private void RecognitionEngine_SpeechRecognitionRejected(object? sender, SpeechRecognitionRejectedEventArgs e)
  {
    SpeechRejected?.Invoke(this, "Recognition rejected");
  }

  public void Dispose()
  {
    StopListening();
    recognitionEngine?.Dispose();
  }
}
