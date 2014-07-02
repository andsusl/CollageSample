using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CollageSample.Core.Models;
using System.Net;
using System.Threading.Tasks;

namespace CollageSample.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region bindable properties

        #region RecentUsers property - list of recently searcheed users
        const string RecentSearchedUsers = "RecentSearchedUsers";

        public ObservableCollection<string> RecentUsers
        {
            get;
            private set;
        }

        void UpdateRecentSearches(string value)
        {
            if (!RecentUsers.Contains(value))
            {
                RecentUsers.Add(value);
                SerializeRecentSearches();
            }
        }

        // In WP8.0 I was able to write ApplicationSettings[RecentSearchedUsers] = RecentUsers;
        // But here I need this workaround.
        void SerializeRecentSearches()
        {
            var compositeValue = new Windows.Storage.ApplicationDataCompositeValue();
            for (int i = 0; i < RecentUsers.Count; ++i)
            {
                compositeValue.Add(i.ToString(), RecentUsers[i]);
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
                    ObservableCollection<string> recents = new ObservableCollection<string>();
                    foreach (var value in compositeValue.Values)
                    {
                        recents.Add(value as string);
                    }
                    RecentUsers = recents;
                }
            }

            if (null == RecentUsers)
            {
                RecentUsers = new ObservableCollection<string>();
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
            throw new NotImplementedException();
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
                UpdateRecentSearches(value);
                if (null != UsersList && 0 == UsersList.Count &&
                    !string.IsNullOrEmpty(m_previousRequest) && value.StartsWith(m_previousRequest))
                {
                    return;
                }

                if (0 == string.Compare(value, m_previousRequest))
                {
                    return;
                }

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
