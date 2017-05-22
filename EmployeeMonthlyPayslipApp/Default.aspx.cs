using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using EmployeePayslipGenerator.Library.EmployeePayslip;
using EmployeePayslipGenerator.Library.ImportFile;
using WebGrease.Css.Extensions;

namespace EmployeeMonthlyPayslipApp {
	public partial class Default : System.Web.UI.Page {
		private string FilePath = string.Empty;
		private string FileName = string.Empty;
		protected void Page_Load(object sender, EventArgs e) { }

		#region GenerateButton_Click Event
		protected void GenerateButton_Click(object sender, EventArgs e) {
			string firstName = FirstName.Text;
			string lastName = LastName.Text;
			string annualIncome = AnnualIncome.Text;
			string superRate = SuperRate.Text;
			string monthStarting = MonthStarting.Text;

			try {
				var importFileDetails = ImportFileProcessor.ProcessFile(firstName, lastName, annualIncome,
																		superRate, monthStarting);
				StartGeneratingPayslips(importFileDetails);
			} catch (Exception ex) {
				UploadMessage.Text = "Error Processing File: " + ex.Message.ToString();
			}
		}

		#endregion

		#region UploadButton_Click Event

		protected void UploadButton_Click(object sender, EventArgs e) {
			try {
				if (FileUploader.HasFile) {
					using (Stream fileStream = FileUploader.PostedFile.InputStream) {
						var fileRecordDetails = ImportFileProcessor.ProcessFile(fileStream, ',');
						StartGeneratingPayslips(fileRecordDetails);
					}
				} else {
					MessageBox.Show(this, "You have not specified a file.");
				}
			} catch (Exception ex) {
				MessageBox.Show(this, "Error Processing File: " + ex.Message.ToString());
			}
		}
		#endregion

		#region Start Generating Payslips
		internal void StartGeneratingPayslips(List<ImportFileDetail> fileRecordDetails) {
			EmployeePayslip employeePaySlip = new EmployeePayslip();
			employeePaySlip.Initialise(fileRecordDetails);
			employeePaySlip.Generate();

			if (employeePaySlip.FileHasErrors()) {
				string errorMessage = FormatErrorOutput(employeePaySlip);
				MessageBox.Show(this, errorMessage);
			} else {
				CreatOutputFile(employeePaySlip);
				PromptUserToSaveFile();
			}
		}
		#endregion

		#region Create Output File
		internal void CreatOutputFile(EmployeePayslip employeePaySlips) {
			FileName = string.Format("EmployeeMonthlyPayslips{0}.csv", DateTime.Now.ToString("yyyyMMdd"));
			FilePath = string.Format(@"{0}\ServerDocs\{1}", Server.MapPath("~"), FileName);

			// Write the string array to a new file named "WriteLines.txt".
			using (StreamWriter outputFile = new StreamWriter(FilePath)) {
				foreach (string line in employeePaySlips.PayslipRecords)
					outputFile.WriteLine(line);
			}
		}
		#endregion

		#region Prompt User To Save File
		internal void PromptUserToSaveFile() {
			HttpResponse response = HttpContext.Current.Response;
			response.ClearContent();
			response.Clear();
			response.ContentType = "text/plain";
			response.AddHeader("Content-Disposition", "attachment; filename=" + FileName + ";");
			response.TransmitFile(FilePath);
			response.Flush();
			response.End();
		}
		#endregion

		#region Format Error Output
		internal string FormatErrorOutput(EmployeePayslip employeePaySlip) {
			String title = @"Unable to Generate Payslip. The following errors occured:\n\n";
			StringBuilder errorMessage = new StringBuilder();
			employeePaySlip.Errors.Take(10).ForEach(error => errorMessage.Append(string.Format(@"{0}\n\t{1}\n\n", error.ErrorHeader, error.ErrorBody)));
			return title + errorMessage.ToString();
		}
		#endregion
	}
	#region Message Box Implementation
	public static class MessageBox {
		public static void Show(this Page page, string errorMessage) {
			page.ClientScript.RegisterStartupScript(
				page.GetType(),
				"MessageBox",
				"<script language='javascript'>alert('" + errorMessage + "');</script>"
			);
		}
	}
	#endregion
}