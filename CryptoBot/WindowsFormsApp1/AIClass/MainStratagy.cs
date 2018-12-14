using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class MainStrategy
    {
       
            public string MarketName = "";
            public Stock BuyStockEX;
            public Stock SellStockEX;
            public Dictionary<decimal, decimal> StrategyBuy = new Dictionary<decimal, decimal>();
            public Dictionary<decimal, decimal> StrategySell = new Dictionary<decimal, decimal>();
        
    }
    public class BTCToUSDStrategy
    {
        public string MarketName = "";
        public Stock stock;
        public decimal CoinBuyQuantity;
        public decimal CoinSellPrice;
        public decimal BTCSellPrice;
    }
    public class StrategyField
    {
        //1 - price, 2 - count
        public Dictionary<decimal, decimal> Data = new Dictionary<decimal, decimal>();

        public override string ToString()
        {
            string temp = "";

            foreach (var i in Data)
            {
                temp += "цена: " + i.Key + " Количество: " + i.Value + '\n';
            }
            return temp;
        }
    }
}
