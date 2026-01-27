using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinFormTest;

public class NoScrollbarPanel : Panel
{
  [DllImport("user32.dll")]
  private static extern int ShowScrollBar(IntPtr hWnd, int wBar, bool bShow);

  private const int SB_HORZ = 0;
  private const int SB_VERT = 1;
  private const int SB_BOTH = 3;

  protected override CreateParams CreateParams
  {
    get
    {
      CreateParams cp = base.CreateParams;
      cp.Style |= 0x00200000; // WS_HSCROLL
      cp.Style |= 0x00100000; // WS_VSCROLL
      return cp;
    }
  }

  protected override void WndProc(ref Message m)
  {
    base.WndProc(ref m);
    
    // Hide scrollbars after they are created
    if (m.Msg == 0x0005) // WM_SIZE
    {
      ShowScrollBar(this.Handle, SB_HORZ, false);
      ShowScrollBar(this.Handle, SB_VERT, false);
    }
  }

  protected override void OnPaint(PaintEventArgs e)
  {
    base.OnPaint(e);
    // Ensure scrollbars are hidden after painting
    ShowScrollBar(this.Handle, SB_HORZ, false);
    ShowScrollBar(this.Handle, SB_VERT, false);
  }
}
