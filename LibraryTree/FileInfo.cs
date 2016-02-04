using Microsoft.SharePoint;
using System.Web.UI.WebControls;

namespace LibraryTree
{
    //This class will store files information for use in  tree view
    public class FileInfo
    {      
        private string _Name;
        public string Name {get{return _Name;}set{_Name = value;}}

        private long _Size;
        public long Size {get{return _Size;}set{_Size = value;}}

        private string _URL;
        public string URL {get{return _URL;}set{_URL = value;}}

        private string _IconURL;
        public string IconURL {get{return _IconURL;}set{_IconURL = value;}}

        private SPFile _File;
        public SPFile File{get{return _File;}set{_File = value;}}
    }
    //We will use this class for sorting FileInfo classes.
    public class FileInfoComparer : System.Collections.Generic.IComparer<FileInfo>      
    {
        private SortDirection m_direction = SortDirection.Ascending;
        public FileInfoComparer()
            : base(){}

        public FileInfoComparer(SortDirection direction)
        {
            m_direction = direction;
        }

        int System.Collections.Generic.IComparer<FileInfo>.Compare(FileInfo x, FileInfo y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            else if (x == null && y != null)
            {
                return (m_direction == SortDirection.Ascending) ? -1 : 1;
            }
            else if (x != null && y == null)
            {
                return (m_direction == SortDirection.Ascending) ? 1 : -1;
            }
            else
            {
                return
                    (m_direction == SortDirection.Ascending)
                        ? x.Name.CompareTo(y.Name)
                        : y.Name.CompareTo(x.Name);
            }
        }       
    }
}
