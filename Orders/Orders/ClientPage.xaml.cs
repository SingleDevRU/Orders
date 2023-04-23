using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			if (string.IsNullOrEmpty(client.Name) || string.IsNullOrEmpty(client.Inn))
			{
				await DisplayAlert("Клиент не сохранён!", "Не заполнено наименование или ИНН!", "ОК");
				return;
			}

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
                    if (!OrdPage.ClientList.Items.Contains(client.Name + ";;" + client.Inn))
                    {
                        OrdPage.ClientList.Items.Add(client.Name + ";;" + client.Inn);
                        OrdPage.ClientList.SelectedItem = client.Name + ";;" + client.Inn;

                    }
                }
                else if (page is TechniquePage TechPage)
                {
                    if (!TechPage.Client.Items.Contains(client.Name + ";;" + client.Inn))
                    {
                        TechPage.Client.Items.Add(client.Name + ";;" + client.Inn);
                        TechPage.Client.SelectedItem = client.Name + ";;" + client.Inn;
                    }
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