namespace BigPharmaEngine.Models;

public class OrderModel : IModelCopyable<OrderModel>
{
    public int Id { get; set; }
    public int MedicationId { get; set; }
    public int TransactionId { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
    public OrderStatus Status { get; set; }
    public OrderModel Copy()
    {
        return new OrderModel
        {
            Id = Id,
            Price = Price,
            Status = Status,
            MedicationId = MedicationId,
            TransactionId = TransactionId,
            Quantity = Quantity,
        };
    }
}