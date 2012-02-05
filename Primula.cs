using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Data;

namespace RehabLight
{
	/// <summary>
	/// Summary description for Primula.
	/// </summary>
	public class Primula : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;
		#region Variables

		private System.Windows.Forms.Button btnPrimulaRemove;
		private System.Windows.Forms.Button btnPrimulaAdd;
		private System.Windows.Forms.Button btnWritePrimula;

		private System.Windows.Forms.TabControl tabControl;
		private CustomDataGrid.MyDataGrid dgToPrimula;
		private CustomDataGrid.MyDataGrid dgInPrimula;
		private System.Windows.Forms.TabPage tabToPrimula;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.TabPage tabInPrimula;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cmbPickMonthToPrimula;
		private System.Windows.Forms.ComboBox cmbPickMonthInPrimula;
		private System.Windows.Forms.Label label2;
		private PrintPreviewDialog printPreviewDialog;
		private System.Windows.Forms.PrintDialog printDialog;
		private PrintPrimula printPrimula;

		private Database database;
		private ArrayList monthsInPrimula;
		private ArrayList monthsToPrimula;
		private int year;
		private ArrayList selectedRows;
		private ArrayList selectedNotes;
		private Note note;
		private int nrofVisitsTotal;
		private int nrofVisitsFreecard;
		private int nrofVisitsPatientPays;
		private int nrofVisitsYouth;

		private System.Windows.Forms.DataGridTableStyle tsPrimula;
		private System.Windows.Forms.CurrencyManager cmInPrimula;
		private System.Windows.Forms.CurrencyManager cmToPrimula;
		private System.Data.DataView dvToPrimula;
		private System.Windows.Forms.ToolTip ttPrimulaRemove;
		private System.Windows.Forms.ToolTip ttWritePrimula;
		private System.Windows.Forms.ToolTip ttPrimulaAdd;
		private System.Windows.Forms.Button btnPrint;
		private System.Windows.Forms.Button btnPrintPreview;
		private System.Windows.Forms.HelpProvider helpProvider;
		private System.Windows.Forms.Label lblNrofVisitsToPrimula;
		private System.Windows.Forms.Label lblNrofVisitsInPrimula;
		private System.Data.DataView dvInPrimula;
		#endregion 

		#region Initiation

		public Primula(Database aDatabase)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			year = System.DateTime.Now.Year;

			database = aDatabase;
			dgToPrimula.SetDataBinding(database.DsMaster, "Joined");
			dgInPrimula.SetDataBinding(database.DsMaster, "Joined");
			
			SetupDataGridPrimula(dgToPrimula);
			SetupDataGridPrimula(dgInPrimula);
			dgToPrimula.CaptionText = "Besök som ska registreras i Primula";			
			dgInPrimula.CaptionText = "Besök registrerade i Primula";

			cmToPrimula = (CurrencyManager)BindingContext[dgToPrimula.DataSource,dgToPrimula.DataMember];
			cmInPrimula = (CurrencyManager)BindingContext[dgInPrimula.DataSource,dgInPrimula.DataMember];
			dvToPrimula = (System.Data.DataView) cmToPrimula.List;
			dvInPrimula = (System.Data.DataView) cmInPrimula.List;
			
			//Select the current month as default
			dvToPrimula.RowFilter = "primula = false AND visitnote = true AND SUBSTRING(visitdatetime,1,4)=" + year.ToString() + " AND SUBSTRING(visitdatetime,6,2)=" + System.DateTime.Now.Month;
			dvToPrimula.Sort = "visitdatetime DESC";

			//Select the current month as default
			dvInPrimula.RowFilter = "primula = true AND visitnote = true AND SUBSTRING(visitdatetime,1,4)=" + year.ToString() + " AND SUBSTRING(visitdatetime,6,2)=" + System.DateTime.Now.Month;
			dvInPrimula.Sort = "visitdatetime DESC";

			monthsInPrimula = new ArrayList();
			monthsToPrimula = new ArrayList();
			selectedRows = new ArrayList();
			selectedNotes = new ArrayList();

			SetupComboBoxPickMonthInPrimula();
			SetupComboBoxPickMonthToPrimula();

			SetupPrinting();

			note = new Note();

			this.ttPrimulaRemove.SetToolTip(this.btnPrimulaRemove, "Ta bort ett eller flera besök från listan med besök som ska registreras i Primula.\nT.ex. om besöken av någon anledning inte ska skickas in till Landstinget.\nObeservera att dessa besök hamnar under den andra fliken");
			this.ttWritePrimula.SetToolTip(this.btnWritePrimula, "Skriver en Primula-fil som ska skickas in till Landstinget.");
			this.ttPrimulaAdd.SetToolTip(this.btnPrimulaAdd, "Lägg till ett besök till listan med besök som ska registreras i Primula.\nT.ex. om besöket ska registreras om.");
			
