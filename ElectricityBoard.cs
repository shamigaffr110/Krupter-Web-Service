
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ConnectionLib;

public class ElectricityBoard
{
    public void CalculateBill(ElectricityBill ebill)
    {
        int units = ebill.UnitsConsumed;
        double total = 0;
        int remaining = units;

        int slab = Math.Min(remaining, 100);
        total += slab * 0;
        remaining -= slab;

        if (remaining > 0)
        {
            slab = Math.Min(remaining, 200);
            total += slab * 1.5;
            remaining -= slab;
        }
        if (remaining > 0)
        {
            slab = Math.Min(remaining, 300);
            total += slab * 3.5;
            remaining -= slab;
        }
        if (remaining > 0)
        {
            slab = Math.Min(remaining, 400);
            total += slab * 5.5;
            remaining -= slab;
        }
        if (remaining > 0)
        {
            total += remaining * 7.5;
        }
        ebill.BillAmount = total;
    }

    public void AddBill(ElectricityBill ebill)
    {
        DBHandler dh = new DBHandler();
        using (SqlConnection con = dh.GetConnection())
        {
            using (SqlCommand cmd = new SqlCommand("INSERT INTO ElectricityBill (consumer_number, consumer_name, units_consumed, bill_amount) VALUES (@cno,@cname,@units,@amt)", con))
            {
                cmd.Parameters.AddWithValue("@cno", ebill.ConsumerNumber);
                cmd.Parameters.AddWithValue("@cname", ebill.ConsumerName);
                cmd.Parameters.AddWithValue("@units", ebill.UnitsConsumed);
                cmd.Parameters.AddWithValue("@amt", ebill.BillAmount);
                cmd.ExecuteNonQuery();
            }
        }
    }

    public List<ElectricityBill> Generate_N_BillDetails(int n)
    {
        List<ElectricityBill> list = new List<ElectricityBill>();
        DBHandler dh = new DBHandler();
        using (SqlConnection con = dh.GetConnection())
        {
            string q = "SELECT TOP (@n) consumer_number, consumer_name, units_consumed, bill_amount FROM ElectricityBill";
            using (SqlCommand cmd = new SqlCommand(q, con))
            {
                cmd.Parameters.AddWithValue("@n", n);
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        ElectricityBill eb = new ElectricityBill();
                        eb.ConsumerNumber = Convert.ToString(r["consumer_number"]);
                        eb.ConsumerName = Convert.ToString(r["consumer_name"]);
                        eb.UnitsConsumed = Convert.ToInt32(r["units_consumed"]);
                        eb.BillAmount = Convert.ToDouble(r["bill_amount"]);
                        list.Add(eb);
                    }
                }
            }
        }
        return list;
    }
}
