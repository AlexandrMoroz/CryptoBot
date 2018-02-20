using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Globalization;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Web;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace ConsoleApplication12
{
    public static class YoubitKey
    {
        public static string ApiKey = "";
        public static string SecretKey = "";
    }
    public static class Helpers
    {
        public static string ToHttpPostString(this Dictionary<string, object> dictionary)
        {
            var output = string.Empty;
            foreach (var entry in dictionary)
            {
                var valueString = entry.Value as string;
                if (valueString == null)
                {
                    output += "&" + entry.Key + "=" + entry.Value;
                }
                else
                {
                    output += "&" + entry.Key + "=" + valueString.Replace(' ', '+');
                }
            }

            return output;
        }
        public static List<string> GetCoins()
        {
            List<string> tp = new List<string>();
            using (StreamReader stream = new StreamReader(
                   "prox/coins.txt"))
            {
                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    tp.Add(line);
                }
            }
            return tp;
        }
        public static string DictionaryViewString(this Dictionary<decimal, decimal> arg)
        {
            string temp = "";
            foreach (var item in arg)
            {
                temp += "ЦЕНА :" + item.Key + " КОЛИЧЕСТВО :" + item.Value + "\n";
            }
            return temp;
        }

    }
    public static class YoubitPostRequst
    {
        internal static readonly DateTime DateTimeUnixEpochStart = new DateTime(2017, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc);
        private static UInt64 CurrentHttpPostNonce { get; set; }
        public static string GetCurrentHttpPostNonce()
        {
            var newHttpPostNonce = Convert.ToUInt64(Math.Round(DateTime.UtcNow.Subtract(DateTimeUnixEpochStart).TotalSeconds, MidpointRounding.AwayFromZero));
            if (newHttpPostNonce > CurrentHttpPostNonce)
            {
                CurrentHttpPostNonce = newHttpPostNonce;
            }
            else
            {
                CurrentHttpPostNonce += 1;
            }

            return CurrentHttpPostNonce.ToString();
        }
        const string ApiCallTemplate = "https://yobit.net/tapi/";
        private static string HashHmac(string message, string secret)
        {
            var keyByte = Encoding.UTF8.GetBytes(secret);
            string sign1 = string.Empty;
            byte[] inputBytes = Encoding.UTF8.GetBytes(message);
            using (var hmac = new HMACSHA512(keyByte))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);

                StringBuilder hex1 = new StringBuilder(hashValue.Length * 2);
                foreach (byte b in hashValue)
                {
                    hex1.AppendFormat("{0:x2}", b);
                }
                sign1 = hex1.ToString();
            }
            return sign1;
        }
        public static string PostString(string method, string postData)
        {
            var nonce = GetCurrentHttpPostNonce();
            string parameters = "&method="+method + postData + "&nonce=" + nonce;
            string sign1 = HashHmac(parameters, YoubitKey.SecretKey);

            WebRequest Request = (HttpWebRequest)System.Net.WebRequest.Create(ApiCallTemplate);
            Request.Method = "POST";
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.Headers.Add("Key", YoubitKey.ApiKey);
            Request.Headers.Add("Sign", sign1);
            Request.ContentLength = parameters.Length;

            byte[] inputBytes = Encoding.UTF8.GetBytes(parameters);
            using (var dataStream = Request.GetRequestStream())
            {
                dataStream.Write(inputBytes, 0, inputBytes.Length);
            }
            string temp= "{success=0}";
            try
            {
                WebResponse resp = Request.GetResponse();
                
                using (StreamReader stream = new StreamReader(
                     resp.GetResponseStream(), Encoding.UTF8))
                {
                    temp = stream.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                //string message = e.Message;
                //MessageBoxButtons buttons = MessageBoxButtons.OK;
                //MessageBox.Show(message, "Exmo", buttons);
                return temp;
            }
            return temp;
        }
    }

    static class Program
    {
        static void Main(string[] args)
        {

            string Address = "GetDepositAddress";
            var list = Helpers.GetCoins();
            var result = new Dictionary<string, string>();
            foreach (var item in list)
            {
                if (item.Contains("USD"))
                {
                    continue;
                }
                var postData = new Dictionary<string, object>();
                postData.Add("coinName", item.Split('-')[1]);
                postData.Add("need_new", 0);
                var temp = YoubitPostRequst.PostString(Address, postData.ToHttpPostString());
                
                var res = JObject.Parse(temp);
                if (Convert.ToInt32(res["success"].ToString())==1)
                {
                    result.Add(item.Split('-')[1], res["return"]["address"].ToString());
                }

            }
            foreach (var item in result)
            {
                Console.WriteLine(item.Key + "   " + item.Value);
            }
            Console.Read();
        }

 }}

