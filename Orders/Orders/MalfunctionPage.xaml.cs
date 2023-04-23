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
	public partial class MalfunctionPage : ContentPage
	{
		public MalfunctionPage ()
		{
			InitializeComponent ();
		}

		private async void SaveMalfunction(object sender, EventArgs e)
		{
			Malfunction malfunction = (Malfunction)BindingContext;

			if (string.IsNullOrEmpty(malfunction.Name))
			{
				await DisplayAlert("Неисправность не сохранена!", "Не заполнено описание!", "ОК");
				return;
			}
            App.OrdersDataBase.SaveMalfunction(malfunction);

			AddMalfunctionInMalfunctionsLists(malfunction);

			await Navigation.PopModalAsync();
		}

		private async void AddMalfunctionInMalfunctionsLists(Malfunction malfunction)
		{
            try
            {
                OrderTableRowPage TabRowPage = (OrderTableRowPage)Application.Current.MainPage.Navigation.ModalStack[1];
                TabRowPage.Malfunctions.Items.Add(malfunction.Name);
                TabRowPage.Malfunctions.SelectedItem = malfunction.Name;

            }
            catch (Exception ex)
			{
				await DisplayAlert("Ошибка!", ex.ToString(), "ок");
			}
        }

		private async void DeleteMalfunction(object sender, EventArgs e)
		{
            bool Answer = await DisplayAlert("Внимание! Вопрос!", "Удалить данные?", "Да", "Нет");
			if (Answer)
			{
				Malfunction malfunction = (Malfunction)BindingContext;
				App.OrdersDataBase.DeleteMalfunction(malfunction.Id);
				await Navigation.PopModalAsync();
			}
        }

		private void Cancel(object sender, EventArgs e)
		{
			Navigation.PopModalAsync();
		}

    }
}