﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orders.Core;
using Orders.Models;
using Orders.Tables;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TechniqueView : ContentPage
	{
		public TechniqueView ()
		{
			InitializeComponent ();
			
		}

        protected override void OnAppearing()
        {
            FillTechniqueList();
            base.OnAppearing();
        }

		private void FillTechniqueList()
		{
            BindingContext = new TechniqueModel
			{
				TechniqueList = App.OrdersDataBase.GetTechniques()
			};
        }

		private async void OnSelected(object sender, SelectedItemChangedEventArgs e)
		{
			Technique SelectedTechnique = (Technique)e.SelectedItem;
			TechniquePage page = new TechniquePage
			{
				BindingContext = SelectedTechnique
			};

			await Navigation.PushModalAsync(page);
		}

        private async void AddTechnique(object sender, EventArgs e)
		{
			Technique technique = new Technique
			{
				Code = CodeGenerator.GetUIDForTechnique(),
				Name = "",
				Parent = "",
				SerialKey = ""
			};
			
			TechniquePage page = new TechniquePage
			{
				BindingContext = technique
			};

			await Navigation.PushModalAsync(page);
		}
    }
}