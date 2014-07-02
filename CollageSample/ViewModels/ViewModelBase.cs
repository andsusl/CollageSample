using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CollageSample.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region bindable properties

        private  bool m_updateIsInProgerss;
        public bool UpdateIsInProgeress
        {
            get
            {
                return m_updateIsInProgerss;
            }

            protected set
            {
                m_updateIsInProgerss = value;
                RaisePropertyChanged();
            }
        }
        #endregion
        
        protected void InvokeWithProgress(System.Threading.Tasks.Task taskToDo)
        {
            UpdateIsInProgeress = true;
            Windows.ApplicationModel.Core.CoreApplication.GetCurrentView().CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, async () =>
                {
                    try
                    {
                        await taskToDo;
                    }
                    finally
                    {
                        UpdateIsInProgeress = false;
                    }
                });
        }

        public virtual void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            // do nothing
        }

        public virtual void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            // do nothing
        }

        public virtual void OnNavigatingFrom(Windows.UI.Xaml.Navigation.NavigatingCancelEventArgs e)
        {
            // do nothing
        }
    }
}
