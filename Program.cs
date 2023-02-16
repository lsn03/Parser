using System;
using System.Diagnostics;   // Нужна, чтобы запускать внешние процессы
using System.Net;           // Нужна, чтобы работать с Web
using System.Threading;
using AngleSharp;
using AngleSharp.Dom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
namespace Parser
{
    class Program
    {
        static void Main( string[] args )
        {
            string url = "https://svoi-knigi.ru/product?product_id=351741907";
            string url2 = "https://svoi-knigi.ru/collection/poeziya/product/skandiaka-n-1242004";
            Parser parser = new Parser(url);
            Parser parser2 = new Parser(url2);
            Book res = parser.GetResult();
            Book res2 = parser2.GetResult();
            List<Book> result = new();
            result.Add( res );
            result.Add( res2 );

            var options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true,

            };

            string res1 = JsonSerializer.Serialize(result,options);
            using ( StreamWriter writter = new( "D:\\Programming\\BookCourt\\Parser\\svoi-knigi.json" ) )
            {
                
                writter.WriteLine( res1 );

                
            }
            Console.WriteLine( res1 );
        }
    }
}
