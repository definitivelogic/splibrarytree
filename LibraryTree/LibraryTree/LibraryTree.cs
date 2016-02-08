using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace LibraryTree
{
    public class LibraryTree : Microsoft.SharePoint.WebPartPages.WebPart
    {

        public LibraryTree()
        {
            this.Title = "Document Library Tree View";
            this.ExportMode = WebPartExportMode.All;
        }

        protected string _LibraryName = "";
        [Personalizable(PersonalizationScope.Shared),
            Browsable(false),
            Category("Tree View Settings")]
        public string LibraryName
        {
            get { return _LibraryName; }
            set { _LibraryName = value; }
        }

        protected int _ExpandDepth = 0;
        [Personalizable(PersonalizationScope.Shared),
            Browsable(false),
            Category("Tree View Settings")]
        public int ExpandDepth
        {
            get { return _ExpandDepth; }
            set { _ExpandDepth = value; }
        }

        protected bool _ShowLines = false;
        [Personalizable(PersonalizationScope.Shared),
            Browsable(false),
            Category("Tree View Settings")]
        public bool ShowLines
        {
            get { return _ShowLines; }
            set { _ShowLines = value; }
        }

        public override EditorPartCollection CreateEditorParts()
        {
            List<EditorPart> parts = new List<EditorPart>();
            LibraryTreeEditorPart libraryEditorPart = new LibraryTreeEditorPart();
            libraryEditorPart.ID = this.ID + "_LibraryTreeEditor";
            parts.Add(libraryEditorPart);
            return new EditorPartCollection(base.CreateEditorParts(), parts);
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            SPWeb wb = SPContext.Current.Web;
            string baseURL = wb.Url.ToString();
            try
            {
                if (_LibraryName == "")
                {
                    throw new Exception("No Document Library selected. Please select one in the web part properties pane.");
                }

                SPDocumentLibrary doclib = (SPDocumentLibrary)wb.Lists[_LibraryName];

                SPTreeView TreeView1 = new SPTreeView();
                SPFolder root = doclib.RootFolder;
                TreeNode node = new TreeNode();
                node = Utility.GetFolderNode(node, root, baseURL);
                node.Text = doclib.Title;
                node.NavigateUrl = doclib.DefaultViewUrl;
                long size = Utility.GetFolderSize(root) / 1024;
                long numFiles = Utility.GetNumberOfFilesInFolder(root);
                node.ToolTip = "Size: " + size.ToString() + " KBs " + " Files: " + numFiles.ToString();
                node.ImageUrl = baseURL + "/_layouts/15/images/folder.gif";
                TreeView1.Nodes.Add(node);
                TreeView1.EnableViewState = false;
                TreeView1.ShowLines = _ShowLines;                
                TreeView1.ExpandDepth = _ExpandDepth;
                this.Controls.Add(TreeView1);
            }
            catch (Exception ex)
            {
                Label errorLabel = new Label();
                string errorType = ex.GetType().Name;
                string errorMessage = "";
                if (errorType == "InvalidCastException")
                {
                    errorMessage = "Error: Please select a document library for tree view.";
                }
                if (errorType == "ArgumentException")
                {
                    errorMessage = "Error: There is no such document library with this name: " + _LibraryName;
                }
                errorLabel.Text = errorMessage;
                this.Controls.Add(errorLabel);
            }
        }

        public void SaveChanges()
        {
            this.SetPersonalizationDirty();
        }
    }

    public class LibraryTreeEditorPart : EditorPart
    {
        private TextBox _libraryName;
        private TextBox _expandDepth;
        private CheckBox _showLines;

        public LibraryTreeEditorPart() : base()
        {
            this.Title = "Library Tree Settings";
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            LibraryTree parent = this.WebPartToEdit as LibraryTree;
            _libraryName = new TextBox();
            _libraryName.Text = parent.LibraryName;
            _expandDepth = new TextBox();
            _expandDepth.Text = parent.ExpandDepth.ToString();
            _showLines = new CheckBox();
            _showLines.Checked = parent.ShowLines;

        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            // list name text box
            this.Controls.Add(new LiteralControl("<div class='UserSectionHead'>"));
            Label listNameHeader = new Label();
            listNameHeader.Text = "Name of Document Library to view:";
            listNameHeader.ToolTip = "The name of a Document Library on the current site.";
            this.Controls.Add(listNameHeader);
            this.Controls.Add(new LiteralControl("</div><div class='UserSectionBody'><div class='UserControlGroup'>"));
            this.Controls.Add(_libraryName);
            this.Controls.Add(new LiteralControl("</div></div><div style='width:100%' class='UserDottedLine'></div>"));

            // expand depth text box
            this.Controls.Add(new LiteralControl("<div class='UserSectionHead'>"));
            Label expandDepthHeader = new Label();
            expandDepthHeader.Text = "Initial expansion depth:";
            expandDepthHeader.ToolTip = "The depth to display when the Tree View is initially displayed.";
            this.Controls.Add(expandDepthHeader);
            this.Controls.Add(new LiteralControl("</div><div class='UserSectionBody'><div class='UserControlGroup'>"));
            this.Controls.Add(_expandDepth);
            this.Controls.Add(new LiteralControl("</div><div class='UserControlGroup'>"));
            Label expandDepthAdditional = new Label();
            expandDepthAdditional.Text = "Default is 0 (fully closed). Set to -1 for full expansion.";
            this.Controls.Add(expandDepthAdditional);
            this.Controls.Add(new LiteralControl("</div></div><div style='width:100%' class='UserDottedLine'></div>"));

            // show lines checkbox
            this.Controls.Add(new LiteralControl("<div class='UserSectionHead'>"));
            Label showLinesHeader = new Label();
            showLinesHeader.Text = "Show lines:";
            showLinesHeader.ToolTip = "Show lines connecting the nodes of the tree view.";
            this.Controls.Add(showLinesHeader);
            this.Controls.Add(new LiteralControl("</div><div class='UserSectionBody'><div class='UserControlGroup'>"));
            this.Controls.Add(_showLines);
            this.Controls.Add(new LiteralControl("</div></div><div style='width:100%' class='UserDottedLine'></div>"));
        }
        public override bool ApplyChanges()
        {
            LibraryTree parent = this.WebPartToEdit as LibraryTree;
            parent.LibraryName = _libraryName.Text.Trim();
            try
            {
                parent.ExpandDepth = int.Parse(_expandDepth.Text);
            }
            catch
            {
                parent.ExpandDepth = 0;
            }
            parent.ShowLines = _showLines.Checked;
            parent.SaveChanges();
            return true;
        }

        public override void SyncChanges()
        {
            LibraryTree parent = this.WebPartToEdit as LibraryTree;
            _libraryName.Text = parent.LibraryName;
            _expandDepth.Text = parent.ExpandDepth.ToString();
            _showLines.Checked = parent.ShowLines;
        }
    }
}
