using System;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace RehabLight
{
	/// <summary>
	/// Summary description for Utils.
	/// </summary>
	public class Utils
	{
		// Copy the display-related properties of the given DataGrid
		// to the given DataGridTableStyle
		public static void CopyDefaultTableStyle(System.Windows.Forms.DataGrid dg, System.Windows.Forms.DataGridTableStyle ts)
		{
			ts.AllowSorting = dg.AllowSorting;
			ts.AlternatingBackColor = dg.AlternatingBackColor;
			ts.BackColor = dg.BackColor;
			ts.ColumnHeadersVisible = dg.ColumnHeadersVisible;
			ts.ForeColor = dg.ForeColor;
			ts.GridLineColor = dg.GridLineColor;
			ts.GridLineStyle = dg.GridLineStyle;
			ts.HeaderBackColor = dg.HeaderBackColor;
			ts.HeaderFont = dg.HeaderFont;
			ts.HeaderForeColor = dg.HeaderForeColor;
			ts.LinkColor = dg.LinkColor;
			ts.PreferredColumnWidth = dg.PreferredColumnWidth;
			ts.PreferredRowHeight = dg.PreferredRowHeight;
			ts.ReadOnly = dg.ReadOnly;
			ts.RowHeadersVisible = dg.RowHeadersVisible;
			ts.RowHeaderWidth = dg.RowHeaderWidth;
			ts.SelectionBackColor = dg.SelectionBackColor;
			ts.SelectionForeColor = dg.SelectionForeColor;
			ts.HeaderFont = dg.HeaderFont;
			
		}

		public static int GetNextUniqueId(System.Data.DataTable table) 
		{
			System.Collections.IEnumerator rowIterator = table.Rows.GetEnumerator();
			string idColumnName = "id";
			switch (table.TableName)
			{
				case "Patients":
					idColumnName = "patientid";
					break;
				case "Notes":
					idColumnName = "noteid";
					break;
				case "Diagnosis":
					idColumnName = "diagnosisid";
					break;
				default:
					Debug.WriteLine("Table not recognized, seroius error");
					break;
			}

			System.Data.DataRow row = null;
			ArrayList ids = new ArrayList();
			while (rowIterator.MoveNext())
			{
				//Iterate until the last row
				row = (System.Data.DataRow) rowIterator.Current;
				int id;
				if (!row.IsNull(idColumnName)) 
				{
					id = (int) row[idColumnName];
					ids.Add(id);
				}
			}
			
			int lastid = 0;
			
			if (row != null)
			{
				if (!row.IsNull(idColumnName)) 
				{
					lastid = (int)row[idColumnName];
				}
			}
			//Check that the lastid is the greatest of all ids, otherwise set lastid to the greatest
			foreach(object o in ids.ToArray())
			{
				int currid = (int)o;
				if (currid > lastid)
				{
					lastid = currid;
					Debug.WriteLine("OBS! lastid was not greatest: lastid=" + lastid.ToString() + " currid=" + currid.ToString());
				}
			}
			
			int result = ++lastid;
			Debug.WriteLine("Next id is: " + result.ToString());
			return result;
		}

		public class Month 
		{
			private int monthNumber;
			private string monthName;
			public int MonthNumber
			{
				get{return monthNumber;}
			}
			public string MonthName
			{
				get{return monthName;}
			}

			public override string ToString()
			{
				return monthName;
			}

			public Month(int number, string name)
			{
				monthNumber = number;
				monthName = name;
			}
		}

		public static bool[] FindPresentMonths(System.Data.DataView aDv, int year)
		{
			System.Data.DataView dv = aDv;
			
			bool[] notesinMonth = new bool[12];

			for (int month = 1; month <= 12; month++)
			{	
				dv.RowFilter = "SUBSTRING(visitdatetime,1,4)=" + year.ToString() + " AND SUBSTRING(visitdatetime,6,2)=" + (month).ToString();
				if (dv.Count > 0)
					notesinMonth[month-1] = true;
				else
					notesinMonth[month-1] = false;
			}
			return notesinMonth;
		}

		public static ArrayList FindPresentYears(System.Data.DataView aDv)
		{
			System.Data.DataView dv = aDv;
			ArrayList years = new ArrayList();

			for (int year = 2006; year <= 2012; year++)
			{	
				dv.RowFilter = "SUBSTRING(visitdatetime,1,4)=" + year.ToString();
				if (dv.Count > 0)
					years.Add(year);
			}

			return years;
		}

		public static System.Data.DataSet SelectedRowsDataSet(System.Data.DataView dv, ArrayList selectedRows)
		{
			System.Data.DataSet ds = new System.Data.DataSet();
			System.Data.DataTable dt = new System.Data.DataTable();
			System.Data.DataRowView[] selectedRowsView = new System.Data.DataRowView[selectedRows.Count];

			foreach(System.Data.DataColumn col in dv.Table.Columns) 
			{ 
				dt.Columns.Add(col.ColumnName, col.DataType); 
			} 
			
			for (int i = 0; i < selectedRows.Count; i++)
			{
				dt.ImportRow(dv[(int)(selectedRows.ToArray())[i]].Row);
			}

			ds.Tables.Add(dt);

			return(ds);
		}

		/*public static bool[] FindPresentYears(System.Data.DataTable aTable)
		{
			DataView dv = new DataView();
			dv.Table = aTable;
			ArrayList presentYears = new ArrayList();

			return presentYears
		}*/

		public struct PatientNote
		{
			public PatientNote(Patient aPatient, Note aNote, Charge aCharge)
			{
				patient = aPatient;
				note = aNote;
				charge = aCharge;
			}

			public Patient patient;
			public Note note;
			public Charge charge;
		}

		public static Utils.PatientNote[] CreatePatientNotesArray(Database database, System.Data.DataView dv)
		{
			ArrayList patientNotes = new ArrayList();

			foreach (System.Data.DataRowView drv in dv)
			{
				Note tmpNote = database.CreateNote(drv.Row);
				Charge tmpCharge = database.CreateCharge(drv.Row);
				Patient tmpPatient = database.CreatePatient(drv.Row, null);
				Utils.PatientNote patientNote = new Utils.PatientNote(tmpPatient, tmpNote, tmpCharge);
				patientNotes.Add(patientNote);
			}
				
			Utils.PatientNote[] patientNotesArray;
			patientNotesArray = (Utils.PatientNote[]) patientNotes.ToArray((new Utils.PatientNote()).GetType());

			return patientNotesArray;
		}

		public static String CleanInput(string strIn)
		{
			// Replace invalid characters with empty strings. Allow . and -
			return Regex.Replace(strIn, @"[^\w\.-]", ""); 
		}


		public class MyDataGridTextBoxColumn : System.Windows.Forms.DataGridTextBoxColumn
		{
			public MyDataGridTextBoxColumn() : base()
			{
				this.HideEditBox();
			}

			protected override void Edit(System.Windows.Forms.CurrencyManager source, int rowNum, System.Drawing.Rectangle bounds, bool readOnly, string instantText, bool cellIsVisible)
			{

			}

			protected override void Edit(System.Windows.Forms.CurrencyManager source, int rowNum, System.Drawing.Rectangle bounds, bool readOnly, string instantText)
			{
				
			}

			protected override void Edit(System.Windows.Forms.CurrencyManager source, int rowNum, System.Drawing.Rectangle bounds, bool readOnly)
			{
				
			}
		}

		public class MyColorDataGridTextBoxColumn : System.Windows.Forms.DataGridTextBoxColumn
		{
			private int column;

			public MyColorDataGridTextBoxColumn()
			{
				column = -2;
			}

			protected override void Paint(System.Drawing.Graphics g, System.Drawing.Rectangle bounds, System.Windows.Forms.CurrencyManager source, int rowNum, System.Drawing.Brush backBrush, System.Drawing.Brush foreBrush, bool alignToRight)
			{
				// the idea is to conditionally set the foreBrush and/or backbrush
				// depending upon some crireria on the cell value
				// Here, we color anything that begins with a letter higher than 'F'
				try
				{
				
					
					System.Windows.Forms.DataGrid grid = this.DataGridTableStyle.DataGrid;
					System.Data.DataRowView drv = (System.Data.DataRowView) source.List[rowNum];

					//first time set the column properly
					if (column == -2)
					{
						int i = this.DataGridTableStyle.GridColumnStyles.IndexOf(this);
						if (i > -1)
							column = i;
					}
					

					//if(grid.CurrentRowIndex == rowNum && grid.CurrentCell.ColumnNumber == column)
					if ((string)drv["chargeprimula"] == "J")
					{
						
						if (!grid.IsSelected(rowNum))
						{
							//backBrush = new System.Drawing.Drawing2D.LinearGradientBrush(bounds, System.Drawing.Color.FromArgb(205,149,12), System.Drawing.Color.FromArgb(238,220,130), System.Drawing.Drawing2D.LinearGradientMode.Vertical);
							backBrush = System.Drawing.Brushes.Wheat;
							foreBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
						}
					}
				}
				catch(Exception ex){ /* empty catch */ }
				finally
				{
					// make sure the base class gets called to do the drawing with
					// the possibly changed brushes
					base.Paint(g, bounds, source, rowNum, backBrush, foreBrush, alignToRight);
				}

			} 
 
			protected override void Edit(System.Windows.Forms.CurrencyManager source, int rowNum, System.Drawing.Rectangle bounds, bool readOnly, string instantText, bool cellIsVisible)
			{

			}

			protected override void Edit(System.Windows.Forms.CurrencyManager source, int rowNum, System.Drawing.Rectangle bounds, bool readOnly, string instantText)
			{
				
			}

			protected override void Edit(System.Windows.Forms.CurrencyManager source, int rowNum, System.Drawing.Rectangle bounds, bool readOnly)
			{
				
			}
		}



		/*public class MyDataGrid : System.Windows.Forms.DataGrid
		{
			public MyDataGrid() : base()
			{
			
			}

			protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e)
			{
				System.Drawing.Point pt = new System.Drawing.Point(e.X, e.Y); 
 
				System.Windows.Forms.DataGrid.HitTestInfo hti = this.HitTest(pt); 
 
				if(hti.Type == System.Windows.Forms.DataGrid.HitTestType.Cell) 
				{ 
					base.OnMouseUp (e);					
					
				}
			}

			protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
			{
				System.Drawing.Point pt = new System.Drawing.Point(e.X, e.Y); 
 
				System.Windows.Forms.DataGrid.HitTestInfo hti = this.HitTest(pt); 
 
				if(hti.Type == System.Windows.Forms.DataGrid.HitTestType.ColumnResize) 
				{ 
					return;					
				}
				base.OnMouseMove(e);
				
			}


			protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e)
			{
				System.Drawing.Point pt = new System.Drawing.Point(e.X, e.Y); 
 
				System.Windows.Forms.DataGrid.HitTestInfo hti = this.HitTest(pt); 
 
				if (hti.Type == System.Windows.Forms.DataGrid.HitTestType.Cell)
				{
					base.OnMouseDown (e);

				}
				
			}			

		}*/
		/// <summary>
		/// 
		/// </summary>
		/// <param name="aPersonNumber">A personnumber, 12 numbers and a -</param>
		/// <returns></returns>
		public static void CheckPersonNumber(string aPersonnumber)
		{
			string shortPersonnumber;
			
			if (aPersonnumber.Length == 11)
			{
				//Just remove the - sign
				shortPersonnumber = aPersonnumber.Remove(8,1);
			}
			else if (aPersonnumber.Length == 13)
			{
				//Remove the - sign and the "sekel"
				shortPersonnumber = (aPersonnumber.Remove(8,1)).Remove(0,2);
			}
			else
				throw new Exception("Personnumret har ett felaktigt format.");
			
			//Now the personnumber should have 9 numbers
			Debug.WriteLine("Personnummer att testa: " + shortPersonnumber);

			//Check that the date is a valid date
			//Here we assume that the "sekel" is either 1900 or 2000
			try
			{
				string date = "19" + shortPersonnumber.Substring(0,2) + "-" + shortPersonnumber.Substring(2,2) + "-" + shortPersonnumber.Substring(4,2);
				System.DateTime dt = System.DateTime.Parse(date);
			}
			catch (FormatException exception1)
			{
				//The date was not correct, try with the 2100 century
				try
				{
					string date = "19" + shortPersonnumber.Substring(0,2) + "-" + shortPersonnumber.Substring(2,2) + "-" + shortPersonnumber.Substring(4,2);
					System.DateTime dt = System.DateTime.Parse(date);
				}
				catch (FormatException exception2)
				{
					//The 2100 century was not correct either
					throw new Exception("Personnumret innehåller ett datum som ej är giltigt.");
				}
			}

			
			int checkNumber = int.Parse(shortPersonnumber[9].ToString());

			string personnumberNoCheck = shortPersonnumber.Substring(0, 9);
			Debug.WriteLine("Personnummer utan kontrollsiffra: " + personnumberNoCheck);

			int[] result = new int[personnumberNoCheck.Length];
			for (int i = 0; i < personnumberNoCheck.Length; i++)
			{
				try
				{
					result[i] = int.Parse(personnumberNoCheck[i].ToString());
					if ((i % 2) == 0)
						result[i] *= 2;
				}
				catch (Exception e)
				{
					throw new Exception("Kunde ej parsa personnumret. \n" + e.Message);
				}
			}

			/*int[] result = numbers;
			for (int i = 0; i < result.Length; i+=2)
			{
				result[i] *=2;
			}*/

			int sum = 0;
			for (int i = 0; i < result.Length; i++)
			{
				int l = (int)(result[i] / 10);
				int r = result[i] % 10;

				int numberSum = l + r;
				sum += numberSum;
			}
			Debug.WriteLine("Siffersumma: " + sum.ToString());

			int lastNumber = sum % 10;
			//Do % 10 because if the lastNumber equals 0 then checksum is 0
			int calculatedCheckNumber = (10 - lastNumber) % 10;

			Debug.WriteLine("Checksiffra: " + calculatedCheckNumber.ToString());

			if (calculatedCheckNumber != checkNumber)
			{
				throw new Exception("Personnumrets kontrollsiffra stämmer ej.");
			}

		}

		public static void AutoSizeDataGrid(string[] flexibleColumns, System.Windows.Forms.DataGrid dataGrid, System.Windows.Forms.BindingContext bindingContext, int tableStyleIndex)
		{
			//First of all, the datagrid needs to have a TableStyle
			if (dataGrid.TableStyles.Count == 0)
				return;

			System.Windows.Forms.DataGridTableStyle dgStyle = dataGrid.TableStyles[tableStyleIndex];

			System.Windows.Forms.CurrencyManager cm = (System.Windows.Forms.CurrencyManager)bindingContext[dataGrid.DataSource,dataGrid.DataMember];
			int numRows = cm.List.Count;

			bool flexibleColumn = false;
			int occupiedWidth = 0;

			//set to zero so horizontal scroll does not show as your size
			//frees up room in case leading cols grid during resize and try to flash the HScroll
			dgStyle.GridColumnStyles[dgStyle.GridColumnStyles.Count - 1].Width = 0;

			int rowHeaderWidth = 0;
			try
			{
				rowHeaderWidth = dataGrid.GetCellBounds(0, 0).X; //The 4 handles the border
			}
			catch (Exception exception) { /*Empty catch*/ }
			int dataGridWidth = dataGrid.Size.Width - rowHeaderWidth - 4;

			if(IsScrollBarVisible(dataGrid))
				dataGridWidth -= System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;

			if (flexibleColumns != null)
			{
				//Do autosizing of the non-flexible columns, i.e. the columns that should have all width enough to
				//display all their content.
				foreach (System.Windows.Forms.DataGridColumnStyle columnStyle in dgStyle.GridColumnStyles)
				{
					//Is the current column style in the list of flexible column styles?
					foreach (string s in flexibleColumns)
					{
						if (columnStyle.MappingName == s)
							flexibleColumn = true;
					}
					//If so, do not autosize this column. 
					//Otherwise, autosize it and increase the width that is occupied by the non-flexible columns
					if (!flexibleColumn)
					{
						occupiedWidth += AutoSizeCol(dataGrid, columnStyle, numRows);
					}

					flexibleColumn = false;
				}
				//Hopefully, some space remains for the flexible columns otherwise the datagrid should be made larger.
				//The flexible columns should share the remaining space.
				int remainingWidth =  dataGridWidth - occupiedWidth;

				if (remainingWidth <= 0)
					return;

				//The flexible columns get equal size
				int colWidth = remainingWidth / flexibleColumns.Length;

				//Set the size of the flexible columns
				for (int i = 0; i < flexibleColumns.Length - 1; i++)
				{
					try
					{
						dgStyle.GridColumnStyles[flexibleColumns[i]].Width = colWidth;
					}
					catch (Exception exception) {} //Empty catch
				}

				string lastColumn = flexibleColumns[flexibleColumns.Length - 1];
				int lastColumnWidth = remainingWidth - (flexibleColumns.Length - 1) * colWidth;
				dgStyle.GridColumnStyles[lastColumn].Width = lastColumnWidth;// -5;
			}
			else
			{
				//Divide the datagrid width equally among the columns
				int colWidth = dataGridWidth / dgStyle.GridColumnStyles.Count;
				int remainingWidth = dataGridWidth;

				for (int i = 0; i < dgStyle.GridColumnStyles.Count - 1; i++)
				{
					dgStyle.GridColumnStyles[i].Width = (int) colWidth; // 8 is for leading and trailing padding 
					

				}

				//make last col fit whatever is left (handles pixels lost to int division
				int lastColumn = dgStyle.GridColumnStyles.Count - 1;
				int lastColumnWidth = dataGridWidth - (lastColumn * colWidth);
				dgStyle.GridColumnStyles[lastColumn].Width = lastColumnWidth;
								
			}
		}

		public static int AutoSizeCol(System.Windows.Forms.DataGrid dataGrid, System.Windows.Forms.DataGridColumnStyle columnStyle, int aNumRows) //Does it work with column name?
		{ 
			float width = 0; 
			int numRows = aNumRows;
			System.Windows.Forms.DataGridTableStyle dgStyle = columnStyle.DataGridTableStyle;									  
			int col = dgStyle.GridColumnStyles.IndexOf(columnStyle);

			System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(dataGrid.Handle); 
			System.Drawing.StringFormat sf = new System.Drawing.StringFormat(System.Drawing.StringFormat.GenericTypographic);  
			System.Drawing.SizeF size = new System.Drawing.SizeF(0,0); 

			//Calculate the largest width in the column and set the column width to that value
			for(int i = 0; i < numRows; ++ i) 
			{ 
				try
				{
					size = g.MeasureString(dataGrid[i, col].ToString(), dataGrid.Font, 500, sf); 
				}
				catch
				{
					Debug.WriteLine("AutoSizeCol: Exception thrown");
				}
 
				if(size.Width > width) 
					width = size.Width; 
			} 

			//Measure the header text as well
			size = g.MeasureString(columnStyle.HeaderText, dataGrid.HeaderFont, 500, sf); 
 
			if(size.Width > width) 
				width = size.Width; 

			g.Dispose(); 
			columnStyle.Width = (int) width + 12; // 8 is for leading and trailing padding 

			return columnStyle.Width;
		} 

		private static bool IsScrollBarVisible(System.Windows.Forms.Control aControl)
		{
			foreach(System.Windows.Forms.Control c in aControl.Controls)
			{
				if (c.GetType().Equals(typeof(System.Windows.Forms.VScrollBar)))
				{
					return c.Visible;
				}
			}
			return false;
		}





	}
}
