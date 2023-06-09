﻿using System;
using System.Collections.Generic;
using System.Linq;
using Orders.Tables;
using SQLite;
using Xamarin.Essentials;

namespace Orders.Core
{
    public class OrderRepository
    {
        SQLiteConnection ordersdatabase;
        public OrderRepository(string databasePath)
        {
            ordersdatabase = new SQLiteConnection(databasePath);
            ordersdatabase.CreateTable<Order>();
            ordersdatabase.CreateTable<OrderTableRow>();
            ordersdatabase.CreateTable<Client>();
            ordersdatabase.CreateTable<Technique>();
            ordersdatabase.CreateTable<Malfunction>();
            ordersdatabase.CreateTable<KitElement>();
            ordersdatabase.CreateTable<ModelGroup>();
            ordersdatabase.CreateTable<Model>();
        }

        public void VacuumDB()
        {
            ordersdatabase.Execute("VACUUM");
        }

        public int SaveModel(Model model)
        {
            if (model.Id != 0)
            {
                return ordersdatabase.Update(model);
            }
            else
            {
                return ordersdatabase.Insert(model);
            }
        }

        public List<Model> GetModelsByName(string partname)
        {
            return ordersdatabase.Query<Model>("SELECT * FROM Models WHERE Title LIKE " + $"'{"%" + partname.ToUpper() + "%"}'").ToList();
        }
        public List<Model> GetAllModels() 
        { 
            return ordersdatabase.Table<Model>().ToList();
        }
        //public IEnumerable<Model> GetModelsByGroup(string CodeGroup)
        //{
        //    var RequestResult = ordersdatabase.Query<Model>("SELECT * FROM Models WHERE GroupCode = " + $"'{CodeGroup}'");
        //    return RequestResult.ToList();
        //}
        public Model GetModelByCode(string Code)
        {
            var RequestResult = ordersdatabase.Query<Model>("SELECT * FROM Models WHERE Code = " + $"'{Code}'");
            Model model = RequestResult.Count() > 0 ? RequestResult[0] : null;
            return model;
        }
        public int SaveModelGroup(ModelGroup modelGroup)
        {
            if (modelGroup.Id != 0)
            {
                return ordersdatabase.Update(modelGroup);
            }
            else
            {
                return ordersdatabase.Insert(modelGroup);
            }
        }

        //public IEnumerable<ModelGroup> GetModelGroups()
        //{
        //    return ordersdatabase.Table<ModelGroup>().ToList();
        //}

        public ModelGroup GetModelGroupByCode(string Code)
        {
            var RequestResult = ordersdatabase.Query<ModelGroup>("SELECT * FROM ModelGroups WHERE Code = " + $"'{Code}'");
            ModelGroup modelGroup = RequestResult.Count() > 0 ? RequestResult[0] : null;
            return modelGroup;
        }

        public IEnumerable<KitElement> GetKitElements()
        {
            return ordersdatabase.Table<KitElement>().ToList();
        }

        public int DeleteKitElement(int id)
        {
            return ordersdatabase.Delete<KitElement>(id);
        }

        public int SaveKitElement(KitElement kitElement)
        {
            if (kitElement.Id != 0)
            {
                return ordersdatabase.Update(kitElement);
            }
            else
            {
                return ordersdatabase.Insert(kitElement);
            }
        }
        public KitElement GetKitElementByName(string Name)
        {
            var RequestResult = ordersdatabase.Query<KitElement>("SELECT * FROM KitElements WHERE Name = " + $"'{Name}'");
            KitElement kitElement = RequestResult.Count() > 0 ? RequestResult[0] : null;
            return kitElement;
        }

        public int GetLastKitElementID()
        {
            var laststr = ordersdatabase.Query<Technique>("SELECT Id FROM KitElements WHERE Id = (SELECT MAX(Id) FROM KitElements)");

            return laststr.Count == 0 ? 0 : laststr[0].Id;
        }

        public IEnumerable<Malfunction> GetMalfunctions()
        {
            return ordersdatabase.Table<Malfunction>().ToList();
        }

