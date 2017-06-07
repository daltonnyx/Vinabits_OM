using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using Vinabits_OM_2017.Module.BusinessObjects;

namespace Vinabits_OM_2017.Module.FunctionCriteriaOperator
{
    public class CurrentEmployeeOidOperator : ICustomFunctionOperator 
    {
        public string Name
        {
            get { return "CurrentEmployeeOid"; }
        }
        public object Evaluate(params object[] operands)
        {
            Guid oid = Guid.Empty;
            Employee currentUser;
            if (SecuritySystem.CurrentUser != null)
            {
                currentUser = (Employee)SecuritySystem.CurrentUser;
                oid = currentUser.Oid;
            }
            else if (SecuritySystem.CurrentUserId != null)
            {
                oid = (Guid)SecuritySystem.CurrentUserId;
            }

            return oid;
        }
        public Type ResultType(params Type[] operands)
        {
            return typeof(Guid);
        }

        static CurrentEmployeeOidOperator()
        {
            CurrentEmployeeOidOperator instance = new CurrentEmployeeOidOperator();
         if (CriteriaOperator.GetCustomFunction(instance.Name) == null) {
            CriteriaOperator.RegisterCustomFunction(instance);
         }
      }
      public static void Register() {
      }
    }
}
