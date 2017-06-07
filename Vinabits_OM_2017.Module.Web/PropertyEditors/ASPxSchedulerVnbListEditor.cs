using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Scheduler.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vinabits_OM_2017.Module.BusinessObjects;
using DevExpress.ExpressApp.Model;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;
using DevExpress.ExpressApp;

namespace Vinabits_OM_2017.Module.Web.PropertyEditors
{
    [ListEditor(typeof(EventExtra),true)]
    public class ASPxSchedulerVnbListEditor : ASPxSchedulerListEditor
    {
        public ASPxSchedulerVnbListEditor(IModelListView model) : base(model)
        {
        }

        public override AppointmentMappingInfo AppointmentsMappings
        {
            get
            {
                AppointmentMappingInfo info = base.AppointmentsMappings;
                info.Subject = "Subject";
                return info;
            }
        }

        protected override ASPxScheduler CreateSchedulerControl()
        {
            ASPxScheduler control = base.CreateSchedulerControl();
            //control.InitAppointmentDisplayText += Control_InitAppointmentDisplayText;
            control.WorkWeekView.ShowWorkTimeOnly = true;
            control.WorkWeekView.WorkTime.Start = TimeSpan.Parse("06:00:00");
            control.WorkWeekView.WorkTime.End = TimeSpan.Parse("18:00:00");
            control.WorkDays.Add(WeekDays.Saturday);
            control.TimelineView.Enabled = false;
            control.MonthView.Enabled = false;
            control.WeekView.AppointmentDisplayOptions.AppointmentHeight = 130;
            control.WeekView.MoreButtonHTML = "Xem";
            control.WeekView.AppointmentDisplayOptions.AutoAdjustForeColor = true;
            control.WeekView.CellAutoHeightOptions.Mode = AutoHeightMode.FitToContent;
            control.CssClass = "aspxscheduler";
            return control;
        }

        protected override void OnControlsCreated()
        {
            base.OnControlsCreated();
            Employee currentEmployee = SecuritySystem.CurrentUser as Employee;
            bool isTasker = currentEmployee.EmployeeRoles.Where(role => role.Name == "Tasker").Count() > 0;
            if (!isTasker)
                this.SchedulerControl.OptionsCustomization.AllowAppointmentDrag = UsedAppointmentType.None;
        }

        private void Control_AllowAppointmentDrag(object sender, AppointmentOperationEventArgs e)
        {
            
                e.Allow = false;
        }
    }
}
