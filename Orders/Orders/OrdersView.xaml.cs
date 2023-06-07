using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using Xamarin.Essentials;
using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms.PlatformConfiguration;

namespace Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersView : ContentPage
    {
        readonly string folderPath = DependencyService.Get<ICachePath>().GetCachePath();
        public OrdersView()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            OrdersList.ItemsSource = App.OrdersDataBase.GetOrders();
            base.OnAppearing();
        }
        private async void OnSelected(object sender, SelectedItemChangedEventArgs e) 
        {
            Order SelectedOrder = (Order)e.SelectedItem;
            OrderPage orderPage = new OrderPage
            {
                BindingContext = SelectedOrder
            };
            await Navigation.PushModalAsync(orderPage);
            
        }
        private async void SaveXML(object sender, EventArgs e)
        {
            Settings settings = new Settings
            {
                FTPAdress = Preferences.Get("FTPAdress", ""),
                FTPPassword = Preferences.Get("FTPPass", ""),
                FTPUser = Preferences.Get("FTPUser", ""),
                Prefix = Preferences.Get("Prefix", "")
            };

            if(!settings.CheckSettings())
            {
                await DisplayAlert("Файл не выгружен!", "Не заполнены настройки!", "ОК");
                return;
            }
            UploadStatus.IsVisible = true;
            if(!await CreateXmlForUpload(settings.Prefix)) return;
            if(!await UploadToFTP(settings.FTPAdress, settings.FTPUser, settings.FTPPassword, settings.Prefix)) return;
            UploadStatus.IsVisible = false;
            UploadProgress.Progress = 0;
            await DisplayAlert("Готово!", "Выгрузка завершена!", "ОК");
        }
        private async Task<bool> CreateXmlForUpload(string prefix)
        {
            string CreatingStatus = await XML.StartUploadXML(folderPath, $"OrdersTo1C_{prefix}.xml");
            if( CreatingStatus != "Файл создан")
            {
                await DisplayAlert("Ошибка создания файла!", CreatingStatus, "ОК");
                UploadStatus.IsVisible = false;
                UploadProgress.Progress = 0;
                return false;
            }
            await UploadProgress.ProgressTo(0.2, 500, Easing.Linear);
            return true;
        }
        private async Task<bool> UploadToFTP(string FTPAdress, string FTPUser, string FTPPassword, string prefix)
        {
            string FTPStatus = await DependencyService.Get<IFtpWebRequest>().Upload(FTPAdress, Path.Combine(folderPath, $"OrdersTo1C_{prefix}.xml"), FTPUser, FTPPassword);
            if(FTPStatus != "Выгрузка завершена")
            {
                await DisplayAlert("Ошибка FTP соединения!", FTPStatus, "ОК");
                UploadStatus.IsVisible = false;
                UploadProgress.Progress = 0;
                return false;
            }
            await UploadProgress.ProgressTo(1, 500, Easing.Linear);
            return true;
        }
        private async void CreateOrder(object sender, EventArgs e)
        {
            Order order = new Order
            {
                Code = "",
                Number = App.OrdersDataBase.GetLast() + 1,
                Date = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                SendMail = false,
                Executor = "",
                Client = "",
                Comment = ""
            };

            OrderPage orderPage = new OrderPage
            {
                BindingContext = order
            };

            await Navigation.PushModalAsync(orderPage);
        }
    }
}