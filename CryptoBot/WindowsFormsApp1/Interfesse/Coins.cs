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
    public class TransformOrders
    {
        //price and quntity
        public Dictionary<decimal, decimal> asks = new Dictionary<decimal, decimal>();
        public Dictionary<decimal, decimal> bids= new Dictionary<decimal, decimal>();

        public TransformOrders() { }
        public TransformOrders(List<List<decimal>> a, List<List<decimal>> b)
        {
            asks = new Dictionary<decimal, decimal>();
            bids = new Dictionary<decimal, decimal>();
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
        public TransformOrders(List<QuantityRate> a, List<QuantityRate> b)
        {
            asks = a.ToDictionary(d => d.Rate, k=>k.Quantity);
            bids = b.ToDictionary(d => d.Rate, k => k.Quantity); 
        }
        public TransformOrders(Dictionary<decimal, decimal> a, Dictionary<decimal, decimal> b)
        {
            asks = a;
            bids = b;
        }
    }
    public class TransformInfo
    {
        public decimal WithdrawFee { get; set; }
        public bool Status { get; set; }


        public TransformInfo(decimal fee, bool status)
        {
            WithdrawFee = fee;
            Status = status;
        }
        public TransformInfo(LiveCoinInfoFields b)
        {
            WithdrawFee = b.withdrawFee;
            Status = b.walletStatus == "normal" ? true: false;
        }
        public TransformInfo(PoloniexCoinsInfoFields b)
        {
            WithdrawFee = b.txFee;
            Status = b.disabled != 0 ? true : false;
        }
        public TransformInfo(BittrexResultFields b)
        {
            WithdrawFee = b.TxFee;
            Status = b.IsActive;
        }
        
        public TransformInfo(CryptoCoinResaltInfo b)
        {
            WithdrawFee = b.WithdrawFee;
            Status = b.Status == "OK" ? true : false;
        }
        public TransformInfo(YobitCoinIdentetiFields b)
        {
             WithdrawFee = b.fee;
        }

        public TransformInfo()
        {
        }
    }
    public class LiveAndPoloniexField
    {
        public List<List<decimal>> asks;
        public List<List<decimal>> bids;
    }

    public class TransformWithdrow
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string Wallet { get; set; }
        public DateTime Date { get; set; }
        public TransformWithdrow()
        {

        }
        public TransformWithdrow(int id, string currencyPair, string address, decimal amount, DateTime date)
        {
            Id = id;
            Amount = amount;
            Currency = currencyPair;
            Wallet = address;
            Date = date;
        }

    }

    public class TransformBallans
    {

        public decimal Available { get; set; }
        public decimal OnOrders { get; set; }

        public TransformBallans() { }
        public TransformBallans(decimal available, decimal onorders)
        { 
            Available = available;
            OnOrders = onorders;
        }
        public TransformBallans(PoloniexWalletField arg)
        {
            Available = arg.available;
            OnOrders = arg.onOrders;
        }
    }

    





}
