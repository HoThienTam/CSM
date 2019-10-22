using CSM.Xam.Models;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;

namespace CSM.Xam.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(InitParamVm initParamVm) : base(initParamVm)
        {
            ListMenuBindProp = new ObservableCollection<string>();
            ListSectionBindProp = new ObservableCollection<string>();
            ListInvoiceBindProp = new ObservableCollection<string>();
            for (int i = 0; i < 5; i++)
            {
                ListMenuBindProp.Add("A");
                ListSectionBindProp.Add("A");
                ListInvoiceBindProp.Add("A");
            }
        }

        #region ListMenuBindProp
        private ObservableCollection<string> _ListMenuBindProp = null;
        public ObservableCollection<string> ListMenuBindProp
        {
            get { return _ListMenuBindProp; }
            set { SetProperty(ref _ListMenuBindProp, value); }
        }
        #endregion

        #region ListSectionBindProp
        private ObservableCollection<string> _ListSectionBindProp = null;
        public ObservableCollection<string> ListSectionBindProp
        {
            get { return _ListSectionBindProp; }
            set { SetProperty(ref _ListSectionBindProp, value); }
        }
        #endregion

        #region ListInvoiceBindProp
        private ObservableCollection<string> _ListInvoiceBindProp = null;
        public ObservableCollection<string> ListInvoiceBindProp
        {
            get { return _ListInvoiceBindProp; }
            set { SetProperty(ref _ListInvoiceBindProp, value); }
        }
        #endregion

    }
}
