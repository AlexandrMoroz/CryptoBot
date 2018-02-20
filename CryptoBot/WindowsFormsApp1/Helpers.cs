using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public static class Helpers
    {
        public static string ToHttpPostString(this Dictionary<string, object> dictionary)
        {
            var output = string.Empty;
            foreach (var entry in dictionary)
            {
                var valueString = entry.Value as string;
                if (valueString == null)
                {
                    output += "&" + entry.Key + "=" + entry.Value;
                }
                else
                {
                    output += "&" + entry.Key + "=" + valueString.Replace(' ', '+');
                }
            }

            return output.Substring(1);
        }
        public static List<string> GetCoins()
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
        public static string DictionaryViewString(this Dictionary<decimal, decimal> arg)
        {
            string temp = "";
            foreach (var item in arg)
            {
                temp += "ЦЕНА :" + item.Key + " КОЛИЧЕСТВО :" + item.Value + "\n";
            }
            return temp;
        }
        
    }
}
