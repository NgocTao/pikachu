using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public static class TextUtils
{
    public static string FormatNumber(long money, string prefix = "")
    {
        string moneyString = "";
        if (money == 0)
        {
            string prefixStr = prefix;
            moneyString = prefixStr + "0";
            return moneyString;
        }

        long startMoney = 1000000000000;

        if (money >= startMoney)
        {
            startMoney = 1000000000;
            moneyString += prefix;
            long longMoney = money;
            int rem = 10000;
            long billion = (longMoney / startMoney);
            long million = (longMoney % startMoney) / rem;

            moneyString += string.Format("{0}", billion);

            if (million > 0)
            {
                string temStr = string.Format("{0}", million);
                int size = string.Format("{0}", rem).Length;
                while (temStr.Length < size)
                {
                    temStr.Insert(0, "0");
                }

                foreach (var letter in temStr)
                {
                    if (letter != '0')
                        break;
                    else
                        temStr.Remove(letter);
                }

                moneyString += ",";
                moneyString += temStr;
            }

            moneyString += " B";
            return moneyString;
        }

        char[] strNum = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', ',' };
        bool isNegative = money < 0;
        moneyString = "";
        int split = 0;
        money = Mathf.Abs((int)money);
        long newMoney = money;
        while (newMoney > 0)
        {
            long n = newMoney % 10;
            newMoney /= 10;
            moneyString += strNum[n];
            split++;
            if (split == 3 && newMoney > 0)
            {
                moneyString += strNum[10];
                split = 0;
            }
        }
        if (isNegative)
        {
            moneyString += "-";
        }
        if (prefix.CompareTo("") != 0)
        {
            moneyString += prefix;
        }

        char[] charArray = moneyString.ToCharArray();
        System.Array.Reverse(charArray);
        moneyString = new string(charArray);

        return moneyString;
    }
    public static Color HexToColor(string hex)
    {
        hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
        byte a = 255;//assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }

    // public static string GetTransactionID(string receipt)
    // {
    //     var orderID = string.Empty;
    //     var receipt_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(receipt);
    //     var payload = (string)receipt_wrapper["Payload"];
    //     var payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(payload);
    //     if (payload_wrapper.ContainsKey("json"))
    //     {
    //         var json = (string)payload_wrapper["json"];
    //         var json_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(json);
    //         orderID = (string)json_wrapper["orderId"];
    //         return orderID;
    //     }
    //     return "empty";
    // }
}

public static class StringExtensions
{
    public static string FirstCharToUpper(this string input)
    {
        var subStr1 = input.Substring(0, 1);
        var subStr2 = input.Substring(1, input.Length - 1);
        return subStr1.ToUpper() + subStr2.ToLower();
    }
}