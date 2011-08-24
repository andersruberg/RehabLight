using System;
using System.Text;
using System.Drawing;

namespace RehabLight
{
	/// <summary>
	/// Summary description for PrintJournal.
	/// </summary>
	public class PrintJournal
	{
		private System.Drawing.Printing.PrintDocument printDocument;
		private float y;
		private int currentPageNr;
		private int nrofPages;
		private bool firstPage;
		private string contentString;
		private string name;
		private string personnumber;
		private string address;
		private string postAddress;

		private System.Drawing.Font fontPageNr;
		private System.Drawing.Font fontContent;
		private StringFormat stringFormat;


		public System.Drawing.Printing.PrintDocument PrintDocument
		{
			get{return printDocument;}
		}

		public PrintJournal()
		{
			printDocument = new System.Drawing.Printing.PrintDocument();
			printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDocument_PrintPage);
			printDocument.BeginPrint +=new System.Drawing.Printing.PrintEventHandler(printDocument_BeginPrint);
		}

		private void printDocument_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{
			firstPage = true;
			y = 0;
			currentPageNr = 0;
			nrofPages = 0;

			fontPageNr = new Font("Times New Roman", 10);
			fontContent = new Font("Times New Roman", 14);
			stringFormat = new StringFormat();
			stringFormat.Trimming = StringTrimming.Word;
			stringFormat.SetTabStops(0.0f, new float[] {120.0f});


		}

		public void Print()
		{
			printDocument.Print();
		}

		private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			RectangleF pageRect;
			currentPageNr++;
			
			//This should only be printed on the first page
			if (firstPage)
			{
				DrawHeader(e);
				SizeF totalContentSize = e.Graphics.MeasureString(contentString, fontContent, e.MarginBounds.Width, stringFormat);
				nrofPages = (int) Math.Ceiling((totalContentSize.Height + y)/e.MarginBounds.Height);
				
				firstPage = false;
				pageRect = new RectangleF(e.MarginBounds.Left, y, e.MarginBounds.Width, e.MarginBounds.Height-(y - e.MarginBounds.Top));
			}
			else
			{
				pageRect = new RectangleF(e.MarginBounds.Left, e.MarginBounds.Top, e.MarginBounds.Width, e.MarginBounds.Height);
			}

			string text = "sida " + currentPageNr.ToString() + " av " + nrofPages.ToString();
			SizeF size = e.Graphics.MeasureString(text, fontPageNr, e.MarginBounds.Width);
			e.Graphics.DrawString(text, fontPageNr, System.Drawing.Brushes.Black, e.MarginBounds.Left + (e.MarginBounds.Width / 2) - (size.Width / 2), e.MarginBounds.Bottom + size.Height);
			//Make room for the footer (number of pages) on the page
			pageRect.Height -= size.Height + 5;
;

			int charsFitted, linesFilled;
			e.Graphics.MeasureString(contentString, fontContent, pageRect.Size, stringFormat, out charsFitted, out linesFilled);
			
			
			string stringToPrint = contentString.Substring(0, charsFitted);
			e.Graphics.DrawString(stringToPrint, fontContent, System.Drawing.Brushes.Black, pageRect, stringFormat);
			
			if (charsFitted < contentString.Length)
			{
				e.HasMorePages = true;
				contentString = contentString.Substring(charsFitted);
			}
			else
				e.HasMorePages = false;
				
		}

		
		public void SetContent(Patient patient, System.Data.DataView dv)
		{
			StringBuilder s = new StringBuilder();

			for (int i = 0; i < dv.Count; i++)
			{
				s.Append(((string)dv[i]["visitdatetime"]).Substring(0,10));
				if ((bool)dv[i]["signed"] == true)
					s.Append("\t" + (string)dv[i]["note"] + "   [Sign/DR]" + "\n\n");
				else
				s.Append("\t" + (string)dv[i]["note"] + "\n\n");
			}

			contentString = s.ToString();

			name = patient.Firstname + " " + patient.Surname;
			personnumber = patient.Personnumber;
			address = patient.Street;
			postAddress = patient.Zipcode + " " + patient.City;

		}

		private void DrawHeader(System.Drawing.Printing.PrintPageEventArgs e)
		{
			int marginLeft = e.MarginBounds.Left;
			int marginRight = e.MarginBounds.Right;
			int marginTop = e.MarginBounds.Top;
			System.Drawing.Brush brush = System.Drawing.Brushes.Black;
			SizeF stringSize = new SizeF();
			
			System.Drawing.Font font = new Font("Arial", 20, System.Drawing.FontStyle.Bold);
			string text = "Journalutskrift";
			stringSize = e.Graphics.MeasureString(text, font);
			y = marginTop - stringSize.Height;
			e.Graphics.DrawString(text, font, brush, marginLeft, y);

			font = new Font("Arial", 10);
			text = "Utskriftdatum: " + System.DateTime.Now.ToShortDateString();
			e.Graphics.DrawString(text, font, brush, marginRight - stringSize.Width, y);

			y += stringSize.Height + 10;
			e.Graphics.DrawLine(new System.Drawing.Pen(brush, 1), marginLeft, y, marginRight, y);

			font = new Font("Arial", 10);
			text = "Vårdgivare:";
			stringSize = e.Graphics.MeasureString(text, font);
			y += stringSize.Height+10;
			e.Graphics.DrawString(text, font, brush, marginLeft, y);

			float _y = y;
			

			font = new Font("Arial", 12);
			text = "Leg. sjukgymnast\nDoris Ruberg\nRepslagaregatan 12\n602 32 Norrköping\nTelefon: 011-20 07 50";
			y += stringSize.Height+10;
			stringSize = e.Graphics.MeasureString(text, font);
			e.Graphics.DrawString(text, font, brush, marginLeft, y);

			font = new Font("Arial", 12);
			text = "Personnummer: " + personnumber + "\nNamn: " + name + "\nAdress: " + address + "\nPostadress: " + postAddress;
			stringSize = e.Graphics.MeasureString(text, font);
			e.Graphics.DrawString(text, font, brush, marginRight-stringSize.Width, y);

			font = new Font("Arial", 10);
			text = "Patient:";
			//stringSize = e.Graphics.MeasureString(text, font);
			e.Graphics.DrawString(text, font, brush, marginRight-stringSize.Width, _y);

			y += stringSize.Height+30;
			e.Graphics.DrawLine(new System.Drawing.Pen(brush, 1), marginLeft, y, marginRight, y);
			y += 20;


		}


	}
}
