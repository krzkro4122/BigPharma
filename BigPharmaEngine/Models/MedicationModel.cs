namespace BigPharmaEngine.Models
{
    public class MedicationModel : IModelCopyable<MedicationModel>
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public MedicationModel Copy()
        {
            return new MedicationModel
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Price = Price,
                Quantity = Quantity
            };
        }
    }
}