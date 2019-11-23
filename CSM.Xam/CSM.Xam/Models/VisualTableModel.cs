using CSM.Logic.Enums;
using Prism.Mvvm;

namespace CSM.Xam.Models
{
    public class VisualTableModel : BindableBase
    {
        #region Id
        private string _Id;
        public string Id
        {
            get { return _Id; }
            set { SetProperty(ref _Id, value); }
        }
        #endregion

        #region TableName
        private string _TableName;
        public string TableName
        {
            get { return _TableName; }
            set { SetProperty(ref _TableName, value); }
        }
        #endregion

        #region TableSize
        private long? _TableSize;
        public long? TableSize
        {
            get { return _TableSize; }
            set { SetProperty(ref _TableSize, value); }
        }
        #endregion

        #region TableType
        private long? _TableType;
        public long? TableType
        {
            get { return _TableType; }
            set { SetProperty(ref _TableType, value); }
        }
        #endregion

        #region FkZone
        private string _FkZone;
        public string FkZone
        {
            get { return _FkZone; }
            set { SetProperty(ref _FkZone, value); }
        }
        #endregion

        #region Status
        private Status _Status;
        public Status Status
        {
            get { return _Status; }
            set { SetProperty(ref _Status, value); }
        }
        #endregion

    }
}