        public Malfunction GetMalfunctionByName(string Name)
        {
            var RequestResult = ordersdatabase.Query<Malfunction>("SELECT * FROM Malfunctions WHERE Name = " + $"'{Name}'");
            Malfunction malfunction = RequestResult.Count() > 0 ? RequestResult[0] : null;
            return malfunction;
        }

        public int DeleteMalfunction(int Id)
        {
            return ordersdatabase.Delete<Malfunction>(Id);
        }

        public int SaveMalfunction(Malfunction Malfunction)
        {
            if (Malfunction.Id != 0)
            {
                return ordersdatabase.Update(Malfunction);
            }
            else
            {
                return ordersdatabase.Insert(Malfunction);
            }
        }
        public int GetLastMalfunctionID()
        {
            var laststr = ordersdatabase.Query<Technique>("SELECT Id FROM Malfunctions WHERE Id = (SELECT MAX(Id) FROM Malfunctions)");

            return laststr.Count == 0 ? 0 : laststr[0].Id;
        }
        public List<Technique> GetTechniques()
        {
            return ordersdatabase.Table<Technique>().ToList();
        }

        //public void DeleteAllTechnique()
        //{
        //    ordersdatabase.DeleteAll<Technique>();
        //}

        public int DeleteTecnique(int Id)
        {
            return ordersdatabase.Delete<Technique>(Id);
        }

        public int SaveTechnique(Technique technique)
        {
            if (technique.Id != 0)
            {
                return ordersdatabase.Update(technique);
            }
            else
            {
                return ordersdatabase.Insert(technique);
            }
        }

        public Technique GetTechniqueByCode(string Code)
        {
            var RequestResult = ordersdatabase.Query<Technique>("SELECT * FROM Techniques WHERE Code = " + $"'{Code}'");
            Technique technique = RequestResult.Count() > 0 ? RequestResult[0] : null;
            return technique;
        }
        public Technique GetTechniqueByParams(string Name, string SerialKey, string Parent)
        {
            var RequestResult = ordersdatabase.Query<Technique>("SELECT * FROM Techniques WHERE Name = " + $"'{Name}'"
                                                                                       + "AND SerialKey = " + $"'{SerialKey}'"
                                                                                       + "AND Parent = " + $"'{Parent}'");
            Technique technique = RequestResult.Count() > 0 ? RequestResult[0] : null;
            return technique;
        }

        public List<Technique> GetTechniqueByParentAndName(string ParentCode, string name)
        {
            string request = "SELECT * FROM Techniques WHERE ParentCode = " + $"'{ParentCode}'" 
                + " AND Title LIKE " + $"'{"%" + name.ToUpper() +"%"}'";
            var Technuques = ordersdatabase.Query<Technique>(request).ToList();
            return Technuques;
        }

        public List<Technique> GetTechniqueByParentCode(string ParentCode)
        {
            string request = "SELECT * FROM Techniques WHERE ParentCode = " + $"'{ParentCode}'";
            var Technuques = ordersdatabase.Query<Technique>(request).ToList();
            return Technuques;
        }
        public int GetLastTechniqueID()
        {
            var laststr = ordersdatabase.Query<Technique>("SELECT Id FROM Techniques WHERE Id = (SELECT MAX(Id) FROM Techniques)");

            return laststr.Count == 0 ? 0 : laststr[0].Id;

        }

        public List<Client> GetClients()
        {
            return ordersdatabase.Table<Client>().ToList();
        }

        public List<Client> GetClientsByPartName(string PartName)
        {
            return ordersdatabase.Query<Client>("SELECT * FROM Clients WHERE Title LIKE " + $"'{"%" + PartName.ToUpper() + "%"}'").ToList();
        }

        public int DeleteClient(int Id)
        {
            return ordersdatabase.Delete<Client>(Id);
        }
        public int SaveClient(Client client)
        {
            if (client.Id != 0)
            {
                ordersdatabase.Update(client);
                return client.Id;
            }
            else
            {
                return ordersdatabase.Insert(client);
            }
        }

