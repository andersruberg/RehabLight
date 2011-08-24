using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace RehabLight
{
	/// <summary>
	/// Summary description for PrintReceiptDlg.
	/// </summary>
	public class PrintReceiptDlg : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label2;
		private ArrayList months;
		private System.Windows.Forms.ComboBox cmbPickMonth;
		//private System.Windows.Forms.DataGrid dgReceipt;
		//private Utils.MyDataGrid dgReceipt;
		
		private System.Windows.Forms.Button btnPrintPreview;
		private System.Windows.Forms.Button btnPrint;
		private System.Windows.Forms.Button btnCancel;
		private System.Data.DataSet dsPatientNotes;
		private int year;
		private ArrayList selectedRows;
		private PrintPreviewDialog printPreviewDialog;
		private PrintDialog printDialog;
		private PrintReceipt printReceipt;
		private Patient patient;
		private Database database;
		private System.Windows.Forms.Label lblPatient;
		private System.Windows.Forms.Label label1;
		private CustomDataGrid.MyDataGrid dgReceipt;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cmbPickYear;
		private System.Windows.Forms.Button btnShowAll;
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PrintReceiptDlg(Database aDatabase, System.Data.DataSet aDsPatientNotes, Patient aPatient)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();


			database = aDatabase;
			dsPatientNotes = aDsPatientNotes;
			patient = aPatient;

			months = new ArrayList();
			year = System.DateTime.Now.Year;
			
			
			
			SetupPrinting(Settings.ReceiptLogoFilename);
			

			selectedRows = new ArrayList();

			lblPatient.Text = lblPatient.Text + " " + patient.Surname + " " + patient.Firstname + ", " + patient.Personnumber;

			dgReceipt.SetDataBinding(database.DsMaster, "Joined");
			SetupDataGridReceipt();

			SetupComboBoxPickYear();
			cmbPickMonth.SelectedIndexChanged +=new EventHandler(cmbPickMonth_SelectedIndexChanged);
			SetupComboBoxPickMonth();

			/*System.Windows.Forms.CurrencyManager cm = (CurrencyManager)BindingContext[dgReceipt.DataSource,dgReceipt.DataMember];
			System.Data.DataView dv = (System.Data.DataView) cm.List;
			
			int selectedMonth = System.DateTime.Now.Month;
			dv.RowFilter = "patientid=" + patient.Id + " AND SUBSTRING(visitdatetime,1,4)=" + year.ToString() + " AND SUBSTRING(visitdatetime,6,2)=" + selectedMonth;
			dv.Sort = "visitdatetime DESC";
			dgReceipt.CaptionText = "Besök under " + System.DateTime.Now.ToString("MMMM");
			*/
		}

		private void SetupComboBoxPickMonth()
		{
			System.Data.DataView dv = new System.Data.DataView(dsPatientNotes.Tables[0]);
			bool[] notesinMonth = Utils.FindPresentMonths(dv, year);

			months.Clear();
			cmbPickMonth.Items.Clear();
			
			for(int month = 1; month <= 12; month++)
			{
				if (notesinMonth[month-1])
				{
					string monthName = (new System.DateTime(year, month, 1)).ToString("MMMM");
					cmbPickMonth.Items.Add(new Utils.Month(month, monthName));
					
				}
			}

			if (cmbPickMonth.Items.Count > 0)
			{
				cmbPickMonth.ValueMember = "MonthNumber";
				cmbPickMonth.DisplayMember = "MonthName";	
				cmbPickMonth.SelectedIndex = 0;
				
			}
		}

		private void SetupComboBoxPickYear()
		{
			System.Data.DataView dv = new System.Data.DataView(dsPatientNotes.Tables[0]);
			
			ArrayList years = Utils.FindPresentYears(dv);
			
			foreach(object o in years)
			{
				int aYear = (int)o;
				cmbPickYear.Items.Add(aYear);
				cmbPickYear.SelectedIndex++;
				//Set the year to the last one present
				year = aYear;
			}
			
		}



		private void SetupDataGridReceipt()
		{
			// 
			// dgReceipt
			// 
			/*this.dgReceipt = new CustomDataGrid.MyDataGrid();
			this.dgReceipt.CaptionFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dgReceipt.DataMember = "";
			this.dgReceipt.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dgReceipt.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgReceipt.Location = new System.Drawing.Point(8, 136);
			this.dgReceipt.Name = "dgReceipt";
			this.dgReceipt.ParentRowsVisible = false;
			this.dgReceipt.RowHeadersVisible = false;
			this.dgReceipt.Size = new System.Drawing.Size(696, 192);
			this.dgReceipt.TabIndex = 7;
			//this.dgReceipt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgReceiptMouseDown);
			this.dgReceipt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgReceipt_MouseUp);*/

	
			this.dgReceipt.ParentRowsVisible = false;
			this.dgReceipt.RowHeadersVisible = false;
			this.dgReceipt.ReadOnly = true;

			System.Windows.Forms.DataGridTableStyle ts = new DataGridTableStyle();
			ts.MappingName = "Joined";

			dgReceipt.RowHeadersVisible = false;
			dgReceipt.ReadOnly = true;

			Utils.CopyDefaultTableStyle(dgReceipt, ts);						

			System.Windows.Forms.DataGridTextBoxColumn dgtbcDateTime = new Utils.MyDataGridTextBoxColumn();
			dgtbcDateTime.Width = 120;
			dgtbcDateTime.MappingName = "visitdatetime";
			dgtbcDateTime.HeaderText = "Datum och tid";
			dgtbcDateTime.ReadOnly = true;

			System.Windows.Forms.DataGridTextBoxColumn dgtbcNote = new Utils.MyDataGridTextBoxColumn();
			dgtbcNote.Width = 300;
			dgtbcNote.MappingName = "note";
			dgtbcNote.HeaderText = "Anteckning";
			dgtbcNote.ReadOnly = true;

			System.Windows.Forms.DataGridTextBoxColumn dgtbcPatientCharge = new Utils.MyDataGridTextBoxColumn();
			dgtbcPatientCharge.Width = 100;
			dgtbcPatientCharge.MappingName = "chargetext";
			dgtbcPatientCharge.HeaderText = "Betalning";
			dgtbcPatientCharge.ReadOnly = true;

			System.Windows.Forms.DataGridTextBoxColumn dgtbcPatientFee = new Utils.MyDataGridTextBoxColumn();
			dgtbcPatientFee.Width = 50;
			dgtbcPatientFee.MappingName = "patientfee";
			dgtbcPatientFee.HeaderText = "Avgift";
			dgtbcPatientFee.ReadOnly = true;

			ts.GridColumnStyles.Add(dgtbcDateTime);
			ts.GridColumnStyles.Add(dgtbcNote);
			ts.GridColumnStyles.Add(dgtbcPatientCharge);
			ts.GridColumnStyles.Add(dgtbcPatientFee);
			
			dgReceipt.TableStyles.Add(ts);

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.lblPatient = new System.Windows.Forms.Label();
			this.cmbPickMonth = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnPrintPreview = new System.Windows.Forms.Button();
			this.btnPrint = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.dgReceipt = new CustomDataGrid.MyDataGrid();
			this.label3 = new System.Windows.Forms.Label();
			this.cmbPickYear = new System.Windows.Forms.ComboBox();
			this.btnShowAll = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgReceipt)).BeginInit();
			this.SuspendLayout();
			// 
			// lblPatient
			// 
			this.lblPatient.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblPatient.Location = new System.Drawing.Point(8, 16);
			this.lblPatient.Name = "lblPatient";
			this.lblPatient.Size = new System.Drawing.Size(680, 24);
			this.lblPatient.TabIndex = 0;
			this.lblPatient.Text = "Skriv ut kvitto för ";
			// 
			// cmbPickMonth
			// 
			this.cmbPickMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbPickMonth.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbPickMonth.Location = new System.Drawing.Point(280, 104);
			this.cmbPickMonth.Name = "cmbPickMonth";
			this.cmbPickMonth.Size = new System.Drawing.Size(152, 24);
			this.cmbPickMonth.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(208, 104);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 24);
			this.label2.TabIndex = 2;
			this.label2.Text = "Månad";
			// 
			// btnPrintPreview
			// 
			this.btnPrintPreview.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnPrintPreview.Location = new System.Drawing.Point(16, 344);
			this.btnPrintPreview.Name = "btnPrintPreview";
			this.btnPrintPreview.Size = new System.Drawing.Size(168, 48);
			this.btnPrintPreview.TabIndex = 4;
			this.btnPrintPreview.Text = "Förhandsgranska...";
			this.btnPrintPreview.Click += new System.EventHandler(this.btnPrintPreview_Click);
			// 
			// btnPrint
			// 
			this.btnPrint.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnPrint.Location = new System.Drawing.Point(208, 344);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(160, 48);
			this.btnPrint.TabIndex = 1;
			this.btnPrint.Text = "Skriv ut kvitto...";
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnCancel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnCancel.Location = new System.Drawing.Point(552, 344);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(112, 40);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Avbryt";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(24, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(640, 56);
			this.label1.TabIndex = 8;
			this.label1.Text = @"Markera de besök som ska skrivas ut. Om alla besök ska skrivas ut behöver inga markeringar göras. För att avmarkera ett markerat besök, klicka bara på besöket igen. Välj ""Förhandsgranska"" för att titta på hur kvittot kommer att se ut på pappret. Välj ""Skriv ut kvitto"" för att skriva ut kvittot på papper.";
			// 
			// dgReceipt
			// 
			this.dgReceipt.CaptionFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dgReceipt.DataMember = "";
			this.dgReceipt.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dgReceipt.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgReceipt.Location = new System.Drawing.Point(8, 144);
			this.dgReceipt.Name = "dgReceipt";
			this.dgReceipt.ParentRowsVisible = false;
			this.dgReceipt.ReadOnly = true;
			this.dgReceipt.RowHeadersVisible = false;
			this.dgReceipt.Size = new System.Drawing.Size(688, 184);
			this.dgReceipt.TabIndex = 9;
			this.dgReceipt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgReceipt_MouseUp);
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(24, 104);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(40, 24);
			this.label3.TabIndex = 10;
			this.label3.Text = "År";
			// 
			// cmbPickYear
			// 
			this.cmbPickYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbPickYear.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbPickYear.Location = new System.Drawing.Point(80, 104);
			this.cmbPickYear.Name = "cmbPickYear";
			this.cmbPickYear.Size = new System.Drawing.Size(96, 24);
			this.cmbPickYear.TabIndex = 11;
			// 
			// btnShowAll
			// 
			this.btnShowAll.Location = new System.Drawing.Point(528, 104);
			this.btnShowAll.Name = "btnShowAll";
			this.btnShowAll.Size = new System.Drawing.Size(104, 23);
			this.btnShowAll.TabIndex = 12;
			this.btnShowAll.Text = "VIsa alla besök";
			this.btnShowAll.Click += new System.EventHandler(this.btnShowAll_Click);
			// 
			// PrintReceiptDlg
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(712, 406);
			this.Controls.Add(this.btnShowAll);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cmbPickYear);
			this.Controls.Add(this.dgReceipt);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnPrint);
			this.Controls.Add(this.btnPrintPreview);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmbPickMonth);
			this.Controls.Add(this.lblPatient);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(720, 440);
			this.MinimumSize = new System.Drawing.Size(720, 440);
			this.Name = "PrintReceiptDlg";
			this.Text = "Skriv ut kvitto";
			this.Load += new System.EventHandler(this.PrintReceiptDlg_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgReceipt)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void cmbPickMonth_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (cmbPickMonth.SelectedIndex != -1)
			{
				//TODO:Is it possible to have a class global dataview?
				
				System.Windows.Forms.CurrencyManager cm = (CurrencyManager)BindingContext[dgReceipt.DataSource,dgReceipt.DataMember];
				System.Data.DataView dv = (System.Data.DataView) cm.List;
				Utils.Month selectedMonth = (Utils.Month) cmbPickMonth.SelectedItem;
			
				dv.RowFilter = "patientid=" + patient.Id + " AND SUBSTRING(visitdatetime,1,4)=" + year.ToString() + " AND SUBSTRING(visitdatetime,6,2)=" + selectedMonth.MonthNumber.ToString();
				dv.Sort = "visitdatetime DESC";
				dgReceipt.CaptionText = "Besök under " + selectedMonth.MonthName;

				Utils.AutoSizeDataGrid(null, dgReceipt, this.BindingContext, 0);
			}
		
		}

		/*private void dgReceiptMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			DataGrid dgSender = (DataGrid) sender;
			System.Windows.Forms.DataGrid.HitTestInfo hti;
			hti = dgSender.HitTest(e.X, e.Y);

			switch (hti.Type) 
			{
				case System.Windows.Forms.DataGrid.HitTestType.Cell :
					//dgSender.Select(hti.Row);
					break;
				case System.Windows.Forms.DataGrid.HitTestType.RowHeader :
					//dgSender.Select(hti.Row);
					break;
			}
		
		}*/

		private void dgReceipt_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			System.Drawing.Point pt = new Point(e.X, e.Y); 
			DataGrid dgSender = (DataGrid) sender;
 
			DataGrid.HitTestInfo hti = dgSender.HitTest(pt); 
 
			if(hti.Type == DataGrid.HitTestType.Cell) 
 
			{ 
				dgSender.CurrentCell = new DataGridCell(hti.Row, hti.Column); 
				int c = dgSender.CurrentRowIndex;
				if (selectedRows.Contains(c))
				{
					dgSender.UnSelect(c);
					selectedRows.Remove(c);
				}
				else
				{
					dgSender.Select(hti.Row);
					selectedRows.Add(c);
				}
				for (int i = 0; i < selectedRows.Count; i++)
				{
					dgSender.Select((int)(selectedRows.ToArray())[i]);
				}
			} 
		
		}

		private void btnPrintPreview_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.CurrencyManager cm = (CurrencyManager)BindingContext[dgReceipt.DataSource,dgReceipt.DataMember];
			System.Data.DataView dvJoined = (System.Data.DataView) cm.List;
			System.Data.DataView dv;
			if (selectedRows.Count > 0)
			{
				System.Data.DataSet ds = Utils.SelectedRowsDataSet(dvJoined, selectedRows);
				dv = ds.Tables[0].DefaultView;
			}
			else
				dv = dvJoined;

			dv.Sort = "visitdatetime DESC";

			
			Utils.PatientNote[] patientNotesArray = Utils.CreatePatientNotesArray(database, dv);
			printReceipt.SetContent(patientNotesArray);
			printPreviewDialog.Document = printReceipt.PrintDocument;
			System.Windows.Forms.PrintPreviewControl printPreviewControl = new PrintPreviewControl();
			foreach (Control c in printPreviewDialog.Controls)
			{
				if (c.GetType() == printPreviewControl.GetType())
					printPreviewControl = (System.Windows.Forms.PrintPreviewControl)c;
			}
			if (printPreviewControl != null)
				printPreviewControl.Zoom = 1.0;
			printPreviewDialog.ShowDialog();
		
		}

		private void SetupPrinting(string filename)
		{
			printReceipt = new PrintReceipt(filename);

			printDialog = new System.Windows.Forms.PrintDialog();
			printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
			
			printPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
			printPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			printPreviewDialog.ClientSize = new System.Drawing.Size(1024, 600);
			
			printPreviewDialog.Enabled = true;
			//printPreviewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog.Icon")));
			printPreviewDialog.Location = new System.Drawing.Point(254, 17);
			printPreviewDialog.MinimumSize = new System.Drawing.Size(500, 400);
			printPreviewDialog.Name = "Förhandsgranskning";
			printPreviewDialog.TransparencyKey = System.Drawing.Color.Empty;
			printPreviewDialog.Visible = false;
		}

		private void btnPrint_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.CurrencyManager cm = (CurrencyManager)BindingContext[dgReceipt.DataSource,dgReceipt.DataMember];
			System.Data.DataView dvJoined = (System.Data.DataView) cm.List;
			System.Data.DataView dv;
			if (selectedRows.Count > 0)
			{
				System.Data.DataSet ds = Utils.SelectedRowsDataSet(dvJoined, selectedRows);
				dv = ds.Tables[0].DefaultView;
			}
			else
				dv = dvJoined;

			dv.Sort = "visitdatetime DESC";
			
			Utils.PatientNote[] patientNotesArray = Utils.CreatePatientNotesArray(database, dv);
			printReceipt.SetContent(patientNotesArray);

			printDialog.Document = printReceipt.PrintDocument;
			if (printDialog.ShowDialog() == DialogResult.OK)
				printReceipt.Print();
		}

		private void PrintReceiptDlg_Load(object sender, System.EventArgs e)
		{
			//cmbPickMonth.Text = "Välj månad";
			int currentMonth = System.DateTime.Now.Month;
			cmbPickMonth.SelectedValue = currentMonth;

			cmbPickYear.SelectedIndexChanged +=new EventHandler(cmbPickYear_SelectedIndexChanged);
			
			

			Utils.AutoSizeDataGrid(null, dgReceipt, this.BindingContext, 0);
			
		}

		private void cmbPickYear_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (cmbPickYear.SelectedIndex != -1)
			{
				year = (int)cmbPickYear.SelectedItem;
				SetupComboBoxPickMonth();
			}
		}

		private void btnShowAll_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.CurrencyManager cm = (CurrencyManager)BindingContext[dgReceipt.DataSource,dgReceipt.DataMember];
			System.Data.DataView dv = (System.Data.DataView) cm.List;
			
			int selectedMonth = System.DateTime.Now.Month;
			dv.RowFilter = "patientid=" + patient.Id;
			dv.Sort = "visitdatetime DESC";
			dgReceipt.CaptionText = "Alla besök";

			cmbPickYear.SelectedIndex = -1;
			cmbPickMonth.SelectedIndex = -1;
		
		}

			

		

		

	}
}
