using Prism.Events;
using Sploosh.UI.Events;
using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace Sploosh.UI.Views
{
    public partial class MainView : Window
    {
        public MainView(IEventAggregator eventAggregator)
        {
            InitializeComponent();

            eventAggregator.GetEvent<ScreenShakeEvent>().Subscribe(ScreenShakeAnimation);
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
