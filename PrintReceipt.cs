using System;
using System.Drawing;
using System.Text;

namespace RehabLight
{
	/// <summary>
	/// Summary description for PrintReceipt.
	/// </summary>
	public class PrintReceipt
	{
		private System.Drawing.Printing.PrintDocument printDocument;

		public System.Drawing.Printing.PrintDocument PrintDocument
		{
			get{return printDocument;}
		}
		
		float y;
		bool firstPage;
		string contentString;
		private int currentPageNr;
		private int nrofPages;

		private System.Drawing.Font fontPageNr;
		private System.Drawing.Font fontContent;
		private StringFormat stringFormat;

		private string name;
		private string personnumber;

		private string logoFilename;

		public PrintReceipt(string aLogoFilename)
		{
			printDocument = new System.Drawing.Printing.PrintDocument();
			printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDocument_PrintPage);
			printDocument.BeginPrint +=new System.Drawing.Printing.PrintEventHandler(printDocument_BeginPrint);
			
			logoFilename = aLogoFilename;
		}

		
	

		private void printDocument_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{
			firstPage = true;
			y = 0;
			nrofPages = 0;
			currentPageNr = 0;

			fontPageNr = new Font("Times New Roman", 8);
			fontContent = new Font("Times New Roman", 12);
			stringFormat = new StringFormat();
			stringFormat.Trimming = StringTrimming.Word;
			stringFormat.SetTabStops(0.0f, new float[] {200.0f});
		}


