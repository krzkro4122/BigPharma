using Dapper;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using BigPharmaEngine.Models;

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

        public static void EditMedication(MedicationModel medication)
        {
            using IDbConnection cnn = new SQLiteConnection(LoadConnectionString());
            cnn.Execute(
                "UPDATE MEDICATIONS SET Name = @Name, Quantity = @Quantity, Price = @Price, Description = @Description  WHERE ID = @Id",
                medication
            );
        }

        public static IEnumerable<OrderModel> LoadOrders()
        {
            using IDbConnection cnn = new SQLiteConnection(LoadConnectionString());
            var output = cnn.Query<OrderModel>("SELECT * FROM Orders", new DynamicParameters());
            return output;
        }
        
        public static OrderModel SaveOrder(OrderModel order)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                var output = cnn.QuerySingle<OrderModel>
                (
                    "INSERT INTO Orders (Price, Status, MedicationId, Quantity, CreationDate, CompletionDate) values (@Price, @Status, @MedicationId, @Quantity, @CreationDate, @CompletionDate) RETURNING *", 
                    order
                );
                return output;
            }
        }

        public static void EditOrder(OrderModel order)
        {
            using IDbConnection cnn = new SQLiteConnection(LoadConnectionString());
            cnn.Execute(
                "UPDATE ORDERS SET STATUS = @Status, Quantity = @Quantity, Price = @Price, CompletionDate = @CompletionDate  WHERE ID = @Id",
                order
            );
        }

        public static void DeleteOrder(OrderModel order)
        {
            using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
            {
                cnn.Execute
                (
                    "DELETE FROM Orders WHERE Id=@Id",
                    order
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
