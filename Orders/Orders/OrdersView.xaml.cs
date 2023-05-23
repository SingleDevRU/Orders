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
            OrderPage orderPage = new OrderPage();
            orderPage.BindingContext= SelectedOrder;
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

            XML.CreateXML(folderPath, $"OrdersTo1C_{settings.Prefix}.xml");

            await DisplayAlert("Результат выгрузки", DependencyService.Get<IFtpWebRequest>().Upload(settings.FTPAdress, Path.Combine(folderPath, $"OrdersTo1C_{Preferences.Get("Prefix","No_prefix")}.xml"), settings.FTPUser, settings.FTPPassword),"ОК");

        }

        private async void CreateOrder(object sender, EventArgs e)
        {
            Order order = new Order
            {
                Number = App.OrdersDataBase.GetLast() + 1,
                Date = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                SendMail = false,
                Executor = "",
                Client = ""
            };

            OrderPage orderPage = new OrderPage
            {
                BindingContext = order
            };

            await Navigation.PushModalAsync(orderPage);
        }
    }
}