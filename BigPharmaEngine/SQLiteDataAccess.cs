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
                foreach (var item in outputList)
                {
                    Debug.WriteLine(item);                  
                }
                return Convert(output);
            }
        }

        public static MedicationModel SaveMedication(MedicationModel medication)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute
                (
                    "INSERT INTO Medications (Name, Price) values (@Name, @Price)", 
                    medication
                );
                var output = cnn.Query<MedicationModel>(
                    "SELECT * FROM Medications WHERE Name=@Name",
                    medication
                );
                return output.First();
            }
        }

        private static string LoadConnectionString()
        {
            return """Data Source=".\data.db";Version=3;""";
        }

        public static ObservableCollection<MedicationModel> Convert(IEnumerable original)
        {
            return new ObservableCollection<MedicationModel>(original.Cast<MedicationModel>());
        }
    }
}
