using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace Orders
{
    struct Settings
    {
        public string FTPUser;
        public string Prefix;
        public string FTPAdress;        
        public string FTPPassword;

        public bool CheckSettings()
        {
            string[] SettingsArray = { FTPUser, Prefix, FTPAdress, FTPPassword };
            foreach (string Setting in SettingsArray)
            {
                if (string.IsNullOrEmpty(Setting)) return false;
            }
            return true;
        }
    }
}
