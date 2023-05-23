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
	public partial class MalfunctionView : ContentPage
	{
		public MalfunctionView ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {	
			FillMalfunctionsList();

			base.OnAppearing();
        }

		private void FillMalfunctionsList()
		{
            MalfunctionList.ItemsSource = App.OrdersDataBase.GetMalfunctions();
        }

		private async void OnSelected(object sender, SelectedItemChangedEventArgs e)
		{
			Malfunction malfunction = (Malfunction)e.SelectedItem;

			MalfunctionPage page = new MalfunctionPage
			{
				BindingContext = malfunction
			};

			await Navigation.PushModalAsync(page);

		}

		private async void AddMalfunction(object sender, EventArgs e)
		{
			Malfunction malfunction = new Malfunction
			{
				Code = CodeGenerator.GetCodeForMalfunction(),
				Name = ""
			};

			MalfunctionPage page = new MalfunctionPage
			{
				BindingContext = malfunction
			};

			await Navigation.PushModalAsync(page);

		}
    }
}