		public void SetContent(Utils.PatientNote[] patientNotesArray)
		{
			Patient patient = patientNotesArray[0].patient;
	
			name = patient.Firstname + " " + patient.Surname;
			personnumber = patient.Personnumber;

			StringBuilder s = new StringBuilder();

			foreach(Utils.PatientNote patientNote in patientNotesArray)
			{
				Charge tmpCharge = patientNote.charge;
				Note tmpNote = patientNote.note;

				s.Append("\n\n" + tmpNote.VisitDateTime.ToShortDateString());
				s.Append("\t" + tmpNote.PatientFee + " kr");
				s.Append("\t" + tmpCharge.Description);
			}

			contentString = s.ToString();

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
				DrawLogo(e);
				firstPage = false;

				SizeF totalContentSize = e.Graphics.MeasureString(contentString, fontContent, e.MarginBounds.Width, stringFormat);
				nrofPages = (int) Math.Ceiling((totalContentSize.Height + (y - e.MarginBounds.Top) + MeasureFooter(e))/e.MarginBounds.Height);
		
				pageRect = new RectangleF(e.MarginBounds.Left, y, e.MarginBounds.Width, e.MarginBounds.Height-(y - e.MarginBounds.Top));
			}
			else
			{
				pageRect = new RectangleF(e.MarginBounds.Left, e.MarginBounds.Top, e.MarginBounds.Width, e.MarginBounds.Height);
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
					y += contentSize.Height + 20;
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

		private void DrawLogo(System.Drawing.Printing.PrintPageEventArgs e)
		{
			int marginLeft = e.MarginBounds.Left;
			int marginRight = e.MarginBounds.Right;
			int marginTop = e.MarginBounds.Top;
			

			try 
			{
				System.Drawing.Image image = System.Drawing.Image.FromFile(logoFilename);

				int imageWidth = image.Width;
				int imageHeight = image.Height;

				e.Graphics.DrawImage(image, marginRight-imageWidth, marginTop);
			}
			catch (Exception exception)
			{
			
				//TODO: Improve the errormanagement here
				//Could not find the logofile or something wrong with the picture
			}

		}

		private void DrawHeader(System.Drawing.Printing.PrintPageEventArgs e)
		{
			int marginLeft = e.MarginBounds.Left;
			int marginRight = e.MarginBounds.Right;
			int marginTop = e.MarginBounds.Top;
			System.Drawing.Brush brush = System.Drawing.Brushes.Black;
			SizeF stringSize = new SizeF();
			
			System.Drawing.Font font = new Font("Arial", 18, System.Drawing.FontStyle.Bold);
			string text = "Kvitto";
			stringSize = e.Graphics.MeasureString(text, font);
			y = marginTop;
			e.Graphics.DrawString(text, font, brush, marginLeft, y);

			font = new Font("Arial", 12);
			text = "Vård med ersättning från landstinget";
			stringSize = e.Graphics.MeasureString(text, font);
			y += stringSize.Height+10;
			e.Graphics.DrawString(text, font, brush, marginLeft, y);

			font = new Font("Arial", 12);
			text = "Vårdgivare:";
			stringSize = e.Graphics.MeasureString(text, font);
			y += stringSize.Height+10;
			e.Graphics.DrawString(text, font, brush, marginLeft, y);


			font = new Font("Arial", 14);
			text = "Leg. sjukgymnast\nDoris Ruberg\nRepslagaregatan 12\n602 32 Norrköping\nTelefon: 070-6617957";
			y += stringSize.Height+15;
			stringSize = e.Graphics.MeasureString(text, font);
			e.Graphics.DrawString(text, font, brush, marginLeft, y);

			y += stringSize.Height+10;
			e.Graphics.DrawLine(new System.Drawing.Pen(brush, 1), marginLeft, y, marginRight, y);

			font = new Font("Arial", 12);
			text = "Patient:";
			stringSize = e.Graphics.MeasureString(text, font);
			y += 10;
			e.Graphics.DrawString(text, font, brush, marginLeft, y);

			font = new Font("Arial", 14);
			text = "Personnummer: " + personnumber + "\nNamn: " + name;
			y += stringSize.Height+15;
			stringSize = e.Graphics.MeasureString(text, font);
			e.Graphics.DrawString(text, font, brush, marginLeft, y);
			

			font = new Font("Arial", 14, System.Drawing.FontStyle.Bold);
			text = "Besöksdatum\tPatientavgift\tKommentar\n\n";
			y += stringSize.Height+20;
			stringSize = e.Graphics.MeasureString(text, font);
			StringFormat stringFormat = new StringFormat();
			stringFormat.SetTabStops(0.0f, new float[] {200.0f});
			e.Graphics.DrawString(text, font, brush, marginLeft, y, stringFormat);

		}

		private float MeasureFooter(System.Drawing.Printing.PrintPageEventArgs e)
		{
			float height = 0;
			System.Drawing.Font font = new Font("Arial", 12);
			string text = "Patientavgiften kvitteras";
			height += 30;
			SizeF stringSize = e.Graphics.MeasureString(text, font);

			height += stringSize.Height+5;
			font = new Font("Arial", 10, System.Drawing.FontStyle.Bold);
			text = "Datum\t\t\t\tSign";
			height += 5;

            stringSize = e.Graphics.MeasureString(text, font);
            height += stringSize.Height + (e.MarginBounds.Width / 12);
            font = new Font("Arial", 10);
            text = "Företaget innehar F-skattsedel. Organisationsnummer: 556471-2221.";
            stringSize = e.Graphics.MeasureString(text, font);
            height += stringSize.Height + 5;

			return height;
		}


		private void DrawFooter(System.Drawing.Printing.PrintPageEventArgs e)
		{
			System.Drawing.Brush brush = System.Drawing.Brushes.Black;
			System.Drawing.Font font = new Font("Arial", 12);
			string text = "Patientbesöken kvitteras";
			y += 30;
			SizeF stringSize = e.Graphics.MeasureString(text, font);
			e.Graphics.DrawString(text, font, brush, e.MarginBounds.Left, y);

			y += stringSize.Height+5;
			e.Graphics.DrawRectangle(new System.Drawing.Pen(brush, (float)0.5), e.MarginBounds.Left, y, e.MarginBounds.Width/(float)1.5, e.MarginBounds.Width/(float)12);
			font = new Font("Arial", 10, System.Drawing.FontStyle.Bold);
			text = "Datum\t\t\t\tSign";
			y += 5;
			e.Graphics.DrawString(text, font, brush, e.MarginBounds.Left+2, y);

            stringSize = e.Graphics.MeasureString(text, font); 
            y+= stringSize.Height + (e.MarginBounds.Width / 12);
            font = new Font("Arial", 10);
            text = "Företaget innehar F-skattsedel. Organisationsnummer: 556471-2221.";
            e.Graphics.DrawString(text, font, brush, e.MarginBounds.Left, y);
		}


	}
}
