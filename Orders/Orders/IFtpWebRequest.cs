﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orders
{
    public interface IFtpWebRequest
    {
        string Upload(string FtpUrl, string FileName, string UserName, string Password, string UploadDirectory = "");

        Task<string> Download(string FtpUrl, string PathTo, string UserName, string Password);

    }
}
