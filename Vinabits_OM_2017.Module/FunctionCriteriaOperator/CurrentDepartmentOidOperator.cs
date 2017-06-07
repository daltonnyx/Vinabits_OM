using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinabits_OM_2017.Module.BusinessObjects;

namespace Vinabits_OM_2017.Module.FunctionCriteriaOperator
{
    public class CurrentDepartmentOidOperator : ICustomFunctionOperator
    {
        const string  CURRENT_DEPARTMENT_OID = "CurrentDepartmentOid";


        static CurrentDepartmentOidOperator()
        {
            CurrentDepartmentOidOperator instance = new CurrentDepartmentOidOperator();
            if(CriteriaOperator.GetCustomFunction(instance.Name) == null)
            {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }

        public static void Register()
        {

        }


        public string Name
        {
            get
            {
                return CURRENT_DEPARTMENT_OID;
            }
        }

        public object Evaluate(params object[] operands)
        {
            Guid oid = Guid.Empty;
            Employee currentUser;
            if (SecuritySystem.CurrentUser != null)
            {
                currentUser = (Employee)SecuritySystem.CurrentUser;
                oid = currentUser.Department.Oid;
            }
            return oid;
        }

        public Type ResultType(params Type[] operands)
        {
            return typeof(Guid);
        }
    }
}
