using Windows.UI.Xaml;
using IncrementalLoadingDemo.SampleData;

namespace IncrementalLoadingDemo
{
    public sealed partial class MainPage
    {
        private readonly MainPageSampleData source;
        public MainPage()
        {
            InitializeComponent();
            DataContext = source = new MainPageSampleData();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //加载缓存
            await source.Source.LoadCache();

            //刷新
            await source.Source.RefreshRequest();
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (!source.Source.IsRefreshing)
            {
                await source.Source.RefreshRequest();
            }
        }
    }
}
