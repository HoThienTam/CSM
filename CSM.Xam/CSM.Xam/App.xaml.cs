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

            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "data.db");
            DbConnectionString = $"Data Source={dbPath};";
            //File.Delete(dbPath);
            if (File.Exists(dbPath) == false)
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(App)).Assembly;
                Stream stream = assembly.GetManifestResourceStream("CSM.Xam.Files.data.db");

                using (var reader = new System.IO.MemoryStream())
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
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<CSM_08Page, CSM_08PageViewModel>();
            containerRegistry.RegisterForNavigation<CSM_08_01Page, CSM_08_01PageViewModel>();
            containerRegistry.RegisterForNavigation<CSM_10Page, CSM_10PageViewModel>();
        }
    }
}
