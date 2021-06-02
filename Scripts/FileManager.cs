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
        FileStream AutoLoginFileStream, NickNameFileStream;

        public void CheckExistsFile()
        {
            if(File.Exists(AutoLoginFilePath))
                AutoLoginFileStream = File.Create(AutoLoginFilePath);
            if(File.Exists(NickNameFilePath))
                NickNameFileStream = File.Create(NickNameFilePath);
        }

        public void WriteAutoLoginValue()
        {
            
        }

        public bool CheckAutoLogin()
        {
            return true;
        }

        public void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }
    }
}
