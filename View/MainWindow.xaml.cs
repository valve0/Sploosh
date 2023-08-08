using System;
using System.Windows;
using System.Windows.Media.Animation;
using Sploosh.ViewModel;

namespace Sploosh.View
{
    public partial class MainWindow : Window
    {
        private MainViewModel _mainViewModel;

        public MainWindow(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            DataContext = _mainViewModel;

            _mainViewModel.ScreenShakeAnimationEvent1 += ScreenShakeAnimation;

            InitializeComponent();
        }

        private void ScreenShakeAnimation()
        {
            DoubleAnimation leftAnimation = new DoubleAnimation();

            leftAnimation.From = this.Left;
            leftAnimation.To = this.Left - 30;
            leftAnimation.Duration = TimeSpan.FromSeconds(0.15);
            leftAnimation.AutoReverse = true;
            this.BeginAnimation(Window.LeftProperty, leftAnimation);
        }

    }
}
