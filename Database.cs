using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Collections;

namespace RehabLight
{
	/// <summary>
	/// Summary description for Database.
	/// </summary>
	//TODO: Make this class static?
	public class Database
	{
		private System.Data.OleDb.OleDbConnection connection;
		private System.Data.DataSet dsMaster;
	
		private System.Data.OleDb.OleDbDataAdapter daNotes;
		private System.Data.OleDb.OleDbDataAdapter daPatients;
		private System.Data.OleDb.OleDbDataAdapter daDiagnosis;
		private System.Data.OleDb.OleDbDataAdapter daCharges;
		private System.Data.OleDb.OleDbDataAdapter daJoined;

		public DataSet DsMaster
		{
			get { return dsMaster;}
		}

		///<summary>
		/// Setups the database and the dataadapters.
		///</summary> 
		public Database(string fileName, string password)
		{
			connection = new OleDbConnection();
			
			//connection.ConnectionString = "Data Source=" + fileName + ";Provider=Microsoft.Jet.OLEDB.4.0;User ID=Admin;Jet OLEDB:Database Password=" + password + ";Jet OLEDB:Encrypt Database=False";
            connection.ConnectionString = "Data Source=" + fileName + ";Provider=Microsoft.ACE.OLEDB.12.0;User ID=Admin;Jet OLEDB:Database Password=" + password + ";Jet OLEDB:Encrypt Database=False";
			
			connection.StateChange += new System.Data.StateChangeEventHandler(Connection_StateChange);

			try
			{
				connection.Open();
			}
			catch (Exception e)
			{
				throw e;
			}
				
			
			SetupDataAdapterNotes();
			SetupDataAdapterPatients();
			SetupDataAdapterDiagnosis();
			SetupDataAdapterCharges();
			SetupDataAdapterJoined();

			dsMaster = new DataSet();

			daPatients.Fill(dsMaster, "Patients");
			daNotes.Fill(dsMaster, "Notes");
			daDiagnosis.Fill(dsMaster, "Diagnosis");
			daCharges.Fill(dsMaster, "Charges");
			daJoined.Fill(dsMaster, "Joined");

			connection.Close();

            dsMaster.Relations.Add("PatientNotes", dsMaster.Tables["Patients"].Columns["patientid"], dsMaster.Tables["Notes"].Columns["patientid"],false);

			dsMaster.Tables["Patients"].Columns["patientid"].AutoIncrement = true;
			dsMaster.Tables["Patients"].Columns["patientid"].AutoIncrementStep = 1;
			dsMaster.Tables["Patients"].Columns["patientid"].AutoIncrementSeed = Utils.GetNextUniqueId(dsMaster.Tables["Patients"]);
			dsMaster.Tables["Notes"].Columns["noteid"].AutoIncrement = true;
			dsMaster.Tables["Notes"].Columns["noteid"].AutoIncrementStep = 1;
			dsMaster.Tables["Notes"].Columns["noteid"].AutoIncrementSeed = Utils.GetNextUniqueId(dsMaster.Tables["Notes"]);
			dsMaster.Tables["Diagnosis"].Columns["diagnosisid"].AutoIncrement = true;
			dsMaster.Tables["Diagnosis"].Columns["diagnosisid"].AutoIncrementStep = 1;
			dsMaster.Tables["Diagnosis"].Columns["diagnosisid"].AutoIncrementSeed = Utils.GetNextUniqueId(dsMaster.Tables["Diagnosis"]);
			

			dsMaster.Tables["Patients"].RowChanging +=new DataRowChangeEventHandler(Patients_RowChanging);
			dsMaster.Tables["Notes"].RowChanging +=new DataRowChangeEventHandler(Notes_RowChanging);
			dsMaster.Tables["Diagnosis"].RowChanging +=new DataRowChangeEventHandler(Diagnosis_RowChanging);
		}

		#region DataAdapter setup

		private void SetupDataAdapterJoined()
		{
			daJoined = new System.Data.OleDb.OleDbDataAdapter();
			OleDbCommand selectCmd = new System.Data.OleDb.OleDbCommand();
			selectCmd.Connection = connection;

			
			daJoined.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																							 new System.Data.Common.DataTableMapping("Table", "Patients", new System.Data.Common.DataColumnMapping[] {
																																																		 new System.Data.Common.DataColumnMapping("chargetext", "chargetext"),
																																																		 new System.Data.Common.DataColumnMapping("chargeprimula", "chargeprimula"),
																																																		 new System.Data.Common.DataColumnMapping("diagnosistext1", "diagnosistext1"),
																																																		 new System.Data.Common.DataColumnMapping("diagnosisnumber1", "diagnosisnumber1"),
																																																		 new System.Data.Common.DataColumnMapping("diagnosisnumber2", "diagnosisnumber2"),
																																																		 new System.Data.Common.DataColumnMapping("diagnosisnumber3", "diagnosisnumber3"),
																																																		 new System.Data.Common.DataColumnMapping("diagnosisnumber4", "diagnosisnumber4"),
																																																		 new System.Data.Common.DataColumnMapping("diagnosisnumber5", "diagnosisnumber5"),
																																																		 new System.Data.Common.DataColumnMapping("city", "city"),
																																																		 new System.Data.Common.DataColumnMapping("firstname", "firstname"),
																																																		 new System.Data.Common.DataColumnMapping("homephone", "homephone"),
																																																		 new System.Data.Common.DataColumnMapping("patientid", "patientid"),
																																																		 new System.Data.Common.DataColumnMapping("info", "info"),
																																																		 new System.Data.Common.DataColumnMapping("mobilephone", "mobilephone"),
																																																		 new System.Data.Common.DataColumnMapping("freecarddate", "freecarddate"),
																																																		 new System.Data.Common.DataColumnMapping("personnumber", "personnumber"),
																																																		 new System.Data.Common.DataColumnMapping("street", "street"),
																																																		 new System.Data.Common.DataColumnMapping("surname", "surname"),
																																																		 new System.Data.Common.DataColumnMapping("workphone", "workphone"),
																																																		 new System.Data.Common.DataColumnMapping("zipcode", "zipcode"),
																																																		 new System.Data.Common.DataColumnMapping("actioncode", "actioncode"),
																																																		 new System.Data.Common.DataColumnMapping("visitnote", "visitnote"),
																																																		 new System.Data.Common.DataColumnMapping("chargeid", "chargeid"),
																																																		 new System.Data.Common.DataColumnMapping("createdatetime", "createdatetime"),
																																																		 new System.Data.Common.DataColumnMapping("diagnosis1", "diagnosis1"),
																																																		 new System.Data.Common.DataColumnMapping("diagnosis2", "diagnosis2"),
																																																		 new System.Data.Common.DataColumnMapping("diagnosis3", "diagnosis3"),
																																																		 new System.Data.Common.DataColumnMapping("diagnosis4", "diagnosis4"),
																																																		 new System.Data.Common.DataColumnMapping("diagnosis5", "diagnosis5"),
																																																		 new System.Data.Common.DataColumnMapping("noteid", "noteid"),
																																																		 new System.Data.Common.DataColumnMapping("newvisit", "newvisit"),
																																																		 new System.Data.Common.DataColumnMapping("note", "note"),
																																																		 new System.Data.Common.DataColumnMapping("patientfee", "patientfee"),
																																																		 new System.Data.Common.DataColumnMapping("primula", "primula"),
																																																		 new System.Data.Common.DataColumnMapping("signed", "signed"),
																																																		 new System.Data.Common.DataColumnMapping("signeddatetime", "signeddatetime"),
																																																		 new System.Data.Common.DataColumnMapping("visitdatetime", "visitdatetime")})});
			
