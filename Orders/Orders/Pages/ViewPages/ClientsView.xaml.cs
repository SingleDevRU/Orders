using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orders.Core;
using Orders.Tables;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Orders.Models;

namespace Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ClientsView : ContentPage
	{
		public ClientsView ()
		{
            InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            
            FillClientList();

            base.OnAppearing();
        }

		private void FillClientList()
		{
            //ClientsList.ItemsSource = App.OrdersDataBase.GetClients();
			BindingContext = new ClientModel
			{
				ClientsList = App.OrdersDataBase.GetClients()
			};
        }

		private async void OnSelected(object sender, SelectedItemChangedEventArgs e)
		{
			Client SelectedClient = (Client)e.SelectedItem;
			ClientPage page = new ClientPage
			{
				BindingContext = SelectedClient
			};

			await Navigation.PushModalAsync(page);
		}


        private async void CreateClient(object sender, EventArgs e)
		{
			Client client = new Client
			{
				Code = CodeGenerator.GetUIDForClient(),
				Email = "",
				Inn = "",
				Name = "",
				PhoneNumber = ""
			};
			ClientPage page = new ClientPage
			{
				BindingContext = client
			};

			await Navigation.PushModalAsync(page);
			
		}
    }
}