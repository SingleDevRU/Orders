using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using System.ComponentModel;
using Xamarin.Essentials;

namespace Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderPage : ContentPage
    {

        public OrderPage()
        {
            InitializeComponent();
           
            FillClientList();

        }

        protected override void OnAppearing()
        {

            FillClientData();

            FillExecutorName();

            FillOrderTableRows();

            base.OnAppearing();
        }

        protected override bool OnBackButtonPressed()
        {
            DeleteNotSavedRow();
            return base.OnBackButtonPressed();
        }

        private void FillOrderTableRows()
        {
            OrderRows.ItemsSource = App.OrdersDataBase.GetRows(int.Parse(OrderNumber.Text));
        }

        private void FillExecutorName()
        {
            if (string.IsNullOrEmpty(Executor.Text))
            {
                Executor.Text = Preferences.Get("User", "<Не установлен>");
            }
        }

        private void FillClientData()
        {
            if ((ClientList.SelectedItem != null) && (!string.IsNullOrEmpty(ClientList.SelectedItem.ToString())))
            {
                string Code = ClientList.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries)[0];
                Client curentClient = App.OrdersDataBase.GetClientByCode(Code);
                ClientInn.Text = curentClient.Inn;
                ClientEmail.Text = curentClient.Email;
                Phone.Text = curentClient.PhoneNumber;
            }
        }

        private void FillClientList()
        {
            var clients = App.OrdersDataBase.GetClients();
            foreach (var client in clients)
            {
                ClientList.Items.Add(client.Code + ": " + client.Name);
            }
        }

        private void SelectedClient(object sender, EventArgs e)
        {
            string Code = ClientList.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries)[0];
            Client curentClient = App.OrdersDataBase.GetClientByCode(Code);
            ClientInn.Text = curentClient.Inn;
            ClientEmail.Text = curentClient.Email;
            Phone.Text = curentClient.PhoneNumber;

        }

        private async void SaveOrder(object sender, EventArgs e)
        {
            Preservation();
            await Navigation.PopModalAsync();

        }

        private void OpenClient(object sender, EventArgs e)
        {
            if ((ClientList.SelectedItem != null) && (!string.IsNullOrEmpty((string)ClientList.SelectedItem)))
            {
                string Code = ClientList.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries)[0];
                Client currentClient = App.OrdersDataBase.GetClientByCode(Code);
                ClientPage page = new ClientPage
                {
                    BindingContext = currentClient
                };
                Navigation.PushModalAsync(page);
            }
        }

        private void AddClient(object sender, EventArgs e)
        {
            Client newclient = new Client
            {
                Code = CodeGenerator.GetUIDForClient(),
                Name = "",
                Inn = "",
                Email = "",
                PhoneNumber = ""
            };
            ClientPage page = new ClientPage
            {
                BindingContext = newclient
            };

            Navigation.PushModalAsync(page);

        }

        private async void DeleteOrder(object sender, EventArgs e) 
        {
            bool Answer = await DisplayAlert("Внимание! Вопрос!", "Удалить данные?", "Да", "Нет");
            if (Answer)
            {
                Order order = (Order)BindingContext;
                App.OrdersDataBase.DeleteOrder(order.Id, int.Parse(OrderNumber.Text));
                await Navigation.PopModalAsync();
            }
        }

        private async void Preservation()
        {
            Order order = (Order)BindingContext;
            order.Executor = Executor.Text;
            order.Client = (string)ClientList.SelectedItem;
            order.isLoaded = false;
            if (string.IsNullOrEmpty(order.Client) || order.Executor == "<Не установлен>")
            {
                await DisplayAlert("Документ не сохранён!", "Не выбран клиент или не установлен пользователь!", "ОК");
                return;
            }

            var OrderRows = App.OrdersDataBase.GetRows(int.Parse(OrderNumber.Text));
            foreach (var row in OrderRows)
            {
                if (!row.isSaved)
                {

                    row.isSaved = true;
                    App.OrdersDataBase.SaveRow(row);
                }
            }
            App.OrdersDataBase.SaveOrder(order);
            
        }

        private async void Cancel(object sender, EventArgs e) 
        {
            DeleteNotSavedRow();
            await Navigation.PopModalAsync();
        }


        private void DeleteNotSavedRow()
        {
            var OrderRows = App.OrdersDataBase.GetRows(int.Parse(OrderNumber.Text));
            foreach (var row in OrderRows)
            {
                if (!row.isSaved)
                {
                    App.OrdersDataBase.DeleteRow(row.Id, row.ParentNumber);
                }
            }
        }

        private async void AddRow(object sender, EventArgs e)
        {
            OrderTableRow row = new OrderTableRow
            {
                ParentNumber = int.Parse(OrderNumber.Text),
                Number = App.OrdersDataBase.GetLastRow(int.Parse(OrderNumber.Text)) + 1,
                isSaved = false,
                Equipment = "",
                Technic = "",
                Malfunction = ""
            };

            OrderTableRowPage page = new OrderTableRowPage
            {
                BindingContext = row
            };
 
            await Navigation.PushModalAsync(page);

        }

        private async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            OrderTableRow SelectedRow = (OrderTableRow)e.SelectedItem;
            OrderTableRowPage page = new OrderTableRowPage
            {
                BindingContext = SelectedRow
            };
            await Navigation.PushModalAsync(page);
        }
    }
}