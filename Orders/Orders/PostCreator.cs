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
                if( client.Inn != DataClient["ИНН"])
                {
                    client.Inn = DataClient["ИНН"];
                }
                else if(client.Name != DataClient["Наименование"])
                {
                    client.Name = DataClient["Наименование"];
                }
                else if(client.PhoneNumber != DataClient["Телефон"])
                {
                    client.PhoneNumber = DataClient["Телефон"];
                }
                else if(client.Email != DataClient["Email"])
                {
                    client.Email = DataClient["Email"];
                }
                else
                {
                    return;
                }
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
        /// массив данных об аппаратуре где:
        /// [0] - Код
        /// [1] - Наименование
        /// [2] - Владелец
        /// [3] - Серийный номер
        /// </param>
        public static void CreateTechnique(string[] DataTechnique)
        {   
            if(App.OrdersDataBase.GetTechniqueByCode(DataTechnique[0]) != null) return;            
            
            string parent;

            Client client = App.OrdersDataBase.GetClientByName(DataTechnique[2]);
            if (client == null) return;
            parent = client.Code + ": " + client.Name;

            Technique technique = App.OrdersDataBase.GetTechniqueByParams(DataTechnique[1], DataTechnique[3], parent);

            if (technique == null)
            {
                technique = new Technique
                {
                    Code = DataTechnique[0],
                    Name = DataTechnique[1],
                    Parent = parent,
                    SerialKey = DataTechnique[3]

                };                
            }
            else
            {
                technique.Code = DataTechnique[0];
            }
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
