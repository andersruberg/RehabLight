using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace RehabLight
{
	/// <summary>
	/// Summary description for FilteredNotes.
	/// </summary>
	public class FilteredNotes : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.DataGrid dgNotes;

		private System.Windows.Forms.DataGridTableStyle tsNotesFiltered;
		private System.Windows.Forms.DataGridTableStyle tsPatientFiltered;
		private System.Data.OleDb.OleDbConnection oleDbConnection1;
		private System.Data.OleDb.OleDbDataAdapter daJoin;
		private System.Data.OleDb.OleDbCommand oleDbSelectCommand1;
		private Database database;

		public FilteredNotes(Database aDatabase)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			database = aDatabase;
			SetupDataGridNotesFiltered();
		}

		private void SetupDataGridNotesFiltered()
		{
			dgNotes.SetDataBinding(database.DsMaster, "Notes.NotesPatient");

			tsNotesFiltered = new DataGridTableStyle();
			tsPatientFiltered = new DataGridTableStyle();
			tsNotesFiltered.MappingName = "Notes";
			tsPatientFiltered.MappingName = "Patients";

			dgNotes.CaptionText = "Journalanteckningar";
			dgNotes.RowHeadersVisible = false;
			dgNotes.ReadOnly = true;

			//Utils.CopyDefaultTableStyle(dgNotes, tsNotesFiltered);			
			
			System.Windows.Forms.DataGridBoolColumn dgbcSigned = new System.Windows.Forms.DataGridBoolColumn();
			dgbcSigned.Width = 100;
			dgbcSigned.MappingName = "signed";
			dgbcSigned.HeaderText = "Signerad";

			System.Windows.Forms.DataGridTextBoxColumn dgtbcNote = new DataGridTextBoxColumn();
			dgtbcNote.Width = 200;
			dgtbcNote.MappingName = "note";
			dgtbcNote.HeaderText = "Anteckning";
			dgtbcNote.NullText = "";

			System.Windows.Forms.DataGridTextBoxColumn dgtbcDateTime = new DataGridTextBoxColumn();
			dgtbcDateTime.Width = 100;
			dgtbcDateTime.MappingName = "visitdatetime";
			dgtbcDateTime.HeaderText = "Datum";

			System.Windows.Forms.DataGridTextBoxColumn dgtbcFirstname = new DataGridTextBoxColumn();
			dgtbcDateTime.Width = 100;
			dgtbcDateTime.MappingName = "firstname";
			dgtbcDateTime.HeaderText = "Förnamn";
			
			System.Windows.Forms.DataGridTextBoxColumn dgtbcSurname = new DataGridTextBoxColumn();
			dgtbcDateTime.Width = 100;
			dgtbcDateTime.MappingName = "surname";
			dgtbcDateTime.HeaderText = "Efternamn";

			tsNotesFiltered.GridColumnStyles.Add(dgtbcDateTime);
			tsPatientFiltered.GridColumnStyles.Add(dgtbcSurname);
			tsPatientFiltered.GridColumnStyles.Add(dgtbcFirstname);
			tsNotesFiltered.GridColumnStyles.Add(dgtbcNote);
			tsNotesFiltered.GridColumnStyles.Add(dgbcSigned);

			dgNotes.TableStyles.Add(tsNotesFiltered);
			dgNotes.TableStyles.Add(tsPatientFiltered);

			/*System.Windows.Forms.CurrencyManager cm = (CurrencyManager)BindingContext[dgNotes.DataSource,dgNotes.DataMember];
			System.Data.DataView dv = (System.Data.DataView) cm.List;
			dv.Sort = "visitdatetime DESC";*/
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
			this.dgNotes = new System.Windows.Forms.DataGrid();
			this.daJoin = new System.Data.OleDb.OleDbDataAdapter();
			this.oleDbConnection1 = new System.Data.OleDb.OleDbConnection();
			this.oleDbSelectCommand1 = new System.Data.OleDb.OleDbCommand();
			((System.ComponentModel.ISupportInitialize)(this.dgNotes)).BeginInit();
			this.SuspendLayout();
			// 
			// dgNotes
			// 
			this.dgNotes.DataMember = "";
			this.dgNotes.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgNotes.Location = new System.Drawing.Point(16, 16);
			this.dgNotes.Name = "dgNotes";
			this.dgNotes.Size = new System.Drawing.Size(744, 336);
			this.dgNotes.TabIndex = 0;
			// 
			// daJoin
			// 
			this.daJoin.SelectCommand = this.oleDbSelectCommand1;
			this.daJoin.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																							 new System.Data.Common.DataTableMapping("Table", "Notes", new System.Data.Common.DataColumnMapping[] {
																																																	  new System.Data.Common.DataColumnMapping("primulatext", "primulatext"),
																																																	  new System.Data.Common.DataColumnMapping("visitnote", "visitnote"),
																																																	  new System.Data.Common.DataColumnMapping("noteid", "noteid"),
																																																	  new System.Data.Common.DataColumnMapping("primula", "primula")})});
			this.daJoin.RowUpdated += new System.Data.OleDb.OleDbRowUpdatedEventHandler(this.oleDbDataAdapter1_RowUpdated);
			// 
			// oleDbConnection1
			// 
			this.oleDbConnection1.ConnectionString = @"Jet OLEDB:Global Partial Bulk Ops=2;Jet OLEDB:Registry Path=;Jet OLEDB:Database Locking Mode=1;Jet OLEDB:Database Password=;Data Source=""C:\Documents and Settings\Anders Ruberg\Mina dokument\Visual Studio Projects\RehabLight\bin\Debug\rehab.mdb"";Password=;Jet OLEDB:Engine Type=5;Jet OLEDB:Global Bulk Transactions=1;Provider=""Microsoft.Jet.OLEDB.4.0"";Jet OLEDB:System database=;Jet OLEDB:SFP=False;Extended Properties=;Mode=Share Deny None;Jet OLEDB:New Database Password=;Jet OLEDB:Create System Database=False;Jet OLEDB:Don't Copy Locale on Compact=False;Jet OLEDB:Compact Without Replica Repair=False;User ID=Admin;Jet OLEDB:Encrypt Database=False";
			// 
			// oleDbSelectCommand1
			// 
			this.oleDbSelectCommand1.CommandText = "SELECT Charges.primulatext, Notes.visitnote, Notes.noteid, Notes.primula FROM (No" +
				"tes INNER JOIN Charges ON Charges.chargeid = Notes.chargeid) WHERE (Notes.primul" +
				"a = FALSE) AND (Notes.visitnote = TRUE) AND (MID([Notes.visitdatetime], 1, 4) = " +
				"\'2007\')";
			this.oleDbSelectCommand1.Connection = this.oleDbConnection1;
			// 
			// FilteredNotes
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(768, 390);
			this.Controls.Add(this.dgNotes);
			this.Name = "FilteredNotes";
			this.Text = "FilteredNotes";
			((System.ComponentModel.ISupportInitialize)(this.dgNotes)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void oleDbDataAdapter1_RowUpdated(object sender, System.Data.OleDb.OleDbRowUpdatedEventArgs e)
		{
		
		}
	}
}
