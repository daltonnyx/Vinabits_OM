using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp.Editors;
using System.Web.UI;
using System.Web.UI.WebControls;

using DevExpress.Xpo;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.TestScripts;
using DevExpress.ExpressApp.Web.Editors;
using System.Collections;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Filtering;
using System.Web;
using DevExpress.Persistent.BaseImpl;

namespace Vinabits_OM_2017.Module.Web.PropertyEditors {
    [PropertyEditor(typeof(BaseObject), EditorAliases.LookupPropertyEditor, false)]
    public class ASPxSearchLookupPropertyEditor : ASPxObjectPropertyEditorBase, IDependentPropertyEditor, ITestable, ISupportViewShowing, IFrameContainer {
        private static int windowWidth = 800;
        private static int windowHeight = 480;
        private WebLookupEditorHelper helper;
        private ASPxSearchDropDownEdit searchDropDownEdit;
        private ListView listView;
        internal NestedFrame frame;
        private NewObjectViewController newObjectViewController;
        private PopupWindowShowAction newObjectWindowAction;
        private object newObject;
        private IObjectSpace newObjectSpace;
        private string editorId;
        private List<IObjectSpace> createdObjectSpaces = new List<IObjectSpace>();
        protected override void SetImmediatePostDataScript(string script) {
            searchDropDownEdit.DropDown.ClientSideEvents.SelectedIndexChanged = script;
        }
        private void UpdateDropDownLookupControlAddButton(ASPxSearchDropDownEdit control) {
            control.AddingEnabled = false;
            if(CurrentObject != null) {
                string diagnosticInfo = "";
                RecreateListView(true);
                control.AddingEnabled = AllowEdit && DataManipulationRight.CanCreate(listView, helper.LookupObjectType, listView.CollectionSource, out diagnosticInfo);
                if(control.AddingEnabled) {
                    if(newObjectViewController != null) {
                        control.AddingEnabled = newObjectViewController.NewObjectAction.Active && newObjectViewController.NewObjectAction.Enabled;
                    }
                }
            }
        }
        private ASPxSearchDropDownEdit CreateSearchDropDownEditControl() {
            ASPxSearchDropDownEdit result = new ASPxSearchDropDownEdit(helper, EmptyValue, DisplayFormat);
            result.Width = Unit.Percentage(100);
            result.DropDown.SelectedIndexChanged += new EventHandler(dropDownLookup_SelectedIndexChanged);
            result.Init += new EventHandler(dropDownLookup_Init);
            result.PreRender += new EventHandler(dropDownLookup_PreRender);
            result.Callback += new EventHandler<CallbackEventArgs>(result_Callback);
            result.ReadOnly = !AllowEdit;
            UpdateDropDownLookup(result);
            return result;
        }
        private void result_Callback(object sender, CallbackEventArgs e) {
            FillSearchDropDownValues(GetObjectByKey(String.Format("{0}({1})", Helper.LookupObjectTypeInfo, e.Argument)));
        }
        private void UpdateDropDownLookup(WebControl editor) {
            ASPxSearchDropDownEdit supportNewObjectCreating = editor as ASPxSearchDropDownEdit;
            if(supportNewObjectCreating != null) {
                if(newObjectViewController != null) {
                    supportNewObjectCreating.NewActionCaption = newObjectViewController.NewObjectAction.Caption;
                }
                UpdateDropDownLookupControlAddButton(supportNewObjectCreating);
                if(application != null) {
                    supportNewObjectCreating.SetClientNewButtonScript(application.PopupWindowManager.GenerateModalOpeningScript(editor, newObjectWindowAction, WindowWidth, WindowHeight, false, supportNewObjectCreating.GetProcessNewObjFunction()));
                }
            }
        }
        private void dropDownLookup_SelectedIndexChanged(object source, EventArgs e) {
            EditValueChangedHandler(source, EventArgs.Empty);
        }
        private void dropDownLookup_Init(object sender, EventArgs e) {
            UpdateDropDownLookup((WebControl)sender);
        }
        private void dropDownLookup_PreRender(object sender, EventArgs e) {
            UpdateDropDownLookup((WebControl)sender);
        }
        private object GetObjectByKey(string key) {
            return helper.GetObjectByKey(CurrentObject, key);
        }
        private string EscapeObjectKey(string key) {
            return key.Replace("'", "\\'");
        }
        private void newObjectViewController_ObjectCreating(object sender, ObjectCreatingEventArgs e) {
            e.ShowDetailView = false;
            // B196715
            if(e.ObjectSpace is INestedObjectSpace) {
                e.ObjectSpace = application.CreateObjectSpace();
            }
        }
        private void newObjectViewController_ObjectCreated(object sender, DevExpress.ExpressApp.SystemModule.ObjectCreatedEventArgs e) {
            newObject = e.CreatedObject;
            newObjectSpace = e.ObjectSpace;
            createdObjectSpaces.Add(newObjectSpace);
        }
        private void newObjectWindowAction_OnCustomizePopupWindowParams(Object sender, CustomizePopupWindowParamsEventArgs args) {
            if(!this.DataSource.AllowAdd) {
                throw new InvalidOperationException();
            }
            if(newObjectViewController != null) {
                //TODO MINAKOV rewrite
                OnViewShowingNotification();//CaptionHelper.GetLocalizedText("DialogButtons", "Add"));
                newObjectViewController.NewObjectAction.DoExecute(newObjectViewController.NewObjectAction.Items[0]);
                args.View = application.CreateDetailView(newObjectSpace, newObject, listView);
            }
        }
        private void newObjectWindowAction_OnExecute(Object sender, PopupWindowShowActionExecuteEventArgs args) {
            if(!this.DataSource.AllowAdd) {
                throw new InvalidOperationException();
            }
            if(objectSpace != args.PopupWindow.View.ObjectSpace) {
                args.PopupWindow.View.ObjectSpace.CommitChanges();
            }
            DetailView detailView = (DetailView)args.PopupWindow.View;
            DataSource.Add(helper.ObjectSpace.GetObject(detailView.CurrentObject));
            ((PopupWindow)args.PopupWindow).ClosureScript = "if(window.opener != null) window.opener.ddLookupResult = '" + detailView.ObjectSpace.GetKeyValueAsString(detailView.CurrentObject) + "';";
        }
        private void RecreateListView(bool ifNotCreatedOnly) {
            if(ViewEditMode == ViewEditMode.Edit && (!ifNotCreatedOnly || listView == null)) {
                listView = null;
                if(CurrentObject != null) {
                    listView = helper.CreateListView(CurrentObject);
                }
                Frame.SetView(listView);
            }
        }
        private void OnViewShowingNotification() {
            if(viewShowingNotification != null) {
                viewShowingNotification(this, EventArgs.Empty);
            }
        }
        protected override void ApplyReadOnly() {
            if(searchDropDownEdit != null) {
                searchDropDownEdit.ReadOnly = !AllowEdit;
            }
        }
        protected override WebControl CreateEditModeControlCore() {
            if(newObjectWindowAction == null) {
                newObjectWindowAction = new PopupWindowShowAction(null, "New", PredefinedCategory.Unspecified.ToString());
                newObjectWindowAction.Execute += new PopupWindowShowActionExecuteEventHandler(newObjectWindowAction_OnExecute);
                newObjectWindowAction.CustomizePopupWindowParams += new CustomizePopupWindowParamsEventHandler(newObjectWindowAction_OnCustomizePopupWindowParams);
                newObjectWindowAction.Application = helper.Application;
            }

            Panel panel = new Panel(); // Use Panel instead of ASPxPanel cause it doesn't affect editor ClientID            
            searchDropDownEdit = CreateSearchDropDownEditControl();
            panel.Controls.Add(searchDropDownEdit);
            return panel;
        }
        protected override object GetControlValueCore() {
            if(ViewEditMode == ViewEditMode.Edit && Editor != null) {
                DevExpress.Web.ASPxComboBox dropDownControl = searchDropDownEdit.DropDown;
                if(dropDownControl.Value != null && dropDownControl.Value.ToString() != WebPropertyEditor.EmptyValue && DataSource is CollectionSourceBase) {
                    CollectionSourceBase collectionSource = (CollectionSourceBase)DataSource;
                    string objectKey = String.Format("{0}({1})", Helper.LookupObjectTypeInfo, dropDownControl.Value);
                    return GetObjectByKey(objectKey);
                }
                return null;
            }
            return MemberInfo.GetValue(CurrentObject);
        }
        protected override void OnCurrentObjectChanged() {
            if(Editor != null) {
                RecreateListView(false);
            }
            base.OnCurrentObjectChanged();
            UpdateDropDownLookup(searchDropDownEdit);
        }
        protected override string GetPropertyDisplayValue() {
            return helper.GetDisplayText(MemberInfo.GetValue(CurrentObject), EmptyValue, DisplayFormat);
        }
        private void FillSearchDropDownValues(object item) {
            searchDropDownEdit.DropDown.Items.Clear();
            if(item != null) {
                searchDropDownEdit.DropDown.DataSource = new List<object>(new object[] { item });
                searchDropDownEdit.DropDown.DataBind();
            }
            searchDropDownEdit.DropDown.SelectedIndex = searchDropDownEdit.DropDown.Items.Count - 1;
        }
        protected override void ReadEditModeValueCore() {
            if(searchDropDownEdit != null) {
                FillSearchDropDownValues(PropertyValue);
            }
        }
        public void SetValueToControl(object obj) {
            DevExpress.Web.ASPxComboBox Control = null;
            if(searchDropDownEdit != null) {
                Control = searchDropDownEdit.DropDown;
            }
            foreach(DevExpress.Web.ListEditItem item in Control.Items) {
                string val = item.Value as string;
                if(val == helper.GetObjectKey(obj)) {
                    Control.SelectedIndex = item.Index;
                    break;
                }
            }
        }
        //protected override IJScriptTestControl GetEditorTestControlImpl() {
        //    return new JSASPxSimpleLookupTestControl();
        //}
        protected override IJScriptTestControl GetInplaceViewModeEditorTestControlImpl() {
            return new JSButtonTestControl();
        }
        protected override WebControl GetActiveControl() {
            if(searchDropDownEdit != null) {
                return searchDropDownEdit.DropDown;
            }
            return base.GetActiveControl();
        }
        protected override string GetEditorClientId() {
            return searchDropDownEdit.ClientID;
        }
        private void UpdateControlId() {
            searchDropDownEdit.ID = editorId;
        }
        protected override void SetEditorId(string controlId) {
            this.editorId = controlId;
            UpdateControlId();
        }
        protected override void Dispose(bool disposing) {
            try {
                if(disposing) {
                    if(newObjectWindowAction != null) {
                        newObjectWindowAction.Execute -= new PopupWindowShowActionExecuteEventHandler(newObjectWindowAction_OnExecute);
                        newObjectWindowAction.CustomizePopupWindowParams -= new CustomizePopupWindowParamsEventHandler(newObjectWindowAction_OnCustomizePopupWindowParams);
                        DisposeAction(newObjectWindowAction);
                        newObjectWindowAction = null;
                    }
                    if(newObjectViewController != null) {
                        newObjectViewController.ObjectCreating -= new EventHandler<ObjectCreatingEventArgs>(newObjectViewController_ObjectCreating);
                        newObjectViewController.ObjectCreated -= new EventHandler<ObjectCreatedEventArgs>(newObjectViewController_ObjectCreated);
                        newObjectViewController = null;
                    }
                    if(frame != null) {
                        frame.SetView(null);
                        frame.Dispose();
                        frame = null;
                    }
                    if(listView != null) {
                        listView.Dispose();
                        listView = null;
                    }
                    foreach(IObjectSpace createdObjectSpace in createdObjectSpaces) {
                        if(!createdObjectSpace.IsDisposed) {
                            createdObjectSpace.Dispose();
                        }
                    }
                    createdObjectSpaces.Clear();
                    newObject = null;
                    newObjectSpace = null;
                }
            }
            finally {
                base.Dispose(disposing);
            }
        }
        public WebLookupEditorHelper WebLookupEditorHelper {
            get { return helper; }
        }
        public override void BreakLinksToControl(bool unwireEventsOnly) {
            if(searchDropDownEdit != null) {
                searchDropDownEdit.DropDown.SelectedIndexChanged -= new EventHandler(dropDownLookup_SelectedIndexChanged);
                searchDropDownEdit.Init -= new EventHandler(dropDownLookup_Init);
                searchDropDownEdit.PreRender -= new EventHandler(dropDownLookup_PreRender);
                searchDropDownEdit.Callback -= new EventHandler<CallbackEventArgs>(result_Callback);
            }
            if(!unwireEventsOnly) {
                searchDropDownEdit = null;
            }
            base.BreakLinksToControl(unwireEventsOnly);
        }
        public ASPxSearchLookupPropertyEditor(Type objectType, IModelMemberViewItem model)
            : base(objectType, model) {
            skipEditModeDataBind = true;
        }
        public void SetControlValue(object val) {
            object selectedObject = GetControlValueCore();
            if(((selectedObject == null && val == null) || (selectedObject != val)) && (CurrentObject != null)) {
                OnValueStoring(helper.GetDisplayText(val, EmptyValue, DisplayFormat));
                MemberInfo.SetValue(CurrentObject, helper.ObjectSpace.GetObject(val));
                OnValueStored();
                ReadValue();
            }
        }
        public override void Setup(IObjectSpace objectSpace, XafApplication application) {
            base.Setup(objectSpace, application);
            helper = new WebLookupEditorHelper(application, objectSpace, MemberInfo.MemberTypeInfo, Model);
        }