        public Client GetClientByINN(string inn)
        {
            var RequestResult = ordersdatabase.Query<Client>("SELECT * FROM Clients WHERE Inn = " + $"'{inn}'");
            Client client = RequestResult.Count() > 0 ? RequestResult[0] : null;
            return client;
        }

        public Client GetClientByCode(string Code)
        {
            var RequestResult = ordersdatabase.Query<Client>("SELECT * FROM Clients WHERE Code = " + $"'{Code}'");
            Client client = RequestResult.Count() > 0 ? RequestResult[0] : null;
            return client;
        }

        public Client GetClientByName(string Name)
        {
            var RequestResult = ordersdatabase.Query<Client>("SELECT * FROM Clients WHERE Name = " + $"'{Name}'");
            Client client = RequestResult.Count() > 0 ? RequestResult[0] : null;
            return client;
        }
        public int GetLastClientID()
        {
            var laststr = ordersdatabase.Query<Client>("SELECT Id FROM Clients WHERE Id = (SELECT MAX(Id) FROM Clients)");

            return laststr.Count == 0 ? 0 : laststr[0].Id;

        }
        public List<Order> GetOrders()
        {
            return ordersdatabase.Table<Order>().ToList();
        }

        public Order GetOrder(int id)
        {
            return ordersdatabase.Get<Order>(id);
        }

        public Order GetOrderByCode(string Code)
        {
            var RequestResult = ordersdatabase.Query<Order>("SELECT * FROM Orders WHERE Code = " + $"'{Code}'");
            Order order = RequestResult.Count() > 0 ? RequestResult[0] : null;
            return order;
        }

        public int DeleteOrder(int id, int Number)
        {
            ordersdatabase.Execute($"DELETE FROM OrderTableRows WHERE ParentNumber = {Number}");
            return ordersdatabase.Delete<Order>(id);
        }

        public int SaveOrder(Order order)
        {
            if (order.Id != 0)
            {
                ordersdatabase.Update(order);
                return order.Id;
            }
            else
            {
                return ordersdatabase.Insert(order);
            }
        }
        public void UpdateOrder(Order order)
        {
            if (order.Id != 0)
            {
                ordersdatabase.Update(order);
            }
        }

        public int GetLast()
        {
            var laststr = ordersdatabase.Query<Order>("SELECT Number FROM Orders WHERE _id = (SELECT MAX(_id) FROM Orders)");

            return laststr.Count == 0 ? 0 : laststr[0].Number;

        }

        public List<OrderTableRow> GetRows(int ParentNumber)
        {

            string request = $"SELECT * FROM OrderTableRows WHERE ParentNumber = {ParentNumber}";
            var rows = ordersdatabase.Query<OrderTableRow>(request);
            return rows;

        }

        public int DeleteRow(int Id, int ParentNumber)
        {
            ordersdatabase.Execute($"UPDATE OrderTableRows SET Number = Number - 1 WHERE Number > (SELECT Number FROM OrderTableRows WHERE _id = {Id}) AND ParentNumber = {ParentNumber}");
            return ordersdatabase.Delete<OrderTableRow>(Id);
        }
        //public void DeleteRows(int Number)
        //{
        //    ordersdatabase.Execute($"DELETE FROM OrderTableRows WHERE ParentNumber = {Number}");
        //}
        public int SaveRow(OrderTableRow row)
        {
            if (row.Id != 0)
            {
                return ordersdatabase.Update(row);
            }
            else
            {
                return ordersdatabase.Insert(row);
            }
        }

        public int GetLastRow(int ParentNumber)
        {
            string request = $"SELECT Number FROM OrderTableRows WHERE _id = (SELECT MAX(_id) FROM OrderTableRows WHERE ParentNumber = {ParentNumber})";
            var lastrow = ordersdatabase.Query<OrderTableRow>(request);
            return lastrow.Count == 0 ? 0 : lastrow[0].Number;
        }
    }
}
