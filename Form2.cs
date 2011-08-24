using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

namespace RehabLight
{
	/// <summary>
	/// Summary description for Form2.
	/// </summary>
	public class Form2 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblMisc;
		private System.Windows.Forms.Label lblPhone2;
		private System.Windows.Forms.Label lblPhone1;
		private System.Windows.Forms.Label lblPersonNumber;
		private System.Windows.Forms.Label lblAddress;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.TextBox txtMisc;
		private System.Windows.Forms.GroupBox boxPatient;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TextBox txtStreet;
		private System.Windows.Forms.TextBox txtFirstname;
		private System.Windows.Forms.TextBox txtSurname;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private RegExControls.RegExTextBox txtMobilePhone;
		private RegExControls.RegExTextBox txtHomePhone;
		private RegExControls.RegExTextBox txtWorkPhone;
		private System.Windows.Forms.TextBox txtCity;
	
		private Patient patient;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.RadioButton rbHasFreecard;
		private System.Windows.Forms.RadioButton rbHasNotFreecard;
		private System.Windows.Forms.Label lblFreecardExpiry;

		private System.Text.RegularExpressions.Regex regExpDate;
		private bool updateMode;
		private string originalPersonnumber;
		private Database database;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private RegExControls.RegExTextBox txtPersonnumber;
		private RegExControls.RegExTextBox txtFreecardDate;
		private RegExControls.RegExTextBox txtZipcode;
		private System.Windows.Forms.Label label5;
		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form2(Database aDatabase, ref Patient aPatient)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			
			patient = aPatient;
			database = aDatabase;

			//Investige if the patient is a new patient or a already existing one
			

			regExpDate = new System.Text.RegularExpressions.Regex(@"(19|20)\d\d[-](0[1-9]|1[012])[-](0[1-9]|[12][0-9]|3[01])");

