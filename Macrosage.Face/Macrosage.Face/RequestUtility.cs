using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Macrosage.Face
{
    internal class RequestUtility
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(RequestUtility));

        private static readonly object _lock = new object();

        private static RestClient _client;
        private static RestClient Client
        {
            get
            {
                if (_client == null)
                {
                    lock (_lock)
                    {
                        if (_client == null)
                        {
                            _client = new RestClient(ApiConfig._baseUri);
                        }
                    }
                }
                return _client;
            }
        }


        private static IRestRequest PrapareRequest(string url, Method method, IDictionary<string, object> parameter = null)
        {
            IRestRequest request = new RestRequest(url);
            request.Method = method;


            if (parameter?.Count() > 0)
            {
                foreach (KeyValuePair<string, object> kv in parameter)
                {
                    if (method == Method.GET)
                    {
                        request.AddQueryParameter(kv.Key, kv.Value.ToString());
                    }

                    if (method == Method.POST)
                    {
                        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                        request.AddParameter(new Parameter() { Name = kv.Key, Value = kv.Value.ToString(), Type = ParameterType.GetOrPost });
                    }
                }
            }
            return request;
        }


        #region 获取请求结果 返回string
        private static string Execute(string url, Method method, IDictionary<string, object> parameter = null)
        {

            IRestRequest request = PrapareRequest(url, method, parameter);

            IRestResponse response = Client.Execute(request);
            Console.WriteLine(response?.Content);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                //记录日志
                // log.Error(response);
            }
            return response.Content;
        }

        public static string ExecuteGet(string url, IDictionary<string, object> parameter = null)
        {
            return Execute(url, Method.GET, parameter);
        }

        public static string ExecutePost(string url, IDictionary<string, object> parameter = null)
        {
            return Execute(url, Method.POST, parameter);
        }
        #endregion

        #region 获取请求结果，返回T
        private static T Execute<T>(string url, Method method, IDictionary<string, object> parameter = null) where T : class, new()
        {
            IRestRequest request = PrapareRequest(url, method, parameter);

            IRestResponse<T> response = Client.Execute<T>(request);

            Console.WriteLine(response?.Content);
            if (response.StatusCode == 0)
            {
                // log.Error(response);
                return default(T);
            }

            return response.Data;
        }

        public static T ExecuteGet<T>(string url, IDictionary<string, object> parameter = null) where T : class, new()
        {
            return Execute<T>(url, Method.GET, parameter);
        }

        public static T ExecutePost<T>(string url, IDictionary<string, object> parameter = null) where T : class, new()
        {
            return Execute<T>(url, Method.POST, parameter);
        }
        #endregion

        #region 不需要返回结果，只需要异步执行，并记录
        private static void ExecuteAsync(string url, Method method, IDictionary<string, object> parameter = null)
        {
            IRestRequest request = PrapareRequest(url, method, parameter);

            Client.ExecuteAsync(request, response =>
            {
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    // log.Error("url:" + url, response.ErrorException);
                }
            });
        }

        public static void ExecuteAsyncGet(string url, IDictionary<string, object> parameter = null)
        {
            ExecuteAsync(url, Method.GET, parameter);
        }

        public static void ExecuteAsyncPost(string url, IDictionary<string, object> parameter = null)
        {
            ExecuteAsync(url, Method.POST, parameter);
        }
        #endregion
    }
}