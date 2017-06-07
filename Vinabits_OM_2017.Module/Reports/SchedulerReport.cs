using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Globalization;

namespace Vinabits_OM_2017.Module.Reports
{
    public partial class SchedulerReport : DevExpress.XtraReports.UI.XtraReport
    {
        public SchedulerReport()
        {
            InitializeComponent();
            
            DateTime today = DateTime.Today;
            int delta = DayOfWeek.Monday - today.DayOfWeek;
            DateTime monday = today.AddDays(delta);
            DateTime saturday = today.AddDays(DayOfWeek.Saturday - today.DayOfWeek);
            this.startTime.Value = monday;
            this.endTime.Value = saturday.AddHours(12);
            this.week.GetValue += Week_GetValue;
        }

        private void Week_GetValue(object sender, GetValueEventArgs e)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            e.Value = dfi.Calendar.GetWeekOfYear((DateTime)this.startTime.Value, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }

        private void weekLabel_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Nullable<DateTime> start = this.startTime.Value as Nullable<DateTime>;
            this.xrLabel8.Text = "Thứ hai";
            this.xrLabel11.Text = "Thứ ba";
            this.xrLabel18.Text = "Thứ tư";
            this.xrLabel23.Text = "Thứ năm";
            this.xrLabel24.Text = "Thứ sáu";
            this.xrLabel27.Text = "Thứ bảy";
            if (start != null) {
                DateTime date = start.Value;
                this.xrLabel8.Text += "\n" + date.ToString("dd/MM");
                this.xrLabel11.Text += "\n" + date.AddDays(1).ToString("dd/MM");
                this.xrLabel18.Text += "\n" + date.AddDays(2).ToString("dd/MM");
                this.xrLabel23.Text += "\n" + date.AddDays(3).ToString("dd/MM");
                this.xrLabel24.Text += "\n" + date.AddDays(4).ToString("dd/MM");
                this.xrLabel27.Text += "\n" + date.AddDays(5).ToString("dd/MM");
            }
        }

        private void SchedulerReport_ParametersRequestSubmit_1(object sender, DevExpress.XtraReports.Parameters.ParametersRequestEventArgs e)
        {
           
        }

        private void SchedulerReport_BandHeightChanged(object sender, BandEventArgs e)
        {
            
        }
    }
}
