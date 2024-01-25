namespace BigPharmaEngine.Models;

public class OrderModel : IModelCopyable<OrderModel>
{
    public int Id { get; set; }
    public int Price { get; set; }
    public OrderStatus Status { get; set; }
    public int MedicationId { get; set; }
    public int Quantity { get; set; }
    public DateTime? CreationDate { get; set; }
    public DateTime? CompletionDate { get; set; }
    public OrderModel Copy()
    {
        return new OrderModel
        {
            Id = Id,
            Price = Price,
            Status = Status,
            MedicationId = MedicationId,
            Quantity = Quantity,
            CreationDate = CreationDate,
            CompletionDate = CompletionDate
        };
    }
}