        /*void IDependentPropertyEditor.Refresh() {
            this.ReadValue();
        }*/
        IList<string> IDependentPropertyEditor.MasterProperties {
            get { return helper.MasterProperties; }
        }
        protected CollectionSourceBase DataSource {
            get {
                if(listView != null) {
                    return listView.CollectionSource;
                }
                return null;
            }
        }

        public static int WindowWidth {
            get { return windowWidth; }
            set { windowWidth = value; }
        }
        public static int WindowHeight {
            get { return windowHeight; }
            set { windowHeight = value; }
        }
        internal LookupEditorHelper Helper {
            get { return helper; }
        }
        #region ISupportViewShowing Members
        private event EventHandler<EventArgs> viewShowingNotification;
        event EventHandler<EventArgs> ISupportViewShowing.ViewShowingNotification {
            add { viewShowingNotification += value; }
            remove { viewShowingNotification -= value; }
        }
        #endregion

        internal string GetSearchActionName() {
            return Frame.GetController<FilterController>().FullTextFilterAction.Caption;
        }

        #region IFrameContainer Members

        public Frame Frame {
            get {
                InitializeFrame();
                return frame;
            }
        }
        public void InitializeFrame() {
            if(frame == null) {
                frame = helper.Application.CreateNestedFrame(this, TemplateContext.LookupControl);
                newObjectViewController = frame.GetController<NewObjectViewController>();
                if(newObjectViewController != null) {
                    newObjectViewController.ObjectCreating += new EventHandler<ObjectCreatingEventArgs>(newObjectViewController_ObjectCreating);
                    newObjectViewController.ObjectCreated += new EventHandler<ObjectCreatedEventArgs>(newObjectViewController_ObjectCreated);
                }
            }
        }

