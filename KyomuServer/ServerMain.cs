using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

/*
"<host>"--"/account/"--"create"
        |            |-"getid"
        |
        |-"/memo/"--"create"
                   |-"get"
                   |-"update"
                   |-"delete"
     */

namespace KyomuServer
{
    class ServerMain
    {
        //static Mock.sAccount mAccount; static Mock.sFusen mFusen; //デバッグ用モック
        static bool EndFlag = false;

        //URLを設定しHttpListenerを開始
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string[] url = { "http://localhost:2000/" };
            //mAccount = new Mock.sAccount(); mFusen = new Mock.sFusen(); //デバッグ用モック
            HttpListener(url);
        }

        //Http要求を受け付ける
        static void HttpListener(string[] prefixes)
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
        

        // Httpの要求から送信内容を生成する関数
        static async void SendInfoAsync(HttpListenerContext context)
        {
            var req = context.Request;
            var res = context.Response;
            var ostr = res.OutputStream;
            int statusCode;
            string message;
            void writemessage(string str)
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
                ostr.Write(buffer, 0, buffer.Length);
            }

            res.AddHeader("Access-Control-Allow-Origin","*");
            await Task.Run(() =>
            {
                var apiurl = req.RawUrl.Split("/");
                if (apiurl.Length == 3)
                {
                    try
                    {
                        JObject reqjson;
                        var reader = new System.IO.StreamReader(req.InputStream);
                        var body = reader.ReadToEnd();
                        reqjson = JObject.Parse(body);
                        switch (apiurl[1])
                        {
                            case "account":
                                switch (apiurl[2])
                                {
                                    case "create":
                                        message = sAccount.AccountCreate(reqjson["userName"].Value<string>(), out statusCode).ToString();
                                        break;
                                    case "getid":
                                        message = sAccount.AccountRefer(reqjson["userName"].Value<string>(), out statusCode).ToString();
                                        break;
                                    default:
                                        statusCode = 404;
                                        message = messagejson("要求URLが間違っています").ToString();
                                        break;
                                }
                                break;
                            case "memo":
                                switch (apiurl[2])
                                {
                                    case "create":
                                        message = sFusen.CreateFusen(reqjson["userID"].Value<string>(), out statusCode).ToString();
                                        break;
                                    case "get":
                                        message = sFusen.GetFusenAllData(reqjson["userID"].Value<string>(), out statusCode).ToString();
                                        break;
                                    case "update":
                                        message = sFusen.UpdateFusen(reqjson, out statusCode).ToString();
                                        break;
                                    case "delete":
                                        message = sFusen.DeleteFusen(reqjson["userID"].Value<string>(), reqjson["fusenID"].Value<string>(), out statusCode).ToString();
                                        break;
                                    default:
                                        statusCode = 404;
                                        message = messagejson("要求URLが間違っています").ToString();
                                        break;
                                }
                                break;
                            default:
                                statusCode = 404;
                                message = messagejson("要求URLが間違っています").ToString();
                                //flag = false;//暫定的に終了のためのコマンドとして使っている
                                break;
                        }
                    }
                    catch (JsonException e)
                    {
                        message = messagejson("JSONの形式に問題があります").ToString();
                        statusCode = 406;
                        Console.WriteLine(e.Message);
                    }
                }
                else { statusCode = 404; message = messagejson("要求URLが間違っています").ToString(); }
                res.StatusCode = statusCode;
                writemessage(message);
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




