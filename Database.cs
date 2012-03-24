using System;
using System.Data;
using MySql.Data.MySqlClient;
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
		private MySql.Data.MySqlClient.MySqlConnection connection;
		private System.Data.DataSet dsMaster;

        private MySql.Data.MySqlClient.MySqlDataAdapter daNotes;
		private MySql.Data.MySqlClient.MySqlDataAdapter daPatients;
		private MySql.Data.MySqlClient.MySqlDataAdapter daDiagnosis;
		private MySql.Data.MySqlClient.MySqlDataAdapter daCharges;
		private MySql.Data.MySqlClient.MySqlDataAdapter daJoined;

		public DataSet DsMaster
		{
			get { return dsMaster;}
		}

		///<summary>
		/// Setups the database and the dataadapters.
		///</summary> 
		public Database(string fileName, string password)
		{
            connection = new MySqlConnection();

            connection.ConnectionString = "server=mysql334.loopia.se;uid=doris@d49694;pwd=" + password + ";database=dorisruberg_se;port=3306;Allow User Variables=True";
            //connection.ConnectionString = "server=localhost;uid=root;pwd=" + password + ";database=dorisruberg_se;port=3306;Allow User Variables=True";
			
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
			daJoined = new MySql.Data.MySqlClient.MySqlDataAdapter();
			MySqlCommand selectCmd = new MySql.Data.MySqlClient.MySqlCommand();
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
			
		selectCmd.CommandText = @"SELECT Charges.text AS chargetext, Charges.primulatext AS chargeprimula, Diagnosis_1.diagnosistext AS diagnosistext1, Diagnosis_1.diagnosisnumber AS diagnosisnumber1, Diagnosis_2.diagnosisnumber AS diagnosisnumber2, Diagnosis_3.diagnosisnumber AS diagnosisnumber3, Diagnosis_4.diagnosisnumber AS diagnosisnumber4, Diagnosis_5.diagnosisnumber AS diagnosisnumber5, Patients.city, Patients.firstname, Patients.homephone, Patients.patientid, Patients.info, Patients.mobilephone, Patients.freecarddate, Patients.personnumber, Patients.street, Patients.surname, Patients.workphone, Patients.zipcode, Notes.actioncode, Notes.visitnote, Notes.chargeid, Notes.createdatetime, Notes.diagnosis1, Notes.diagnosis2, Notes.diagnosis3, Notes.diagnosis4, Notes.diagnosis5, Notes.noteid, Notes.newvisit, Notes.note, Notes.patientfee, Notes.primula, Notes.signed, Notes.signeddatetime, Notes.visitdatetime FROM (((((((Patients INNER JOIN Notes ON Patients.patientid = Notes.patientid) LEFT OUTER JOIN Diagnosis Diagnosis_1 ON Diagnosis_1.diagnosisid = Notes.diagnosis1) LEFT OUTER JOIN Diagnosis Diagnosis_2 ON Diagnosis_2.diagnosisid = Notes.diagnosis2) LEFT OUTER JOIN Diagnosis Diagnosis_3 ON Diagnosis_3.diagnosisid = Notes.diagnosis3) LEFT OUTER JOIN Diagnosis Diagnosis_4 ON Diagnosis_4.diagnosisid = Notes.diagnosis4) LEFT OUTER JOIN Diagnosis Diagnosis_5 ON Diagnosis_5.diagnosisid = Notes.diagnosis5) INNER JOIN Charges ON Charges.chargeid = Notes.chargeid)";
		daJoined.SelectCommand = selectCmd;
		}

		private void SetupDataAdapterCharges()
		{
			daCharges = new MySql.Data.MySqlClient.MySqlDataAdapter();
			MySqlCommand selectCmd = new MySql.Data.MySqlClient.MySqlCommand();
			selectCmd.Connection = connection;

			daCharges.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
				new System.Data.Common.DataTableMapping("Table", "Charges", new System.Data.Common.DataColumnMapping[] {
					   new System.Data.Common.DataColumnMapping("chargeid", "chargeid"),
					   new System.Data.Common.DataColumnMapping("primulatext", "primulatext"),
					   new System.Data.Common.DataColumnMapping("text", "text")})});
		
			selectCmd.CommandText = "SELECT chargeid, primulatext, text FROM Charges";

			daCharges.SelectCommand = selectCmd;
		}
		
		private void SetupDataAdapterDiagnosis()
		{
			daDiagnosis = new MySql.Data.MySqlClient.MySqlDataAdapter();
			MySqlCommand deleteCmd = new MySql.Data.MySqlClient.MySqlCommand();
			MySqlCommand insertCmd = new MySql.Data.MySqlClient.MySqlCommand();
			MySqlCommand selectCmd = new MySql.Data.MySqlClient.MySqlCommand();
			MySqlCommand updateCmd = new MySql.Data.MySqlClient.MySqlCommand();
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

            insertCmd.CommandText = "INSERT INTO Diagnosis(diagnosisnumber, diagnosistext) VALUES (?diagnosisnumber, ?diagnosistext)";
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?diagnosisnumber", MySql.Data.MySqlClient.MySqlDbType.VarChar, 7, "diagnosisnumber"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?diagnosistext", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, "diagnosistext"));

            updateCmd.CommandText = "UPDATE Diagnosis SET diagnosisnumber = ?diagnosisnumber, diagnosistext = ?diagnosistext WHERE (diagnosisid = ?diagnosisid)";
            updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?diagnosisid", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, "diagnosisid"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?diagnosisnumber", MySql.Data.MySqlClient.MySqlDbType.VarChar, 7, "diagnosisnumber"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?diagnosistext", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, "diagnosistext"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosisid", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosisid", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosisnumber", MySql.Data.MySqlClient.MySqlDbType.VarChar, 7, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosisnumber", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosisnumber1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 7, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosisnumber", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosistext", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosistext", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosistext1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosistext", System.Data.DataRowVersion.Original, null));

            deleteCmd.CommandText = "DELETE FROM Diagnosis WHERE (diagnosisid = ?diagnosisid)";
            deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?diagnosisid", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, "diagnosisid"));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosisid", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosisid", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosisnumber", MySql.Data.MySqlClient.MySqlDbType.VarChar, 7, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosisnumber", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosisnumber1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 7, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosisnumber", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosistext", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosistext", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosistext1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosistext", System.Data.DataRowVersion.Original, null));

			daDiagnosis.DeleteCommand = deleteCmd;
			daDiagnosis.InsertCommand = insertCmd;
			daDiagnosis.SelectCommand = selectCmd;
			daDiagnosis.UpdateCommand = updateCmd;
		}

		private void SetupDataAdapterNotes()
		{
			daNotes = new MySql.Data.MySqlClient.MySqlDataAdapter();
			MySqlCommand deleteCmd = new MySql.Data.MySqlClient.MySqlCommand();
			MySqlCommand insertCmd = new MySql.Data.MySqlClient.MySqlCommand();
			MySqlCommand selectCmd = new MySql.Data.MySqlClient.MySqlCommand();
			MySqlCommand updateCmd = new MySql.Data.MySqlClient.MySqlCommand();
			selectCmd.Connection = connection;
			insertCmd.Connection = connection;
			deleteCmd.Connection = connection;
			updateCmd.Connection = connection;
            
            daNotes.UpdateBatchSize = 0;
            updateCmd.UpdatedRowSource = UpdateRowSource.None;

			
			
			
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
				"diagnosis4, diagnosis5, noteid, newvisit, note, patientfee, patientid, primula, si" +
				"gned, signeddatetime, visitdatetime, visitnote FROM Notes";

            insertCmd.CommandText = "INSERT INTO Notes(actioncode, chargeid, createdatetime, diagnosis1, diagnosis2, diagnosis3, diagnosis4, diagnosis5, newvisit, note, patientfee, patientid, primula, signed, signeddatetime, visitdatetime, visitnote) VALUES (?actioncode, ?chargeid, ?createdatetime, ?diagnosis1, ?diagnosis2, ?diagnosis3, ?diagnosis4, ?diagnosis5, ?newvisit, ?note, ?patientfee, ?patientid, ?primula, ?signed, ?signeddatetime, ?visitdatetime, ?visitnote)";
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?actioncode", MySql.Data.MySqlClient.MySqlDbType.VarChar, 5, "actioncode"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?chargeid", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, "chargeid"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?createdatetime", MySql.Data.MySqlClient.MySqlDbType.VarChar, 16, "createdatetime"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?diagnosis1", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, "diagnosis1"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?diagnosis2", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, "diagnosis2"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?diagnosis3", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, "diagnosis3"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?diagnosis4", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, "diagnosis4"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?diagnosis5", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, "diagnosis5"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?newvisit", MySql.Data.MySqlClient.MySqlDbType.Int16, 2, "newvisit"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?note", MySql.Data.MySqlClient.MySqlDbType.VarChar, 0, "note"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?patientfee", MySql.Data.MySqlClient.MySqlDbType.VarChar, 3, "patientfee"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?patientid", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, "patientid"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?primula", MySql.Data.MySqlClient.MySqlDbType.Int16, 2, "primula"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?signed", MySql.Data.MySqlClient.MySqlDbType.Int16, 2, "signed"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?signeddatetime", MySql.Data.MySqlClient.MySqlDbType.VarChar, 16, "signeddatetime"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?visitdatetime", MySql.Data.MySqlClient.MySqlDbType.VarChar, 16, "visitdatetime"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?visitnote", MySql.Data.MySqlClient.MySqlDbType.Int16, 2, "visitnote"));

            updateCmd.CommandText = "UPDATE Notes SET actioncode = ?actioncode, chargeid = ?chargeid, createdatetime = ?createdatetime, diagnosis1 = ?diagnosis1, diagnosis2 = ?diagnosis2, diagnosis3 = ?diagnosis3, diagnosis4 = ?diagnosis4, diagnosis5 = ?diagnosis5, newvisit = ?newvisit, note = ?note, patientfee = ?patientfee, patientid = ?patientid, primula = ?primula, signed = ?signed, signeddatetime = ?signeddatetime, visitdatetime = ?visitdatetime, visitnote = ?visitnote WHERE (noteid = ?noteid)";
            updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?noteid", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, "noteid"));
            updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?actioncode", MySql.Data.MySqlClient.MySqlDbType.VarChar, 5, "actioncode"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?chargeid", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, "chargeid"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?createdatetime", MySql.Data.MySqlClient.MySqlDbType.VarChar, 16, "createdatetime"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?diagnosis1", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, "diagnosis1"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?diagnosis2", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, "diagnosis2"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?diagnosis3", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, "diagnosis3"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?diagnosis4", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, "diagnosis4"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?diagnosis5", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, "diagnosis5"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?newvisit", MySql.Data.MySqlClient.MySqlDbType.Int16, 2, "newvisit"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?note", MySql.Data.MySqlClient.MySqlDbType.VarChar, 0, "note"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?patientfee", MySql.Data.MySqlClient.MySqlDbType.VarChar, 3, "patientfee"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?patientid", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, "patientid"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?primula", MySql.Data.MySqlClient.MySqlDbType.Int16, 2, "primula"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?signed", MySql.Data.MySqlClient.MySqlDbType.Int16, 2, "signed"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?signeddatetime", MySql.Data.MySqlClient.MySqlDbType.VarChar, 16, "signeddatetime"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?visitdatetime", MySql.Data.MySqlClient.MySqlDbType.VarChar, 16, "visitdatetime"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?visitnote", MySql.Data.MySqlClient.MySqlDbType.Int16, 2, "visitnote"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_id", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "noteid", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_actioncode", MySql.Data.MySqlClient.MySqlDbType.VarChar, 5, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "actioncode", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_actioncode1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 5, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "actioncode", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_chargeid", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "chargeid", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_chargeid1", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "chargeid", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_createdatetime", MySql.Data.MySqlClient.MySqlDbType.VarChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "createdatetime", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_createdatetime1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "createdatetime", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosis1", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis1", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosis11", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis1", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosis2", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis2", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosis21", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis2", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosis3", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis3", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosis31", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis3", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosis4", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis4", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosis41", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis4", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosis5", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis5", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosis51", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis5", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_newvisit", MySql.Data.MySqlClient.MySqlDbType.Int16, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "newvisit", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_patientfee", MySql.Data.MySqlClient.MySqlDbType.VarChar, 3, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "patientfee", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_patientfee1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 3, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "patientfee", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_patientid", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "patientid", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_patientid1", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "patientid", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_primula", MySql.Data.MySqlClient.MySqlDbType.Int16, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "primula", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_signed", MySql.Data.MySqlClient.MySqlDbType.Int16, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "signed", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_signeddatetime", MySql.Data.MySqlClient.MySqlDbType.VarChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "signeddatetime", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_signeddatetime1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "signeddatetime", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_visitdatetime", MySql.Data.MySqlClient.MySqlDbType.VarChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "visitdatetime", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_visitdatetime1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "visitdatetime", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_visitnote", MySql.Data.MySqlClient.MySqlDbType.Int16, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "visitnote", System.Data.DataRowVersion.Original, null));

            deleteCmd.CommandText = "DELETE FROM Notes WHERE (noteid = ?noteid)";
            deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?noteid", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, "noteid"));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_id", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "noteid", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_actioncode", MySql.Data.MySqlClient.MySqlDbType.VarChar, 5, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "actioncode", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_actioncode1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 5, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "actioncode", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_chargeid", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "chargeid", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_chargeid1", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "chargeid", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_createdatetime", MySql.Data.MySqlClient.MySqlDbType.VarChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "createdatetime", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_createdatetime1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "createdatetime", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosis1", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis1", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosis11", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis1", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosis2", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis2", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosis21", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis2", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosis3", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis3", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosis31", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis3", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosis4", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis4", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosis41", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis4", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosis5", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis5", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_diagnosis51", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "diagnosis5", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_newvisit", MySql.Data.MySqlClient.MySqlDbType.Int16, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "newvisit", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_patientfee", MySql.Data.MySqlClient.MySqlDbType.VarChar, 3, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "patientfee", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_patientfee1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 3, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "patientfee", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_patientid", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "patientid", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_patientid1", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "patientid", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_primula", MySql.Data.MySqlClient.MySqlDbType.Int16, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "primula", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_signed", MySql.Data.MySqlClient.MySqlDbType.Int16, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "signed", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_signeddatetime", MySql.Data.MySqlClient.MySqlDbType.VarChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "signeddatetime", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_signeddatetime1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "signeddatetime", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_visitdatetime", MySql.Data.MySqlClient.MySqlDbType.VarChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "visitdatetime", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_visitdatetime1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 16, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "visitdatetime", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_visitnote", MySql.Data.MySqlClient.MySqlDbType.Int16, 2, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "visitnote", System.Data.DataRowVersion.Original, null));
		
			
			daNotes.DeleteCommand = deleteCmd;
			daNotes.InsertCommand = insertCmd;
			daNotes.SelectCommand = selectCmd;
			daNotes.UpdateCommand = updateCmd;


		}

		private void SetupDataAdapterPatients()
		{
			daPatients = new MySql.Data.MySqlClient.MySqlDataAdapter();
			MySqlCommand deleteCmd = new MySql.Data.MySqlClient.MySqlCommand();
			MySqlCommand insertCmd = new MySql.Data.MySqlClient.MySqlCommand();
			MySqlCommand selectCmd = new MySql.Data.MySqlClient.MySqlCommand();
			MySqlCommand updateCmd = new MySql.Data.MySqlClient.MySqlCommand();
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

            insertCmd.CommandText = "INSERT INTO Patients(city, firstname, freecarddate, homephone, info, mobilephone, personnumber, street, surname, workphone, zipcode) VALUES (?city, ?firstname, ?freecarddate, ?homephone, ?info, ?mobilephone, ?personnumber, ?street, ?surname, ?workphone, ?zipcode)";
			
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?city", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, "city"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?firstname", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, "firstname"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?freecarddate", MySql.Data.MySqlClient.MySqlDbType.VarChar, 10, "freecarddate"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?homephone", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, "homephone"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?info", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, "info"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?mobilephone", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, "mobilephone"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?personnumber", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, "personnumber"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?street", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, "street"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?surname", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, "surname"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?workphone", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, "workphone"));
			insertCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?zipcode", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, "zipcode"));

            updateCmd.CommandText = "UPDATE Patients SET city = ?city, firstname = ?firstname, freecarddate = ?freecarddate, homephone = ?homephone, info = ?info, mobilephone = ?mobilephone, personnumber = ?personnumber, street = ?street, surname = ?surname, workphone = ?workphone, zipcode = ?zipcode WHERE (patientid = ?patientid)";
            updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?patientid", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, "patientid"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?city", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, "city"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?firstname", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, "firstname"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?freecarddate", MySql.Data.MySqlClient.MySqlDbType.VarChar, 10, "freecarddate"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?homephone", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, "homephone"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?info", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, "info"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?mobilephone", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, "mobilephone"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?personnumber", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, "personnumber"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?street", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, "street"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?surname", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, "surname"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?workphone", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, "workphone"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?zipcode", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, "zipcode"));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_id", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "patientid", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_city", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "city", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_city1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "city", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_firstname", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "firstname", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_firstname1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "firstname", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_freecarddate", MySql.Data.MySqlClient.MySqlDbType.VarChar, 10, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "freecarddate", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_freecarddate1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 10, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "freecarddate", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_homephone", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "homephone", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_homephone1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "homephone", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_info", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "info", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_info1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "info", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_mobilephone", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "mobilephone", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_mobilephone1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "mobilephone", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_personnumber", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "personnumber", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_personnumber1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "personnumber", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_street", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "street", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_street1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "street", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_surname", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "surname", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_surname1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "surname", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_workphone", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "workphone", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_workphone1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "workphone", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_zipcode", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "zipcode", System.Data.DataRowVersion.Original, null));
			updateCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_zipcode1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "zipcode", System.Data.DataRowVersion.Original, null));
			
			deleteCmd.CommandText = "DELETE FROM Patients WHERE (patientid = ?)";
            deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("?patientid", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, "patientid"));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_id", MySql.Data.MySqlClient.MySqlDbType.Int32, 0, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "patientid", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_city", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "city", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_city1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "city", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_firstname", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "firstname", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_firstname1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "firstname", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_freecarddate", MySql.Data.MySqlClient.MySqlDbType.VarChar, 10, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "freecarddate", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_freecarddate1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 10, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "freecarddate", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_homephone", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "homephone", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_homephone1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "homephone", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_info", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "info", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_info1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "info", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_mobilephone", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "mobilephone", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_mobilephone1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "mobilephone", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_personnumber", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "personnumber", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_personnumber1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "personnumber", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_street", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "street", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_street1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "street", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_surname", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "surname", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_surname1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "surname", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_workphone", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "workphone", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_workphone1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "workphone", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_zipcode", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "zipcode", System.Data.DataRowVersion.Original, null));
			deleteCmd.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("Original_zipcode1", MySql.Data.MySqlClient.MySqlDbType.VarChar, 50, System.Data.ParameterDirection.Input, false, ((System.Byte)(0)), ((System.Byte)(0)), "zipcode", System.Data.DataRowVersion.Original, null));


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
            // Update the joined table
            dsMaster.Tables["Joined"].Clear();
            daJoined.Fill(dsMaster, "Joined");
		}

		public void Update(Note[] noteArray)
		{
			//Setup a dataview in order to find the note that should be updated
			System.Data.DataView dv = new System.Data.DataView();
			DataTable dt = dsMaster.Tables["Notes"];

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

                    
					//dr.BeginEdit();
	               
					//dr["noteid"] = note.Id;
                    
					dt.Rows[index]["patientId"] = note.PatientId;
					dt.Rows[index]["createdatetime"] = note.CreateDateTime.ToString("g");
					dt.Rows[index]["signed"] = note.Signed;
					dt.Rows[index]["signeddatetime"] = note.SignedDateTime.ToString("g");
					dt.Rows[index]["newvisit"] = note.NewVisit;
					dt.Rows[index]["note"] = note.JournalNote;
					dt.Rows[index]["primula"] = note.Primula;
					dt.Rows[index]["visitdatetime"] = note.VisitDateTime.ToString("g");
					dt.Rows[index]["chargeid"] = note.ChargeId;
					dt.Rows[index]["patientfee"] = note.PatientFee;
					dt.Rows[index]["diagnosis1"] = note.Diagnosis1;
					dt.Rows[index]["diagnosis2"] = note.Diagnosis2;
					dt.Rows[index]["diagnosis3"] = note.Diagnosis3;
					dt.Rows[index]["diagnosis4"] = note.Diagnosis4;
					dt.Rows[index]["diagnosis5"] = note.Diagnosis5;
					dt.Rows[index]["actioncode"] = note.ActionCode;
					dt.Rows[index]["visitnote"] = note.VisitNote;
					//dr.EndEdit();

				}
			}
			daNotes.Update(dt);

            // Update the joined table
            dsMaster.Tables["Joined"].Clear();
            daJoined.Fill(dsMaster, "Joined");
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

        public bool IsPatientIdValid(int id)
        {
            DataView dv = new DataView();
            dv.Table = dsMaster.Tables["Patients"];
            dv.Sort = "patientid";
            if (dv.Find(id) != -1)
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
			MySqlCommand command = new MySqlCommand(aSqlCommand, connection);
			connection.Open();
			int result = -1;
			try 
			{
				result = System.Convert.ToInt32(command.ExecuteScalar());
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
			//Debug.WriteLine("Joined table is updated");
			//dsMaster.Tables["Joined"].Clear();
			//daJoined.Fill(dsMaster, "Joined");
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
