using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CSM.Xam.Models
{
    public class InitParamVm
    {
        public InitParamVm(
            INavigationService navigationService,
            IPageDialogService pageDialogService)
        {
            NavigationService = navigationService;
            PageDialogService = pageDialogService;
        }

        public INavigationService NavigationService { get; private set; }
        public IPageDialogService PageDialogService { get; private set; }

    }
}
