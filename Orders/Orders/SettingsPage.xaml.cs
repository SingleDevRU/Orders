using System;
using System.Collections.Generic;
using System.IO;
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
        readonly string folderPath = DependencyService.Get<ICachePath>().GetCachePath();
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

        private async void DownloadFromFTP(object sender, EventArgs e)
        {
            Settings settings = new Settings
            { 
                FTPAdress = FTPAdress.Text,
                FTPUser = FTPUser.Text,
                Prefix = Prefix.Text,
                FTPPassword = FTPPass.Text
            };            

            if(!settings.CheckSettings())
            {
                await DisplayAlert("Файл не выгружен!", "Не заполнены настройки!", "ОК");
                return;
            }
            
            LoadStatus.IsVisible = true;
            if (!await FTPConection(settings)) return;
            if(!await UploadingData()) return;
            LoadStatus.IsVisible = false;
            ProgressLoad.Progress = 0;
            await DisplayAlert("Готово!", "Загрузка завершена.", "ОК");
        }

        private async Task<bool> FTPConection(Settings settings)
        {
            string FTPAdress = settings.FTPAdress[settings.FTPAdress.Length - 1] == '/' ? settings.FTPAdress + "DataToMobile.xml" : settings.FTPAdress + "/DataToMobile.xml";
            string FTPConectResult = await DependencyService.Get<IFtpWebRequest>().Download(FTPAdress, Path.Combine(folderPath, "DataToMobile.xml"), settings.FTPUser, settings.FTPPassword);

            if (FTPConectResult != "Загрузка завершена")
            {
                LoadStatus.IsVisible = false;
                await DisplayAlert("Ошибка FTP соединения!", FTPConectResult, "ОК");
                return false;
            }
            await ProgressLoad.ProgressTo(0.2, 500, Easing.Linear);
            return true;
        }
        private async Task<bool> UploadingData()
        {
            string DataUploadStatus = await XML.ReadXml(Path.Combine(folderPath, "DataToMobile.xml"));

            if (DataUploadStatus != "Прогрузка данных завершена")
            {
                LoadStatus.IsVisible = false;
                ProgressLoad.Progress = 0;
                await DisplayAlert("Ошибка чтения xml!", DataUploadStatus, "ОК");
                return false;
            }
            await ProgressLoad.ProgressTo(1, 500, Easing.Linear);
            return true;
        }
    }

}