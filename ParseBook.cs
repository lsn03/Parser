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
	class ParseBook
	{
		private string _url;
		private IDocument _document;
		
		private string _author;

		public ParseBook(string url )
		{
			this._url = url;
			_document = GetDocument();
            if ( _document == null )
            {
				Console.WriteLine( "Error to parse this url: " + url );
				
				return;
            }
		}
		private IDocument GetDocument()
		{
			try
			{
				var config = Configuration.Default.WithDefaultLoader();
				var context = BrowsingContext.New(config);
				return context.OpenAsync( _url ).Result;
			}
			catch(Exception ex )
            {
				Console.WriteLine( ex.Message );
				return null;
            }
		}

		public Book GetBook()
		{
			Book book = new Book()
			{
				author = GetAuthor(),
				name = GetName(),
				description = GetDescription(),
				price = GetPrice(),
				sourceName = GetSourceName(),
				//image = GetImage(),
				//genre = GetGenre(),
				numberOfPages = GetNumberOfPages(),
				publisherName = GetPublisherName(),
				isbn = GetISBN(),
			};
			
			
			
			return book;
		}


		private string GetAuthor()
		{
			try
			{
				var textWitHResultSearchElements = _document.GetElementsByTagName("td");
				for(int i = 0; i < textWitHResultSearchElements.Length;i++ )
                {
                    if ( textWitHResultSearchElements[i].TextContent.Contains( "Автор" ) )
                    {
						string res = textWitHResultSearchElements[i + 1].TextContent.Trim( new char[] { '\t', '\n' } );
						res = res.Replace( '\n',' ' );
						res = res.Trim( ' ' );
						_author = res;
						return res;
					}
                }
				_author = null;
				return null;
			}
			catch ( Exception ex )
			{
				_author = null;
				return null;
				return ex.Message;
			}
		}

		private string GetName()
		{
			try
			{
				var textWitHResultSearchElements = _document.GetElementsByClassName("title");

				if ( _author == null )
					return textWitHResultSearchElements[0].TextContent;
				for (int i = 0;i< textWitHResultSearchElements.Length;i++ )
                {
                    if ( textWitHResultSearchElements[i].TextContent.Contains( _author ) )
                    {
						string[] res = textWitHResultSearchElements[i].TextContent.Split(_author+" ");
						return res[1];
					}
                }
				
				return null;
			
			
			}
			catch ( Exception ex )
			{
				return null;
				return ex.Message;
			}
		}
		private string GetDescription()
		{

            try
            {
				var textWitHResultSearchElements = _document.GetElementsByClassName("description editor-content");

				//int index = textWitHResultSearchElements[0].TextContent.IndexOf("<span>");

				string res = textWitHResultSearchElements[0].TextContent.Trim(new char[]{'\n',' ' } );
				var r = res.Split("Описание\n");
				string result = r[1].Trim(' ');
				result = result.Replace( '\u00A0', ' ' );
				result = result.Trim( new char[] { ' ', '\n' } );
				return result;
            }catch(Exception ex )
            {
				return null;
				return ex.Message;
            }
		}
		private string GetPrice()
		{
			try
			{
				var textWitHResultSearchElements = _document.GetElementById("price-field");
				string priceContent =  textWitHResultSearchElements.TextContent;
				string res = priceContent.Remove(priceContent.Length-4,4);
				return res;
			}
			catch ( Exception ex )
			{
				return null;
				return ex.Message;
			}
		}
		private string GetSourceName()
		{
			try
			{
				var resArray = _url.Split('/');

				return resArray[2];
			}
			catch ( Exception ex )
			{
				return null;
				return ex.Message;
			}
		}
		private string GetImage()
		{
			try
			{
				return "";
			}
			catch ( Exception ex )
			{
				return ex.Message;
			}
		}

		private string GetGenre()
		{
			try
			{
				var textWitHResultSearchElements = _document.GetElementsByClassName("breadcrumbs center-align");
				
				return textWitHResultSearchElements[0].TextContent;

				//"breadcrumbs center-align";
			}
			catch ( Exception ex )
			{
				return ex.Message;
			}
		}
		
		private string GetNumberOfPages()
		{
			try
			{
				var textWitHResultSearchElements = _document.GetElementsByTagName("td");
				for ( int i = 0; i < textWitHResultSearchElements.Length; i++ )
				{
					if ( textWitHResultSearchElements[i].TextContent.Contains( "Страниц в книге:" ) )
					{
						string res = textWitHResultSearchElements[i + 1].TextContent.Trim( new char[] { '\t', '\n' } );
						res = res.Replace( '\n', ' ' );
						res = res.Trim( ' ' );
						
						return res;
					}
				}
				
				return null;
			}
			catch(Exception ex )
            {
				return null;
				return ex.Message;
            }
		}

		private string GetPublisherName()
		{
			try
			{
				var textWitHResultSearchElements = _document.GetElementsByTagName("td");
				for ( int i = 0; i < textWitHResultSearchElements.Length; i++ )
				{
					if ( textWitHResultSearchElements[i].TextContent.Contains( "Издательство" ) )
					{
						string res = textWitHResultSearchElements[i + 1].TextContent.Trim( new char[] { '\t', '\n' } );
						res = res.Replace( '\n', ' ' );
						res = res.Trim( ' ' );

						return res;
					}
				}

				return null;
				
			}
			catch ( Exception ex )
			{
				return null;
				return ex.Message;
			}
		}

		private string GetISBN()
		{
			try
			{
				var textWitHResultSearchElements = _document.GetElementsByTagName("td");
				for ( int i = 0; i < textWitHResultSearchElements.Length; i++ )
				{
					if ( textWitHResultSearchElements[i].TextContent.Contains( "ISBN" ) )
					{
						string res = textWitHResultSearchElements[i + 1].TextContent.Trim( new char[] { '\t', '\n' } );
						res = res.Replace( '\n', ' ' );
						res = res.Trim( ' ' );

						return res;
					}
				}

				return null;
			}
			catch ( Exception ex )
			{
				return null;
				return ex.Message;
			}
		}

	}
}
