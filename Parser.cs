using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using AngleSharp;
using AngleSharp.Dom;

namespace Parser
{
    class Parser
    {
        private string url;
        private List<Book> books = new();
        private List<string> links;
        private IDocument document;
        public Parser( string url )
        {
            this.url = url;
            this.document = GetDocument();
        }
        private IDocument GetDocument()
        {
            try
            {
                var config = Configuration.Default.WithDefaultLoader();
                var context = BrowsingContext.New(config);
                return context.OpenAsync( this.url ).Result;
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.Message );
                return null;
            }
        }
        private IDocument GetDocument(string url)
        {
            try
            {
                var config = Configuration.Default.WithDefaultLoader();
                var context = BrowsingContext.New(config);
                return context.OpenAsync( url ).Result;
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.Message );
                return null;
            }
        }
        public string GetResultJson()
        {
            DoParse();
            var options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true,

            };
            string resultJson = JsonSerializer.Serialize(books,options);
            return resultJson;
        }
        private void DoParse()
        {
            var textWithResultSearchElements = document.GetElementsByTagName("a");
            List<string> attributes = new();
            for(int i = 10; i <= 67;i ++ )
            {
                attributes.Add( textWithResultSearchElements[i].GetAttribute( "href" ) );
            }
            //textWithResultSearchElements[10].GetAttribute( "href" );
            //textWithResultSearchElements[67].GetAttribute( "href" );
            for(int i = 0; i< attributes.Count;i++ )
            {
                OpenPage( url + attributes[i] );
                
            }
        }
        private void OpenPage(string url)
        {
            try
            {
                IDocument doc = GetDocument(url);
                var textWithResultSearchElements = doc.GetElementsByClassName("pagination fr");
                int maxCountOfPages = 1,currentPage = 1;
                string countString = textWithResultSearchElements[0].TextContent;
                var res = countString.Split(" ");
                countString = res[res.Length - 7];
                countString = countString.Trim( '\n' );
                maxCountOfPages = int.Parse( countString );

                for ( currentPage = 1; currentPage <= maxCountOfPages; currentPage++ )
                {
                    IDocument LocalDoc;
                    Console.WriteLine(url+"\t\t"+ currentPage );
                    if ( currentPage == 1 )
                    {
                        LocalDoc = doc;

                        var resultGetElementsByTagA = LocalDoc.GetElementsByTagName("a");

                        int start = 72;
                        int end =resultGetElementsByTagA.Length-162;

                        for ( int i = start; i < end; i += 2 )
                        {
                            string currentURL = this.url;
                            currentURL += resultGetElementsByTagA[i].GetAttribute( "href" );
                            ParseBook parserBook = new(currentURL);
                            // Console.WriteLine( i + "\t" + currentURL );
                            Book book = parserBook.GetBook();
                            books.Add( book );


                        }
                    }
                    else
                    {
                        string urlPage = url + "?page=" + currentPage;
                       // Console.WriteLine( urlPage );
                        LocalDoc = GetDocument( urlPage );
                        var resultGetElementsByTagA = LocalDoc.GetElementsByTagName("a");

                        int start = 72+currentPage;
                        int end =resultGetElementsByTagA.Length-162;
                        for ( int i = start; i < end; i += 2 )
                        {
                            string currentURL = this.url;
                            currentURL += resultGetElementsByTagA[i].GetAttribute( "href" );
                            ParseBook parserBook = new(currentURL);
                           // Console.WriteLine( i + "\t" + currentURL );
                            Book book = parserBook.GetBook();
                            books.Add( book );


                        }

                    }

                }
            }catch(Exception ex )
            {
                Console.WriteLine( url+"\t\n\n"+ ex.Message );
            }

        }
    }
}
