using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Orders
{
    public class PostCreator
    {   
    /// <summary>
    /// Создает запись о клиенте в бд
    /// </summary>
    /// <param name="DataClient">
    /// словарь с информацией о клиенте
    /// </param>
        public static void CreateClient(Dictionary<string,string> DataClient)
        {            
            if (string.IsNullOrEmpty(DataClient["ИНН"])) return;
            Client client = App.OrdersDataBase.GetClientByINN(DataClient["ИНН"]);
            if(client != null)
            {
                bool isChanged = false;
                if( client.Inn != DataClient["ИНН"])
                {
                    client.Inn = DataClient["ИНН"];
                    isChanged = true;
                }
                if(client.Name != DataClient["Наименование"])
                {
                    client.Name = DataClient["Наименование"];
                    isChanged = true;
                }
                if(client.PhoneNumber != DataClient["Телефон"])
                {
                    client.PhoneNumber = DataClient["Телефон"];
                    isChanged = true;
                }
                if(client.Email != DataClient["Email"])
                {
                    client.Email = DataClient["Email"];
                    isChanged = true;
                }
                if (!isChanged) return;
            }
            else
            {
                client = new Client
                {
                    Code = CodeGenerator.GetUIDForClient(),
                    Name = DataClient["Наименование"],
                    Inn = DataClient["ИНН"],
                    Email = DataClient["Email"],
                    PhoneNumber = DataClient["Телефон"]

                };
            }
            App.OrdersDataBase.SaveClient(client);
        }

        /// <summary>
        /// Создает запись об Аппаратуре в бд
        /// </summary>
        /// <param name="DataTechnique">
        /// словарь с данными об аппаратуре
        /// </param>
        public static void CreateTechnique(Dictionary<string,string> DataTechnique)
        {
            Client client = App.OrdersDataBase.GetClientByName(DataTechnique["Владелец"]);
            string parent = client != null ? (client.Code + ": " + client.Name) : "";

            Technique technique = App.OrdersDataBase.GetTechniqueByParams(DataTechnique["Наименование"], DataTechnique["Серийный"], parent);

            if (technique == null)
            {
                technique = new Technique
                {
                    Code = DataTechnique["Код"],
                    Name = DataTechnique["Наименование"],
                    Parent = parent,
                    SerialKey = DataTechnique["Серийный"]

                };                
            }
            else
            {
                bool isChanged = false;
                if(technique.Code != DataTechnique["Код"])
                {
                    technique.Code = DataTechnique["Код"];
                    isChanged = true;
                }
                if(technique.Name != DataTechnique["Наименование"])
                {
                    technique.Name = DataTechnique["Наименование"];
                    isChanged = true;
                }
                if(technique.Parent != parent)
                {
                    technique.Parent = parent;
                    isChanged = true;
                }
                if(technique.SerialKey != DataTechnique["Серийный"])
                {
                    technique.SerialKey = DataTechnique["Серийный"];
                    isChanged = true;
                }
                if (!isChanged) return;
            }
            App.OrdersDataBase.SaveTechnique(technique);
        }
        
        public static void CreateMalfunction(string name)
        {
            if (App.OrdersDataBase.GetMalfunctionByName(name) != null) return;
            Malfunction malfunction = new Malfunction
            {
                Code = CodeGenerator.GetCodeForMalfunction(),
                Name = name
            };
            App.OrdersDataBase.SaveMalfunction(malfunction);
        }
        public static void CreateKitElement(string name)
        {
            if (App.OrdersDataBase.GetKitElementByName(name) != null) return;
            KitElement kitElement = new KitElement
            {
                Code = CodeGenerator.GetCodeForKitElement(),
                Name = name
            };
            App.OrdersDataBase.SaveKitElement(kitElement);
        }
        public static void OrderisLoadChange(string Code)
        {
            Order order = App.OrdersDataBase.GetOrderByCode(Code);            
            if (order == null) return;
            order.isLoaded = true;
            App.OrdersDataBase.UpdateOrder(order);
        }
    }
}
