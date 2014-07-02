using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace CollageSample.Core.WevUtils
{
    class WebInterface
    {
        const string ClientIDParameterName = "client_id";
        const string ClientID = "b784af13b5004e18a9ce00017af1daa4";

        static readonly Uri m_instagramApiBaseUrl = new Uri("https://api.instagram.com/v1/");

        static Uri CreateRequestUri(string relativePath, Dictionary<string, object> parametersDictionary = null)
        {
            Uri requestUri = null;
            StringBuilder builder = new StringBuilder(relativePath);
            Dictionary<string, object> parameters = null;

            if (null != parametersDictionary)
            {
                parameters = new Dictionary<string, object>(parametersDictionary);
            }
            else
            {
                parameters = new Dictionary<string, object>();
            }

            if ('?' != relativePath[relativePath.Length - 1])
            {
                builder.Append('?');
            }

            if (!parameters.ContainsKey(ClientIDParameterName))
            {
                parameters.Add(ClientIDParameterName, ClientID);
            }

            for (int i = 0; i < parameters.Count; ++i)
            {
                var pair = parameters.ElementAt(i);
                builder.AppendFormat("{0}={1}&", pair.Key, System.Net.WebUtility.UrlEncode(pair.Value.ToString()));
            }

            builder.Remove(builder.Length - 1, 1);

            requestUri = new Uri(m_instagramApiBaseUrl, builder.ToString());
            System.Diagnostics.Debug.WriteLine(requestUri.ToString());

            return requestUri;
        }

        public static Task<MessageResponse> Get<MessageResponse>(string relativePath, Dictionary<string, object> parametersDictionary = null) where MessageResponse : class
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(CreateRequestUri(relativePath, parametersDictionary));
            request.Method = "GET";
            return ProcessReqest<MessageResponse>(request);
        }

        static async Task<MessageResponse> ProcessReqest<MessageResponse>(HttpWebRequest request) where MessageResponse : class
        {
            using (HttpWebResponse response = (await Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, request).ConfigureAwait(false)) as HttpWebResponse)
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    // seems that for instagram root oject's name is always data
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RootObject<MessageResponse>));
                    var responseObj = serializer.ReadObject(responseStream) as RootObject<MessageResponse>;
                    return responseObj.Data as MessageResponse;
                }
            }
        }
    }

    [DataContract]
    class RootObject<MessageResponse>
    {
        [DataMember (Name = "data")]
        public MessageResponse Data 
        { 
            get; 
            set; 
        }
    }
}
