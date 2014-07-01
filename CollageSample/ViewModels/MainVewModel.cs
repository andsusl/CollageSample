using CollageSample.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CollageSample.ViewModels
{
    public class MainVewModel : ViewModelBase
    {
        #region bindable properties

        #region RecentUsers property - list of recently viewed users
        public ObservableCollection<string> RecentUsers
        {
            get;
            private set;
        }
        #endregion

        #region UserName - part of user name to match
        private string m_userName;
        public string UserName
        {
            get
            {
                return m_userName;
            }

            set
            {
                m_userName = value;
                RaisePropertyChanged();
                RunSearchAsync(value);
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
        #endregion

        private List<InstagramUser> m_popularUsersList;

        public MainVewModel()
        {
            ViewUserCommand = new CollageSample.ViewModels.Utils.DelegateCommand<InstagramUser>(OnUserSelected);

            GetPopularUsersAsync();
        }

        private void GetPopularUsersAsync()
        {
        }

        private void RunSearchAsync(string value)
        {
            throw new NotImplementedException();
        }
    }
}
