using GalaSoft.MvvmLight.Messaging;
using Logic.Ui;
using Logic.Ui.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ui.Desktop
{
  /// <summary>
  /// Cesntral listener for all messages of the app
  /// </summary>
  public class MessageListener
  {
    #region Public methods

    public MessageListener()
    {
      initMessenger();
    }

    #endregion

    #region Private methods

    /// <summary>
    /// Is called by the constructor to define messages we're 
    /// interested in
    /// </summary>
    private void initMessenger()
    {
      // Hook to the message that states that some caller wants to 
      // open a child window
      Messenger.Default.Register<OpenChildWindowMessage>(
        this,
        msg =>
        {
          var window = new ChildWindow();
          var model = window.DataContext as ChildViewModel;
          if (model != null)
          {
            model.MessageFromParent = msg.SomeText;
          }

          window.ShowDialog();
        });
    }

    #endregion

    #region Properties

    /// <summary>
    /// We need this property so that this type can be put into the 
    /// ressources
    /// </summary>
    public bool BindableProperty => true;

    #endregion
  }
}
