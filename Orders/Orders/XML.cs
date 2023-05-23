using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Linq;
using Xamarin.Essentials;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices.ComTypes;

namespace Orders
{
    public class XML
    {
        public static async Task<string> StartUploadXML(string folderPath, string fileName)
        {
            await Task.Yield();
            var orderlist = App.OrdersDataBase.GetOrders();
            if (orderlist.Count() == 0) return "Нет данных для выгрузки";
            try
            {
                CreateXml(folderPath, fileName, orderlist);
                return "Файл создан";
            }
            catch(Exception ex) 
            { 
                return ex.ToString();
            }
            
                //XmlDocument xmlDoc = new XmlDocument();
                //var xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                //xmlDoc.AppendChild(xmlDeclaration);
                //XmlNode root = xmlDoc.CreateElement("Квитанции");
                //xmlDoc.AppendChild(root);

                //foreach (var ord in orderlist)
                //{
                //    XmlNode order = xmlDoc.CreateElement("Квитанция");
                //    XmlNode OrderID = xmlDoc.CreateElement("ИД");
                //    XmlNode OrdNumber = xmlDoc.CreateElement("Номер");
                //    XmlNode OrderDate = xmlDoc.CreateElement ("Дата");
                //    XmlNode Executor = xmlDoc.CreateElement("Исполнитель");
                //    XmlNode OrderClient = xmlDoc.CreateElement("Клиент");
                //    XmlNode SendEmail = xmlDoc.CreateElement("ОтправитьEmail");
                    
                //    XmlAttribute ClientInn = xmlDoc.CreateAttribute("ИНН");
                //    XmlAttribute ClientEmail = xmlDoc.CreateAttribute ("email");
                //    XmlAttribute ClientPhone = xmlDoc.CreateAttribute("Телефон");

                //    XmlText Id = xmlDoc.CreateTextNode($"{ord.Id}{Preferences.Get("Prefix", "No_Prefix")}");
                //    XmlText Date = xmlDoc.CreateTextNode(ord.Date);
                //    XmlText OrdNumberText = xmlDoc.CreateTextNode(ord.Number.ToString());
                //    XmlText ExecutorName = xmlDoc.CreateTextNode(ord.Executor);

                //    string[] ClientInfo = ord.Client.Split(new string[] {": "},StringSplitOptions.RemoveEmptyEntries);
                //    Client client = App.OrdersDataBase.GetClientByCode(ClientInfo[0]);
                //    XmlText ClientName = xmlDoc.CreateTextNode(ClientInfo[1]);
                //    XmlText Inn = xmlDoc.CreateTextNode(client.Inn);                    
                //    XmlText Email = xmlDoc.CreateTextNode(client.Email);
                //    XmlText Phone = xmlDoc.CreateTextNode(client.PhoneNumber);

                //    string ConvertBool;
                //    if (ord.SendMail) 
                //    {
                //        ConvertBool = "Истина";
                //    }
                //    else
                //    {
                //        ConvertBool = "Ложь";
                //    }
                //    XmlText SendMail = xmlDoc.CreateTextNode(ConvertBool);
                
                //    OrderID.AppendChild(Id);
                //    OrdNumber.AppendChild(OrdNumberText);
                //    OrderDate.AppendChild(Date);
       
                //    ClientInn.AppendChild(Inn);
                //    ClientEmail.AppendChild(Email);
                //    ClientPhone.AppendChild(Phone);
                //    Executor.AppendChild(ExecutorName);
               
                //    OrderClient.Attributes.Append(ClientInn);
                //    OrderClient.Attributes.Append(ClientEmail);
                //    OrderClient.Attributes.Append(ClientPhone);
                //    OrderClient.AppendChild(ClientName);

                //    SendEmail.AppendChild(SendMail);

                //    order.AppendChild(OrderID);
                //    order.AppendChild(OrdNumber);
                //    order.AppendChild(OrderDate);
                //    order.AppendChild(Executor);
                //    order.AppendChild(OrderClient);
                //    order.AppendChild(SendEmail);

                //    var TableRows = App.ordersdatabase.GetRows(ord.Number);

                //    if(TableRows.Count() > 0)
                //    {
                //        XmlNode Table = xmlDoc.CreateElement("ТЧ");
                //        foreach(var row in TableRows)
                //        {
                //            XmlNode TableString = xmlDoc.CreateElement("Строка");

                //            XmlNode Number = xmlDoc.CreateElement("НомерСтроки");
                //            XmlNode Technique = xmlDoc.CreateElement("Техника");
                //            XmlNode Malfunction = xmlDoc.CreateElement("Неисправность");
                //            XmlNode Equipment = xmlDoc.CreateElement("Комплектация");

                //            XmlAttribute serialKey = xmlDoc.CreateAttribute("СерийныйНомер");
                //            XmlAttribute TechniqueCode = xmlDoc.CreateAttribute("Код");

                //            XmlText TextNum = xmlDoc.CreateTextNode(row.Number.ToString());
                //            XmlText MalfunctionName = xmlDoc.CreateTextNode(row.Malfunction);
                //            XmlText KitElements = xmlDoc.CreateTextNode(row.Equipment);

                //            string[] TechniqueInfo = row.Technic.Split(new string[] {": "},StringSplitOptions.RemoveEmptyEntries);
                //            XmlText TechniqueCodeText = xmlDoc.CreateTextNode(TechniqueInfo[0]);
                //            XmlText TechniqueName = xmlDoc.CreateTextNode(TechniqueInfo[1]);
                //            XmlText SerialKeyText = xmlDoc.CreateTextNode(TechniqueInfo.Length == 3 ? TechniqueInfo[2] : "");                            

                //            Number.AppendChild(TextNum);
                //            serialKey.AppendChild(SerialKeyText);
                //            TechniqueCode.AppendChild(TechniqueCodeText);
                           
                //            Technique.Attributes.Append(serialKey);
                //            Technique.Attributes.Append(TechniqueCode);

                //            Technique.AppendChild(TechniqueName);

                //            Malfunction.AppendChild(MalfunctionName);
                //            Equipment.AppendChild(KitElements);

                //            TableString.AppendChild(Number);
                //            TableString.AppendChild(Technique);

                //            if (!string.IsNullOrEmpty(Malfunction.InnerText))
                //            {
                //                TableString.AppendChild(Malfunction);
                //            }
                //            if (!string.IsNullOrEmpty(Equipment.InnerText))
                //            {
                //                TableString.AppendChild(Equipment);
                //            }

                //            Table.AppendChild(TableString);
                //        }
                //        order.AppendChild(Table);
                //    }

                //    root.AppendChild(order);
                //}
                //xmlDoc.Save(Path.Combine(folderPath, fileName));
        }
        private static void CreateXml(string folderPath,string fileName, IEnumerable<Order> orderlist)
        {
            XmlDocument xmlDoc = new XmlDocument();
            var xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(xmlDeclaration);
            XmlNode root = xmlDoc.CreateElement("Квитанции");
            xmlDoc.AppendChild(root);

            foreach (var ord in orderlist)
            {
                XmlNode order = xmlDoc.CreateElement("Квитанция");
                XmlNode OrderID = xmlDoc.CreateElement("ИД");
                XmlNode OrdNumber = xmlDoc.CreateElement("Номер");
                XmlNode OrderDate = xmlDoc.CreateElement("Дата");
                XmlNode Executor = xmlDoc.CreateElement("Исполнитель");
                XmlNode OrderClient = xmlDoc.CreateElement("Клиент");
                XmlNode SendEmail = xmlDoc.CreateElement("ОтправитьEmail");

                XmlAttribute ClientInn = xmlDoc.CreateAttribute("ИНН");
                XmlAttribute ClientEmail = xmlDoc.CreateAttribute("email");
                XmlAttribute ClientPhone = xmlDoc.CreateAttribute("Телефон");

                XmlText Id = xmlDoc.CreateTextNode($"{ord.Id}{Preferences.Get("Prefix", "No_Prefix")}");
                XmlText Date = xmlDoc.CreateTextNode(ord.Date);
                XmlText OrdNumberText = xmlDoc.CreateTextNode(ord.Number.ToString());
                XmlText ExecutorName = xmlDoc.CreateTextNode(ord.Executor);

                string[] ClientInfo = ord.Client.Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                Client client = App.OrdersDataBase.GetClientByCode(ClientInfo[0]);
                XmlText ClientName = xmlDoc.CreateTextNode(ClientInfo[1]);
                XmlText Inn = xmlDoc.CreateTextNode(client.Inn);
                XmlText Email = xmlDoc.CreateTextNode(client.Email);
                XmlText Phone = xmlDoc.CreateTextNode(client.PhoneNumber);

                string ConvertBool;
                if (ord.SendMail)
                {
                    ConvertBool = "Истина";
                }
                else
                {
                    ConvertBool = "Ложь";
                }
                XmlText SendMail = xmlDoc.CreateTextNode(ConvertBool);

                OrderID.AppendChild(Id);
                OrdNumber.AppendChild(OrdNumberText);
                OrderDate.AppendChild(Date);

                ClientInn.AppendChild(Inn);
                ClientEmail.AppendChild(Email);
                ClientPhone.AppendChild(Phone);
                Executor.AppendChild(ExecutorName);

                OrderClient.Attributes.Append(ClientInn);
                OrderClient.Attributes.Append(ClientEmail);
                OrderClient.Attributes.Append(ClientPhone);
                OrderClient.AppendChild(ClientName);

                SendEmail.AppendChild(SendMail);

                order.AppendChild(OrderID);
                order.AppendChild(OrdNumber);
                order.AppendChild(OrderDate);
                order.AppendChild(Executor);
                order.AppendChild(OrderClient);
                order.AppendChild(SendEmail);

                var TableRows = App.ordersdatabase.GetRows(ord.Number);

                if (TableRows.Count() > 0)
                {
                    XmlNode Table = xmlDoc.CreateElement("ТЧ");
                    foreach (var row in TableRows)
                    {
                        XmlNode TableString = xmlDoc.CreateElement("Строка");

                        XmlNode Number = xmlDoc.CreateElement("НомерСтроки");
                        XmlNode Technique = xmlDoc.CreateElement("Техника");
                        XmlNode Malfunction = xmlDoc.CreateElement("Неисправность");
                        XmlNode Equipment = xmlDoc.CreateElement("Комплектация");

                        XmlAttribute serialKey = xmlDoc.CreateAttribute("СерийныйНомер");
                        XmlAttribute TechniqueCode = xmlDoc.CreateAttribute("Код");

                        XmlText TextNum = xmlDoc.CreateTextNode(row.Number.ToString());
                        XmlText MalfunctionName = xmlDoc.CreateTextNode(row.Malfunction);
                        XmlText KitElements = xmlDoc.CreateTextNode(row.Equipment);

                        string[] TechniqueInfo = row.Technic.Split(new string[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                        XmlText TechniqueCodeText = xmlDoc.CreateTextNode(TechniqueInfo[0]);
                        XmlText TechniqueName = xmlDoc.CreateTextNode(TechniqueInfo[1]);
                        XmlText SerialKeyText = xmlDoc.CreateTextNode(TechniqueInfo.Length == 3 ? TechniqueInfo[2] : "");

                        Number.AppendChild(TextNum);
                        serialKey.AppendChild(SerialKeyText);
                        TechniqueCode.AppendChild(TechniqueCodeText);

                        Technique.Attributes.Append(serialKey);
                        Technique.Attributes.Append(TechniqueCode);

                        Technique.AppendChild(TechniqueName);

                        Malfunction.AppendChild(MalfunctionName);
                        Equipment.AppendChild(KitElements);

                        TableString.AppendChild(Number);
                        TableString.AppendChild(Technique);

                        if (!string.IsNullOrEmpty(Malfunction.InnerText))
                        {
                            TableString.AppendChild(Malfunction);
                        }
                        if (!string.IsNullOrEmpty(Equipment.InnerText))
                        {
                            TableString.AppendChild(Equipment);
                        }

                        Table.AppendChild(TableString);
                    }
                    order.AppendChild(Table);
                }

                root.AppendChild(order);
            }
            xmlDoc.Save(Path.Combine(folderPath, fileName));
        }
        /// <summary>
        /// Инициализация XML документа и начало загрузки данных в бд
        /// </summary>
        /// <param name="PathToFile">Путь к файлу XML, полученного из 1с</param>
        /// <returns>Результат загрузки данных</returns>
        public static async Task<string> ReadXml(string PathToFile)
        {
            try
            {
                await Task.Yield();
                XmlDocument doc = new XmlDocument();
                doc.Load(PathToFile);
                LoadFromXML(doc);
                return "Прогрузка данных завершена";
            }
            catch(Exception ex)
            { 
                return ex.ToString();
            }
        }

        /// <summary>
        /// Прогрузка данных в бд из файла, полученного из 1с
        /// </summary>
        /// <param name="doc">XML файл, полученного из 1с</param>
        private static void LoadFromXML(XmlDocument doc)
        {
            XmlElement root = doc.DocumentElement;

            foreach (XmlElement node in root)
            {
                if (node.Name == "Контрагенты")
                {
                    foreach (XmlNode node2 in node.ChildNodes)
                    {

                        Dictionary<string,string> DataClient = new Dictionary<string,string>
                        {
                            {"Наименование", node2.Attributes.GetNamedItem("Наименование").Value },
                            {"ИНН", node2.Attributes.GetNamedItem("ИНН").Value },
                            {"Email", node2.Attributes.GetNamedItem("email").Value },
                            {"Телефон",node2.Attributes.GetNamedItem("Телефон").Value }
                        };
                        PostCreator.CreateClient(DataClient);
                    }
                }
                else if (node.Name == "Аппаратуры")
                {
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        Dictionary<string, string> DataTechnique = new Dictionary<string, string>
                        {
                            {"Код", node2.Attributes.GetNamedItem("Код").Value},
                            {"Наименование", node2.Attributes.GetNamedItem("Наименование").Value},
                            {"Владелец", node2.Attributes.GetNamedItem("Владелец").Value},
                            {"Серийный", node2.Attributes.GetNamedItem("Серийный").Value}
                        };
                        PostCreator.CreateTechnique(DataTechnique);
                    }
                }
                else if (node.Name == "Неисправности")
                {
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        PostCreator.CreateMalfunction(node2.Attributes.GetNamedItem("Наименование").Value);                        
                    }
                }
                else if (node.Name == "Комплектация")
                {
                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        PostCreator.CreateKitElement(node2.Attributes.GetNamedItem("Наименование").Value);
                    }
                }
            }
        }
    }
}
