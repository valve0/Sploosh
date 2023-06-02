using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Sploosh.Images;

namespace Sploosh.ViewModels
{
    //PropertyChanged(this, new PropertyChangedEventArgs("ImagePath"));
    //= new RelayCommand(SelectImageMethod, CanSelectImage);

    class MainViewModel : INotifyPropertyChanged
    {

        private ObservableCollection<ImageHolder> bombImages;

        public ObservableCollection<ImageHolder> BombImages
        {
            get { return bombImages; }
            set
            {

                if (bombImages != value)
                {
                    bombImages = value;

                    //Necessary for the view to update with the new property change
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(BombImages)));


                }
            }
        }



        public MainViewModel()
        {
           //Load Bombs
            
            BombImages = new ObservableCollection<ImageHolder>();

            for (int i = 0; i < 24; i++)
            {
                string bombImagePath = @"C:\Users\tommy\Documents\Visual Studio 2022\WPF\Sploosh\Images\BombAvailable.png";
                string bombName = $"Bomb {i + 1}";
                BombImages.Add(new ImageHolder(bombImagePath, bombName));
            }

            //Necessary for the view to update with the new property change
            PropertyChanged(this, new PropertyChangedEventArgs(nameof(BombImages)));

        }

        public event PropertyChangedEventHandler? PropertyChanged = delegate { };

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

    }
}

