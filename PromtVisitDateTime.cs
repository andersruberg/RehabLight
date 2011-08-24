using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace RehabLight
{
	/// <summary>
	/// Summary description for PromtVisitDateTime.
	/// </summary>
	public class PromtVisitDateTime : System.Windows.Forms.Form
	{
		private Note note;
		private System.Windows.Forms.DateTimePicker dtpVisitTime;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.MonthCalendar monthCalendar;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton rbVisitNote;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox grpDateTime;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PromtVisitDateTime(ref Note aNote)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			note = aNote;

			rbVisitNote.Checked = true;
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
			this.dtpVisitTime = new System.Windows.Forms.DateTimePicker();
			this.monthCalendar = new System.Windows.Forms.MonthCalendar();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.rbVisitNote = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.grpDateTime = new System.Windows.Forms.GroupBox();
			this.grpDateTime.SuspendLayout();
			this.SuspendLayout();
			// 
			// dtpVisitTime
			// 
			this.dtpVisitTime.CustomFormat = "HH:mm";
			this.dtpVisitTime.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.dtpVisitTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpVisitTime.Location = new System.Drawing.Point(136, 312);
			this.dtpVisitTime.Name = "dtpVisitTime";
			this.dtpVisitTime.ShowUpDown = true;
			this.dtpVisitTime.Size = new System.Drawing.Size(80, 29);
			this.dtpVisitTime.TabIndex = 2;
			// 
			// monthCalendar
			// 
			this.monthCalendar.BackColor = System.Drawing.Color.Gainsboro;
			this.monthCalendar.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.monthCalendar.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.monthCalendar.Location = new System.Drawing.Point(24, 32);
			this.monthCalendar.MaxSelectionCount = 1;
			this.monthCalendar.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
			this.monthCalendar.Name = "monthCalendar";
			this.monthCalendar.ShowWeekNumbers = true;
			this.monthCalendar.TabIndex = 7;
			this.monthCalendar.TitleBackColor = System.Drawing.Color.SteelBlue;
			this.monthCalendar.TitleForeColor = System.Drawing.SystemColors.ControlText;
			this.monthCalendar.TrailingForeColor = System.Drawing.SystemColors.AppWorkspace;
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnOk.Location = new System.Drawing.Point(128, 208);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(96, 40);
			this.btnOk.TabIndex = 5;
			this.btnOk.Text = "Ok";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnCancel.Location = new System.Drawing.Point(16, 208);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(96, 40);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "Avbryt";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(40, 312);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 24);
			this.label1.TabIndex = 8;
			this.label1.Text = "Klockan";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// rbVisitNote
			// 
			this.rbVisitNote.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.rbVisitNote.Location = new System.Drawing.Point(40, 40);
			this.rbVisitNote.Name = "rbVisitNote";
			this.rbVisitNote.Size = new System.Drawing.Size(176, 40);
			this.rbVisitNote.TabIndex = 9;
			this.rbVisitNote.Text = "Besöksanteckning";
			this.rbVisitNote.CheckedChanged += new System.EventHandler(this.rbVisitNote_CheckedChanged);
			// 
			// radioButton2
			// 
			this.radioButton2.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.radioButton2.Location = new System.Drawing.Point(40, 80);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(168, 48);
			this.radioButton2.TabIndex = 10;
			this.radioButton2.Text = "Journalanteckning";
			this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(16, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(216, 120);
			this.groupBox1.TabIndex = 11;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Anteckningstyp";
			// 
			// grpDateTime
			// 
			this.grpDateTime.Controls.Add(this.monthCalendar);
			this.grpDateTime.Controls.Add(this.label1);
			this.grpDateTime.Controls.Add(this.dtpVisitTime);
			this.grpDateTime.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.grpDateTime.Location = new System.Drawing.Point(256, 16);
			this.grpDateTime.Name = "grpDateTime";
			this.grpDateTime.Size = new System.Drawing.Size(288, 352);
			this.grpDateTime.TabIndex = 12;
			this.grpDateTime.TabStop = false;
			this.grpDateTime.Text = "Datum och klockslag";
			// 
			// PromtVisitDateTime
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Gainsboro;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(568, 392);
			this.ControlBox = false;
			this.Controls.Add(this.radioButton2);
			this.Controls.Add(this.rbVisitNote);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.grpDateTime);
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(264, 426);
			this.Name = "PromtVisitDateTime";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Ange anteckningstyp";
			this.grpDateTime.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Button and radiobutton handling
		private void btnOk_Click(object sender, System.EventArgs e)
		{
			if (rbVisitNote.Checked)
			{
				System.DateTime date = monthCalendar.SelectionStart;
				System.DateTime time = dtpVisitTime.Value;
			
				note.VisitDateTime = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, 0);
				note.VisitNote = true;
				//Set default charge id to 1
				note.ChargeId = 1;
			}
			else
			{
				note.VisitDateTime = note.CreateDateTime;
				note.VisitNote = false;
				//Set not chargeid
				note.ChargeId = 0;
				note.DiagnosisArray = new int[] {0, 0, 0, 0, 0};
			}
		}

		private void radioButton2_CheckedChanged(object sender, System.EventArgs e)
		{
			grpDateTime.Enabled = false;
			grpDateTime.Visible = false;

			this.Size = new System.Drawing.Size(264, 426);
		}

		private void rbVisitNote_CheckedChanged(object sender, System.EventArgs e)
		{
			grpDateTime.Enabled = true;
			grpDateTime.Visible = true;

			this.Size = new System.Drawing.Size(576, 426);
		
		}
		#endregion
	}
}
