using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orders.Core;
using Orders.Tables;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class KitElementsView : ContentPage
	{
		public KitElementsView ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
			FillEquipmentsList();

            base.OnAppearing();
        }

		private void FillEquipmentsList()
		{
            KitElementsList.ItemsSource = App.OrdersDataBase.GetKitElements();
        }

        private async void OnSelected(object sender, SelectedItemChangedEventArgs e)
		{
            KitElement kitElement = (KitElement)e.SelectedItem;

			KitElementPage page = new KitElementPage
			{
				BindingContext = kitElement
			};

			await Navigation.PushModalAsync(page);
		}

		private async void CreateKitElement(object sender, EventArgs e)
		{
			KitElement kitElement = new KitElement
			{
				Code = CodeGenerator.GetCodeForKitElement(),
				Name = ""
			};

			KitElementPage page = new KitElementPage
			{
				BindingContext= kitElement,
			};

			await Navigation.PushModalAsync(page);
		}
	}
}