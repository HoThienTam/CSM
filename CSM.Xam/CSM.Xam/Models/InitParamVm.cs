using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using AutoMapper;
using CSM.EFCore;

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
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Menu, VisualMenuModel>();
            });
            Mapper = config.CreateMapper();
        }

        public INavigationService NavigationService { get; private set; }
        public IPageDialogService PageDialogService { get; private set; }
        public IMapper Mapper { get; private set; }

    }
}
