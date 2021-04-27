using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Your_Turn_Client.Scripts
{
    class DB
    {
        public static MySqlConnection Connection;
        string DB_IP = "127.0.0.1", DB_Port = "3306", DB_Name = "login", DB_ID = "root", DB_PWD = "m60606060";
        string FIndNickNameSql = "select * from userinfo";


        public DB()
        {
            ConnectDataBase();
        }
        
        void ConnectDataBase()
        {
            string ConnectionString = String.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4}", DB_IP, DB_Port, DB_Name, DB_ID, DB_PWD);
            Connection = new MySqlConnection(ConnectionString);
            Connection.Open();
        }

        public string TryLogin(string Id, string Password)
        {
            ConnectDataBase();
            MySqlCommand cmd = new MySqlCommand(FIndNickNameSql, Connection);
            MySqlDataReader Reader = cmd.ExecuteReader();

            while (Reader.Read())
                {
                if (Reader["Id"].ToString().Equals(Id))
                    {
                    if (Reader["Password"].ToString().Equals(Password))
                        {
                        return "로그인 성공";
                        }
                    return "아이디 또는 비밀번호가 다릅니다.";
                    }
                }
            return "로그인 실패";
        }

        public int InsertUser(string Id, string Password, string Nickname)
        {
            ConnectDataBase();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO UserInfo VALUES('" + Id + "','" + Password + "','" + Nickname + "')", Connection);
            return cmd.ExecuteNonQuery();
        }

        public bool CheckOverlapNickName(string NickName)
        {
            ConnectDataBase();
            MySqlCommand cmd = new MySqlCommand(FIndNickNameSql, Connection);
            MySqlDataReader Reader = cmd.ExecuteReader();

            bool isOverlap = false;
            while(Reader.Read())
               {
                if (Reader["Nickname"].ToString().Equals(NickName))
                    isOverlap = true;
               }

            Console.WriteLine(NickName + " " + isOverlap);
            if (isOverlap)
                return true;
            else
                return false;
        }

        public bool CheckOverlapID(string ID)
        {
            ConnectDataBase();
            MySqlCommand cmd = new MySqlCommand(FIndNickNameSql, Connection);
            MySqlDataReader Reader = cmd.ExecuteReader();

            bool isOverlap = false;
            while (Reader.Read())
            {
                if (Reader["Id"].ToString().Equals(ID))
                    isOverlap = true;
            }

            Console.WriteLine(ID + " " + isOverlap);
            if (isOverlap)
                return true;
            else
                return false;
        }
    }
}
