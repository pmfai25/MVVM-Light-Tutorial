using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace Logic.Ui
{
  public class DataErrorInfoViewModel : ViewModelBase, IDataErrorInfo
  {
    #region Fields
    private string firstname;
    private string lastname;
    private Dictionary<string, string> Errors { get; } = new Dictionary<string, string>();

    #endregion

    #region Properties and Commands

    /// <summary>
    /// Represetn the first name of a user
    /// </summary>
    public string Firstname
    {
      get
      {
        return firstname;
      }
      set
      {
        if(value == firstname)
        {
          return;
        }

        firstname = value;
        RaisePropertyChanged();
      }
    }

    public string Lastname
    {
      get
      {
        return lastname;
      }
      set
      {
        if (value == lastname)
        {
          return;
        }

        lastname = value;
        RaisePropertyChanged();
      }
    }

    public bool HasErrors => Errors.Any();
    public bool IsOk => !HasErrors;

    public RelayCommand OkCommand { get; private set; }

    #endregion

    #region Public methods

    public DataErrorInfoViewModel()
    {
      OkCommand = new RelayCommand(
      () => {
        Trace.TraceInformation("Ok");
        Lastname += Lastname; // TODO remove
      },
      () => IsOk);
    }

    #endregion


    #region Private methods

    private void collectErrors()
    {
      Errors.Clear();

      if (string.IsNullOrEmpty(Firstname))
      {
        Errors.Add(nameof(Firstname), "Firstname must be defined");
      }

      if (string.IsNullOrEmpty(Lastname))
      {
        Errors.Add(nameof(Lastname), "Lastname must be defined");
      }

      OkCommand.RaiseCanExecuteChanged();

    }

    #endregion

    #region IDataErrorInfo

    public string this[string propertyName]
    {
      get
      {
        collectErrors();
        return Errors.ContainsKey(propertyName) ? Errors[propertyName] : string.Empty;
      }
    }

    public string Error => string.Empty;

    #endregion
  }
}
