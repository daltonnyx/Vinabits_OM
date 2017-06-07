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
    #region CheckIsReadOperator - Xác định xem Employee có xem qua thông tin này hay chưa?
    public class CheckIsReadOperator : ICustomFunctionOperator //ICustomFunctionOperatorBrowsable, //ICustomFunctionOperator
    {
        public const string OperatorName = "CheckIsRead";
        public string Name
        {
            get { return OperatorName; }
        }
        public object Evaluate(params object[] operands)
        {
            if (!(operands != null && operands.Length == 1)) // && operands[0] is Guid
            {
                return false;
                //throw new ArgumentException("CheckIsRead operator should have one parameter - Guid.");
            }
            if(operands[0] == null)
            {
                return false;
            }
            bool ret = true;
            Employee emp = null;

            if(SecuritySystem.CurrentUser != null)
            {
                emp = (Employee)SecuritySystem.CurrentUser;
            }
            else if(SecuritySystem.CurrentUserId != null)
            {
                emp = SecuritySystem.LogonObjectSpace.FindObject<Employee>(new BinaryOperator("Oid", SecuritySystem.CurrentUserId));
            }

            if (emp != null && operands[0] != null)
            {
                string strCrit = string.Format("ObjectType = '{0}' And ObjOid = {{{1}}} And EmpAttack = {{{2}}} And TypeAttack = {3}",
                                                                operands[0].GetType().Name, ((BaseObject)operands[0]).Oid, emp.Oid, 0);
                TrackEmployee trackEmp = emp.Session.FindObject<TrackEmployee>(CriteriaOperator.Parse(strCrit));
                if(trackEmp != null)
                {
                    ret = false;
                }

                //using (ExplicitUnitOfWork euow = new ExplicitUnitOfWork(emp.Session.DataLayer))
                //{
                //    XPCollection<TrackEmployee> tracks = new XPCollection<TrackEmployee>(euow);
                //    string strCrit = string.Format("ObjectType = '{0}' And ObjOid = {{{1}}} And EmpAttack = {{{2}}} And TypeAttack = {3}",
                //                                                operands[0].GetType().Name, ((BaseObject)operands[0]).Oid, emp.Oid, 0);
                //                                                //EmpAttackType.t0_read ~ 0
                //    tracks.Criteria = CriteriaOperator.Parse(strCrit);
                //    tracks.Load();
                //    if (tracks != null && tracks.Count > 0)
                //        ret = false;
                //}
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
