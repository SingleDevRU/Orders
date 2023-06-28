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
using System.Xml.Linq;
using Orders.Tables;
using Orders.Core;
using Orders.Models;
using Orders.Pages.ViewPages;

namespace Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderPage : ContentPage
    {

        private bool isChanged = false;
        public OrderPage()
        {
            InitializeComponent();
           
            //FillClientList();

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
            //OrderRows.ItemsSource = App.OrdersDataBase.GetRows(int.Parse(OrderNumber.Text));
            OrderRows.BindingContext = new OrderTableRowModel
            {
                OrderTableRowList = App.OrdersDataBase.GetRows(int.Parse(OrderNumber.Text))
            };
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
            //Order order = (Order)BindingContext;

            if (!string.IsNullOrEmpty(ClientCode.Text))
            {

                //string Code = ClientList.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries)[0];
                Client curentClient = App.OrdersDataBase.GetClientByCode(ClientCode.Text);
                DataClient.BindingContext = curentClient;
                //int ind = ((List<Client>)ClientList.ItemsSource).FindIndex(client => client.Code == ClientCode.Text);
                //ClientList.SelectedIndex = ((List<Client>)ClientList.ItemsSource).FindIndex(client => client.Code == order.ClientCode);
                //ClientList.SelectedIndex = 0;
                //ClientList.SelectedItem = ClientList.ItemsSource[ind];
                //Client curentClient = (Client)ClientList.SelectedItem;
                //Inn.Text = curentClient.Inn;
                //ClientEmail.Text = curentClient.Email;
                //Phone.Text = curentClient.PhoneNumber;
                //DataClient.BindingContext = curentClient;
            }
        }

        private void FillClientList()
        {
            //var clients = App.OrdersDataBase.GetClients();
            //foreach (var client in clients)
            //{
            //    ClientList.Items.Add(client.Code + ": " + client.Name);
            //}
            //ClientList.BindingContext = new ClientModel
            //{
            //    ClientsList = App.OrdersDataBase.GetClients(),
            //};
        }

        //private void SelectedClient(object sender, EventArgs e)
        //{
        //    //if (ClientList.SelectedItem == null) return;
        //    //string Code = ClientList.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries)[0];
        //    //Client curentClient = App.OrdersDataBase.GetClientByCode(Code);
        //    //Inn.Text = curentClient.Inn;
        //    //ClientEmail.Text = curentClient.Email;
        //    //Phone.Text = curentClient.PhoneNumber;
        //    Client client = (Client)((Picker)sender).SelectedItem;
        //    //DataClient.BindingContext = client;
        //    //Inn.Text = client.Inn;
        //    //ClientEmail.Text = client.Email;
        //    //Phone.Text = client.PhoneNumber;
        //    ClientCode.Text = client.Code;

        //}

        private async void SaveOrder(object sender, EventArgs e)
        {
            Preservation();
            await Navigation.PopModalAsync();

        }

        private void OpenClient(object sender, EventArgs e)
        {
            //if (ClientList.SelectedItem != null)
            if (!string.IsNullOrEmpty(ClientCode.Text))
            {
                //string Code = ClientList.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries)[0];
                //Client currentClient = App.OrdersDataBase.GetClientByCode(Code);
                //Client currentClient = (Client)ClientList.SelectedItem;
                Client currentClient = (Client)DataClient.BindingContext;
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
                PhoneNumber = "",
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
            order.ClientCode = ClientCode.Text;
            //order.Client = ((Client)ClientList.SelectedItem).Name;
            order.Client = SelectedClient.Text;
            order.Comment = Comment.Text ?? "";
            if (string.IsNullOrEmpty(order.Client) || order.Executor == "<Не установлен>")
            {
                await DisplayAlert("Документ не сохранён!", "Не выбран клиент или не установлен пользователь!", "ОК");
                return;
            }

            OrderChanged(order);
            SaveNotSavedRow();
            if(isChanged)
            {
                order.isLoaded = false;
                order.isChanged = true;
            }

            App.OrdersDataBase.SaveOrder(order);
            if(string.IsNullOrEmpty(order.Code))
            {
                order.Code = CodeGenerator.GetCodeForOrder(order);
                App.OrdersDataBase.UpdateOrder(order);
            }            
        }
        private void SaveNotSavedRow()
        {
            var OrderRows = App.OrdersDataBase.GetRows(int.Parse(OrderNumber.Text));
            foreach (var row in OrderRows)
            {
                if (!row.isSaved)
                {
                    row.isSaved = true;
                    isChanged = true;
                    App.OrdersDataBase.SaveRow(row);
                }
            }
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

        //private void EnableSearch(object sender, CheckedChangedEventArgs e)
        //{
        //    if(NameForSearch.IsVisible)
        //    { 
        //        NameForSearch.IsVisible = false; 
        //        NameForSearch.Text = string.Empty;
        //        string SelectedClient = (string)ClientList.SelectedItem;
        //        FillClientList();
        //        ClientList.SelectedItem = SelectedClient;
        //    }
        //    else 
        //    { 
        //        NameForSearch.IsVisible = true; 
        //    }
        //}

        //private void StartSearch(object sender, FocusEventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(((Entry)sender).Text))
        //    {
        //        ClientList.Items.Clear();
        //        ClientList.SelectedItem = null;
        //        FillClientListForSearch(((Entry)sender).Text);
        //    }
        //}

        //private void FillClientListForSearch(string PartName)
        //{            
        //    var clients = App.OrdersDataBase.GetClientsByPartName(PartName);
        //    foreach (var client in clients)
        //    {
        //        ClientList.Items.Add(client.Code + ": " + client.Name);
        //    }
        //}

        private void OrderChanged(Order order)
        {
            if (order.Id == 0) return;
            Order OldData = App.OrdersDataBase.GetOrder(order.Id);
            isChanged = ((OldData.Client != order.Client) ||
                (OldData.SendMail != order.SendMail) ||
                (OldData.Comment != order.Comment));
        }

        private async void SearchClient(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ClientSearchView());
        }
    }
}