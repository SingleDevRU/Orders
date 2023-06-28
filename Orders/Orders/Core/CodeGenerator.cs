using System;
using System.Collections.Generic;
using System.Text;
using Orders.Tables;
using Xamarin.Essentials;

namespace Orders.Core
{
    public class CodeGenerator
    {
        public static string GetUIDForClient()
        {
            int LastId = App.OrdersDataBase.GetLastClientID();
            return "КЛ-" + LastId.ToString();
        }

        public static string GetUIDForTechnique()
        {
            int LastId = App.OrdersDataBase.GetLastTechniqueID();
            return "АП-" + LastId.ToString();
        }

        public static string GetCodeForMalfunction()
        {
            int LastId = App.OrdersDataBase.GetLastMalfunctionID();
            return "Н-" + LastId.ToString();
        }

        public static string GetCodeForKitElement()
        {
            int LastId = App.OrdersDataBase.GetLastKitElementID();
            return "К-" + LastId.ToString();
        }
        public static string GetCodeForOrder(Order order)
        {
            return $"{order.Id}{Preferences.Get("Prefix", "No_Prefix")}";
        }
    }
}
