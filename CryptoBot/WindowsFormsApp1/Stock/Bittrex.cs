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
    public class BittrexIdenteti
    {
        public class BittrexField
        {
            public class ResultIdentetiFields
            {
                public string MarketName;
                public bool IsActive;
                public double TradeFee;
                public string MarketCurrency;
            }
            public bool Success;
            public List<ResultIdentetiFields> result;

        }
        public BittrexIdenteti()
        {
         

        }

        private BittrexField GetTradePairs()
        {
            string site = "https://bittrex.com/api/v1.1/public/getmarkets";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
            try
            {
                WebResponse resp = req.GetResponse();

                StreamReader stream = new StreamReader(
                        resp.GetResponseStream(), Encoding.UTF8);
                string str = stream.ReadToEnd();
                var a = JsonConvert.DeserializeObject<BittrexField>(str);
                a.result = a.result.Where(d => d.IsActive == true).ToList();
                return a;
            }
            catch (System.Net.WebException)
            {
                return null;
            }
        }

    }
    public class BittrexResultFields
    {
        public string Currency;
        public decimal TxFee;
        public bool IsActive;
    }
    public class BittrexInfo:IGetInfo
    {
        public class Field
        {
            public List<BittrexResultFields> result;
        }
        public BittrexInfo()
        {
            
        }
        public Dictionary<string, TransformInfo> GetInfo()
        {
            string site = "https://bittrex.com/api/v1.1/public/getcurrencies";
            Dictionary<string, TransformInfo> temp = new Dictionary<string, TransformInfo>();
            
            try {
                WebResponse resp = BittrexGetRequst.Requst(site);
                
                using (StreamReader stream = new StreamReader
                         (resp.GetResponseStream(), Encoding.UTF8))
                {
                    string str = stream.ReadToEnd();
                    var a = JsonConvert.DeserializeObject<Field>(str);
                    temp = a.result.ToDictionary(d => d.Currency, d => new TransformInfo(d));
                }
            }
            catch (System.Net.WebException)
            {
                temp = null;
            }
            return temp;
        }
        public Task<Dictionary<string, TransformInfo>> GetInfoAsync()
        {
            return new Task<Dictionary<string, TransformInfo>>(() => GetInfo());
        }
    }
    public class QuantityRate
    {
        public decimal Quantity;
        public decimal Rate;
    }

    public class BittrexOrder: IGetOrders
    {

        public class Field
        {
            public class BittrexResult
            {
                public List<QuantityRate> buy;
                public List<QuantityRate> sell;
            }
            public bool success;
            public BittrexResult result;
        }

        public BittrexOrder()
        {
            
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
        public TransformOrders GetOrder(string MainCoinName, string SecondCoinName)
        {
            if (MainCoinName=="USD")
            {
                return new TransformOrders();
            }
            string site = String.Format("https://bittrex.com/api/v1.1/public/getorderbook?market={0}&type=both&depth=30", MainCoinName + '-' + SecondCoinName);
           
            TransformOrders temp = new TransformOrders();
            try
            {
                WebResponse resp = BittrexGetRequst.Requst(site);
                using (StreamReader stream = new StreamReader(
                       resp.GetResponseStream(), Encoding.UTF8))
                {
                    string str = stream.ReadToEnd();
                    var res = JsonConvert.DeserializeObject<Field>(str);
                    if (res.success == true)
                    {
                        temp = new TransformOrders(res.result.sell, res.result.buy);
                    }
                }
                return temp;
            }
            catch (WebException e)
            {
                string message = e.Message;
                //MessageBoxButtons buttons = MessageBoxButtons.OK;
                //MessageBox.Show(message, "BITTREX", buttons);
                return temp;
            }

        }
        

        public Task<Dictionary<string, TransformOrders>> GetOrdersAsync(List<KeyValuePair<string, string>> arg)
        {
            return  Task<Dictionary<string, TransformOrders>>.Factory.StartNew(() => GetOrders(arg));
        }

        public Task<TransformOrders> GetOrderAsync(string MainCoinName, string SecondCoinName)
        {
            return  Task<TransformOrders>.Factory.StartNew(() => GetOrder(MainCoinName, SecondCoinName));
        }
    }

    public class BittrexWallet : IWallet
    {

        const string ApiCallGetBalances = "account/getbalances";
        const string ApiCallGetAdrress = "account/getdepositaddress";
        public Dictionary<string, TransformBallans> GetBalances()
        {
            Dictionary<string, TransformBallans> temp = new Dictionary<string, TransformBallans>();     
            var resp = BittrexPostRequst.PostString(ApiCallGetBalances, "");
            dynamic  jObject = JObject.Parse(resp);

            foreach (var item in jObject.result)
            {
                temp.Add(item.Currency.Value, new TransformBallans(Convert.ToDecimal(item.Available.Value), Convert.ToDecimal(item.Pending.Value)));
            }
            return temp;
        }
        public Dictionary<string, string> GetDepositAddresses()
        {
            var temp = new Dictionary<string, string>();
            var lt = AccseptCoins.GetCoins();
            var postData = new List<string>();
            foreach (var item in lt)
            {
                postData.Add("&currency=" + item); 
            }
            foreach (var item in postData)
            {
                var resp = BittrexPostRequst.PostString(ApiCallGetAdrress, item);
                dynamic jObject = JObject.Parse(resp);
                temp.Add(jObject.Currency, jObject.Address);
            }
            return temp;
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
    public class BittrexTraid : ITrading
    {
        private string Buy = "market/buylimit";
        private string Sell = "market/selllimit";
        private string Withdraw = "account/withdraw";
        public string PostOrder(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            var postData = new Dictionary<string, object>()
            {
                {"market", "BTC-"+currencyPair},
                {"quantity",amountQuote },
                {"rate",pricePerCoin }
            };   
            var resp = BittrexPostRequst.PostString(GetOrderType(type), postData.ToHttpPostString());
            dynamic jObject = JObject.Parse(resp);
            return Convert.ToString(jObject.uuid);
        }
        public TransformWithdrow PostWihdrow(string currencyPair, string adrress, decimal amountQuote)
        {
            var postData = new Dictionary<string, object>()
            {
                {"currency", currencyPair},
                {"quantity",amountQuote },
                {"address",adrress }
            };
            var resp = BittrexPostRequst.PostString(Withdraw, postData.ToHttpPostString());
            dynamic jObject = JObject.Parse(resp);
            return Convert.ToString(jObject.uuid);
        }
        public Task<string> PostOrderAsync(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            return  Task<string>.Factory.StartNew(() => PostOrder(currencyPair, type, pricePerCoin, amountQuote));  
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
                    return Buy;

                case OrderType.Sell:
                    return Sell;

            }
            throw new Exception("Wrong OrderType");
        }

    }

    public class Bittrex : Stock
    {
        public Bittrex()
        {
            StockName = "Bittrex";
            Info = new BittrexInfo();
            Orders = new BittrexOrder();
            Ballans = new BittrexWallet();
            Traid = new BittrexTraid();
        }
    }
}
