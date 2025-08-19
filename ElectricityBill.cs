
using System;
using System.Text.RegularExpressions;

public class ElectricityBill
{
    public string ConsumerNumber
    {
        get { return consumerNumber; }
        set
        {
            if (!Regex.IsMatch(value ?? "", @"^EB\d{5}$"))
                throw new FormatException("Invalid Consumer Number");
            consumerNumber = value;
        }
    }
    private string consumerNumber;

    public string ConsumerName { get; set; }
    public int UnitsConsumed { get; set; }
    public double BillAmount { get; set; }
}
