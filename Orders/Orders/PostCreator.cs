using System;
using System.Collections.Generic;
using System.Text;

namespace Orders
{
    public class PostCreator
    {   
    /// <summary>
    /// Создает запись о клиенте в бд
    /// </summary>
    /// <param name="DataClient">
    /// массив с информацией о клиенте где:
    /// [0] - Наименование,
    /// [1] - ИНН
    /// </param>
        public static void CreateClient(string[] DataClient)
        {
            if (string.IsNullOrEmpty(DataClient[1])) return;

            Client client = new Client
            {
                Name = DataClient[0],
                Inn = DataClient[1],
                Email = "",
                PhoneNumber = ""
               
            };
            App.OrdersDataBase.SaveClient(client);
        }

        /// <summary>
        /// Создает запись об Аппаратуре в бд
        /// </summary>
        /// <param name="DataTechnique">
        /// массив данных об аппаратуре где:
        /// [0] - Наименование
        /// [1] - Владелец
        /// [2] - Серийный номер
        /// </param>
        public static void CreateTechnique(string[] DataTechnique)
        {
            
            string parent = "";
            if (!string.IsNullOrEmpty(DataTechnique[1]))
            {
                Client client = App.OrdersDataBase.GetClientByName(DataTechnique[1]);
                if (client == null) return;
                parent = client.Name + ";;" + client.Inn;
            }
            Technique technique = new Technique
            {
                Name = DataTechnique[0],
                Parent = parent,
                SerialKey = DataTechnique[2]

            };
            App.OrdersDataBase.SaveTechnique(technique);
        }
        
        public static void CreateMalfunction(string name)
        {
            Malfunction malfunction = new Malfunction
            {
                Name = name,
            };
            App.OrdersDataBase.SaveMalfunction(malfunction);
        }
        public static void CreateKitElement(string name)
        {
            KitElement kitElement = new KitElement
            {
                Name = name,
            };
            App.OrdersDataBase.SaveKitElement(kitElement);
        }
    }
}
