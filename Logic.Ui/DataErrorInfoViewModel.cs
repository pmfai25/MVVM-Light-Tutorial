using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;

namespace Logic.Ui
{
  public class DataErrorInfoViewModel : ViewModelBase, IDataErrorInfo
  {
    #region Fields

    private Dictionary<string, string> Errors { get; } = new Dictionary<string, string>();

    #endregion

    #region Properties and Commands

    /// <summary>
    /// Represents the first name of a user
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Firstname must be defined")]
    public string Firstname { get; set; }

    /// <summary>
    /// Represents the last name of a user
    /// </summary>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Lastname must be defined")]
    public string Lastname { get; set; }


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
