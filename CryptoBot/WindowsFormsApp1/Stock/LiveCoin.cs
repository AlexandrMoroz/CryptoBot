using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Interfesse;
namespace WindowsFormsApp1
{
    public class LiveCoinInfoFields
    {
        public string symbol;
        public string walletStatus;
        public decimal withdrawFee;
    }
    public class LiveCoinInfo : IGetInfo
    {
        public class Fields
        {
            public bool success;
            public List<LiveCoinInfoFields> info;
        }

        public LiveCoinInfo()
        {

        }
        public Dictionary<string, TransformInfo> GetInfo()
        {
            string site = "https://api.livecoin.net/info/coinInfo";

            Dictionary<string, TransformInfo> temp;

            WebResponse resp = LiveCoinGetRequst.Requst(site);

            using (StreamReader stream = new StreamReader(
                    resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                if (!str.Contains("Error 502"))
                {
                    var res = JsonConvert.DeserializeObject<Fields>(str);
                    temp = res.info.ToDictionary(d => d.symbol, x => new TransformInfo(x));
                }
                else
                {
                    temp = new Dictionary<string, TransformInfo>();
                }
            }


            return temp;
        }
        //public bool StatusCheaker(string arg)
        //{
        //    if (Data[arg].Data.Status)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        public Task<Dictionary<string, TransformInfo>> GetInfoAsync()
        {
            return new Task<Dictionary<string, TransformInfo>>(() => GetInfo());
        }
    }
    public class LiveCoinOrders : IGetOrders
    {

        public LiveCoinOrders()
        {

        }



        public TransformOrders GetOrder(string MainCoinName, string SecondCoinName)
        {
            if (MainCoinName == "USDT")
            {
                return new TransformOrders();
            }
            string site = String.Format("https://api.livecoin.net/exchange/order_book?currencyPair={0}", SecondCoinName + "/" + MainCoinName);
            List<List<decimal>> asks = new List<List<decimal>>();
            List<List<decimal>> bids = new List<List<decimal>>();
            try
            {
                WebResponse resp = LiveCoinGetRequst.Requst(site);
                using (StreamReader stream = new StreamReader(
                 resp.GetResponseStream(), Encoding.UTF8))
                {
                    string str = stream.ReadToEnd();
                    dynamic res = JsonConvert.DeserializeObject(str);
                    if (res.success == null )
                    {
                        foreach (var item in res.asks)
                        {
                            string first = item.First.Value;
                            decimal cost = Decimal.Parse(first.Replace('.', ','), NumberStyles.Float);
                            long second = item.Last.Value;
                            decimal weight =second;
                            var temp = new List<decimal>();
                            temp.Add(cost);
                            temp.Add(weight);
                            asks.Add(temp);
                        }
                        foreach (var item in res.bids)
                        {
                            string first = item.First.Value;
                            decimal cost = Decimal.Parse(first.Replace('.', ','), NumberStyles.Float);
                            long second = item.Last.Value;
                            decimal weight = second;
                            var temp = new List<decimal>();
                            temp.Add(cost);
                            temp.Add(weight);
                            bids.Add(temp);
                        }
                    }
                }

                return new TransformOrders(asks,bids);
            }
            catch (WebException e)
            {
                string message = e.Message;
                //MessageBoxButtons buttons = MessageBoxButtons.OK;
                //MessageBox.Show(message, "LIVECOIN", buttons);
                return new TransformOrders();
            }
           
        }
        public Dictionary<string, TransformOrders> GetOrders(List<KeyValuePair<string, string>> arg)
        {
            Dictionary<string, TransformOrders> temp = new Dictionary<string, TransformOrders>();
            Dictionary<string, Task<TransformOrders>> tempAsync = new Dictionary<string, Task<TransformOrders>>();
            foreach (var i in arg)
            {
                tempAsync.Add(i.Key + AccseptCoins.SPLITER + i.Value, GetOrderAsync(i.Key, i.Value));
                temp.Add(i.Key + AccseptCoins.SPLITER + i.Value, new TransformOrders());
            }
            foreach (var i in tempAsync)
            {
                temp[i.Key] = i.Value.Result;
            }
            return temp;
        }

        public Task<Dictionary<string, TransformOrders>> GetOrdersAsync(List<KeyValuePair<string, string>> arg)
        {
            return Task<Dictionary<string, TransformOrders>>.Factory.StartNew(() => GetOrders(arg));
        }

