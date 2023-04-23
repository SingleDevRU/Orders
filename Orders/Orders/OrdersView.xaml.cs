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

namespace Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersView : ContentPage
    {
        // readonly string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        readonly string folderPath = FileSystem.CacheDirectory;
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

        private async void DownloadFromFTP(object sender, EventArgs e)
        {
            
            await DisplayAlert("Download", DependencyService.Get<IFtpWebRequest>().Download("ftp://192.168.0.107/1cToOrders.xml", Path.Combine(folderPath, "1cToOrders.xml"), "FTPUser", "FTP333x333x"), "ok");

        }

        private async void SaveXML(object sender, EventArgs e)
        {

            string[] setings = { Preferences.Get("FTPAdress", ""),
                                 Preferences.Get("FTPUser", ""),
                                 Preferences.Get("FTPPass", ""),
                                 Preferences.Get("Prefix", "")
                               };

            foreach (string seting in setings)
            {
                if (string.IsNullOrEmpty(seting))
                {
                    await DisplayAlert("Файл не выгружен!", "Не заполнены настройки!", "ОК");
                    return;
                }
            }

            XML.CreateXML(folderPath, $"OrdersTo1C_{setings[3]}.xml");

            FileList.ItemsSource = Directory.GetFiles(folderPath).Select(f => Path.GetFileName(f));

            await DisplayAlert("Результат выгрузки", DependencyService.Get<IFtpWebRequest>().Upload(setings[0], Path.Combine(folderPath, $"OrdersTo1C_{Preferences.Get("Prefix","No_prefix")}.xml"), setings[1], setings[2]),"ОК");

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