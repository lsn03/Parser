using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Parser
{
    class Parser
    {
        private string _url;
        private IDocument _document;
        

        public Parser(string url )
        {
            this._url = url;
            _document = GetDocument();
        }
        public string GetResult()
        {
            Book book = new Book()
            {
                author = GetAuthor()
            };
            var options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.BasicLatin,System.Text.Unicode.UnicodeRanges.Cyrillic)
                
            };
            string res= JsonSerializer.Serialize(book,options);
            
            
            return res;
        }
        private string GetAuthor()
        {
            var textWitHResultSearchElements = _document.GetElementsByClassName("product-detail-title__author");
            return textWitHResultSearchElements[0].TextContent.Trim( new char[] { ' ', '\n' } );
        }

        private string GetName( IDocument doc )
        {
            var textWitHResultSearchElements = doc.GetElementsByClassName("product-detail-title__header");
            return textWitHResultSearchElements[0].TextContent;
        }

        private IDocument GetDocument()
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            return context.OpenAsync( _url ).Result;
        }
    }
}
