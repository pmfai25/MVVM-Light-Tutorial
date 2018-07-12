using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Logic.Ui.BaseTypes;
using Logic.Ui.Messages;
using Logic.Ui.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Logic.Ui
{
  /// <summary>
  /// This class contains properties that the main View can data bind to.
  /// <para>
  /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
  /// </para>
  /// <para>
  /// You can also use Blend to data bind with the tool's support.
  /// </para>
  /// <para>
  /// See http://www.galasoft.ch/mvvm
  /// </para>
  /// </summary>
  public class MainViewModel : BaseViewModel
  {
    /// <summary>
    /// Initializes a new instance of the MainViewModel class.
    /// </summary>
    public MainViewModel()
    {
      if (IsInDesignMode)
      {
        WindowTitle = "MVVM Light tutorial (design mode)";
      }
      else
      {
        WindowTitle = "MVVM Light tutorial";

        Task.Run(
          () =>
          {
            Task.Delay(2000).ContinueWith(
                t =>
                {
                  while (Progress < 100)
                  {
                    DispatcherHelper.RunAsync(() => Progress += 5);
                    Task.Delay(500).Wait();
                  }
                }
              );
          }
          );

        PersonModel = new PersonModel();
        var personList = new ObservableCollection<PersonModel>();
        for (var i = 0; i < 10; i++)
        {
          personList.Add(new PersonModel
          {
            Firstname = Guid.NewGuid().ToString("N").Substring(0, 10),
            Lastname = Guid.NewGuid().ToString("N").Substring(0, 10)
          });
        }

        Persons = personList;

        OpenChildCommand = new RelayCommand(() => MessengerInstance.Send(new OpenChildWindowMessage("Hello Child!")));
        AddPersonCommand = new RelayCommand(() => Persons.Add(new PersonModel()));
      }
    }

    /// <summary>
    /// A person to edit.
    /// </summary>
    public PersonModel PersonModel { get; set; } = new PersonModel();

    /// <summary>
    /// Indicates the progress.
    /// </summary>
    public int Progress { get; set; }

    /// <summary>
    /// Opens a new child window.
    /// </summary>
    public RelayCommand OpenChildCommand { get; private set; }

    /// <summary>
    /// Creates a new person
    /// </summary>
    public RelayCommand AddPersonCommand { get; private set; }

    public ObservableCollection<PersonModel> Persons { get; set; }
  }
}
