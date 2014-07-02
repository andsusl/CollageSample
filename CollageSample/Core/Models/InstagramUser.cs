using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CollageSample.Core.Models
{
    [DataContract]
    public class InstagramUser
    {
        private InstagramUser()
        {
        }

        public static Task<List<InstagramUser>> SearchUsersByNameAsync(string name, int resultsCount = 20)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            // create query parameter
            parameters.Add("q", name);
            // set number of users to return
            parameters.Add("count", resultsCount);

            return WevUtils.WebInterface.Get<List<InstagramUser>>("users/search", parameters);
        }

        #region Properties
        [DataMember(Name = "full_name")]
        string FullName
        {
            get;
            set;
        }

        [DataMember(Name = "username")]
        string UserName
        {
            get;
            set;
        }

        [DataMember(Name = "id")]
        public string Id
        {
            get;
            private set;
        }

        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(FullName))
                {
                    return UserName;
                }
                return FullName;
            }
        }

        [DataMember(Name = "profile_picture")]
        public string AvatarUrl
        {
            get;
            private set;
        }
        #endregion
    }
}
