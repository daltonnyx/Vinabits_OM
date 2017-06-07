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
            
            return control;
        }

        protected override void OnControlsCreated()
        {
            base.OnControlsCreated();
            Employee currentEmployee = SecuritySystem.CurrentUser as Employee;

            this.SchedulerControl.WorkWeekView.ShowWorkTimeOnly = true;
            this.SchedulerControl.WorkWeekView.WorkTime.Start = TimeSpan.Parse("06:00:00");
            this.SchedulerControl.WorkWeekView.WorkTime.End = TimeSpan.Parse("18:00:00");
            this.SchedulerControl.WorkDays.Add(WeekDays.Saturday);
            this.SchedulerControl.TimelineView.Enabled = false;
            this.SchedulerControl.MonthView.Enabled = false;
            this.SchedulerControl.WeekView.AppointmentDisplayOptions.AppointmentHeight = 130;
            this.SchedulerControl.WeekView.MoreButtonHTML = "Xem thêm";
            this.SchedulerControl.WeekView.AppointmentDisplayOptions.AutoAdjustForeColor = true;
            this.SchedulerControl.WeekView.CellAutoHeightOptions.Mode = AutoHeightMode.FitToContent;
            this.SchedulerControl.CssClass = "aspxscheduler";
            this.SchedulerControl.WorkWeekView.MoreButtonHTML = "Xem thêm";
            this.SchedulerControl.ActiveView.ShowMoreButtons = true;
            this.SchedulerControl.OptionsCustomization.AllowAppointmentResize = UsedAppointmentType.None;

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
