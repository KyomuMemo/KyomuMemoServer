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
            string[] url = { "http://localhost:2000/" };
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
                tudukeruFlag = SendInfo(cxt);
            }
            listner.Stop();
        }

        static bool SendInfo(HttpListenerContext context)
        {
            bool flag = true;
            var req = context.Request;
            var res = context.Response;
            var ostr = res.OutputStream;
            int statusCode = 440;
            void writemessage(string str)
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(str);
                ostr.Write(buffer, 0, buffer.Length);
            }

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
                            writemessage("<HTML><BODY> afun </BODY></HTML>");
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
                            var json = JObject.Parse(ServerTest01.Fsample);
                            writemessage(json.ToString());
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
                    statusCode = 440;
                    writemessage("<HTML><BODY> Hello, world</BODY></HTML>");
                    flag = false;
                    break;
            }

            res.StatusCode = statusCode;
            res.Close();
            return flag;
        }



    }
}
