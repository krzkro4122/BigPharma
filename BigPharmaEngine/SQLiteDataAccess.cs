using Dapper;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;

namespace BigPharmaEngine
{
    public class SQLiteDataAccess
    {
        public static ObservableCollection<MedicationModel> LoadMedictaions()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<MedicationModel>("SELECT * FROM Medications", new DynamicParameters());
                var outputList = output.ToList();
                return Convert(output);
            }
        }

        public static MedicationModel SaveMedication(MedicationModel medication)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.QuerySingle<MedicationModel>
                (
                    "INSERT INTO Medications (Name, Price, Quantity, Description) values (@Name, @Price, @Quantity, @Description) RETURNING *", 
                    medication
                );
                return output;
            }
        }

        public static MedicationModel UpdateMedication(MedicationModel medication)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.QuerySingle<MedicationModel>
                (
                    "UPDATE Medications SET Name = @Name, Price = @Price, Quantity = @Quantity, Description = @Description WHERE Id=@Id RETURNING *",
                    medication
                );
                return output;
            }
        }

        public static void DeleteMedication(MedicationModel medication)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute
                (
                    "DELETE FROM Medications WHERE Id=@Id",
                    medication
                );
            }
        }
        public static ObservableCollection<User> LoadUsers()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<User>("SELECT * FROM Users", new DynamicParameters());
                var outputList = output.ToList();
                return ConvertUser(output);
            }
        }
        public static User SaveUser(User user)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.QuerySingle<User>
                (
                                       "INSERT INTO Users (Username, Email, Password) values (@Username, @Email, @Password) RETURNING *",
                                                          user
                                                                         );
                return output;
            }
        }
        public static void DeleteUser(User user)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute
                (
                                       "DELETE FROM Users WHERE Id=@Id",
                                                          user
                                                                         );
            }
        }
        private static string LoadConnectionString()
        {
            return """Data Source=".\data.db";Version=3;""";
        }

        public static MedicationModel GetMedicationWithHighestStock()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                // Query to get the medication with the highest stock
                var output = cnn.QuerySingle<MedicationModel>("SELECT * FROM Medications ORDER BY Quantity DESC LIMIT 1", new DynamicParameters());
                return output;
            }
        }
        public static ObservableCollection<MedicationModel> Convert(IEnumerable original)
        {
            return new ObservableCollection<MedicationModel>(original.Cast<MedicationModel>());
        }
        public static ObservableCollection<User> ConvertUser(IEnumerable original)
        {
            return new ObservableCollection<User>(original.Cast<User>());
        }


    }
}
