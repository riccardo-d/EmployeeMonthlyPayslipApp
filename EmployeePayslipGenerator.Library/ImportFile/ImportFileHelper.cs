using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using EmployeePayslipGenerator.Library.EmployeePayslip;
using EmployeePayslipGenerator.Library.ImportFile;
using EmployeePayslipGenerator.Library;

namespace EmployeePayslipGenerator.Library.ImportFile {
	public static class ImportFileHelper {
		#region Monthly Range Enum
		public enum MonthlyRange {
			[Description("01january-31january")]
			January,
			[Description("01february-28february")]
			February,
			[Description("01february-29february")]
			FebruaryLeapYear,
			[Description("01march-31march")]
			March,
			[Description("01april-30april")]
			April,
			[Description("01may-31may")]
			May,
			[Description("01june-30june")]
			June,
			[Description("01july-31july")]
			July,
			[Description("01august-31august")]
			August,
			[Description("01september-30september")]
			September,
			[Description("01october-31october")]
			October,
			[Description("01november-30november")]
			November,
			[Description("01december-31december")]
			December
		}
		#endregion

		#region Get Monthly Range Enum Description Attribute
		public static string GetEnumDescription(Enum value) {
			FieldInfo enumInfo = value.GetType().GetField(value.ToString());
			DescriptionAttribute[] attributes =
				(DescriptionAttribute[])enumInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

			if (attributes.Any() && attributes.Length > 0)
				return attributes[0].Description;

			return value.ToString();
		}
		#endregion
		
		#region Validate Each Row
		internal static List<string> ValidateEachRow(this ImportFileDetail importFileDetail) {
			List<string> errorDetails = new List<string>();
			errorDetails.Add(importFileDetail.ValidateFirstName());
			errorDetails.Add(importFileDetail.ValidateLastName());
			errorDetails.Add(importFileDetail.ValidateAnnualIncome());
			errorDetails.Add(importFileDetail.ValidateSuperRate());
			errorDetails.Add(importFileDetail.ValidateMonthStarting());
			return errorDetails.Where(errDetail => !string.IsNullOrEmpty(errDetail)).ToList();
		}
		#endregion

		#region Validate First Name
		internal static string ValidateFirstName(this ImportFileDetail importFileDetail) {
			return string.IsNullOrEmpty(importFileDetail.FirstName) ? @"Value for First Name cannot be blank" : string.Empty;
		}
		#endregion

		#region Validate Last Name
		internal static string ValidateLastName(this ImportFileDetail importFileDetail) {
			return string.IsNullOrEmpty(importFileDetail.LastName) ? @"Value for Last Name cannot be blank" : string.Empty;
		}
		#endregion

		#region Validate Annual Income
		internal static string ValidateAnnualIncome(this ImportFileDetail importFileDetail) {
			decimal annualIncome;
			string errorMessage = string.Empty;
			if (decimal.TryParse(importFileDetail.AnnualIncomeString.Replace(",", string.Empty).Replace("$", string.Empty),
								out annualIncome)) {
				importFileDetail.AnnualIncome = annualIncome;
			} else {
				importFileDetail.IsValid = false;
				errorMessage = @"Value for Annual Income is not a valid decimal";
			}
			return errorMessage;
		}
		#endregion

		#region Validate Super Rate & Super Rate Range
		internal static string ValidateSuperRate(this ImportFileDetail importFileDetail) {
			decimal superRate;
			string errorMessage = string.Empty;
			// Remove % character from super rate input
			string tempSuperRate = importFileDetail.SuperRateString.Replace("%", string.Empty);
			if (decimal.TryParse(tempSuperRate, out superRate)) {
				importFileDetail.SuperRate = superRate;

				if (importFileDetail.SuperRate < 0 || importFileDetail.SuperRate > 50) {
					errorMessage = @"Value for Super Rate must be between 0 and 50%";
				}
			} else {
				importFileDetail.IsValid = false;
				errorMessage = @"Value for Super Rate is not a valid decimal";
			}
			return errorMessage;
		}
		#endregion

		#region Validate Month Starting
		internal static string ValidateMonthStarting(this ImportFileDetail importFileDetail) {
			bool isValidMonthStarting = false;
			string errorMessage = string.Empty;
			string tempMonthStarting = importFileDetail.MonthStarting.Replace(" ", String.Empty).Trim().ToLower();
			foreach (MonthlyRange monthlyRange in Enum.GetValues(typeof(MonthlyRange))) {
				if (GetEnumDescription(monthlyRange) == tempMonthStarting) {
					isValidMonthStarting = true;
					break;
				}
			}

			if (!isValidMonthStarting) {
				importFileDetail.IsValid = false;
				errorMessage = @"Value for Month Starting is not valid";
			}
			return errorMessage;
		}
		#endregion

		#region Calculate Gross Monthly Income
		internal static void CalculateGrossMonthlyIncome(this ImportFileDetail importFileDetail) {
			importFileDetail.GrossMonthlyIncome = decimal.Round((importFileDetail.AnnualIncome / 12), 0);
		}
		#endregion

		#region Calculate Income Tax
		internal static void CalculateIncomeTax(this ImportFileDetail importFileDetail, IncomeTaxDetail incomeTaxDetail) {
			importFileDetail.IncomeTax = decimal.Round(((incomeTaxDetail.TaxOnIncome
														+ (importFileDetail.AnnualIncome - (incomeTaxDetail.LowerLimit - 1))*incomeTaxDetail.TaxPerDollar)/12), 0);
		}
		#endregion

		#region Calculate Net Income
		internal static void CalculateNetIncome(this ImportFileDetail importFileDetail) {
			importFileDetail.NetIncome = decimal.Round((importFileDetail.GrossMonthlyIncome - importFileDetail.IncomeTax), 0);
		}
		#endregion

		#region Calculate Super Deduction
		internal static void CalculateSuperDeduction(this ImportFileDetail importFileDetail) {
			importFileDetail.SuperDeduction = decimal.Round((importFileDetail.GrossMonthlyIncome * (importFileDetail.SuperRate / 100)), 0);
		}
		#endregion

		#region Format Payslip Output
		public static string FormatPayslipOutput(this ImportFileDetail importFileDetail) {
			return importFileDetail.FirstName + " " + importFileDetail.LastName + ',' + importFileDetail.MonthStarting + "," +
					importFileDetail.GrossMonthlyIncome.ToString() + "," + importFileDetail.IncomeTax.ToString() + "," +
					importFileDetail.NetIncome.ToString() + "," + importFileDetail.SuperDeduction.ToString();
		}
		#endregion

		#region Remove Illegal Characters from String
		public static string RemoveIllegalCharacters(this string input) {
			try {
				return Regex.Replace(input, "[^0-9A-Za-z ,]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
			} catch (Exception) {
				return input;
			}
		}
		#endregion
	}
}