		selectCmd.CommandText = @"SELECT Charges.[text] AS chargetext, Charges.primulatext AS chargeprimula, Diagnosis_1.diagnosistext AS diagnosistext1, Diagnosis_1.diagnosisnumber AS diagnosisnumber1, Diagnosis_2.diagnosisnumber AS diagnosisnumber2, Diagnosis_3.diagnosisnumber AS diagnosisnumber3, Diagnosis_4.diagnosisnumber AS diagnosisnumber4, Diagnosis_5.diagnosisnumber AS diagnosisnumber5, Patients.city, Patients.firstname, Patients.homephone, Patients.patientid, Patients.info, Patients.mobilephone, Patients.freecarddate, Patients.personnumber, Patients.street, Patients.surname, Patients.workphone, Patients.zipcode, Notes.actioncode, Notes.visitnote, Notes.chargeid, Notes.createdatetime, Notes.diagnosis1, Notes.diagnosis2, Notes.diagnosis3, Notes.diagnosis4, Notes.diagnosis5, Notes.noteid, Notes.newvisit, Notes.[note], Notes.patientfee, Notes.primula, Notes.signed, Notes.signeddatetime, Notes.visitdatetime FROM (((((((Patients INNER JOIN Notes ON Patients.patientid = Notes.patientid) LEFT OUTER JOIN Diagnosis Diagnosis_1 ON Diagnosis_1.diagnosisid = Notes.diagnosis1) LEFT OUTER JOIN Diagnosis Diagnosis_2 ON Diagnosis_2.diagnosisid = Notes.diagnosis2) LEFT OUTER JOIN Diagnosis Diagnosis_3 ON Diagnosis_3.diagnosisid = Notes.diagnosis3) LEFT OUTER JOIN Diagnosis Diagnosis_4 ON Diagnosis_4.diagnosisid = Notes.diagnosis4) LEFT OUTER JOIN Diagnosis Diagnosis_5 ON Diagnosis_5.diagnosisid = Notes.diagnosis5) INNER JOIN Charges ON Charges.chargeid = Notes.chargeid)";
		daJoined.SelectCommand = selectCmd;
		}

		private void SetupDataAdapterCharges()
		{
			daCharges = new System.Data.OleDb.OleDbDataAdapter();
			OleDbCommand selectCmd = new System.Data.OleDb.OleDbCommand();
			selectCmd.Connection = connection;

			daCharges.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
				new System.Data.Common.DataTableMapping("Table", "Charges", new System.Data.Common.DataColumnMapping[] {
					   new System.Data.Common.DataColumnMapping("chargeid", "chargeid"),
					   new System.Data.Common.DataColumnMapping("primulatext", "primulatext"),
					   new System.Data.Common.DataColumnMapping("text", "text")})});
		
			selectCmd.CommandText = "SELECT chargeid, primulatext, [text] FROM Charges";

			daCharges.SelectCommand = selectCmd;
		}
		
		private void SetupDataAdapterDiagnosis()
		{
			daDiagnosis = new System.Data.OleDb.OleDbDataAdapter();
			OleDbCommand deleteCmd = new System.Data.OleDb.OleDbCommand();
			OleDbCommand insertCmd = new System.Data.OleDb.OleDbCommand();
			OleDbCommand selectCmd = new System.Data.OleDb.OleDbCommand();
			OleDbCommand updateCmd = new System.Data.OleDb.OleDbCommand();
			selectCmd.Connection = connection;
			insertCmd.Connection = connection;
			deleteCmd.Connection = connection;
			updateCmd.Connection = connection;


			daDiagnosis.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
					new System.Data.Common.DataTableMapping("Table", "Diagnosis", new System.Data.Common.DataColumnMapping[] {
						 new System.Data.Common.DataColumnMapping("diagnosisnumber", "diagnosisnumber"),
						 new System.Data.Common.DataColumnMapping("diagnosistext", "diagnosistext"),
						 new System.Data.Common.DataColumnMapping("diagnosisid", "diagnosisid")})});

			
			selectCmd.CommandText = "SELECT diagnosisnumber, diagnosistext, diagnosisid FROM Diagnosis";
			
			insertCmd.CommandText = "INSERT INTO Diagnosis(diagnosisnumber, diagnosistext) VALUES (?, ?)";
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("diagnosisnumber", System.Data.OleDb.OleDbType.VarWChar, 7, "diagnosisnumber"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("diagnosistext", System.Data.OleDb.OleDbType.VarWChar, 50, "diagnosistext"));
			
			updateCmd.CommandText = "UPDATE Diagnosis SET diagnosisnumber = ?, diagnosistext = ? WHERE (diagnosisid = ?) AND (d" +
				"iagnosisnumber = ? OR ? IS NULL AND diagnosisnumber IS NULL) AND (diagnosistext " +
				"= ? OR ? IS NULL AND diagnosistext IS NULL)";
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("diagnosisnumber", System.Data.OleDb.OleDbType.VarWChar, 7, "diagnosisnumber"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("diagnosistext", System.Data.OleDb.OleDbType.VarWChar, 50, "diagnosistext"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosisid", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosisid", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosisnumber", System.Data.OleDb.OleDbType.VarWChar, 7, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosisnumber", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosisnumber1", System.Data.OleDb.OleDbType.VarWChar, 7, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosisnumber", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosistext", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosistext", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosistext1", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosistext", System.Data.DataRowVersion.Original, null));
			
			deleteCmd.CommandText = "DELETE FROM Diagnosis WHERE (diagnosisid = ?) AND (diagnosisnumber = ? OR ? IS NULL AND di" +
				"agnosisnumber IS NULL) AND (diagnosistext = ? OR ? IS NULL AND diagnosistext IS " +
				"NULL)";
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosisid", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosisid", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosisnumber", System.Data.OleDb.OleDbType.VarWChar, 7, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosisnumber", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosisnumber1", System.Data.OleDb.OleDbType.VarWChar, 7, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosisnumber", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosistext", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosistext", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosistext1", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosistext", System.Data.DataRowVersion.Original, null));

			daDiagnosis.DeleteCommand = deleteCmd;
			daDiagnosis.InsertCommand = insertCmd;
			daDiagnosis.SelectCommand = selectCmd;
			daDiagnosis.UpdateCommand = updateCmd;
		}

		private void SetupDataAdapterNotes()
		{
			daNotes = new System.Data.OleDb.OleDbDataAdapter();
			OleDbCommand deleteCmd = new System.Data.OleDb.OleDbCommand();
			OleDbCommand insertCmd = new System.Data.OleDb.OleDbCommand();
			OleDbCommand selectCmd = new System.Data.OleDb.OleDbCommand();
			OleDbCommand updateCmd = new System.Data.OleDb.OleDbCommand();
			selectCmd.Connection = connection;
			insertCmd.Connection = connection;
			deleteCmd.Connection = connection;
			updateCmd.Connection = connection;

			
			
			
			daNotes.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
																										new System.Data.Common.DataTableMapping("Table", "Notes", new System.Data.Common.DataColumnMapping[] {
																																																				 new System.Data.Common.DataColumnMapping("actioncode", "actioncode"),
																																																				 new System.Data.Common.DataColumnMapping("chargeid", "chargeid"),
																																																				 new System.Data.Common.DataColumnMapping("createdatetime", "createdatetime"),
																																																				 new System.Data.Common.DataColumnMapping("diagnosis1", "diagnosis1"),
																																																				 new System.Data.Common.DataColumnMapping("diagnosis2", "diagnosis2"),
																																																				 new System.Data.Common.DataColumnMapping("diagnosis3", "diagnosis3"),
																																																				 new System.Data.Common.DataColumnMapping("diagnosis4", "diagnosis4"),
																																																				 new System.Data.Common.DataColumnMapping("diagnosis5", "diagnosis5"),
																																																				 new System.Data.Common.DataColumnMapping("noteid", "noteid"),
																																																				 new System.Data.Common.DataColumnMapping("newvisit", "newvisit"),
																																																				 new System.Data.Common.DataColumnMapping("note", "note"),
																																																				 new System.Data.Common.DataColumnMapping("patientfee", "patientfee"),
																																																				 new System.Data.Common.DataColumnMapping("patientid", "patientid"),
																																																				 new System.Data.Common.DataColumnMapping("primula", "primula"),
																																																				 new System.Data.Common.DataColumnMapping("signed", "signed"),
																																																				 new System.Data.Common.DataColumnMapping("signeddatetime", "signeddatetime"),
																																																				 new System.Data.Common.DataColumnMapping("visitdatetime", "visitdatetime"),
																																																				 new System.Data.Common.DataColumnMapping("visitnote", "visitnote")})});
			
			
			selectCmd.CommandText = "SELECT actioncode, chargeid, createdatetime, diagnosis1, diagnosis2, diagnosis3, " +
				"diagnosis4, diagnosis5, noteid, newvisit, [note], patientfee, patientid, primula, si" +
				"gned, signeddatetime, visitdatetime, visitnote FROM Notes";
			
			insertCmd.CommandText = @"INSERT INTO Notes(actioncode, chargeid, createdatetime, diagnosis1, diagnosis2, diagnosis3, diagnosis4, diagnosis5, newvisit, [note], patientfee, patientid, primula, signed, signeddatetime, visitdatetime, visitnote) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("actioncode", System.Data.OleDb.OleDbType.VarWChar, 5, "actioncode"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("chargeid", System.Data.OleDb.OleDbType.Integer, 0, "chargeid"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("createdatetime", System.Data.OleDb.OleDbType.VarWChar, 16, "createdatetime"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("diagnosis1", System.Data.OleDb.OleDbType.Integer, 0, "diagnosis1"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("diagnosis2", System.Data.OleDb.OleDbType.Integer, 0, "diagnosis2"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("diagnosis3", System.Data.OleDb.OleDbType.Integer, 0, "diagnosis3"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("diagnosis4", System.Data.OleDb.OleDbType.Integer, 0, "diagnosis4"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("diagnosis5", System.Data.OleDb.OleDbType.Integer, 0, "diagnosis5"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("newvisit", System.Data.OleDb.OleDbType.Boolean, 2, "newvisit"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("note", System.Data.OleDb.OleDbType.VarWChar, 0, "note"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("patientfee", System.Data.OleDb.OleDbType.VarWChar, 3, "patientfee"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("patientid", System.Data.OleDb.OleDbType.Integer, 0, "patientid"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("primula", System.Data.OleDb.OleDbType.Boolean, 2, "primula"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("signed", System.Data.OleDb.OleDbType.Boolean, 2, "signed"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("signeddatetime", System.Data.OleDb.OleDbType.VarWChar, 16, "signeddatetime"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("visitdatetime", System.Data.OleDb.OleDbType.VarWChar, 16, "visitdatetime"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("visitnote", System.Data.OleDb.OleDbType.Boolean, 2, "visitnote"));
			
			updateCmd.CommandText = @"UPDATE Notes SET actioncode = ?, chargeid = ?, createdatetime = ?, diagnosis1 = ?, diagnosis2 = ?, diagnosis3 = ?, diagnosis4 = ?, diagnosis5 = ?, newvisit = ?, [note] = ?, patientfee = ?, patientid = ?, primula = ?, signed = ?, signeddatetime = ?, visitdatetime = ?, visitnote = ? WHERE (noteid = ?) AND (actioncode = ? OR ? IS NULL AND actioncode IS NULL) AND (chargeid = ? OR ? IS NULL AND chargeid IS NULL) AND (createdatetime = ? OR ? IS NULL AND createdatetime IS NULL) AND (diagnosis1 = ? OR ? IS NULL AND diagnosis1 IS NULL) AND (diagnosis2 = ? OR ? IS NULL AND diagnosis2 IS NULL) AND (diagnosis3 = ? OR ? IS NULL AND diagnosis3 IS NULL) AND (diagnosis4 = ? OR ? IS NULL AND diagnosis4 IS NULL) AND (diagnosis5 = ? OR ? IS NULL AND diagnosis5 IS NULL) AND (newvisit = ?) AND (patientfee = ? OR ? IS NULL AND patientfee IS NULL) AND (patientid = ? OR ? IS NULL AND patientid IS NULL) AND (primula = ?) AND (signed = ?) AND (signeddatetime = ? OR ? IS NULL AND signeddatetime IS NULL) AND (visitdatetime = ? OR ? IS NULL AND visitdatetime IS NULL) AND (visitnote = ?)";
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("actioncode", System.Data.OleDb.OleDbType.VarWChar, 5, "actioncode"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("chargeid", System.Data.OleDb.OleDbType.Integer, 0, "chargeid"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("createdatetime", System.Data.OleDb.OleDbType.VarWChar, 16, "createdatetime"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("diagnosis1", System.Data.OleDb.OleDbType.Integer, 0, "diagnosis1"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("diagnosis2", System.Data.OleDb.OleDbType.Integer, 0, "diagnosis2"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("diagnosis3", System.Data.OleDb.OleDbType.Integer, 0, "diagnosis3"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("diagnosis4", System.Data.OleDb.OleDbType.Integer, 0, "diagnosis4"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("diagnosis5", System.Data.OleDb.OleDbType.Integer, 0, "diagnosis5"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("newvisit", System.Data.OleDb.OleDbType.Boolean, 2, "newvisit"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("note", System.Data.OleDb.OleDbType.VarWChar, 0, "note"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("patientfee", System.Data.OleDb.OleDbType.VarWChar, 3, "patientfee"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("patientid", System.Data.OleDb.OleDbType.Integer, 0, "patientid"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("primula", System.Data.OleDb.OleDbType.Boolean, 2, "primula"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("signed", System.Data.OleDb.OleDbType.Boolean, 2, "signed"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("signeddatetime", System.Data.OleDb.OleDbType.VarWChar, 16, "signeddatetime"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("visitdatetime", System.Data.OleDb.OleDbType.VarWChar, 16, "visitdatetime"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("visitnote", System.Data.OleDb.OleDbType.Boolean, 2, "visitnote"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_id", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "noteid", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_actioncode", System.Data.OleDb.OleDbType.VarWChar, 5, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "actioncode", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_actioncode1", System.Data.OleDb.OleDbType.VarWChar, 5, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "actioncode", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_chargeid", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "chargeid", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_chargeid1", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "chargeid", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_createdatetime", System.Data.OleDb.OleDbType.VarWChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "createdatetime", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_createdatetime1", System.Data.OleDb.OleDbType.VarWChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "createdatetime", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosis1", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis1", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosis11", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis1", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosis2", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis2", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosis21", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis2", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosis3", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis3", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosis31", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis3", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosis4", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis4", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosis41", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis4", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosis5", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis5", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosis51", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis5", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_newvisit", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "newvisit", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_patientfee", System.Data.OleDb.OleDbType.VarWChar, 3, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "patientfee", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_patientfee1", System.Data.OleDb.OleDbType.VarWChar, 3, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "patientfee", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_patientid", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "patientid", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_patientid1", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "patientid", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_primula", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "primula", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_signed", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "signed", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_signeddatetime", System.Data.OleDb.OleDbType.VarWChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "signeddatetime", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_signeddatetime1", System.Data.OleDb.OleDbType.VarWChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "signeddatetime", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_visitdatetime", System.Data.OleDb.OleDbType.VarWChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "visitdatetime", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_visitdatetime1", System.Data.OleDb.OleDbType.VarWChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "visitdatetime", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_visitnote", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "visitnote", System.Data.DataRowVersion.Original, null));
			
			deleteCmd.CommandText = @"DELETE FROM Notes WHERE (noteid = ?) AND (actioncode = ? OR ? IS NULL AND actioncode IS NULL) AND (chargeid = ? OR ? IS NULL AND chargeid IS NULL) AND (createdatetime = ? OR ? IS NULL AND createdatetime IS NULL) AND (diagnosis1 = ? OR ? IS NULL AND diagnosis1 IS NULL) AND (diagnosis2 = ? OR ? IS NULL AND diagnosis2 IS NULL) AND (diagnosis3 = ? OR ? IS NULL AND diagnosis3 IS NULL) AND (diagnosis4 = ? OR ? IS NULL AND diagnosis4 IS NULL) AND (diagnosis5 = ? OR ? IS NULL AND diagnosis5 IS NULL) AND (newvisit = ?) AND (patientfee = ? OR ? IS NULL AND patientfee IS NULL) AND (patientid = ? OR ? IS NULL AND patientid IS NULL) AND (primula = ?) AND (signed = ?) AND (signeddatetime = ? OR ? IS NULL AND signeddatetime IS NULL) AND (visitdatetime = ? OR ? IS NULL AND visitdatetime IS NULL) AND (visitnote = ?)";
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_id", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "noteid", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_actioncode", System.Data.OleDb.OleDbType.VarWChar, 5, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "actioncode", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_actioncode1", System.Data.OleDb.OleDbType.VarWChar, 5, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "actioncode", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_chargeid", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "chargeid", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_chargeid1", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "chargeid", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_createdatetime", System.Data.OleDb.OleDbType.VarWChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "createdatetime", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_createdatetime1", System.Data.OleDb.OleDbType.VarWChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "createdatetime", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosis1", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis1", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosis11", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis1", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosis2", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis2", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosis21", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis2", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosis3", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis3", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosis31", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis3", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosis4", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis4", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosis41", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis4", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosis5", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis5", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_diagnosis51", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis5", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_newvisit", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "newvisit", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_patientfee", System.Data.OleDb.OleDbType.VarWChar, 3, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "patientfee", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_patientfee1", System.Data.OleDb.OleDbType.VarWChar, 3, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "patientfee", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_patientid", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "patientid", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_patientid1", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "patientid", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_primula", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "primula", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_signed", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "signed", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_signeddatetime", System.Data.OleDb.OleDbType.VarWChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "signeddatetime", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_signeddatetime1", System.Data.OleDb.OleDbType.VarWChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "signeddatetime", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_visitdatetime", System.Data.OleDb.OleDbType.VarWChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "visitdatetime", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_visitdatetime1", System.Data.OleDb.OleDbType.VarWChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "visitdatetime", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_visitnote", System.Data.OleDb.OleDbType.Boolean, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "visitnote", System.Data.DataRowVersion.Original, null));
		
			
			daNotes.DeleteCommand = deleteCmd;
			daNotes.InsertCommand = insertCmd;
			daNotes.SelectCommand = selectCmd;
			daNotes.UpdateCommand = updateCmd;


		}

		private void SetupDataAdapterPatients()
		{
			daPatients = new System.Data.OleDb.OleDbDataAdapter();
			OleDbCommand deleteCmd = new System.Data.OleDb.OleDbCommand();
			OleDbCommand insertCmd = new System.Data.OleDb.OleDbCommand();
			OleDbCommand selectCmd = new System.Data.OleDb.OleDbCommand();
			OleDbCommand updateCmd = new System.Data.OleDb.OleDbCommand();
			selectCmd.Connection = connection;
			insertCmd.Connection = connection;
			deleteCmd.Connection = connection;
			updateCmd.Connection = connection;

			daPatients.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
				new System.Data.Common.DataTableMapping("Table", "Patients", new System.Data.Common.DataColumnMapping[] {
				new System.Data.Common.DataColumnMapping("city", "city"),
				new System.Data.Common.DataColumnMapping("firstname", "firstname"),
				new System.Data.Common.DataColumnMapping("homephone", "homephone"),
				new System.Data.Common.DataColumnMapping("patientid", "patientid"),
				new System.Data.Common.DataColumnMapping("info", "info"),
				new System.Data.Common.DataColumnMapping("mobilephone", "mobilephone"),
				new System.Data.Common.DataColumnMapping("freecarddate", "freecarddate"),
				new System.Data.Common.DataColumnMapping("personnumber", "personnumber"),
				new System.Data.Common.DataColumnMapping("street", "street"),
				new System.Data.Common.DataColumnMapping("surname", "surname"),
				new System.Data.Common.DataColumnMapping("workphone", "workphone"),
				new System.Data.Common.DataColumnMapping("zipcode", "zipcode")})});

			selectCmd.CommandText = "SELECT city, firstname, freecarddate, homephone, patientid, info, mobilephone, personnum" +
				"ber, street, surname, workphone, zipcode FROM Patients";
			
			insertCmd.CommandText = "INSERT INTO Patients(city, firstname, freecarddate, homephone, info, mobilephone," +
				" personnumber, street, surname, workphone, zipcode) VALUES (?, ?, ?, ?, ?, ?, ?," +
				" ?, ?, ?, ?)";
			
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("city", System.Data.OleDb.OleDbType.VarWChar, 50, "city"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("firstname", System.Data.OleDb.OleDbType.VarWChar, 50, "firstname"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("freecarddate", System.Data.OleDb.OleDbType.VarWChar, 10, "freecarddate"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("homephone", System.Data.OleDb.OleDbType.VarWChar, 50, "homephone"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("info", System.Data.OleDb.OleDbType.VarWChar, 50, "info"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("mobilephone", System.Data.OleDb.OleDbType.VarWChar, 50, "mobilephone"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("personnumber", System.Data.OleDb.OleDbType.VarWChar, 50, "personnumber"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("street", System.Data.OleDb.OleDbType.VarWChar, 50, "street"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("surname", System.Data.OleDb.OleDbType.VarWChar, 50, "surname"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("workphone", System.Data.OleDb.OleDbType.VarWChar, 50, "workphone"));
			insertCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("zipcode", System.Data.OleDb.OleDbType.VarWChar, 50, "zipcode"));
			
			updateCmd.CommandText = @"UPDATE Patients SET city = ?, firstname = ?, freecarddate = ?, homephone = ?, info = ?, mobilephone = ?, personnumber = ?, street = ?, surname = ?, workphone = ?, zipcode = ? WHERE (patientid = ?) AND (city = ? OR ? IS NULL AND city IS NULL) AND (firstname = ? OR ? IS NULL AND firstname IS NULL) AND (freecarddate = ? OR ? IS NULL AND freecarddate IS NULL) AND (homephone = ? OR ? IS NULL AND homephone IS NULL) AND (info = ? OR ? IS NULL AND info IS NULL) AND (mobilephone = ? OR ? IS NULL AND mobilephone IS NULL) AND (personnumber = ? OR ? IS NULL AND personnumber IS NULL) AND (street = ? OR ? IS NULL AND street IS NULL) AND (surname = ? OR ? IS NULL AND surname IS NULL) AND (workphone = ? OR ? IS NULL AND workphone IS NULL) AND (zipcode = ? OR ? IS NULL AND zipcode IS NULL)";
			
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("city", System.Data.OleDb.OleDbType.VarWChar, 50, "city"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("firstname", System.Data.OleDb.OleDbType.VarWChar, 50, "firstname"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("freecarddate", System.Data.OleDb.OleDbType.VarWChar, 10, "freecarddate"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("homephone", System.Data.OleDb.OleDbType.VarWChar, 50, "homephone"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("info", System.Data.OleDb.OleDbType.VarWChar, 50, "info"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("mobilephone", System.Data.OleDb.OleDbType.VarWChar, 50, "mobilephone"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("personnumber", System.Data.OleDb.OleDbType.VarWChar, 50, "personnumber"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("street", System.Data.OleDb.OleDbType.VarWChar, 50, "street"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("surname", System.Data.OleDb.OleDbType.VarWChar, 50, "surname"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("workphone", System.Data.OleDb.OleDbType.VarWChar, 50, "workphone"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("zipcode", System.Data.OleDb.OleDbType.VarWChar, 50, "zipcode"));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_id", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "patientid", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_city", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "city", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_city1", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "city", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_firstname", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "firstname", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_firstname1", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "firstname", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_freecarddate", System.Data.OleDb.OleDbType.VarWChar, 10, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "freecarddate", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_freecarddate1", System.Data.OleDb.OleDbType.VarWChar, 10, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "freecarddate", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_homephone", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "homephone", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_homephone1", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "homephone", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_info", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "info", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_info1", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "info", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_mobilephone", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "mobilephone", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_mobilephone1", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "mobilephone", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_personnumber", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "personnumber", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_personnumber1", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "personnumber", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_street", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "street", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_street1", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "street", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_surname", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "surname", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_surname1", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "surname", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_workphone", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "workphone", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_workphone1", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "workphone", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_zipcode", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "zipcode", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_zipcode1", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "zipcode", System.Data.DataRowVersion.Original, null));
			
			deleteCmd.CommandText = @"DELETE FROM Patients WHERE (patientid = ?) AND (city = ? OR ? IS NULL AND city IS NULL) AND (firstname = ? OR ? IS NULL AND firstname IS NULL) AND (freecarddate = ? OR ? IS NULL AND freecarddate IS NULL) AND (homephone = ? OR ? IS NULL AND homephone IS NULL) AND (info = ? OR ? IS NULL AND info IS NULL) AND (mobilephone = ? OR ? IS NULL AND mobilephone IS NULL) AND (personnumber = ? OR ? IS NULL AND personnumber IS NULL) AND (street = ? OR ? IS NULL AND street IS NULL) AND (surname = ? OR ? IS NULL AND surname IS NULL) AND (workphone = ? OR ? IS NULL AND workphone IS NULL) AND (zipcode = ? OR ? IS NULL AND zipcode IS NULL)";
			
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_id", System.Data.OleDb.OleDbType.Integer, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "patientid", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_city", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "city", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_city1", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "city", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_firstname", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "firstname", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_firstname1", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "firstname", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_freecarddate", System.Data.OleDb.OleDbType.VarWChar, 10, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "freecarddate", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_freecarddate1", System.Data.OleDb.OleDbType.VarWChar, 10, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "freecarddate", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_homephone", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "homephone", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_homephone1", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "homephone", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_info", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "info", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_info1", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "info", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_mobilephone", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "mobilephone", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_mobilephone1", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "mobilephone", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_personnumber", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "personnumber", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_personnumber1", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "personnumber", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_street", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "street", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_street1", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "street", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_surname", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "surname", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_surname1", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "surname", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_workphone", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "workphone", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_workphone1", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "workphone", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_zipcode", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "zipcode", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new System.Data.OleDb.OleDbParameter("Original_zipcode1", System.Data.OleDb.OleDbType.VarWChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "zipcode", System.Data.DataRowVersion.Original, null));


			daPatients.DeleteCommand = deleteCmd;
			daPatients.InsertCommand = insertCmd;
			daPatients.SelectCommand = selectCmd;
			daPatients.UpdateCommand = updateCmd;

		}
		
		#endregion DataAdapter setup

		/// <summary>
		/// Adds a new patient to the database. If the new patients personnumber already exists in the database
		/// this indicates either lack of personnumber-check or that existing patientinformation is supposed to be updated.
		/// Since this function expects control of personnumber at a higher level, it will call the Update function
		/// with the patient if the personnumber already exists.
		/// </summary>
		/// <param name="patient"></param>
		public void Add(Patient patient)
		{
			
			// Search for id and personnumber in table Patients
			if (IsAlreadyExsisting(patient))
			{
				Debug.WriteLine("" + this.GetType().Name + "(Add): En patient med detta personnummer finns redan inlagd, uppdaterar existerande data istället");
				Update(patient);
				return;
			}	

			System.Data.DataRow dr = dsMaster.Tables["Patients"].NewRow();
			dr["firstname"] = patient.Firstname;
			dr["surname"] = patient.Surname;
			dr["personnumber"] = patient.Personnumber;
			dr["street"] = patient.Street;
			dr["zipcode"] = patient.Zipcode;
			dr["city"] = patient.City;
			dr["homephone"] = patient.HomePhone;
			dr["workphone"] = patient.WorkPhone;
			dr["mobilephone"] = patient.MobilePhone;
			dr["info"] = patient.Info;
			dr["freecarddate"] = patient.FreecardDate;

			dsMaster.Tables["Patients"].Rows.Add(dr);
			daPatients.Update(dsMaster, "Patients");
		}

		public void Update(Patient patient)
		{
			System.Data.DataView dv = new System.Data.DataView();
			dv.Table = dsMaster.Tables["Patients"];
			dv.Sort = "patientid";
			
			if (patient != null)
			{
				int index = dv.Find(patient.Id);
				if (index == -1)
				{
					//The patient could not be found in the database
					Debug.WriteLine("" + this.GetType().Name + "(Update): En patient med detta ID kunde inte hittas i database, lägger till som ny data istället");
					Add(patient);
				}

				System.Data.DataRow dr = dv[index].Row;

				dr.BeginEdit();

				dr["firstname"] = patient.Firstname;
				dr["surname"] = patient.Surname;
				dr["personnumber"] = patient.Personnumber;
				dr["street"] = patient.Street;
				dr["zipcode"] = patient.Zipcode;
				dr["city"] = patient.City;
				dr["homephone"] = patient.HomePhone;
				dr["workphone"] = patient.WorkPhone;
				dr["mobilephone"] = patient.MobilePhone;
				dr["info"] = patient.Info;
				dr["freecarddate"] = patient.FreecardDate;
				
				dr.EndEdit();

				if (dr.RowState != System.Data.DataRowState.Unchanged)
					daPatients.Update(dsMaster, "Patients");
			}	
		}
		/// <summary>
		/// Creates a new patient object or updates an existing patient object.
		/// </summary>
		/// <param name="dr">A datarow with patient information.</param>
		/// <param name="aPatient">If the intention is to copy (i.e. update) an already existing patient object, then pass the object otherwise null</param>
		/// <returns></returns>
		public Patient CreatePatient(System.Data.DataRow dr, Patient aPatient)
		{
			Patient patient;

			if (aPatient == null)
				patient = new Patient();
			else
				patient = aPatient;

			if (!dr.HasErrors && dr != null)
			{
				
				if (dr["patientid"] != System.DBNull.Value)
					patient.Id = (int) dr["patientid"];
				else
					throw new Exception("Programmet försökte läsa in en patient som saknar id.");
				if (dr["surname"] != System.DBNull.Value)
					patient.Surname = (string) dr["surname"];
				if (dr["firstname"] != System.DBNull.Value)
					patient.Firstname = (string) dr["firstname"];
				if (dr["street"] != System.DBNull.Value)
					patient.Street = (string) dr["street"];
				if (dr["zipcode"] != System.DBNull.Value)
					patient.Zipcode = (string) dr["zipcode"];
				if (dr["city"] != System.DBNull.Value)
					patient.City = (string) dr["city"];
				if (dr["homephone"] != System.DBNull.Value)
					patient.HomePhone = (string) dr["homephone"];
				if (dr["workphone"] != System.DBNull.Value)
					patient.WorkPhone = (string) dr["workphone"];
				if (dr["mobilephone"] != System.DBNull.Value)
					patient.MobilePhone = (string) dr["mobilephone"];
				if (dr["personnumber"] != System.DBNull.Value)
					patient.Personnumber = (string) dr["personnumber"];
				else
					throw new Exception("Programmet försökte läsa in en patient som saknar personnummer.");
				if (dr["info"] != System.DBNull.Value)
					patient.Info = (string) dr["info"];
				if (dr["freecarddate"] != System.DBNull.Value)
					patient.FreecardDate = System.DateTime.Parse((string) dr["freecarddate"]);

			}

			return patient;
		}


		public void Add(Diagnosis diagnosis)
		{
			//TODO; Take care of the exception
			if (IsAlreadyExsisting(diagnosis))
				throw new Exception("En diagnos med samma diagnosnummer finns redan inlagd i database");
			
			System.Data.DataRow dr = dsMaster.Tables["Diagnosis"].NewRow();
			dr["diagnosisnumber"] = diagnosis.DiagnosisNumber;
			dr["diagnosistext"] = diagnosis.DiagnosisText;

			dsMaster.Tables["Diagnosis"].Rows.Add(dr);
			daDiagnosis.Update(dsMaster, "Diagnosis");

		}

		public void Add(Note note)
		{
			// Search for a note with the same date and patientid
			//TODO; Take care of the exception
			//Already existing notes with the same date is taken care of in functions calling this function
			/*if (IsAlreadyExsisting(note))
				throw new Exception("En anteckning med samma datum finns redan");*/
			
			System.Data.DataRow dr = dsMaster.Tables["Notes"].NewRow();
			dr["patientid"] = note.PatientId;
			dr["createdatetime"] = note.CreateDateTime.ToString("g");
			dr["signed"] = note.Signed;
			dr["signeddatetime"] = note.SignedDateTime.ToString("g");
			dr["note"] = note.JournalNote;
			dr["newvisit"] = note.NewVisit;
			dr["primula"] = note.Primula;
			dr["visitdatetime"] = note.VisitDateTime.ToString("g");
			dr["chargeid"] = note.ChargeId;
			dr["patientfee"] = note.PatientFee;
			dr["diagnosis1"] = note.Diagnosis1;
			dr["diagnosis2"] = note.Diagnosis2;
			dr["diagnosis3"] = note.Diagnosis3;
			dr["diagnosis4"] = note.Diagnosis4;
			dr["diagnosis5"] = note.Diagnosis5;
			dr["actioncode"] = note.ActionCode;
			dr["visitnote"] = note.VisitNote;
			
			dsMaster.Tables["Notes"].Rows.Add(dr);
			daNotes.Update(dsMaster, "Notes");
	
		}


		public void Update(Note note)
		{
			System.Data.DataView dv = new System.Data.DataView();
			dv.Table = dsMaster.Tables["Notes"];
			dv.Sort = "noteid";
			
			
			if (note != null)
			{
				int index = dv.Find(note.Id);					
				System.Data.DataRow dr;
	
				try 
				{
					dr = dv[index].Row;
				}
				catch(Exception exception)
				{
					throw new Exception(exception.Message + "\n\nAnteckningen kunde inte uppdateras eftersom dess id inte kunde hittas i databasen.");
				}

				dr.BeginEdit();
	
				//dr["noteid"] = note.Id;
				dr["patientId"] = note.PatientId;
				dr["createdatetime"] = note.CreateDateTime.ToString("g");
				dr["signed"] = note.Signed;
				dr["signeddatetime"] = note.SignedDateTime.ToString("g");
				dr["newvisit"] = note.NewVisit;
				dr["note"] = note.JournalNote;
				dr["primula"] = note.Primula;
				dr["visitdatetime"] = note.VisitDateTime.ToString("g");
				dr["chargeid"] = note.ChargeId;
				dr["patientfee"] = note.PatientFee;
				dr["diagnosis1"] = note.Diagnosis1;
				dr["diagnosis2"] = note.Diagnosis2;
				dr["diagnosis3"] = note.Diagnosis3;
				dr["diagnosis4"] = note.Diagnosis4;
				dr["diagnosis5"] = note.Diagnosis5;
				dr["actioncode"] = note.ActionCode;
				dr["visitnote"] = note.VisitNote;
				dr.EndEdit();

				daNotes.Update(dsMaster, "Notes");
			}
		}

		public void Update(Note[] noteArray)
		{
			//Setup a dataview in order to find the note that should be updated
			System.Data.DataView dv = new System.Data.DataView();
			dv.Table = dsMaster.Tables["Notes"];
			dv.Sort = "noteid";
			
			foreach (Note note in noteArray)
			{
				if (note != null)
				{
					int index = dv.Find(note.Id);
					System.Data.DataRow dr;

					try 
					{
						dr = dv[index].Row;
					}
					catch(Exception exception)
					{
						throw new Exception(exception.Message + "\n\nAnteckningen kunde inte uppdateras eftersom dess id inte kunde hittas i databasen.");
					}

					dr.BeginEdit();
	
					//dr["noteid"] = note.Id;
					dr["patientId"] = note.PatientId;
					dr["createdatetime"] = note.CreateDateTime.ToString("g");
					dr["signed"] = note.Signed;
					dr["signeddatetime"] = note.SignedDateTime.ToString("g");
					dr["newvisit"] = note.NewVisit;
					dr["note"] = note.JournalNote;
					dr["primula"] = note.Primula;
					dr["visitdatetime"] = note.VisitDateTime.ToString("g");
					dr["chargeid"] = note.ChargeId;
					dr["patientfee"] = note.PatientFee;
					dr["diagnosis1"] = note.Diagnosis1;
					dr["diagnosis2"] = note.Diagnosis2;
					dr["diagnosis3"] = note.Diagnosis3;
					dr["diagnosis4"] = note.Diagnosis4;
					dr["diagnosis5"] = note.Diagnosis5;
					dr["actioncode"] = note.ActionCode;
					dr["visitnote"] = note.VisitNote;
					dr.EndEdit();

				}
			}
			daNotes.Update(dsMaster, "Notes");
		}

		public Note CreateNote(System.Data.DataRow dr)
		{
			Note note = new Note();

			if (!dr.HasErrors && dr != null)
			{
				
				if (dr["noteid"] != System.DBNull.Value)
					note.Id = (int) dr["noteid"];
				else
					throw new Exception("Programmet försökte öppna en anteckning som saknar id.");
				if (dr["createdatetime"] != System.DBNull.Value)
					note.CreateDateTime = System.DateTime.Parse((string)dr["createdatetime"]);
				if (dr["signeddatetime"] != System.DBNull.Value)
					note.SignedDateTime = System.DateTime.Parse((string) dr["signedDateTime"]);
				if (dr["patientid"] != System.DBNull.Value)
					note.PatientId = (int) dr["patientid"];
				if (dr["signed"] != System.DBNull.Value)
					note.Signed = (bool) dr["signed"];
				if (dr["newvisit"] != System.DBNull.Value)
					note.NewVisit = (bool) dr["newvisit"];
				if (dr["note"] != System.DBNull.Value)
					note.JournalNote =  (string) dr["note"];
				if (dr["chargeid"] != System.DBNull.Value)
					note.ChargeId = (int) dr["chargeid"];
				if (dr["patientfee"] != System.DBNull.Value)
					note.PatientFee = (string) dr["patientfee"];
				if (dr["diagnosis1"] != System.DBNull.Value)
					note.Diagnosis1 = (int) dr["diagnosis1"];
				if (dr["diagnosis2"] != System.DBNull.Value)
					note.Diagnosis2 = (int) dr["diagnosis2"];
				if (dr["diagnosis3"] != System.DBNull.Value)
					note.Diagnosis3 = (int) dr["diagnosis3"];
				if (dr["diagnosis4"] != System.DBNull.Value)
					note.Diagnosis4 = (int) dr["diagnosis4"];
				if (dr["diagnosis5"] != System.DBNull.Value)
					note.Diagnosis5 = (int) dr["diagnosis5"];
				if (dr["actioncode"] != System.DBNull.Value)
					note.ActionCode = (string) dr["actioncode"];
				if (dr["primula"] != System.DBNull.Value)
					note.Primula = (bool) dr["primula"];
				if (dr["visitdatetime"] != System.DBNull.Value)
					note.VisitDateTime = System.DateTime.Parse((string) dr["visitdatetime"]);
				if (dr["visitnote"] != System.DBNull.Value)
					note.VisitNote = (bool) dr["visitnote"];
			}

			return note;
		}

		public bool Delete(Note aNote)
		{
			//Setup a dataview in order to find the note that should be updated
			System.Data.DataView dv = new System.Data.DataView();
			dv.Table = dsMaster.Tables["Notes"];
			dv.Sort = "noteid";
			
			if (aNote != null)
			{
					int index = dv.Find(aNote.Id);
					System.Data.DataRow dr;

					try 
					{
						dr = dv[index].Row;
					}
					catch(Exception exception)
					{
						throw new Exception(exception.Message + "\n\nAnteckningen kunde inte tas bort eftersom dess id inte kunde hittas i databasen.");
					}
				
				dsMaster.Tables["Notes"].Rows.Remove(dr);

				daNotes.Update(dsMaster, "Notes");

				Debug.WriteLine("Anteckningen " + aNote.Id.ToString() + " " + aNote.VisitDateTime.ToShortDateString() + " " + aNote.JournalNote + " togs bort.");

				connection.Close();

				return true;
			}
			return false;
		}

		public Diagnosis CreateDiagnosis(System.Data.DataRow dr)
		{

			Diagnosis diagnosis = new Diagnosis();

			if (!dr.HasErrors && dr != null)
			{
				object o = dr["diagnosisnumber"];
				if (o == System.DBNull.Value)
				{
					diagnosis = null;
				}
				diagnosis.Id = (int) dr["diagnosisid"];
				diagnosis.DiagnosisText = (string) dr["diagnosistext"];
				diagnosis.DiagnosisNumber = (string) dr["diagnosisnumber"];
			}

			return diagnosis;
		}

		/// <summary>
		/// This will only work with the joined table!!!
		/// </summary>
		/// <param name="dr"></param>
		/// <returns></returns>
		public Charge CreateCharge(System.Data.DataRow dr)
		{
			//TODO: Fix this so it works with the Charge table as well
			Charge charge = new Charge();

			if (!dr.HasErrors && dr != null)
			{
				charge.Id = (int)dr["chargeid"];
				charge.Description = (string) dr["chargetext"];
				charge.PrimulaCharachter = (string) dr["chargeprimula"];
			}

			return charge;
		}

		public ArrayList FindDiagnosisNumbers(Note note)
		{
			ArrayList result = new ArrayList();
			
			//TODO: link notes with diagnosis in a better way
			DataView dv = dsMaster.Tables["Diagnosis"].DefaultView;
			dv.Sort = "diagnosisid";
			foreach (int diagnosisId in note.DiagnosisArray)
			{
				int i = dv.Find(diagnosisId);
				if (i > -1)
				{
					object o = dv[i]["diagnosisnumber"];
					if (o != System.DBNull.Value)
						result.Add((string) o);
				}
			}
			
			return result;
		}

		public int FindMatchingNotes(string rowFilter)
		{
			DataView dv = new DataView();
			dv.Table = dsMaster.Tables["Notes"];
			dv.Sort = "noteid";
			dv.RowFilter = rowFilter;

			//TODO: Return ArrayList with Note objects
			return dv.Count;
		}

		public int[] FindLatestVisitNote(System.Data.DataRowView selectedPatientRow)
		{
			System.Data.DataSet ds = new DataSet();
			ds.Merge(selectedPatientRow.Row.GetChildRows("PatientNotes"));
			int[] diagnosisArray = new int[5] {0, 0, 0, 0, 0};
			if (ds.Tables.Count > 0)
			{
				System.Data.DataView dv = ds.Tables[0].DefaultView;
				dv.RowFilter = "visitnote = true";
				dv.Sort = "visitdatetime DESC";
				
				if (dv.Count > 0)
				{
					diagnosisArray[0] = (int)dv[0]["diagnosis1"];
					diagnosisArray[1] = (int)dv[0]["diagnosis2"];
					diagnosisArray[2] = (int)dv[0]["diagnosis3"];
					diagnosisArray[3] = (int)dv[0]["diagnosis4"];
					diagnosisArray[4] = (int)dv[0]["diagnosis5"];
				}
			}
				
			return diagnosisArray;
		}





		public bool IsAlreadyExsisting(Patient patient)
		{
			DataView dv = new DataView();
			dv.Table = dsMaster.Tables["Patients"];
			dv.Sort = "patientid";
			dv.RowFilter = "personnumber='" + patient.Personnumber + "'";
			if (dv.Count > 0)
				return true;
			else
				return false;

		}

		

		public Note ReturnAlreadyExsisting(Note note)
		{
			DataView dv = new DataView();
			dv.Table = dsMaster.Tables["Notes"];
			//dv.Sort = "createdatetime";
			dv.RowFilter = "SUBSTRING(createdatetime, 1, 10)='" + note.CreateDateTime.ToShortDateString() + "'" + " AND patientid=" + note.PatientId;
			dv.Sort = "visitdatetime DESC";
			if (dv.Count > 0)
			{
				//Return the latest created note
				return CreateNote(dv[0].Row);
			}
			else
				return null;

		}

		public Patient ReturnAlreadyExsisting(Patient patient)
		{
			DataView dv = new DataView();
			dv.Table = dsMaster.Tables["Patients"];
			dv.RowFilter = "personnumber='" + patient.Personnumber + "'";
			
			if (dv.Count == 1)
			{
				return CreatePatient(dv[0].Row, patient);
			}
			else if (dv.Count < 1)
				return null;
			else
				throw new Exception("Allvarligt fel. Det finns redan två existerande patienter med samma personnummer.");

		}


		public bool IsAlreadyExsisting(Note note)
		{
			DataView dv = new DataView();
			dv.Table = dsMaster.Tables["Notes"];
			//dv.Sort = "createdatetime";
			dv.RowFilter = "SUBSTRING(createdatetime, 1, 10)='" + note.CreateDateTime.ToShortDateString() + "'" + " AND patientid=" + note.PatientId;
			if (dv.Count > 0)
			{
				return true;
			}
			else
				return false;

		}

		private bool IsAlreadyExsisting(Diagnosis diagnosis)
		{
			DataView dv = new DataView();
			dv.Table = dsMaster.Tables["Diagnosis"];
			
			dv.RowFilter = "diagnosisnumber = '" + diagnosis.DiagnosisNumber + "'";
			if (dv.Count > 0)
				return true;
			else
				return false;

		}		


		public int QueryDatabase(string aSqlCommand)
		{
			if (aSqlCommand.Length == 0)
				return -1;
			OleDbCommand command = new OleDbCommand(aSqlCommand, connection);
			connection.Open();
			int result = -1;
			try 
			{
				result = (int)command.ExecuteScalar();
			}
			finally
			{
				connection.Close();
			}
			return result;
		}


		
		private void Patients_RowChanging( object sender, DataRowChangeEventArgs e )
		{
			if (e.Action != DataRowAction.Delete)
				Debug.WriteLine( "Patients_Row_Changing Event: " + e.Row["surname"].ToString() +e.Action.ToString());
			//TODO: To update the joined table (do this in a better way)
			Debug.WriteLine("Joined table is updated");
			dsMaster.Tables["Joined"].Clear();
			daJoined.Fill(dsMaster, "Joined");
		}

		private  void Notes_RowChanging( object sender, DataRowChangeEventArgs e )
		{
			//if (e.Action != DataRowAction.Delete)
				//Debug.WriteLine( "Notes_Row_Changing Event: " + e.Row["note"].ToString() +e.Action.ToString());
			Debug.WriteLine("Notes_RowChanging: " + e.Action.ToString());
			//TODO: To update the joined table (do this in a better way)
			Debug.WriteLine("Joined table is updated");
			dsMaster.Tables["Joined"].Clear();
			daJoined.Fill(dsMaster, "Joined");
		}

		private void Diagnosis_RowChanging( object sender, DataRowChangeEventArgs e )
		{
			if (e.Action != DataRowAction.Delete)
				Debug.WriteLine( "Diagnosis_Row_Changing Event: " + e.Row["diagnosistext"].ToString() +e.Action.ToString());
			//TODO: To update the joined table (do this in a better way)
			Debug.WriteLine("Joined table is updated");
			dsMaster.Tables["Joined"].Clear();
			daJoined.Fill(dsMaster, "Joined");
		}

		private void Connection_StateChange(object sender, System.Data.StateChangeEventArgs e)
		{
			Debug.WriteLine("Database status: " + e.CurrentState.ToString() + " (from state: " + e.OriginalState.ToString() + ")");
		}

	}
}
