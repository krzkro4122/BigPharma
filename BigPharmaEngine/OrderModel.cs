namespace BigPharmaEngine;

public class OrderModel
{
    public int Id { get; set; }
    public int Price { get; set; }
    public OrderStatus Status { get; set; }
    public int MedicationId { get; set; }
    public int Quantity { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime CompletionDate { get; set; }
}