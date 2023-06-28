using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Orders.Tables;
using Orders.Models;

namespace Orders.Pages.ViewPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientSearchView : ContentPage
    {
        public ClientSearchView()
        {
            InitializeComponent();
            FillClientList();
        }

        private void FillClientList()
        {
            BindingContext = new ClientModel
            {
                ClientsList = App.OrdersDataBase.GetClients()
            };
        }

        private async void ClientsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Client client = (Client)e.SelectedItem;
            var PreviousPage = Application.Current.MainPage.Navigation.ModalStack[0];
            if(PreviousPage is OrderPage page)
            {
                page.ClientCode.Text = client.Code;
            }
            else if(PreviousPage is TechniquePage page1)
            {
                page1.ClientCode.Text = client.Code;
            }
            await Navigation.PopModalAsync();
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            string PartName = ((Entry)sender).Text;
            if (!string.IsNullOrEmpty(PartName))
            {
                BindingContext = new ClientModel
                {
                    ClientsList = App.OrdersDataBase.GetClientsByPartName(PartName)
                };
            }
            else
            {
                BindingContext = new ClientModel
                {
                    ClientsList = App.OrdersDataBase.GetClients()
                };
            }
        }
    }
}