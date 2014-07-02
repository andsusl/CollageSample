using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace CollageSample.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CollagePage : BasePage
    {

        public CollagePage()
        {
            this.InitializeComponent();

            DataContext = new ViewModels.CollageViewModel();
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            this.ImagesSelectionList.SelectionChanged += ImagesSelectionList_SelectionChanged;
        }

        void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            // TODO: GridViews handle bak key event first, so it doesn't navigate back from first press.
            Frame frame = Window.Current.Content as Frame;
            if (null != frame && frame.CanGoBack)
            {
                frame.GoBack();
                e.Handled = true;
            }
        }

        void ImagesSelectionList_SelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            foreach (var removed in e.RemovedItems)
            {
                CollageViewGrid.Items.Remove(removed);
            } 
           
            foreach (var added in e.AddedItems)
            {
                CollageViewGrid.Items.Add(added);
            }
        }

        private async void AppBarButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModels.CollageViewModel;
            if (null != viewModel && null != viewModel.SendCollageCommand &&
                viewModel.SendCollageCommand.CanExecute(CollageViewGrid))
            {
                viewModel.SendCollageCommand.Execute(CollageViewGrid);
            }
        }


    }
}
