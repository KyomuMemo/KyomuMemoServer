using System;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

/*
"<host>"--"/account/<username>/"--"create"
        |                       |-"getid"
        |
        |-"/memo/<userid>/<memoid>/"--"create"
                                    |-"get"
                                    |-"update"
                                    |-"delete"
     */

namespace KyomuServer
{
    class ServerMain
    {
        static Mock.sAccount mAccount; static Mock.sFusen mFusen;
        static bool EndFlag = false;
        static string RootPath;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello Web World!");
            RootPath = System.IO.Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\..\..\..\KyomuMemoClient\public");//ルートディレクトリの指定
            string[] url = { "http://localhost:2000/" };
            mAccount = new Mock.sAccount(); mFusen = new Mock.sFusen(); //デバッグ用モック
            SimpleLister(url);
        }

        static void SimpleLister(string[] prefixes)
        {
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");
            var listner = new HttpListener();
            foreach(var s in prefixes) listner.Prefixes.Add(s);
            listner.Start();
            while (!EndFlag)
            {
                var cxt = listner.GetContext();
                SendInfoAsync(cxt);
            }
            listner.Stop();
        }
        
        static async void SendInfoAsync(HttpListenerContext context)
        {
            var req = context.Request;
            var res = context.Response;
            var ostr = res.OutputStream;
            int statusCode;
            string message;
            byte[] resbody;

            res.AddHeader("Access-Control-Allow-Origin","*");
            await Task.Run(() =>
            {
                var rawurl = (req.RawUrl!="/") ? req.RawUrl : "/index.html";
                var apiurl = rawurl.Split("/");
                if (apiurl.Length > 1)
                    switch (apiurl[1])
                    {
                        case "account":
                            if (apiurl.Length == 4)
                                switch (apiurl[3])
                                {
                                    case "create":
                                        message = sAccount.AccountCreate(apiurl[2], out statusCode).ToString();
                                        break;
                                    case "getid":
                                        message = sAccount.AccountRefer(apiurl[2], out statusCode).ToString();
                                        break;
                                    default:
                                        statusCode = 404;
                                        message = messagejson("要求URLが間違っています").ToString();
                                        break;
                                }
                            else { statusCode = 404; message = messagejson("要求URLが間違っています").ToString(); }
                            resbody = System.Text.Encoding.UTF8.GetBytes(message);
                            break;
                        case "memo":
                            if (apiurl.Length == 5)
                                switch (apiurl[4])
                                {
                                    case "create":
                                        message = sFusen.CreateFusen(apiurl[2], out statusCode).ToString();
                                        break;
                                    case "get":
                                        message = sFusen.GetFusenAllData(apiurl[2], out statusCode).ToString();
                                        break;
                                    case "update":
                                        try
                                        {
                                            var reader = new System.IO.StreamReader(req.InputStream);
                                            var body = reader.ReadToEnd();
                                            message = sFusen.UpdateFusen(apiurl[2], apiurl[3], JObject.Parse(body), out statusCode).ToString();
                                        }
                                        catch (Newtonsoft.Json.JsonReaderException e)
                                        {
                                            message = messagejson("JSONの形式に問題があります").ToString();
                                            statusCode = 406;
                                            Console.WriteLine(e.Message);
                                        }
                                        break;
                                    case "delete":
                                        message = sFusen.DeleteFusen(apiurl[2], apiurl[3], out statusCode).ToString();
                                        break;
                                    default:
                                        statusCode = 404;
                                        message = messagejson("要求URLが間違っています").ToString();
                                        break;
                                }
                            else { statusCode = 404; message=messagejson("要求URLが間違っています").ToString(); }
                            resbody = System.Text.Encoding.UTF8.GetBytes(message);
                            break;
                        default:
                            try
                            {
                                var filepath = RootPath + rawurl.Replace("/", "\\");
                                statusCode = 200;
                                resbody = System.IO.File.ReadAllBytes(filepath);
                            }catch(Exception e)
                            {
                                statusCode = 404;
                                Console.WriteLine(e.Message);
                                resbody = System.Text.Encoding.UTF8.GetBytes(messagejson("不正なアクセスの可能性があります").ToString());
                            }
                            break;
                    }
                else { statusCode = 404; resbody = System.Text.Encoding.UTF8.GetBytes(messagejson("要求URLが間違っています").ToString()); }
                res.StatusCode = statusCode;
                ostr.Write(resbody, 0, resbody.Length);
            });
            
            res.Close();
        }

        public static JObject messagejson(string message)
        {
            var json = new JObject();
            json.Add("message", new JValue(message));
            return json;
        }
    }
}




