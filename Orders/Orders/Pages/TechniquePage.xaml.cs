using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orders.Core;
using Orders.Pages.ViewPages;
using Orders.Tables;
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
            //FillClientsList();
            //FillGroupModelList();
        }

        protected override void OnAppearing()
        {
            FillClientData();
            FillModel();
            base.OnAppearing();
        }

        private void FillClientsList()
        {
            //var clients = App.OrdersDataBase.GetClients();
            //foreach (var client in clients)
            //{
            //    Client.Items.Add(client.Code + ": " + client.Name);
            //}
        }

        private void FillModel()
        {
            if(!string.IsNullOrEmpty(ModelCode.Text))
            {
                SelectedModel.BindingContext = App.OrdersDataBase.GetModelByCode(ModelCode.Text);
            }
        }

        private void FillClientData()
        {
            if (!string.IsNullOrEmpty(ClientCode.Text))
            {
                DataClient.BindingContext = App.OrdersDataBase.GetClientByCode(ClientCode.Text);
            }
        }

        //private void FillGroupModelList()
        //{
        //    var GroupsModel = App.OrdersDataBase.GetModelGroups();
        //    foreach (var group in GroupsModel)
        //    { 
        //        ModelGroup.Items.Add(group.Perfomance);
        //    }
        //}

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
			technique.Parent = SelectedClient.Text;
            technique.ParentCode = ClientCode.Text;
            technique.Title = technique.Name.ToUpper();
            //technique.GroupModel = (string)ModelGroup.SelectedItem;
            technique.ModelCode = ModelCode.Text;
            technique.Model = SelectedModel.Text;
			
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
                if (ordPage.ClientCode.Text == ClientCode.Text)
                {
                    //string AddSerial = technique.SerialKey.Length > 0 ? ": " + technique.SerialKey : "";
                    //OrdTablPage.Techniques.Items.Add(technique.Code + ": " + technique.Name + AddSerial);
                    //OrdTablPage.Techniques.SelectedItem = technique.Code + ": " + technique.Name + AddSerial;
                    OrdTablPage.TechniqueCode.Text = technique.Code;
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

        //private void ModelGroup_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string CodeModelGroup = ((string)(((Picker)sender).SelectedItem)).Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries)[0];
        //    var Models = App.OrdersDataBase.GetModelsByGroup(CodeModelGroup);
        //    Model.Items.Clear();
        //    foreach( var model in Models) 
        //    {
        //        Model.Items.Add(model.Perfomance);
        //    }
        //}

        private async void SearchClient(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ClientSearchView());
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ModelSearchView());
        }
    }
}

