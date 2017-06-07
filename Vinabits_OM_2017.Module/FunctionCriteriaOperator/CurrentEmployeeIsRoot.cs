using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using Vinabits_OM_2017.Module.BusinessObjects;
using DevExpress.ExpressApp;

namespace Vinabits_OM_2017.Module.FunctionCriteriaOperator
{
    class CurrentEmployeeIsRootOperator : ICustomFunctionOperator
    {
        public const string OperatorName = "CurrentEmployeeIsRoot";
        public string Name
        {
            get { return OperatorName; }
        }
        public object Evaluate(params object[] operands)
        {
            bool ret = false;
            Employee emp = null;
            if (SecuritySystem.CurrentUser != null)
            {
                emp = (Employee)SecuritySystem.CurrentUser;
            }
            else if (SecuritySystem.CurrentUserId != null)
            {
                emp = SecuritySystem.LogonObjectSpace.FindObject<Employee>(new BinaryOperator("Oid", SecuritySystem.CurrentUserId));
            }

            if (emp != null)
            {
                foreach (EmployeeRole er in emp.EmployeeRoles)
                {
                    if (er.Name == "Root") { ret = true; }
                }
            }

            return ret;
        }
        public Type ResultType(params Type[] operands)
        {
            return typeof(bool);
        }
        static CurrentEmployeeIsRootOperator()
        {
            CurrentEmployeeIsRootOperator instance = new CurrentEmployeeIsRootOperator();
            if (CriteriaOperator.GetCustomFunction(instance.Name) == null)
            {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }

        public static void Register()
        {
        }
    }
}
