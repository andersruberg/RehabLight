using System;
using System.Drawing;
using System.Text;

namespace RehabLight
{
	/// <summary>
	/// Summary description for PrintPrimula.
	/// </summary>
	public class PrintPrimula
	{
		private System.Drawing.Printing.PrintDocument printDocument;

		public System.Drawing.Printing.PrintDocument PrintDocument
		{
			get{return printDocument;}
		}
		
		float y;
		bool firstPage;
		string contentString;
		string contentFooter;
		private int currentPageNr;
		private int nrofPages;

		private System.Drawing.Font fontPageNr;
		private System.Drawing.Font fontContent;
		private StringFormat stringFormat;


		public PrintPrimula()
		{
			printDocument = new System.Drawing.Printing.PrintDocument();
			printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDocument_PrintPage);
			printDocument.BeginPrint +=new System.Drawing.Printing.PrintEventHandler(printDocument_BeginPrint);
		}

		
	

		private void printDocument_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{
			firstPage = true;
			y = 0;
			nrofPages = 0;
			currentPageNr = 0;

			fontPageNr = new Font("Times New Roman", 10);
			fontContent = new Font("Times New Roman", 14);
			stringFormat = new StringFormat();
			stringFormat.Trimming = StringTrimming.Word;
			stringFormat.SetTabStops(0.0f, new float[] {110.0f, 300.0f, 110.0f, 30.0f});
		}


		public void SetContent(Utils.PatientNote[] patientNotesArray, int aTotalNrofVisits, int aNrofFreecards, int aNrofPatientPays, int aNrofYouths)
		{
			StringBuilder s = new StringBuilder();
			StringBuilder footer = new StringBuilder();

			footer.Append("\n\nTotalt antal besök: " + aTotalNrofVisits);
			footer.Append("\nAntal frikkortsbesök: " + aNrofFreecards);
			footer.Append("\nAntal besök där patienten betlat själv: " + aNrofPatientPays);
			footer.Append("\nAntal besök barn och ungdom: " + aNrofYouths);

			foreach(Utils.PatientNote patientNote in patientNotesArray)
			{
				Charge tmpCharge = patientNote.charge;
				Note tmpNote = patientNote.note;
				Patient tmpPatient = patientNote.patient;

				s.Append("\n" + tmpNote.VisitDateTime.ToShortDateString());
				s.Append("\t" + tmpPatient.Surname + " " + tmpPatient.Firstname);
				s.Append("\t" + tmpPatient.Personnumber);
				s.Append("\t" + tmpNote.PatientFee + " kr");
				s.Append("\t" + tmpCharge.PrimulaCharachter);

				//TODO: Make sure that if one row is longer than the width of the page, cut some chars from the name
			}

			contentString = s.ToString();
			contentFooter = footer.ToString();

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
				firstPage = false;

				SizeF totalContentSize = e.Graphics.MeasureString(contentString, fontContent, e.MarginBounds.Width, stringFormat);
				nrofPages = (int) Math.Ceiling((totalContentSize.Height + y + MeasureFooter(e))/e.MarginBounds.Height);
		
				pageRect = new RectangleF(e.MarginBounds.Left, y, e.MarginBounds.Width, e.MarginBounds.Height-(y - e.MarginBounds.Top));
			}
			else
			{
				pageRect = new RectangleF(e.MarginBounds.Left, e.MarginBounds.Top, e.MarginBounds.Width, e.MarginBounds.Height-10);
				y = e.MarginBounds.Top;

			}

			string text = "sida " + currentPageNr.ToString() + " av " + nrofPages.ToString();
			SizeF size = e.Graphics.MeasureString(text, fontPageNr, e.MarginBounds.Width);
			e.Graphics.DrawString(text, fontPageNr, System.Drawing.Brushes.Black, e.MarginBounds.Left + (e.MarginBounds.Width / 2) - (size.Width / 2), e.MarginBounds.Bottom + size.Height);
			//Make room for the footer (number of pages) on the page
			pageRect.Height -= size.Height + 5;

			if (contentString.Length > 0)
			{
				
				int charsFitted, linesFilled;
				SizeF contentSize = e.Graphics.MeasureString(contentString, fontContent, pageRect.Size, stringFormat, out charsFitted, out linesFilled);
			
				SizeF spaceLeft = pageRect.Size - contentSize;
			
				
				string stringToPrint = contentString.Substring(0, charsFitted);
				e.Graphics.DrawString(stringToPrint, fontContent, System.Drawing.Brushes.Black, pageRect, stringFormat);
			
				if (charsFitted < contentString.Length)
				{
					e.HasMorePages = true;
					contentString = contentString.Substring(charsFitted);
				}
				else
				{
				
					if (spaceLeft.Height < MeasureFooter(e))
					{
						e.HasMorePages = true;
					}
					//The footer fits on this page
					y += contentSize.Height + 10;
					DrawFooter(e);
			
					e.HasMorePages = false;
				
				}
			}
			else
			{
				//This is the last page and only the footer is printed on it
				y = e.MarginBounds.Top;
				DrawFooter(e);
			}



		}

		private void DrawHeader(System.Drawing.Printing.PrintPageEventArgs e)
		{
			int marginLeft = e.MarginBounds.Left;
			int marginRight = e.MarginBounds.Right;
			int marginTop = e.MarginBounds.Top;
			System.Drawing.Brush brush = System.Drawing.Brushes.Black;
			SizeF stringSize = new SizeF();

			y = marginTop;
			System.Drawing.Font font = new Font("Arial", 12);
			string text = "Utskriftsdatum: " + System.DateTime.Now.ToShortDateString();
			stringSize = e.Graphics.MeasureString(text, font);
			e.Graphics.DrawString(text, font, brush, marginRight - stringSize.Width, y);

			font = new Font("Arial", 16, System.Drawing.FontStyle.Bold);
			text = "Besök som ska registreras i Primula";
			stringSize = e.Graphics.MeasureString(text, font);
			
			e.Graphics.DrawString(text, font, brush, marginLeft, y);

			y += stringSize.Height + 5;
			e.Graphics.DrawLine(System.Drawing.Pens.Black, e.MarginBounds.Left, y, e.MarginBounds.Right, y);
		}


		
		private float MeasureFooter(System.Drawing.Printing.PrintPageEventArgs e)
		{
			float height = 0;
			System.Drawing.Font font = new Font("Arial", 12, FontStyle.Bold);
			string text = contentFooter;
			
			SizeF stringSize = e.Graphics.MeasureString(text, font);

			height += stringSize.Height+5;

			return height;
		}


		private void DrawFooter(System.Drawing.Printing.PrintPageEventArgs e)
		{
			
			//TODO: Write the total number of visits and frikort/patienten betalar själv
			System.Drawing.Brush brush = System.Drawing.Brushes.Black;
			e.Graphics.DrawLine(System.Drawing.Pens.Black, e.MarginBounds.Left, y, e.MarginBounds.Right, y);
			System.Drawing.Font font = new Font("Arial", 12, FontStyle.Bold);
			string text = contentFooter;
			
			SizeF stringSize = e.Graphics.MeasureString(text, font);
			e.Graphics.DrawString(text, font, brush, e.MarginBounds.Left, y);
			
		}


	}
}
