using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;

namespace RehabLight
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		#region Variables


		private Database database;
		private System.Windows.Forms.DataGridTableStyle tsPatients;
		private System.Windows.Forms.DataGridTableStyle tsNotes;
		private System.Windows.Forms.DataGridTableStyle tsJoined;
		private Patient patient;
		private Note note;

		private System.Windows.Forms.PrintDialog printDialog;
		private System.Windows.Forms.PrintPreviewDialog printPreviewDialog;
		private PrintReceipt printReceipt;
		private PrintJournal printJournal;

		private System.Data.DataRowView selectedPatientRow;

		//private const string settingsFilename = "C:\\Documents and Settings\\Anders Ruberg\\Mina dokument\\Visual Studio Projects\\Rehab2\\Settings.xml";
		//private const string databaseFilename = "C:\\Documents and Settings\\Anders Ruberg\\Mina dokument\\Visual Studio Projects\\Rehab2\\rehab.mdb";
		private const string settingsFilename = "Settings.xml";
		private const string databaseFilename = "rehab.mdb";
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuFile;
		private System.Windows.Forms.MenuItem menuClose;
		private System.Windows.Forms.MenuItem menuTools;
		private System.Windows.Forms.MenuItem menuSettings;
		private System.Windows.Forms.MenuItem menuPrimula;
		private System.Windows.Forms.MenuItem menuBackup;
		private System.Windows.Forms.MenuItem menuHelp;
		private System.Windows.Forms.MenuItem menuHelpcontents;
		private System.Windows.Forms.MenuItem menuAbout;
		private System.Windows.Forms.TabPage tabJournal;
		private System.Windows.Forms.Button btnChangePatient;
		private CustomDataGrid.MyDataGrid dgNotes;
		private System.Windows.Forms.Button btnSign;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.Button btnNewNote;
		private CustomDataGrid.MyDataGrid dgPatients;
		private System.Windows.Forms.TextBox txtSearchPatient;
		private System.Windows.Forms.Button btnNewPatient;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.GroupBox groupBox6;
		private System.Windows.Forms.Button btnClearSearchPatient;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.Button btnPrintReciet;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuPrint;
		private System.Windows.Forms.MenuItem menuPrintReciet;
		private System.Windows.Forms.MenuItem menuPreviewJournal;
		private System.Windows.Forms.MenuItem menuPrintJournal;
		private System.Windows.Forms.Button btnPrintJournal;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menyAddDiagnosisNumber;
		private System.Windows.Forms.ComboBox cmbShowFiltered;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region Initiation
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			try 
			{
				Settings.LoadSettings(settingsFilename);
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, "Felmeddelande",MessageBoxButtons.OK);
				this.Close();
				return;
			}

			Form4 dlgWelcome = new Form4();
			if (dlgWelcome.ShowDialog() == DialogResult.OK)
			{
				try
				{
					database = new Database(databaseFilename, dlgWelcome.Password);
				}
				catch (Exception exception)
				{
					MessageBox.Show(exception.Message, "Felmeddelande",MessageBoxButtons.OK);
					this.Close();
					return;
				}

				SetupDataGridPatients();

				System.Windows.Forms.CurrencyManager cm = (CurrencyManager)BindingContext[dgPatients.DataSource,dgPatients.DataMember];
				System.Data.DataView dv = (System.Data.DataView) cm.List;
				
				if (dv.Count > 0)
				{
					dgPatients.Select(0);
					dgPatients.CurrentRowIndex = 0;
					UpdatePatient(dgPatients);
				}

				SetupDataGridNotes();
				SetupDataGridJoined();
				SetupComboBoxShowFiltered();

				Utils.AutoSizeDataGrid(new string[]{"note"},dgNotes, this.BindingContext, 0);
				Utils.AutoSizeDataGrid(null, dgPatients, this.BindingContext, 0);
				
				SetupPrinting(Settings.ReceiptLogoFilename);
			}
			else
				this.Close();
		}
		#endregion
		
		#region Setup datagrids
		/// <summary>
		/// Setup datagrids
		/// </summary>
		private void SetupDataGridPatients()
		{
			dgPatients.SetDataBinding(database.DsMaster, "Patients");

			tsPatients = new DataGridTableStyle();
			tsPatients.MappingName = "Patients";

			dgPatients.CaptionText = "Patienter";
			dgPatients.RowHeadersVisible = false;

			Utils.CopyDefaultTableStyle(dgPatients, tsPatients);

			// Setup the datagridtablestyle
			
			
			System.Windows.Forms.DataGridTextBoxColumn dgtbcFirstname = new DataGridTextBoxColumn();
			dgtbcFirstname.Width = 145;
			dgtbcFirstname.MappingName = "firstname";
			dgtbcFirstname.HeaderText = "Förnamn";
			dgtbcFirstname.NullText = "";

			System.Windows.Forms.DataGridTextBoxColumn dgtbcSurname = new DataGridTextBoxColumn();
			dgtbcSurname.Width = 140;
			dgtbcSurname.MappingName = "surname";
			dgtbcSurname.HeaderText = "Efternamn";
			dgtbcSurname.NullText = "";

			System.Windows.Forms.DataGridTextBoxColumn dgtbcPersonnumber = new DataGridTextBoxColumn();
			dgtbcPersonnumber.Width = 140;
			dgtbcPersonnumber.MappingName = "personnumber";
			dgtbcPersonnumber.HeaderText = "Personnummer";
			dgtbcPersonnumber.NullText = "";

			System.Windows.Forms.DataGridTextBoxColumn dgtbcHomePhone = new DataGridTextBoxColumn();
			dgtbcHomePhone.Width = 120;
			dgtbcHomePhone.MappingName = "homephone";
			dgtbcHomePhone.HeaderText = "Telefon hem";
			dgtbcHomePhone.NullText = "";

			System.Windows.Forms.DataGridTextBoxColumn dgtbcWorkPhone = new DataGridTextBoxColumn();
			dgtbcWorkPhone.Width = 120;
			dgtbcWorkPhone.MappingName = "workphone";
			dgtbcWorkPhone.HeaderText = "Telefon arbete";
			dgtbcWorkPhone.NullText = "";

			System.Windows.Forms.DataGridTextBoxColumn dgtbcMobilePhone = new DataGridTextBoxColumn();
			dgtbcMobilePhone.Width = 120;
			dgtbcMobilePhone.MappingName = "mobilephone";
			dgtbcMobilePhone.HeaderText = "Mobiltelefon";
			dgtbcMobilePhone.NullText = "";

			tsPatients.GridColumnStyles.Add(dgtbcSurname);
			tsPatients.GridColumnStyles.Add(dgtbcFirstname);
			tsPatients.GridColumnStyles.Add(dgtbcPersonnumber);
			tsPatients.GridColumnStyles.Add(dgtbcHomePhone);
			tsPatients.GridColumnStyles.Add(dgtbcWorkPhone);
			tsPatients.GridColumnStyles.Add(dgtbcMobilePhone);

			dgPatients.TableStyles.Add(tsPatients);

			System.Windows.Forms.CurrencyManager cm = (CurrencyManager)BindingContext[dgPatients.DataSource,dgPatients.DataMember];
			DataView dv = (DataView) cm.List;
			dv.Sort = "surname";

			
		}

		private void SetupDataGridNotes()
		{
			dgNotes.SetDataBinding(database.DsMaster, "Patients.PatientNotes");

			tsNotes = new DataGridTableStyle();
			tsNotes.MappingName = "Notes";

			dgNotes.CaptionText = "Journalanteckningar";
			dgNotes.RowHeadersVisible = false;
			dgNotes.ReadOnly = true;

			Utils.CopyDefaultTableStyle(dgNotes, tsNotes);			
			
			System.Windows.Forms.DataGridBoolColumn dgbcSigned = new System.Windows.Forms.DataGridBoolColumn();
			dgbcSigned.Width = 100;
			dgbcSigned.MappingName = "signed";
			dgbcSigned.HeaderText = "Signerad";

			System.Windows.Forms.DataGridTextBoxColumn dgtbcNote = new DataGridTextBoxColumn();
			dgtbcNote.Width = 670;
			dgtbcNote.MappingName = "note";
			dgtbcNote.HeaderText = "Anteckning";
			dgtbcNote.NullText = "";

			System.Windows.Forms.DataGridTextBoxColumn dgtbcDateTime = new DataGridTextBoxColumn();
			dgtbcDateTime.Width = 160;
			dgtbcDateTime.MappingName = "visitdatetime";
			dgtbcDateTime.HeaderText = "Datum och tid";

			
			tsNotes.GridColumnStyles.Add(dgtbcDateTime);
			tsNotes.GridColumnStyles.Add(dgtbcNote);
			tsNotes.GridColumnStyles.Add(dgbcSigned);

			dgNotes.TableStyles.Add(tsNotes);

			System.Windows.Forms.CurrencyManager cm = (CurrencyManager)BindingContext[dgNotes.DataSource,dgNotes.DataMember];
			System.Data.DataView dv = (System.Data.DataView) cm.List;
			dv.Sort = "visitdatetime DESC";

		
		}

		private void SetupDataGridJoined()
		{
			//Setup datagridtablestyle for Joined table


			tsJoined = new DataGridTableStyle();
			tsJoined.MappingName = "Joined";

			System.Windows.Forms.DataGridTextBoxColumn dgtbcDateTime = new DataGridTextBoxColumn();
			dgtbcDateTime.Width = 160;
			dgtbcDateTime.MappingName = "visitdatetime";
			dgtbcDateTime.HeaderText = "Datum och tid";

			System.Windows.Forms.DataGridTextBoxColumn dgtbcSurname = new DataGridTextBoxColumn();
			dgtbcSurname.Width = 120;
			dgtbcSurname.MappingName = "surname";
			dgtbcSurname.HeaderText = "Efternamn";

			System.Windows.Forms.DataGridTextBoxColumn dgtbcFirstname = new DataGridTextBoxColumn();
			dgtbcFirstname.Width = 120;
			dgtbcFirstname.MappingName = "firstname";
			dgtbcFirstname.HeaderText = "Förnamn";

			System.Windows.Forms.DataGridTextBoxColumn dgtbcPersonNumber = new DataGridTextBoxColumn();
			dgtbcPersonNumber.Width = 100;
			dgtbcPersonNumber.MappingName = "personnumber";
			dgtbcPersonNumber.HeaderText = "Personnummer";

			System.Windows.Forms.DataGridTextBoxColumn dgtbcNote = new DataGridTextBoxColumn();
			dgtbcNote.Width = 350;
			dgtbcNote.MappingName = "note";
			dgtbcNote.HeaderText = "Anteckning";

			tsJoined.GridColumnStyles.Add(dgtbcDateTime);
			tsJoined.GridColumnStyles.Add(dgtbcSurname);
			tsJoined.GridColumnStyles.Add(dgtbcFirstname);
			tsJoined.GridColumnStyles.Add(dgtbcPersonNumber);
			tsJoined.GridColumnStyles.Add(dgtbcNote);

			dgNotes.TableStyles.Add(tsJoined);
		}
		
		#endregion

		#region Setup combobox
		private void SetupComboBoxShowFiltered()
		{
			cmbShowFiltered.Items.Add("Visa dagens");
			cmbShowFiltered.Items.Add("Visa gårdagens");
			cmbShowFiltered.Items.Add("Visa osignerade");
			cmbShowFiltered.Items.Add("Visa tomma");

			cmbShowFiltered.SelectedIndexChanged +=new EventHandler(cmbShowFiltered_SelectedIndexChanged);

		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mainMenu1 = new System.Windows.Forms.MainMenu();
			this.menuFile = new System.Windows.Forms.MenuItem();
			this.menuClose = new System.Windows.Forms.MenuItem();
			this.menuTools = new System.Windows.Forms.MenuItem();
			this.menuSettings = new System.Windows.Forms.MenuItem();
			this.menuPrimula = new System.Windows.Forms.MenuItem();
			this.menuBackup = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menyAddDiagnosisNumber = new System.Windows.Forms.MenuItem();
			this.menuPrint = new System.Windows.Forms.MenuItem();
			this.menuPrintReciet = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuPreviewJournal = new System.Windows.Forms.MenuItem();
			this.menuPrintJournal = new System.Windows.Forms.MenuItem();
			this.menuHelp = new System.Windows.Forms.MenuItem();
			this.menuHelpcontents = new System.Windows.Forms.MenuItem();
			this.menuAbout = new System.Windows.Forms.MenuItem();
			this.tabJournal = new System.Windows.Forms.TabPage();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.btnPrintJournal = new System.Windows.Forms.Button();
			this.btnChangePatient = new System.Windows.Forms.Button();
			this.btnNewPatient = new System.Windows.Forms.Button();
			this.txtSearchPatient = new System.Windows.Forms.TextBox();
			this.dgPatients = new CustomDataGrid.MyDataGrid();
			this.dgNotes = new CustomDataGrid.MyDataGrid();
			this.btnSign = new System.Windows.Forms.Button();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.cmbShowFiltered = new System.Windows.Forms.ComboBox();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.btnNewNote = new System.Windows.Forms.Button();
			this.btnPrintReciet = new System.Windows.Forms.Button();
			this.groupBox6 = new System.Windows.Forms.GroupBox();
			this.btnClearSearchPatient = new System.Windows.Forms.Button();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabJournal.SuspendLayout();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgPatients)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgNotes)).BeginInit();
			this.groupBox4.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.groupBox6.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuFile,
																					  this.menuTools,
																					  this.menuPrint,
																					  this.menuHelp});
			// 
			// menuFile
			// 
			this.menuFile.Index = 0;
			this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuClose});
			this.menuFile.Text = "Arkiv";
			// 
			// menuClose
			// 
			this.menuClose.Index = 0;
			this.menuClose.Text = "Avsluta";
			this.menuClose.Click += new System.EventHandler(this.menuClose_Click);
			// 
			// menuTools
			// 
			this.menuTools.Index = 1;
			this.menuTools.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuSettings,
																					  this.menuPrimula,
																					  this.menuBackup,
																					  this.menuItem1,
																					  this.menuItem2});
			this.menuTools.Text = "Verktyg";
			// 
			// menuSettings
			// 
			this.menuSettings.Index = 0;
			this.menuSettings.Text = "Inställningar...";
			this.menuSettings.Click += new System.EventHandler(this.menuSettings_Click);
			// 
			// menuPrimula
			// 
			this.menuPrimula.Index = 1;
			this.menuPrimula.Text = "Primula...";
			this.menuPrimula.Click += new System.EventHandler(this.menuPrimula_Click);
			// 
			// menuBackup
			// 
			this.menuBackup.Index = 2;
			this.menuBackup.Text = "Säkerhetskopiera...";
			this.menuBackup.Click += new System.EventHandler(this.menuBackup_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 3;
			this.menuItem1.Text = "Statistik...";
			this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 4;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menyAddDiagnosisNumber});
			this.menuItem2.Text = "Diagnosnummer";
			// 
			// menyAddDiagnosisNumber
			// 
			this.menyAddDiagnosisNumber.Index = 0;
			this.menyAddDiagnosisNumber.Text = "Lägg till...";
			this.menyAddDiagnosisNumber.Click += new System.EventHandler(this.menyAddDiagnosisNumber_Click);
			// 
			// menuPrint
			// 
			this.menuPrint.Index = 2;
			this.menuPrint.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuPrintReciet,
																					  this.menuItem5,
																					  this.menuPreviewJournal,
																					  this.menuPrintJournal});
			this.menuPrint.Text = "Utskrifter";
			// 
			// menuPrintReciet
			// 
			this.menuPrintReciet.Index = 0;
			this.menuPrintReciet.Text = "Skriv ut kvitto...";
			this.menuPrintReciet.Click += new System.EventHandler(this.menuPrintReciet_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 1;
			this.menuItem5.Text = "-";
			// 
			// menuPreviewJournal
			// 
			this.menuPreviewJournal.Index = 2;
			this.menuPreviewJournal.Text = "Förhandsgranska journal...";
			this.menuPreviewJournal.Click += new System.EventHandler(this.menuPreviewJournal_Click);
			// 
			// menuPrintJournal
			// 
			this.menuPrintJournal.Index = 3;
			this.menuPrintJournal.Text = "Skriv ut journal...";
			this.menuPrintJournal.Click += new System.EventHandler(this.menuPrintJournal_Click);
			// 
			// menuHelp
			// 
			this.menuHelp.Enabled = false;
			this.menuHelp.Index = 3;
			this.menuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuHelpcontents,
																					 this.menuAbout});
			this.menuHelp.Text = "Hjälp";
			// 
			// menuHelpcontents
			// 
			this.menuHelpcontents.Index = 0;
			this.menuHelpcontents.Text = "Hjälp";
			// 
			// menuAbout
			// 
			this.menuAbout.Index = 1;
			this.menuAbout.Text = "Om...";
			// 
			// tabJournal
			// 
			this.tabJournal.BackColor = System.Drawing.Color.Gainsboro;
			this.tabJournal.Controls.Add(this.groupBox3);
			this.tabJournal.Controls.Add(this.txtSearchPatient);
			this.tabJournal.Controls.Add(this.dgPatients);
			this.tabJournal.Controls.Add(this.dgNotes);
			this.tabJournal.Controls.Add(this.btnSign);
			this.tabJournal.Controls.Add(this.groupBox4);
			this.tabJournal.Controls.Add(this.groupBox5);
			this.tabJournal.Controls.Add(this.groupBox6);
			this.tabJournal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tabJournal.Location = new System.Drawing.Point(4, 27);
			this.tabJournal.Name = "tabJournal";
			this.tabJournal.Size = new System.Drawing.Size(968, 617);
			this.tabJournal.TabIndex = 1;
			this.tabJournal.Text = "Journalhantering";
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.btnPrintJournal);
			this.groupBox3.Controls.Add(this.btnChangePatient);
			this.groupBox3.Controls.Add(this.btnNewPatient);
			this.groupBox3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.groupBox3.Location = new System.Drawing.Point(816, 24);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(144, 232);
			this.groupBox3.TabIndex = 15;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Hantera patienter";
			// 
			// btnPrintJournal
			// 
			this.btnPrintJournal.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnPrintJournal.Location = new System.Drawing.Point(16, 160);
			this.btnPrintJournal.Name = "btnPrintJournal";
			this.btnPrintJournal.Size = new System.Drawing.Size(120, 40);
			this.btnPrintJournal.TabIndex = 22;
			this.btnPrintJournal.Text = "Skriv ut journal...";
			this.btnPrintJournal.Click += new System.EventHandler(this.btnPrintJournal_Click);
			// 
			// btnChangePatient
			// 
			this.btnChangePatient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnChangePatient.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnChangePatient.Location = new System.Drawing.Point(16, 104);
			this.btnChangePatient.Name = "btnChangePatient";
			this.btnChangePatient.Size = new System.Drawing.Size(120, 32);
			this.btnChangePatient.TabIndex = 12;
			this.btnChangePatient.Text = "Ändra upgift...";
			this.btnChangePatient.Click += new System.EventHandler(this.btnChangePatient_Click);
			// 
			// btnNewPatient
			// 
			this.btnNewPatient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnNewPatient.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnNewPatient.Location = new System.Drawing.Point(16, 48);
			this.btnNewPatient.Name = "btnNewPatient";
			this.btnNewPatient.Size = new System.Drawing.Size(120, 32);
			this.btnNewPatient.TabIndex = 11;
			this.btnNewPatient.Text = "Ny patient...";
			this.btnNewPatient.Click += new System.EventHandler(this.btnNewPatient_Click);
			// 
			// txtSearchPatient
			// 
			this.txtSearchPatient.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtSearchPatient.Location = new System.Drawing.Point(16, 304);
			this.txtSearchPatient.Name = "txtSearchPatient";
			this.txtSearchPatient.Size = new System.Drawing.Size(224, 26);
			this.txtSearchPatient.TabIndex = 10;
			this.txtSearchPatient.Text = "";
			this.txtSearchPatient.TextChanged += new System.EventHandler(this.txtSearchPatient_TextChanged);
			// 
			// dgPatients
			// 
			this.dgPatients.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.dgPatients.DataMember = "";
			this.dgPatients.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dgPatients.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgPatients.Location = new System.Drawing.Point(0, 0);
			this.dgPatients.Name = "dgPatients";
			this.dgPatients.ReadOnly = true;
			this.dgPatients.Size = new System.Drawing.Size(808, 272);
			this.dgPatients.TabIndex = 6;
			this.dgPatients.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgPatients_MouseDown);
			this.dgPatients.SizeChanged += new System.EventHandler(this.dgPatients_SizeChanged);
			this.dgPatients.DoubleClick += new System.EventHandler(this.dgPatients_DoubleClick);
			this.dgPatients.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgPatients_MouseUp);
			// 
			// dgNotes
			// 
			this.dgNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.dgNotes.DataMember = "";
			this.dgNotes.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dgNotes.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgNotes.Location = new System.Drawing.Point(0, 344);
			this.dgNotes.Name = "dgNotes";
			this.dgNotes.Size = new System.Drawing.Size(968, 280);
			this.dgNotes.TabIndex = 0;
			this.dgNotes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgNotes_MouseDown);
			this.dgNotes.SizeChanged += new System.EventHandler(this.dgNotes_SizeChanged);
			this.dgNotes.DoubleClick += new System.EventHandler(this.dgNotes_DoubleClick);
			this.dgNotes.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgNotes_MouseUp);
			// 
			// btnSign
			// 
			this.btnSign.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnSign.Location = new System.Drawing.Point(464, 296);
			this.btnSign.Name = "btnSign";
			this.btnSign.Size = new System.Drawing.Size(88, 32);
			this.btnSign.TabIndex = 1;
			this.btnSign.Text = "Signera";
			this.btnSign.Click += new System.EventHandler(this.btnSign_Click);
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.Controls.Add(this.cmbShowFiltered);
			this.groupBox4.Location = new System.Drawing.Point(728, 280);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(232, 56);
			this.groupBox4.TabIndex = 18;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Filtrera anteckningar";
			// 
			// cmbShowFiltered
			// 
			this.cmbShowFiltered.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cmbShowFiltered.Location = new System.Drawing.Point(16, 24);
			this.cmbShowFiltered.Name = "cmbShowFiltered";
			this.cmbShowFiltered.Size = new System.Drawing.Size(200, 24);
			this.cmbShowFiltered.TabIndex = 0;
			this.cmbShowFiltered.Text = "Välj filter här...";
			// 
			// groupBox5
			// 
			this.groupBox5.Controls.Add(this.btnNewNote);
			this.groupBox5.Controls.Add(this.btnPrintReciet);
			this.groupBox5.Location = new System.Drawing.Point(296, 280);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(416, 56);
			this.groupBox5.TabIndex = 19;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Hantera anteckningar";
			// 
			// btnNewNote
			// 
			this.btnNewNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.btnNewNote.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnNewNote.Location = new System.Drawing.Point(24, 16);
			this.btnNewNote.Name = "btnNewNote";
			this.btnNewNote.Size = new System.Drawing.Size(136, 32);
			this.btnNewNote.TabIndex = 2;
			this.btnNewNote.Text = "Ny anteckning...";
			this.btnNewNote.Click += new System.EventHandler(this.btnNewNote_Click);
			// 
			// btnPrintReciet
			// 
			this.btnPrintReciet.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnPrintReciet.Location = new System.Drawing.Point(264, 16);
			this.btnPrintReciet.Name = "btnPrintReciet";
			this.btnPrintReciet.Size = new System.Drawing.Size(136, 32);
			this.btnPrintReciet.TabIndex = 21;
			this.btnPrintReciet.Text = "Skriv ut kvitto...";
			this.btnPrintReciet.Click += new System.EventHandler(this.btnPrintReciet_Click);
			// 
			// groupBox6
			// 
			this.groupBox6.Controls.Add(this.btnClearSearchPatient);
			this.groupBox6.Location = new System.Drawing.Point(8, 280);
			this.groupBox6.Name = "groupBox6";
			this.groupBox6.Size = new System.Drawing.Size(280, 56);
			this.groupBox6.TabIndex = 20;
			this.groupBox6.TabStop = false;
			this.groupBox6.Text = "Sök patient";
			// 
			// btnClearSearchPatient
			// 
			this.btnClearSearchPatient.Location = new System.Drawing.Point(248, 24);
			this.btnClearSearchPatient.Name = "btnClearSearchPatient";
			this.btnClearSearchPatient.Size = new System.Drawing.Size(24, 24);
			this.btnClearSearchPatient.TabIndex = 0;
			this.btnClearSearchPatient.Text = "---";
			this.btnClearSearchPatient.Click += new System.EventHandler(this.btnClearSearchPatient_Click);
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabJournal);
			this.tabControl.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.tabControl.ItemSize = new System.Drawing.Size(92, 23);
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(976, 648);
			this.tabControl.TabIndex = 0;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Gainsboro;
			this.ClientSize = new System.Drawing.Size(976, 688);
			this.Controls.Add(this.tabControl);
			this.Menu = this.mainMenu1;
			this.MinimumSize = new System.Drawing.Size(984, 722);
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Doris Ruberg REHAB AB";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.tabJournal.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgPatients)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgNotes)).EndInit();
			this.groupBox4.ResumeLayout(false);
			this.groupBox5.ResumeLayout(false);
			this.groupBox6.ResumeLayout(false);
			this.tabControl.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Handle patient datagrid and patient object update
		private void dgPatients_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (dgNotes.DataMember != "Patients.PatientNotes")
				dgNotes.DataMember = "Patients.PatientNotes";

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

		private void dgPatients_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			System.Drawing.Point pt = new Point(e.X, e.Y); 
			DataGrid dgSender = (DataGrid) sender;
 
			DataGrid.HitTestInfo hti = dgSender.HitTest(pt); 
 
			if(hti.Type == DataGrid.HitTestType.Cell) 
 
			{ 
				
				dgSender.CurrentCell = new DataGridCell(hti.Row, hti.Column); 
				dgSender.Select(hti.Row); 

				
				//When a new patient is selected, the notes datagrid needs to be sorted
				System.Windows.Forms.CurrencyManager cm = (CurrencyManager)BindingContext[dgNotes.DataSource,dgNotes.DataMember];
				System.Data.DataView dv = (System.Data.DataView) cm.List;
				dv.Sort = "visitdatetime DESC";

				UpdatePatient(dgPatients);
				
			} 
		
		}

		private void dgPatients_DoubleClick(object sender, System.EventArgs e)
		{
			Form2 dlgPatient = new Form2(database, ref patient);
			if (dlgPatient.ShowDialog() == DialogResult.OK)
			{
				database.Update(patient);
				UpdatePatient(dgPatients);
			}

			dlgPatient.Dispose();
		
		}


		private void UpdatePatient(DataGrid dataGrid)
		{
			System.Windows.Forms.CurrencyManager cm = (CurrencyManager)BindingContext[dataGrid.DataSource,dataGrid.DataMember];
			System.Data.DataView dv = (System.Data.DataView) cm.List;
			
			selectedPatientRow = dv[cm.Position];

			//Create the corresponding Patient object
			patient = database.CreatePatient(selectedPatientRow.Row, null);
			
			Debug.WriteLine("Current patient: id=" + patient.Id + ", " + patient.Firstname + " " + patient.Surname);

			Utils.AutoSizeDataGrid(new string[]{"note"}, dgNotes, this.BindingContext, 0);
		}

		
		#endregion

		#region Handle note datagrid and note object update
		private void dgNotes_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
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

		private void dgNotes_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			System.Drawing.Point pt = new Point(e.X, e.Y); 
			DataGrid dgSender = (DataGrid) sender;
 
			DataGrid.HitTestInfo hti = dgSender.HitTest(pt); 
 
			if(hti.Type == DataGrid.HitTestType.Cell) 
 
			{ 
				dgSender.CurrentCell = new DataGridCell(hti.Row, hti.Column); 
				dgSender.Select(hti.Row); 
				
				UpdateNote();
				if (dgSender.DataMember == "Joined")
					UpdatePatient(dgNotes);
				
			} 
		
		}


		private void dgNotes_DoubleClick(object sender, System.EventArgs e)
		{
			
			Form3 dlgNote = new Form3(database, ref note, ref patient);

			if (dlgNote.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				database.Update(note);
			}

			dlgNote.Dispose();
		
		}

		private void UpdateNote()
		{
			System.Windows.Forms.CurrencyManager cm = (CurrencyManager)BindingContext[dgNotes.DataSource,dgNotes.DataMember];
			System.Data.DataView dv = (System.Data.DataView) cm.List;
				
			if (dv.Count > 0)
			{
				note = database.CreateNote(dv[dgNotes.CurrentRowIndex].Row);
				
				//Update controls
				if (note.Signed)
					btnSign.Enabled = false;
				else
					btnSign.Enabled = true;
				
				Debug.WriteLine("Current note: id=" + note.Id + ", " + note.JournalNote);

			}
		}
		#endregion

		#region Handle menu
		private void menuPrimula_Click(object sender, System.EventArgs e)
		{
			Primula primulaForm = new Primula(database);

			primulaForm.ShowDialog();
		}

		private void menuSettings_Click(object sender, System.EventArgs e)
		{
			Settings dlgSettings = new Settings();
			dlgSettings.ShowDialog();

			dlgSettings.Dispose();
		}

		private void menuBackup_Click(object sender, System.EventArgs e)
		{
			Backup dlgBackup = new Backup();
			dlgBackup.ShowDialog();

			dlgBackup.Dispose();
		}

		private void menuClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			Statistics dlgStatistics = new Statistics(database);
			dlgStatistics.ShowDialog();

			dlgStatistics.Dispose();
		}

		private void menyAddDiagnosisNumber_Click(object sender, System.EventArgs e)
		{
			AddDiagnosisNumber dlgAddDiagnosisNumber = new AddDiagnosisNumber(database);
			
			dlgAddDiagnosisNumber.ShowDialog();

			dlgAddDiagnosisNumber.Dispose();
		}

		#endregion

		#region Handle buttons, textbox and combobox
		private void btnClearSearchPatient_Click(object sender, System.EventArgs e)
		{
			txtSearchPatient.ResetText();
			
		}

		private void btnChangePatient_Click(object sender, System.EventArgs e)
		{
			if (patient != null)
			{
				Form2 dlgPatient = new Form2(database, ref patient);
				if (dlgPatient.ShowDialog() == DialogResult.OK)
				{
					database.Update(patient);
					//TODO: Select the patient
					UpdatePatient(dgPatients);
				}

				dlgPatient.Dispose();
			}
			else
				MessageBox.Show("Välj en patient först", "Kunde inte ändra patientuppgift");
		}

		private void btnSign_Click(object sender, System.EventArgs e)
		{
			if (note != null)
			{
				note.Signed = true;
				note.SignedDateTime = System.DateTime.Now;
				database.Update(note);
			}
			else
				MessageBox.Show("Du måste välja en anteckning att signera först", "Det gick inte att signera anteckning");
		}

		private void btnNewNote_Click(object sender, System.EventArgs e)
		{
			if ((patient != null) && (database.IsAlreadyExsisting(patient)) && (database.IsPatientIdValid(patient.Id)))
            {
                bool bUpdateExistingNote = false; 
				note = new Note(patient.Id);

				if (database.IsAlreadyExsisting(note))
				{
					//Checks so that the user by mistake does not create two notes for the same patient, the same day
					DialogResult dialogResult = MessageBox.Show("Det har redan skapats en anteckning för den här patienten idag.\nVill du fortsätta att skapa en ny anteckning för patienten?\n\nOm du vill fortsätta med att skapa en ny anteckning tryck Ja.\nFör att öppna den redan skapade anteckningen tryck Nej.\nTryck Avbryt för att avbryta.", "Meddelande", MessageBoxButtons.YesNoCancel);
					switch (dialogResult)
					{
						case DialogResult.Cancel:
							return;
						case DialogResult.No:
							note = database.ReturnAlreadyExsisting(note);
                            bUpdateExistingNote = true;
							break;
					}
				}

				if (note == null)
					throw new Exception("Något fel har inträffat, en anteckning som refererar till null försökte användas.");

				note.DiagnosisArray = database.FindLatestVisitNote(selectedPatientRow);
				
				Form3 dlgNewNote = new Form3(database, ref note, ref patient);

				if (dlgNewNote.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
                    if (!database.IsPatientIdValid(patient.Id))
                        throw new Exception("Något fel har inträffat, en anteckning som refererar till en patient som inte finns försökte skapas.");

                    if (bUpdateExistingNote)
                        database.Update(note);
                    else
                    {
                        database.Add(note);
                        //TODO: Select the note
                        note = null;
                        System.Windows.Forms.CurrencyManager cm = (CurrencyManager)BindingContext[dgNotes.DataSource, dgNotes.DataMember];
                        System.Data.DataView dv = (System.Data.DataView)cm.List;
                        dv.Sort = "visitdatetime DESC";
                    }
				}
				
				dlgNewNote.Dispose();
			}
			else
				MessageBox.Show("Välj en patient först", "Kunde inte skapa ny anteckning");
			
		}

		private void btnNewPatient_Click(object sender, System.EventArgs e)
		{
			Patient newPatient = new Patient();
			Form2 dlgNewPatient = new Form2(database, ref newPatient);

			if (dlgNewPatient.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				database.Add(newPatient);
				//TODO: Select the new patient and sort the datagrid
				patient = null;
			}

			dlgNewPatient.Dispose();
		}

		private void txtSearchPatient_TextChanged(object sender, System.EventArgs e)
		{
			//Select the patient(s) that match the search criteria
			TextBox txtBox = (TextBox)sender;
			System.Windows.Forms.CurrencyManager cm = (CurrencyManager)BindingContext[dgPatients.DataSource,dgPatients.DataMember];
			DataView dv = (DataView) cm.List;
			dv.RowFilter = "surname LIKE '" + txtBox.Text + "*'";
			if (dv.Count > 0)
			{
				dgPatients.Select(0);
				UpdatePatient(dgPatients);

				//When a new patient is selected, the notes datagrid needs to be sorted
	
				if (dgNotes.DataMember != "Patients.PatientNotes")
					dgNotes.DataMember = "Patients.PatientNotes";

				cm = (CurrencyManager)BindingContext[dgNotes.DataSource,dgNotes.DataMember];
				dv = (System.Data.DataView) cm.List;
				dv.Sort = "visitdatetime DESC";

				Utils.AutoSizeDataGrid(new string[]{"note"}, dgNotes, this.BindingContext, 0);
				Utils.AutoSizeDataGrid(null, dgPatients, this.BindingContext, 0);
			}
		}

		private void cmbShowFiltered_SelectedIndexChanged(object sender, EventArgs e)
		{
			dgNotes.DataMember = "Joined";
			dgNotes.RowHeadersVisible = false;
			System.Windows.Forms.CurrencyManager cm = (CurrencyManager)BindingContext[dgNotes.DataSource,dgNotes.DataMember];
			System.Data.DataView dv = (System.Data.DataView) cm.List;
			
		
			switch ((string)cmbShowFiltered.SelectedItem)
			{
				case "Visa dagens" :
					string [] todayDate = System.DateTime.Now.GetDateTimeFormats('d', new System.Globalization.CultureInfo("en-US", true));
					dv.RowFilter = "SUBSTRING(visitdatetime,1,10) = #" + todayDate[3] + "#";
					break;
				case "Visa gårdagens" :
					System.DateTime yeasterday = System.DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
					string [] yeasterdayDate = yeasterday.GetDateTimeFormats('d', new System.Globalization.CultureInfo("en-US", true));
					dv.RowFilter = "SUBSTRING(visitdatetime,1,10) = #" + yeasterdayDate[3] + "#";
					break;
				case "Visa osignerade" :
					dv.RowFilter = "signed = false";
					break;
				case "Visa tomma" :
					dv.RowFilter = "note = ''";
					break;
			}
		
			dv.Sort = "visitdatetime DESC";
			Utils.AutoSizeDataGrid(new string[]{"note"}, dgNotes, this.BindingContext, 1);
		}

		#endregion
		
		#region Setup and handle printing
		private void SetupPrinting(string filename)
		{
			printReceipt = new PrintReceipt(filename);
			printJournal = new PrintJournal();

			printDialog = new System.Windows.Forms.PrintDialog();
			printPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
			
			printPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
			printPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			printPreviewDialog.ClientSize = new System.Drawing.Size(1024,600);
			
			printPreviewDialog.Enabled = true;
			//printPreviewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog.Icon")));
			printPreviewDialog.Location = new System.Drawing.Point(254, 17);
			printPreviewDialog.MinimumSize = new System.Drawing.Size(500, 400);
			printPreviewDialog.Name = "Förhandsgranskning";
			printPreviewDialog.TransparencyKey = System.Drawing.Color.Empty;
			printPreviewDialog.Visible = false;
		}


		
		
		private void btnPrintReciet_Click(object sender, System.EventArgs e)
		{
			if (patient != null)
			{
				System.Data.DataSet dsPatientNotes = new System.Data.DataSet();
				dsPatientNotes.Merge(selectedPatientRow.Row.GetChildRows("PatientNotes"));
				if (dsPatientNotes.Tables.Count > 0)
				{
					PrintReceiptDlg printReceiptDlg = new PrintReceiptDlg(database, dsPatientNotes, patient);
					printReceiptDlg.ShowDialog();
				}
				else
					MessageBox.Show("Patienten har inte några besök registrerade", "Kunde ej skriva ut kvitto");
				
				
			}
			else
				MessageBox.Show("Välj en patient att skriva ut kvitto för", "Kunde ej skriva ut kvitto");
			
		}


		private void menuPrintReciet_Click(object sender, System.EventArgs e)
		{
			if (patient != null)
			{
				System.Data.DataSet dsPatientNotes = new System.Data.DataSet();
				dsPatientNotes.Merge(selectedPatientRow.Row.GetChildRows("PatientNotes"));
				PrintReceiptDlg printReceiptDlg = new PrintReceiptDlg(database, dsPatientNotes, patient);
				printReceiptDlg.ShowDialog();

				
			}
			else
				MessageBox.Show("Välj en patient att skriva ut kvitto för", "Kunde ej skriva ut kvitto");
		
		}

		private void menuPreviewJournal_Click(object sender, System.EventArgs e)
		{
			if (patient != null && selectedPatientRow != null)
			{
				//TODO: Should only be possible to print visitnotes??
				DataSet dsPrintJournal = new DataSet();
				dsPrintJournal.Merge(selectedPatientRow.Row.GetChildRows("PatientNotes"));
				dsPrintJournal.Tables[0].DefaultView.Sort = "visitdatetime DESC";
				
				printJournal.SetContent(patient, dsPrintJournal.Tables[0].DefaultView);
				
				printPreviewDialog.Document = printJournal.PrintDocument;
				
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
			else
				MessageBox.Show("Välj en patient att skriva ut journal för", "Kunde ej skriva ut journal");
			
		}

		private void menuPrintJournal_Click(object sender, System.EventArgs e)
		{
			if (patient != null && selectedPatientRow != null)
			{
				//TODO: Should only be possible to print treatmentnotes
				DataSet dsPrintJournal = new DataSet();
				dsPrintJournal.Merge(selectedPatientRow.Row.GetChildRows("PatientNotes"));
				dsPrintJournal.Tables[0].DefaultView.Sort = "visitdatetime DESC";
				
				printJournal.SetContent(patient, dsPrintJournal.Tables[0].DefaultView);
				printDialog.Document = printJournal.PrintDocument;
				if (printDialog.ShowDialog() == DialogResult.OK)
					printJournal.Print();

				
			}
			else
				MessageBox.Show("Välj en patient att skriva ut journal för", "Kunde ej skriva ut journal");
		
		}

		private void btnPrintJournal_Click(object sender, System.EventArgs e)
		{
			if (patient != null && selectedPatientRow != null)
			{
				//TODO: Should only be possible to print treatmentnotes
				DataSet dsPrintJournal = new DataSet();
				dsPrintJournal.Merge(selectedPatientRow.Row.GetChildRows("PatientNotes"));
				dsPrintJournal.Tables[0].DefaultView.Sort = "visitdatetime DESC";
				
				printJournal.SetContent(patient, dsPrintJournal.Tables[0].DefaultView);
				
				printDialog.Document = printJournal.PrintDocument;
				if (printDialog.ShowDialog() == DialogResult.OK)
					printJournal.Print();

				
			}
			else
				MessageBox.Show("Välj en patient att skriva ut journal för", "Kunde ej skriva ut journal");

		
		}

		#endregion

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void dgNotes_SizeChanged(object sender, System.EventArgs e)
		{
			if (dgNotes.DataMember == "Joined")
				Utils.AutoSizeDataGrid(new string[]{"note"}, dgNotes, this.BindingContext, 1);
			else
				Utils.AutoSizeDataGrid(new string[]{"note"}, dgNotes, this.BindingContext, 0);
		}

		private void dgPatients_SizeChanged(object sender, System.EventArgs e)
		{
			Utils.AutoSizeDataGrid(null, dgPatients, this.BindingContext, 0);
		}
		
	}
}
