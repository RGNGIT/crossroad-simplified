using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using UNR_Crossroad.Core;

namespace UNR_Crossroad.Data
{
    // Класс для взаимодействия с Базой Данных
    public static class DbEngine
    {
        // Всякие ридеры, коммандеры, коннекторы
        private static SQLiteDataReader reader;
        private static SQLiteCommand command;
        private static SQLiteConnection connect;
        // Тут коннектимся к файлу БД
        public static void Connect()
        {
            connect = new SQLiteConnection("Data Source=DB\\UNR_Database.db;");
            connect.Open();
        }
        // Метод получения всех юзеров
        public static Dictionary<string,string> GetUsers()
        {
            command = new SQLiteCommand("SELECT * FROM 'Users';", connect);
            reader = command.ExecuteReader();
            Dictionary<string, string> users = new Dictionary<string, string>();
            foreach (DbDataRecord record in reader)
            {
                users.Add(record["login"].ToString(),record["password"].ToString());
            }
            return users;
        }
        // Метод получения уровня доступа по имени аккаунта
        public static int GetAccLevel(string name)
        {
            command = new SQLiteCommand("SELECT * FROM 'Users' WHERE login='"+name+"';", connect);
            reader = command.ExecuteReader();
            int lvl = 0;
            foreach (DbDataRecord record in reader)
            {
                lvl = Convert.ToInt32(record["level"]);
            }
            return lvl;
        }
        // Метод проверки юзера. Проверяем логин пароль и если совпало - логинимся
        public static bool UserCheck(string name, string pass)
        {
            command = new SQLiteCommand("SELECT * FROM 'Users' WHERE login='"+name+"' AND password='"+pass+"';", connect);
            reader = command.ExecuteReader();
            return reader.HasRows;
        }
        // Проверка существует ли такой логин
        public static bool LoginCheck(string name)
        {
            command = new SQLiteCommand("SELECT * FROM 'Users' WHERE login='"+name+"';", connect);
            reader = command.ExecuteReader();
            return reader.HasRows;
        }
        // Добавление нового юзера
        public static string AddUser(string login, string pass, int lvl)
        {
            command = new SQLiteCommand("INSERT INTO 'Users' ('id','login','password','level') VALUES ((SELECT MAX(id) FROM Users)+1,'"+login+"','"+pass+"',"+lvl+");",connect);
            command.ExecuteNonQuery();
            return "Пользователь добавлен";
        }
        // Сохраняем дороги
        public static void SaveRoad(string name,int right, int left, int up, int down,int i1,int i2,bool pright, bool pleft, bool pup, bool pdown)
        {
            command = new SQLiteCommand("INSERT INTO 'Crossroads' VALUES ((SELECT MAX(id) FROM Crossroads)+1,'" + name + "'," + right + "," + left + "," + up + "," + down + "," + i1 + "," + i2 + ",'" + pright + "','" + pleft + "','" + pup + "','" + pdown + "');", connect);
            command.ExecuteNonQuery();
        }
        // Получаем все дороги
        public static Dictionary<int, string> GetRoads()
        {
            Connect();
            command = new SQLiteCommand("SELECT * FROM 'Crossroads';", connect);
            reader = command.ExecuteReader();
            Dictionary<int, string> roads = new Dictionary<int, string>();
            foreach (DbDataRecord record in reader)
            {
                roads.Add(Convert.ToInt32(record["id"]), record["name"].ToString());
            }
            Close();
            return roads;
        }
        // Проверяем наличие дороги с таким названием
        public static bool NameCheck(string name)
        {
            Connect();
            command = new SQLiteCommand("SELECT * FROM 'Crossroads' WHERE name='" + name + "';", connect);
            reader = command.ExecuteReader();
            bool b = reader.HasRows;
            Close();
            return b;
        }
        // Удаление перекрестка по ИД
        public static void DeleteSelectedId(int id)
        {
            Connect();
            command = new SQLiteCommand("DELETE FROM 'Crossroads' WHERE id="+id, connect);
            command.ExecuteNonQuery();
            Close();
        }
        // Удаление перекрестка по имени
        public static void DeleteSelectedName(string name)
        {
            Connect();
            command = new SQLiteCommand("DELETE FROM 'Crossroads' WHERE name='" + name + "'", connect);
            command.ExecuteNonQuery();
            Close();
        }
        // Загрузка перекрестка по ИД
        public static Road LoadRoadSelectedId(int id)
        {
            Connect();
            command = new SQLiteCommand("SELECT * FROM 'Crossroads' WHERE id=" + id, connect);
            reader = command.ExecuteReader();
            Road r = null;
            foreach (DbDataRecord record in reader)
            {
                r = new Road(Convert.ToInt32(record["polLeft"]), Convert.ToInt32(record["polRight"]), Convert.ToInt32(record["polUp"]), Convert.ToInt32(record["polDown"]));
            }
            Close();
            return r;
        }
        // Загрузка перекрестка по имени
        public static Road LoadRoadSelectedName(string name)
        {
            Connect();
            command = new SQLiteCommand("SELECT * FROM 'Crossroads' WHERE name='" + name + "'", connect);
            reader = command.ExecuteReader();
            Road r = null;
            foreach (DbDataRecord record in reader)
            {
                r = new Road(Convert.ToInt32(record["polLeft"]), Convert.ToInt32(record["polRight"]), Convert.ToInt32(record["polUp"]), Convert.ToInt32(record["polDown"]));
            }
            Close();
            return r;
        }
        // Загрузка пешеходников по имени перекрестка
        public static RoadTransit LoadTransitSelectedName(string name)
        {
            Connect();
            command = new SQLiteCommand("SELECT * FROM 'Crossroads' WHERE name='" + name + "'", connect);
            reader = command.ExecuteReader();
            RoadTransit r = null;
            foreach (DbDataRecord record in reader)
            {
                r = new RoadTransit(Convert.ToBoolean(record["peoUp"]), Convert.ToBoolean(record["peoDown"]), Convert.ToBoolean(record["peoLeft"]), Convert.ToBoolean(record["peoRight"]), Convert.ToInt32(record["polLeft"]), Convert.ToInt32(record["polRight"]), Convert.ToInt32(record["polUp"]), Convert.ToInt32(record["polDown"]));
            }
            Close();
            return r;
        }
        // Загрузка пешеходников по ИД перекрестка
        public static RoadTransit LoadTransitSelectedId(int id)
        {
            Connect();
            command = new SQLiteCommand("SELECT * FROM 'Crossroads' WHERE id=" + id, connect);
            reader = command.ExecuteReader();
            RoadTransit r = null;
            foreach (DbDataRecord record in reader)
            {
                r = new RoadTransit(Convert.ToBoolean(record["peoUp"]), Convert.ToBoolean(record["peoDown"]), Convert.ToBoolean(record["peoLeft"]), Convert.ToBoolean(record["peoRight"]), Convert.ToInt32(record["polLeft"]), Convert.ToInt32(record["polRight"]), Convert.ToInt32(record["polUp"]), Convert.ToInt32(record["polDown"]));
            }
            Close();
            return r;
        }
        // Загрузка интервалов по имени перекрестка
        public static int LoadIntervalName(string name, int i)
        {
            Connect();
            command = new SQLiteCommand("SELECT * FROM 'Crossroads' WHERE name='" + name + "'", connect);
            reader = command.ExecuteReader();
            int r = 30;
            foreach (DbDataRecord record in reader)
            {
                r = Convert.ToInt32(record["int" + i]);
            }
            Close();
            return r;
        }
        // Загрузка интервалов по ИД перекрестка
        public static int LoadIntervalId(int id,int i)
        {
            Connect();
            command = new SQLiteCommand("SELECT * FROM 'Crossroads' WHERE id=" + id, connect);
            reader = command.ExecuteReader();
            int r = 30;
            foreach (DbDataRecord record in reader)
            {
                r =Convert.ToInt32(record["int"+i]);
            }
            Close();
            return r;
        }

        public static void Close()
        {
            connect.Close();
        }
    }
}
