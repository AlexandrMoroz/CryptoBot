using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Interfesse;

namespace WindowsFormsApp1
{

    public class YobitCoinIdentetiFields
    {
        public decimal fee;
        public decimal fee_buyer;
        public decimal fee_seller;

    }
    public class YobitInfo : IGetInfo
    {
        public class Field
        {
            public Dictionary<string, YobitCoinIdentetiFields> pairs;
        }
        public YobitInfo()
        {
        }

        public Dictionary<string, TransformInfo> GetInfo()
        {
            Dictionary<string, TransformInfo> temp;
            string site = "https://yobit.net/api/3/info";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
            WebResponse resp = req.GetResponse();

            using (StreamReader stream = new StreamReader(
                      resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var a = JsonConvert.DeserializeObject<Field>(str);
                temp = a.pairs.ToDictionary(d => d.Key.Split('_')[0].ToUpper(), d => new TransformInfo(d.Value));
            }
            return temp;
        }

        public Task<Dictionary<string, TransformInfo>> GetInfoAsync()
        {
            return Task<Dictionary<string, TransformInfo>>.Factory.StartNew(() => GetInfo());
        }
    }
    public class YobitOrders : IGetOrders
    {
        public class Field
        {
            public List<List<decimal>> asks;
            public List<List<decimal>> bids;
        }
        public YobitOrders()
        {

        }
        public Dictionary<string, TransforfOrders> GetOrders(List<KeyValuePair<string, string>> arg)
        {

            Dictionary<string, TransforfOrders> temp = new Dictionary<string, TransforfOrders>();
            foreach (var i in arg)
            {
                temp.Add(i.Key + AccseptCoins.SPLITER + i.Value, GetOrder(i.Key, i.Value)); 
            }
            return temp;

        }
        public TransforfOrders GetOrder(string MainCoinName, string SecondCoinName)
        {
            if (MainCoinName == "USDT")
            {
                return new TransforfOrders();
            }
            string end = @"?limit=30";
            string start = "https://yobit.net/api/3/depth/";
            TransforfOrders temp = new TransforfOrders();
            string site = start + SecondCoinName.ToLower() + "_"+ MainCoinName.ToLower() + end;
            try
            {

                WebResponse resp = GetRequst.Requst(site);

            using (StreamReader stream = new StreamReader(
                     resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                if (!str.Contains("Invalid pair name") && !str.Contains("Ddos"))
                {
                    var res = JsonConvert.DeserializeObject<Dictionary<string, Field>>(str);
                    temp = new TransforfOrders(res.First().Value.asks, res.First().Value.bids);
                }
            }
            return temp;
            }
            catch (WebException e)
            {
                string message = e.Message;
                //MessageBoxButtons buttons = MessageBoxButtons.OK;
                //MessageBox.Show(message, "YOBIT", buttons);
                return temp;
            }
        }

        public Task<Dictionary<string, TransforfOrders>> GetOrdersAsync(List<KeyValuePair<string, string>> arg)
        {
            return  Task<Dictionary<string, TransforfOrders>>.Factory.StartNew(() => GetOrders(arg));
        }

        public Task<TransforfOrders> GetOrderAsync(string MainCoinName, string SecondCoinName)
        {
            return Task<TransforfOrders>.Factory.StartNew(() => GetOrder(MainCoinName,SecondCoinName));
        }
    }
    public class YoubitWallet : IWallet
    {
        public string Address = "GetDepositAddress";
        public string Balance = "getInfo";
        public Dictionary<string, TransformBallans> GetBalances()
        {
            var list = Helpers.GetCoins();
            var result = new Dictionary<string, TransformBallans>();
            var temp = YoubitPostRequst.PostString(Balance, "");
            if (!temp.Contains("funds"))
            {
                foreach (var item in list)
                {
                    result.Add(item, new TransformBallans(0, 0));
                }
            }
            else
            {
                var res = JObject.Parse(temp);
                foreach (var item in list)
                {
                    var avaible = Convert.ToDecimal(res["return"]["funds"][item]);
                    var onorders = Convert.ToDecimal(res["return"]["funds_incl_orders"][item]) - avaible;
                    result.Add(item, new TransformBallans(avaible, onorders));
                }

            }

            return result;
        }
        public Dictionary<string, string> GetDepositAddresses()
        {
            var list = Helpers.GetCoins();
            var result = new Dictionary<string, string>();
            foreach (var item in list)
            {
                var postData = new Dictionary<string, object>();
                postData.Add("coinName", item);
                postData.Add("need_new", 0);
                var temp = YoubitPostRequst.PostString(Address, postData.ToHttpPostString());
                var res = JObject.Parse(temp);
                result.Add(item, res["return"]["address"].ToString());
            }
            return result;
        }
        public Task<Dictionary<string, TransformBallans>> GetBalancesAsync()
        {
            return Task<Dictionary<string, TransformBallans>>.Factory.StartNew(() => GetBalances());
        }

        public Task<Dictionary<string, string>> GetDepositAddressesAsync()
        {
            return Task<Dictionary<string, string>>.Factory.StartNew(() => GetDepositAddresses());
        }
    }
    public class YobitTraid : ITrading
    {
        public string Trade = "Trade";
        public string Withdrow = "WithdrawCoinsToAddress";
        public string PostOrder(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            var postData = new Dictionary<string, object>();
            postData.Add("pair", currencyPair.ToLower() + "_btc");
            postData.Add("type", GetOrderType(type));
            postData.Add("rate", pricePerCoin);
            postData.Add("amount", amountQuote);
            var temp = YoubitPostRequst.PostString(Trade, postData.ToHttpPostString());
            var res = JObject.Parse(temp);
            return res["return"]["order_id"].ToString();
        }
        public TransformWithdrow PostWihdrow(string currencyPair, string adrress, decimal amountQuote)
        {
            var postData = new Dictionary<string, object>();
            postData.Add("coinName", currencyPair);
            postData.Add("amount", amountQuote);
            postData.Add("address", adrress);
            var temp = YoubitPostRequst.PostString(Withdrow, postData.ToHttpPostString());
            var res = JObject.Parse(temp);
            return new TransformWithdrow(0,currencyPair,adrress,amountQuote,DateTime.Now);
        }
        public Task<string> PostOrderAsync(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            return Task<string>.Factory.StartNew(() => PostOrder(currencyPair, type, pricePerCoin, amountQuote));
        }

        public Task<TransformWithdrow> PostWihdrowAsync(string currencyPair, string adrress, decimal amountQuote)
        {
            return Task<TransformWithdrow>.Factory.StartNew(() => PostWihdrow(currencyPair, adrress, amountQuote));
        }
        private string GetOrderType(OrderType arg)
        {
            switch (arg)
            {
                case OrderType.Buy:
                    return "buy";

                case OrderType.Sell:
                    return "sell";

            }
            throw new Exception("Wrong OrderType");
        }
    }

    public class Yobit:Stock
    {
        public Yobit()
        {
            StockName = "Yobit";
            Info = new YobitInfo();
            Orders = new YobitOrders();
            Ballans = new YoubitWallet();
            Traid = new YobitTraid();
        }
    }
}
