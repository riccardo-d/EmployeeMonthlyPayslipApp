using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeePayslipGenerator.Library.ImportFile;

namespace EmployeePayslipGenerator.Library.EmployeePayslip {
	public interface IEmployeePayslip {
		void Initialise();
		void ValidateImportFile();
	}
}
