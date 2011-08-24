using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace RehabLight
{
	/// <summary>
	/// Summary description for Statistics.
	/// </summary>
	public class Statistics : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox grpDiagnosisNumber;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtDiagnosis;
		private System.Windows.Forms.DataGrid dgDiagnosis;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.DateTimePicker dtpFromDate;
		private System.Windows.Forms.DateTimePicker dtpToDate;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox lbStatistics;
		private System.Windows.Forms.Label lblQueryResult;
		private System.Windows.Forms.Button btnExecuteCommand;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnClose;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private const string statNrofPatients = "Antal olika individer som fått behandling";
		private const string statNrofNewVisits = "Antal nybesök";
		private const string statNrofVisits = "Total antal besök";
		private const string statNrofVisitsDiagnosis = "Antal behandlingar med en viss diagnos";
		private const string statMostCommonDiagnosis = "De tio vanligaste diagnoserna";

		private System.Windows.Forms.DataGridTableStyle tsDiagnosis;
		private Database database;
		private Diagnosis selectedDiagnosis;

		public Statistics(Database aDatabase)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			database = aDatabase;

			//Setup statisticstab
			dgDiagnosis.SetDataBinding(database.DsMaster, "Diagnosis");
			SetupStatisticsControls();
			SetupDataGridDiagnosis();
		}

		#region Setup and handle statistics
	
		private void SetupStatisticsControls()
		{
			grpDiagnosisNumber.Enabled = false;
			lbStatistics.Items.Add(statNrofPatients);
			lbStatistics.Items.Add(statNrofNewVisits);
			lbStatistics.Items.Add(statNrofVisits);
			lbStatistics.Items.Add(statNrofVisitsDiagnosis);
			lbStatistics.Items.Add(statMostCommonDiagnosis);
		}

		private void SetupDataGridDiagnosis()
		{
			tsDiagnosis = new DataGridTableStyle();
			tsDiagnosis.MappingName = "Diagnosis";

			dgDiagnosis.ReadOnly = true;

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
			dv.Sort = "diagnosistext DESC";
		}

		private void lbStatistics_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			switch ((string)lbStatistics.SelectedItem)
			{
				case statNrofVisitsDiagnosis :
					grpDiagnosisNumber.Enabled = true;
					break;
			}
		}
		private void btnExecuteCommand_Click(object sender, System.EventArgs e)
		{
			string fromDate = dtpFromDate.Value.Month.ToString() + "/" + dtpFromDate.Value.Day.ToString() + "/" + dtpFromDate.Value.Year.ToString();
			string toDate = dtpToDate.Value.Month.ToString() + "/" + dtpToDate.Value.Day.ToString() + "/" + dtpToDate.Value.Year.ToString();
			
			string queryString = "";
			switch ((string)lbStatistics.SelectedItem)
			{
				case statNrofVisits:
					queryString = "SELECT COUNT(noteid) FROM notes WHERE (visitdatetime BETWEEN #" + fromDate + "# AND #" + toDate + "#)";
					break;
				case statNrofNewVisits :
					queryString = "SELECT COUNT(noteid) FROM notes WHERE ((newvisit=TRUE) AND (visitdatetime BETWEEN #" + fromDate + "# AND #" + toDate + "#))";
					break;
				case statNrofPatients :
					queryString = "SELECT COUNT(patientid) FROM patients WHERE EXSISTS (SELECT COUNT(notes.noteid) FROM notes WHERE ((patients.patientid = notes.patientid) AND (notes.visitdatetime BETWEEN #" + fromDate + "# AND #" + toDate + "#)))";
					break;
				case statNrofVisitsDiagnosis :
					if (selectedDiagnosis != null)
					{
						queryString = "SELECT COUNT(noteid) from notes WHERE diagnosis1=" + selectedDiagnosis.Id;
					}
					else
					{
						MessageBox.Show("Ange en diagnos först", "Kunde ej beräkna antalet besök för en viss diagnos");
						return;
					}
					break;
			}

			lblQueryResult.Text = "Resultat: " + database.QueryDatabase(queryString);
		}
		#endregion

		#region Diagnosisnumberhandling
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

			selectedDiagnosis = new Diagnosis();
			selectedDiagnosis.Id = (int) dv[dgDiagnosis.CurrentRowIndex]["id"];
			selectedDiagnosis.DiagnosisText = (string) dv[dgDiagnosis.CurrentRowIndex]["diagnosistext"];
			selectedDiagnosis.DiagnosisNumber = (string) dv[dgDiagnosis.CurrentRowIndex]["diagnosisnumber"];
		}


		private void dgDiagnosis_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
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
		
		
		}

		private void dgDiagnosis_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			System.Drawing.Point pt = new Point(e.X, e.Y); 
			DataGrid dgTemp = (DataGrid) sender;
 
			DataGrid.HitTestInfo hti = dgTemp.HitTest(pt); 
 
			if(hti.Type == DataGrid.HitTestType.Cell) 
 
			{ 
				dgTemp.CurrentCell = new DataGridCell(hti.Row, hti.Column); 
				dgTemp.Select(hti.Row); 
			} 
		
		}

		private void dgDiagnosis_CurrentCellChanged(object sender, System.EventArgs e)
		{
			UpdateSelectedDiagnosis();
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.grpDiagnosisNumber = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtDiagnosis = new System.Windows.Forms.TextBox();
			this.dgDiagnosis = new System.Windows.Forms.DataGrid();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
			this.dtpToDate = new System.Windows.Forms.DateTimePicker();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lbStatistics = new System.Windows.Forms.ListBox();
			this.lblQueryResult = new System.Windows.Forms.Label();
			this.btnExecuteCommand = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.btnClose = new System.Windows.Forms.Button();
			this.grpDiagnosisNumber.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgDiagnosis)).BeginInit();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// grpDiagnosisNumber
			// 
			this.grpDiagnosisNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grpDiagnosisNumber.Controls.Add(this.label3);
			this.grpDiagnosisNumber.Controls.Add(this.txtDiagnosis);
			this.grpDiagnosisNumber.Controls.Add(this.dgDiagnosis);
			this.grpDiagnosisNumber.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.grpDiagnosisNumber.Location = new System.Drawing.Point(16, 216);
			this.grpDiagnosisNumber.Name = "grpDiagnosisNumber";
			this.grpDiagnosisNumber.Size = new System.Drawing.Size(488, 168);
			this.grpDiagnosisNumber.TabIndex = 44;
			this.grpDiagnosisNumber.TabStop = false;
			this.grpDiagnosisNumber.Text = "Välj diagnosnummer";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.label3.Location = new System.Drawing.Point(16, 144);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(32, 16);
			this.label3.TabIndex = 34;
			this.label3.Text = "Sök";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// txtDiagnosis
			// 
			this.txtDiagnosis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.txtDiagnosis.Location = new System.Drawing.Point(56, 136);
			this.txtDiagnosis.Name = "txtDiagnosis";
			this.txtDiagnosis.Size = new System.Drawing.Size(224, 22);
			this.txtDiagnosis.TabIndex = 1;
			this.txtDiagnosis.Text = "";
			this.txtDiagnosis.TextChanged += new System.EventHandler(this.txtDiagnosis_TextChanged);
			// 
			// dgDiagnosis
			// 
			this.dgDiagnosis.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.dgDiagnosis.CaptionVisible = false;
			this.dgDiagnosis.DataMember = "";
			this.dgDiagnosis.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgDiagnosis.Location = new System.Drawing.Point(16, 24);
			this.dgDiagnosis.Name = "dgDiagnosis";
			this.dgDiagnosis.ReadOnly = true;
			this.dgDiagnosis.RowHeadersVisible = false;
			this.dgDiagnosis.Size = new System.Drawing.Size(448, 112);
			this.dgDiagnosis.TabIndex = 0;
			this.dgDiagnosis.CurrentCellChanged += new System.EventHandler(this.dgDiagnosis_CurrentCellChanged);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.dtpFromDate);
			this.groupBox2.Controls.Add(this.dtpToDate);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.groupBox2.Location = new System.Drawing.Point(432, 16);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(264, 184);
			this.groupBox2.TabIndex = 43;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "2. Välj tidsperiod";
			// 
			// dtpFromDate
			// 
			this.dtpFromDate.CustomFormat = "";
			this.dtpFromDate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dtpFromDate.Location = new System.Drawing.Point(56, 40);
			this.dtpFromDate.Name = "dtpFromDate";
			this.dtpFromDate.TabIndex = 4;
			// 
			// dtpToDate
			// 
			this.dtpToDate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dtpToDate.Location = new System.Drawing.Point(56, 112);
			this.dtpToDate.Name = "dtpToDate";
			this.dtpToDate.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 112);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(40, 24);
			this.label2.TabIndex = 8;
			this.label2.Text = "till";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 24);
			this.label1.TabIndex = 7;
			this.label1.Text = "från";
			// 
			// lbStatistics
			// 
			this.lbStatistics.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbStatistics.ItemHeight = 16;
			this.lbStatistics.Location = new System.Drawing.Point(32, 32);
			this.lbStatistics.Name = "lbStatistics";
			this.lbStatistics.Size = new System.Drawing.Size(352, 148);
			this.lbStatistics.TabIndex = 41;
			this.lbStatistics.SelectedIndexChanged += new System.EventHandler(this.lbStatistics_SelectedIndexChanged);
			// 
			// lblQueryResult
			// 
			this.lblQueryResult.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblQueryResult.Location = new System.Drawing.Point(24, 400);
			this.lblQueryResult.Name = "lblQueryResult";
			this.lblQueryResult.Size = new System.Drawing.Size(232, 32);
			this.lblQueryResult.TabIndex = 40;
			this.lblQueryResult.Text = "Resultat:";
			// 
			// btnExecuteCommand
			// 
			this.btnExecuteCommand.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnExecuteCommand.Location = new System.Drawing.Point(536, 256);
			this.btnExecuteCommand.Name = "btnExecuteCommand";
			this.btnExecuteCommand.Size = new System.Drawing.Size(192, 56);
			this.btnExecuteCommand.TabIndex = 39;
			this.btnExecuteCommand.Text = "Räkna";
			this.btnExecuteCommand.Click += new System.EventHandler(this.btnExecuteCommand_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(400, 192);
			this.groupBox1.TabIndex = 42;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "1. Välj typ av statistik";
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnClose.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnClose.Location = new System.Drawing.Point(536, 328);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(192, 56);
			this.btnClose.TabIndex = 45;
			this.btnClose.Text = "Stäng";
			// 
			// Statistics
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(784, 438);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.grpDiagnosisNumber);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.lbStatistics);
			this.Controls.Add(this.lblQueryResult);
			this.Controls.Add(this.btnExecuteCommand);
			this.Controls.Add(this.groupBox1);
			this.MaximizeBox = false;
			this.Name = "Statistics";
			this.Text = "Statistics";
			this.grpDiagnosisNumber.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgDiagnosis)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
