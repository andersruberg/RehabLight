using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Data;
using System.Text;


namespace RehabLight
{
	/// <summary>
	/// Summary description for Form3.
	/// </summary>
	public class Form3 : System.Windows.Forms.Form
	{
		#region Variables
		private System.Windows.Forms.TextBox txtNote;
		private System.Windows.Forms.TreeView treeView;
	
		private System.Windows.Forms.DataGridTableStyle tsDiagnosis;
		private PreviousNotes dlgPreviousNotes;
		
		private Diagnosis selectedDiagnosis;
		private TreeNode root;
		private Database database;
		private Patient patient;
		private bool freecardHasExpired;

		private Note note;
		private System.Windows.Forms.Button btnSaveNote;
		private System.Windows.Forms.Button btnSign;
		private System.Windows.Forms.Button btnChange;
		private System.Windows.Forms.TextBox txtPersonnumber;
		private System.Windows.Forms.TextBox txtFirstname;
		private System.Windows.Forms.GroupBox boxPatient;
		private System.Windows.Forms.Label lblPersonNumber;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.TextBox txtSurname;
		private System.Windows.Forms.Label label1;
		
		private const int WM_LBUTTONDOWN = 0x0201;
		

		private const string patientCharge = "J";
		private const string freeCardCharge = "F";
		private const string militaryCharge = "V";
		private const string childCharge = "B";
		private const string otherCharge = "A";
		private System.Windows.Forms.GroupBox grpVisitDate;
		private System.Windows.Forms.DateTimePicker dtpVisitDate;
		private System.Windows.Forms.DateTimePicker dtpVisitTime;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox txtPatientFee;
		private System.Windows.Forms.GroupBox grpDiagnosisNumber;
		private CustomDataGrid.MyDataGrid dgDiagnosis;
		private System.Windows.Forms.TextBox txtDiagnosis;
		private System.Windows.Forms.Button btnAddDiagnosis;
		private System.Windows.Forms.Button btnRemoveDiagnosis;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cmbVisitType;
		private System.Windows.Forms.ComboBox cmbPatientCharge;
		private System.Windows.Forms.ListBox lstDiagnosis;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnShowPreviousNotes;
		private System.Windows.Forms.Label lblFreecardExpiry;
		private System.Windows.Forms.Label lblCurrancy;
		private System.Windows.Forms.Label lblVisitType;
		private System.Windows.Forms.Label lblPatientFee;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox chkNoVisit;
		private System.Windows.Forms.TextBox txtNoVisitFee;
		private System.Windows.Forms.Label lblNoVisitFee;
		private System.Windows.Forms.Button btnDeleteNote;
		private const int WM_LBUTTONUP = 0x0202;
		[DllImport("User32.dll",EntryPoint="SendMessage")]
		private static extern int SendMessage(IntPtr hWnd,int Msg,int wParam,int lParam);

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region Initiation
		public Form3(Database aDatabase, ref Note aNote, ref Patient aPatient)
		{
			InitializeComponent();
		
			database = aDatabase;

			note = aNote;
			patient = aPatient;

			dgDiagnosis.SetDataBinding(database.DsMaster, "Diagnosis");
			
			
			SetupPatientChargeComboBox();
			SetupVisitTypeComboBox();
			SetupTreeView();
			SetupDataGridDiagnosis();

			
		}

		private void Form3_Load(object sender, System.EventArgs e)
		{
			if (note.VisitDateTime.Ticks == ((long) 0))
			{
				//This is a new note, promt the user for visitdate and time
				
				PromtVisitDateTime dlgPromtVisitDateTime = new PromtVisitDateTime(ref note);
				if (dlgPromtVisitDateTime.ShowDialog() != DialogResult.OK)
				{
					this.Close();
					return;
				}
			}

			cmbPatientCharge.SelectedIndexChanged += new System.EventHandler(this.cmbPatientCharge_SelectedIndexChanged);

			if (!note.VisitNote)
				this.Size = new System.Drawing.Size(1024, 620);

			UpdateForm();
			
			//Simulate leftclick on textbox
			SendMessage(this.txtNote.Handle, WM_LBUTTONDOWN, 0, 0);
			SendMessage(this.txtNote.Handle, WM_LBUTTONUP, 0, 0);
			txtNote.SelectionStart = txtNote.Text.Length;

			Utils.AutoSizeDataGrid(new string[]{"diagnosistext"}, dgDiagnosis, this.BindingContext, 0);
		}
		#endregion
		
		#region DataGrid and TreeView setup
		private void SetupTreeView()
		{
			//Add text for the root
			root = treeView.Nodes.Add(Settings.XnPhraseLibrary.Attributes["description"].Value);

			AddNode(Settings.XnPhraseLibrary, root);

			treeView.ExpandAll();
		}

		private void AddNode(XmlNode inXmlNode, TreeNode inTreeNode)
		{
			TreeNode tNode;
			XmlNodeList nodeList = inXmlNode.ChildNodes;

			foreach(System.Xml.XmlNode xmlNode in nodeList)
			{
				if (xmlNode.Attributes["description"] != null)
				{
					int nodeNumber = 0;
					nodeNumber = inTreeNode.Nodes.Add(new TreeNode(xmlNode.Attributes["description"].Value));
					tNode = inTreeNode.Nodes[nodeNumber];
					AddNode(xmlNode, tNode);
				}

				else 
				{
					inTreeNode.Nodes.Add(new TreeNode(xmlNode.InnerText));
				}

			}

		}

		private void SetupPatientChargeComboBox()
		{
			cmbPatientCharge.DataSource = database.DsMaster;
			cmbPatientCharge.ValueMember = "Charges.chargeid";
			cmbPatientCharge.DisplayMember = "Charges.text";
			cmbPatientCharge.SelectedIndex = 0;
		}

