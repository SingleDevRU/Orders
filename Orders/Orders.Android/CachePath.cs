using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Security;
using Orders.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: Xamarin.Forms.Dependency(typeof(CachePath))]
namespace Orders.Droid
{
    class CachePath: ICachePath
    {
        public CachePath() { }
        public string GetCachePath()
        {
            string path = Android.App.Application.Context.GetExternalCacheDirs()[0].AbsolutePath;
            return path;
        }
    }
}