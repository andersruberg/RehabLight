using System;

namespace RehabLight
{
	/// <summary>
	/// Summary description for Patient.
	/// </summary>
	public class Patient
	{
		public Patient()
		{
			freecardDate = new System.DateTime(((long)(0)));
			firstname = "";
			surname = "";
			personnumber = "";
			street = "";
			zipcode = "";
			city = "";
			homePhone = "";
			workPhone = "";
			mobilePhone = "";
			info = "";
		}

		private string firstname, surname, personnumber, street, zipcode, city, homePhone, workPhone,
			mobilePhone, info;
		private int id;

		private System.DateTime freecardDate;

		public System.DateTime FreecardDate
		{
			get {return freecardDate;} set {freecardDate = value;}
		}

		public string Firstname
		{
			get {return firstname;} set{firstname = value;}
		}

		public string Surname
		{
			get {return surname;} set {surname = value;}
		}

		public string Street
		{
			get{return street;} set{street = value;}
		}

		public string Zipcode
		{
			get{return zipcode;} set{zipcode = value;}
		}

		public string City
		{
			get{return city;} set {city = value;}
		}

		public string WorkPhone
		{
			get{return workPhone;} set{workPhone = value;}
		}

		public string HomePhone
		{
			get{return homePhone;} set{homePhone = value;}
		}

		public string MobilePhone
		{
			get{return mobilePhone;} set{mobilePhone = value;}
		}

		public string Info
		{
			get{return info;} set{info = value;}
		}

		public string Personnumber
		{
			get{return personnumber;} set{personnumber = value;}
		}

		public int Id
		{
			get {return id;} set {id = value;}
		}
	}
}
