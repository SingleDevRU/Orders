using Android.OS;
using Android.Systems;
using Orders.Droid;
using System;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using Xamarin.Forms.PlatformConfiguration;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(FTP))]
namespace Orders.Droid
{
    class FTP: IFtpWebRequest
    {
        public FTP() { }

        public string Upload(string FtpUrl, string FileName, string UserName, string Password, string UploadDirectory = "")
        {
            try
            {
                string PureName = new FileInfo(FileName).Name;
                string UploadURL = $"{FtpUrl}{UploadDirectory}/{PureName}";
                FtpWebRequest req = (FtpWebRequest)FtpWebRequest.Create(UploadURL);
                req.Proxy = null;
                int Timeout = 6000;
                req.Timeout = Timeout;
                req.Method = WebRequestMethods.Ftp.UploadFile;
                req.Credentials = new NetworkCredential(UserName, Password);
                req.UseBinary = true;
                req.UsePassive = true;
                byte[] data = File.ReadAllBytes(FileName);
                req.ContentLength = data.Length;
                Stream stream = req.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                FtpWebResponse resp = (FtpWebResponse)req.GetResponse();
                return resp.StatusDescription;

            }
            catch(Exception e)
            { 
                return e.ToString();
            }
        }

        public async Task<string> Download(string FtpUrl, string PathTo, string UserName, string Password)
        { 
            try
            {
                await Task.Yield();
                FtpWebRequest req = (FtpWebRequest)WebRequest.Create(FtpUrl);
                req.Method = WebRequestMethods.Ftp.DownloadFile;
                req.Credentials = new NetworkCredential(UserName, Password);
                req.UsePassive = true;
                req.UseBinary = true;
                int Timeout = 6000;
                req.Timeout = Timeout;
                FtpWebResponse resp= (FtpWebResponse)req.GetResponse();
                Stream respStream = resp.GetResponseStream();
                FileStream file = File.Create(PathTo);
                byte[] data = new byte[64];
                int read;
                while((read = respStream.Read(data,0,data.Length)) > 0)
                {
                    file.Write(data, 0, read);
                }
                file.Close();
                respStream.Close();
                resp.Close();

                return "Загрузка завершена";

            }
            catch (Exception e) 
            { 
                return e.ToString();
            }
        } 
    }
}