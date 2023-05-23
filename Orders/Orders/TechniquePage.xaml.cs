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
	public partial class TechniquePage : ContentPage
	{
        public TechniquePage ()
		{
			InitializeComponent ();
            FillClientsList();
        }

        private void FillClientsList()
        {
            var clients = App.OrdersDataBase.GetClients();
            foreach (var client in clients)
            {
                Client.Items.Add(client.Code + ": " + client.Name);
            }
        }

        private void AddClient(object sender, EventArgs e)
        {
            Client newclient = new Client
            {
                Code = CodeGenerator.GetUIDForClient(),
                Email = "",
                Inn = "",
                Name = "",
                PhoneNumber = ""
            };

            ClientPage page = new ClientPage
            {
                BindingContext = newclient
            };

            Navigation.PushModalAsync(page);
        }

        private async void SaveTechnique(object sender, EventArgs e)
        {
            Technique technique = (Technique)BindingContext;
			technique.Parent = (string)Client.SelectedItem;
			
            if(string.IsNullOrEmpty(technique.Name))
			{
                await DisplayAlert("Техника не сохранена!", "Не заполнено наименование!", "ОК");
                return;
			}
            
            App.OrdersDataBase.SaveTechnique(technique);

            if (Application.Current.MainPage.Navigation.ModalStack[0] is OrderPage)
            {
                AddTechniqueInTechniquesList(technique);
            }

            await Navigation.PopModalAsync();
        }

        private async void AddTechniqueInTechniquesList(Technique technique)
        {
            try
            {
                OrderPage ordPage = (OrderPage)Application.Current.MainPage.Navigation.ModalStack[0];              
                OrderTableRowPage OrdTablPage = (OrderTableRowPage)Application.Current.MainPage.Navigation.ModalStack[1];
                if ((string)ordPage.ClientList.SelectedItem == (string)Client.SelectedItem)
                {
                    string AddSerial = technique.SerialKey.Length > 0 ? ": " + technique.SerialKey : "";
                    OrdTablPage.Techniques.Items.Add(technique.Code + ": " + technique.Name + AddSerial);
                    OrdTablPage.Techniques.SelectedItem = technique.Code + ": " + technique.Name + AddSerial;
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Ошибка!", ex.ToString(), "ок");
            }
        }

		private async void DeleteTechnique(object sender, EventArgs e)
		{
            bool Answer = await DisplayAlert("Внимание! Вопрос!", "Удалить данные?", "Да", "Нет");
            if(Answer)
            {
                Technique technique = (Technique)BindingContext;
                App.OrdersDataBase.DeleteTecnique(technique.Id);
                await Navigation.PopModalAsync();
            }
		}

        private void Cancel(object sender, EventArgs e) 
		{
			Navigation.PopModalAsync();
		}
    }
}

