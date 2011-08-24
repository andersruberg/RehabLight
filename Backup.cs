using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;


namespace RehabLight
{
	/// <summary>
	/// Summary description for Backup.
	/// </summary>
	public class Backup : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnStartBackup;
		private System.Windows.Forms.ListBox lbBackupPaths;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Backup()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			foreach(object o in Settings.BackupPaths.ToArray())
				lbBackupPaths.Items.Add((string)o);
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
			this.btnStartBackup = new System.Windows.Forms.Button();
			this.lbBackupPaths = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnStartBackup
			// 
			this.btnStartBackup.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnStartBackup.Location = new System.Drawing.Point(16, 168);
			this.btnStartBackup.Name = "btnStartBackup";
			this.btnStartBackup.Size = new System.Drawing.Size(144, 48);
			this.btnStartBackup.TabIndex = 0;
			this.btnStartBackup.Text = "Starta säkerhetskopiering";
			this.btnStartBackup.Click += new System.EventHandler(this.btnStartBackup_Click);
			// 
			// lbBackupPaths
			// 
			this.lbBackupPaths.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbBackupPaths.ItemHeight = 16;
			this.lbBackupPaths.Location = new System.Drawing.Point(8, 48);
			this.lbBackupPaths.Name = "lbBackupPaths";
			this.lbBackupPaths.Size = new System.Drawing.Size(320, 84);
			this.lbBackupPaths.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(408, 24);
			this.label1.TabIndex = 2;
			this.label1.Text = "Databasen kommer att säkerhetskopieras till följande sökväg(ar):";
			// 
			// Backup
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(472, 294);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lbBackupPaths);
			this.Controls.Add(this.btnStartBackup);
			this.Name = "Backup";
			this.Text = "Säkerhetskopiering";
			this.ResumeLayout(false);

		}
		#endregion

		private void btnStartBackup_Click(object sender, System.EventArgs e)
		{
			foreach(object o in Settings.BackupPaths.ToArray())
			{
				//TODO: Databasefilepath should be in Settings
				try
				{
					File.Copy("rehab.mdb", (string)o, true);
				}
				catch (Exception exception)
				{
					MessageBox.Show("Det gick ej att säkerhetskopiera, kontrollera att USB-minnena sitter i: \nFelmeddelande från programmet: " + exception.Message, "Kunde ej säkerhetskopiera");
					return;
				}
			}

			MessageBox.Show("Säkerhetskopieringen lyckades", "Meddelande");

			this.Close();

		}
	}
}
