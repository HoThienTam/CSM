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
                cfg.CreateMap<Category, VisualCategoryModel>();
                cfg.CreateMap<Employee, VisualEmployeeModel>();
                cfg.CreateMap<Menu, VisualMenuModel>();
                cfg.CreateMap<Table, VisualTableModel>();
                cfg.CreateMap<Zone, VisualZoneModel>();

                cfg.CreateMap<Item, VisualItemMenuModel>()
                .ForMember(destination => destination.Name,
               opts => opts.MapFrom(source => source.ItemName))
                .ForMember(destination => destination.Value,
               opts => opts.MapFrom(source => source.Price));

                cfg.CreateMap<Discount, VisualItemMenuModel>()
                .ForMember(destination => destination.Name,
               opts => opts.MapFrom(source => source.DiscountName))
                .ForMember(destination => destination.Value,
               opts => opts.MapFrom(source => source.DiscountValue))
                .AfterMap((source, destination) => destination.IsDiscount = true);
            });
            Mapper = config.CreateMapper();
        }

        public INavigationService NavigationService { get; private set; }
        public IPageDialogService PageDialogService { get; private set; }
        public IMapper Mapper { get; private set; }

    }
}
