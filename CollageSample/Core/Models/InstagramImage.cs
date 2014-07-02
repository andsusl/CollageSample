using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CollageSample.Core.Models
{
    [DataContract]
    public class InstagramImage
    {
        #region Properties
        [DataMember(Name = "likes")]
        private LikesInfo LikesObj
        {
            get;
            set;
        }

        public int Likes
        {
            get
            {
                return LikesObj.Count;
            }
        }

        [DataMember(Name = "images")]
        private ImageInfo ImageObj
        {
            get;
            set;
        }

        public string ImageUrl
        {
            get
            {
                return ImageObj.Standard.Url;
            }
        }
        #endregion
    }

    [DataContract]
    class LikesInfo
    {
        [DataMember(Name = "count")]
        public int Count
        {
            get;
            private set;
        }
    }

    [DataContract]
    class ImageInfo
    {
        [DataMember(Name = "standard_resolution")]
        public Resolution Standard
        {
            get;
            private set;
        }
    }

    [DataContract]
    class Resolution
    {
        [DataMember(Name = "url")]
        public string Url
        {
            get;
            private set;
        }
    }
}
