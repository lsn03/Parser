using System;
using System.Diagnostics;   // Нужна, чтобы запускать внешние процессы
using System.Net;           // Нужна, чтобы работать с Web
using System.Threading;
using AngleSharp;
using AngleSharp.Dom;
using System.Collections;
using System.Collections.Generic;
namespace Parser
{
    class Program
    {
        static void Main( string[] args )
        {
            string url = "https://svoi-knigi.ru/product?product_id=351741907";

            Parser parser = new Parser(url);

            Console.WriteLine(parser.GetResult());
        }
    }
}
