using Prism;
using Prism.Ioc;
using CSM.Xam.ViewModels;
using CSM.Xam.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CSM.Xam.Models;
using System.IO;
using System;
using System.Reflection;
using System.Threading;
using System.Globalization;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CSM.Xam
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        public string DbConnectionString { get; set; } = "";

        protected override async void OnInitialized()
        {
            InitializeComponent();

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("vi-VN");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("vi-VN");

            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "data.db");
            DbConnectionString = $"Data Source={dbPath};";

            File.Delete(dbPath);
            if (File.Exists(dbPath) == false)
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
                Stream stream = assembly.GetManifestResourceStream("CSM.Xam.Files.data.db");

                using (var reader = new MemoryStream())
                {
                    await stream.CopyToAsync(reader);
                    File.WriteAllBytes(dbPath, reader.GetBuffer());
                }
            }

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<InitParamVm>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>(); //Trang chu
            containerRegistry.RegisterForNavigation<CSM_01Page, CSM_01PageViewModel>(); // Trang dang nhap
            containerRegistry.RegisterForNavigation<CSM_02Page, CSM_02PageViewModel>(); // Trang tao mat hang
            containerRegistry.RegisterForNavigation<CSM_02_01Page, CSM_02_01PageViewModel>(); // Trang danh muc
            containerRegistry.RegisterForNavigation<CSM_03Page>(); // Trang tao giam gia
            containerRegistry.RegisterForNavigation<CSM_04Page>(); // Trang hoat dong
            containerRegistry.RegisterForNavigation<CSM_05Page>(); // Trang thong ke
            containerRegistry.RegisterForNavigation<CSM_06Page>(); // Trang hang hoa
            containerRegistry.RegisterForNavigation<CSM_07Page, CSM_07PageViewModel>(); // Trang nhan vien
            containerRegistry.RegisterForNavigation<CSM_07_01Page, CSM_07_01PageViewModel>(); // Trang tao nhan vien
            containerRegistry.RegisterForNavigation<CSM_08Page, CSM_08PageViewModel>(); // Trang cai dat
            containerRegistry.RegisterForNavigation<CSM_08_01Page, CSM_08_01PageViewModel>(); // Trang tao khu vuc
            containerRegistry.RegisterForNavigation<CSM_08_02Page, CSM_08_02PageViewModel>(); // Trang tao ban
            containerRegistry.RegisterForNavigation<CSM_09Page>(); // Trang phien lam viec
            containerRegistry.RegisterForNavigation<CSM_10Page, CSM_10PageViewModel>(); // Trang thanh toan
            containerRegistry.RegisterForNavigation<CSM_11Page, CSM_11PageViewModel>(); // Trang danh sach item
            containerRegistry.RegisterForNavigation<CSM_12Page, CSM_12PageViewModel>();// Trang chinh sua menu
        }
    }
}