        #endregion
    }

    public class ASPxSearchDropDownEdit : Table, INamingContainer, ICallbackEventHandler {
        private DevExpress.Web.ASPxComboBox dropDown;
        private DevExpress.Web.EditButton newButton;
        private DevExpress.Web.EditButton clearButton;
        private bool addingEnabled;
        private bool isPrerendered = false;
        private string newButtonScript;
        private string clearButtonScript;
        private void UpdateClientButtonsScript() {
            dropDown.ClientSideEvents.ButtonClick = @"function(s, e) {
                if(e.buttonIndex == 0) {" +
                    newButtonScript +
                @"}
                if(e.buttonIndex == 1) {" +
                    clearButtonScript +
                @"}             
            }";
        }
        public ASPxSearchDropDownEdit(WebLookupEditorHelper helper, string emptyValue, string displayFormat) {
            this.Helper = helper;
            this.EmptyValue = emptyValue;
            this.DisplayFormat = displayFormat;

            dropDown = RenderHelper.CreateASPxComboBox();
            dropDown.ID = "DD";
            dropDown.Width = Unit.Percentage(100);
            dropDown.CssClass = "xafLookupEditor";
            dropDown.IncrementalFilteringMode = DevExpress.Web.IncrementalFilteringMode.Contains;
            dropDown.DropDownButton.Visible = false;
            dropDown.EnableCallbackMode = true;
            dropDown.CallbackPageSize = 10;
            dropDown.ItemRequestedByValue += new DevExpress.Web.ListEditItemRequestedByValueEventHandler(dropDown_ItemRequestedByValue);
            dropDown.ItemsRequestedByFilterCondition += new DevExpress.Web.ListEditItemsRequestedByFilterConditionEventHandler(dropDown_ItemsRequestedByFilterCondition);

            dropDown.TextField = Helper.DisplayMember.Name;
            dropDown.ValueField = Helper.LookupObjectTypeInfo.KeyMember.Name;
            //dropDown.ValueField = String.Format("{0}({1})", Helper.LookupObjectTypeInfo.ToString(), Helper.LookupObjectTypeInfo.KeyMember.Name);
            //TODO Add columns
            if(Helper.LookupObjectTypeInfo.Type.FullName == "MainDemo.Module.BusinessObjects.Contact") {
                dropDown.Columns.Add("FullName", "FullName", 300);
                dropDown.Columns.Add("SpouseName", "SpouseName", 300);
                dropDown.TextFormatString = "{0} {1}";
            }

            newButton = dropDown.Buttons.Add();
            clearButton = dropDown.Buttons.Add();
            ASPxImageHelper.SetImageProperties(newButton.Image, "Action_New_12x12");
            ASPxImageHelper.SetImageProperties(clearButton.Image, "Editor_Clear");

            CellPadding = 0;
            CellSpacing = 0;
            Rows.Add(new TableRow());
            Rows[0].Cells.Add(new TableCell());

            Rows[0].Cells[0].Width = Unit.Percentage(100);
            Rows[0].Cells[0].Attributes["align"] = "left";
            Rows[0].Cells[0].Attributes["valign"] = "middle";
            Rows[0].Cells[0].Controls.Add(dropDown);
        }


        const int NUMBER_CHAR_SEARCH = 1;
        void dropDown_ItemsRequestedByFilterCondition(object source, DevExpress.Web.ListEditItemsRequestedByFilterConditionEventArgs e) {
            if(String.IsNullOrEmpty(e.Filter) || e.Filter.Length < NUMBER_CHAR_SEARCH)
                return;
            DevExpress.Web.ASPxComboBox editor = source as DevExpress.Web.ASPxComboBox;
            //editor.Items.Clear();
            //IList ds = GetLookupSource(e.Filter);
            //for(int i = e.BeginIndex; i < Math.Min(e.EndIndex + 1, ds.Count); i++) {
            //    editor.Items.Add(Helper.GetEscapedDisplayText(ds[i], EmptyValue, DisplayFormat), Helper.GetObjectKey(ds[i]));
            //}
            editor.DataSource = GetLookupSource(e.Filter);
            editor.DataBind();
        }
        void dropDown_ItemRequestedByValue(object source, DevExpress.Web.ListEditItemRequestedByValueEventArgs e) {

        }
        public IList GetLookupSource(string filter) {
            SearchCriteriaBuilder criteriaBuilder = new SearchCriteriaBuilder();
            criteriaBuilder.TypeInfo = Helper.LookupObjectTypeInfo;
            criteriaBuilder.SearchInStringPropertiesOnly = false;
            criteriaBuilder.SearchText = filter;
            criteriaBuilder.SearchMode = SearchMode.SearchInObject;
            criteriaBuilder.SetSearchProperties("FullName", "SpouseName");
            return ObjectSpace.GetObjects(Helper.LookupObjectType, criteriaBuilder.BuildCriteria());
            //return ObjectSpace.GetObjects(Helper.LookupObjectType, new BinaryOperator(Helper.DisplayMember.Name, "%" + filter + "%", BinaryOperatorType.Like));
        }
        protected override void OnPreRender(EventArgs e) {
            isPrerendered = true;
            if(!ReadOnly) {
                clearButtonScript = @"var processOnServer = false;
						var dropDownControl = aspxGetControlCollection().Get('" + dropDown.ClientID + @"');
						if(dropDownControl) {
							dropDownControl.ClearItems();                            
							processOnServer = dropDownControl.RaiseValueChangedEvent();
						}
						e.processOnServer = processOnServer;";
                UpdateClientButtonsScript();
            }
            else {
                clearButton.Visible = false;
                newButton.Visible = false;
            }
            base.OnPreRender(e);
        }
        protected override void Render(HtmlTextWriter writer) {
            if(!isPrerendered) {
                OnPreRender(EventArgs.Empty);
            }
            base.Render(writer);
        }
        public DevExpress.Web.ASPxComboBox DropDown {
            get { return dropDown; }
        }
        public bool ReadOnly {
            get { return dropDown.ReadOnly; }
            set {
                dropDown.ReadOnly = value;
                dropDown.Enabled = !value;
            }
        }
        public IObjectSpace ObjectSpace { get { return (IObjectSpace)Helper.ObjectSpace; } }
        public WebLookupEditorHelper Helper { get; set; }
        public string EmptyValue { get; set; }
        public string DisplayFormat { get; set; }

        public void SetClientNewButtonScript(string value) {
            newButtonScript = value;
            UpdateClientButtonsScript();
        }
        public bool AddingEnabled {
            get {
                return addingEnabled;
            }
            set {
                addingEnabled = value;
                if(newButton != null) {
                    newButton.Enabled = value;
                    newButton.Visible = value;
                }
            }
        }
        public string NewActionCaption {
            get {
                return newButton.Text;
            }
            set {
                newButton.ToolTip = value;
                if(newButton.Image.IsEmpty) {
                    newButton.Text = value;
                }
            }
        }
        public string GetProcessNewObjFunction() {
            return "xafDropDownLookupProcessNewObject('" + UniqueID + "')";
        }

        #region ICallbackEventHandler Members
        public event EventHandler<CallbackEventArgs> Callback;
        public string GetCallbackResult() {
            StringBuilder result = new StringBuilder();
            foreach(DevExpress.Web.ListEditItem item in dropDown.Items) {
                result.AppendFormat("{0}<{1}{2}|", HttpUtility.HtmlAttributeEncode(item.Text), item.Value, dropDown.SelectedItem == item ? "<" : "");
            }
            if(result.Length > 0) {
                result.Remove(result.Length - 1, 1);
            }
            return string.Format("{0}><{1}", dropDown.ClientID, result.ToString());
        }
        public void RaiseCallbackEvent(string eventArgument) {
            if(Callback != null) {
                Callback(this, new CallbackEventArgs(eventArgument));
            }
        }
        #endregion
    }

}
