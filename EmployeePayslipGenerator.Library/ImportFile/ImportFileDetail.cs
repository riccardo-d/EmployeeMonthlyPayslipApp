
namespace EmployeePayslipGenerator.Library.ImportFile {
	public class ImportFileDetail {
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string AnnualIncomeString { get; set; }
		public string SuperRateString { get; set; }
		public decimal AnnualIncome { get; set; }
		public decimal SuperRate { get; set; }
		public string MonthStarting { get; set; }
		public decimal GrossMonthlyIncome { get; set; }
		public decimal IncomeTax { get; set; }
		public decimal NetIncome { get; set; }
		public decimal SuperDeduction { get; set; }
		public bool IsValid { get; set; }
	}
}
