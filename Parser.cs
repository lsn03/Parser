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
        private IDocument GetDocument()
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            return context.OpenAsync( _url ).Result;
        }

        public string GetResult()
        {
            Book book = new Book()
            {
                author = GetAuthor(),
                name = GetName(),
                isbn = GetISBN(),
                price = GetPrice(),
                numberOfPages = GetNumberOfPages(),
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
            var textWitHResultSearchElements = _document.GetElementsByClassName("green");
            return textWitHResultSearchElements[1].TextContent;
        }

        private string GetName()
        {
            var textWitHResultSearchElements = _document.GetElementsByClassName("pm-c-e-name");
            return textWitHResultSearchElements[0].TextContent;
        }
        private string GetISBN()
        {
            var textWitHResultSearchElements = _document.GetElementsByTagName("td");
            string res = textWitHResultSearchElements[43].TextContent;
            res = res.Trim(new char[] {'\n','\t',' ','-' } );
            var ISBN = res.Remove(0,9);
            return ISBN;
        }
        private string GetPrice()
        {
            var textWitHResultSearchElements = _document.GetElementsByClassName("catalog-price");
            string priceContent =  textWitHResultSearchElements[0].TextContent;
            string[] res = priceContent.Split(' ');
            return res[0];
            
        }
        private string GetNumberOfPages()
        {
            var textWitHResultSearchElements = _document.GetElementsByTagName("td");
            string res = textWitHResultSearchElements[43].TextContent;
            res = res.Trim( new char[] { '\n', '\t', ' ', '-' } );
            var numberOfPages = res.Remove(0,9);
            return numberOfPages;
        }

    }
}
