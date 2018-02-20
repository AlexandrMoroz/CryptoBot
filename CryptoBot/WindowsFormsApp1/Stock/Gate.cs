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

    public class GateInfo : IGetInfo
    {

        public GateInfo()
        {

        }

        public Task<Dictionary<string, TransformInfo>> GetInfoAsync()
        {
            return Task<Dictionary<string, TransformInfo>>.Factory.StartNew(() => GetInfo());
        }

        public List<string> GetPairs()
        {
            string site = "http://data.gate.io/api2/1/pairs";

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
            WebResponse resp = req.GetResponse();
            List<string> temp = new List<string>();
            using (StreamReader stream = new StreamReader
                     (resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var a = JsonConvert.DeserializeObject<List<string>>(str);
                temp = a.Where(x => x.Contains("btc")).ToList();
            }
            return temp;
        }
        public Dictionary<string, TransformInfo> GetInfo()
        {
            string site = "http://data.gate.io/api2/1/marketinfo";

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
            WebResponse resp = req.GetResponse();
            Dictionary<string, TransformInfo> temp = new Dictionary<string, TransformInfo>();
            using (StreamReader stream = new StreamReader
                     (resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                dynamic jObject = JObject.Parse(str);
                List<Dictionary<string, dynamic>> s = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(JsonConvert.SerializeObject(jObject.pairs));
                foreach (var item in s)
                {
                    temp.Add(item.First().Key, item.First().Value.fee);
                }
            }
            return temp;
        }
    }
    public class GateOrders : IGetOrders
    {
        public class Field
        {

            public List<List<decimal>> asks;
            public List<List<decimal>> bids;
            public bool result;
        }
        public GateOrders()
        {

        }

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

            string site = String.Format("http://data.gate.io/api2/1/orderBook/{0}", SecondCoinName.ToLower() + "_"+ MainCoinName.ToLower());
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
            TransforfOrders temp= new TransforfOrders();  
            try
            {
                WebResponse resp = req.GetResponse();

                using (StreamReader stream = new StreamReader(
                         resp.GetResponseStream(), Encoding.UTF8))
                {
                    string str = stream.ReadToEnd();
                    if (!str.Contains("invalid currency pair"))
                    {
                        var a = JsonConvert.DeserializeObject<Field>(str);

                         temp = new TransforfOrders(a.asks, a.bids);
                    }
                    return temp;
                }
            }
            catch (Exception e)
            {
                string message = e.Message;
                //MessageBoxButtons buttons = MessageBoxButtons.OK;
                //MessageBox.Show(message, "Gate", buttons);
                return temp;
            }

        }

        public Task<Dictionary<string, TransforfOrders>> GetOrdersAsync(List<KeyValuePair<string, string>> arg)
        {
            return Task<Dictionary<string, TransforfOrders>>.Factory.StartNew(() => GetOrders(arg));
        }

        public Task<TransforfOrders> GetOrderAsync(string MainCoinName, string SecondCoinName)
        {
            return Task<TransforfOrders>.Factory.StartNew(() => GetOrder(MainCoinName, SecondCoinName));
        }
    }
    public class GateWallet : IWallet
    {
        public string Balance = "private/balances";
        public string Address = "private/depositAddress";
        public List<string> GetCoins()
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
        public GateWallet()
        {

        }

        public Dictionary<string, TransformBallans> GetBalances()
        {

            var str = GatePostRequst.PostString(Balance, "");

            Dictionary<string, TransformBallans> resault = new Dictionary<string, TransformBallans>();
            Dictionary<string, string> available = new Dictionary<string, string>();

            dynamic tp = JsonConvert.DeserializeObject(str);

            foreach (var item in tp.available)
            {
                available.Add(item.Name, item.Value);
            }
            foreach (var item in tp.locked)
            {
                if (available.ContainsKey(item.Name))
                {
                    resault.Add(item.Name, new TransformBallans(available[item.Name], item.Value));
                }
            }
            return resault;
        }
        public Dictionary<string, string> GetDepositAddresses()
        {
            var postData = new Dictionary<string, string>();
            foreach (var item in GetCoins())
            {
                postData.Add(item, "&currency=" + item);
            }
            var resault = new Dictionary<string, string>();
            foreach (var item in postData)
            {
                var str = GatePostRequst.PostString(Balance, item.Value);
                dynamic tp = JsonConvert.DeserializeObject(str);
                resault.Add(item.Key, tp.addr.Value);
            }
            return resault;
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
    public class GateTraid : ITrading
    {
        private string Sell = "private/sell";
        private string Buy = "private/buy";
        private string Withdrow = "private/withdraw";
        public string PostOrder(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            var postData = new Dictionary<string, object>()
            {
                {"currencyPair",currencyPair + "_btc" },
                {"rate", pricePerCoin },
                {"amount", amountQuote }
            };
            string res = GatePostRequst.PostString(GetType(type), postData.ToHttpPostString());
            dynamic a = JsonConvert.DeserializeObject(res);
            return Convert.ToString(a.orderNumber);
        }
        public TransformWithdrow PostWihdrow(string currencyPair, string adrress, decimal amountQuote)
        {
            var postData = new Dictionary<string, object>()
            {
                {"currencyPair",currencyPair},
                {"amount", amountQuote },
                {"adress", adrress }
            };
            string res = GatePostRequst.PostString(Withdrow, postData.ToHttpPostString());
            var temp = new TransformWithdrow() { Amount = amountQuote, Currency = currencyPair, Wallet = adrress, Date = DateTime.Now };
            return temp;
        }
        public Task<string> PostOrderAsync(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            return Task<string>.Factory.StartNew(() => PostOrder(currencyPair, type, pricePerCoin, amountQuote));
        }

        public Task<TransformWithdrow> PostWihdrowAsync(string currencyPair, string adrress, decimal amountQuote)
        {
            return Task<TransformWithdrow>.Factory.StartNew(() => PostWihdrow(currencyPair, adrress, amountQuote));
        }
        private string GetType(OrderType type)
        {
            if (type == OrderType.Buy)
            {
                return Buy;
            }
            else if (type == OrderType.Sell)
            {
                return Sell;
            }
            else
            {
                throw new Exception("Invalid Type");
            }
        }
    }

    public class Gate : Stock
    {
        public Gate()
        {
            StockName = "Gate";
            Info = new GateInfo();
            Orders = new GateOrders();
            Ballans = new GateWallet();
            Traid = new GateTraid();
        }
    }

}
