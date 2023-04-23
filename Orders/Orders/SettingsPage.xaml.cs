using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Orders
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage ()
		{
			InitializeComponent ();
			User.Text = Preferences.Get("User", "");
			Prefix.Text = Preferences.Get("Prefix", "");
			FTPAdress.Text = Preferences.Get("FTPAdress", "");
			FTPUser.Text = Preferences.Get("FTPUser", "");
			FTPPass.Text = Preferences.Get("FTPPass", "");
		}

        private async void SaveSettings(object sender, EventArgs e)
        {
			Preferences.Set("User", User.Text);
			Preferences.Set("Prefix",Prefix.Text);
			Preferences.Set("FTPAdress", FTPAdress.Text);
			Preferences.Set("FTPUser", FTPUser.Text);
			Preferences.Set("FTPPass", FTPPass.Text);
			await DisplayAlert("Готово!", "Настройки сохранены.", "ОК");
        }
    }

}