using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace CollageSample.Views
{
    public class BasePage : Page
    {
        Grid m_mainContainer = new Grid();
        Grid m_grayGrid = new Grid() { Visibility = Windows.UI.Xaml.Visibility.Collapsed };
        ProgressBar m_progress = new ProgressBar() { IsIndeterminate = true, VerticalAlignment = VerticalAlignment.Top };

        object m_previousContext = null;
        public BasePage()
        {
            m_grayGrid.Children.Add(m_progress);
            base.DataContextChanged += BasePage_DataContextChanged;
        }

        void BasePage_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (!object.Equals(m_previousContext, base.DataContext))
            {
                if (null != m_previousContext && m_previousContext is CollageSample.ViewModels.ViewModelBase)
                {
                    (m_previousContext as CollageSample.ViewModels.ViewModelBase).PropertyChanged -= ViewModel_PropertyChanged;
                }
                m_previousContext = base.DataContext;

                var viewModel = GetViewModel();
                if (null != viewModel)
                {
                    viewModel.PropertyChanged += ViewModel_PropertyChanged;
                }
            }
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "UpdateIsInProgeress")
            {
                if (GetViewModel().UpdateIsInProgeress)
                {
                    m_grayGrid.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    m_progress.IsEnabled = true;
                    
                    base.IsEnabled = false;
                }
                else
                {
                    m_grayGrid.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                    m_progress.IsEnabled = false;

                    base.IsEnabled = true;
                }
            }
        }

        CollageSample.ViewModels.ViewModelBase GetViewModel()
        {
            return base.DataContext as CollageSample.ViewModels.ViewModelBase;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var viewModel = GetViewModel();
            if (null != viewModel)
            {
                viewModel.OnNavigatedTo(e);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var viewModel = GetViewModel();
            if (null != viewModel)
            {
                viewModel.OnNavigatedFrom(e);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            var viewModel = GetViewModel();
            if (null != viewModel)
            {
                viewModel.OnNavigatingFrom(e);
            }
        }

        public new UIElement Content 
        { 
            get
            {
                return m_mainContainer;
            }

            set
            {
                m_mainContainer.Children.Clear();
                m_mainContainer.Children.Add(m_grayGrid);
                m_mainContainer.Children.Add(value);

                base.Content = m_mainContainer;
            }
        }
    }
}
