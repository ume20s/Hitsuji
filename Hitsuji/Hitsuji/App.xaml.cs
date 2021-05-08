using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Hitsuji
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            // スマホのスリープモードをオンにする
            DependencyService.Get<IDeviceService>().EnableSleep();

            // プログラムの終了
            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
        }

        protected override void OnResume()
        {
        }
    }
}
