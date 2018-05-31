using log4net;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace TestFrameWork.TestFramework
{
    class APIFunctions
    
    {
        public static bool HttpPost(string uri, List<KeyValuePair<string, string>> pairs)
        {
            bool Rtn = false;
            ILog log = LogManager.GetLogger(typeof(APIFunctions));
            var client = new HttpClient();


            var content = new FormUrlEncodedContent(pairs);

            var response = client.PostAsync(uri, content).Result;

            if (!response.IsSuccessStatusCode)
            {
                log.Error("Http post failed, status code=" + response.StatusCode);
                log.Error(response.Content);
                Rtn = false;
            }
            else
            {
                log.Info("Http post success, status code=" + response.StatusCode);
                log.Info(response.Content);
                Rtn = true;
            }
            return Rtn;
        }
        public static bool AddStepValues(string Step, string key, string value)
        {
            bool Rtn = false;


            return Rtn;
        }
    }
}
