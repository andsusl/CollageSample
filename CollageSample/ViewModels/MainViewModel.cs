using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using CollageSample.Core.Models;
using System.Net;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CollageSample.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region bindable properties

        #region RecentUsers property - list of recently searcheed users
        const string RecentSearchedUsers = "RecentSearchedUsers";

        Dictionary<string, long> m_dict = null;
        public List<string> RecentUsers
        {
            get
            {
                var query = (from pair in m_dict orderby pair.Value select pair.Key).Take(20);
                return query.ToList();
            }
        }

        void UpdateRecentSearches(string value)
        {
            if (!m_dict.ContainsKey(value))
            {
                m_dict.Add(value, 1);
            }
            else
            {
                m_dict[value] = m_dict[value] + 1;
            }
            SerializeRecentSearches();
            RaisePropertyChanged("RecentUsers");
        }

        // In WP8.0 I was able to write ApplicationSettings[RecentSearchedUsers] = RecentUsers;
        // But here I need this workaround.
        void SerializeRecentSearches()
        {
            var compositeValue = new Windows.Storage.ApplicationDataCompositeValue();
            for (int i = 0; i < RecentUsers.Count; ++i)
            {
                compositeValue.Add(m_dict.Keys.ElementAt(i), m_dict.Values.ElementAt(i));
            }

            Windows.Storage.ApplicationData.Current.LocalSettings.Values[RecentSearchedUsers] = compositeValue;
        }

        void DeserializeRecentSearches()
        {
            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey(RecentSearchedUsers))
            {
                var compositeValue = Windows.Storage.ApplicationData.Current.LocalSettings.Values[RecentSearchedUsers] as Windows.Storage.ApplicationDataCompositeValue;
                if (null != compositeValue)
                {
                    m_dict = new Dictionary<string, long>();
                    foreach (var pair in compositeValue)
                    {
                        m_dict.Add(pair.Key, (long)pair.Value);
                    }
                }
            }

            if (null == m_dict)
            {
                m_dict = new Dictionary<string, long>();
            }

        }

        #endregion

        #region UsersList - list to display
        private List<InstagramUser> m_usersList;
        public List<InstagramUser> UsersList
        {
            get
            {
                return m_usersList;
            }

            set
            {
                m_usersList = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region ViewUserCommand - command to process user selection: change page and so on
        public ICommand ViewUserCommand
        {
            get;

            private set;
        }

        void OnUserSelected(InstagramUser user)
        {
            // TODO: needs some location service
            (Window.Current.Content as Frame).Navigate(typeof(CollageSample.Views.CollagePage), user);
        }
        #endregion

        #region StartSearchCommand - command which runs search request
        public ICommand StartSearchCommand
        {
            get;
            private set;
        }

        string m_previousRequest = string.Empty;

        private async Task RunSearchAsync(string value)
        {
            try
            {
                if (0 == string.Compare(value, m_previousRequest))
                {
                    return;
                }

                UpdateRecentSearches(value);

                UsersList = await InstagramUser.SearchUsersByNameAsync(value);
                m_previousRequest = value;
            }
            catch (WebException wexc)
            {
                System.Diagnostics.Debug.WriteLine("{0} - {1}", wexc.Status, wexc.Message);
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine("{0}", exc.Message);
            }
        }
        #endregion

        #endregion

        public MainViewModel()
        {
            DeserializeRecentSearches();

            ViewUserCommand = new CollageSample.ViewModels.Utils.DelegateCommand<InstagramUser>(OnUserSelected);
            StartSearchCommand = new CollageSample.ViewModels.Utils.DelegateCommand<string>((str) => 
                {
                    InvokeWithProgress(RunSearchAsync(str));
                });
        }
    }
}