        public Task<TransformOrders> GetOrderAsync(string MainCoinName, string SecondCoinName)
        {
            return Task<TransformOrders>.Factory.StartNew(() => GetOrder(MainCoinName, SecondCoinName));
        }
    }

    public class LiveCoinWallet : IWallet
    {
        public class Field
        {
            public string type;
            public string currency;
            [JsonProperty("value")]
            public string price;
        }
        private static string Payment = "payment/";
        public Dictionary<string, TransformBallans> GetBalances()
        {
            WebResponse response = LiveCoinGetRequst.AuthRequst(Payment + "balances", "");
            Dictionary<string, TransformBallans> temp = new Dictionary<string, TransformBallans>();
            using (StreamReader stream = new StreamReader(
                   response.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var resalt = JsonConvert.DeserializeObject<List<Field>>(str);
                foreach (var item in resalt)
                {
                    if (temp.ContainsKey(item.currency))
                    {
                        if (item.type == "available")
                        {
                            temp[item.currency].Available = Convert.ToDecimal(item.price);
                        }
                        if (item.type == "trade")
                        {
                            temp[item.currency].OnOrders = Convert.ToDecimal(item.price);
                        }
                    }
                    else
                    {
                        if (item.type == "available")
                        {
                            var a = new TransformBallans();
                            a.Available = Convert.ToDecimal(item.price);
                            temp.Add(item.currency, a);
                        }
                        if (item.type == "trade")
                        {
                            var a = new TransformBallans();
                            a.OnOrders = Convert.ToDecimal(item.price);
                            temp.Add(item.currency, a);
                        }
                    }
                }
                return temp;
            }
        }

        public Dictionary<string, string> GetDepositAddresses()
        {
            WebResponse response = LiveCoinGetRequst.AuthRequst(Payment + "balances", "");
            Dictionary<string, string> temp;
            using (StreamReader stream = new StreamReader(
                   response.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                temp = JsonConvert.DeserializeObject<Dictionary<string, string>>(str);

            }
            return temp;
        }

        public Task<Dictionary<string, TransformBallans>> GetBalancesAsync()
        {
            return new Task<Dictionary<string, TransformBallans>>(() => GetBalances());
        }

        public Task<Dictionary<string, string>> GetDepositAddressesAsync()
        {
            return new Task<Dictionary<string, string>>(() => GetDepositAddresses());
        }


    }
    public class LiveCoinTraide : ITrading
    {
        public class Field
        {
            public int id;
            public decimal amount;
            public string currency;
            public string wallet;
            public string date;
        }
        private static string Exchange = "exchange/";
        private string GetOrderType(OrderType arg)
        {
            switch (arg)
            {
                case OrderType.Buy:
                    return "buylimit";

                case OrderType.Sell:
                    return "selllimit";

            }
            throw new Exception("Wrong OrderType");
        }
        public string PostOrder(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            Dictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("currencyPair", currencyPair);
            postdata.Add("price", pricePerCoin);
            postdata.Add("quantity", amountQuote);

            var res = LiveCoinPostRequst.PostString(Exchange + GetOrderType(type), postdata.ToHttpPostString());
            dynamic jObject = JObject.Parse(res);
            return Convert.ToString(jObject.id);
        }
        public TransformWithdrow PostWihdrow(string currencyPair, string address, decimal amount)
        {
            Dictionary<string, object> postdata = new Dictionary<string, object>();
            postdata.Add("currency", currencyPair);
            postdata.Add("wallet", address);
            postdata.Add("amount", amount);
            var str = LiveCoinPostRequst.PostString("payment/out/coin", postdata.ToHttpPostString());
            Field temp = JsonConvert.DeserializeObject<Field>(str);
            TransformWithdrow tr = new TransformWithdrow(temp.id, temp.currency, temp.wallet, temp.amount, Convert.ToDateTime(temp.date));
            return tr;
        }
        public Task<string> PostOrderAsync(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            return new Task<string>(() => PostOrder(currencyPair, type, pricePerCoin, amountQuote));
        }

        public Task<TransformWithdrow> PostWihdrowAsync(string currencyPair, string address, decimal amount)
        {
            return new Task<TransformWithdrow>(() => PostWihdrow(currencyPair, address, amount));
        }
    }
    public class LiveCoin : Stock
    {
        public LiveCoin()
        {
            StockName = "LiveCoin";
            Orders = new LiveCoinOrders();
            Info = new LiveCoinInfo();
            Ballans = new LiveCoinWallet();
            Traid = new LiveCoinTraide();
        }
    }
}
