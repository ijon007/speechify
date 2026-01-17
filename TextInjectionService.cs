using System.Windows.Forms;

namespace WinFormTest;

public class TextInjectionService
{
  private IntPtr originalForegroundWindow = IntPtr.Zero;

  public void SaveForegroundWindow(IntPtr hWnd)
  {
    originalForegroundWindow = hWnd;
  }

  public void InjectText(string text)
  {
    if (string.IsNullOrEmpty(text))
      return;

    try
    {
      // Restore focus to the original window
      if (originalForegroundWindow != IntPtr.Zero)
      {
        WindowsApiHelper.SetForegroundWindow(originalForegroundWindow);
        System.Threading.Thread.Sleep(150); // Give window time to receive focus
      }

      // Try clipboard method first (more reliable across apps)
      InjectViaClipboard(text);
    }
    catch
    {
      // Last resort: just set clipboard
      try
      {
        Clipboard.SetText(text);
      }
      catch
      {
        // Ignore clipboard errors
      }
    }
  }

  private void InjectViaClipboard(string text)
  {
    try
    {
      // Save current clipboard
      string? originalClipboard = null;
      try
      {
        originalClipboard = Clipboard.GetText();
      }
      catch { }

      // Clear and set text to clipboard
      Clipboard.Clear();
      System.Threading.Thread.Sleep(10);
      Clipboard.SetText(text);
      
      // Verify clipboard was set
      string? verifyClipboard = null;
      try
      {
        verifyClipboard = Clipboard.GetText();
      }
      catch { }

      if (verifyClipboard != text)
      {
        // Retry setting clipboard
        Clipboard.SetText(text);
        System.Threading.Thread.Sleep(50);
      }

      // Ensure window still has focus
      if (originalForegroundWindow != IntPtr.Zero)
      {
        WindowsApiHelper.SetForegroundWindow(originalForegroundWindow);
        System.Threading.Thread.Sleep(50);
      }

      // Send Ctrl+V to paste
      SendKeys.SendWait("^v");
      System.Threading.Thread.Sleep(50);

      // Restore original clipboard after a delay
      Task.Delay(200).ContinueWith(_ =>
      {
        try
        {
          if (!string.IsNullOrEmpty(originalClipboard))
          {
            Clipboard.SetText(originalClipboard);
          }
          else
          {
            Clipboard.Clear();
          }
        }
        catch { }
      });
    }
    catch
    {
      // If clipboard method fails, just set clipboard as fallback
      try
      {
        Clipboard.SetText(text);
      }
      catch { }
    }
  }

  public void RestoreFocus()
  {
    try
    {
      if (originalForegroundWindow != IntPtr.Zero)
      {
        WindowsApiHelper.SetForegroundWindow(originalForegroundWindow);
      }
    }
    catch { }
  }
}
