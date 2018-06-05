using System;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

/*
"<host>"--"/account/<username>/"--"create"
        |                       |-"getid"
        |                       |-"getmemoall"
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
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var listner = new HttpListener();
            
            var json = new JObject();//これがjson
            var jvar = new JValue("hoge"); //jsonの数値
            var jary = new JArray();//jsonの配列
        }

        void httpprocede(HttpListenerContext context)
        {
            var req = context.Request;
            var res = context.Response;
            var ostr = res.OutputStream;
            int statusCode = 440;

            var apiurl = req.RawUrl.Split("/");
            switch (apiurl[1])
            {
                case "account":
                    switch (apiurl[3])
                    {
                        case "create":
                            sAccount.AccountCreate(null,out statusCode);
                            break;
                        case "getid":
                            sAccount.AccountRefer(null,out statusCode);
                            break;
                        case "getmemoall":
                            sFusen.GetFusenAllData(int.Parse(apiurl[2]));
                            break;
                        default:
                            statusCode = 400;
                            break;
                    }
                    break;
                case "memo":
                    switch (apiurl[4])
                    {
                        case "create":
                            sFusen.CreateFusen(int.Parse(apiurl[2]), int.Parse(apiurl[3]), out statusCode);
                            break;
                        case "get":
                            sFusen.GetFusenAllData(int.Parse(apiurl[2]));
                            break;
                        case "update":
                            sFusen.UpdateFusen(int.Parse(apiurl[2]), int.Parse(apiurl[3]), null, out statusCode);
                            break;
                        case "delete":
                            sFusen.DeleteFusen(int.Parse(apiurl[2]), int.Parse(apiurl[3]), out statusCode);
                            break;
                        default:
                            statusCode = 400;
                            break;
                    }
                    break;
                default:
                    statusCode = 400;
                    break;
            }

            res.StatusCode = statusCode;
            res.Close();
        }
    }
}
