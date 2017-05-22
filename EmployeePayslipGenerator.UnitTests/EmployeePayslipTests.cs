using System;
using System.Collections.Generic;
using System.Linq;
using EmployeePayslipGenerator.Library.EmployeePayslip;
using EmployeePayslipGenerator.Library.ImportFile;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EmployeePayslipGenerator.UnitTests {
	[TestClass]
	public class EmployeePayslipTests {
		#region Variable Declarations
		private readonly List<ImportFileDetail> payslipCalculationTest = new List<ImportFileDetail>();
		private readonly EmployeePayslip paySlipCalculation = new EmployeePayslip();
		
		private readonly List<ImportFileDetail> taxBracketCalculationTest = new List<ImportFileDetail>();
		private readonly EmployeePayslip taxBracketCalculation = new EmployeePayslip();
		
		private readonly List<ImportFileDetail> employeePayslipOutputTest = new List<ImportFileDetail>();
		private readonly EmployeePayslip employeePaySlipOutput = new EmployeePayslip();
		#endregion

		#region Employee Payslip Tests
		public EmployeePayslipTests() {
			BuildTestData();
		}
		#endregion

		#region Build Test Data
		public void BuildTestData() {
			payslipCalculationTest.AddTestImportData("PayCalculation1", "PayCalcultaion1", "60050", "9%", "01 March - 31 March");
			paySlipCalculation.GenerateTestPayslips(payslipCalculationTest);

			taxBracketCalculationTest.AddTestImportData("TaxBracket1", "TaxBracket1", "10000", "9%", "01 March - 31 March");
			taxBracketCalculationTest.AddTestImportData("TaxBracket2", "TaxBracket2", "20000", "9%", "01 March - 31 March");
			taxBracketCalculationTest.AddTestImportData("TaxBracket3", "TaxBracket3", "40000", "9%", "01 March - 31 March");
			taxBracketCalculationTest.AddTestImportData("TaxBracket4", "TaxBracket4", "100000", "9%", "01 March - 31 March");
			taxBracketCalculationTest.AddTestImportData("TaxBracket5", "TaxBracket5", "200000", "9%", "01 March - 31 March");
			taxBracketCalculation.GenerateTestPayslips(taxBracketCalculationTest);

			employeePayslipOutputTest.AddTestImportData("EmpPayslipOutput1", "EmpPayslipOutput1", "10000", "9%", "01 March - 31 March");
			employeePaySlipOutput.GenerateTestPayslips(employeePayslipOutputTest);
		}
		#endregion

		#region Unit Tests for Payfile Calculations
		[TestMethod]
		[TestCategory("PayfileCalculations")]
		public void CheckGrossMonthlyIncomeCalculation() {
			decimal expectedResult = 5004;
			decimal actualResult = paySlipCalculation.ImportFileDetails.Select(importFileDetail => importFileDetail.GrossMonthlyIncome).FirstOrDefault();
			Assert.IsTrue(paySlipCalculation.FileHasErrors() == false, "Import File has returned some errors.");
			Assert.IsTrue(expectedResult == actualResult, string.Format("Expected Result : {0}. Actual Result : {1}", expectedResult.ToString(), actualResult.ToString()));
		}

		[TestMethod]
		[TestCategory("PayfileCalculations")]
		public void CheckIncomeTaxCalculation() {
			decimal expectedResult = 922;
			decimal actualResult = paySlipCalculation.ImportFileDetails.Select(importFileDetail => importFileDetail.IncomeTax).FirstOrDefault();
			Assert.IsTrue(paySlipCalculation.FileHasErrors() == false, "Import File has returned some errors.");
			Assert.IsTrue(expectedResult == actualResult, string.Format("Expected Result : {0}. Actual Result : {1}", expectedResult.ToString(), actualResult.ToString()));
		}

		[TestMethod]
		[TestCategory("PayfileCalculations")]
		public void CheckNetIncomeCalculation() {
			decimal expectedResult = 4082;
			decimal actualResult = paySlipCalculation.ImportFileDetails.Select(importFileDetail => importFileDetail.NetIncome).FirstOrDefault();
			Assert.IsTrue(paySlipCalculation.FileHasErrors() == false, "Import File has returned some errors.");
			Assert.IsTrue(expectedResult == actualResult, string.Format("Expected Result : {0}. Actual Result : {1}", expectedResult.ToString(), actualResult.ToString()));
		}

		[TestMethod]
		[TestCategory("PayfileCalculations")]
		public void CheckSuperDeductionCalculation() {
			decimal expectedResult = 450;
			decimal actualResult = paySlipCalculation.ImportFileDetails.Select(importFileDetail => importFileDetail.SuperDeduction).FirstOrDefault();
			Assert.IsTrue(paySlipCalculation.FileHasErrors() == false, "Import File has returned some errors.");
			Assert.IsTrue(expectedResult == actualResult, string.Format("Expected Result : {0}. Actual Result : {1}", expectedResult.ToString(), actualResult.ToString()));
		}

		[TestMethod]
		[TestCategory("PayfileCalculations")]
		[Description("Check pay calculations for an employee with an annual salary < $18,200")]
		public void CheckIncomeTaxCalculationForTaxBracketOne() {
			decimal expectedGrossMonthlyIncome = 833;
			decimal expectedIncomeTax = 0;
			decimal expectedNetIncome = 833;
			decimal expectedSuperDeduction = 75;
			string employeeFirstName = "TaxBracket1";

			Assert.IsTrue(taxBracketCalculation.FileHasErrors() == false, "Import File has returned some errors.");
			ImportFileDetail importFileDetail = taxBracketCalculation.ImportFileDetails.FirstOrDefault(impFileDetail => impFileDetail.FirstName == employeeFirstName);
			Assert.IsTrue(importFileDetail != null, string.Format("Failed to retrieve a matching record for employee with FirstName {0}", employeeFirstName));
			Assert.IsTrue(expectedGrossMonthlyIncome == importFileDetail.GrossMonthlyIncome, string.Format("Expected Gross Monthly Income Result : {0}. Actual Result : {1}", expectedGrossMonthlyIncome.ToString(), importFileDetail.GrossMonthlyIncome.ToString()));
			Assert.IsTrue(expectedIncomeTax == importFileDetail.IncomeTax, string.Format("Expected Income Tax Result : {0}. Actual Result : {1}", expectedIncomeTax.ToString(), importFileDetail.IncomeTax.ToString()));
			Assert.IsTrue(expectedNetIncome == importFileDetail.NetIncome, string.Format("Expected Net Income Result : {0}. Actual Result : {1}", expectedNetIncome.ToString(), importFileDetail.NetIncome.ToString()));
			Assert.IsTrue(expectedSuperDeduction == importFileDetail.SuperDeduction, string.Format("Expected Super Deduction Result : {0}. Actual Result : {1}", expectedSuperDeduction.ToString(), importFileDetail.SuperDeduction.ToString()));
		}

		[TestMethod]
		[TestCategory("PayfileCalculations")]
		[Description("Check pay calculations for an employee with an annual salary between $18,201 - $37,000")]
		public void CheckIncomeTaxCalculationForTaxBracketTwo() {
			decimal expectedGrossMonthlyIncome = 1667;
			decimal expectedIncomeTax = 28;
			decimal expectedNetIncome = 1639;
			decimal expectedSuperDeduction = 150;
			string employeeFirstName = "TaxBracket2";

			Assert.IsTrue(taxBracketCalculation.FileHasErrors() == false, "Import File has returned some errors.");
			ImportFileDetail importFileDetail = taxBracketCalculation.ImportFileDetails.FirstOrDefault(impFileDetail => impFileDetail.FirstName == employeeFirstName);
			Assert.IsTrue(importFileDetail != null, string.Format("Failed to retrieve a matching record for employee with FirstName {0}", employeeFirstName));
			Assert.IsTrue(expectedGrossMonthlyIncome == importFileDetail.GrossMonthlyIncome, string.Format("Expected Gross Monthly Income Result : {0}. Actual Result : {1}", expectedGrossMonthlyIncome.ToString(), importFileDetail.GrossMonthlyIncome.ToString()));
			Assert.IsTrue(expectedIncomeTax == importFileDetail.IncomeTax, string.Format("Expected Income Tax Result : {0}. Actual Result : {1}", expectedIncomeTax.ToString(), importFileDetail.IncomeTax.ToString()));
			Assert.IsTrue(expectedNetIncome == importFileDetail.NetIncome, string.Format("Expected Net Income Result : {0}. Actual Result : {1}", expectedNetIncome.ToString(), importFileDetail.NetIncome.ToString()));
			Assert.IsTrue(expectedSuperDeduction == importFileDetail.SuperDeduction, string.Format("Expected Super Deduction Result : {0}. Actual Result : {1}", expectedSuperDeduction.ToString(), importFileDetail.SuperDeduction.ToString()));
		}

		[TestMethod]
		[TestCategory("PayfileCalculations")]
		[Description("Check pay calculations for an employee with an annual salary between $37,001 - $80,000")]
		public void CheckIncomeTaxCalculationForTaxBracketThree() {
			decimal expectedGrossMonthlyIncome = 3333;
			decimal expectedIncomeTax = 379;
			decimal expectedNetIncome = 2954;
			decimal expectedSuperDeduction = 300;
			string employeeFirstName = "TaxBracket3";

			Assert.IsTrue(taxBracketCalculation.FileHasErrors() == false, "Import File has returned some errors.");
			ImportFileDetail importFileDetail = taxBracketCalculation.ImportFileDetails.FirstOrDefault(impFileDetail => impFileDetail.FirstName == employeeFirstName);
			Assert.IsTrue(importFileDetail != null, string.Format("Failed to retrieve a matching record for employee with FirstName {0}", employeeFirstName));
			Assert.IsTrue(expectedGrossMonthlyIncome == importFileDetail.GrossMonthlyIncome, string.Format("Expected Gross Monthly Income Result : {0}. Actual Result : {1}", expectedGrossMonthlyIncome.ToString(), importFileDetail.GrossMonthlyIncome.ToString()));
			Assert.IsTrue(expectedIncomeTax == importFileDetail.IncomeTax, string.Format("Expected Income Tax Result : {0}. Actual Result : {1}", expectedIncomeTax.ToString(), importFileDetail.IncomeTax.ToString()));
			Assert.IsTrue(expectedNetIncome == importFileDetail.NetIncome, string.Format("Expected Net Income Result : {0}. Actual Result : {1}", expectedNetIncome.ToString(), importFileDetail.NetIncome.ToString()));
			Assert.IsTrue(expectedSuperDeduction == importFileDetail.SuperDeduction, string.Format("Expected Super Deduction Result : {0}. Actual Result : {1}", expectedSuperDeduction.ToString(), importFileDetail.SuperDeduction.ToString()));
		}

		[TestMethod]
		[TestCategory("PayfileCalculations")]
		[Description("Check pay calculations for an employee with an annual salary between $80,001 - $180,000")]
		public void CheckIncomeTaxCalculationForTaxBracketFour() {
			decimal expectedGrossMonthlyIncome = 8333;
			decimal expectedIncomeTax = 2079;
			decimal expectedNetIncome = 6254;
			decimal expectedSuperDeduction = 750;
			string employeeFirstName = "TaxBracket4";

			Assert.IsTrue(taxBracketCalculation.FileHasErrors() == false, "Import File has returned some errors.");
			ImportFileDetail importFileDetail = taxBracketCalculation.ImportFileDetails.FirstOrDefault(impFileDetail => impFileDetail.FirstName == employeeFirstName);
			Assert.IsTrue(importFileDetail != null, string.Format("Failed to retrieve a matching record for employee with FirstName {0}", employeeFirstName));
			Assert.IsTrue(expectedGrossMonthlyIncome == importFileDetail.GrossMonthlyIncome, string.Format("Expected Gross Monthly Income Result : {0}. Actual Result : {1}", expectedGrossMonthlyIncome.ToString(), importFileDetail.GrossMonthlyIncome.ToString()));
			Assert.IsTrue(expectedIncomeTax == importFileDetail.IncomeTax, string.Format("Expected Income Tax Result : {0}. Actual Result : {1}", expectedIncomeTax.ToString(), importFileDetail.IncomeTax.ToString()));
			Assert.IsTrue(expectedNetIncome == importFileDetail.NetIncome, string.Format("Expected Net Income Result : {0}. Actual Result : {1}", expectedNetIncome.ToString(), importFileDetail.NetIncome.ToString()));
			Assert.IsTrue(expectedSuperDeduction == importFileDetail.SuperDeduction, string.Format("Expected Super Deduction Result : {0}. Actual Result : {1}", expectedSuperDeduction.ToString(), importFileDetail.SuperDeduction.ToString()));
		}

		[TestMethod]
		[TestCategory("PayfileCalculations")]
		[Description("Check pay calculations for an employee with an annual salary of $180,000+")]
		public void CheckIncomeTaxCalculationForTaxBracketFive() {
			decimal expectedGrossMonthlyIncome = 16667;
			decimal expectedIncomeTax = 5296;
			decimal expectedNetIncome = 11371;
			decimal expectedSuperDeduction = 1500;
			string employeeFirstName = "TaxBracket5";

			Assert.IsTrue(taxBracketCalculation.FileHasErrors() == false, "Import File has returned some errors.");
			ImportFileDetail importFileDetail = taxBracketCalculation.ImportFileDetails.FirstOrDefault(impFileDetail => impFileDetail.FirstName == employeeFirstName);
			Assert.IsTrue(importFileDetail != null, string.Format("Failed to retrieve a matching record for employee with FirstName {0}", employeeFirstName));
			Assert.IsTrue(expectedGrossMonthlyIncome == importFileDetail.GrossMonthlyIncome, string.Format("Expected Gross Monthly Income Result : {0}. Actual Result : {1}", expectedGrossMonthlyIncome.ToString(), importFileDetail.GrossMonthlyIncome.ToString()));
			Assert.IsTrue(expectedIncomeTax == importFileDetail.IncomeTax, string.Format("Expected Income Tax Result : {0}. Actual Result : {1}", expectedIncomeTax.ToString(), importFileDetail.IncomeTax.ToString()));
			Assert.IsTrue(expectedNetIncome == importFileDetail.NetIncome, string.Format("Expected Net Income Result : {0}. Actual Result : {1}", expectedNetIncome.ToString(), importFileDetail.NetIncome.ToString()));
			Assert.IsTrue(expectedSuperDeduction == importFileDetail.SuperDeduction, string.Format("Expected Super Deduction Result : {0}. Actual Result : {1}", expectedSuperDeduction.ToString(), importFileDetail.SuperDeduction.ToString()));
		}
		#endregion

		#region Unit Test for Payfile Output
		[TestMethod]
		[TestCategory("PayfileCalculations")]
		public void CheckEmployeePayslipOutput() {
			string expectedResult = "EmpPayslipOutput1 EmpPayslipOutput1,01 March - 31 March,833,0,833,75";

			Assert.IsTrue(employeePaySlipOutput.FileHasErrors() == false, "Import File has returned some errors.");
			ImportFileDetail importFileDetail = employeePaySlipOutput.ImportFileDetails.FirstOrDefault(impFileDetail => impFileDetail.FirstName == "EmpPayslipOutput1");
			Assert.IsTrue(importFileDetail != null, "Failed to retrieve a matching record for employee with FirstName EmpPayslipOutput1");
			string actualResult = importFileDetail.FormatPayslipOutput();

			Assert.IsTrue(expectedResult == actualResult, string.Format("Expected Result : {0}. Actual Result : {1}", expectedResult.ToString(), actualResult.ToString()));
		}
		#endregion
	}
}
