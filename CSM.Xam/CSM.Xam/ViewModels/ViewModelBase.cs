using CSM.Xam.Models;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CSM.Xam.ViewModels
{
    public class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible
    {
        protected INavigationService NavigationService { get; private set; }
        protected IPageDialogService PageDialogService { get; private set; }
        protected IEventAggregator EventAggregator { get; private set; }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #region IsBusy
        private bool _IsBusy = false;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set
            {
                if (SetProperty(ref _IsBusy, value))
                {
                    RaisePropertyChanged(nameof(IsNotBusy));
                }
            }
        }
        #endregion

        public bool IsNotBusy { get { return !_IsBusy; } }
        private ViewModelBase() : base()
        {
            Type thisType = this.GetType();
            do
            {
                var methods = thisType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                                      .Where(h => h.GetCustomAttributes<InitializeAttribute>().Any())
                                      .ToList();
                foreach (var method in methods)
                {
                    method.Invoke(this, null);
                }

                if (thisType == typeof(BindableBase))
                {
                    break;
                }

                thisType = thisType.BaseType;
            } while (true);
        }

        public ViewModelBase(InitParamVm initParamVm) : this()
        {
            NavigationService = initParamVm.NavigationService;
            PageDialogService = initParamVm.PageDialogService;
            EventAggregator = initParamVm.EventAggregator;
        }

        #region GoBackCommand

        public DelegateCommand<object> GoBackCommand { get; private set; }
        private async void OnGoBack(object obj)
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;
            try
            {
                await NavigationService.GoBackAsync();
            }
            catch (Exception ex)
            {
                await ShowError(ex);
            }
            finally
            {
                IsBusy = false;
            }

        }
        [Initialize]
        private void InitGoBackCommand()
        {
            GoBackCommand = new DelegateCommand<object>(OnGoBack);
            GoBackCommand.ObservesCanExecute(() => IsNotBusy);
        }

        #endregion

        public virtual void Initialize(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {

        }

        public virtual void Destroy()
        {

        }
        protected Task ShowError(Exception ex)
        {
            return PageDialogService.DisplayAlertAsync("Lỗi hệ thống", $"Đã có lỗi trong quá trình xử lý:\n{ex.Message}", "Đóng");
        }
    }
}
