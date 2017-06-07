using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using Vinabits_OM_2017.Module.BusinessObjects;

namespace Vinabits_OM_2017.Module.FunctionCriteriaOperator
{
    public class CurrentEmployeeUserNameOperator : ICustomFunctionOperator 
    {
        public string Name
        {
            get { return "CurrentEmployeeUserName"; }
        }
        public object Evaluate(params object[] operands)
        {
            string username = string.Empty;
            Employee currentUser;
            if (SecuritySystem.CurrentUserName != null)
            {
                username = SecuritySystem.CurrentUserName;
            }
            else if (SecuritySystem.CurrentUser != null)
            {
                currentUser = (Employee)SecuritySystem.CurrentUser;
                username = currentUser.UserName;
            }
            return username;
        }
        public Type ResultType(params Type[] operands)
        {
            return typeof(Guid);
        }

        static CurrentEmployeeUserNameOperator()
        {
            CurrentEmployeeUserNameOperator instance = new CurrentEmployeeUserNameOperator();
         if (CriteriaOperator.GetCustomFunction(instance.Name) == null) {
            CriteriaOperator.RegisterCustomFunction(instance);
         }
      }
      public static void Register() {
      }
    }
}
