using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroductionToEntityFramework
{
    class _IntroductionToEntityFramework
    {
        static void Main(string[] args)
        {
            //Each problem is solved in different class - uncomment the class to start executing the solution for given problem with CTRL+F5:

            var gringottsContext = new GringottsContext();
            //FirstLetter.GetFirstLetter(gringottsContext);

            var context = new SoftuniContext();
            //EmployeesFullInformation.GetEmployeesFullInformation(context);
            //EmployeesWithSalaryOver5000.GetEmployeesWithSalariesOver5000(context);
            //EmployeesFromSeattle.GetEmployeesFromSeattle(context);
            //AddNewAddressAndUpdateEmployee.AddingNewAddressAndUpdateEmployee(context);
            //FindEmployeesInPeriod.FindingEmployeesInPeriod(context);
            //the result on the console depends on the regional options in the control panel, because
            //the project.EndDate can be Null and doesn't support .ToString();
            //for correct result in Judge the regional options for "Date formats - Short date" should be "M/d/yyyy";
            //AddressesByTownName.GetAddressesByTownName(context);
            //EmployeeWithId147.GetAddressesByTownName(context);
            //DepartmentsWithMoreThanFiveEmployees.GetDepartmentsWithMoreThanFiveEmployees(context);
            //FindLatestTenProjects.FindingLatestTenProjects(context);
            //IncreaseSalaries.IncreaseSalariesMethod(context);
            //FindEmployeesByFirstNameStartingWith.FindEmployeesByFirstName(context);
            //DeleteProjectById.DeleteProjectWithId(context);
            //RemoveTowns.RemoveTown(context);
            //NativeSQLQuerry.UseNativeSQLQuerry(context);
        }
    }
}
