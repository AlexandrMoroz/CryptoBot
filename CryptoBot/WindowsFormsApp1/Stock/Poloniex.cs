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
    public class PoloniexCoinsInfoFields
    {
        public string name;
        public decimal txFee;
        public int disabled;
    }
    public class PoloniexInfo: IGetInfo
    {

        public PoloniexInfo()
        {
            
        }
        public Dictionary<string, TransformInfo> GetInfo()
        {
            string site = "https://poloniex.com/public?command=returnCurrencies";

                WebResponse resp = GetRequst.Requst(site); 

                Dictionary<string, TransformInfo> temp;
                using (StreamReader stream = new StreamReader(
                     resp.GetResponseStream(), Encoding.UTF8))
                {

                    string str = stream.ReadToEnd();
                    var res = JsonConvert.DeserializeObject<Dictionary<string, PoloniexCoinsInfoFields>>(str);
                    temp = res.ToDictionary(d => d.Key, d => new TransformInfo(d.Value));
                }

            return temp;
        }

        public Task<Dictionary<string, TransformInfo>> GetInfoAsync()
        {
            return new Task<Dictionary<string, TransformInfo>>(() => GetInfo());
        }

        private string FormatString(string arg)
        {
            string res = arg.Split('_')[1] + '/' + arg.Split('_')[0];
            return res;
        }
    }
    public class PoloniexOrders: IGetOrders
    {
        public PoloniexOrders()
        {
            
        }
        //public Dictionary<string, TransforfOrders> GetAllOrderBook()
        //{
            
        //    string site = "https://poloniex.com/public?command=returnOrderBook&currencyPair=all";
        //    WebResponse resp = GetRequst.Requst(site);

        //    Dictionary<string, TransforfOrders> temp;
        //    PoloniexInfo polinfo = new PoloniexInfo();
        //    using (StreamReader stream = new StreamReader(
        //         resp.GetResponseStream(), Encoding.UTF8))
        //    {
        //        string str = stream.ReadToEnd();
        //        var res = JsonConvert.DeserializeObject<Dictionary<string, LiveAndPoloniexField>>(str);
        //        var t = res.Where(y => y.Key.Split('_')[0] == "BTC" && y.Key.Split('_')[1] != "BTC" && polinfo.GetInfo().ContainsKey(y.Key.Split('_')[1]));
        //        temp = t.ToDictionary(x => FormatString(x.Key), y => new TransforfOrders(y.Value.asks, y.Value.bids)); ;
        //    }
        //    return temp;
        //}
        public Dictionary<string, TransforfOrders> GetOrders(List<KeyValuePair<string, string>> arg)
        {
            
            Dictionary<string, TransforfOrders> temp = new Dictionary<string, TransforfOrders>();
            foreach (var i in arg)
            {
                var order = GetOrder(i.Key, i.Value);
                temp.Add(i.Key + AccseptCoins.SPLITER + i.Value, order);
            }

            return temp;
        }

        public TransforfOrders GetOrder(string MainCoinName, string SecondCoinName)
        {
            if (MainCoinName == "USD")
            {
                return new TransforfOrders();
            }
            string site = String.Format("https://poloniex.com/public?command=returnOrderBook&currencyPair={0}", MainCoinName + "_" + SecondCoinName);
            TransforfOrders temp = new TransforfOrders();
            try
            {
                WebResponse resp = PoloniexGetRequst.ProxyRequst(site);
                if (resp!=null)
                {
                using (StreamReader stream = new StreamReader(
                 resp.GetResponseStream(), Encoding.UTF8))
                {
                    string str = stream.ReadToEnd();
                    if (!str.Contains("error")){
                        var res = JsonConvert.DeserializeObject<LiveAndPoloniexField>(str);
                        temp = new TransforfOrders(res.asks, res.bids);
                    } }
                }
                return temp;
            }
            catch (WebException e)
            {
                string message = e.Message;
                //MessageBoxButtons buttons = MessageBoxButtons.OK;
                //MessageBox.Show(message, "POLONIEX", buttons);
                return temp;
            }
           
        }

        public Task<Dictionary<string, TransforfOrders>> GetOrdersAsync(List<KeyValuePair<string, string>> arg)
        {
            return  Task<Dictionary<string, TransforfOrders>>.Factory.StartNew(() => GetOrders(arg));
        }

        public Task<TransforfOrders> GetOrderAsync(string MainCoinName, string SecondCoinName)
        {
            return  Task<TransforfOrders>.Factory.StartNew(() => GetOrder(MainCoinName,SecondCoinName));            
        }


    }


    public class PoloniexWalletField
    {
        public decimal available;
        public decimal onOrders;
        public decimal btcValue;
    }
    public class PoloniexWallet : IWallet
    {
        public Dictionary<string, TransformBallans> GetBalances()
        {
            var postData = new Dictionary<string, object>();
            postData.Add("command", "returnCompleteBalances");
            postData.Add("nonce", PoloniexPostRequst.GetCurrentHttpPostNonce());

            var str = PoloniexPostRequst.PostString("tradingApi", postData.ToHttpPostString());
            Dictionary<string, TransformBallans> temp = new Dictionary<string, TransformBallans>();
            var t = JsonConvert.DeserializeObject<Dictionary<string, PoloniexWalletField>>(str);
            temp = t.ToDictionary(x => x.Key, y => new TransformBallans(y.Value));
            return temp;
        }
        public Dictionary<string, string> GetDepositAddresses()
        {
            var postData = new Dictionary<string, object>();
            postData.Add("command", "returnDepositAddresses");
            postData.Add("nonce", PoloniexPostRequst.GetCurrentHttpPostNonce());

            var str = PoloniexPostRequst.PostString("tradingApi", postData.ToHttpPostString());
            Dictionary<string, string> temp = new Dictionary<string, string>();
            var t = JsonConvert.DeserializeObject<Dictionary<string, string>>(str);

            return t;
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
    public class PoloniexTraid : ITrading
    {

        public string PostOrder(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            var postData = new Dictionary<string, object> {
                { "command", GetOrderType(type)},
                { "currencyPair", currencyPair },
                { "rate", Convert.ToString(pricePerCoin) },
                { "amount", Convert.ToString(amountQuote) },
                { "nonce", PoloniexPostRequst.GetCurrentHttpPostNonce()}
            };

            var str = PoloniexPostRequst.PostString("tradingApi", postData.ToHttpPostString());
            var tp = JsonConvert.DeserializeObject<JObject>(str);
            return tp.Value<string>("orderNumber");
        }
        public TransformWithdrow PostWithdrow(string currencyPair, string adrress, decimal amountQuote)
        {
            var postData = new Dictionary<string, object> {
                { "command", "withdraw"},
                { "currencyPair", currencyPair },
                { "adrress", adrress },
                { "amount", Convert.ToString(amountQuote) },
                { "nonce", PoloniexPostRequst.GetCurrentHttpPostNonce()}
            };

            var str = PoloniexPostRequst.PostString("tradingApi", postData.ToHttpPostString());
            var tp = JsonConvert.DeserializeObject<JObject>(str);
            return new TransformWithdrow(tp.Value<int>("id"), tp.Value<string>("currency"), tp.Value<string>("address"), tp.Value<decimal>("amount"), tp.Value<DateTime>("date"));
        }

        public Task<string> PostOrderAsync(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            return Task<string>.Factory.StartNew(() => PostOrder(currencyPair, type, pricePerCoin, amountQuote));
        }

        public Task<TransformWithdrow> PostWihdrowAsync(string currencyPair, string adrress, decimal amountQuote)
        {
            return Task<TransformWithdrow>.Factory.StartNew(() => PostWithdrow(currencyPair, adrress, amountQuote));
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
    public class Poloniex: Stock    
    {
        public Poloniex()
        {
            StockName = "Poloniex";
            Info = new PoloniexInfo();
            Orders = new PoloniexOrders();
            Ballans = new PoloniexWallet();
            Traid = new PoloniexTraid();
        }
    }
}
