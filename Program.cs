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
using System.Text.Json.Serialization;


namespace Parser
{
   
    class Program
    {
        static void Main( string[] args )
        {

            Parser parser = new("https://svoi-knigi.ru");
            //string res = parser.GetResultJson();
            string file;
            List<Book> obj;
            List<Book> newBooks = new();
            using (StreamReader reader = new StreamReader( "D:\\Programming\\BookCourt\\Parser\\svoi-knigi.json" ) )
            {
                file = reader.ReadToEnd();

                 obj = JsonSerializer.Deserialize<List<Book>>( file );
                //JsonObject obj = JsonSerializer.Deserialize<JsonObject>( file );
                bool flag = false;
                for(int i = 0; i < obj.Count;i++ )
                {
                    flag = false;
                    int cntOfNull = 0;
                    if(obj[i].name!=null && obj[i].name.Contains( "Лучицкая С. Рыцари, крестоносцы и сарацины. Запад и Восток в эпоху крестовых походов." ) )
                    {
                        flag = true;
                    }
                    if ( obj[i].author == null ) cntOfNull++;
                    if ( obj[i].name == null ) cntOfNull++;
                    if ( obj[i].description == null ) cntOfNull++;
                    if ( obj[i].price == null ) cntOfNull++;
                    if ( obj[i].numberOfPages == null ) cntOfNull++;
                    if ( obj[i].publisherName == null ) cntOfNull++;
                    if ( obj[i].isbn == null ) cntOfNull++;

                    if ( cntOfNull >= 6 ) flag = true;

                    if ( obj[i].description != null )
                    {
                        obj[i].description = obj[i].description.Trim( '\n' );
                       // newBooks.Add( obj[i] );
                    }
                    if ( !flag )
                    {
                        newBooks.Add( obj[i] );
                    }
                    
                }
            }
            Console.WriteLine( obj.Count );
            using ( StreamWriter writter = new( "D:\\Programming\\BookCourt\\Parser\\svoi-knigi.json" ) )
            {
                var options = new JsonSerializerOptions
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    WriteIndented = true,

                };
                string resultJson = JsonSerializer.Serialize(newBooks,options);

                writter.WriteLine( resultJson );


            }
            //string url = "https://svoi-knigi.ru/product?product_id=351741907";
            //string url2 = "https://svoi-knigi.ru/collection/poeziya/product/skandiaka-n-1242004";
            //ParseBook parser = new ParseBook(url);
            //ParseBook parser2 = new ParseBook(url2);
            //Book res = parser.GetBook();
            //Book res2 = parser2.GetBook();
            //List<Book> result = new();
            //result.Add( res );
            //result.Add( res2 );

            //var options = new JsonSerializerOptions
            //{
            //    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            //    WriteIndented = true,
            //};

            //string res1 = JsonSerializer.Serialize(result,options);
            //using ( StreamWriter writter = new( "D:\\Programming\\BookCourt\\Parser\\svoi-knigi.json" ) )
            //{

            //    writter.WriteLine( res1 );


            //}
            //Console.WriteLine( res1 );
        }
    }
}
