using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace RehabLight
{
	/// <summary>
	/// Summary description for ShowJoinedTable.
	/// </summary>
	public class ShowJoinedTable : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnPrimulaRemove;
		private System.Windows.Forms.Button btnPrimulaAdd;
		private System.Windows.Forms.DataGrid dgPrimula;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public ShowJoinedTable()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.btnPrimulaRemove = new System.Windows.Forms.Button();
			this.btnPrimulaAdd = new System.Windows.Forms.Button();
			this.dgPrimula = new System.Windows.Forms.DataGrid();
			((System.ComponentModel.ISupportInitialize)(this.dgPrimula)).BeginInit();
			this.SuspendLayout();
			// 
			// btnPrimulaRemove
			// 
			this.btnPrimulaRemove.Location = new System.Drawing.Point(144, 464);
			this.btnPrimulaRemove.Name = "btnPrimulaRemove";
			this.btnPrimulaRemove.Size = new System.Drawing.Size(104, 32);
			this.btnPrimulaRemove.TabIndex = 8;
			this.btnPrimulaRemove.Text = "Ta bort";
			// 
			// btnPrimulaAdd
			// 
			this.btnPrimulaAdd.Location = new System.Drawing.Point(16, 464);
			this.btnPrimulaAdd.Name = "btnPrimulaAdd";
			this.btnPrimulaAdd.Size = new System.Drawing.Size(104, 32);
			this.btnPrimulaAdd.TabIndex = 7;
			this.btnPrimulaAdd.Text = "Lägg till...";
			// 
			// dgPrimula
			// 
			this.dgPrimula.DataMember = "";
			this.dgPrimula.HeaderForeColor = System.Drawing.SystemColors.ControlText;
			this.dgPrimula.Location = new System.Drawing.Point(0, 48);
			this.dgPrimula.Name = "dgPrimula";
			this.dgPrimula.ReadOnly = true;
			this.dgPrimula.Size = new System.Drawing.Size(992, 400);
			this.dgPrimula.TabIndex = 6;
			// 
			// ShowJoinedTable
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(1016, 566);
			this.Controls.Add(this.btnPrimulaRemove);
			this.Controls.Add(this.btnPrimulaAdd);
			this.Controls.Add(this.dgPrimula);
			this.Name = "ShowJoinedTable";
			this.Text = "ShowJoinedTable";
			((System.ComponentModel.ISupportInitialize)(this.dgPrimula)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
	}
}
