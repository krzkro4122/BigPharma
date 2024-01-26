using Dapper;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SQLite;
using BigPharmaEngine.Models;

namespace BigPharmaEngine;

public class SQLiteDataAccess
{
    public static ObservableCollection<MedicationModel> LoadMedictaions()
    {
        using IDbConnection cnn = new SQLiteConnection(LoadConnectionString());
        var output = cnn.Query<MedicationModel>("SELECT * FROM Medications", new DynamicParameters());
        return new ObservableCollection<MedicationModel>(output);
    }
        
    public static MedicationModel SaveMedication(MedicationModel medication)
    {
        using IDbConnection cnn = new SQLiteConnection(LoadConnectionString());
        var output = cnn.QuerySingle<MedicationModel>
        (
            "INSERT INTO Medications (Name, Price, Quantity, Description) values (@Name, @Price, @Quantity, @Description) RETURNING *", 
            medication
        );
        return output;
    }

    public static MedicationModel UpdateMedication(MedicationModel medication)
    {
        using IDbConnection cnn = new SQLiteConnection(LoadConnectionString());
        var output = cnn.QuerySingle<MedicationModel>
        (
            "UPDATE Medications SET Name = @Name, Price = @Price, Quantity = @Quantity, Description = @Description WHERE Id=@Id RETURNING *",
            medication
        );
        return output;
    }

    public static void DeleteMedication(MedicationModel medication)
    {
        using IDbConnection cnn = new SQLiteConnection(LoadConnectionString());
        cnn.Execute
        (
            "DELETE FROM Medications WHERE Id=@Id",
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
        using IDbConnection cnn = new SQLiteConnection(LoadConnectionString());
        var output = cnn.QuerySingle<OrderModel>
        (
            "INSERT INTO Orders (Price, Status, Quantity, MedicationId, TransactionId) values (@Price, @Status, @Quantity, @MedicationId, @TransactionId) RETURNING *", 
            order
        );
        return output;
    }

    public static void EditOrder(OrderModel order)
    {
        using IDbConnection cnn = new SQLiteConnection(LoadConnectionString());
        cnn.Execute(
            "UPDATE ORDERS SET STATUS = @Status, Quantity = @Quantity, Price = @Price  WHERE ID = @Id",
            order
        );
    }

    public static void DeleteOrder(OrderModel order)
    {
        using IDbConnection cnn = new SQLiteConnection(LoadConnectionString());
        cnn.Execute
        (
            "DELETE FROM Orders WHERE Id=@Id",
            order
        );
    }

    public static TransactionModel SaveTransaction(TransactionModel newTransaction)
    {
        using IDbConnection cnn = new SQLiteConnection(LoadConnectionString());
        return cnn.QuerySingle<TransactionModel>(
            "INSERT INTO TRANSACTIONS (CompletionDate) values (@CompletionDate) RETURNING *",
            newTransaction
        );
    }
    
    
    public static ObservableCollection<User> LoadUsers()
    {
        using IDbConnection cnn = new SQLiteConnection(LoadConnectionString());
        var output = cnn.Query<User>("SELECT * FROM Users", new DynamicParameters());
        return new ObservableCollection<User>(output);
    }
        
    public static User SaveUser(User user)
    {
        using IDbConnection cnn = new SQLiteConnection(LoadConnectionString());
        var output = cnn.QuerySingle<User>
        (
            "INSERT INTO Users (Username, Email, Password) values (@Username, @Email, @Password) RETURNING *",
            user
        );
        return output;
    }
        
    public static void DeleteUser(User user)
    {
        using IDbConnection cnn = new SQLiteConnection(LoadConnectionString());
        cnn.Execute
        (
            "DELETE FROM Users WHERE Id=@Id",
            user
        );
    }
        
        
    private static string LoadConnectionString()
    {
        return """Data Source=".\data.db";Version=3;""";
    }
    public static int GetTotalOrderValue()
    {
        using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
        {
            // Query to get the sum of all order values
            var output = cnn.ExecuteScalar<int>("SELECT SUM(Price) FROM Orders");
            return output;
        }
    }
    public static string GetMostOrderedMedicationName()
    {
        using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
        {
            // Query to get the name of the medication that is ordered most often
            var output = cnn.ExecuteScalar<string>("SELECT Medications.Name FROM Medications " +
                                                  "JOIN Orders ON Medications.Id = Orders.MedicationId " +
                                                  "GROUP BY Medications.Id " +
                                                  "ORDER BY SUM(Orders.Quantity) DESC " +
                                                  "LIMIT 1");
            return output;
        }
    }
    public static int GetHighestOrderValue()
    {
        using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
        {
            // Query to get the value of the highest order
            var output = cnn.ExecuteScalar<int>("SELECT MAX(Price) FROM Orders");
            return output;
        }
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
}