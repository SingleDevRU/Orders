using Orders.Models;
using Orders.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Orders.Pages.ViewPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModelSearchView : ContentPage
	{
		public ModelSearchView ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
			FillModelList();
            base.OnAppearing();
        }

		private void FillModelList()
		{
			BindingContext = new ModelVTModel
            {
                ModelsList = App.OrdersDataBase.GetAllModels()
            };
		}

        private async void ModelsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
			int index = Application.Current.MainPage.Navigation.ModalStack.Count - 1;
			var techniquePage = Application.Current.MainPage.Navigation.ModalStack[index - 1];
			if (techniquePage is TechniquePage page) 
			{
				page.ModelCode.Text = ((Model)e.SelectedItem).Code;
			}
			await Navigation.PopModalAsync();
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            string PartName = ((Entry)sender).Text;
            if (!string.IsNullOrEmpty(PartName))
            {
                BindingContext = new ModelVTModel
                {
                    ModelsList = App.OrdersDataBase.GetModelsByName(PartName)
                };
            }
            else
            {
                BindingContext = new ModelVTModel
                {
                    ModelsList = App.OrdersDataBase.GetAllModels()
                };
            }
        }
    }
}