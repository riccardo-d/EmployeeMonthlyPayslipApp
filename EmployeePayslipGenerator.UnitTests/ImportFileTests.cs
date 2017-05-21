using System;
using System.Collections.Generic;
using System.Linq;
using EmployeePayslipGenerator.Library;
using EmployeePayslipGenerator.Library.EmployeePayslip;
using EmployeePayslipGenerator.Library.ImportFile;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmployeePayslipGenerator.UnitTests {
	[TestClass]
	public class ImportFileTests {
		private readonly List<ImportFileDetail> validImportFileTest = new List<ImportFileDetail>();
		private readonly EmployeePayslip validImportFile = new EmployeePayslip();
		
		private readonly List<ImportFileDetail> invalidImportRecordTest = new List<ImportFileDetail>();
		private readonly EmployeePayslip invalidImportRecord = new EmployeePayslip();

		private readonly List<ImportFileDetail> febLeapYearRecordTest = new List<ImportFileDetail>();
		private readonly EmployeePayslip febLeapYearRecord = new EmployeePayslip();
		
		private readonly string expectedFirstNameErrorBody = @"Value for First Name cannot be blank";
		private readonly string expectedLastNameErrorBody = @"Value for Last Name cannot be blank";
		private readonly string expectedAnnualIncomeErrorBody = @"Value for Annual Income is not a valid decimal";
		
		private readonly string expectedSuperRateErrorBody = @"Value for Super Rate is not a valid decimal";
		private readonly string expectedSuperRateOutOfRangeErrorBody = @"Value for Super Rate must be between 0 and 50%";
		private readonly string expectedMonthStartingErrorBody = @"Value for Month Starting is not valid";

		private readonly string invalidRecordFormat = @"Test,Test,110000,10.5%,01 April - 30 April,100,200";

		public ImportFileTests() {
			BuildTestData();
		}

		public void BuildTestData() {
			validImportFileTest.AddTestImportData("Ricky", "D'Silva", "110000", "10.5%", "01 April - 30 April");
			validImportFile.GenerateTestPayslips(invalidImportRecordTest);
			
			invalidImportRecordTest.AddTestImportData(string.Empty, "NoFirstName", "60050", "9%", "01 March - 31 March");
			invalidImportRecordTest.AddTestImportData("NoLastName", string.Empty, "60050", "9%", "01 March - 31 March");
			invalidImportRecordTest.AddTestImportData("NoAnnualIncome", "LastName", "blank", "9%", "01 March - 31 March");
			invalidImportRecordTest.AddTestImportData("NoSuperRate", "LastName", "60050", "blank", "01 March - 31 March");
			invalidImportRecordTest.AddTestImportData("InvalidSuperRate", "LastName", "60050", "80%", "01 March - 31 March");
			invalidImportRecordTest.AddTestImportData("InvalidMonthStarting", "LastName", "60050", "9%", "01 March - 29-March");
			invalidImportRecord.GenerateTestPayslips(invalidImportRecordTest);

			febLeapYearRecordTest.AddTestImportData("Leap", "Year", "80000", "9.5%", "01 February - 29 February");
			febLeapYearRecord.GenerateTestPayslips(febLeapYearRecordTest);
		}

		[TestMethod]
		[TestCategory("ImportFile")]
		public void ImportValidRecord() { 
			Assert.IsTrue(validImportFile.FileHasErrors() == false, "Expected No. of Errors : 0. Actual Result : {0}.", validImportFile.Errors.Count().ToString()); 
		}

		[TestMethod]
		[TestCategory("ImportFile")]
		public void ImportRecordWithInvalidFirstName() {
			ImportFileDetail importFileDetail = invalidImportRecord.ImportFileDetails.FirstOrDefault(impFileDetail => string.IsNullOrEmpty(impFileDetail.FirstName));
			string expectedErrorHeader = invalidImportRecord.FormatErrorHeader(importFileDetail);
			Assert.IsTrue(importFileDetail != null, "Failed to retrieve a matching record for an employee with a blank First Name");
			
			ErrorDetail errorDetail = invalidImportRecord.Errors.FirstOrDefault(err => err.ErrorHeader == expectedErrorHeader);
			Assert.IsTrue(errorDetail != null, "Failed to retrieve a matching error detail");
			Assert.IsTrue(expectedErrorHeader == errorDetail.ErrorHeader, string.Format("Expected Error Header Result : {0}. Actual Result : {1}", expectedErrorHeader.ToString(), errorDetail.ErrorHeader));
			Assert.IsTrue(expectedFirstNameErrorBody == errorDetail.ErrorBody, string.Format("Expected Error Body Result : {0}. Actual Result : {1}", expectedFirstNameErrorBody.ToString(), errorDetail.ErrorBody));
		}

		[TestMethod]
		[TestCategory("ImportFile")]
		public void ImportRecordWithInvalidLastName() {
			ImportFileDetail importFileDetail = invalidImportRecord.ImportFileDetails.FirstOrDefault(impFileDetail => string.IsNullOrEmpty(impFileDetail.LastName));
			string expectedErrorHeader = invalidImportRecord.FormatErrorHeader(importFileDetail);
			Assert.IsTrue(importFileDetail != null, "Failed to retrieve a matching record for an employee with a blank Last Name");

			ErrorDetail errorDetail = invalidImportRecord.Errors.FirstOrDefault(err => err.ErrorHeader == expectedErrorHeader);
			Assert.IsTrue(errorDetail != null, "Failed to retrieve a matching error detail");
			Assert.IsTrue(expectedErrorHeader == errorDetail.ErrorHeader, string.Format("Expected Error Header Result : {0}. Actual Result : {1}", expectedErrorHeader.ToString(), errorDetail.ErrorHeader));
			Assert.IsTrue(expectedLastNameErrorBody == errorDetail.ErrorBody, string.Format("Expected Error Body Result : {0}. Actual Result : {1}", expectedLastNameErrorBody.ToString(), errorDetail.ErrorBody));
		}

		[TestMethod]
		[TestCategory("ImportFile")]
		public void ImportRecordWithInvalidAnnualIncome() {
			ImportFileDetail importFileDetail = invalidImportRecord.ImportFileDetails.FirstOrDefault(impFileDetail => impFileDetail.AnnualIncomeString == "blank");
			string expectedErrorHeader = invalidImportRecord.FormatErrorHeader(importFileDetail);
			Assert.IsTrue(importFileDetail != null, "Failed to retrieve a matching record for an employee with an invalid Annual Income");

			ErrorDetail errorDetail = invalidImportRecord.Errors.FirstOrDefault(err => err.ErrorHeader == expectedErrorHeader);
			Assert.IsTrue(errorDetail != null, "Failed to retrieve a matching error detail");
			Assert.IsTrue(expectedErrorHeader == errorDetail.ErrorHeader, string.Format("Expected Error Header Result : {0}. Actual Result : {1}", expectedErrorHeader.ToString(), errorDetail.ErrorHeader));
			Assert.IsTrue(expectedAnnualIncomeErrorBody == errorDetail.ErrorBody, string.Format("Expected Error Body Result : {0}. Actual Result : {1}", expectedAnnualIncomeErrorBody.ToString(), errorDetail.ErrorBody));
		}

		[TestMethod]
		[TestCategory("ImportFile")]
		public void ImportRecordWithInvalidSuperRate() {
			ImportFileDetail importFileDetail = invalidImportRecord.ImportFileDetails.FirstOrDefault(impFileDetail => impFileDetail.SuperRateString == "blank");
			string expectedErrorHeader = invalidImportRecord.FormatErrorHeader(importFileDetail);
			Assert.IsTrue(importFileDetail != null, "Failed to retrieve a matching record for an employee with an invalid Super Rate");

			ErrorDetail errorDetail = invalidImportRecord.Errors.FirstOrDefault(err => err.ErrorHeader == expectedErrorHeader);
			Assert.IsTrue(errorDetail != null, "Failed to retrieve a matching error detail");
			Assert.IsTrue(expectedErrorHeader == errorDetail.ErrorHeader, string.Format("Expected Error Header Result : {0}. Actual Result : {1}", expectedErrorHeader.ToString(), errorDetail.ErrorHeader));
			Assert.IsTrue(expectedSuperRateErrorBody == errorDetail.ErrorBody, string.Format("Expected Error Body Result : {0}. Actual Result : {1}", expectedSuperRateErrorBody.ToString(), errorDetail.ErrorBody));
		}

		[TestMethod]
		[TestCategory("ImportFile")]
		public void ImportRecordWithInvalidMonthStarting() {
			ImportFileDetail importFileDetail = invalidImportRecord.ImportFileDetails.FirstOrDefault(impFileDetail => impFileDetail.MonthStarting == "01 March - 29-March");
			string expectedErrorHeader = invalidImportRecord.FormatErrorHeader(importFileDetail);
			Assert.IsTrue(importFileDetail != null, "Failed to retrieve a matching record for an employee with an invalid Month Starting");

			ErrorDetail errorDetail = invalidImportRecord.Errors.FirstOrDefault(err => err.ErrorHeader == expectedErrorHeader);
			Assert.IsTrue(errorDetail != null, "Failed to retrieve a matching error detail");
			Assert.IsTrue(expectedErrorHeader == errorDetail.ErrorHeader, string.Format("Expected Error Header Result : {0}. Actual Result : {1}", expectedErrorHeader.ToString(), errorDetail.ErrorHeader));
			Assert.IsTrue(expectedMonthStartingErrorBody == errorDetail.ErrorBody, string.Format("Expected Error Body Result : {0}. Actual Result : {1}", expectedMonthStartingErrorBody.ToString(), errorDetail.ErrorBody));
		}

		[TestMethod]
		[TestCategory("ImportFile")]
		public void ImportRecordWithAnOutOfRangeSuperRate() {
			ImportFileDetail importFileDetail = invalidImportRecord.ImportFileDetails.FirstOrDefault(impFileDetail => impFileDetail.FirstName == "InvalidSuperRate");
			string expectedErrorHeader = invalidImportRecord.FormatErrorHeader(importFileDetail);
			Assert.IsTrue(importFileDetail != null, "Failed to retrieve a matching record for an employee with an out of range Super Rate");

			ErrorDetail errorDetail = invalidImportRecord.Errors.FirstOrDefault(err => err.ErrorHeader == expectedErrorHeader);
			Assert.IsTrue(errorDetail != null, "Failed to retrieve a matching error detail");
			Assert.IsTrue(expectedErrorHeader == errorDetail.ErrorHeader, string.Format("Expected Error Header Result : {0}. Actual Result : {1}", expectedErrorHeader.ToString(), errorDetail.ErrorHeader));
			Assert.IsTrue(expectedSuperRateOutOfRangeErrorBody == errorDetail.ErrorBody, string.Format("Expected Error Body Result : {0}. Actual Result : {1}", expectedSuperRateOutOfRangeErrorBody.ToString(), errorDetail.ErrorBody));
		}

		[TestMethod]
		[TestCategory("ImportFile")]
		public void ImportRecordForFebruaryLeapYear() {
			Assert.IsTrue(febLeapYearRecord.FileHasErrors() == false, "Expected No. of Errors : 0. Actual Result : {0}.", febLeapYearRecord.Errors.Count().ToString());
		}

		[TestMethod]
		[TestCategory("ImportFile")]
		public void ImportInvalidRecordFormat() {
			string expectedOutcome = string.Format("Invalid record format - {0}", invalidRecordFormat);
			try {
				ImportFileProcessor.SplitRecordByDelimeter(invalidRecordFormat, ',');
			} catch (Exception ex) {
				Assert.IsTrue(expectedOutcome == ex.Message, string.Format("Expected Outcome : {0}. Actual Outcome : {1}", expectedOutcome, ex.Message));
			}
		}
	}
}
