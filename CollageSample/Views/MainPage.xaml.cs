using CollageSample.Views;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace CollageSample.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : BasePage
    {
        private Windows.ApplicationModel.Resources.ResourceLoader m_resourceLoader;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            DataContext = new CollageSample.ViewModels.MainViewModel();

            m_resourceLoader = new Windows.ApplicationModel.Resources.ResourceLoader();
            
            UserNameSuggestBox.KeyUp += UserNameSuggestBox_KeyUp;
            ListOfUsers.Items.VectorChanged += Items_VectorChanged;
        }

        void Items_VectorChanged(Windows.Foundation.Collections.IObservableVector<object> sender, Windows.Foundation.Collections.IVectorChangedEventArgs @event)
        {
            // TODO: create UserControl: listbox with empty message
            if (0 == ListOfUsers.Items.Count)
            {
                NoResultsTextBlock.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        // default behaviour - run search by enter key
        void UserNameSuggestBox_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                System.Windows.Input.ICommand command = (DataContext as CollageSample.ViewModels.MainViewModel).StartSearchCommand;

                if (null != command && command.CanExecute(UserNameSuggestBox.Text))
                {
                    NoResultsTextBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    command.Execute(UserNameSuggestBox.Text);
                }
            }
        }
    }
}
