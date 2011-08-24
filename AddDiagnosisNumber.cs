using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace RehabLight
{
	/// <summary>
	/// Summary description for AddDiagnosisNumber.
	/// </summary>
	public class AddDiagnosisNumber : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtNumber;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.Label lblNumber;
		private System.Windows.Forms.Label lblText;
		private Database database;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnAdd;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AddDiagnosisNumber(Database aDatabase)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			database = aDatabase;

			btnAdd.Enabled = false;
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.lblText = new System.Windows.Forms.Label();
			this.lblNumber = new System.Windows.Forms.Label();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.txtNumber = new System.Windows.Forms.TextBox();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnAdd = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.lblText);
			this.groupBox1.Controls.Add(this.lblNumber);
			this.groupBox1.Controls.Add(this.txtDescription);
			this.groupBox1.Controls.Add(this.txtNumber);
			this.groupBox1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(24, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(472, 104);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Diagnosnummer och beskrivning";
			// 
			// lblText
			// 
			this.lblText.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblText.Location = new System.Drawing.Point(144, 32);
			this.lblText.Name = "lblText";
			this.lblText.Size = new System.Drawing.Size(136, 24);
			this.lblText.TabIndex = 3;
			this.lblText.Text = "Beskrivning";
			// 
			// lblNumber
			// 
			this.lblNumber.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblNumber.Location = new System.Drawing.Point(16, 32);
			this.lblNumber.Name = "lblNumber";
			this.lblNumber.Size = new System.Drawing.Size(96, 16);
			this.lblNumber.TabIndex = 2;
			this.lblNumber.Text = "Nummer";
			// 
			// txtDescription
			// 
			this.txtDescription.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtDescription.Location = new System.Drawing.Point(144, 64);
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.Size = new System.Drawing.Size(304, 26);
			this.txtDescription.TabIndex = 1;
			this.txtDescription.Text = "";
			this.txtDescription.TextChanged += new System.EventHandler(this.txtDescription_TextChanged);
			// 
			// txtNumber
			// 
			this.txtNumber.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtNumber.Location = new System.Drawing.Point(16, 64);
			this.txtNumber.MaxLength = 5;
			this.txtNumber.Name = "txtNumber";
			this.txtNumber.Size = new System.Drawing.Size(88, 26);
			this.txtNumber.TabIndex = 0;
			this.txtNumber.Text = "";
			this.txtNumber.TextChanged += new System.EventHandler(this.txtNumber_TextChanged);
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnClose.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnClose.Location = new System.Drawing.Point(408, 136);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(88, 32);
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "Stäng";
			// 
			// btnAdd
			// 
			this.btnAdd.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnAdd.Location = new System.Drawing.Point(312, 136);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(88, 32);
			this.btnAdd.TabIndex = 2;
			this.btnAdd.Text = "Lägg till";
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// AddDiagnosisNumber
			// 
			this.AcceptButton = this.btnAdd;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(520, 182);
			this.ControlBox = false;
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.groupBox1);
			this.MaximumSize = new System.Drawing.Size(528, 216);
			this.MinimumSize = new System.Drawing.Size(528, 216);
			this.Name = "AddDiagnosisNumber";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Lägg till diagnos i databasen";
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			Diagnosis diagnosis = new Diagnosis();
			diagnosis.DiagnosisNumber = txtNumber.Text;
			diagnosis.DiagnosisText = txtDescription.Text;

			try 
			{
				database.Add(diagnosis);

				MessageBox.Show("Diagnosnumret lades till i databasen", "Uppdateringen av databasen lyckades");
				txtNumber.ResetText();
				txtDescription.ResetText();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, "Fel vid uppdateringen av databasen");
			}
		}
	

		private void txtNumber_TextChanged(object sender, System.EventArgs e)
		{
			if ((txtNumber.Text.Length != 0) && (txtDescription.Text.Length != 0))
				btnAdd.Enabled = true;
			else
				btnAdd.Enabled = false;
		}

		private void txtDescription_TextChanged(object sender, System.EventArgs e)
		{
			if ((txtNumber.Text.Length != 0) && (txtDescription.Text.Length != 0))
				btnAdd.Enabled = true;
			else
				btnAdd.Enabled = false;
		
		}
	}
}
