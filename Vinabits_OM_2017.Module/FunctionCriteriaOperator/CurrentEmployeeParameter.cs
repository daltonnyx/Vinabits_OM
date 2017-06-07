using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using Vinabits_OM_2017.Module.BusinessObjects;


namespace Vinabits_OM_2017.Module.FunctionCriteriaOperator
{
    public class CurrentEmployeeOperator : ICustomFunctionOperator 
    {
        public string Name
        {
            get { return "CurrentEmployee"; }
        }
        public object Evaluate(params object[] operands)
        {
            Employee currentUser = null;
            if (SecuritySystem.CurrentUser != null)
            {
                currentUser = (Employee)SecuritySystem.CurrentUser;
            }
            else if (SecuritySystem.CurrentUserId != null && SecuritySystem.LogonObjectSpace != null)
            {
                currentUser = SecuritySystem.LogonObjectSpace.FindObject<Employee>(new BinaryOperator("Oid", SecuritySystem.CurrentUserId));
            }
            return currentUser;
        }
        public Type ResultType(params Type[] operands)
        {
            return typeof(Employee);
        }

        static CurrentEmployeeOperator()
        {
            CurrentEmployeeOperator instance = new CurrentEmployeeOperator();
         if (CriteriaOperator.GetCustomFunction(instance.Name) == null) {
            CriteriaOperator.RegisterCustomFunction(instance);
         }
      }
      public static void Register() {
      }
    }
}
