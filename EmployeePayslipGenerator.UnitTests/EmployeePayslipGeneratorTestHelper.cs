using System.Collections.Generic;
using EmployeePayslipGenerator.Library.EmployeePayslip;
using EmployeePayslipGenerator.Library.ImportFile;

namespace EmployeePayslipGenerator.UnitTests {
	public static class EmployeePayslipGeneratorTestHelper {
		public static void AddTestImportData(this List<ImportFileDetail> importFileDetails, string firstName, string lastName, string annualIncome, string superRate, string monthStarting) {
			importFileDetails.Add(new ImportFileDetail {
					FirstName = firstName,
					LastName = lastName,
					AnnualIncomeString = annualIncome,
					SuperRateString = superRate,
					MonthStarting = monthStarting
			});
		}
		public static void GenerateTestPayslips(this EmployeePayslip employeePaySlip, List<ImportFileDetail> importFileDetails) {
			employeePaySlip.Initialise(importFileDetails);
			employeePaySlip.Generate();
		}
	}
}
