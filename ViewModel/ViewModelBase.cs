using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Sploosh.ViewModel
{
  public class ViewModelBase : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public virtual Task LoadAsync() => Task.CompletedTask;
  }
}

//protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
//{
//    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//}

//protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
//{
//    if (EqualityComparer<T>.Default.Equals(field, value)) return false;
//    field = value;
//    RaisePropertyChanged(propertyName);
//    return true;
//}
