using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSM.Xam.Models
{
    public class VisualEmployeeModel : BindableBase
    {
        #region Id
        private string _Id;
        public string Id
        {
            get { return _Id; }
            set { SetProperty(ref _Id, value); }
        }
        #endregion

        #region EmployeeName
        private string _EmployeeName;
        public string EmployeeName
        {
            get { return _EmployeeName; }
            set { SetProperty(ref _EmployeeName, value); }
        }
        #endregion

        #region FullName
        private string _FullName;
        public string FullName
        {
            get { return _FullName; }
            set { SetProperty(ref _FullName, value); }
        }
        #endregion

        #region Password
        private string _Password;
        public string Password
        {
            get { return _Password; }
            set { SetProperty(ref _Password, value); }
        }
        #endregion

        #region Role
        private long _Role;
        public long Role
        {
            get { return _Role; }
            set { SetProperty(ref _Role, value); }
        }
        #endregion

        #region IsSelected
        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set { SetProperty(ref _IsSelected, value); }
        }
        #endregion

    }
}
