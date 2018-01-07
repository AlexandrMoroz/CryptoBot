using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class ExmoInfo : IGetInfo
    {

        public Dictionary<string, TransformInfo> GetInfo()
        {
            Dictionary<string, TransformInfo> temp = new Dictionary<string, TransformInfo>();
            var coins = AccseptCoins.GetCoins();
            foreach (var item in coins)
            {
                try
                {
                    string site = "	https://api.exmo.com/v1/trades/?pair=" + item.Key + "_" + item.Value;
                    WebResponse resp = GetRequst.Requst(site);


                    using (StreamReader stream = new StreamReader(
                         resp.GetResponseStream(), Encoding.UTF8))
                    {

                        string str = stream.ReadToEnd();
                        dynamic res = JsonConvert.DeserializeObject(str);

                    }
                }
                catch (Exception e)
                {

                }
            }
            return temp;
        }
        public Task<Dictionary<string, TransformInfo>> GetInfoAsync()
        {
            throw new NotImplementedException();
        }
    }

    public class ExmoGetOrders : IGetOrders
    {
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
            string site = "	https://api.exmo.com/v1/order_book/?pair=" + SecondCoinName + "_" + MainCoinName;
            TransforfOrders temp = new TransforfOrders();
            List<List<decimal>> ask = new List<List<decimal>>();
            List<List<decimal>> bid = new List<List<decimal>>();
            try
            {
                WebResponse resp = ExmoGetRequst.Requst(site);
                using (StreamReader stream = new StreamReader(
                       resp.GetResponseStream(), Encoding.UTF8))
                {
                    string str = stream.ReadToEnd();
                    if (str == "{}")
                    {
                        return temp;
                    } 
                    var res = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(str);
                    if (res.First().Value.ask==null)
                    {
                        return temp;
                    }
                    foreach (var i in res.First().Value.ask)
                    {
                        decimal price = i[0];
                        decimal count = i[1];
                        var buf = new List<decimal>();
                        buf.Add(price);
                        buf.Add(count);
                        ask.Add(buf);
                    }
                    foreach (var i in res.First().Value.bid)
                    {
                        decimal price = i[0];
                        decimal count = i[1];
                        var buf = new List<decimal>();
                        buf.Add(price);
                        buf.Add(count);
                        bid.Add(buf);
                    }
                    temp = new TransforfOrders(ask, bid);

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
            return Task<Dictionary<string, TransforfOrders>>.Factory.StartNew(() => GetOrders(arg));
        }

        public Task<TransforfOrders> GetOrderAsync(string MainCoinName, string SecondCoinName)
        {
            return Task<TransforfOrders>.Factory.StartNew(() => GetOrder(MainCoinName, SecondCoinName));
        }
    }

    public class ExmoWallet : IWallet
    {
        public string Balance = "user_info";
        public string Address = "deposit_address";
        public class ExmoBalanceField
        {
            public Dictionary<string, string> balances;
            public Dictionary<string, string> reserved;
        }
        public Dictionary<string, TransformBallans> GetBalances()
        {
            var postData = new Dictionary<string, string>();
            var str = ExmoPostRequst.PostString(Balance, postData);
            Dictionary<string, TransformBallans> temp = new Dictionary<string, TransformBallans>();
            var t = JsonConvert.DeserializeObject<ExmoBalanceField>(str);

            foreach (var item in t.balances)
            {
                var buf = new TransformBallans();
                buf.Available = Convert.ToDecimal(item.Value.Replace('.', ','));
                buf.OnOrders = Convert.ToDecimal(t.reserved.Where(x => x.Key == item.Key).
                                                            First().Value.Replace('.', ','));
                temp.Add(item.Key, buf);
            }
            return temp;
        }
        public Dictionary<string, string> GetDepositAddresses()
        {
            var postData = new Dictionary<string, string>();
            var str = ExmoPostRequst.PostString(Balance, postData);
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
    public class Exmo : Stock
    {
        public Exmo()
        {
            StockName = "Exmo";
            Info = new ExmoInfo();
            Orders = new ExmoGetOrders();
            Ballans = new ExmoWallet();
            Traid = new ExmoTraid();
        }
    }

    public class ExmoTraid : ITrading
    {
        public Task<string> PostOrderAsync(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote)
        {
            throw new NotImplementedException();
        }

        public Task<TransformWithdrow> PostWihdrowAsync(string currencyPair, string adrress, decimal amountQuote)
        {
            throw new NotImplementedException();
        }
    }
}
