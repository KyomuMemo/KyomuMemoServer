using System;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

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
    }
}
