using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Xamarin.Forms;
using Hitsuji.Droid.Services;

[assembly: Dependency(typeof(DeviceService))]
namespace Hitsuji.Droid.Services
{
    class DeviceService : IDeviceService
    {
        private static PowerManager.WakeLock _wakeLock = null;

        // スリープを無効にする
        public void DisableSleep()
        {
            PowerManager pm = (PowerManager)Forms.Context.GetSystemService(Context.PowerService);
            Context context = Forms.Context;    //Android.App.Application.Context;
            var packageName = context.PackageManager.GetPackageInfo(context.PackageName, 0).PackageName;
            _wakeLock = pm.NewWakeLock(WakeLockFlags.Full, packageName);
            _wakeLock.Acquire();

        }

        // スリープを有効にする
        public void EnableSleep()
        {
            if (_wakeLock != null) {
                _wakeLock.Release();
                _wakeLock = null;
            }
        }
    }
}