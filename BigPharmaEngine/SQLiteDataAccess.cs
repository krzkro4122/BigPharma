using Dapper;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;

namespace BigPharmaEngine
{
    public class SQLiteDataAccess
    {
        public static List<MedicationModel> LoadMedictaions()
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.Query<MedicationModel>("SELECT * FROM Medications", new DynamicParameters());
                var outputList = output.ToList();
                foreach (var item in outputList)
                {
                    Debug.WriteLine(item);                  
                }
                return output.ToList();
            }
        }

        public static void SaveMedication(MedicationModel medication)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute
                (
                    "INSERT INTO Medications (Name, Price) values (@Name, @Price)", 
                    medication
                );
                Debug.WriteLine("Did it");
            }
        }

        private static string LoadConnectionString()
        {
            return """Data Source=".\data.db";Version=3;""";
        }
    }
}