		private void SetupVisitTypeComboBox()
		{
			cmbVisitType.Items.Add("Återbesök");
			cmbVisitType.Items.Add("Nybesök");
			cmbVisitType.SelectedIndex = 0;
		}

		private void SetupDataGridDiagnosis()
		{
			tsDiagnosis = new DataGridTableStyle();
			tsDiagnosis.MappingName = "Diagnosis";

			dgDiagnosis.ReadOnly = true;
			dgDiagnosis.RowHeadersVisible = false;

			Utils.CopyDefaultTableStyle(dgDiagnosis, tsDiagnosis);			

			System.Windows.Forms.DataGridTextBoxColumn dgtbcDiagnosisText = new DataGridTextBoxColumn();
			dgtbcDiagnosisText.Width = 300;
			dgtbcDiagnosisText.MappingName = "diagnosistext";
			dgtbcDiagnosisText.HeaderText = "Diagnos";
			dgtbcDiagnosisText.NullText = "";

			System.Windows.Forms.DataGridTextBoxColumn dgtbcDiagnosisNumber = new DataGridTextBoxColumn();
			dgtbcDiagnosisNumber.Width = 76;
			dgtbcDiagnosisNumber.MappingName = "diagnosisnumber";
			dgtbcDiagnosisNumber.HeaderText = "Diagnosnummer";

			
			tsDiagnosis.GridColumnStyles.Add(dgtbcDiagnosisNumber);
			tsDiagnosis.GridColumnStyles.Add(dgtbcDiagnosisText);

			dgDiagnosis.TableStyles.Add(tsDiagnosis);

			

			System.Windows.Forms.CurrencyManager cm = (CurrencyManager)BindingContext[dgDiagnosis.DataSource,dgDiagnosis.DataMember];
			System.Data.DataView dv = (System.Data.DataView) cm.List;
			dv.Sort = "diagnosistext ASC";
			
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.treeView = new System.Windows.Forms.TreeView();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.btnSaveNote = new System.Windows.Forms.Button();
            this.btnSign = new System.Windows.Forms.Button();
            this.btnChange = new System.Windows.Forms.Button();
            this.txtPersonnumber = new System.Windows.Forms.TextBox();
            this.txtFirstname = new System.Windows.Forms.TextBox();
            this.boxPatient = new System.Windows.Forms.GroupBox();
            this.lblPersonNumber = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtSurname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPatientFee = new System.Windows.Forms.TextBox();
            this.lblFreecardExpiry = new System.Windows.Forms.Label();
            this.grpVisitDate = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpVisitTime = new System.Windows.Forms.DateTimePicker();
            this.dtpVisitDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblCurrancy = new System.Windows.Forms.Label();
            this.grpDiagnosisNumber = new System.Windows.Forms.GroupBox();
            this.lstDiagnosis = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRemoveDiagnosis = new System.Windows.Forms.Button();
            this.btnAddDiagnosis = new System.Windows.Forms.Button();
            this.txtDiagnosis = new System.Windows.Forms.TextBox();
            this.dgDiagnosis = new CustomDataGrid.MyDataGrid();
            this.cmbVisitType = new System.Windows.Forms.ComboBox();
            this.cmbPatientCharge = new System.Windows.Forms.ComboBox();
            this.lblVisitType = new System.Windows.Forms.Label();
            this.lblPatientFee = new System.Windows.Forms.Label();
            this.btnShowPreviousNotes = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkNoVisit = new System.Windows.Forms.CheckBox();
            this.txtNoVisitFee = new System.Windows.Forms.TextBox();
            this.lblNoVisitFee = new System.Windows.Forms.Label();
            this.btnDeleteNote = new System.Windows.Forms.Button();
            this.boxPatient.SuspendLayout();
            this.grpVisitDate.SuspendLayout();
            this.grpDiagnosisNumber.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDiagnosis)).BeginInit();
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView.Location = new System.Drawing.Point(16, 240);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(168, 384);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // txtNote
            // 
            this.txtNote.AcceptsReturn = true;
            this.txtNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNote.Font = new System.Drawing.Font("Arial", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNote.Location = new System.Drawing.Point(192, 240);
            this.txtNote.Multiline = true;
            this.txtNote.Name = "txtNote";
            this.txtNote.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtNote.Size = new System.Drawing.Size(808, 384);
            this.txtNote.TabIndex = 3;
            this.txtNote.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtNote_MouseDown);
            this.txtNote.MouseUp += new System.Windows.Forms.MouseEventHandler(this.txtNote_MouseUp);
            // 
            // btnSaveNote
            // 
            this.btnSaveNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveNote.BackColor = System.Drawing.Color.Gainsboro;
            this.btnSaveNote.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSaveNote.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSaveNote.Location = new System.Drawing.Point(808, 640);
            this.btnSaveNote.Name = "btnSaveNote";
            this.btnSaveNote.Size = new System.Drawing.Size(152, 32);
            this.btnSaveNote.TabIndex = 4;
            this.btnSaveNote.Text = "Spara och stäng";
            this.btnSaveNote.UseVisualStyleBackColor = false;
            this.btnSaveNote.Click += new System.EventHandler(this.btnSaveNote_Click);
            // 
            // btnSign
            // 
            this.btnSign.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSign.BackColor = System.Drawing.Color.Gainsboro;
            this.btnSign.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSign.Location = new System.Drawing.Point(440, 640);
            this.btnSign.Name = "btnSign";
            this.btnSign.Size = new System.Drawing.Size(152, 32);
            this.btnSign.TabIndex = 17;
            this.btnSign.Text = "Signera";
            this.btnSign.UseVisualStyleBackColor = false;
            this.btnSign.Click += new System.EventHandler(this.btnSign_Click);
            // 
            // btnChange
            // 
            this.btnChange.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChange.BackColor = System.Drawing.Color.Gainsboro;
            this.btnChange.Location = new System.Drawing.Point(768, 24);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(184, 24);
            this.btnChange.TabIndex = 0;
            this.btnChange.Text = "Visa/ändra patientuppgifter...";
            this.btnChange.UseVisualStyleBackColor = false;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // txtPersonnumber
            // 
            this.txtPersonnumber.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPersonnumber.Enabled = false;
            this.txtPersonnumber.Location = new System.Drawing.Point(616, 24);
            this.txtPersonnumber.MaxLength = 13;
            this.txtPersonnumber.Name = "txtPersonnumber";
            this.txtPersonnumber.Size = new System.Drawing.Size(104, 22);
            this.txtPersonnumber.TabIndex = 21;
            // 
            // txtFirstname
            // 
            this.txtFirstname.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFirstname.Enabled = false;
            this.txtFirstname.Location = new System.Drawing.Point(328, 24);
            this.txtFirstname.Name = "txtFirstname";
            this.txtFirstname.Size = new System.Drawing.Size(152, 22);
            this.txtFirstname.TabIndex = 20;
            // 
            // boxPatient
            // 
            this.boxPatient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.boxPatient.Controls.Add(this.lblPersonNumber);
            this.boxPatient.Controls.Add(this.lblName);
            this.boxPatient.Controls.Add(this.txtSurname);
            this.boxPatient.Controls.Add(this.label1);
            this.boxPatient.Controls.Add(this.txtPersonnumber);
            this.boxPatient.Controls.Add(this.txtFirstname);
            this.boxPatient.Controls.Add(this.btnChange);
            this.boxPatient.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boxPatient.Location = new System.Drawing.Point(8, 8);
            this.boxPatient.Name = "boxPatient";
            this.boxPatient.Size = new System.Drawing.Size(992, 56);
            this.boxPatient.TabIndex = 22;
            this.boxPatient.TabStop = false;
            this.boxPatient.Text = "Patientuppgifter";
            // 
            // lblPersonNumber
            // 
            this.lblPersonNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPersonNumber.Location = new System.Drawing.Point(504, 24);
            this.lblPersonNumber.Name = "lblPersonNumber";
            this.lblPersonNumber.Size = new System.Drawing.Size(104, 24);
            this.lblPersonNumber.TabIndex = 11;
            this.lblPersonNumber.Text = "Personnummer";
            // 
            // lblName
            // 
            this.lblName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblName.Location = new System.Drawing.Point(256, 24);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(64, 24);
            this.lblName.TabIndex = 9;
            this.lblName.Text = "Förnamn";
            // 
            // txtSurname
            // 
            this.txtSurname.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSurname.Enabled = false;
            this.txtSurname.Location = new System.Drawing.Point(88, 24);
            this.txtSurname.Name = "txtSurname";
            this.txtSurname.Size = new System.Drawing.Size(152, 22);
            this.txtSurname.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(8, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 24);
            this.label1.TabIndex = 19;
            this.label1.Text = "Efternamn";
            // 
            // txtPatientFee
            // 
            this.txtPatientFee.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPatientFee.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPatientFee.Location = new System.Drawing.Point(232, 152);
            this.txtPatientFee.Name = "txtPatientFee";
            this.txtPatientFee.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtPatientFee.Size = new System.Drawing.Size(40, 22);
            this.txtPatientFee.TabIndex = 2;
            this.txtPatientFee.Text = "0";
            // 
            // lblFreecardExpiry
            // 
            this.lblFreecardExpiry.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFreecardExpiry.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFreecardExpiry.Location = new System.Drawing.Point(184, 200);
            this.lblFreecardExpiry.Name = "lblFreecardExpiry";
            this.lblFreecardExpiry.Size = new System.Drawing.Size(136, 32);
            this.lblFreecardExpiry.TabIndex = 33;
            // 
            // grpVisitDate
            // 
            this.grpVisitDate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpVisitDate.Controls.Add(this.label2);
            this.grpVisitDate.Controls.Add(this.dtpVisitTime);
            this.grpVisitDate.Controls.Add(this.dtpVisitDate);
            this.grpVisitDate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpVisitDate.Location = new System.Drawing.Point(8, 80);
            this.grpVisitDate.Name = "grpVisitDate";
            this.grpVisitDate.Size = new System.Drawing.Size(256, 56);
            this.grpVisitDate.TabIndex = 35;
            this.grpVisitDate.TabStop = false;
            this.grpVisitDate.Text = "Datum och tidpunkt för besöket";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(120, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "klockan";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dtpVisitTime
            // 
            this.dtpVisitTime.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpVisitTime.CustomFormat = "HH:mm";
            this.dtpVisitTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpVisitTime.Location = new System.Drawing.Point(176, 24);
            this.dtpVisitTime.Name = "dtpVisitTime";
            this.dtpVisitTime.ShowUpDown = true;
            this.dtpVisitTime.Size = new System.Drawing.Size(64, 22);
            this.dtpVisitTime.TabIndex = 0;
            // 
            // dtpVisitDate
            // 
            this.dtpVisitDate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpVisitDate.CustomFormat = "yyyy-MM-d";
            this.dtpVisitDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpVisitDate.Location = new System.Drawing.Point(16, 24);
            this.dtpVisitDate.Name = "dtpVisitDate";
            this.dtpVisitDate.Size = new System.Drawing.Size(96, 22);
            this.dtpVisitDate.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.Location = new System.Drawing.Point(20, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 23);
            this.label6.TabIndex = 0;
            this.label6.Text = "klockan";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(0, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 23);
            this.label5.TabIndex = 0;
            // 
            // lblCurrancy
            // 
            this.lblCurrancy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrancy.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrancy.Location = new System.Drawing.Point(280, 152);
            this.lblCurrancy.Name = "lblCurrancy";
            this.lblCurrancy.Size = new System.Drawing.Size(16, 16);
            this.lblCurrancy.TabIndex = 36;
            this.lblCurrancy.Text = "kr";
            // 
            // grpDiagnosisNumber
            // 
            this.grpDiagnosisNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDiagnosisNumber.Controls.Add(this.lstDiagnosis);
            this.grpDiagnosisNumber.Controls.Add(this.label3);
            this.grpDiagnosisNumber.Controls.Add(this.btnRemoveDiagnosis);
            this.grpDiagnosisNumber.Controls.Add(this.btnAddDiagnosis);
            this.grpDiagnosisNumber.Controls.Add(this.txtDiagnosis);
            this.grpDiagnosisNumber.Controls.Add(this.dgDiagnosis);
            this.grpDiagnosisNumber.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpDiagnosisNumber.Location = new System.Drawing.Point(320, 72);
            this.grpDiagnosisNumber.Name = "grpDiagnosisNumber";
            this.grpDiagnosisNumber.Size = new System.Drawing.Size(680, 168);
            this.grpDiagnosisNumber.TabIndex = 37;
            this.grpDiagnosisNumber.TabStop = false;
            this.grpDiagnosisNumber.Text = "Diagnosnummer";
            // 
            // lstDiagnosis
            // 
            this.lstDiagnosis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstDiagnosis.ItemHeight = 16;
            this.lstDiagnosis.Location = new System.Drawing.Point(16, 24);
            this.lstDiagnosis.Name = "lstDiagnosis";
            this.lstDiagnosis.Size = new System.Drawing.Size(80, 116);
            this.lstDiagnosis.TabIndex = 35;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(216, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 16);
            this.label3.TabIndex = 34;
            this.label3.Text = "Sök";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRemoveDiagnosis
            // 
            this.btnRemoveDiagnosis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveDiagnosis.Location = new System.Drawing.Point(112, 80);
            this.btnRemoveDiagnosis.Name = "btnRemoveDiagnosis";
            this.btnRemoveDiagnosis.Size = new System.Drawing.Size(80, 24);
            this.btnRemoveDiagnosis.TabIndex = 5;
            this.btnRemoveDiagnosis.Text = "Ta bort ->";
            this.btnRemoveDiagnosis.Click += new System.EventHandler(this.btnRemoveDiagnosis_Click);
            // 
            // btnAddDiagnosis
            // 
            this.btnAddDiagnosis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddDiagnosis.Location = new System.Drawing.Point(112, 40);
            this.btnAddDiagnosis.Name = "btnAddDiagnosis";
            this.btnAddDiagnosis.Size = new System.Drawing.Size(80, 24);
            this.btnAddDiagnosis.TabIndex = 4;
            this.btnAddDiagnosis.Text = "<- Lägg till";
            this.btnAddDiagnosis.Click += new System.EventHandler(this.btnAddDiagnosis_Click);
            // 
            // txtDiagnosis
            // 
            this.txtDiagnosis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDiagnosis.Location = new System.Drawing.Point(264, 136);
            this.txtDiagnosis.Name = "txtDiagnosis";
            this.txtDiagnosis.Size = new System.Drawing.Size(224, 22);
            this.txtDiagnosis.TabIndex = 1;
            this.txtDiagnosis.TextChanged += new System.EventHandler(this.txtDiagnosis_TextChanged);
            // 
            // dgDiagnosis
            // 
            this.dgDiagnosis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgDiagnosis.CaptionVisible = false;
            this.dgDiagnosis.DataMember = "";
            this.dgDiagnosis.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dgDiagnosis.Location = new System.Drawing.Point(216, 16);
            this.dgDiagnosis.Name = "dgDiagnosis";
            this.dgDiagnosis.ParentRowsVisible = false;
            this.dgDiagnosis.ReadOnly = true;
            this.dgDiagnosis.RowHeadersVisible = false;
            this.dgDiagnosis.Size = new System.Drawing.Size(448, 112);
            this.dgDiagnosis.TabIndex = 0;
            this.dgDiagnosis.SizeChanged += new System.EventHandler(this.dgDiagnosis_SizeChanged);
            this.dgDiagnosis.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgDiagnosis_MouseUp);
            // 
            // cmbVisitType
            // 
            this.cmbVisitType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbVisitType.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbVisitType.Location = new System.Drawing.Point(16, 200);
            this.cmbVisitType.Name = "cmbVisitType";
            this.cmbVisitType.Size = new System.Drawing.Size(160, 24);
            this.cmbVisitType.TabIndex = 38;
            // 
            // cmbPatientCharge
            // 
            this.cmbPatientCharge.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPatientCharge.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPatientCharge.Location = new System.Drawing.Point(16, 152);
            this.cmbPatientCharge.Name = "cmbPatientCharge";
            this.cmbPatientCharge.Size = new System.Drawing.Size(200, 24);
            this.cmbPatientCharge.TabIndex = 1;
            this.cmbPatientCharge.Text = "Välj betalning här";
            // 
            // lblVisitType
            // 
            this.lblVisitType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVisitType.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVisitType.Location = new System.Drawing.Point(16, 184);
            this.lblVisitType.Name = "lblVisitType";
            this.lblVisitType.Size = new System.Drawing.Size(104, 16);
            this.lblVisitType.TabIndex = 40;
            this.lblVisitType.Text = "Besökstyp";
            // 
            // lblPatientFee
            // 
            this.lblPatientFee.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPatientFee.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPatientFee.Location = new System.Drawing.Point(16, 136);
            this.lblPatientFee.Name = "lblPatientFee";
            this.lblPatientFee.Size = new System.Drawing.Size(104, 16);
            this.lblPatientFee.TabIndex = 41;
            this.lblPatientFee.Text = "Patientavgift";
            // 
            // btnShowPreviousNotes
            // 
            this.btnShowPreviousNotes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShowPreviousNotes.BackColor = System.Drawing.Color.Gainsboro;
            this.btnShowPreviousNotes.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowPreviousNotes.Location = new System.Drawing.Point(248, 640);
            this.btnShowPreviousNotes.Name = "btnShowPreviousNotes";
            this.btnShowPreviousNotes.Size = new System.Drawing.Size(160, 40);
            this.btnShowPreviousNotes.TabIndex = 42;
            this.btnShowPreviousNotes.Text = "Visa tidigare anteckningar";
            this.btnShowPreviousNotes.UseVisualStyleBackColor = false;
            this.btnShowPreviousNotes.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnShowPreviousNotes_MouseDown);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.Gainsboro;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(808, 680);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(152, 32);
            this.btnCancel.TabIndex = 43;
            this.btnCancel.Text = "Avbryt";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkNoVisit
            // 
            this.chkNoVisit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkNoVisit.Enabled = false;
            this.chkNoVisit.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkNoVisit.Location = new System.Drawing.Point(16, 200);
            this.chkNoVisit.Name = "chkNoVisit";
            this.chkNoVisit.Size = new System.Drawing.Size(200, 32);
            this.chkNoVisit.TabIndex = 44;
            this.chkNoVisit.Text = "Uteblivet besök";
            this.chkNoVisit.Visible = false;
            this.chkNoVisit.CheckedChanged += new System.EventHandler(this.chkNoVisit_CheckedChanged);
            // 
            // txtNoVisitFee
            // 
            this.txtNoVisitFee.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtNoVisitFee.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNoVisitFee.Location = new System.Drawing.Point(232, 208);
            this.txtNoVisitFee.Name = "txtNoVisitFee";
            this.txtNoVisitFee.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txtNoVisitFee.Size = new System.Drawing.Size(40, 22);
            this.txtNoVisitFee.TabIndex = 45;
            this.txtNoVisitFee.Text = "0";
            // 
            // lblNoVisitFee
            // 
            this.lblNoVisitFee.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblNoVisitFee.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNoVisitFee.Location = new System.Drawing.Point(168, 200);
            this.lblNoVisitFee.Name = "lblNoVisitFee";
            this.lblNoVisitFee.Size = new System.Drawing.Size(56, 32);
            this.lblNoVisitFee.TabIndex = 46;
            this.lblNoVisitFee.Text = "Avgift:";
            this.lblNoVisitFee.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDeleteNote
            // 
            this.btnDeleteNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteNote.BackColor = System.Drawing.Color.Gainsboro;
            this.btnDeleteNote.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteNote.Location = new System.Drawing.Point(56, 640);
            this.btnDeleteNote.Name = "btnDeleteNote";
            this.btnDeleteNote.Size = new System.Drawing.Size(160, 40);
            this.btnDeleteNote.TabIndex = 47;
            this.btnDeleteNote.Text = "Ta bort anteckningen";
            this.btnDeleteNote.UseVisualStyleBackColor = false;
            this.btnDeleteNote.Click += new System.EventHandler(this.btnDeleteNote_Click);
            // 
            // Form3
            // 
            this.AcceptButton = this.btnSaveNote;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1016, 721);
            this.Controls.Add(this.btnDeleteNote);
            this.Controls.Add(this.lblNoVisitFee);
            this.Controls.Add(this.txtNoVisitFee);
            this.Controls.Add(this.txtPatientFee);
            this.Controls.Add(this.txtNote);
            this.Controls.Add(this.chkNoVisit);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnShowPreviousNotes);
            this.Controls.Add(this.lblPatientFee);
            this.Controls.Add(this.lblVisitType);
            this.Controls.Add(this.cmbPatientCharge);
            this.Controls.Add(this.cmbVisitType);
            this.Controls.Add(this.lblCurrancy);
            this.Controls.Add(this.lblFreecardExpiry);
            this.Controls.Add(this.btnSign);
            this.Controls.Add(this.btnSaveNote);
            this.Controls.Add(this.boxPatient);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.grpVisitDate);
            this.Controls.Add(this.grpDiagnosisNumber);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1024, 620);
            this.Name = "Form3";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Patientjournal";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.boxPatient.ResumeLayout(false);
            this.boxPatient.PerformLayout();
            this.grpVisitDate.ResumeLayout(false);
            this.grpDiagnosisNumber.ResumeLayout(false);
            this.grpDiagnosisNumber.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDiagnosis)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region TreeView handling
		

		private void treeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			//If user clicks on parent node, insert parent name + : (so the user can write text after)
			//if user clicks on child node, insert parent name + : + child name
			TreeNode tnTemp = e.Node;
			
			int stringLength = 0;
			int insertPos = this.txtNote.SelectionStart;
			
			if (tnTemp != root)
			{
				if (tnTemp.Nodes.Count > 0)
				{
					//We have selected a node with children (category node)
					//Print just out the category name and the user will fill in the rest manually
					stringLength = tnTemp.Text.Length + 3;
					this.txtNote.Text = this.txtNote.Text.Insert(this.txtNote.SelectionStart, tnTemp.Text + ": ");
				}
				else
				{
					//We have selected a child (item)
					if (tnTemp.Parent != root)
					{
						//We have selected a child that belongs to a category node
						stringLength = tnTemp.Text.Length + tnTemp.Parent.Text.Length + 4;
						this.txtNote.Text = this.txtNote.Text.Insert(this.txtNote.SelectionStart, tnTemp.Parent.Text + ": " + tnTemp.Text + " ");
					}
					else
					{
						//We have selected a chil directly under the root node
						stringLength = tnTemp.Text.Length + 2;
						this.txtNote.Text = this.txtNote.Text.Insert(this.txtNote.SelectionStart, tnTemp.Text + ": ");
					}
				}
			}
			
			this.treeView.SelectedNode = null;
			
			//Simulate leftclick on textbox
			SendMessage(this.txtNote.Handle, WM_LBUTTONDOWN, 0, 0);
			SendMessage(this.txtNote.Handle, WM_LBUTTONUP, 0, 0);

			this.txtNote.SelectionStart = stringLength + insertPos;
		}
		#endregion

		#region Button handling
		private void btnSaveNote_Click(object sender, System.EventArgs e)
		{
			

			if (cmbPatientCharge.SelectedIndex != -1)
			{
				System.Data.DataRowView drv = (System.Data.DataRowView) cmbPatientCharge.SelectedItem;
				if (((string) drv["text"] == "Frikort") && note.VisitNote)
				{
					if (patient.FreecardDate == new System.DateTime((long)0))
					{
						if (MessageBox.Show("Inget frikort är registrerat på patienten, vill du fortsätta?", "Meddelande", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							this.DialogResult = DialogResult.None;
							return;
						}
					}
					if (patient.FreecardDate != new System.DateTime((long)0) && freecardHasExpired)
					{
						if (MessageBox.Show("Det registrerade frikortet har gått ut, vill du fortsätta?", "Meddelande", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							this.DialogResult = DialogResult.None;
							return;
						}
					}
				}
			}

			try 
			{
				SaveNote();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, "Kunde ej spara journalanteckningen");
				this.DialogResult = DialogResult.None;
			}
			
		}

		private void btnSign_Click(object sender, System.EventArgs e)
		{
			note.Signed = true;
			note.SignedDateTime = System.DateTime.Now;

			SaveNote();
			
			UpdateForm();
			
		}

		private void btnChange_Click(object sender, System.EventArgs e)
		{
			Form2 dlgPatient = new Form2(database, ref patient);
			if (dlgPatient.ShowDialog() == DialogResult.OK)
			{
				try 
				{
					SaveNote();
					database.Update(patient);
					UpdateForm();
				}
				catch (Exception exception)
				{
					MessageBox.Show(exception.Message, "Gick ej att spara anteckningen");
					this.DialogResult = DialogResult.None;
				}
										  
				
			}
		}

		private void btnShowPreviousNotes_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Left)
			{
				dlgPreviousNotes = new PreviousNotes(database, patient);
				dlgPreviousNotes.Show();
			}
		
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			if (txtNote.Text != note.JournalNote)
			{
				if (MessageBox.Show("Vill du verkligen avbryta", "Stäng journalanteckningen", MessageBoxButtons.YesNo) == DialogResult.Yes)
					this.DialogResult = DialogResult.Cancel;
				else
					this.DialogResult = DialogResult.None;
			}
		}
		#endregion

		#region Note object save and update
		private void SaveNote()
		{
			note.JournalNote = txtNote.Text;

			
			if (note.VisitNote)
			{
				System.DateTime date = dtpVisitDate.Value;
				System.DateTime time = dtpVisitTime.Value;
			
				note.VisitDateTime = new DateTime(date.Year,date.Month,date.Day,time.Hour, time.Minute, time.Second);

				if (cmbPatientCharge.SelectedIndex != -1)
					note.ChargeId = (int) cmbPatientCharge.SelectedValue;
				else
					throw new Exception("Var god välj hur patienten betalade");
		
				System.Data.DataRowView dr = (System.Data.DataRowView) cmbPatientCharge.SelectedItem;
				if (((string)dr.Row["primulatext"] == "J") && (txtPatientFee.Text == "0" || txtPatientFee.Text.Length == 0))
				{
					if (DialogResult.No == MessageBox.Show("Du har angivit att patienten har betalat 0 kr, stämmer det?", "Välj hur mycket patienten har betalat för besöket",MessageBoxButtons.YesNo))
						this.DialogResult = DialogResult.None;
					else
						note.PatientFee = txtPatientFee.Text;
					
				}
				else
					note.PatientFee = txtPatientFee.Text;

				
				//note.ActionCode = txtActionCode.Text;
			
			
				if (cmbVisitType.Text == "Nybesök")
					note.NewVisit = true;
				else
					note.NewVisit = false;
			}
			else
			{
				if (note.NotShownVisit())
				{
					if ((txtNoVisitFee.Text == "0") || (txtNoVisitFee.Text.Length == 0))
					{
						if (DialogResult.No == MessageBox.Show("Du har angivit att patienten ska betalat 0 kr, stämmer det?", "Välj hur mycket patienten ska betala för det uteblivna besöket",MessageBoxButtons.YesNo))
							this.DialogResult = DialogResult.None;
						else
							note.PatientFee = txtNoVisitFee.Text;
					
					}
					else
						note.PatientFee = txtNoVisitFee.Text;
				}
					
			}
				
		}

		private void UpdateForm()
		{
			//Make all controls enabled and visible
			foreach (Control c in this.Controls)
			{
				c.Enabled = true;
				c.Visible = true;
			}

			txtFirstname.Text = patient.Firstname;
			txtSurname.Text = patient.Surname;
			txtPersonnumber.Text = patient.Personnumber;
			txtNote.Text = note.JournalNote;
			
			if (note.VisitNote)
			{
				cmbPatientCharge.SelectedValue = note.ChargeId;
				txtPatientFee.Text = note.PatientFee;
				//txtActionCode.Text = note.ActionCode;

				dtpVisitDate.Value = note.VisitDateTime;
				dtpVisitTime.Value = note.VisitDateTime;

				chkNoVisit.Visible = false;
				chkNoVisit.Enabled = false;
				txtNoVisitFee.Enabled = false;
				txtNoVisitFee.Visible = false;
				lblNoVisitFee.Visible = false;
				lblNoVisitFee.Enabled = false;


				if (patient.FreecardDate != new System.DateTime((long)0))
				{
					
					StringBuilder text = new StringBuilder("Frikortet går ut om:\n");

					try
					{
						System.TimeSpan expireTime = patient.FreecardDate.Subtract(System.DateTime.Now);

						/*int years = (int)(expireTime.TotalDays / 365);
						int daysLeftinYear = (int)expireTime.TotalDays % 365;
						int months = (int)(daysLeftinYear / 30);
						int daysLeftinMonth = daysLeftinYear % 30;*/

						int weeks = (int) expireTime.Days / 7;
						int days = (int) expireTime.Days % 7;
					
						if (weeks > 1)
							text.Append(weeks.ToString() + " veckor, ");
						if (weeks == 1)
							text.Append(weeks.ToString() + " vecka, ");
						if (days > 0)
							text.Append(days.ToString() + " dagar");

						lblFreecardExpiry.ForeColor = Color.Black;
						lblFreecardExpiry.Text = text.ToString();
						freecardHasExpired = false;

						if (expireTime.Days == 1)
							lblFreecardExpiry.Text = "Frikortet går ut om två dagar";
						if (expireTime.Days == 0)
							lblFreecardExpiry.Text = "Frikortet går ut imorgon";
						if (expireTime.Days < 0)
						{
							lblFreecardExpiry.Text = "Frikortet har gått ut";
							lblFreecardExpiry.ForeColor = Color.Red;
							freecardHasExpired = true;
						}

					}
					catch (Exception exception)
					{
						lblFreecardExpiry.Text = "Frikortet har gått ut";
						lblFreecardExpiry.ForeColor = Color.Red;
					}
				}
			
				
				if (note.NewVisit)
					cmbVisitType.Text = "Nybesök";
				else
					cmbVisitType.Text = "Återbesök";

				lstDiagnosis.Items.Clear();
				lstDiagnosis.Items.AddRange(database.FindDiagnosisNumbers(note).ToArray());
				
			}
			
			else
			{
				grpDiagnosisNumber.Visible = false;
				grpDiagnosisNumber.Enabled = false;
				grpVisitDate.Visible = false;
				grpVisitDate.Enabled = false;
				cmbPatientCharge.Enabled = false;
				cmbPatientCharge.Visible = false;
				cmbVisitType.Visible = false;
				cmbVisitType.Enabled = false;
				txtPatientFee.Visible = false;
				txtPatientFee.Enabled = false;
				lblPatientFee.Visible = false;
				lblVisitType.Visible = false;
				lblCurrancy.Visible = false;
				lblFreecardExpiry.Visible = false;

				chkNoVisit.Visible = true;
				chkNoVisit.Enabled = true;

				if (note.NotShownVisit())
				{
					chkNoVisit.Checked = true;
					lblNoVisitFee.Enabled = true;
					lblNoVisitFee.Visible = true;
					txtNoVisitFee.Enabled = true;
					txtNoVisitFee.Visible = true;
					txtNoVisitFee.Text = note.PatientFee;
				}
				else
				{
					chkNoVisit.Checked = false;
					lblNoVisitFee.Enabled = false;
					lblNoVisitFee.Visible = false;
					txtNoVisitFee.Enabled = false;
					txtNoVisitFee.Visible = false;
				}

			}
		
			
			
			//Based on note condition, make some controls enabled and invisible
			if (note.Signed == true)
			{

				btnSign.Enabled = false;
				//txtNote.Enabled = false;
				txtNote.ReadOnly = true;
				btnDeleteNote.Enabled = false;
				//txtNote.BackColor = System.Drawing.Color.Wheat;
				
				grpVisitDate.Enabled = false;
				grpDiagnosisNumber.Enabled = false;
				treeView.Enabled = false;
			}

		}
		#endregion
	
		#region Diagnosisnumber handling
		private void txtDiagnosis_TextChanged(object sender, System.EventArgs e)
		{
			//Select the patient(s) that match the search criteria
			TextBox txtBox = (TextBox)sender;
			System.Windows.Forms.CurrencyManager cm = (CurrencyManager)BindingContext[dgDiagnosis.DataSource,dgDiagnosis.DataMember];
			DataView dv = (DataView) cm.List;
			dv.RowFilter = "diagnosistext LIKE '" + txtBox.Text + "*'";
			if (dv.Count > 0)
			{
				UpdateSelectedDiagnosis();
				dgDiagnosis.Select(0);
			}
		}

		private void UpdateSelectedDiagnosis()
		{
			System.Windows.Forms.CurrencyManager cm = (CurrencyManager)BindingContext[dgDiagnosis.DataSource,dgDiagnosis.DataMember];
			DataView dv = (DataView) cm.List;

			selectedDiagnosis = database.CreateDiagnosis(dv[dgDiagnosis.CurrentRowIndex].Row);
			//Check if the null value is selected
			//TODO: Fix so that diagnosisfield is null if no diagnosis is selected

			Debug.WriteLine("Current diagnosis: " + selectedDiagnosis.Id.ToString() + ", " +  selectedDiagnosis.DiagnosisNumber + ", " + selectedDiagnosis.DiagnosisText);
		}

		private void btnAddDiagnosis_Click(object sender, System.EventArgs e)
		{
			if ((selectedDiagnosis != null) && (lstDiagnosis.Items.Count < 5))
			{
				bool isAlreadyinList = false;
				foreach(int i in note.DiagnosisArray)
				{
					if (selectedDiagnosis.Id == i)
						isAlreadyinList = true;
				}
				if (selectedDiagnosis.DiagnosisNumber == null)
				{
					Debug.WriteLine("Selected diagnosisnumber is null");
					return;
				}
				if (!isAlreadyinList)
				{
					int index = lstDiagnosis.Items.Add((string)selectedDiagnosis.DiagnosisNumber);
					int[] diagnosisArray = note.DiagnosisArray;
					diagnosisArray[index] = selectedDiagnosis.Id;
					note.DiagnosisArray = diagnosisArray;
					Debug.WriteLine("Added diagnosisnumer " + selectedDiagnosis.DiagnosisNumber +" with id: " + selectedDiagnosis.Id);
					Debug.WriteLine(note.Diagnosis1.ToString() + ", " + note.Diagnosis2.ToString() + ", " + note.Diagnosis3.ToString() + ", " + note.Diagnosis4.ToString() + ", " + note.Diagnosis5.ToString());
				}
				else
					Debug.WriteLine("Diagnosisnumer " + selectedDiagnosis.DiagnosisNumber +" with id: " + selectedDiagnosis.Id + " is already in list");
			}
		}

		private void btnRemoveDiagnosis_Click(object sender, System.EventArgs e)
		{
			int index = lstDiagnosis.SelectedIndex;
			if (index != -1)
			{
				lstDiagnosis.Items.RemoveAt(index);
				int[] diagnosisArray = note.DiagnosisArray;
				diagnosisArray[index] = 0;
				note.DiagnosisArray = diagnosisArray;
				Debug.WriteLine(note.Diagnosis1.ToString() + ", " + note.Diagnosis2.ToString() + ", " + note.Diagnosis3.ToString() + ", " + note.Diagnosis4.ToString() + ", " + note.Diagnosis5.ToString());
			}
		}


		/*private void dgDiagnosis_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			DataGrid tempGrid = (DataGrid) sender;
			System.Windows.Forms.DataGrid.HitTestInfo hti;
			hti = tempGrid.HitTest(e.X, e.Y);

			switch (hti.Type) 
			{
				case System.Windows.Forms.DataGrid.HitTestType.Cell :
					tempGrid.Select(hti.Row);
					tempGrid.CurrentRowIndex = hti.Row;
					break;
				case System.Windows.Forms.DataGrid.HitTestType.RowHeader :
					tempGrid.Select(hti.Row);
					tempGrid.CurrentRowIndex = hti.Row;
					break;	
			}
		
		
		}*/

		private void dgDiagnosis_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			System.Drawing.Point pt = new Point(e.X, e.Y); 
			DataGrid dgTemp = (DataGrid) sender;
 
			DataGrid.HitTestInfo hti = dgTemp.HitTest(pt); 
 
			if(hti.Type == DataGrid.HitTestType.Cell) 
 
			{ 
				dgTemp.CurrentCell = new DataGridCell(hti.Row, hti.Column); 
				dgTemp.Select(hti.Row); 
				UpdateSelectedDiagnosis();
			} 
		
		}


		#endregion	

		#region Textbox handling (middle mouse-button)
		private void txtNote_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Middle)
			{
				dlgPreviousNotes = new PreviousNotes(database, patient, MousePosition);
				dlgPreviousNotes.Show();
			}


		}


		private void txtNote_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Middle)
			{
				dlgPreviousNotes.Close();
			}
		}
		#endregion

		#region Combobox and checkbox handling
		private void cmbPatientCharge_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (cmbPatientCharge.SelectedIndex != -1)
			{
				System.Data.DataRowView drv = (System.Data.DataRowView) cmbPatientCharge.SelectedItem;
				string selectedCharge = (string) drv["text"];
				switch (selectedCharge) 
				{
					case "Frikort":
			
						if (patient.FreecardDate == new System.DateTime((long)0))
							MessageBox.Show("Inget frikort är registrerat på patienten.\nDu kan registera ett frikort genom att gå in i visa/ändra patientuppgifter", "Meddelande");
						if (patient.FreecardDate != new System.DateTime((long)0) && freecardHasExpired)
							MessageBox.Show("Det registrerade frikortet har gått ut", "Meddelande");
						txtPatientFee.Text = "0";
						break;
			
					case "Uteblivet besök":
						MessageBox.Show("Kan ej journalföra ett uteblivet besök som en besöksanteckning,\nvar god gör en daganteckning istället", "Kunde ej sätta uteblivet besök");
						cmbPatientCharge.SelectedIndex = -1;
						break;
					case "Patienten betalar själv" :
						txtPatientFee.Text = "80";
						break;
					default:
						txtPatientFee.Text = "0";
						break;
				}
			}
			else
				cmbPatientCharge.Text = "Välj betalning här";
		}


		private void chkNoVisit_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkNoVisit.Checked)
			{
				//TODO: This is note good, fix it
				note.ChargeId = 7;

				lblNoVisitFee.Enabled = true;
				lblNoVisitFee.Visible = true;
				txtNoVisitFee.Enabled = true;
				txtNoVisitFee.Visible = true;

				//Set default value 
				txtNoVisitFee.Text = "80";
				
			}
			else
			{
				note.ChargeId = 0;
				note.PatientFee = "0";
				txtNoVisitFee.ResetText();

				lblNoVisitFee.Enabled = false;
				lblNoVisitFee.Visible = false;
				txtNoVisitFee.Enabled = false;
				txtNoVisitFee.Visible = false;
			}
		}
		#endregion

	
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

		private void dgDiagnosis_SizeChanged(object sender, System.EventArgs e)
		{
			Utils.AutoSizeDataGrid(new string[]{"diagnosistext"}, dgDiagnosis, this.BindingContext, 0);
		
		}

		private void btnDeleteNote_Click(object sender, System.EventArgs e)
		{
			if (DialogResult.Yes == MessageBox.Show("Är du säker på att du vill ta bort den här anteckningen?", "Bekräfta att du vill ta bort den här ateckningen.", MessageBoxButtons.YesNo))
			{
				database.Delete(note);
				this.DialogResult = DialogResult.Cancel;
			}
		}
	
	}

}
