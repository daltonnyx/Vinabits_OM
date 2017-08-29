using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace Vinabits_OM_2017.Module.Reports
{
    public partial class VanBanReport : DevExpress.XtraReports.UI.XtraReport
    {
        public VanBanReport()
        {
            InitializeComponent();
            DateTime today = DateTime.Today;
            int delta = DayOfWeek.Monday - today.DayOfWeek;
            DateTime monday = today.AddDays(delta);
            DateTime saturday = today.AddDays(DayOfWeek.Saturday - today.DayOfWeek);
            this.TuNgay.Value = monday;
            this.DenNgay.Value = saturday.AddHours(12);
        }

    }
}
