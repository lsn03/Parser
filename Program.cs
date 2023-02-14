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
            string url = "https://www.chitai-gorod.ru/product/vtoromu-igroku-prigotovitsya-2951034";
            
            DoParse( url );
        }
        static void DoParse(string url )
        {
           // WebClient client = new WebClient();
           // client.Encoding = System.Text.Encoding.UTF8;
           // string document = client.DownloadString(url);
            //Console.WriteLine( document );
            
           // int indexOfStartDiv =  document.IndexOf("<div class=\"product-detail-title__authors\">");

            IDocument document2 = GetDocument(url);

            var textWitHResultSearchElements = document2.GetElementsByClassName("product-detail-title__author");
            

                
            string  r = textWitHResultSearchElements[0].TextContent.Trim(new char[] { ' ','\n' } );
            

            Console.WriteLine(r);
            Console.WriteLine( GetAuthor( document2 ) );
            Console.WriteLine( GetName( document2 ) );
            // string res = document[indexOfStartDiv].ToString();

            // Console.WriteLine( res );
        }
        public static IDocument GetDocument( string url )
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            return context.OpenAsync( url ).Result;
        }
        static string GetAuthor(IDocument doc )
        {
            var textWitHResultSearchElements = doc.GetElementsByClassName("product-detail-title__author");
            return  textWitHResultSearchElements[0].TextContent.Trim(new char[] { ' ','\n' } );   
        }
        static string GetName( IDocument doc )
        {
            var textWitHResultSearchElements = doc.GetElementsByClassName("product-detail-title__header");
            return textWitHResultSearchElements[0].TextContent;
        }
    }
}
