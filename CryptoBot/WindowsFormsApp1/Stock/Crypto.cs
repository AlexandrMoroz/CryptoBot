using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Interfesse;
namespace WindowsFormsApp1
{

    public class CryptoCoinResaltInfo
    {
        public string Symbol;
        public string Status;
        public decimal WithdrawFee;
    }
    public class CryptoInfo : IGetInfo
    {
        public class Field
        {
            public List<CryptoCoinResaltInfo> Data;
        }
        public CryptoInfo()
        {

        }
        public Dictionary<string, TransformInfo> GetInfo()
        {
            string site = "https://www.cryptopia.co.nz/api/GetCurrencies";
            WebResponse resp = CryptoGetRequst.Requst(site);
            Dictionary<string, TransformInfo> temp = new Dictionary<string, TransformInfo>();
            using (StreamReader stream = new StreamReader(
                      resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<Field>(str);
                temp = res.Data.ToDictionary(d => d.Symbol, d => new TransformInfo(d));
            }
            return temp;
        }
        public Task<Dictionary<string, TransformInfo>> GetInfoAsync()
        {
            return new Task<Dictionary<string, TransformInfo>>(() => GetInfo());
        }
    }
    #region Незаконченно
    public class CryptoIdenteti
    {
        public class Field
        {
            public class CryptoCoinIdentetiFields
            {
                public int Id;
                public string Label;
                public string Status;
                public string Symbol;
                public decimal TradeFee;

            }
            public List<CryptoCoinIdentetiFields> Data;
        }
        public Field field;
        public CryptoIdenteti()
        {
            field = GetTradePairs();
        }

        public Field GetTradePairs()
        {
            string site = "https://www.cryptopia.co.nz/api/GetTradePairs";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
            WebResponse resp = req.GetResponse();
            Field a;
            using (StreamReader stream = new StreamReader(
                      resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                a = JsonConvert.DeserializeObject<Field>(str);
                a.Data = a.Data.Where(y => y.Label.Split('/')[0] != "BTC" && y.Label.Split('/')[1] == "BTC").ToList();

            }
            return a;
        }
    }
    #endregion

    public class CryptoOrders : IGetOrders
    {
        public class BuySellFields
        {
            public int TradePairId;
            public decimal Price;
            public decimal Volume;
            public decimal Total;
        }
        public class Field
        {
            public bool Success;
            public string Error;
            public class BuySellData
            {

                public List<BuySellFields> Buy;
                public List<BuySellFields> Sell;

            }
            public BuySellData Data;
        }
        public CryptoOrders()
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
            if (MainCoinName == "USD")
            {
                return new TransforfOrders();
            }
            TransforfOrders temp = new TransforfOrders();
            string site = String.Format("https://www.cryptopia.co.nz/api/GetMarketOrders/{0}/30", SecondCoinName + '_' + MainCoinName);
        
            try
            {
                WebResponse resp = CryptoGetRequst.Requst(site);
                using (StreamReader stream = new StreamReader(
                            resp.GetResponseStream(), Encoding.UTF8))
                {
                    string str = stream.ReadToEnd();
                    var res = JsonConvert.DeserializeObject<Field>(str);

                    if (res.Success == true&&res.Error==null)
                    {
                        temp = new TransforfOrders(res.Data.Sell.ToDictionary(d => d.Price, k => k.Volume), res.Data.Buy.ToDictionary(d => d.Price, k => k.Volume));
                    }
                }
                return temp;
            }
            catch (WebException e)
            {
                string message = e.Message;
                //MessageBoxButtons buttons = MessageBoxButtons.OK;
                //MessageBox.Show(message, "CRYPTO", buttons);
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
    public class CryptoWallet : IWallet
    {
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
        public class BallanceField
        {
            public class BallanceField2
            {
                public string Symbol;
                public string Available;
                public string HeldForTrades;
            }
            public List<BallanceField2> Data;
        }

        public Dictionary<string, TransformBallans> GetBalances()
        {
            var postData = new Dictionary<string, object>();
            postData.Add("Currency", "");
            var str = CryptoriaPostRequst.PostString("GetBalance", postData.ToHttpPostString());
            var temp = JsonConvert.DeserializeObject<BallanceField>(str);
            System.IFormatProvider cultureUS =  new System.Globalization.CultureInfo("en-US");
            return temp.Data.ToDictionary(x => x.Symbol, y => new TransformBallans(Convert.ToDecimal(y.Available, cultureUS), Convert.ToDecimal(y.HeldForTrades, cultureUS)));
        }
        public Dictionary<string, string> GetDepositAddresses()
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();
            List<string> currencis = GetCoins();
            foreach (var item in currencis)
            {
                var postData = new Dictionary<string, object>();
                postData.Add("Currency", item);
                var str = CryptoriaPostRequst.PostString("GetBalance", postData.ToHttpPostString());
                var Jobj = JsonConvert.DeserializeObject<JObject>(str);
                temp.Add(Jobj.Value<string>("Currency"), Jobj.Value<string>("Address"));
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
    public class CryptoTrading : ITrading
    {
        private string GetOrderType(OrderType arg)
        {
            switch (arg)
            {
                case OrderType.Buy:
                    return "Buy";

                case OrderType.Sell:
                    return "Sell";

            }
            throw new Exception("Wrong OrderType");
        }
        public string PostOrder(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            var postData = new Dictionary<string, object> {
                { "Market", currencyPair },
                { "Type", GetOrderType(type) },
                { "Rate", Convert.ToString(pricePerCoin) },
                { "Amount", Convert.ToString(amountQuote)}
            };

            var str = CryptoriaPostRequst.PostString("SubmitTrade", postData.ToHttpPostString());
            var tp = JsonConvert.DeserializeObject<JObject>(str);
            return tp.Value<string>("OrderId");

        }
        public TransformWithdrow PostWihdrow(string Currency, string adrress, decimal amountQuote)
        {
            var postData = new Dictionary<string, object> {
                { "Currency", Currency },
                { "Adrress", adrress },
                { "PaymentId","" },
                { "Amount", Convert.ToString(amountQuote) }
            };
            var str = CryptoriaPostRequst.PostString("SubmitTrade", postData.ToHttpPostString());
            var tp = JsonConvert.DeserializeObject<JObject>(str);
            return new TransformWithdrow(0, Currency, adrress, amountQuote, tp.Value<DateTime>("Date"));
        }
        public Task<string> PostOrderAsync(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            return Task<string>.Factory.StartNew(() => PostOrder(currencyPair, type, pricePerCoin, amountQuote));
        }
        public Task<TransformWithdrow> PostWihdrowAsync(string currencyPair, string adrress, decimal amountQuote)
        {
            return Task<TransformWithdrow>.Factory.StartNew(() => PostWihdrow(currencyPair, adrress, amountQuote));
        }
    }
    public class Cryptopia : Stock
    {
        public Cryptopia()
        {
            StockName = "Cryptopia";
            Traid = new CryptoTrading();
            Ballans = new CryptoWallet();
            Orders = new CryptoOrders();
            Info = new CryptoInfo();
        }
    }
}
