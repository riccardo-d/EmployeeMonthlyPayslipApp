using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EmployeeMonthlyPayslipApp.Startup))]
namespace EmployeeMonthlyPayslipApp
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
        }
    }
}
