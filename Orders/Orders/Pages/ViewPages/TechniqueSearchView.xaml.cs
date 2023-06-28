using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orders.Models;
using Orders.Tables;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Orders.Pages.ViewPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TechniqueSearchView : ContentPage
    {
        public TechniqueSearchView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            FillTechniqueList();
            base.OnAppearing();
        }

        private void FillTechniqueList()
        {
            var OrdPage = Application.Current.MainPage.Navigation.ModalStack[0];
            if(OrdPage is OrderPage page)
            {
                BindingContext = new TechniqueModel
                {
                    TechniqueList = App.OrdersDataBase.GetTechniqueByParentCode(page.ClientCode.Text)
                };
            }
        }

        private async void Techniques_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var TableRowPage = Application.Current.MainPage.Navigation.ModalStack[1];
            if (TableRowPage is OrderTableRowPage page)
            {
                page.TechniqueCode.Text = ((Technique)e.SelectedItem).Code;
            }
            await Navigation.PopModalAsync();
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var OrdPage = Application.Current.MainPage.Navigation.ModalStack[0];
            string parentcode = "";
            if (OrdPage is OrderPage page)
            {
                parentcode = page.ClientCode.Text;
            }

            string PartName = ((Entry)sender).Text;
            if (!string.IsNullOrEmpty(PartName))
            {
                BindingContext = new TechniqueModel
                {
                    TechniqueList = App.OrdersDataBase.GetTechniqueByParentAndName(parentcode,PartName)
                };
            }
            else
            {
                BindingContext = new TechniqueModel
                {
                    TechniqueList = App.OrdersDataBase.GetTechniqueByParentCode(parentcode)
                };
            }
        }
    }
}