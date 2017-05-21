using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EmployeePayslipGenerator.Library.ImportFile {
	public static class ImportFileProcessor {
		public static List<ImportFileDetail> ProcessFile(String firstName, String lastName, String annualIncome,
														String superRate, String monthStarting) {
			List<ImportFileDetail> importFileDetails = new List<ImportFileDetail>();
			importFileDetails.Add(new ImportFileDetail {
				FirstName = firstName.CleanUp(),
				LastName = lastName.CleanUp(),
				AnnualIncomeString = annualIncome,
				SuperRateString = superRate,
				MonthStarting = monthStarting,
				IsValid = true
			});

			return importFileDetails;
		}

		public static List<ImportFileDetail> ProcessFile(Stream fileStream, char delimeter) {
			List<ImportFileDetail> importFileDetails = new List<ImportFileDetail>();
			using (StreamReader streamReader = new StreamReader(fileStream)) {
				String fileRecord;
				while ((fileRecord = streamReader.ReadLine()) != null) {
					importFileDetails.Add(SplitRecordByDelimeter(fileRecord, delimeter));
				}
			}
			return importFileDetails;
		}

		public static ImportFileDetail SplitRecordByDelimeter(string fileRecord, char delimeter) {
			string[] splitRecord = fileRecord.Split(delimeter);
			if (splitRecord.Count() == 5) {
				return new ImportFileDetail {
					FirstName = splitRecord[0].CleanUp(),
					LastName = splitRecord[1].CleanUp(),
					AnnualIncomeString = splitRecord[2],
					SuperRateString = splitRecord[3],
					MonthStarting = splitRecord[4],
					IsValid = true
				};
			} 
				
			throw new Exception(string.Format("Invalid record format - {0}", fileRecord));
		}
	}
}