			CalculateVisits();

		}

		private void Primula_Load(object sender, System.EventArgs e)
		{
			//Show the current month by selecting it in the comboboc
			int currentMonth = System.DateTime.Now.Month;

			cmbPickMonthToPrimula.SelectedValue = currentMonth;
			cmbPickMonthInPrimula.SelectedValue = currentMonth;

			cmbPickMonthToPrimula.SelectedIndexChanged +=new EventHandler(cmbPickMonthToPrimula_SelectedIndexChanged);
			cmbPickMonthInPrimula.SelectedIndexChanged +=new EventHandler(cmbPickMonthInPrimula_SelectedIndexChanged);

			Utils.AutoSizeDataGrid(null, dgToPrimula, this.BindingContext, 0);
			Utils.AutoSizeDataGrid(null, dgInPrimula, this.BindingContext, 0);

		

		}
		#endregion

		#region Setup comboboxes and datagrids
		private void SetupComboBoxPickMonthInPrimula()
		{
			bool[] notesinMonth = Utils.FindPresentMonths(dvInPrimula, year);
			//The month 0 is reserved and will show all months
			monthsInPrimula.Add(new Utils.Month(0, " Visa alla för " + year.ToString()));

			for(int month = 1; month <= 12; month++)
			{
				if (notesinMonth[month - 1])
				{
					string monthName = (new System.DateTime(year, month, 1)).ToString("MMMM");
					monthsInPrimula.Add(new Utils.Month(month, monthName));
				}
			}

			cmbPickMonthInPrimula.DataSource = monthsInPrimula;
			cmbPickMonthInPrimula.DisplayMember = "MonthName";
			cmbPickMonthInPrimula.ValueMember = "MonthNumber";

		}

		private void SetupComboBoxPickMonthToPrimula()
		{
			bool[] notesinMonth = Utils.FindPresentMonths(dvToPrimula, year);
			//The month 0 is reserved and will show all months
			monthsToPrimula.Add(new Utils.Month(0, " Visa alla för " + year.ToString()));

			for(int month = 1; month <= 12; month++)
			{
				if (notesinMonth[month - 1])
				{
					string monthName = (new System.DateTime(year, month, 1)).ToString("MMMM");
					monthsToPrimula.Add(new Utils.Month(month, monthName));
				}
			}

			cmbPickMonthToPrimula.DataSource = monthsToPrimula;
			cmbPickMonthToPrimula.DisplayMember = "MonthName";
			cmbPickMonthToPrimula.ValueMember = "MonthNumber";

		}

		private void SetupDataGridPrimula(DataGrid dataGrid)
		{
			tsPrimula = new DataGridTableStyle();
			tsPrimula.MappingName = "Joined";

			dataGrid.RowHeadersVisible = false;
			dataGrid.ReadOnly = true;

			Utils.CopyDefaultTableStyle(dataGrid, tsPrimula);						

			Utils.MyColorDataGridTextBoxColumn dgtbcDateTime = new RehabLight.Utils.MyColorDataGridTextBoxColumn();
			//System.Windows.Forms.DataGridTextBoxColumn dgtbcDateTime = new DataGridTextBoxColumn();
			dgtbcDateTime.Width = 120;
			dgtbcDateTime.MappingName = "visitdatetime";
			dgtbcDateTime.HeaderText = "Datum och tid";

			Utils.MyColorDataGridTextBoxColumn dgtbcSurname = new RehabLight.Utils.MyColorDataGridTextBoxColumn();
			//System.Windows.Forms.DataGridTextBoxColumn dgtbcSurname = new DataGridTextBoxColumn();
			dgtbcSurname.Width = 120;
			dgtbcSurname.MappingName = "surname";
			dgtbcSurname.HeaderText = "Efternamn";

			Utils.MyColorDataGridTextBoxColumn dgtbcFirstname = new RehabLight.Utils.MyColorDataGridTextBoxColumn();
			//System.Windows.Forms.DataGridTextBoxColumn dgtbcFirstname = new DataGridTextBoxColumn();
			dgtbcFirstname.Width = 120;
			dgtbcFirstname.MappingName = "firstname";
			dgtbcFirstname.HeaderText = "Förnamn";

			Utils.MyColorDataGridTextBoxColumn dgtbcPersonNumber = new RehabLight.Utils.MyColorDataGridTextBoxColumn();
			//System.Windows.Forms.DataGridTextBoxColumn dgtbcPersonNumber = new DataGridTextBoxColumn();
			dgtbcPersonNumber.Width = 100;
			dgtbcPersonNumber.MappingName = "personnumber";
			dgtbcPersonNumber.HeaderText = "Personnummer";

			Utils.MyColorDataGridTextBoxColumn dgtbcPatientCharge = new RehabLight.Utils.MyColorDataGridTextBoxColumn();
			//System.Windows.Forms.DataGridTextBoxColumn dgtbcPatientCharge = new DataGridTextBoxColumn();
			dgtbcPatientCharge.Width = 100;
			dgtbcPatientCharge.MappingName = "chargetext";
			dgtbcPatientCharge.HeaderText = "Betalning";

			Utils.MyColorDataGridTextBoxColumn dgtbcPatientFee = new RehabLight.Utils.MyColorDataGridTextBoxColumn();
			//System.Windows.Forms.DataGridTextBoxColumn dgtbcPatientFee = new DataGridTextBoxColumn();
			dgtbcPatientFee.Width = 50;
			dgtbcPatientFee.MappingName = "patientfee";
			dgtbcPatientFee.HeaderText = "Avgift";

			Utils.MyColorDataGridTextBoxColumn dgtbcPrimulaText = new RehabLight.Utils.MyColorDataGridTextBoxColumn();
			//System.Windows.Forms.DataGridTextBoxColumn dgtbcPrimulaText = new DataGridTextBoxColumn();
			dgtbcPrimulaText.Width = 0;
			dgtbcPrimulaText.MappingName = "chargeprimula";
			dgtbcPrimulaText.HeaderText = "Primulatext";

			System.Windows.Forms.DataGridTextBoxColumn dgtbcDiagnosisNumber = new DataGridTextBoxColumn();
			dgtbcDiagnosisNumber.Width = 120;
			dgtbcDiagnosisNumber.MappingName = "diagnosisnumber1";
			dgtbcDiagnosisNumber.HeaderText = "Huvuddiagnosnummer";
			dgtbcDiagnosisNumber.NullText = "-";


			System.Windows.Forms.DataGridTextBoxColumn dgtbcDiagnosisText = new DataGridTextBoxColumn();
			dgtbcDiagnosisText.Width = 120;
			dgtbcDiagnosisText.MappingName = "diagnosistext1";
			dgtbcDiagnosisText.HeaderText = "Huvuddiagnos";
			dgtbcDiagnosisText.NullText = "-";


			tsPrimula.GridColumnStyles.Add(dgtbcDateTime);
			tsPrimula.GridColumnStyles.Add(dgtbcSurname);
			tsPrimula.GridColumnStyles.Add(dgtbcFirstname);
			tsPrimula.GridColumnStyles.Add(dgtbcPersonNumber);
			tsPrimula.GridColumnStyles.Add(dgtbcPatientCharge);
			tsPrimula.GridColumnStyles.Add(dgtbcPatientFee);
			tsPrimula.GridColumnStyles.Add(dgtbcPrimulaText);
			

			dataGrid.TableStyles.Add(tsPrimula);

		}
		#endregion

		#region Handle datagrid dgToPrimula and note object update
		private void dgToPrimula_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			DataGrid dgSender = (DataGrid) sender;
			System.Windows.Forms.DataGrid.HitTestInfo hti;
			hti = dgSender.HitTest(e.X, e.Y);

			switch (hti.Type) 
			{
				case System.Windows.Forms.DataGrid.HitTestType.Cell :
					dgSender.Select(hti.Row);
					break;
				case System.Windows.Forms.DataGrid.HitTestType.RowHeader :
					dgSender.Select(hti.Row);
					break;
			}
		
		}

		private void dgToPrimula_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			System.Drawing.Point pt = new Point(e.X, e.Y); 
			DataGrid dgSender = (DataGrid) sender;
 
			DataGrid.HitTestInfo hti = dgSender.HitTest(pt); 
 
			if(hti.Type == DataGrid.HitTestType.Cell) 
 
			{ 
				dgSender.CurrentCell = new DataGridCell(hti.Row, hti.Column); 
				dgSender.Select(hti.Row);
				UpdateNote(dgToPrimula);

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
				
				bool alreadySelected = false;
				for (int i = 0; i < selectedNotes.Count; i++)
				{
					Note tmpNote = (Note)selectedNotes.ToArray()[i];
					if (tmpNote.Id == note.Id)
					{
						selectedNotes.RemoveAt(i);
						alreadySelected = true;
					}
				}
				
				if (!alreadySelected)
				{
					selectedNotes.Add(note);
				}

				for (int i = 0; i < selectedRows.Count; i++)
				{
					dgSender.Select((int)(selectedRows.ToArray())[i]);
				}

			} 
		
		}

		private void dgInPrimula_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			DataGrid dgSender = (DataGrid) sender;
			System.Windows.Forms.DataGrid.HitTestInfo hti;
			hti = dgSender.HitTest(e.X, e.Y);

			switch (hti.Type) 
			{
				case System.Windows.Forms.DataGrid.HitTestType.Cell :
					dgSender.Select(hti.Row);
					break;
				case System.Windows.Forms.DataGrid.HitTestType.RowHeader :
					dgSender.Select(hti.Row);
					break;
			}
		
		}

		private void dgInPrimula_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			System.Drawing.Point pt = new Point(e.X, e.Y); 
			DataGrid dgSender = (DataGrid) sender;
 
			DataGrid.HitTestInfo hti = dgSender.HitTest(pt); 
 
			if(hti.Type == DataGrid.HitTestType.Cell) 
 
			{ 
				dgSender.CurrentCell = new DataGridCell(hti.Row, hti.Column); 
				dgSender.Select(hti.Row); 
				UpdateNote(dgInPrimula);

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
				
				bool alreadySelected = false;
				for (int i = 0; i < selectedNotes.Count; i++)
				{
					Note tmpNote = (Note)selectedNotes.ToArray()[i];
					if (tmpNote.Id == note.Id)
					{
						selectedNotes.RemoveAt(i);
						alreadySelected = true;
					}
				}
				
				if (!alreadySelected)
				{
					selectedNotes.Add(note);
				}

				for (int i = 0; i < selectedRows.Count; i++)
				{
					dgSender.Select((int)(selectedRows.ToArray())[i]);
				}
			} 
		
		}


		private void UpdateNote(DataGrid dataGrid)
		{
			System.Windows.Forms.CurrencyManager cm = (CurrencyManager)BindingContext[dataGrid.DataSource,dataGrid.DataMember];
			System.Data.DataView dv = (System.Data.DataView) cm.List;
				
			if (dv.Count > 0)
			{
				note = new Note();
				note.Id = (int) dv[dataGrid.CurrentRowIndex]["noteid"];
				note.CreateDateTime = System.DateTime.Parse((string)dv[dataGrid.CurrentRowIndex]["createdatetime"]);
				note.SignedDateTime = System.DateTime.Parse((string) dv[dataGrid.CurrentRowIndex]["signedDateTime"]);
				note.PatientId = (int) dv[dataGrid.CurrentRowIndex]["patientid"];
				note.Signed = (bool) dv[dataGrid.CurrentRowIndex]["signed"];
				note.NewVisit = (bool) dv[dataGrid.CurrentRowIndex]["newvisit"];
				//TODO: Check maximum nr of chars a string can hold
				note.JournalNote =  (string) dv[dataGrid.CurrentRowIndex]["note"];
				note.ChargeId = (int) dv[dataGrid.CurrentRowIndex]["chargeid"];
				note.PatientFee = (string) dv[dataGrid.CurrentRowIndex]["patientfee"];
				note.Diagnosis1 = (int) dv[dataGrid.CurrentRowIndex]["diagnosis1"];
				note.Diagnosis2 = (int) dv[dataGrid.CurrentRowIndex]["diagnosis2"];
				note.Diagnosis3 = (int) dv[dataGrid.CurrentRowIndex]["diagnosis3"];
				note.Diagnosis4 = (int) dv[dataGrid.CurrentRowIndex]["diagnosis4"];
				note.Diagnosis5 = (int) dv[dataGrid.CurrentRowIndex]["diagnosis5"];
				note.ActionCode = (string) dv[dataGrid.CurrentRowIndex]["actioncode"];
				note.Primula = (bool) dv[dataGrid.CurrentRowIndex]["primula"];
				note.VisitDateTime = System.DateTime.Parse((string) dv[dataGrid.CurrentRowIndex]["visitdatetime"]);
				note.VisitNote = (bool)dv[dataGrid.CurrentRowIndex]["visitnote"];

				Debug.WriteLine("Selected note at currentrowindex: " + dataGrid.CurrentRowIndex + ", primula = " + note.Primula.ToString() +" " + note.JournalNote);
			}
		}
		#endregion

		#region Handle writing of Primula file
		private bool WritePrimulaFile()
		{
			System.Data.DataView dv;
			bool success = false;
			ArrayList patientNotes = new ArrayList();
			string filename = Settings.PrimulaFilename + "." + Settings.PrimulaFileNumber;
			
			System.Windows.Forms.CurrencyManager cm = (CurrencyManager)BindingContext[dgToPrimula.DataSource,dgToPrimula.DataMember];
			dv = (System.Data.DataView) cm.List;

			//If rows were selected, create a dataset containing only those
			if (selectedRows.Count > 0)
			{
				System.Data.DataSet ds = Utils.SelectedRowsDataSet(dv, selectedRows);
				dv = ds.Tables[0].DefaultView;
			}

			foreach (System.Data.DataRowView drv in dv)
			{
				Note tmpNote = database.CreateNote(drv.Row);
				Patient tmpPatient = database.CreatePatient(drv.Row, null);
				Charge tmpCharge = database.CreateCharge(drv.Row);
				Utils.PatientNote patientNote = new Utils.PatientNote(tmpPatient, tmpNote, tmpCharge);
				patientNotes.Add(patientNote);
			}

			//Create an array with note objects in order to update all notes at the same time when the file has been written
			Note[] noteArray = new Note[patientNotes.Count];
			int index = 0;

			try 
			{
				using (StreamWriter writer = new StreamWriter(filename))
				{
					foreach (object o in patientNotes.ToArray())
					{
						
						Utils.PatientNote patientNote = (Utils.PatientNote) o;
						Note currentNote = patientNote.note;
						Patient currentPatient = patientNote.patient;
						Charge currentCharge = patientNote.charge;

						System.DateTime date = currentNote.VisitDateTime;
						

						//Field 1 - Personnummer
						writer.Write(currentPatient.Personnumber.Remove(8,1));
						writer.Write(";");
						//Field 2
						writer.Write(";");
						//Field 3
						writer.Write(";");
						//Field 4
						writer.Write(";");
						//Field 5
						writer.Write(";");
						//Field 6
						writer.Write(";");
						////Field 7 - Avtalskod
						writer.Write("0164");
						writer.Write(";");
						//Field 8
						writer.Write(";");
						//Field 9
						writer.Write(";");
						//Field 10
						writer.Write(";");
						//Field 11
						writer.Write(";");
						//Field 12 - Kontaktdatum
						writer.Write(date.ToString("yyyyMMdd"));
						writer.Write(";");
						//Field 13 - Kontakttid
						writer.Write(date.ToString("HHmm"));
						writer.Write(";");
						//Field 14
						writer.Write(";");
						//Field 15 - Vårdgivarkategori
						writer.Write("07");
						writer.Write(";");
						//Field 16
						writer.Write(";");
						//Field 17
						writer.Write(";");
						//Field 18
						writer.Write(";");
						//Field 19
						writer.Write(";");
						//Field 20
						writer.Write(";");
						//Field 21
						writer.Write(";");
						//Field 22
						writer.Write(";");
						//Field 23
						writer.Write(";");
						//Field 24
						writer.Write(";");
						//Field 25
						writer.Write(";");
						//Field 26
						writer.Write(";");
						//Field 27
						writer.Write(";");
						//Field 28 
						writer.Write(";");
						//Field 29
						writer.Write(";");
						//Field 30
						writer.Write(";");
						//Field 31
						writer.Write(";");
						//Field 32
						writer.Write(";");
						//Field 33
						writer.Write(";");
						//Field 34
						writer.Write(";");
						//Field 35
						writer.Write(";");
						//Field 36- Åtgärdskod
						writer.Write(currentNote.ActionCode);
						writer.Write(";");
						//Field 37
						writer.Write(";");
						//Field 38
						writer.Write(";");
						//Field 39
						writer.Write(";");
						//Field 40
						writer.Write(";");
						//Field 41
						writer.Write(";");
						//Field 42
						writer.Write(";");
						//Field 43
						writer.Write(";");
						//Field 44
						writer.Write(";");
						//Field 45 - Patientavgift, kod
						writer.Write(currentCharge.PrimulaCharachter);
						writer.Write(";");
						//Field 46 - Patientavgift, belopp
						writer.Write(currentNote.PatientFee);
						writer.Write(";");
						//Field 47
						writer.Write(";");
						//Field 48
						writer.Write(";");
						//Field 49
						writer.Write(";");
						//Field 50
						writer.Write(";");
						//Field 51
						writer.Write(";");
						//Field 52
						writer.Write(";");
						//Field 53
						writer.Write(";");
						//Field 54
						writer.Write(";");
						//Field 55 - Vårdgivarspecialitet
						writer.Write("22");
						writer.Write(";");
						//Field 56
						writer.Write(";");
						//Field 57
						writer.Write(";");
						//Field 58
						writer.Write(";");
						//Field 59
						writer.Write(";");
						//Field 60
						writer.Write(";");
						//Field 61
						writer.Write(";");
						//Field 62
						writer.Write(";");
						//Field 63
						writer.Write(";");
						//Field 64
						writer.Write(";");
						//Field 65
						writer.Write(";");
						//Field 66
						writer.Write(";");
						//Field 67
						writer.Write(";");
						//Field 68
						writer.WriteLine(";");
						
						
						//This is the update
						currentNote.Primula = true;

						//Instead of invoking database update, store the note object in an array and call the update later
						//database.Update(currentNote);
						noteArray[index] = currentNote;
						index++;

					}
				}

				int nrofLines = 0;

				using (StreamReader reader = new StreamReader(filename))
				{
					while (reader.ReadLine() != null)
						nrofLines++;
				}

				if (nrofLines != noteArray.Length)
					throw new Exception("Antalet besök i den skapta Primula-filen stämmer inte överens med antalet besök som ska registreras.");


				//TODO: Increase performance by calling the update function with an array of notes
				database.Update(noteArray);

				Settings.IncPrimulaFileNumber();

				MessageBox.Show("Primulafilen " + filename + " skapades", "Skrivning av Primulafil lyckades");

				//TODO: This should be done on exit
				Settings.SaveSettings();

				success = true;
				selectedRows.Clear();
				selectedNotes.Clear();

				//Update the printed number of visits etc.
				CalculateVisits();
			}

			catch(Exception exception)
			{
				MessageBox.Show("Det gick ej skapa Primulafilen, kontrollera att USB-minnet sitter i: \nFelmeddelande från programmet: " + exception.Message, "Kunde ej skapa Primulafil");
			}
			return success;

		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.btnPrimulaRemove = new System.Windows.Forms.Button();
			this.btnPrimulaAdd = new System.Windows.Forms.Button();
			this.dgToPrimula = new CustomDataGrid.MyDataGrid();
			this.btnWritePrimula = new System.Windows.Forms.Button();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabToPrimula = new System.Windows.Forms.TabPage();
			this.lblNrofVisitsToPrimula = new System.Windows.Forms.Label();
			this.cmbPickMonthToPrimula = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabInPrimula = new System.Windows.Forms.TabPage();
			this.lblNrofVisitsInPrimula = new System.Windows.Forms.Label();
			this.cmbPickMonthInPrimula = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.dgInPrimula = new CustomDataGrid.MyDataGrid();
			this.btnClose = new System.Windows.Forms.Button();
			this.ttPrimulaRemove = new System.Windows.Forms.ToolTip(this.components);
			this.ttWritePrimula = new System.Windows.Forms.ToolTip(this.components);
			this.ttPrimulaAdd = new System.Windows.Forms.ToolTip(this.components);
			this.btnPrint = new System.Windows.Forms.Button();
			this.btnPrintPreview = new System.Windows.Forms.Button();
			this.helpProvider = new System.Windows.Forms.HelpProvider();
			((System.ComponentModel.ISupportInitialize)(this.dgToPrimula)).BeginInit();
			this.tabControl.SuspendLayout();
			this.tabToPrimula.SuspendLayout();
			this.tabInPrimula.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgInPrimula)).BeginInit();
			this.SuspendLayout();
			// 
			// btnPrimulaRemove
			// 
			this.btnPrimulaRemove.Location = new System.Drawing.Point(648, 528);
			this.btnPrimulaRemove.Name = "btnPrimulaRemove";
			this.btnPrimulaRemove.Size = new System.Drawing.Size(136, 32);
			this.btnPrimulaRemove.TabIndex = 5;
			this.btnPrimulaRemove.Text = "Ta bort besök";
			this.btnPrimulaRemove.Click += new System.EventHandler(this.btnPrimulaRemove_Click);
			// 
			// btnPrimulaAdd
			// 
			this.btnPrimulaAdd.Location = new System.Drawing.Point(792, 528);
			this.btnPrimulaAdd.Name = "btnPrimulaAdd";
			this.btnPrimulaAdd.Size = new System.Drawing.Size(168, 40);
			this.btnPrimulaAdd.TabIndex = 4;
			this.btnPrimulaAdd.Text = "Lägg till markerat besök";
			this.btnPrimulaAdd.Click += new System.EventHandler(this.btnPrimulaAdd_Click);
			// 
			// dgToPrimula
			// 
			this.dgToPrimula.DataMember = "";
			this.dgToPrimula.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dgToPrimula.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.helpProvider.SetHelpString(this.dgToPrimula, @"De besök som ska skickas till Landstinget ses i den här fliken ([Besök som ska registreras]). Aktuell månad väljs automatiskt, en annan månad kan väljas längst ner i formuläret. Den här vyn kan användas för att få en överblick över de besök som ska in i Primula. Besök som inte ska skickas in (som kanske redan har skickats in) kan markeras och tas bort genom att klicka på knappen [Ta bort besök]. Då hamnar besöket i den andra fliken ([Besök som har registreras])");
			this.dgToPrimula.Location = new System.Drawing.Point(0, 8);
			this.dgToPrimula.Name = "dgToPrimula";
			this.dgToPrimula.ReadOnly = true;
			this.helpProvider.SetShowHelp(this.dgToPrimula, true);
			this.dgToPrimula.Size = new System.Drawing.Size(984, 440);
			this.dgToPrimula.TabIndex = 3;
			this.dgToPrimula.Tag = "toPrimula";
			this.dgToPrimula.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgToPrimula_MouseDown);
			this.dgToPrimula.SizeChanged += new System.EventHandler(this.dgToPrimula_SizeChanged);
			this.dgToPrimula.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgToPrimula_MouseUp);
			// 
			// btnWritePrimula
			// 
			this.btnWritePrimula.Location = new System.Drawing.Point(824, 528);
			this.btnWritePrimula.Name = "btnWritePrimula";
			this.btnWritePrimula.Size = new System.Drawing.Size(136, 32);
			this.btnWritePrimula.TabIndex = 6;
			this.btnWritePrimula.Text = "Skriv Primula fil";
			this.btnWritePrimula.Click += new System.EventHandler(this.btnWritePrimula_Click);
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabToPrimula);
			this.tabControl.Controls.Add(this.tabInPrimula);
			this.tabControl.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tabControl.Location = new System.Drawing.Point(8, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(992, 608);
			this.tabControl.TabIndex = 8;
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
			// 
			// tabToPrimula
			// 
			this.tabToPrimula.Controls.Add(this.lblNrofVisitsToPrimula);
			this.tabToPrimula.Controls.Add(this.dgToPrimula);
			this.tabToPrimula.Controls.Add(this.btnPrimulaRemove);
			this.tabToPrimula.Controls.Add(this.btnWritePrimula);
			this.tabToPrimula.Controls.Add(this.cmbPickMonthToPrimula);
			this.tabToPrimula.Controls.Add(this.label1);
			this.tabToPrimula.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tabToPrimula.Location = new System.Drawing.Point(4, 27);
			this.tabToPrimula.Name = "tabToPrimula";
			this.tabToPrimula.Size = new System.Drawing.Size(984, 577);
			this.tabToPrimula.TabIndex = 0;
			this.tabToPrimula.Text = "Besök som ska registreras";
			// 
			// lblNrofVisitsToPrimula
			// 
			this.lblNrofVisitsToPrimula.Location = new System.Drawing.Point(16, 464);
			this.lblNrofVisitsToPrimula.Name = "lblNrofVisitsToPrimula";
			this.lblNrofVisitsToPrimula.Size = new System.Drawing.Size(712, 32);
			this.lblNrofVisitsToPrimula.TabIndex = 10;
			// 
			// cmbPickMonthToPrimula
			// 
			this.cmbPickMonthToPrimula.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbPickMonthToPrimula.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbPickMonthToPrimula.Location = new System.Drawing.Point(344, 536);
			this.cmbPickMonthToPrimula.Name = "cmbPickMonthToPrimula";
			this.cmbPickMonthToPrimula.Size = new System.Drawing.Size(176, 26);
			this.cmbPickMonthToPrimula.TabIndex = 8;
			this.cmbPickMonthToPrimula.SelectedIndexChanged += new System.EventHandler(this.cmbPickMonthToPrimula_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(24, 536);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(312, 23);
			this.label1.TabIndex = 9;
			this.label1.Text = "Välj vilken månad som ska visas";
			// 
			// tabInPrimula
			// 
			this.tabInPrimula.BackColor = System.Drawing.SystemColors.Control;
			this.tabInPrimula.Controls.Add(this.lblNrofVisitsInPrimula);
			this.tabInPrimula.Controls.Add(this.cmbPickMonthInPrimula);
			this.tabInPrimula.Controls.Add(this.label2);
			this.tabInPrimula.Controls.Add(this.dgInPrimula);
			this.tabInPrimula.Controls.Add(this.btnPrimulaAdd);
			this.tabInPrimula.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tabInPrimula.Location = new System.Drawing.Point(4, 27);
			this.tabInPrimula.Name = "tabInPrimula";
			this.tabInPrimula.Size = new System.Drawing.Size(984, 577);
			this.tabInPrimula.TabIndex = 1;
			this.tabInPrimula.Text = "Besök som är registrerade";
			// 
			// lblNrofVisitsInPrimula
			// 
			this.lblNrofVisitsInPrimula.Location = new System.Drawing.Point(16, 464);
			this.lblNrofVisitsInPrimula.Name = "lblNrofVisitsInPrimula";
			this.lblNrofVisitsInPrimula.Size = new System.Drawing.Size(712, 32);
			this.lblNrofVisitsInPrimula.TabIndex = 12;
			// 
			// cmbPickMonthInPrimula
			// 
			this.cmbPickMonthInPrimula.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbPickMonthInPrimula.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbPickMonthInPrimula.Location = new System.Drawing.Point(344, 536);
			this.cmbPickMonthInPrimula.Name = "cmbPickMonthInPrimula";
			this.cmbPickMonthInPrimula.Size = new System.Drawing.Size(176, 26);
			this.cmbPickMonthInPrimula.TabIndex = 10;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(24, 536);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(312, 23);
			this.label2.TabIndex = 11;
			this.label2.Text = "Välj vilken månad som ska visas";
			// 
			// dgInPrimula
			// 
			this.dgInPrimula.DataMember = "";
			this.dgInPrimula.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dgInPrimula.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.helpProvider.SetHelpString(this.dgInPrimula, @"De besök som är skickade till Landstinget ses i den här fliken ([Besök som är registrerade]). Aktuell månad väljs automatiskt, en annan månad kan väljas längst ner i formuläret. Den här vyn kan användas för att jämföra med den sammanställning som Primula visar. Besök som måste registreras om (dvs. en ny Primula-fil med dessa besök ska genereras) kan markeras här. Genom att trycka på knappen [Lägg till besök] hamnar de i listan i den andra fliken ([Besök som ska registreras])");
			this.dgInPrimula.Location = new System.Drawing.Point(0, 8);
			this.dgInPrimula.Name = "dgInPrimula";
			this.dgInPrimula.ReadOnly = true;
			this.helpProvider.SetShowHelp(this.dgInPrimula, true);
			this.dgInPrimula.Size = new System.Drawing.Size(984, 440);
			this.dgInPrimula.TabIndex = 4;
			this.dgInPrimula.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgInPrimula_MouseDown);
			this.dgInPrimula.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgInPrimula_MouseUp);
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnClose.Location = new System.Drawing.Point(840, 624);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(136, 32);
			this.btnClose.TabIndex = 7;
			this.btnClose.Text = "Stäng";
			// 
			// btnPrint
			// 
			this.btnPrint.Location = new System.Drawing.Point(616, 616);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(136, 40);
			this.btnPrint.TabIndex = 9;
			this.btnPrint.Text = "Slrriv ut...";
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
			// 
			// btnPrintPreview
			// 
			this.btnPrintPreview.Location = new System.Drawing.Point(464, 616);
			this.btnPrintPreview.Name = "btnPrintPreview";
			this.btnPrintPreview.Size = new System.Drawing.Size(136, 40);
			this.btnPrintPreview.TabIndex = 10;
			this.btnPrintPreview.Text = "Förhandsgranska utskrift...";
			this.btnPrintPreview.Click += new System.EventHandler(this.btnPrintPreview_Click);
			// 
			// Primula
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(1000, 666);
			this.Controls.Add(this.btnPrintPreview);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnPrint);
			this.HelpButton = true;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(1008, 700);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(1008, 700);
			this.Name = "Primula";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Primula";
			this.Load += new System.EventHandler(this.Primula_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgToPrimula)).EndInit();
			this.tabControl.ResumeLayout(false);
			this.tabToPrimula.ResumeLayout(false);
			this.tabInPrimula.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgInPrimula)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Button handling
		private void btnWritePrimula_Click(object sender, System.EventArgs e)
		{
			if (selectedRows.Count > 0)
			{
				if (DialogResult.No == MessageBox.Show("Vill du skriva Primula fil för de markerade " + selectedRows.Count.ToString() + " besöken?", "Skriva Primulafil", MessageBoxButtons.YesNo))
					return;
			}
			else
			{
				System.Windows.Forms.CurrencyManager cm = (CurrencyManager)BindingContext[dgToPrimula.DataSource,dgToPrimula.DataMember];

				if (DialogResult.No == MessageBox.Show("Vill du skriva Primula fil för " + cm.List.Count.ToString() + " besök?", "Skriva Primulafil", MessageBoxButtons.YesNo))
					return;
			}

			WritePrimulaFile();
		}

		private void btnPrimulaRemove_Click(object sender, System.EventArgs e)
		{
			if (selectedNotes.Count > 0)
			{
				Note[] noteArray = new Note[selectedNotes.Count];

				for (int i = 0; i < selectedNotes.Count; i++)
				{
					Note n = (Note) selectedNotes[i];
					n.Primula = true;
					
					noteArray[i] = n;
				}
				
				database.Update(noteArray);

				dgToPrimula.Refresh();

				selectedRows.Clear();
				selectedNotes.Clear();

				CalculateVisits();
				
				Utils.AutoSizeDataGrid(null, dgInPrimula, this.BindingContext, 0);
		
			}
			else
				MessageBox.Show("Välj ett besök eller fera besök att ta bort först", "Kunde ej ta bort besök från listan");
		}

		private void btnPrimulaAdd_Click(object sender, System.EventArgs e)
		{
			if (selectedNotes.Count > 0)
			{
				Note[] noteArray = new Note[selectedNotes.Count];

				for (int i = 0; i < selectedNotes.Count; i++)
				{
					Note n = (Note) selectedNotes[i];
					n.Primula = false;
					
					noteArray[i] = n;
				}
				
				database.Update(noteArray);

				dgInPrimula.Refresh();

				selectedRows.Clear();
				selectedNotes.Clear();

				CalculateVisits();

				Utils.AutoSizeDataGrid(null, dgToPrimula, this.BindingContext, 0);
			}
			else
				MessageBox.Show("Välj ett besök att lägga till först", "Kunde ej lägga till besök");
		}
		#endregion

		#region Combobox handling
		private void cmbPickMonthToPrimula_SelectedIndexChanged(object sender, EventArgs e)
		{
		
			if (cmbPickMonthToPrimula.SelectedIndex != -1)
			{
				//TODO:Is it possible to have a class global dataview?
			
				System.Data.DataView dv = (System.Data.DataView) cmToPrimula.List;

				Utils.Month selectedMonth = (Utils.Month) cmbPickMonthToPrimula.SelectedItem;

				string rowFilter = "primula = 0 AND visitnote = 1 AND SUBSTRING(visitdatetime,1,4)=" + year.ToString();
			
				if (selectedMonth.MonthNumber != 0)
					rowFilter = rowFilter + " AND SUBSTRING(visitdatetime,6,2)=" + selectedMonth.MonthNumber.ToString();

				dv.RowFilter = rowFilter;
				dv.Sort = "visitdatetime DESC";

				CalculateVisits();

				Utils.AutoSizeDataGrid(null, dgToPrimula, this.BindingContext, 0);

			}

		}

		private void cmbPickMonthInPrimula_SelectedIndexChanged(object sender, EventArgs e)
		{
		
			if (cmbPickMonthInPrimula.SelectedIndex != -1)
			{
				//TODO:Is it possible to have a class global dataview?
				System.Data.DataView dv = (System.Data.DataView) cmInPrimula.List;

				Utils.Month selectedMonth = (Utils.Month) cmbPickMonthInPrimula.SelectedItem;

				string rowFilter = "primula = true AND visitnote = true AND SUBSTRING(visitdatetime,1,4)=" + year.ToString();
			
				if (selectedMonth.MonthNumber != 0)
					rowFilter = rowFilter + " AND SUBSTRING(visitdatetime,6,2)=" + selectedMonth.MonthNumber.ToString();

				dv.RowFilter = rowFilter;
				dv.Sort = "visitdatetime DESC";

				cmbPickMonthInPrimula.SelectionLength = 0;
				cmbPickMonthToPrimula.SelectionStart = 0;

				CalculateVisits();

				Utils.AutoSizeDataGrid(null, dgInPrimula, this.BindingContext, 0);
			}

		}
		#endregion

		private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int currentMonth = System.DateTime.Now.Month;

			if (tabControl.SelectedIndex == 0)
			{	
				int tmp_index = cmbPickMonthToPrimula.SelectedIndex;
				cmbPickMonthToPrimula.SelectedIndex =  -1;
				cmbPickMonthToPrimula.SelectedIndex =  tmp_index;
				Utils.AutoSizeDataGrid(null, dgToPrimula, this.BindingContext, 0);
				
			}
			else
			{
				int tmp_index = cmbPickMonthInPrimula.SelectedIndex;
				cmbPickMonthInPrimula.SelectedIndex =  -1;
				cmbPickMonthInPrimula.SelectedIndex =  tmp_index;
				Utils.AutoSizeDataGrid(null, dgInPrimula, this.BindingContext, 0);
			}

			selectedRows.Clear();
			selectedNotes.Clear();
			
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

		private void btnPrintPreview_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.CurrencyManager cm;

			if (tabControl.SelectedIndex == 0)
				cm = (CurrencyManager)BindingContext[dgToPrimula.DataSource,dgToPrimula.DataMember];
			else
				cm = (CurrencyManager)BindingContext[dgInPrimula.DataSource,dgInPrimula.DataMember];

			System.Data.DataView dv = (System.Data.DataView) cm.List;
			
			Utils.PatientNote[] patientNotesArray = Utils.CreatePatientNotesArray(database, dv);
			printPrimula.SetContent(patientNotesArray, nrofVisitsTotal ,nrofVisitsFreecard ,nrofVisitsPatientPays, nrofVisitsYouth); 
			printPreviewDialog.Document = printPrimula.PrintDocument;
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

		private void SetupPrinting()
		{
			printPrimula = new PrintPrimula();

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

		private void CalculateVisits()
		{
			Utils.Month selectedMonth;

			if (tabControl.SelectedIndex == 0)
				selectedMonth = (Utils.Month) cmbPickMonthToPrimula.SelectedItem;
			else
				selectedMonth = (Utils.Month) cmbPickMonthInPrimula.SelectedItem;

			
			string queryString = "SELECT COUNT(Notes.noteid) FROM (Notes INNER JOIN Charges ON Charges.chargeid = Notes.chargeid) WHERE (Notes.visitnote = 1) AND (MID((Notes.visitdatetime), 1, 4) = " + year.ToString() + ")";
			
			
			if (selectedMonth.MonthNumber != 0)
				queryString += " AND (MID((Notes.visitdatetime), 6, 2) = " +selectedMonth.MonthNumber + ")";

			if (tabControl.SelectedIndex == 0)
				queryString += "AND (Notes.primula = 0)";
			else
				queryString += "AND (Notes.primula = 1)";

			string queryStringFreecard = queryString + "AND (Charges.primulatext = 'F')";
			string queryStringPatientPays = queryString + "AND (Charges.primulatext = 'J')";
			string queryStringYouth = queryString + "AND (Charges.primulatext = 'B')";
			
			nrofVisitsTotal = database.QueryDatabase(queryString);
			nrofVisitsFreecard = database.QueryDatabase(queryStringFreecard);
			nrofVisitsPatientPays = database.QueryDatabase(queryStringPatientPays);
			nrofVisitsYouth = database.QueryDatabase(queryStringYouth);

			lblNrofVisitsToPrimula.Text = "Totalt antal besök: " + nrofVisitsTotal.ToString() + ". Antal frikortsbesök: " + nrofVisitsFreecard.ToString() + ". Antal besök patienten betalat: " + nrofVisitsPatientPays.ToString() + "." + " Antal besök barn och ungdom: " + nrofVisitsYouth.ToString() + ".";

			lblNrofVisitsInPrimula.Text = "Totalt antal besök: " + nrofVisitsTotal.ToString() + ". Antal frikortsbesök: " + nrofVisitsFreecard.ToString() + ". Antal besök patienten betalat: " + nrofVisitsPatientPays.ToString() + "." + " Antal besök barn och ungdom: " + nrofVisitsYouth.ToString() + ".";


		}

		private void dgToPrimula_SizeChanged(object sender, System.EventArgs e)
		{
			Utils.AutoSizeDataGrid(null, dgToPrimula, this.BindingContext, 0);
		}

		private void btnPrint_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.CurrencyManager cm;

			if (tabControl.SelectedIndex == 0)
				cm = (CurrencyManager)BindingContext[dgToPrimula.DataSource,dgToPrimula.DataMember];
			else
				cm = (CurrencyManager)BindingContext[dgInPrimula.DataSource,dgInPrimula.DataMember];

			System.Data.DataView dv = (System.Data.DataView) cm.List;
			
			Utils.PatientNote[] patientNotesArray = Utils.CreatePatientNotesArray(database, dv);
			printPrimula.SetContent(patientNotesArray, nrofVisitsTotal ,nrofVisitsFreecard ,nrofVisitsPatientPays, nrofVisitsYouth); 
			printDialog.Document = printPrimula.PrintDocument;

			if (printDialog.ShowDialog() == DialogResult.OK)
				printPrimula.Print();
		
		}		
		
	}
}
