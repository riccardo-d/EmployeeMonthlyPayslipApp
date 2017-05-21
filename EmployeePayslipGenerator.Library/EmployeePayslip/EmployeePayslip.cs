using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EmployeePayslipGenerator.Library.ImportFile;

namespace EmployeePayslipGenerator.Library.EmployeePayslip {
	public class EmployeePayslip : EmployeePayslipBase {
		public List<string> PayslipRecords = new List<string>();

		public void Initialise(List<ImportFileDetail> importFileDetails) {
			ImportFileDetails = importFileDetails;
			base.Initialise();
		}

		public void Generate() {
			ValidateImportFile();
			if (!FileHasErrors()) {
				CalculatePayslips();
				GeneratePayslipsOutput();
			}
		}

		internal void GeneratePayslipsOutput() {
			ImportFileDetails.ForEach(importFileDetail => PayslipRecords.Add(importFileDetail.FormatPayslipOutput()));
		}
	}
}
