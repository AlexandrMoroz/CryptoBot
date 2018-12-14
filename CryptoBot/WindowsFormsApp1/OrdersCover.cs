using System.Collections.Generic;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class OrdersCoverOneStock
    {
        public Stock stock;
        public Task<Dictionary<string, TransformOrders>> CoinMainCoin;
        public Task<Dictionary<string, TransformOrders>> CoinValue;
        public Task<TransformOrders> MainCoinValue;
        public OrdersCoverOneStock(Stock stockArg, List<string> Maincoins,string SecondCoin,string ValueCoin)
        {
            stock = stockArg;
            var TempCoins = new List<KeyValuePair<string, string>>();
            foreach (var item in Maincoins)
            {
                TempCoins.Add(new KeyValuePair<string, string>(SecondCoin,item));
            }    
            CoinMainCoin = stockArg.Orders.GetOrdersAsync(TempCoins);
            
            TempCoins = new List<KeyValuePair<string, string>>();
            foreach (var item in Maincoins)
            {
                TempCoins.Add(new KeyValuePair<string, string>(ValueCoin, item));
            }
            CoinValue = stockArg.Orders.GetOrdersAsync(TempCoins);
            
            MainCoinValue = stockArg.Orders.GetOrderAsync(SecondCoin, ValueCoin);
            Task.WaitAll(CoinMainCoin);
            Task.WaitAll(MainCoinValue);
            Task.WaitAll(CoinValue);
        }
    }
    public class OrdersCover2Stock
    {
        public Stock stock;
        public Task<Dictionary<string, TransformOrders>> order;
        public OrdersCover2Stock(Stock stockArg, List<KeyValuePair<string, string>> pairs)
        {
            stock = stockArg;
            order = stockArg.Orders.GetOrdersAsync(pairs);

        }

    }
}