using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Linq;
using Xamarin.Essentials;

namespace Orders
{
    public class XML
    {
        public static void CreateXML(string folderPath, string fileName)
        {
            var orderlist = App.OrdersDataBase.GetOrders();
            if (orderlist.Count() > 0)
            {
                XmlDocument xmlDoc = new XmlDocument();
                var xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                xmlDoc.AppendChild(xmlDeclaration);
                XmlNode root = xmlDoc.CreateElement("Заказы");
                xmlDoc.AppendChild(root);

                foreach (var ord in orderlist)
                {
                    XmlNode order = xmlDoc.CreateElement("Заказ");
                    XmlNode OrderID = xmlDoc.CreateElement("ИД");
                    XmlNode OrdNumber = xmlDoc.CreateElement("Номер");
                    XmlNode OrderDate = xmlDoc.CreateElement ("Дата");
                    XmlNode Executor = xmlDoc.CreateElement("Исполнитель");
                    XmlNode OrderClient = xmlDoc.CreateElement("Клиент");
                    XmlNode SendEmail = xmlDoc.CreateElement("ОтправитьEmail");
                    
                    XmlAttribute ClientInn = xmlDoc.CreateAttribute("ИНН");
                    XmlAttribute ClientEmail = xmlDoc.CreateAttribute ("email");
                    XmlAttribute ClientPhone = xmlDoc.CreateAttribute("Телефон");

                    XmlText Id = xmlDoc.CreateTextNode($"{ord.Id}{Preferences.Get("Prefix", "No_Prefix")}");
                    XmlText Date = xmlDoc.CreateTextNode(ord.Date);
                    XmlText OrdNumberText = xmlDoc.CreateTextNode(ord.Number.ToString());
                    XmlText ExecutorName = xmlDoc.CreateTextNode(ord.Executor);

                    string[] ClientInfo = ord.Client.Split(new string[] {";;"},StringSplitOptions.RemoveEmptyEntries);
                    XmlText ClientName = xmlDoc.CreateTextNode(ClientInfo[0]);
                    XmlText Inn = xmlDoc.CreateTextNode(ClientInfo[1]);

                    Client client = App.OrdersDataBase.GetClientByINN(ClientInfo[1]);
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

                    if(TableRows.Count() > 0)
                    {
                        XmlNode Table = xmlDoc.CreateElement("ТЧ");
                        foreach(var row in TableRows)
                        {
                            XmlNode TableString = xmlDoc.CreateElement("Строка");

                            XmlNode Number = xmlDoc.CreateElement("НомерСтроки");
                            XmlNode Technique = xmlDoc.CreateElement("Техника");
                            XmlNode Malfunction = xmlDoc.CreateElement("Неисправность");
                            XmlNode Equipment = xmlDoc.CreateElement("Комплектация");

                            XmlAttribute serialKey = xmlDoc.CreateAttribute("СерийныйНомер");

                            XmlText TextNum = xmlDoc.CreateTextNode(row.Number.ToString());
                            XmlText MalfunctionName = xmlDoc.CreateTextNode(row.Malfunction);
                            XmlText KitElements = xmlDoc.CreateTextNode(row.Equipment);

                            string[] TechniqueInfo = row.Technic.Split(new string[] {";;"},StringSplitOptions.RemoveEmptyEntries);
                            XmlText TechniqueName = xmlDoc.CreateTextNode(TechniqueInfo[0]);
                            XmlText SerialKeyText = xmlDoc.CreateTextNode(TechniqueInfo[1]);

                            Number.AppendChild(TextNum);
                            serialKey.AppendChild(SerialKeyText);
                            Technique.Attributes.Append(serialKey);
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
        }
    }
}