			UpdateForm();	


		}

		private void UpdateForm()
		{
			if (patient.Personnumber != null)
			{
				updateMode = true;
				originalPersonnumber = patient.Personnumber;
			}
			else
				updateMode = false;
			
			txtFirstname.Text = patient.Firstname;
			txtSurname.Text = patient.Surname;
			txtPersonnumber.Text = patient.Personnumber;
			txtStreet.Text = patient.Street;
			txtZipcode.Text = patient.Zipcode;
			txtCity.Text = patient.City;
			txtWorkPhone.Text = patient.WorkPhone;
			txtMobilePhone.Text = patient.MobilePhone;
			txtHomePhone.Text = patient.HomePhone;
			txtMisc.Text = patient.Info;

			if (patient.FreecardDate != new System.DateTime((long)0))
			{
				rbHasFreecard.Checked = true;
				txtFreecardDate.Text = patient.FreecardDate.ToShortDateString();
			}
			else
			{
				rbHasNotFreecard.Checked = true;
				txtFreecardDate.Visible = false;
				lblFreecardExpiry.Visible = false;
			}

			txtPersonnumber.ValidateControl();
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
			this.lblMisc = new System.Windows.Forms.Label();
			this.lblPhone2 = new System.Windows.Forms.Label();
			this.lblPhone1 = new System.Windows.Forms.Label();
			this.lblPersonNumber = new System.Windows.Forms.Label();
			this.lblAddress = new System.Windows.Forms.Label();
			this.lblName = new System.Windows.Forms.Label();
			this.btnSave = new System.Windows.Forms.Button();
			this.txtMisc = new System.Windows.Forms.TextBox();
			this.txtMobilePhone = new RegExControls.RegExTextBox();
			this.boxPatient = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtFreecardDate = new RegExControls.RegExTextBox();
			this.txtPersonnumber = new RegExControls.RegExTextBox();
			this.rbHasNotFreecard = new System.Windows.Forms.RadioButton();
			this.rbHasFreecard = new System.Windows.Forms.RadioButton();
			this.label6 = new System.Windows.Forms.Label();
			this.lblFreecardExpiry = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.txtWorkPhone = new RegExControls.RegExTextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txtCity = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txtHomePhone = new RegExControls.RegExTextBox();
			this.txtStreet = new System.Windows.Forms.TextBox();
			this.txtSurname = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txtFirstname = new System.Windows.Forms.TextBox();
			this.txtZipcode = new RegExControls.RegExTextBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider();
			this.boxPatient.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblMisc
			// 
			this.lblMisc.Location = new System.Drawing.Point(8, 240);
			this.lblMisc.Name = "lblMisc";
			this.lblMisc.Size = new System.Drawing.Size(72, 24);
			this.lblMisc.TabIndex = 14;
			this.lblMisc.Text = "Information";
			// 
			// lblPhone2
			// 
			this.lblPhone2.Location = new System.Drawing.Point(520, 200);
			this.lblPhone2.Name = "lblPhone2";
			this.lblPhone2.Size = new System.Drawing.Size(40, 24);
			this.lblPhone2.TabIndex = 13;
			this.lblPhone2.Text = "Mobil";
			// 
			// lblPhone1
			// 
			this.lblPhone1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblPhone1.Location = new System.Drawing.Point(8, 200);
			this.lblPhone1.Name = "lblPhone1";
			this.lblPhone1.Size = new System.Drawing.Size(64, 24);
			this.lblPhone1.TabIndex = 12;
			this.lblPhone1.Text = "Telefon: ";
			// 
			// lblPersonNumber
			// 
			this.lblPersonNumber.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblPersonNumber.Location = new System.Drawing.Point(8, 72);
			this.lblPersonNumber.Name = "lblPersonNumber";
			this.lblPersonNumber.Size = new System.Drawing.Size(104, 24);
			this.lblPersonNumber.TabIndex = 11;
			this.lblPersonNumber.Text = "Personnummer";
			// 
			// lblAddress
			// 
			this.lblAddress.Location = new System.Drawing.Point(8, 112);
			this.lblAddress.Name = "lblAddress";
			this.lblAddress.Size = new System.Drawing.Size(72, 24);
			this.lblAddress.TabIndex = 10;
			this.lblAddress.Text = "Adress";
			// 
			// lblName
			// 
			this.lblName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblName.Location = new System.Drawing.Point(400, 32);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(64, 24);
			this.lblName.TabIndex = 9;
			this.lblName.Text = "Förnamn";
			// 
			// btnSave
			// 
			this.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnSave.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.btnSave.Location = new System.Drawing.Point(608, 344);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(96, 40);
			this.btnSave.TabIndex = 20;
			this.btnSave.Text = "Spara";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// txtMisc
			// 
			this.txtMisc.Location = new System.Drawing.Point(112, 240);
			this.txtMisc.Name = "txtMisc";
			this.txtMisc.Size = new System.Drawing.Size(584, 22);
			this.txtMisc.TabIndex = 9;
			this.txtMisc.Text = "";
			// 
			// txtMobilePhone
			// 
			this.txtMobilePhone.Location = new System.Drawing.Point(568, 200);
			this.txtMobilePhone.Name = "txtMobilePhone";
			this.txtMobilePhone.Regular_Expression = "^(([+]\\d{2}[ ][1-9]\\d{0,2}[ ])|([0]\\d{1,3}[-]))((\\d{2}([ ]\\d{2}){2})|(\\d{3}([ ]\\d" +
				"{3})*([ ]\\d{2})+))$";
			this.txtMobilePhone.Size = new System.Drawing.Size(120, 22);
			this.txtMobilePhone.TabIndex = 8;
			this.txtMobilePhone.Text = "";
			this.txtMobilePhone.Validating += new System.ComponentModel.CancelEventHandler(this.txtMobilePhone_Validating);
			// 
			// boxPatient
			// 
			this.boxPatient.Controls.Add(this.label5);
			this.boxPatient.Controls.Add(this.txtFreecardDate);
			this.boxPatient.Controls.Add(this.txtPersonnumber);
			this.boxPatient.Controls.Add(this.rbHasNotFreecard);
			this.boxPatient.Controls.Add(this.rbHasFreecard);
			this.boxPatient.Controls.Add(this.label6);
			this.boxPatient.Controls.Add(this.lblFreecardExpiry);
			this.boxPatient.Controls.Add(this.label4);
			this.boxPatient.Controls.Add(this.txtWorkPhone);
			this.boxPatient.Controls.Add(this.label3);
			this.boxPatient.Controls.Add(this.txtCity);
			this.boxPatient.Controls.Add(this.label2);
			this.boxPatient.Controls.Add(this.lblMisc);
			this.boxPatient.Controls.Add(this.lblPhone2);
			this.boxPatient.Controls.Add(this.lblPhone1);
			this.boxPatient.Controls.Add(this.lblPersonNumber);
			this.boxPatient.Controls.Add(this.lblAddress);
			this.boxPatient.Controls.Add(this.lblName);
			this.boxPatient.Controls.Add(this.txtMisc);
			this.boxPatient.Controls.Add(this.txtMobilePhone);
			this.boxPatient.Controls.Add(this.txtHomePhone);
			this.boxPatient.Controls.Add(this.txtStreet);
			this.boxPatient.Controls.Add(this.txtSurname);
			this.boxPatient.Controls.Add(this.label1);
			this.boxPatient.Controls.Add(this.txtFirstname);
			this.boxPatient.Controls.Add(this.txtZipcode);
			this.boxPatient.Location = new System.Drawing.Point(8, 16);
			this.boxPatient.Name = "boxPatient";
			this.boxPatient.Size = new System.Drawing.Size(728, 312);
			this.boxPatient.TabIndex = 17;
			this.boxPatient.TabStop = false;
			this.boxPatient.Text = "Patientuppgifter";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(72, 200);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(40, 24);
			this.label5.TabIndex = 33;
			this.label5.Text = "Hem";
			// 
			// txtFreecardDate
			// 
			this.txtFreecardDate.Location = new System.Drawing.Point(400, 280);
			this.txtFreecardDate.MaxLength = 10;
			this.txtFreecardDate.Name = "txtFreecardDate";
			this.txtFreecardDate.Regular_Expression = @"^((\d{2}(([02468][048])|([13579][26]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|([1-2][0-9])))))|(\d{2}(([02468][1235679])|([13579][01345789]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))))(\s(((0?[1-9])|(1[0-9])|(2[0-3]))\:([0-5][0-9])((\s)|(\:([0-5][0-9])))?))?$";
			this.txtFreecardDate.Size = new System.Drawing.Size(88, 22);
			this.txtFreecardDate.TabIndex = 11;
			this.txtFreecardDate.Text = "";
			this.txtFreecardDate.Validating += new System.ComponentModel.CancelEventHandler(this.txtFreecardDate_Validating);
			// 
			// txtPersonnumber
			// 
			this.txtPersonnumber.Location = new System.Drawing.Point(112, 72);
			this.txtPersonnumber.MaxLength = 13;
			this.txtPersonnumber.Name = "txtPersonnumber";
			this.txtPersonnumber.Regular_Expression = "[1-2][0|9][0-9]{2}[0-1][0-9][0-3][0-9][-][0-9]{4}";
			this.txtPersonnumber.Size = new System.Drawing.Size(144, 22);
			this.txtPersonnumber.TabIndex = 2;
			this.txtPersonnumber.Text = "";
			this.txtPersonnumber.Validating += new System.ComponentModel.CancelEventHandler(this.txtPersonnumber_Validating);
			// 
			// rbHasNotFreecard
			// 
			this.rbHasNotFreecard.Location = new System.Drawing.Point(192, 280);
			this.rbHasNotFreecard.Name = "rbHasNotFreecard";
			this.rbHasNotFreecard.Size = new System.Drawing.Size(48, 24);
			this.rbHasNotFreecard.TabIndex = 30;
			this.rbHasNotFreecard.Text = "Nej";
			this.rbHasNotFreecard.CheckedChanged += new System.EventHandler(this.rbHasNotFreecard_CheckedChanged);
			// 
			// rbHasFreecard
			// 
			this.rbHasFreecard.Location = new System.Drawing.Point(144, 280);
			this.rbHasFreecard.Name = "rbHasFreecard";
			this.rbHasFreecard.Size = new System.Drawing.Size(48, 24);
			this.rbHasFreecard.TabIndex = 10;
			this.rbHasFreecard.Text = "Ja";
			this.rbHasFreecard.CheckedChanged += new System.EventHandler(this.rbHasFreecard_CheckedChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 280);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(128, 24);
			this.label6.TabIndex = 28;
			this.label6.Text = "Patienten har frikort";
			// 
			// lblFreecardExpiry
			// 
			this.lblFreecardExpiry.Location = new System.Drawing.Point(248, 280);
			this.lblFreecardExpiry.Name = "lblFreecardExpiry";
			this.lblFreecardExpiry.Size = new System.Drawing.Size(152, 24);
			this.lblFreecardExpiry.TabIndex = 27;
			this.lblFreecardExpiry.Text = "Frikortet giltigt t.o.m.";
			this.lblFreecardExpiry.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(280, 200);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(48, 24);
			this.label4.TabIndex = 25;
			this.label4.Text = "Arbete";
			// 
			// txtWorkPhone
			// 
			this.txtWorkPhone.Location = new System.Drawing.Point(336, 200);
			this.txtWorkPhone.Name = "txtWorkPhone";
			this.txtWorkPhone.Regular_Expression = "^(([+]\\d{2}[ ][1-9]\\d{0,2}[ ])|([0]\\d{1,3}[-]))((\\d{2}([ ]\\d{2}){2})|(\\d{3}([ ]\\d" +
				"{3})*([ ]\\d{2})+))$";
			this.txtWorkPhone.Size = new System.Drawing.Size(128, 22);
			this.txtWorkPhone.TabIndex = 7;
			this.txtWorkPhone.Text = "";
			this.txtWorkPhone.Validating += new System.ComponentModel.CancelEventHandler(this.txtWorkPhone_Validating);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(256, 160);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(48, 24);
			this.label3.TabIndex = 23;
			this.label3.Text = "Stad";
			// 
			// txtCity
			// 
			this.txtCity.Location = new System.Drawing.Point(312, 160);
			this.txtCity.Name = "txtCity";
			this.txtCity.Size = new System.Drawing.Size(272, 22);
			this.txtCity.TabIndex = 5;
			this.txtCity.Text = "";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 160);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 24);
			this.label2.TabIndex = 22;
			this.label2.Text = "Postnummer";
			// 
			// txtHomePhone
			// 
			this.txtHomePhone.Location = new System.Drawing.Point(112, 200);
			this.txtHomePhone.Name = "txtHomePhone";
			this.txtHomePhone.Regular_Expression = "^(([+]\\d{2}[ ][1-9]\\d{0,2}[ ])|([0]\\d{1,3}[-]))((\\d{2}([ ]\\d{2}){2})|(\\d{3}([ ]\\d" +
				"{3})*([ ]\\d{2})+))$";
			this.txtHomePhone.Size = new System.Drawing.Size(128, 22);
			this.txtHomePhone.TabIndex = 6;
			this.txtHomePhone.Text = "";
			this.txtHomePhone.Validating += new System.ComponentModel.CancelEventHandler(this.txtHomePhone_Validating);
			// 
			// txtStreet
			// 
			this.txtStreet.Location = new System.Drawing.Point(112, 112);
			this.txtStreet.Name = "txtStreet";
			this.txtStreet.Size = new System.Drawing.Size(272, 22);
			this.txtStreet.TabIndex = 3;
			this.txtStreet.Text = "";
			// 
			// txtSurname
			// 
			this.txtSurname.Location = new System.Drawing.Point(112, 32);
			this.txtSurname.Name = "txtSurname";
			this.txtSurname.Size = new System.Drawing.Size(256, 22);
			this.txtSurname.TabIndex = 0;
			this.txtSurname.Text = "";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(8, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 24);
			this.label1.TabIndex = 19;
			this.label1.Text = "Efternamn";
			// 
			// txtFirstname
			// 
			this.txtFirstname.Location = new System.Drawing.Point(464, 32);
			this.txtFirstname.Name = "txtFirstname";
			this.txtFirstname.Size = new System.Drawing.Size(224, 22);
			this.txtFirstname.TabIndex = 1;
			this.txtFirstname.Text = "";
			// 
			// txtZipcode
			// 
			this.txtZipcode.Location = new System.Drawing.Point(112, 160);
			this.txtZipcode.MaxLength = 8;
			this.txtZipcode.Name = "txtZipcode";
			this.txtZipcode.Regular_Expression = "^(s-|S-){0,1}[0-9]{3}\\s?[0-9]{2}$";
			this.txtZipcode.TabIndex = 4;
			this.txtZipcode.Text = "";
			this.txtZipcode.Validating += new System.ComponentModel.CancelEventHandler(this.txtZipcode_Validating);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(488, 344);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(96, 40);
			this.btnCancel.TabIndex = 21;
			this.btnCancel.Text = "Avbryt";
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// Form2
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
			this.ClientSize = new System.Drawing.Size(752, 390);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.boxPatient);
			this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.MaximizeBox = false;
			this.Name = "Form2";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Lägg till ny patient";
			this.boxPatient.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnSave_Click(object sender, System.EventArgs e)
		{
			
			//Do validation of personnumber here
			if (txtPersonnumber.ValidateControl() == false)
			{
				this.DialogResult = DialogResult.None;
				return;
			}
			else
			{
				try 
				{
					//Check if the personnumber is valid
					if  (Settings.IsCheckPersonnumberEnabled)
						Utils.CheckPersonNumber(txtPersonnumber.Text);
				}
				catch (Exception exception)
				{
					//If e.g. the date is invalid an exception is thrown
					this.errorProvider1.SetError(txtPersonnumber, exception.Message);
					this.DialogResult = DialogResult.None;
					return;
				}

				//The personnumber is valid but before saving, check if it already exists in the database
				patient.Personnumber = txtPersonnumber.Text;
			}

			
			patient.Firstname = txtFirstname.Text;
			patient.Surname = txtSurname.Text;
			
			patient.Street = txtStreet.Text;

			//Non-mandatory fields 
			//TODO: modify regexp to allow an empty string?
			patient.Zipcode = txtZipcode.Text;
			/*if (txtZipcode.Text.Length != 0)
			{					
				if (txtZipcode.ValidateControl() == false)
					this.DialogResult = DialogResult.None;
				else
					patient.Zipcode = txtZipcode.Text;
			}*/


			patient.City = txtCity.Text;
			patient.HomePhone = txtHomePhone.Text;
			patient.WorkPhone = txtWorkPhone.Text;
			patient.MobilePhone = txtMobilePhone.Text;
			patient.Info = txtMisc.Text;


			//Validation of date for expiry of freecard
			if (rbHasFreecard.Checked)
			{
				if (txtFreecardDate.ValidateControl() == false)
				{
					this.DialogResult = DialogResult.None;
				}
				else
				{
					//The try is not neccessary since the textbox validation checks that the date also is valid, not only the format
					try
					{
						patient.FreecardDate = System.DateTime.Parse(txtFreecardDate.Text);
					}
					catch (FormatException exception)
					{
						this.errorProvider1.SetError(txtFreecardDate, "Det angivna datumet är ogiltigt.");
						this.DialogResult = DialogResult.None;
					}
				}
			}
			else
				patient.FreecardDate = new System.DateTime((long)0);

			
			//Last thing before saving the patient, check if it already exists int the database
			if ((originalPersonnumber != patient.Personnumber) && database.IsAlreadyExsisting(patient))
			{
				//The user is notified if there already exists a patient with the given personnumber
				DialogResult dialogResult = MessageBox.Show("Det finns redan en patient med det här personnumret.\nVill du öppna den redan existerande patienten med det angivna personnumret?\n\nOm du vill öppna den existerande patienten tryck Ja.\nTryck Nej för att mata in ett nytt personnummer.", "Meddelande", MessageBoxButtons.YesNo);
				switch (dialogResult)
				{
					case DialogResult.Yes:
						patient = database.ReturnAlreadyExsisting(patient);
						UpdateForm();
						this.DialogResult = DialogResult.None;
						break;
					case DialogResult.No:
						txtPersonnumber.ResetText();
						this.DialogResult = DialogResult.None;
						break;
				}
			}
		
		}

		private void rbHasFreecard_CheckedChanged(object sender, System.EventArgs e)
		{
			if (rbHasFreecard.Checked)
			{
				lblFreecardExpiry.Visible = true;
				txtFreecardDate.Visible = true;
			}
		}

		private void rbHasNotFreecard_CheckedChanged(object sender, System.EventArgs e)
		{
			if (rbHasNotFreecard.Checked)
			{
				lblFreecardExpiry.Visible = false;
				txtFreecardDate.Visible = false;
			}
		
		}

		private void txtPersonnumber_Validating(object sender, CancelEventArgs e)
		{
			if (txtPersonnumber.ValidateControl() == false)
				this.errorProvider1.SetError(txtPersonnumber, "Skriv personnumret på formen ÅÅÅÅMMDD-XXXX, ex. 19810704-2087");
			else
				this.errorProvider1.SetError(txtPersonnumber,"");
				
			
		}

		private void txtFreecardDate_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (txtFreecardDate.ValidateControl() == false)
				this.errorProvider1.SetError(txtFreecardDate, "Ogiltigt datum eller felaktigt format.\nSkriv datumet på formatet ÅÅÅÅ-MM-DD, ex. 2007-02-21 el. 2007-2-21");
			else
				this.errorProvider1.SetError(txtFreecardDate,"");
		
		}

		private void txtZipcode_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			/*if (txtZipcode.ValidateControl() == false)
			{
				this.errorProvider1.SetError(txtZipcode, "Felaktigt format. Ett postnummer består av 5 siffror och kan skrivas på formaten 25227, 252 27, S-252 27 ");
			}
			else
			{
				this.errorProvider1.SetError(txtZipcode,"");
			}*/
		
		}

		private void txtHomePhone_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			/*if (txtHomePhone.ValidateControl() == false)
				this.errorProvider1.SetError(txtHomePhone, "Felaktigt format. Telefonnummer skrivs på följande format: ex. +46 8 123 456 78, 08-123 456 78, 0123-456 78 ");
			else
				this.errorProvider1.SetError(txtHomePhone,"");*/
		}

		private void txtMobilePhone_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			/*if (txtMobilePhone.ValidateControl() == false)
				this.errorProvider1.SetError(txtMobilePhone, "Felaktigt format. Telefonnummer skrivs på följande format: ex. +46 8 123 456 78, 08-123 456 78, 0123-456 78 ");
			else
				this.errorProvider1.SetError(txtMobilePhone,"");*/
		}

		private void txtWorkPhone_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			/*if (txtWorkPhone.ValidateControl() == false)
				this.errorProvider1.SetError(txtWorkPhone, "Felaktigt format. Telefonnummer skrivs på följande format: ex. +46 8 123 456 78, 08-123 456 78, 0123-456 78 ");
			else
				this.errorProvider1.SetError(txtWorkPhone,"");*/
		}
	}
}
