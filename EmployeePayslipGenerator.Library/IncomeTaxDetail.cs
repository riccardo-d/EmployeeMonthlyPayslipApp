using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeePayslipGenerator.Library {
	public class IncomeTaxDetail {
		public string FinancialYear { get; set; }
		public decimal LowerLimit { get; set; }
		public decimal UpperLimit { get; set; }
		public decimal TaxPerDollar { get; set; }
		public decimal TaxOnIncome { get; set; }
	}
}
