﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orders.Core;
using Orders.Tables;
using Orders.Pages.ViewPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderTableRowPage : ContentPage
    {
        public OrderTableRowPage()
        {
            InitializeComponent();

            //FillTechniquesList();

            FillMalfunctionsList();

            FillEquipmentsList();
        }

        protected override void OnAppearing()
        {
            FillTechniqueData();
            base.OnAppearing();
        }

        private void FillTechniqueData()
        {
            if (!string.IsNullOrEmpty(TechniqueCode.Text))
            {
                TechniqueData.BindingContext = App.OrdersDataBase.GetTechniqueByCode(TechniqueCode.Text);
            }
        }

        private void FillTechniquesList()
        {
            //OrderPage nvPage = (OrderPage)Application.Current.MainPage.Navigation.ModalStack[0];
            //string client = (string)nvPage.ClientList.SelectedItem;
            //var ParentTechniques = App.OrdersDataBase.GetTechniqueByParent(client);
            //foreach (var technique in ParentTechniques)
            //{
            //    string AddSerial = technique.SerialKey.Length > 0 ? ": " + technique.SerialKey : "";
            //    string TechniqueName = technique.Code + ": " + technique.Name + AddSerial;
            //    Techniques.Items.Add(TechniqueName);
            //}
        }

        private void FillMalfunctionsList()
        {
            var MalfunctionsList = App.OrdersDataBase.GetMalfunctions();
            foreach (var malfunction in MalfunctionsList)
            {
                Malfunctions.Items.Add(malfunction.Name);
            }
        }

        private void FillEquipmentsList()
        {
            var kitElements = App.OrdersDataBase.GetKitElements();
            foreach (var element in kitElements)
            {
                Equipments.Items.Add(element.Name);
            }
        }

        private void AddKitElement(object sender, EventArgs e)
        {
            KitElement kitElement = new KitElement
            {
                Code = CodeGenerator.GetCodeForKitElement(),
                Name = ""
            };
            KitElementPage page = new KitElementPage
            {
                BindingContext = kitElement
            };
            Navigation.PushModalAsync(page);
        }

        private void SelectedKitElement(object sender, EventArgs e)
        {
            if (Equipments.SelectedItem != null)
            {
                string SpaceSymbol = (Equipment.Text != null && Equipment.Text.Length > 0) ? " " : "";
                Equipment.Text = Equipment.Text + SpaceSymbol + (string)Equipments.SelectedItem + ";";
                Equipments.SelectedItem = null;
            }
        }

        private void AddMalfunctions(object sender, EventArgs e)
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
            Navigation.PushModalAsync(page);

        }

        private void SelectedMalfunction(object sender, EventArgs e)
        {
            if (Malfunctions.SelectedItem != null)
            {
                string SpaceSymbol = (Malfunction.Text != null && Malfunction.Text.Length > 0) ? " " : "";
                Malfunction.Text = Malfunction.Text + SpaceSymbol + (string)Malfunctions.SelectedItem + ";";
                Malfunctions.SelectedItem = null;
            }
        }

        private void AddTechniques(object sender, EventArgs e)
        {
            OrderPage nvPage = (OrderPage)Application.Current.MainPage.Navigation.ModalStack[0];

            Client client = App.OrdersDataBase.GetClientByCode(nvPage.ClientCode.Text);

            Technique technique = new Technique
            {
                Code = CodeGenerator.GetUIDForTechnique(),
                Name = "",
                SerialKey = "",
                ParentCode = client.Code,
                Parent = client.Name
            };

            TechniquePage page = new TechniquePage
            {
                BindingContext = technique,
            };

            Navigation.PushModalAsync(page);
        }

        private void SaveRow(object sender, EventArgs e)
        {
            OrderTableRow OrderRow = (OrderTableRow)BindingContext;

            OrderRow.Technic = SelectedTechnique.Text;

            OrderRow.TechniqueCode = TechniqueCode.Text;

            OrderRow.Malfunction = Malfunction.Text;

            if (!string.IsNullOrEmpty(OrderRow.Technic))
            {
                App.OrdersDataBase.SaveRow(OrderRow);
            }
            Navigation.PopModalAsync();
        }

        private async void DeleteRow(object sender, EventArgs e)
        {
            bool Answer = await DisplayAlert("Внимание! Вопрос!", "Удалить данные?", "Да", "Нет");

            if (Answer)
            {
                OrderTableRow OrderRow = (OrderTableRow)BindingContext;
                App.OrdersDataBase.DeleteRow(OrderRow.Id, OrderRow.ParentNumber);
                await Navigation.PopModalAsync();
            }
        }
        
        private void Cancel(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void EntryCompleted(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(((Entry)sender).Text) && ((Entry)sender).Text[((Entry)sender).Text.Length - 1] != ';')
            {
                ((Entry)sender).Text = ((Entry)sender).Text + ";";
            }
        }

        private void OpenTechnique(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TechniqueCode.Text))
            {
                //string Code = Techniques.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries)[0];
                Technique technique = App.OrdersDataBase.GetTechniqueByCode(TechniqueCode.Text);
                TechniquePage page = new TechniquePage
                {
                    BindingContext = technique
                };
                Navigation.PushModalAsync(page);
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new TechniqueSearchView());
        }
    }
}