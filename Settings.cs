using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;

namespace RehabLight
{
	/// <summary>
	/// Summary description for Settings.
	/// </summary>
	public class Settings : System.Windows.Forms.Form
	{
		#region Variables
		private System.Windows.Forms.TreeView treeView;
		private static XmlDocument xmlSettings;
		private static string filename;
		private XmlNode selectedNode;
		private XmlNode selectedChildNode;
		private System.Windows.Forms.Button btnAddNode;
		private System.Windows.Forms.TextBox txtNodeText;

		
		private System.Windows.Forms.Button btnRemoveNode;
		private System.Windows.Forms.CheckBox chkCheckPersonnumber;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		
		public Settings()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			UpdateTreeView();	

			chkCheckPersonnumber.Checked = Settings.isCheckPersonnumberEnabled;
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
			this.treeView = new System.Windows.Forms.TreeView();
			this.btnAddNode = new System.Windows.Forms.Button();
			this.txtNodeText = new System.Windows.Forms.TextBox();
			this.btnRemoveNode = new System.Windows.Forms.Button();
			this.chkCheckPersonnumber = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// treeView
			// 
			this.treeView.ImageIndex = -1;
			this.treeView.Location = new System.Drawing.Point(16, 16);
			this.treeView.Name = "treeView";
			this.treeView.SelectedImageIndex = -1;
			this.treeView.Size = new System.Drawing.Size(280, 456);
			this.treeView.TabIndex = 0;
			this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
			// 
			// btnAddNode
			// 
			this.btnAddNode.Location = new System.Drawing.Point(24, 488);
			this.btnAddNode.Name = "btnAddNode";
			this.btnAddNode.Size = new System.Drawing.Size(104, 32);
			this.btnAddNode.TabIndex = 1;
			this.btnAddNode.Text = "Lägg till nod";
			this.btnAddNode.Click += new System.EventHandler(this.btnAddNode_Click);
			// 
			// txtNodeText
			// 
			this.txtNodeText.Location = new System.Drawing.Point(160, 496);
			this.txtNodeText.Name = "txtNodeText";
			this.txtNodeText.Size = new System.Drawing.Size(136, 20);
			this.txtNodeText.TabIndex = 2;
			this.txtNodeText.Text = "";
			// 
			// btnRemoveNode
			// 
			this.btnRemoveNode.Location = new System.Drawing.Point(24, 528);
			this.btnRemoveNode.Name = "btnRemoveNode";
			this.btnRemoveNode.Size = new System.Drawing.Size(104, 32);
			this.btnRemoveNode.TabIndex = 3;
			this.btnRemoveNode.Text = "Ta bort nod";
			this.btnRemoveNode.Click += new System.EventHandler(this.btnRemoveNode_Click);
			// 
			// chkCheckPersonnumber
			// 
			this.chkCheckPersonnumber.Location = new System.Drawing.Point(24, 592);
			this.chkCheckPersonnumber.Name = "chkCheckPersonnumber";
			this.chkCheckPersonnumber.Size = new System.Drawing.Size(192, 24);
			this.chkCheckPersonnumber.TabIndex = 4;
			this.chkCheckPersonnumber.Text = "Kontrollera personnummer";
			this.chkCheckPersonnumber.CheckedChanged += new System.EventHandler(this.chkCheckPersonnumber_CheckedChanged);
			// 
			// Settings
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(312, 630);
			this.Controls.Add(this.chkCheckPersonnumber);
			this.Controls.Add(this.btnRemoveNode);
			this.Controls.Add(this.txtNodeText);
			this.Controls.Add(this.btnAddNode);
			this.Controls.Add(this.treeView);
			this.MaximizeBox = false;
			this.Name = "Settings";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Settings";
			this.ResumeLayout(false);

		}
		#endregion
		
		#region TreeView handling
		private void UpdateTreeView()
		{
			treeView.Nodes.Clear();
			TreeNode root = treeView.Nodes.Add(xmlSettings.DocumentElement.Attributes["description"].Value);

			AddNode(xmlSettings.DocumentElement, root);

			treeView.ExpandAll();
		}
	

		private void AddNode(XmlNode inXmlNode, TreeNode inTreeNode)
		{
			TreeNode tNode;
			XmlNodeList nodeList = inXmlNode.ChildNodes;

			foreach(System.Xml.XmlNode xmlNode in nodeList)
			{
				if (xmlNode.Attributes["description"] != null)
				{
					int nodeNumber = 0;
					nodeNumber = inTreeNode.Nodes.Add(new TreeNode(xmlNode.Attributes["description"].Value));
					tNode = inTreeNode.Nodes[nodeNumber];
					AddNode(xmlNode, tNode);
				}

				else 
				{
					inTreeNode.Nodes.Add(new TreeNode(xmlNode.InnerText));
				}

			}

		}

