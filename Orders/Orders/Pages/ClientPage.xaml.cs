using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orders.Tables;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ClientPage : ContentPage
	{
		public ClientPage ()
		{
			InitializeComponent ();

		}

		private async void SaveClient(object sender, EventArgs e)
		{
			Client client = (Client)BindingContext;
			client.Title = client.Name.ToUpper();
			if (string.IsNullOrEmpty(client.Name) || string.IsNullOrEmpty(client.Inn))
			{
				await DisplayAlert("Клиент не сохранён!", "Не заполнено наименование или ИНН!", "ОК");
				return;
			}
            client.Title = client.Name.ToUpper();
            try
			{
				App.OrdersDataBase.SaveClient(client);
			}
			catch(Exception ex)
			{
				await DisplayAlert("Ошибка сохранения!", ex.ToString(), "ОК");
			}

			AddClientInClientsLists(client);

            await Navigation.PopModalAsync();
		}

		private void AddClientInClientsLists(Client client)
		{
            var ModStack = Application.Current.MainPage.Navigation.ModalStack;
            foreach (var page in ModStack)
            {
                if (page is OrderPage OrdPage)
                {
					OrdPage.ClientCode.Text = client.Code;
                    //if (!OrdPage.ClientList.Items.Contains(client.Code + ": " + client.Name))
                    //{
                    //    OrdPage.ClientList.Items.Add(client.Code + ": " + client.Name);
                    //    OrdPage.ClientList.SelectedItem = client.Code + ": " + client.Name;
                    //}
					//if (((List<Client>)OrdPage.ClientList.ItemsSource).FindIndex(element => element.Inn == client.Inn ) == -1)
					//{
					//	OrdPage.ClientCode.Text = client.Code;
					//	OrdPage.ClientList.ItemsSource.Add(client);
					//	OrdPage.ClientList.SelectedIndex = ((List<Client>)OrdPage.ClientList.ItemsSource).FindIndex(element => element.Inn == client.Inn);
					//}
                }
                else if (page is TechniquePage TechPage)
                {
					TechPage.ClientCode.Text = client.Code;
                    //if (!TechPage.Client.Items.Contains(client.Code + ": " + client.Name))
                    //{
                    //    TechPage.Client.Items.Add(client.Code + ": " + client.Name);
                    //    TechPage.Client.SelectedItem = client.Code + ": " + client.Name;
                    //}
                }
            }
        }

		private async void DeleteClient(object sender, EventArgs e)
		{
            bool Answer = await DisplayAlert("Внимание! Вопрос!", "Удалить данные?", "Да", "Нет");
			if(Answer)
			{
				var client = (Client)BindingContext;
				App.OrdersDataBase.DeleteClient(client.Id);
				await Navigation.PopModalAsync();
			}

		}

        private void Cancel(object sender, EventArgs e) 
		{ 
			Navigation.PopModalAsync();
		}

    }
}