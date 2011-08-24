using System;

namespace RehabLight
{
	/// <summary>
	/// Summary description for Note.
	/// </summary>
	public class Note
	{
		public Note()
		{
			note = "";
			patientFee = "0";
			diagnosis1 = 0;
			diagnosis2 = 0;
			diagnosis3 = 0;
			diagnosis4 = 0;
			diagnosis5 = 0;
			signed = false;
			newVisit = false;
			id = 0;
			primula = false;
			chargeId = 0;
		}

		public Note(int aPatientId)
		{
			createDateTime = System.DateTime.Now;
			signedDateTime = new System.DateTime(((long)(0)));
			visitDateTime = new System.DateTime(((long)(0)));
			note = "";
			patientFee = "0";
			diagnosis1 = 0;
			diagnosis2 = 0;
			diagnosis3 = 0;
			diagnosis4 = 0;
			diagnosis5 = 0;
			//TODO:The actioncode should not be hardcoded
			actionCode = "02";
			signed = false;
			newVisit = false;
			patientId = aPatientId;
			id = 0;
			primula = false;
			chargeId = 0;

		}

		public bool NotShownVisit()
		{
			if (chargeId == 7)
				return true;
			else
				return false;
		}


		System.DateTime createDateTime, signedDateTime, visitDateTime;
		private string note;
		private int id, patientId, chargeId;
		private bool signed, newVisit, primula;
		
		private string actionCode;		
		public string ActionCode {get{return actionCode;} set{actionCode = value;}}
		public int ChargeId {get{return chargeId;} set{chargeId = value;}}
		
		private string patientFee;
		public string PatientFee {get{return patientFee;} set{patientFee = value;}}
		
		public System.DateTime VisitDateTime {get{return visitDateTime;} set{visitDateTime = value;}}
		public System.DateTime CreateDateTime {get{return createDateTime;} set{createDateTime = value;}}
		public System.DateTime SignedDateTime {get{return signedDateTime;} set{signedDateTime = value;}}

		public bool Primula {get{return primula;} set{primula = value;}}

		private bool visitNote;
		public bool VisitNote {get {return visitNote;} set {visitNote = value;}}

		private int diagnosis1, diagnosis2, diagnosis3, diagnosis4, diagnosis5;
		public int Diagnosis1 {get{return diagnosis1;} set{diagnosis1 = value;}}
		public int Diagnosis2 {get{return diagnosis2;} set{diagnosis2 = value;}}
		public int Diagnosis3 {get{return diagnosis3;} set{diagnosis3 = value;}}
		public int Diagnosis4 {get{return diagnosis4;} set{diagnosis4 = value;}}
		public int Diagnosis5 {get{return diagnosis5;} set{diagnosis5 = value;}}

		public int[] DiagnosisArray
		{
			get {return new int[5] {diagnosis1, diagnosis2, diagnosis3, diagnosis4, diagnosis5};}
			set {diagnosis1 = value[0]; diagnosis2 = value[1]; diagnosis3=value[2]; diagnosis4=value[3]; diagnosis5=value[4];}
		}
		
		public int PatientId
		{
			get {return patientId;}
			set {patientId = value;}
		}
		
		public string JournalNote
		{
			get {return note;}
			set {note = value;}
		}
		public bool Signed
		{
			get {return signed;}
			set {signed = value;}
		}
		public bool NewVisit
		{
			get {return newVisit;}
			set {newVisit = value;}
		}
		public int Id
		{
			get {return id;}
			set {id = value;}
		}

		
		
	}
}
