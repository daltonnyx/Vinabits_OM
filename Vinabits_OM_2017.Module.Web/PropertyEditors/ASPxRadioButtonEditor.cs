using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Web.Editors;
using DevExpress.Web;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxHtmlEditor;
using System.Web;
using DevExpress.ExpressApp.Utils;
using System.Collections.Generic;

namespace Vinabits_OM_2017.Module.Web.PropertyEditors
{
    [PropertyEditor(typeof(Enum), "ASPxRadioButtonToEnumEditors", false)]
    public class ASPxRadioButtonEditor : WebPropertyEditor
    {
        private EnumDescriptor enumDescriptor;
        private Dictionary<ASPxRadioButton, object> controlsHash = new Dictionary<ASPxRadioButton, object>();

        public ASPxRadioButtonEditor(Type objectType, IModelMemberViewItem info)
            : base(objectType, info) {
            this.enumDescriptor = new EnumDescriptor(MemberInfo.MemberType);
        }

        protected override WebControl CreateEditModeControlCore()
        {
            Panel placeHolder = new Panel();
            controlsHash.Clear();
            foreach (object enumValue in enumDescriptor.Values)
            {
                ASPxRadioButton radioButton = new ASPxRadioButton();
                radioButton.ID = "radioButton_" + enumValue.ToString();
                controlsHash.Add(radioButton, enumValue);
                radioButton.Text = enumDescriptor.GetCaption(enumValue);
                radioButton.CheckedChanged += new EventHandler(radioButton_CheckedChanged);
                radioButton.GroupName = propertyName;
                placeHolder.Controls.Add(radioButton);
            }
            return placeHolder;
        }

        void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            EditValueChangedHandler(sender, e);
        }
        protected override string GetPropertyDisplayValue()
        {
            return enumDescriptor.GetCaption(PropertyValue);
        }
        protected override void ReadEditModeValueCore()
        {
            object value = PropertyValue;
            if (value != null)
            {
                foreach (ASPxRadioButton radioButton in Editor.Controls)
                {
                    radioButton.Checked = value.Equals(controlsHash[radioButton]);
                }
            }
        }

        protected override object GetControlValueCore()
        {
            object result = null;
            foreach (ASPxRadioButton radioButton in Editor.Controls)
            {
                if (radioButton.Checked)
                {
                    result = controlsHash[radioButton];
                    break;
                }
            }
            return result;
        }
        public override void BreakLinksToControl(bool unwireEventsOnly)
        {
            if (Editor != null)
            {
                foreach (ASPxRadioButton radioButton in Editor.Controls)
                {
                    radioButton.CheckedChanged -= new EventHandler(radioButton_CheckedChanged);
                }
                if (!unwireEventsOnly)
                {
                    controlsHash.Clear();
                }
            }
            base.BreakLinksToControl(unwireEventsOnly);
        }
        protected override void SetImmediatePostDataScript(string script)
        {
            foreach (ASPxRadioButton radioButton in controlsHash.Keys)
            {
                radioButton.ClientSideEvents.CheckedChanged = script;
            }
        }
        public new Panel Editor
        {
            get { return (Panel)base.Editor; }
        }
    }
}