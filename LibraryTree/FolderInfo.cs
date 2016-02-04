using System.Web.UI.WebControls;

namespace LibraryTree
{
    public class FolderInfo
    {
        private string _Name;
        public string Name{get{return _Name;}set{_Name = value;}}

        private long _Size;
        public long Size{get{return _Size;}set{_Size = value;}}

        private string _URL;
        public string URL{get{return _URL;}set{_URL = value;}}

        private long _FilesNumber;
        public long FilesNumber{get{return _FilesNumber;}set{_FilesNumber = value;}}
    }


    public class FolderInfoComparer : System.Collections.Generic.IComparer<FolderInfo>
    {
        private SortDirection m_direction = SortDirection.Ascending;

        public FolderInfoComparer()
            : base(){}

        public FolderInfoComparer(SortDirection direction)
        {
            m_direction = direction;
        }

        int System.Collections.Generic.IComparer<FolderInfo>.Compare(FolderInfo x, FolderInfo y)
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
