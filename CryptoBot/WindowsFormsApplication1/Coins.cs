using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class TransforfField
    {
        public Dictionary<decimal, decimal> asks = new Dictionary<decimal, decimal>();
        public Dictionary<decimal, decimal> bids = new Dictionary<decimal, decimal>();

        public TransforfField() { }
        public TransforfField(List<List<decimal>> a, List<List<decimal>> b)
        {
            foreach (var item in a)
            {
                if (!asks.ContainsKey(item[0]))
                {
                    asks.Add(item[0], item[1]);
                }
                else
                {
                    asks[item[0]] += item[1];
                }
            }
            foreach (var item in b)
            {
                if (!bids.ContainsKey(item[0]))
                {
                    bids.Add(item[0], item[1]);
                }
                else
                {
                    bids[item[0]] += item[1];
                }
            }
        }
        public TransforfField(List<QuantityRate> a, List<QuantityRate> b)
        {
            asks = a.ToDictionary(d => d.Rate, k=>k.Quantity);
            bids = b.ToDictionary(d => d.Rate, k => k.Quantity); 
        }
        
        public TransforfField(List<BuySellFields> a, List<BuySellFields> b)
        {
            asks = a.ToDictionary(d => d.Price, k=>k.Volume);
            bids = b.ToDictionary(d => d.Price, k => k.Volume);
        }
    }
    public class TransformInfo
    {
        public class Field
        {
            public decimal WithdrawFee;
            public bool Status;
        }
        public Field Data;
      
        public TransformInfo(LiveCoinInfoFields b)
        {
            Data = new Field() { WithdrawFee = b.withdrawFee, Status = b.walletStatus == "normal" ? true: false};
        }
        public TransformInfo(PoloniexCoinsInfoFields b)
        {
            Data =  new Field() { WithdrawFee = b.txFee, Status = b.disabled != 0 ? true : false };
        }
        public TransformInfo(BittrexResultFields b)
        {
            Data = new Field() { WithdrawFee = b.TxFee, Status = b.IsActive };
        }
        
        public TransformInfo(CryptoCoinResaltInfo b)
        {
            Data = new Field() { WithdrawFee = b.WithdrawFee, Status = b.Status=="OK"?true:false};
        }
        public TransformInfo(YobitCoinIdentetiFields b)
        {
            Data = new Field() { WithdrawFee = b.fee};
        }
        
    }
    public class LiveAndPoloniexField
    {
        public List<List<decimal>> asks;
        public List<List<decimal>> bids;
    }

    public class LiveCoinInfoFields
    {
        public string symbol;
        public string walletStatus;
        public decimal withdrawFee;
    }
    public class LiveCoinInfo
    {
        public class Fields
        {
            public bool success;
            public List<LiveCoinInfoFields> info;
        }
        public Dictionary<string, TransformInfo> Data;
        public LiveCoinInfo()
        {
            Data = GetCoinInfo();
        }
        static public Dictionary<string, TransformInfo> GetCoinInfo()
        {
            string site = "https://api.livecoin.net/info/coinInfo";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
            WebResponse resp = req.GetResponse();
            Dictionary<string, TransformInfo> temp;
            using (StreamReader stream = new StreamReader(
                 resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<Fields>(str);
                temp = res.info.ToDictionary(d=>d.symbol, x=>new TransformInfo(x));
            }
            return temp;
        }
       public bool StatusCheaker(string arg)
        {
            if (Data[arg].Data.Status)
            {
                return true;
            }
            return false;
        }
    }
    public class LiveCoinCoins
    {
        

        public Dictionary<string, TransforfField> Data;
        public LiveCoinCoins()
        {
            Data = GetOrderBook();
        }
        static public Dictionary<string, TransforfField> GetOrderBook()
        {
            LiveCoinInfo LiveInfo = new LiveCoinInfo();

            string order = "https://api.livecoin.net/exchange/all/order_book";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(order);
            WebResponse resp;
            try
            {
                 resp = req.GetResponse();
            }
            catch (System.Net.WebException)
            {
                return null;
            }
            Dictionary<string, TransforfField> temp;
            using (StreamReader stream = new StreamReader(
                 resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<Dictionary<string, LiveAndPoloniexField>>(str);
                var t = res.Where(y => Cheaker(y) && LiveInfo.StatusCheaker(y.Key.Split('/')[0]));
                temp = t.ToDictionary(x => x.Key, y => new TransforfField(y.Value.asks,y.Value.bids));
            }
            return temp;
        }
        private static bool Cheaker(KeyValuePair<string,LiveAndPoloniexField> arg)
        {
            if(arg.Key.Split('/')[0] != "BTC")
            {
                if (arg.Key.Split('/')[1] == "BTC")
                {
                    return true;
                }
                return false;
            }

            return false;
            
        }
    }

    public class PoloniexCoinsInfoFields
    {
        public string name;
        public decimal txFee;
        public int disabled;
    }
    public class PoloniexCoinsInfo
    {
        public Dictionary<string, TransformInfo> Data;
        public PoloniexCoinsInfo()
        {
            Data = GetPoloniexInfo();
        }
        static public Dictionary<string, TransformInfo> GetPoloniexInfo()
        {
            string site = "https://poloniex.com/public?command=returnCurrencies";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
            WebResponse resp = req.GetResponse();
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
        static string FormatString(string arg)
        {
            string res = arg.Split('_')[1] + '/' + arg.Split('_')[0];
            return res;
        }
    }
    public class PoloniexCoins
    {
        string order = "https://poloniex.com/public?command=returnOrderBook&currencyPair=all";

        public Dictionary<string, TransforfField> Data;
        public PoloniexCoins()
        {
            Data = GetLiveAndPoloniexOrderBook(order);
        }
        static public Dictionary<string, TransforfField> GetLiveAndPoloniexOrderBook(string site)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
            WebResponse resp = req.GetResponse();
            Dictionary<string, TransforfField> temp;
            using (StreamReader stream = new StreamReader(
                 resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<Dictionary<string, LiveAndPoloniexField>>(str);
                var t = res.Where(y => y.Key.Split('_')[0] == "BTC" && y.Key.Split('_')[1] != "BTC");
                temp=t.ToDictionary(x => FormatString(x.Key), y => new TransforfField(y.Value.asks, y.Value.bids)); ;
            }
            return temp;
        }
        static string FormatString(string arg)
        {
            string res = arg.Split('_')[1] + '/' + arg.Split('_')[0];
            return res;
        }
    }
    
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
        public BittrexField Data;
        public BittrexIdenteti()
        {
            Data = GetTradePairs();
            
        }

        private BittrexField GetTradePairs()
        {
            string site = "https://bittrex.com/api/v1.1/public/getmarkets";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
            WebResponse resp = req.GetResponse();

            StreamReader stream = new StreamReader(
                    resp.GetResponseStream(), Encoding.UTF8);
            string str = stream.ReadToEnd();
            var a = JsonConvert.DeserializeObject<BittrexField>(str);
            a.result = a.result.Where( d=> d.IsActive==true).ToList();
            return a;
        }

    }
    public class BittrexResultFields
    {
            public string Currency;
            public decimal TxFee;
            public bool IsActive;
    }
    public class BittrexCoinInfo
    {

        public class Field
        {
            public List<BittrexResultFields> result;
        }
        public Dictionary<string, TransformInfo> Data;
        public BittrexCoinInfo()
        {
            Data = GetBittrexCoinInfo();
        }

        private Dictionary<string, TransformInfo> GetBittrexCoinInfo()
        {
            string site = "https://bittrex.com/api/v1.1/public/getcurrencies";

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
            WebResponse resp = req.GetResponse();
            Dictionary<string, TransformInfo> temp = new Dictionary<string, TransformInfo>();

            using (StreamReader stream = new StreamReader
                     (resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var a = JsonConvert.DeserializeObject<Field>(str);
                temp = a.result.ToDictionary(d => d.Currency, d => new TransformInfo(d));
            }
            return temp;
        }
    }
    public class QuantityRate
    {
        public decimal Quantity;
        public decimal Rate;
    }
    public class BittrexCoin
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
        public /*Task<*/Dictionary<string, TransforfField>/*>*/ Data;

        //public BittrexCoin()
        //{
        //    Data = GetBittrexOrderBook();
        //}
        public BittrexCoin(Dictionary<string, TransformInfo> arg)
        {
            Data = GetBittrexOrderBook(arg);
        }
        private async Task<Dictionary<string, TransforfField>> GetBittrexOrderBook()
        {
            return await new Task<Dictionary<string, TransforfField>>(delegate
            {
               BittrexIdenteti Ident = new BittrexIdenteti();
               BittrexCoinInfo CoinInfo = new BittrexCoinInfo();
               Dictionary<string, TransforfField> temp = new Dictionary<string, TransforfField>();
               bool flag;
               foreach (var i in Ident.Data.result)
               {
                   try
                   {
                       flag = CoinInfo.Data[i.MarketName.Split('-')[1]].Data.Status;
                   }
                   catch (KeyNotFoundException)
                   {
                       continue;
                   }
                   if (flag == false)
                   {
                       continue;
                   }
                   string site = String.Format("https://bittrex.com/api/v1.1/public/getorderbook?market={0}&type=both&depth=10", i.MarketName);
                   HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
                   WebResponse resp = req.GetResponse();

                   using (StreamReader stream = new StreamReader(
                            resp.GetResponseStream(), Encoding.UTF8))
                   {
                       string str = stream.ReadToEnd();
                       var a = JsonConvert.DeserializeObject<Field>(str);
                       temp.Add(i.MarketName, new TransforfField(a.result.sell, a.result.buy));
                   }
               }
               return temp;
           });
        }
        private /*async Task<*/Dictionary<string, TransforfField>/*>*/ GetBittrexOrderBook(Dictionary<string, TransformInfo> arg)
        {
            //return await new Task<Dictionary<string, TransforfField>>(delegate
            //{
                BittrexIdenteti Ident = new BittrexIdenteti();
                Ident.Data.result = Ident.Data.result.Where(d => arg.ContainsKey(d.MarketCurrency)).ToList();
                Dictionary<string, TransforfField> temp = new Dictionary<string, TransforfField>();
                bool flag;
                foreach (var i in Ident.Data.result)
                {
                    try
                    {
                        flag = arg[i.MarketCurrency].Data.Status;
                    }
                    catch (KeyNotFoundException)
                    {
                        continue;
                    }
                    if (flag == false)
                    {
                        continue;
                    }
                    string site = String.Format("https://bittrex.com/api/v1.1/public/getorderbook?market={0}&type=both&depth=10", i.MarketName);
                    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
                    WebResponse resp = req.GetResponse();

                    using (StreamReader stream = new StreamReader(
                             resp.GetResponseStream(), Encoding.UTF8))
                    {
                        string str = stream.ReadToEnd();
                        var a = JsonConvert.DeserializeObject<Field>(str);
                        temp.Add(FormatString(i.MarketName), new TransforfField(a.result.sell, a.result.buy));
                    }
                }
                return temp;
            //});
        }

        static string FormatString(string arg)
        {
            string res = arg.Split('-')[1] + '/' + arg.Split('-')[0];
            return res;
        }
        //public void Update()
        //{
        //    Data = GetBittrexOrderBook();
        //}
    }

    public class CryptoCoinResaltInfo
    {
        public string Symbol;
        public string Status;
        public decimal WithdrawFee;

    }
    public class CryptoCoinInfo
    {

        public class field
        {
              public List<CryptoCoinResaltInfo> Data;
        }
        public Dictionary<string, TransformInfo> Data;
        public CryptoCoinInfo()
        {
            Data = GetTradePairs();
        }

        private Dictionary<string, TransformInfo> GetTradePairs()
        {
            string site = "https://www.cryptopia.co.nz/api/GetCurrencies";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
            WebResponse resp = req.GetResponse();
            Dictionary<string, TransformInfo> temp;
            using (StreamReader stream = new StreamReader(
                      resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var res = JsonConvert.DeserializeObject<field>(str);
                temp = res.Data.ToDictionary(d => d.Symbol, d => new TransformInfo(d));
            }            
            return temp;
        }
    }
    public class CryptoCoinIdenteti
    {
        public class field
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
        public field Field;
        public CryptoCoinIdenteti()
        {
            Field = GetTradePairs();
        }

        private field GetTradePairs()
        {
            string site = "https://www.cryptopia.co.nz/api/GetTradePairs";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
            WebResponse resp = req.GetResponse();
            field a;
            using (StreamReader stream = new StreamReader(
                      resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                a = JsonConvert.DeserializeObject<field>(str);
                a.Data = a.Data.Where(y => y.Label.Split('/')[0] != "BTC" && y.Label.Split('/')[1] == "BTC").ToList();
               
            }
            return a;
        }
    }
    public class BuySellFields
    {
        public int TradePairId;
        public decimal Price;
        public decimal Volume;
        public decimal Total;
    }
    public class CryptoCoins
    {
        public class Field
        {
            public bool Success;

            public class BuySellData
            {
                public List<BuySellFields> Buy;
                public List<BuySellFields> Sell;

            }
           public BuySellData Data;
        }
        public Task<Dictionary<string, TransforfField>> AsyncData;
        public Dictionary<string, TransforfField> Data; 
        public CryptoCoins()
        {
            Data = GetCryptoOrderBook();
        }
        public CryptoCoins(Dictionary<string, TransformInfo> arg)
        {
            Data = GetCryptoOrderBook(arg);
        }
        private Dictionary<string, TransforfField> GetCryptoOrderBook()
        {
          
                CryptoCoinIdenteti Ident = new CryptoCoinIdenteti();
                Dictionary<string, TransforfField> temp = new Dictionary<string, TransforfField>();
                foreach (var i in Ident.Field.Data)
                {
                    string site = String.Format("https://www.cryptopia.co.nz/api/GetMarketOrders/{0}/10", i.Id);
                    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
                    WebResponse resp = req.GetResponse();

                    using (StreamReader stream = new StreamReader(
                             resp.GetResponseStream(), Encoding.UTF8))
                    {
                        string str = stream.ReadToEnd();
                        var res = JsonConvert.DeserializeObject<Field>(str);

                        if (res.Success == true)
                        {
                            temp.Add(i.Label, new TransforfField(res.Data.Sell, res.Data.Buy));
                        }
                    }
                }
                return temp;
        }
        private async Task<Dictionary<string, TransforfField>> AsyncGetCryptoOrderBook()
        {
            return await new Task<Dictionary<string, TransforfField>>(delegate
            {
                CryptoCoinIdenteti Ident = new CryptoCoinIdenteti();
                Dictionary<string, TransforfField> temp = new Dictionary<string, TransforfField>();
                foreach (var i in Ident.Field.Data)
                {
                    string site = String.Format("https://www.cryptopia.co.nz/api/GetMarketOrders/{0}/10", i.Id);
                    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
                    WebResponse resp = req.GetResponse();

                    using (StreamReader stream = new StreamReader(
                             resp.GetResponseStream(), Encoding.UTF8))
                    {
                        string str = stream.ReadToEnd();
                        var res = JsonConvert.DeserializeObject<Field>(str);

                        if (res.Success == true)
                        {
                            temp.Add(i.Label, new TransforfField(res.Data.Sell, res.Data.Buy));
                        }
                    }
                }
                return temp;
            });
        }
        private Dictionary<string, TransforfField> GetCryptoOrderBook(Dictionary<string, TransformInfo> arg)
        {
           
                CryptoCoinIdenteti Ident = new CryptoCoinIdenteti();
                Ident.Field.Data = Ident.Field.Data.Where(d => arg.ContainsKey(d.Symbol)).ToList();
                Dictionary<string, TransforfField> temp = new Dictionary<string, TransforfField>();
                foreach (var i in Ident.Field.Data)
                {
                    string site = String.Format("https://www.cryptopia.co.nz/api/GetMarketOrders/{0}/10", i.Id);
                    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
                    WebResponse resp = req.GetResponse();

                    using (StreamReader stream = new StreamReader(
                             resp.GetResponseStream(), Encoding.UTF8))
                    {
                        string str = stream.ReadToEnd();
                        var res = JsonConvert.DeserializeObject<Field>(str);

                        if (res.Success == true)
                        {
                            temp.Add(i.Label, new TransforfField(res.Data.Sell, res.Data.Buy));
                        }
                    }
                }
                return temp;
         
        }
        private async Task<Dictionary<string, TransforfField>> AsyncGetCryptoOrderBook(Dictionary<string, TransformInfo> arg)
        {
            return await new Task<Dictionary<string, TransforfField>>(delegate
            {
                CryptoCoinIdenteti Ident = new CryptoCoinIdenteti();
                Ident.Field.Data =Ident.Field.Data.Where(d => arg.ContainsKey(d.Symbol)).ToList();
                Dictionary<string, TransforfField> temp = new Dictionary<string, TransforfField>();
                foreach (var i in Ident.Field.Data)
                {
                    string site = String.Format("https://www.cryptopia.co.nz/api/GetMarketOrders/{0}/10", i.Id);
                    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
                    WebResponse resp = req.GetResponse();

                    using (StreamReader stream = new StreamReader(
                             resp.GetResponseStream(), Encoding.UTF8))
                    {
                        string str = stream.ReadToEnd();
                        var res = JsonConvert.DeserializeObject<Field>(str);

                        if (res.Success == true)
                        {
                            temp.Add(i.Label, new TransforfField(res.Data.Sell, res.Data.Buy));
                        }
                    }
                }
                return temp;
            });
        }

        static string FormatString(string arg)
        {
            string res = arg.Split('_')[0] + '/' + arg.Split('_')[1];
            return res;
        }
    }

    public class YobitCoinIdentetiFields
    {
        public decimal fee;
        public decimal fee_buyer;
        public decimal fee_seller;

    }
    public class YobitCoinIdenteti
    {
        public class field
        {
            public Dictionary<string, YobitCoinIdentetiFields> pairs;
        }
        public Dictionary<string, TransformInfo> Data;

        public YobitCoinIdenteti()
        {
            Data = GetTradePairs();
        }

        private Dictionary<string, TransformInfo> GetTradePairs()
        {
            string site = "https://yobit.net/api/3/info";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
            WebResponse resp = req.GetResponse();
            Dictionary<string, TransformInfo> temp;
            using (StreamReader stream = new StreamReader(
                      resp.GetResponseStream(), Encoding.UTF8))
            {
                string str = stream.ReadToEnd();
                var a = JsonConvert.DeserializeObject<field>(str);
                temp = a.pairs.ToDictionary(d => d.Key, d => new TransformInfo(d.Value));
            }
            return temp;
        }
    }
    public class YobitCoins
    {
        public class Field
        {
            public List<List<decimal>> asks;
            public List<List<decimal>> bids;
        }
        public Dictionary<string, TransforfField> Data;

        public YobitCoins()
        {
            //Data = GetOrderBookAsync();
        }
        public YobitCoins(YobitCoinIdenteti coins)
        {
            Data = GetOrderBook(coins);
        }
        private async Task<Dictionary<string, TransforfField>> GetOrderBookAsync()
        {
            return await new Task<Dictionary<string, TransforfField>>(delegate{
            YobitCoinIdenteti Ident = new YobitCoinIdenteti();
            Dictionary<string, TransforfField> temp = new Dictionary<string, TransforfField>();
            foreach (var i in Ident.Data)
            {
                string site = String.Format("https://yobit.net/api/2/{0}/depth", i.Key);
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
                WebResponse resp = req.GetResponse();

                using (StreamReader stream = new StreamReader(
                         resp.GetResponseStream(), Encoding.UTF8))
                {
                    string str = stream.ReadToEnd();
                    var res = JsonConvert.DeserializeObject<Field>(str);
                    if (res.bids != null && res.asks != null)
                    {
                        temp.Add(FormatString(i.Key), new TransforfField(res.asks, res.bids));
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            return temp;
        });
        }
        private async Task<Dictionary<string, TransforfField>> GetOrderBookAsync(YobitCoinIdenteti coins)
        {
            return await new Task<Dictionary<string, TransforfField>>(delegate {
                YobitCoinIdenteti Ident = coins;
                Dictionary<string, TransforfField> temp = new Dictionary<string, TransforfField>();
                String end = @"?ignore_invalid=1&limit=20";
                string site ="https://yobit.net/api/3/depth/";
                foreach (var i in Ident.Data)
                {
                    site += i.Key;
                }
                site += end;
                foreach (var i in Ident.Data)
                {
                    
                    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
                    WebResponse resp = req.GetResponse();

                    using (StreamReader stream = new StreamReader(
                             resp.GetResponseStream(), Encoding.UTF8))
                    {
                        string str = stream.ReadToEnd();
                        var res = JsonConvert.DeserializeObject<Field>(str);
                        if (res.bids != null && res.asks != null)
                        {
                            temp.Add(FormatString(i.Key), new TransforfField(res.asks, res.bids));
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                return temp;
            });
        }
        private Dictionary<string, TransforfField> GetOrderBook(YobitCoinIdenteti coins)
        {
            
                YobitCoinIdenteti Ident = coins;
                Dictionary<string, TransforfField> temp = new Dictionary<string, TransforfField>();
                String end = @"?ignore_invalid=1&limit=20";
                string site = "https://yobit.net/api/3/depth/";
                foreach (var i in Ident.Data)
                {
                    site +="-"+i.Key;
                }
                site += end;
                foreach (var i in Ident.Data)
                {

                    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(site);
                    WebResponse resp = req.GetResponse();

                    using (StreamReader stream = new StreamReader(
                             resp.GetResponseStream(), Encoding.UTF8))
                    {
                        string str = stream.ReadToEnd();
                        var res = JsonConvert.DeserializeObject<Field>(str);
                        if (res.bids != null && res.asks != null)
                        {
                            temp.Add(FormatString(i.Key), new TransforfField(res.asks, res.bids));
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
                return temp;
         
        }
        static string FormatString(string arg)
        {
            string res = arg.Split('_')[0] + '/' + arg.Split('_')[1];
            return res;
        }
    }
}
