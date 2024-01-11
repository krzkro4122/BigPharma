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
