using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public interface IGetOrders
    {
        Task<Dictionary<string, TransformOrders>> GetOrdersAsync(List<KeyValuePair<string, string>> arg);

        Task<TransformOrders> GetOrderAsync(string MainCoinName, string SecondCoinName);
    }
    public interface IGetInfo
    {

        Task<Dictionary<string, TransformInfo>> GetInfoAsync();

    }
    public interface IWallet
    {
        Dictionary<string, TransformBallans> GetBalances();
        Dictionary<string, string> GetDepositAddresses();
        Task<Dictionary<string, TransformBallans>> GetBalancesAsync();

        /// <summary>Returns all of your deposit addresses.</summary>
        Task<Dictionary<string, string>> GetDepositAddressesAsync();

        
    }
    public interface ITrading
    {
        Task<string> PostOrderAsync(string currencyPair, OrderType type, decimal pricePerCoin, decimal amountQuote);

        //Task<IList<IOrder>> GetOpenOrdersAsync(CurrencyPair currencyPair);
        Task<TransformWithdrow> PostWihdrowAsync(string currencyPair, string adrress, decimal amountQuote);
    }

    
    public class Stock
    {
        public string StockName { get;  set; }
        public IGetInfo Info;
        public IGetOrders Orders;
        public IWallet Ballans;
        public ITrading Traid;
        public Stock() { }
        public Stock(Stock arg)
        {
            StockName = arg.StockName;
            Info = arg.Info;
            Orders = arg.Orders;
            Ballans = arg.Ballans;
            Traid = arg.Traid;
        }
        public Stock(string stockname,IGetInfo info,IGetOrders orders, IWallet ballans,ITrading traid)
        {
            StockName = StockName;
            Info = info;
            Orders = orders;
            Ballans = ballans;
            Traid = traid;
        }
    }
}
