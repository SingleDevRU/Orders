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
	public partial class KitElementPage : ContentPage
	{
		public KitElementPage ()
		{
			InitializeComponent ();
		}

		private async void SaveKitElement(object sender, EventArgs e)
		{
			KitElement kitElement = (KitElement)BindingContext;
            if (string.IsNullOrEmpty(kitElement.Name))
            {
                await DisplayAlert("Элемент комплекта не сохранен!", "Не заполнено наименование!", "ОК");
                return;
            }
			App.OrdersDataBase.SaveKitElement(kitElement);
            
            AddKitElementInKitElementsLists(kitElement);

			await Navigation.PopModalAsync();
        }

        private async void AddKitElementInKitElementsLists(KitElement kitElement)
        {
            try
            {
                OrderTableRowPage TabRowPage = (OrderTableRowPage)Application.Current.MainPage.Navigation.ModalStack[1];
                TabRowPage.Equipments.Items.Add(kitElement.Name);
                TabRowPage.Equipments.SelectedItem = kitElement.Name;
            }
            catch(Exception ex)
            {
                await DisplayAlert("Ошибка!", ex.ToString(), "ок");  
            }

        }

		private async void DeleteKitElement(object sender, EventArgs e)
		{
            bool Answer = await DisplayAlert("Внимание! Вопрос!", "Удалить данные?", "Да", "Нет");
            if (Answer)
            {
                KitElement kitElement = (KitElement)BindingContext;
                App.OrdersDataBase.DeleteKitElement(kitElement.Id);
                await Navigation.PopModalAsync();
            }
        }

        private void Cancel(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}