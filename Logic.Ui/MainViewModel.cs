using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using Logic.Ui.BaseTypes;
using Logic.Ui.Messages;
using Logic.Ui.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;

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

        var personList = new ObservableCollection<PersonModel>();
        for (var i = 0; i < 10; i++)
        {
          personList.Add(new PersonModel
          {
            Firstname = Guid.NewGuid().ToString("N").Substring(0, 10),
            Lastname = Guid.NewGuid().ToString("N").Substring(0, 10)
          });
        }

        Persons = new ObservableCollection<PersonModel>(personList);
        PersonsView = CollectionViewSource.GetDefaultView(Persons) as ListCollectionView;

        // Inform the person model  When the current item of our 
        // persons view is changed.
        PersonsView.CurrentChanged += (s, e) =>
        {
          RaisePropertyChanged(() => PersonModel);
        };

        PersonsView.SortDescriptions.Clear();
        PersonsView.SortDescriptions.Add(new SortDescription(nameof(PersonModel.Firstname), ListSortDirection.Ascending));

        // Hook an event handler for PropertyChanged to all items currently in Persons
        foreach (var item in Persons)
        {
          item.PropertyChanged += PersonsOnPropertyChanged;
        }

        Persons.CollectionChanged += (s, e) =>
        {
          // Hook an event handler for PropertyChanged to all new added items in Persons
          foreach (INotifyPropertyChanged added in e.NewItems)
          {
            added.PropertyChanged += PersonsOnPropertyChanged;
          }

          // Un-hook the event handler for PropertyChanged to all removed items in Persons
          foreach (INotifyPropertyChanged removed in e.OldItems)
          {
            removed.PropertyChanged -= PersonsOnPropertyChanged;
          }
        };

        OpenChildCommand = new RelayCommand(() => MessengerInstance.Send(new OpenChildWindowMessage("Hello Child!")));
        AddPersonCommand = new RelayCommand(() => Persons.Add(new PersonModel()));
      }
    }

    /// <summary>
    /// This event handler refreshes the PersonsView.
    /// 
    /// So e.g. new sorting is displayed when one item's property 
    /// changed (see the hooks above).
    /// 
    /// In the following situations the refresh is *not* done:
    /// - Errors occur
    /// - Only the IsOk status flag has changed
    /// - While editing is active
    /// - While adding a new item ist active
    /// 
    /// Note: This way of sorting things in the view model is way
    /// faster than to do it in the DataGrid (view) itself and you 
    /// have more control how to sort.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PersonsOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if(e.PropertyName == nameof(PersonModel.HasErrors) || e.PropertyName==nameof(PersonModel.IsOk))
      {
        return;
      }

      if(PersonsView.IsEditingItem || PersonsView.IsAddingNew)
      {
        return;
      }

      PersonsView.Refresh();
    }

    /// <summary>
    /// Indicates the progress.
    /// </summary>
    public int Progress { get; set; }

    /// <summary>
    /// A person to edit.
    /// </summary>
    public PersonModel PersonModel
    {
      get => PersonsView?.CurrentItem as PersonModel;
      set
      {
        PersonsView?.MoveCurrentTo(value);
        RaisePropertyChanged();
      }
    }

    public ObservableCollection<PersonModel> Persons { get; set; }

    public ListCollectionView PersonsView { get; }


    /// <summary>
    /// Opens a new child window.
    /// </summary>
    public RelayCommand OpenChildCommand { get; private set; }

    /// <summary>
    /// Creates a new person
    /// </summary>
    public RelayCommand AddPersonCommand { get; private set; }

  }
}
