using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Orders
{
    public partial class App : Application
    {
        public const string ORDERS_DATABASE_NAME = "Orders.db";
        public static OrderRepository ordersdatabase;
        public static OrderRepository OrdersDataBase
        {
            get
            {
                if (ordersdatabase == null)
                {
                    ordersdatabase = new OrderRepository(Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), ORDERS_DATABASE_NAME));
                }
                return ordersdatabase;
            }
        
        }

        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            MainPage = new ShelMenu();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
