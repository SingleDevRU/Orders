using System;
using System.Collections.Generic;
using System.Text;

namespace Orders
{
    public interface IFtpWebRequest
    {
        string Upload(string FtpUrl, string FileName, string UserName, string Password, string UploadDirectory = "");

        string Download(string FtpUrl, string PathTo, string UserName, string Password);

    }
}
