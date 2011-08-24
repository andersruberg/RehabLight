using System;

namespace RehabLight
{
	/// <summary>
	/// Summary description for Diagnosis.
	/// </summary>
	public class Diagnosis
	{
		public Diagnosis()
		{
			diagnosisText = "";
			diagnosisNumber = "";
			id = 0;
		}

		private string diagnosisText;
		public string DiagnosisText
		{
			get {return diagnosisText;} set {diagnosisText = value;}
		}
		private string diagnosisNumber;
		public string DiagnosisNumber
		{
			get {return diagnosisNumber;} set {diagnosisNumber = value;}
		}
		private int id;
		public int Id
		{
			get {return id;} set {id = value;}
		}

}
}
