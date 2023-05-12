using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using Dapper;

namespace HabitLogger
{
    public class SqliteDataAccess
    {
        public string Path { get; set; }


        public static List<HabitModel> LoadHabits()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<HabitModel>("select * from Habits", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void SaveHabit(HabitModel habit)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("insert into Habits(HabitName,Quantity,Units) values (@HabitName,@Quantity,@Units)", habit);
            }
        }
        public static void UpdateHabit(HabitModel habit)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("update Habits set HabitName = @HabitName, Quantity = @Quantity, Units = @Units where Id = @id", habit);
            }
        }
        public static void DeleteHabit(HabitModel habit)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute("delete from Habits where Id = @id", habit);
            }
        }

        private static string LoadConnectionString(string id = "Default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
