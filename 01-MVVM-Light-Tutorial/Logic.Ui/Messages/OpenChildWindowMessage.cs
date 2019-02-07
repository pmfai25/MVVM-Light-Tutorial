using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Ui.Messages
{
  /// <summary>
  /// If sent through the Messenger this message tells that a view 
  /// model wants to open the child window
  /// </summary>
  public class OpenChildWindowMessage
  {
    public OpenChildWindowMessage(string someText)
    {
      SomeText = someText;
    }


    /// <summary>
    /// Just some text that comes from the sender
    /// </summary>
    public string SomeText { get; set; }
  }
}
