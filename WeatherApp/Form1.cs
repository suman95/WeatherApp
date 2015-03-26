using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Web;
using System.Xml.Linq;
using System.Xml;
using System.Net;
using System.IO;
//using System.ServiceModel.Syndication;

namespace WeatherApp
{
    public partial class Form1 : Form
    {

        string Temperature;
        string Condition;
        string Humidity;
        string Windspeed;
        string Town;
        string Region;
        string Country;
        string Code;
        string Local;
        string woeid;
        string LastUpdate;
        string[] next_day=new string[10];
        string[] next_cond = new string[10];
        string[] next_condt = new string[10];
        string[] next_high = new string[10];
        string[] next_low = new string[10];
        //string IfCond;
        //string IfHigh;
        //string Iflow;


        public Form1()
        {
            InitializeComponent();
            woeid = "2294864";
            getWeather();
            label1.Text = Temperature;
            label2.Text = Town+", "+Region;
            label6.Text = Condition;
            label7.Text = Humidity;
            label8.Text = Windspeed;
            label10.Text = string.Format("\u00B0")+ "F";
            label12.Text = next_day[1];
            label15.Text = next_day[2];
            label17.Text = next_day[3];
            label19.Text = next_day[4];
            label20.Text = Country;
            label13.Text = next_condt[1];
            label14.Text = next_condt[2];
            label16.Text = next_condt[3];
            label18.Text = next_condt[4];

            label21.Text = next_high[1] + string.Format("\u00B0") + "F";
            label24.Text = next_high[2] + string.Format("\u00B0") + "F";
            label26.Text = next_high[3] + string.Format("\u00B0") + "F";
            label28.Text = next_high[4] + string.Format("\u00B0") + "F";
            label22.Text = next_low[1] + string.Format("\u00B0") + "F";
            label23.Text = next_low[2] + string.Format("\u00B0") + "F";
            label25.Text = next_low[3] + string.Format("\u00B0") + "F";
            label27.Text = next_low[4] + string.Format("\u00B0") + "F";
            setIcons();
            setIcon();

            comboBox1.Items.Add("Berhampur");
            comboBox1.Items.Add("Bhubaneshwar");
            comboBox1.Items.Add("Delhi");
            comboBox1.Items.Add("Kolkata");
            comboBox1.Items.Add("Mumbai");
            comboBox1.Items.Add("Chennai");
            comboBox1.Items.Add("Rourkela");
            comboBox1.Items.Add("New York");
            comboBox1.Items.Add("Los Angeles");
            comboBox1.Items.Add("London");
            comboBox1.Items.Add("Paris");
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void setIcons()
        {
            pictureBox1.Image = Image.FromFile(getString(next_cond[1]));
            pictureBox3.Image = Image.FromFile(getString(next_cond[2]));
            pictureBox4.Image = Image.FromFile(getString(next_cond[3]));
            pictureBox5.Image = Image.FromFile(getString(next_cond[4]));
        }

        private string getString(string code)
        {
            if(code.Equals("3200"))
            {
                return "../Pics/na.png";
            }
            else
            {
                return "../Pics/" + code + ".png";
            }
        }
        private void setIcon()
        {
            if(Code.Equals("3200"))
            {
                pictureBox2.Image = Image.FromFile("../Pics/na.png");
            }
            else
            {
                string st = "../Pics/"+Code+".png";
                pictureBox2.Image = Image.FromFile(st);
            }
        }
        private void getWeather()
        {
            if (CheckForInternetConnection())
            {
                comboBox1.Visible = true;
                label29.Text = "";
                FileStream fs = new FileStream("data.txt", FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                String st = String.Format(@"http://weather.yahooapis.com/forecastrss?w=");
                st = st + woeid;
                XmlDocument wData = new XmlDocument();
                wData.Load(st);

                XmlNamespaceManager manager = new XmlNamespaceManager(wData.NameTable);
                manager.AddNamespace("yweather", @"http://xml.weather.yahoo.com/ns/rss/1.0");

                XmlNode channel = wData.SelectSingleNode("rss").SelectSingleNode("channel");
                // XmlNodeList nodes = wData.SelectNodes("rss/channel/item/yweather:forecast", manager);

                Temperature = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["temp"].Value;
                sw.WriteLine(Temperature);
                Condition = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["text"].Value;
                sw.WriteLine(Condition);
                Code = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["code"].Value;
                sw.WriteLine(Code);
                Humidity = channel.SelectSingleNode("yweather:atmosphere", manager).Attributes["humidity"].Value;
                sw.WriteLine(Humidity);
                Windspeed = channel.SelectSingleNode("yweather:wind", manager).Attributes["speed"].Value;
                sw.WriteLine(Windspeed);
                Town = channel.SelectSingleNode("yweather:location", manager).Attributes["city"].Value;
                sw.WriteLine(Town);
                Region = channel.SelectSingleNode("yweather:location", manager).Attributes["region"].Value;
                sw.WriteLine(Region);
                Country = channel.SelectSingleNode("yweather:location", manager).Attributes["country"].Value;
                sw.WriteLine(Country);
                Local = channel.SelectSingleNode("item").SelectSingleNode("title", manager).InnerXml;
                sw.WriteLine(Local);
                LastUpdate = channel.SelectSingleNode("item").SelectSingleNode("pubDate", manager).InnerXml;
                sw.WriteLine(LastUpdate);
                //foreach( channel.SelectSingleNode("item").Select("yweather:forecast", manager))
                XmlNodeList forecast = channel.SelectSingleNode("item").SelectNodes("yweather:forecast", manager);

                for (int i = 0; i < forecast.Count; i++)
                {
                    next_day[i] = forecast[i].Attributes["day"].Value;
                    sw.WriteLine(next_day[i]);
                    next_cond[i] = forecast[i].Attributes["code"].Value;
                    sw.WriteLine(next_cond[i]);
                    next_condt[i] = forecast[i].Attributes["text"].Value;
                    sw.WriteLine(next_condt[i]);
                    next_high[i] = forecast[i].Attributes["high"].Value;
                    sw.WriteLine(next_high[i]);
                    next_low[i] = forecast[i].Attributes["low"].Value;
                    sw.WriteLine(next_low[i]);              
                }

                label11.Text = "Last Updated on : " + LastUpdate;
                sw.WriteLine(label11.Text);
                double x = (Double.Parse(Temperature) - 32) * 5.0 / 9.0;
                x = (int)x;
                label9.Text = x.ToString() + " " + string.Format("\u00B0") + "C";
                sw.WriteLine(label9.Text);
                //  IfCond = channel.SelectSingleNode("item").SelectSingleNode("yweather:forecast").Attributes["text"].Value;
                // IfHigh = channel.SelectSingleNode("item").SelectSingleNode("yweather:forecast").Attributes["high"].Value;
                // Iflow = channel.SelectSingleNode("item").SelectSingleNode("yweather:forecast").Attributes["low"].Value;
                sw.Flush();
                fs.Close();
            }
            else
            {
                comboBox1.Visible = false;
                label29.Text = "Offline";
                StreamReader file = new StreamReader("data.txt");
                Temperature = file.ReadLine();
                Condition = file.ReadLine();
                Code = file.ReadLine();
                Humidity = file.ReadLine();
                Windspeed = file.ReadLine();
                Town = file.ReadLine();
                Region = file.ReadLine();
                Country = file.ReadLine();
                Local = file.ReadLine();
                LastUpdate = file.ReadLine();

                for (int i = 0; i < 5; i++)
                {
                    next_day[i] = file.ReadLine();
                    next_cond[i] = file.ReadLine();
                    next_condt[i] = file.ReadLine();
                    next_high[i] = file.ReadLine();
                    next_low[i] = file.ReadLine();
                }

                label11.Text = file.ReadLine();
                label19.Text = file.ReadLine();
                file.Close();
            }
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var Client = new WebClient()) 
                using (var stream = Client.OpenRead("http://www.google.com"))
                {
                    return true;
                }

            }
            catch
            {
                return false;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            switch(comboBox1.Text)
            {
                case "Berhampur": woeid = "2295220"; break;
                case "Bhubaneshwar": woeid = "2294941"; break;
                case "Delhi": woeid = "2295019"; break;
                case "Chennai": woeid = "2295424"; break;
                case "Kolkata": woeid = "2295386"; break;
                case "Rourkela": woeid = "2294864"; break;
                case "Los Angeles": woeid = "2442047"; break;
                case "London": woeid = "44418"; break;
                case "Paris": woeid = "615702"; break;
                case "New York": woeid = "2459115"; break;
                default: woeid = comboBox1.Text; break;
            }
            getWeather();
            label1.Text = Temperature;
            label2.Text = Town + ", " + Region;
            label6.Text = Condition;
            label7.Text = Humidity;
            label8.Text = Windspeed;
            label10.Text = string.Format("\u00B0") + "F";
            label12.Text = next_day[1];
            label15.Text = next_day[2];
            label17.Text = next_day[3];
            label19.Text = next_day[4];
            label20.Text = Country;
            label13.Text = next_condt[1];
            label14.Text = next_condt[2];
            label16.Text = next_condt[3];
            label18.Text = next_condt[4];
            setIcons();
            setIcon();
        }
    }


}
