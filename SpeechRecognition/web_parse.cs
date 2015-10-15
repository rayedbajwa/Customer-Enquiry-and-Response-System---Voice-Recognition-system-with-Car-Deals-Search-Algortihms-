using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HtmlAgilityPack;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Data.SqlServerCe;
using System.Windows;
using System.Data;
using System.Threading;


namespace SpeechRecognition
{
    class web_parse
    {
        String col;
        String num;
        String compDesc;
        SqlCeConnection con = new SqlCeConnection(@"Data Source=C:\Users\RnBz\Documents\Visual Studio 2012\Projects\Speech Recognition2\SpeechRecognition\bin\Debug\icers.sdf");
        String price;
        String reg_city;
        int record = 0;
        public web_parse()
        {
            
            try
            {
                string docString = HtmlDocument("http://www.pakwheels.com/used-cars/search/");
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(docString);

                foreach (HtmlNode nod in htmlDoc.DocumentNode.SelectNodes("//*[@id=\"main-container\"]/div[4]/div[2]/div[1]/div/div/div[2]/div/div[3]/ul/li/div/div/div[2]"))
                {
                    if (nod.InnerText == null) Console.WriteLine("null");
                    String a = nod.InnerText;


                  //  Console.WriteLine(a);
                }
                foreach (HtmlNode nodUrl in htmlDoc.DocumentNode.SelectNodes("//*[@id=\"main-container\"]/div[4]/div[2]/div[1]/div/div/div[2]/div/div[3]/ul/li/div/div/div[@class='span7']"))
                {

                    HtmlNode contactLink = nodUrl.Element("a");
                    String link = contactLink.Attributes["href"].Value;

                    String carN = contactLink.Element("h3").InnerText;

                    String loc = nodUrl.Element("p").InnerText;
                    HtmlNode desc = nodUrl.Element("div");
                    HtmlNode smallDesc = desc.Element("ul");

                    String year = smallDesc.Element("li").InnerText;

                    String mil = smallDesc.SelectSingleNode("li[2]").InnerText;

                    String trans = smallDesc.SelectSingleNode("li[3]").InnerText;
                    String fuel = smallDesc.SelectSingleNode("li[4]").InnerText;
                    compDesc = desc.Element("p").InnerText;
                    compDesc = compDesc.Replace(System.Environment.NewLine, "");

                    HtmlAgilityPack.HtmlDocument listings = new HtmlAgilityPack.HtmlDocument();
                    String docList = HtmlDocument("http://www.pakwheels.com" + link);
                    
                    listings.LoadHtml(docList);
                    Console.WriteLine("2nd node-job working");
                    try
                    {
                        foreach (HtmlNode nod in listings.DocumentNode.SelectNodes("//*[@id=\"main-container\"]/div[5]/div[2]/div[1]/h2"))
                        {
                            if (nod != null)
                            {
                                price = Regex.Replace(nod.InnerText, @"\D", "");
                                if (price == "")
                                {
                                    price = "Call";
                                }

                                Console.WriteLine(price);

                            }
                        }
                    }
                    catch (Exception e)
                    {
                        price = "Not Listed";

                    }
                    foreach (HtmlNode numNod in listings.DocumentNode.SelectNodes("//script[contains(text(), 'displayPhone')]"))
                    {
                        if (numNod != null)
                        {
                            num = numNod.InnerHtml;
                            Regex r = new Regex(Regex.Escape("(") + "(.*?)" + Regex.Escape(")"));
                            MatchCollection matches = r.Matches(num);
                            int i = 1;
                            foreach (Match match in matches)
                            {

                                if (i == 16)
                                    num = Regex.Replace(match.Groups[1].Value, @"\D", "");

                                i++;
                            }
                            Console.WriteLine(num);
                            try
                            {
                                foreach (HtmlNode colNod in listings.DocumentNode.SelectNodes("//*[@id=\"main-container\"]/div[5]/div[1]/table/tbody/tr[1]/td[4]"))
                                {
                                    col = colNod.InnerText;
                                    Console.WriteLine(col);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message +"   color");
                                col = "";
                            }
                            try
                            {
                                foreach (HtmlNode regNod in listings.DocumentNode.SelectNodes("//*[@id=\"main-container\"]/div[5]/div[1]/table/tbody/tr[7]/td[2]"))
                                {
                                    reg_city = regNod.InnerText;
                                    Console.WriteLine(reg_city);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message + "   color");
                                reg_city = "";
                            }
                        }


                    }

                   

                   num=num.Substring(0,11);



                    try
                    {

                        if (con.State == System.Data.ConnectionState.Closed)
                            con.Open();
                        SqlCeCommand cmd = new SqlCeCommand("INSERT INTO sellers (title,budget,engine,trans,year,milage,contact,description,dealer_name,colour,reg_city) VALUES (@val1,@val2,@val3,@val4,@val5,@val6,@val7,@desc,'pakwheels',@col,@reg)", con);

                        cmd.Parameters.AddWithValue("@val1", carN);
                        cmd.Parameters.AddWithValue("@val2", price);
                        cmd.Parameters.AddWithValue("@val3", fuel);
                        cmd.Parameters.AddWithValue("@val4", trans);
                        cmd.Parameters.AddWithValue("@val5", year);
                        cmd.Parameters.AddWithValue("@val6", mil);
                        cmd.Parameters.AddWithValue("@val7", num);
                        cmd.Parameters.AddWithValue("@desc", compDesc);
                        cmd.Parameters.AddWithValue("@col", col);
                        cmd.Parameters.AddWithValue("@reg", reg_city);
                        cmd.ExecuteNonQuery();
                        record++;
                        Console.WriteLine("added");
                        Thread t = new Thread(delegate()
                        {
                            loadingInfo check = new loadingInfo(carN + " being loaded...");
                            check.Show();
                            Thread.Sleep(1000);
                            check.Hide();
                        });
                        t.SetApartmentState(ApartmentState.STA);
                        t.Start();

                    }
                    catch (Exception er)
                    {
                        Thread t = new Thread(delegate()
                        {
                            loadingInfo check = new loadingInfo("Data Already Exists...."+carN);
                            check.Show();
                            Thread.Sleep(1000);
                            check.Hide();
                        });
                        t.SetApartmentState(ApartmentState.STA);
                        t.Start();
                    }

                }
                MessageBox.Show("Total Records Added: " + record);
            }
            catch (Exception e)
            {
               // MessageBox.Show(e.Message+e.InnerException);
                Console.WriteLine("error");
            }
        }

        string HtmlDocument(String url)
        {
            string text="";
            try
            {
                WebClient wc = new WebClient();
                Stream data = wc.OpenRead(url);
                StreamReader reader = new StreamReader(data);
               text= reader.ReadToEnd();
                reader.Close();
                data.Close();
              
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Console.WriteLine("html error");
            }
            return text;
        }

    }
}
