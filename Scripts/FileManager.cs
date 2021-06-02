using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Your_Turn_Client.Scripts
{
    class FileManager
    {
        string AutoLoginFilePath = @"D:\github\Your_Turn_Client\FileStream\AutoLogin.txt";
        string NickNameFilePath = @"D:\github\Your_Turn_Client\FileStream\NickName.txt";

        public void WriteAutoLoginValue(bool? isCheckAutoLogin, string NickName)
        {
            WriteText(isCheckAutoLogin.ToString(), AutoLoginFilePath);
        }
        public string ReadAutoLoginValue()
        {
            return ReadText(AutoLoginFilePath).Contains("True") ? "True" : "False";
        }

        public void WriteNickName(string NickName)
        {
            WriteText(NickName, NickNameFilePath);
        }

        public void WriteText(string value, string path)
        {
            using (FileStream fs = File.Create(path))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(value);
                fs.Write(info, 0, info.Length);
                fs.Close();
            }
        }

        public string ReadText(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                string text = "";
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    text += temp.GetString(b);
                }
                fs.Close();
                return text;
            }
        }
    }
}
