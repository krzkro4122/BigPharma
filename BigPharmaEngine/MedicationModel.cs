namespace BigPharmaEngine
{
    public class MedicationModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
    }
}