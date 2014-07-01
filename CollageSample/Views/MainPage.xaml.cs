using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace CollageSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Windows.ApplicationModel.Resources.ResourceLoader m_resourceLoader;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            m_resourceLoader = new Windows.ApplicationModel.Resources.ResourceLoader();
            
            UsersListTitleTextBlock.Text = m_resourceLoader.GetString("MostLikedUsersListTitle");

            UserNameSuggestBox.TextChanged += UserNameSuggestBox_TextChanged;
        }

        void UserNameSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (string.IsNullOrEmpty(sender.Text))
            {
                UsersListTitleTextBlock.Text = m_resourceLoader.GetString("MostLikedUsersListTitle");
            }
            else
            {
                UsersListTitleTextBlock.Text = string.Format("{0} {1}", m_resourceLoader.GetString("SearchResultsListTitle"), sender.Text);
            }

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Enhancement: clear search box, update cached lists
        }
    }
}
