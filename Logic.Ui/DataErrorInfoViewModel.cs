using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Logic.Ui
{
  public class DataErrorInfoViewModel : ViewModelBase, IDataErrorInfo
  {
    #region Fields

    private Dictionary<string, string> Errors { get; } = new Dictionary<string, string>();
    private static List<PropertyInfo> propertyInfos;

    #endregion

    #region Properties and Commands


    [Required(AllowEmptyStrings = false, ErrorMessage = "Firstname must be defined!")]
    [MaxLength(10, ErrorMessage = "A maximum of 10 characters is allowed!")]
    public string Firstname { get; set; }

    // NOTE You can either use the Required or the MinLenght attribute.
    // They don't work together. This is because Required internal is 
    // a MinLength=0 attribute, and MinLength can only be added once.
    [MinLength(4, ErrorMessage = "A minimum of 4 characters must be given!")]
    [MaxLength(20, ErrorMessage = "A maximum of 20 characters is allowed!")]
    public string Lastname { get; set; }

    public RelayCommand OkCommand { get; private set; }

    /// <summary>
    /// This is just a little runtime optimization. The PropertyInfos 
    /// don't change at run time, so they don't needed to be created 
    /// every time collectErrors() is called. Here they are inizialized
    /// once and the held in a static list.
    /// </summary>
    protected List<PropertyInfo> PropertyInfos
    {
      get
      {
        if(propertyInfos == null)
        {
          propertyInfos = GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => prop.IsDefined(typeof(RequiredAttribute), true)
              || prop.IsDefined(typeof(MaxLengthAttribute), true)
              || prop.IsDefined(typeof(MinLengthAttribute), true))
            .ToList();
        }

        return propertyInfos;
      }
    }

    public bool HasErrors => Errors.Any();
    public bool IsOk => !HasErrors;

    #endregion

    #region Public methods

    public DataErrorInfoViewModel()
    {
      OkCommand = new RelayCommand(
      () =>
      {
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

      PropertyInfos.ForEach(
          prop =>
          {
            var currentValue = prop.GetValue(this);
            var requiredAttr = prop.GetCustomAttribute<RequiredAttribute>();
            var maxLenAttr = prop.GetCustomAttribute<MaxLengthAttribute>();
            var minLenAttr = prop.GetCustomAttribute<MinLengthAttribute>();

            if (requiredAttr != null)
            {
              if (string.IsNullOrEmpty(currentValue?.ToString() ?? string.Empty))
              {
                Errors.Add(prop.Name, requiredAttr.ErrorMessage);
              }
            }

            if (maxLenAttr != null)
            {
              if ((currentValue?.ToString() ?? string.Empty).Length > maxLenAttr.Length)
              {
                Errors.Add(prop.Name, maxLenAttr.ErrorMessage);
              }
            }

            if (minLenAttr != null)
            {
              if ((currentValue?.ToString() ?? string.Empty).Length < minLenAttr.Length)
              {
                Errors.Add(prop.Name, minLenAttr.ErrorMessage);
              }
            }
          });

      // we have to this because the Dictionary does not implement INotifyPropertyChanged            
      RaisePropertyChanged(nameof(HasErrors));
      RaisePropertyChanged(nameof(IsOk));

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
