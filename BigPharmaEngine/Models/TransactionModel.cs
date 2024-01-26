namespace BigPharmaEngine.Models;

public class TransactionModel
{
    public int Id { get; set; }
    public List<OrderModel> OrderModels { get; set; }
    public DateTime? CompletionDate { get; set; }
}