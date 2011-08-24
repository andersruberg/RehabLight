using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace RehabLight
{
	/// <summary>
	/// Summary description for PreviousNotes.
	/// </summary>
	public class PreviousNotes : System.Windows.Forms.Form
	{
		private System.Windows.Forms.DataGridTableStyle tsNotes;
		private Database database;
		private System.Windows.Forms.DataGrid dgNotes;
		private Point location;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PreviousNotes(Database aDatabase, Patient aPatient, Point aLocation)
		{
			//
			// Required for Windows Form Designer support
			//
			location = aLocation;
			InitializeComponent();

			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Location = location;
			database = aDatabase;
			
			CurrencyManager cm = (CurrencyManager) this.BindingContext[database.DsMaster, "Patients"];
			System.Data.DataView dv = (System.Data.DataView) cm.List;
			dv.Sort = "id";
			int position = dv.Find(aPatient.Id);
			this.BindingContext[database.DsMaster, "Patients"].Position = position;
			dgNotes.SetDataBinding(database.DsMaster, "Patients.PatientNotes");
			

			SetupDataGridNotes();

		}

		public PreviousNotes(Database aDatabase, Patient aPatient)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			database = aDatabase;
			
			CurrencyManager cm = (CurrencyManager) this.BindingContext[database.DsMaster, "Patients"];
			System.Data.DataView dv = (System.Data.DataView) cm.List;
			dv.Sort = "patientid";
			int position = dv.Find(aPatient.Id);
			this.BindingContext[database.DsMaster, "Patients"].Position = position;
			dgNotes.SetDataBinding(database.DsMaster, "Patients.PatientNotes");
			

			SetupDataGridNotes();

		}

		private void SetupDataGridNotes()
		{
			tsNotes = new DataGridTableStyle();
			tsNotes.MappingName = "Notes";

			dgNotes.CaptionText = "Journalanteckningar";
			dgNotes.RowHeadersVisible = true;
			dgNotes.ReadOnly = true;

			Utils.CopyDefaultTableStyle(dgNotes, tsNotes);			


			System.Windows.Forms.DataGridTextBoxColumn dgtbcNote = new DataGridTextBoxColumn();
			dgtbcNote.Width = 640;
			dgtbcNote.MappingName = "note";
			dgtbcNote.HeaderText = "Anteckning";
			dgtbcNote.NullText = "";

			System.Windows.Forms.DataGridTextBoxColumn dgtbcDateTime = new DataGridTextBoxColumn();
			dgtbcDateTime.Width = 100;
			dgtbcDateTime.MappingName = "visitdatetime";
			dgtbcDateTime.HeaderText = "Datum";

			
			tsNotes.GridColumnStyles.Add(dgtbcDateTime);
			tsNotes.GridColumnStyles.Add(dgtbcNote);

			dgNotes.TableStyles.Add(tsNotes);

			System.Windows.Forms.CurrencyManager cm = (CurrencyManager)BindingContext[dgNotes.DataSource,dgNotes.DataMember];
			System.Data.DataView dv = (System.Data.DataView) cm.List;
			dv.Sort = "visitdatetime DESC";
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
			((System.ComponentModel.ISupportInitialize)(this.dgNotes)).BeginInit();
			this.SuspendLayout();
			// 
			// dgNotes
			// 
			this.dgNotes.AllowNavigation = false;
			this.dgNotes.AlternatingBackColor = System.Drawing.Color.White;
			this.dgNotes.BackColor = System.Drawing.Color.White;
			this.dgNotes.BackgroundColor = System.Drawing.Color.Gainsboro;
			this.dgNotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.dgNotes.CaptionBackColor = System.Drawing.Color.Silver;
			this.dgNotes.CaptionFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dgNotes.CaptionForeColor = System.Drawing.Color.Black;
			this.dgNotes.DataMember = "";
			this.dgNotes.FlatMode = true;
			this.dgNotes.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dgNotes.ForeColor = System.Drawing.Color.DarkSlateGray;
			this.dgNotes.GridLineColor = System.Drawing.Color.DarkGray;
			this.dgNotes.HeaderBackColor = System.Drawing.Color.DarkGreen;
			this.dgNotes.HeaderFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dgNotes.HeaderForeColor = System.Drawing.Color.White;
			this.dgNotes.LinkColor = System.Drawing.Color.DarkGreen;
			this.dgNotes.Location = new System.Drawing.Point(0, 0);
			this.dgNotes.Name = "dgNotes";
			this.dgNotes.ParentRowsBackColor = System.Drawing.Color.Gainsboro;
			this.dgNotes.ParentRowsForeColor = System.Drawing.Color.Black;
			this.dgNotes.ReadOnly = true;
			this.dgNotes.SelectionBackColor = System.Drawing.Color.DarkSeaGreen;
			this.dgNotes.SelectionForeColor = System.Drawing.Color.Black;
			this.dgNotes.Size = new System.Drawing.Size(728, 400);
			this.dgNotes.TabIndex = 14;
			// 
			// PreviousNotes
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(728, 238);
			this.Controls.Add(this.dgNotes);
			this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "PreviousNotes";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Tidigare anteckningar";
			((System.ComponentModel.ISupportInitialize)(this.dgNotes)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
	}
}