		private void treeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			TreeNode tnTemp = e.Node;
			TreeNode tnParent = tnTemp.Parent;
			selectedChildNode = null;
			if (tnParent != null)
			{
				selectedNode = xmlSettings.DocumentElement.SelectSingleNode("//category[@description='" + tnParent.Text + "']");
				if (selectedNode != null)
				{
					Debug.WriteLine(selectedNode.FirstChild.InnerText);
					foreach (XmlNode n in selectedNode.ChildNodes)
					{
						if (n.InnerText == tnTemp.Text)
						{
							selectedChildNode = n;
							Debug.WriteLine("Selected child : " + n.InnerText);
						}
					}
				}
				
				
			}
			
		}
		#endregion TreeView handling

		#region Adding and removing elements
		private void btnAddNode_Click(object sender, System.EventArgs e)
		{
			XmlElement element = xmlSettings.CreateElement("item");
			XmlText text = xmlSettings.CreateTextNode(txtNodeText.Text);
			if (selectedNode != null)
			{
				selectedNode.AppendChild(element);
				selectedNode.LastChild.AppendChild(text);
			
				xmlSettings.Save(filename);
				UpdateTreeView();
				txtNodeText.ResetText();
			}
		}

		private void btnRemoveNode_Click(object sender, System.EventArgs e)
		{
			if ((selectedChildNode != null) && (selectedNode != null))
			{
				selectedNode.RemoveChild(selectedChildNode);
				Debug.WriteLine("Removed child : " + selectedChildNode.Value);
				xmlSettings.Save(filename);
				UpdateTreeView();
			}

				
		}
		#endregion

		public static bool LoadSettings(string aFilename)
		{
			filename = aFilename;
			xmlSettings = new XmlDocument();
			XmlTextReader reader = new XmlTextReader(filename);
			xmlSettings.Load(reader);
			reader.Close();

			xnPhraseLibrary = xmlSettings.DocumentElement.SelectSingleNode("//category[@description='Frasbibliotek']");

			xnPrimulaFileNumber = xmlSettings.DocumentElement.SelectSingleNode("//category[@description='Filnummer']");
			primulaFileNumber = xnPrimulaFileNumber.FirstChild.InnerText;

			//TODO: Test /category[@description = 'Primula']/category[@description = 'Filnamn']
			XmlNode xnPrimulaFilename = xmlSettings.DocumentElement.SelectSingleNode("//category[@description='Filnamn']");
			primulaFilename = xnPrimulaFilename.FirstChild.InnerText;

			//TODO: DO Try-Catch here and display a more detailed error-message
			XmlNode xnReceiptLogoFilename = xmlSettings.DocumentElement.SelectSingleNode("//category[@description='Logofilnamn']");
			receiptLogoFilename = xnReceiptLogoFilename.FirstChild.InnerText;

			backupPaths = new ArrayList();
			XmlNode xnBackupPath = xmlSettings.DocumentElement.SelectSingleNode("//category[@description='Sökväg']");
			foreach(XmlNode n in xnBackupPath.ChildNodes)
			{
				backupPaths.Add(n.InnerText);
			}

			Debug.WriteLine("Primula filename: " + primulaFilename + "." + primulaFileNumber);

			return true;
		}

		public static void SaveSettings()
		{
			xnPrimulaFileNumber.FirstChild.InnerText = primulaFileNumber;
			xmlSettings.Save(filename);
		}

		public static void IncPrimulaFileNumber()
		{
			int index = int.Parse(primulaFileNumber);
			index++;
			string s;
			if (index < 10)
				s = "00" + index.ToString();
			else if (index < 100)
				s = "0" + index.ToString();
			else if (index < 1000)
				s = index.ToString();
			else
				s = "001";

			primulaFileNumber = s;
		}

		private static ArrayList backupPaths;
		public static ArrayList BackupPaths
		{
			get{return backupPaths;}
		}
		private static string primulaFilename;
		public static string PrimulaFilename
		{
			get {return primulaFilename;}
		}

		private static string receiptLogoFilename;
		public static string ReceiptLogoFilename
		{
			get {return receiptLogoFilename;}
		}

		private static XmlNode xnPrimulaFileNumber;

		private static string primulaFileNumber;
		public static string PrimulaFileNumber
		{
			get {return primulaFileNumber;}
		}
		private static XmlNode xnPhraseLibrary;
		public static XmlNode XnPhraseLibrary
		{
			get {return xnPhraseLibrary;}
		}
		private static bool isCheckPersonnumberEnabled = true;

		private void chkCheckPersonnumber_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkCheckPersonnumber.Checked)
				isCheckPersonnumberEnabled = true;
			else
				isCheckPersonnumberEnabled = false;
		}
	
		public static bool IsCheckPersonnumberEnabled
		{
			get {return isCheckPersonnumberEnabled;}
		}

	}
}
