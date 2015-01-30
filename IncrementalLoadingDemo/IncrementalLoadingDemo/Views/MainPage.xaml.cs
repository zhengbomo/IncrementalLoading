using Windows.UI.Xaml;
using IncrementalLoadingDemo.SampleData;

namespace IncrementalLoadingDemo.Views
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (NormalPage));
        }

        private void ButtonBase1_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(EmptyPage));
        }
    }
}
