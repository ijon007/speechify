using System.Windows.Forms;

namespace WinFormTest;

public class ClippingPanel : Panel
{
  public ClippingPanel()
  {
    SetStyle(ControlStyles.AllPaintingInWmPaint | 
             ControlStyles.UserPaint | 
             ControlStyles.DoubleBuffer | 
             ControlStyles.ResizeRedraw, true);
    SetStyle(ControlStyles.ContainerControl, true);
  }

  protected override CreateParams CreateParams
  {
    get
    {
      CreateParams cp = base.CreateParams;
      cp.ExStyle |= 0x02000000; // WS_CLIPCHILDREN
      return cp;
    }
  }
}
