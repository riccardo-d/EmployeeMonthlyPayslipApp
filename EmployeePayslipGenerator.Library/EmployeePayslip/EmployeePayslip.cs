using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EmployeePayslipGenerator.Library.ImportFile;

namespace EmployeePayslipGenerator.Library.EmployeePayslip {
	public class EmployeePayslip : EmployeePayslipBase {
		#region Variable Declarations
		public List<string> PayslipRecords = new List<string>();
		#endregion

		#region Initialise
		public void Initialise(List<ImportFileDetail> importFileDetails) {
			ImportFileDetails = importFileDetails;
			base.Initialise();
		}
		#endregion

		#region Generate
		public void Generate() {
			ValidateImportFile();
			if (!FileHasErrors()) {
				CalculatePayslips();
				GeneratePayslipsOutput();
			}
		}
		#endregion

		#region Generate Payslips Output
		internal void GeneratePayslipsOutput() {
			ImportFileDetails.ForEach(importFileDetail => PayslipRecords.Add(importFileDetail.FormatPayslipOutput()));
		}
		#endregion
	}
}
