using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Dsm
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnLoaded(RoutedEventArgs e)
        {
            base.OnLoaded(e);
            var screen = Screens.ScreenFromVisual(this);
           
        }
    }
}