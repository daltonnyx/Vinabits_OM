using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using Vinabits_OM_2017.Module.BusinessObjects;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System.Reflection;
using DevExpress.Persistent.BaseImpl;

namespace Vinabits_OM_2017.Module.FunctionCriteriaOperator
{
    #region DocumentCheckIsFollowOperator - Xác định xem Employee đánh dấu theo dõi văn bản này không?
    public class DocumentCheckIsFollowOperator : ICustomFunctionOperator
    {
        public const string OperatorName = "DocumentCheckIsFollow";
        public string Name
        {
            get { return OperatorName; }
        }
        public object Evaluate(params object[] operands)
        {
            if (!(operands != null && operands.Length >= 1)) 
            {
                return false;
            }

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

            if (emp != null && operands[0] != null && operands[0].GetType() == typeof(Document))
            {
                Document curDoc = operands[0] as Document;
                CriteriaOperator crit = CriteriaOperator.And(new BinaryOperator("LinkEmployee.Oid", emp.Oid), new BinaryOperator("LinkDocument.Oid", curDoc.Oid));

                DocumentEmployees docEmp = emp.Session.FindObject<DocumentEmployees>(crit);
                if (docEmp != null && docEmp.IsFollow)
                    ret = true;
            }

            return ret;
        }
        public Type ResultType(params Type[] operands)
        {
            return typeof(bool);
        }
    }
    #endregion
}
