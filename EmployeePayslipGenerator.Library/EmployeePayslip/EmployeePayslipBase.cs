using System;
using System.Collections.Generic;
using System.Linq;
using EmployeePayslipGenerator.Library.ImportFile;

namespace EmployeePayslipGenerator.Library.EmployeePayslip {
	public class EmployeePayslipBase : IEmployeePayslip {
		#region Parameter Declaration
		internal List<IncomeTaxDetail> IncomeTaxData = new List<IncomeTaxDetail>();

		public List<ImportFileDetail> ImportFileDetails = new List<ImportFileDetail>();

		public List<ErrorDetail> Errors = new List<ErrorDetail>();

		public char Delimeter = new char();
		#endregion

		#region Initialise
		public virtual void Initialise() {
			GenerateIncomeTaxData();
		}
		#endregion

		#region Error Logging
		public bool FileHasErrors() { return Errors.Any(); }
		internal void LogError(string errorHeader, string errorBody) {
			Errors.Add(new ErrorDetail {ErrorHeader = errorHeader, ErrorBody = errorBody});
		}
		public string FormatErrorHeader(ImportFileDetail importFileDetail) {
			string errorHeader = @"Unable to process record - " + importFileDetail.FirstName + "," + importFileDetail.LastName + ","
								+ importFileDetail.AnnualIncomeString + ","
								+ importFileDetail.SuperRateString + "," + importFileDetail.MonthStarting;
			return errorHeader;
		}
		#endregion

		#region Validate Import File
		public void ValidateImportFile() {
			ImportFileDetails.ForEach(
									 importFileDetail =>
										importFileDetail.ValidateEachRow().ForEach(errorMessage => LogError(FormatErrorHeader(importFileDetail), errorMessage)));
		}
		#endregion

		#region Generate Income Tax Data
		private void GenerateIncomeTaxData() {
			List<string> taxDataList = new List<string>();
			taxDataList.Add("FY12-13, 0, 18200, 0, 0");
			taxDataList.Add("FY12-13, 18201, 37000, 0.190, 0");
			taxDataList.Add("FY12-13, 37001, 80000, 0.325, 3572");
			taxDataList.Add("FY12-13, 80001, 180000, 0.370, 17547");
			taxDataList.Add("FY12-13, 180001, 1000000, 0.450, 54547");

			foreach (string taxDataItem in taxDataList) {
				string[] itemSplit = taxDataItem.Split(',');
				IncomeTaxData.Add(new IncomeTaxDetail {
					FinancialYear = itemSplit[0],
					LowerLimit = decimal.Parse(itemSplit[1]),
					UpperLimit = decimal.Parse(itemSplit[2]),
					TaxPerDollar = decimal.Parse(itemSplit[3]),
					TaxOnIncome = decimal.Parse(itemSplit[4])
				});
			}
		}
		#endregion

		#region Calculate Payslips
		internal void CalculatePayslips() {
			foreach (ImportFileDetail importFileDetail in ImportFileDetails) {
				importFileDetail.CalculateGrossMonthlyIncome();
				IncomeTaxDetail incomeTaxBracket =
					IncomeTaxData.FirstOrDefault(incomeTaxData => importFileDetail.AnnualIncome >= incomeTaxData.LowerLimit
																&& importFileDetail.AnnualIncome <= incomeTaxData.UpperLimit);
				if (incomeTaxBracket != null) {
					importFileDetail.CalculateIncomeTax(incomeTaxBracket);
					importFileDetail.CalculateNetIncome();
				}
				importFileDetail.CalculateSuperDeduction();
			}
		}
		#endregion
	}
}
