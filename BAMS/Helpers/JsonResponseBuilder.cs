using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMS.Helpers
{


    public class JsonResponseBuilder
    {
        public const string OK = "OK";
        public const string FAILED = "FAILED";
        public const string ERROR = "ERROR";

        public const string NOT_FOUND = "NOT_FOUND";
        public const string INVALID_REQUEST = "INVALID_REQUEST";
        public const string REQUEST_DENIED = "REQUEST_DENIED";
        


        public JObject _response { get; set; }


        public static JsonResponseBuilder GetInstance()
        {
            return new JsonResponseBuilder();
        }


        #region quick response


        public static JsonResponseBuilder GetSuccessResponseBuilder()
        {
            return GetInstance().SetStatus(OK);                
        }

        public static string GetSuccessResponse()
        {
            return GetResponse(OK);
        }

        public static string GetFailedResponse(string message = null)
        {
            return GetResponse(FAILED, message);
        }

        public static string GetErrorResponse(string message = null)
        {
            return GetResponse(ERROR, message);
        }


        public static string GetResponse(string status, string message = null)
        {
            return GetInstance()
                .SetStatus(status)
                .SetMessage(message)
                .Build();
        }


        #endregion


        public JsonResponseBuilder()
        {
            _response = new JObject();
        }

        public JsonResponseBuilder SetStatus(string status)
        {
            _response["status"] = status;
            return this;
        }

        public JsonResponseBuilder SetMessage(string message)
        {
            if (message != null)
            {
                _response["message"] = message;
            }            
            return this;
        }

        public JsonResponseBuilder SetProperty(string name, string value)
        {
            _response[name] = value;
            return this;
        }

        public JsonResponseBuilder SetProperty(string name, int value)
        {
            _response[name] = value;
            return this;
        }


        public JsonResponseBuilder SetProperty(string name, JToken content)
        {
            _response[name] = content;
            return this;
        }

        public JsonResponseBuilder Add(JToken content)
        {
            _response.Merge(content);
            return this;
        }

        public string Build()
        {
            return _response.ToString();
        }
    }
}

