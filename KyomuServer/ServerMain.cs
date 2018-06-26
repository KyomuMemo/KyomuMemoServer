﻿using System;
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

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
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
            bool tudukeruFlag = true;
            listner.Start();
            while (tudukeruFlag)
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
            void writemessage(string str)
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
                ostr.Write(buffer, 0, buffer.Length);
            }

            res.AddHeader("Access-Control-Allow-Origin","*");
            await Task.Run(() =>
            {
                var apiurl = req.RawUrl.Split("/");
                if (apiurl.Length > 1)
                    switch (apiurl[1])
                    {
                        case "account":
                            if (apiurl.Length == 4)
                                switch (apiurl[3])
                                {
                                    case "create":
                                        writemessage(mAccount.AccountCreate(apiurl[2], out statusCode).ToString());
                                        break;
                                    case "getid":
                                        writemessage(mAccount.AccountRefer(apiurl[2], out statusCode).ToString());
                                        break;
                                    default:
                                        statusCode = 404;
                                        writemessage(messagejson("api error").ToString());
                                        break;
                                }
                            else { statusCode = 404; writemessage(messagejson("api error").ToString()); }
                            break;
                        case "memo":
                            if (apiurl.Length == 5)
                                switch (apiurl[4])
                                {
                                    case "create":
                                        writemessage(mFusen.CreateFusen(apiurl[2], apiurl[3], out statusCode).ToString());
                                        break;
                                    case "get":
                                        writemessage(mFusen.GetFusenAllData(apiurl[2], out statusCode).ToString());
                                        break;
                                    case "update":
                                        try
                                        {
                                            var reader = new System.IO.StreamReader(req.InputStream);
                                            var body = reader.ReadToEnd();
                                            writemessage(mFusen.UpdateFusen(apiurl[2], apiurl[3], JObject.Parse(body), out statusCode).ToString());
                                        }
                                        catch (Newtonsoft.Json.JsonReaderException e)
                                        {
                                            writemessage(messagejson(e.Message).ToString());
                                            statusCode = 406;
                                        }
                                        break;
                                    case "delete":
                                        writemessage(mFusen.DeleteFusen(apiurl[2], apiurl[3], out statusCode).ToString());
                                        break;
                                    default:
                                        statusCode = 404;
                                        writemessage(messagejson("api error").ToString());
                                        break;
                                }
                            else { statusCode = 404; writemessage(messagejson("api error").ToString()); }
                                break;
                        default:
                            statusCode = 404;
                            writemessage(messagejson("api error").ToString());
                            //flag = false;//暫定的に終了のためのコマンドとして使っている
                            break;
                    }
                else { statusCode = 404; writemessage(messagejson("api error").ToString()); }
                res.StatusCode = statusCode;
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
