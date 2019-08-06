using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace csv_convertion
{
    public class FRecord
    {
        public string date { get; set; }
        public string type { get; set; }
        public List<Order> orders = null;
        public FRecord()
        {
            orders = new List<Order>();
        }

    }
   public  class Order
    {
        public string date { get; set; }
        public string code { get; set; }
        public string orderNumber { get; set; }
        public Buyer oBuyer = null;
        public Timing timings= null;       
        public List<Item> orderItems = null;
        public  Order()
        {
            orderItems = new List<Item>();
            oBuyer = new Buyer();
            timings = new Timing();
        }
      

    }
   public class Buyer
    {
       public string name { get; set; }
        public string street { get; set; }
        public string zip { get; set; }
    }
   public class Item
    {
        public string sku { get; set; }
        public string quantity { get; set; }
    }

   public class Timing
    {
        public int start { get; set; }
        public int stop { get; set; }
        public int gap { get; set; }
        public int offset { get; set; }
        public int pause { get; set; }

    }
    class Ender
    {
        public int process { get; set; }
        public int paid { get; set; }
        public int created { get; set; }

    }
    static class Program
    {
        
        static void Main(string[] args)
        {

            FRecord fileRecord = new FRecord();

            Order order = new Order();

            List<Order> lstOrder= new List<Order>();
           
            Buyer buyer = new Buyer();
          
            List<Item> items = new List<Item>();

            Timing timing = new Timing();

            Ender ender = new Ender();

            var finalList = new List<List<string>>();
            Dictionary<string, string> dict = new Dictionary<string, string>();

            
            string[] seps = { "," };

            foreach (var line in File.ReadLines(@"D:\csvR.csv"))
            {
                var fields = line
                    .Split(seps, StringSplitOptions.None)
                    .Select(s => s.Replace("\"", ""))
                    .ToList();

                finalList.Add(fields);
            }
           

            foreach (List<string> mylist in finalList)
            {
                
                if (mylist[0].Equals("F"))
                {
                    fileRecord.date = mylist[1];
                    fileRecord.type = mylist[2];
                   
                }
                else if (mylist[0].Equals("O"))
                {
              
                    order.date = mylist[1];
                    order.code = mylist[2];
                    order.orderNumber = mylist[3];
                    order.oBuyer = buyer;
                    order.timings = timing;
                    lstOrder.Add(order);


                }
                else if (mylist[0].Equals("B"))
                {

                    buyer.name = mylist[1];
                    buyer.street = mylist[2];
                    buyer.zip = mylist[3];

                }
                else if(mylist[0].Equals("L"))
                {
                  order.orderItems.Add(new Item
                   {
                        sku=mylist[1],
                        quantity=mylist[2],
                   });
                   
                }

                else if (mylist[0].Equals("T"))
                {
                    timing.start =Convert.ToInt16( mylist[1]);
                    timing.stop = Convert.ToInt16(mylist[2]);
                    timing.gap = Convert.ToInt16(mylist[3]);
                    timing.offset = Convert.ToInt16(mylist[4]);
                    timing.pause = Convert.ToInt16(mylist[5]);
                }
                else if (mylist[0].Equals("E"))
                {
                    ender.process = Convert.ToInt16(mylist[1]);
                    ender.paid = Convert.ToInt16(mylist[2]);
                    ender.created = Convert.ToInt16(mylist[2]);
                }
              
            }
           
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(@"D:\Output.json"))
            {
                sw.WriteLine("{");
                sw.WriteLine("\"date\":" + "\"" + fileRecord.date + "\"" + ",");
                sw.WriteLine("\"type\":" + "\"" + fileRecord.type + "\"" + ",");
                sw.WriteLine("\"orders\":[");
                         
                fileRecord.orders=lstOrder.Select(s => s).ToList();
                foreach (var lOrder in fileRecord.orders)
                {   
                        sw.WriteLine("{");
                        sw.WriteLine("\"Date\":" + "\"" + lOrder.date + "\"" + ",");
                        sw.WriteLine("\"Code\":" + "\"" + lOrder.code + "\"" + ",");
                        sw.WriteLine("\"Number\":" + "\"" + lOrder.orderNumber + "\"" + ",");
                        sw.WriteLine("\"buyer\":{");
                        sw.WriteLine("\"Name\":" + "\"" + lOrder.oBuyer.name + "\"" + ",");
                        sw.WriteLine("\"Street\":" + "\"" + lOrder.oBuyer.street + "\"" + ",");
                        sw.WriteLine("\"Zip\":" + "\"" + lOrder.oBuyer.zip + "\"");
                        sw.WriteLine("},");
                        sw.WriteLine("\"Items\":[");
                       
                    foreach (var lItem in lOrder.orderItems)
                    {
                        sw.WriteLine("{");
                        sw.WriteLine("\"sku\":" + "\"" + lItem.sku + "\"" + ",");
                        sw.WriteLine("\"qty\":" + "\"" + lItem.quantity + "\"" );
                        if(lItem==lOrder.orderItems.Last())
                        sw.WriteLine("}");
                        else
                            sw.WriteLine("},");
                    }
                    //sw.WriteLine("}");
                    sw.WriteLine("],");
                    sw.WriteLine("\"timings\":{");
                    sw.WriteLine("\"start\":" + "\"" + lOrder.timings.start + "\"" + ",");
                    sw.WriteLine("\"stop\":" + "\"" + lOrder.timings.stop + "\"" + ",");
                    sw.WriteLine("\"gap\":" + "\"" + lOrder.timings.gap + "\"" + ",");
                    sw.WriteLine("\"offset\" :" + "\"" + lOrder.timings.offset + "\"" + ",");
                    sw.WriteLine("\"pause\" :" + "\"" + lOrder.timings.pause + "\"" );
                    sw.WriteLine("}");
                    sw.WriteLine("}");
                    sw.WriteLine("],");
                }

                sw.WriteLine("\"Ender\":{");
                sw.WriteLine("\"process\":" + "\"" + ender.process + "\"" + ",");
                sw.WriteLine("\"paid\":" + "\"" + ender.paid + "\"" + ",");
                sw.WriteLine("\"created\":" + "\"" + ender.created + "\"" );
                sw.WriteLine("}");
                sw.WriteLine("}");
            }

        }
    }
}
