using System;

namespace RehabLight
{
	/// <summary>
	/// Summary description for Charge.
	/// </summary>
	public class Charge
	{
		private int id;
		private string description;
		private string primulaCharachter;

		public int Id
		{
			get{return id;} set{id = value;}
		}

		public string Description
		{
			get{return description;} set{description = value;}
		}

		public string PrimulaCharachter
		{
			get{return primulaCharachter;} set{primulaCharachter = value;}
		}
		
		public Charge()
		{
		
		}
	}
}
