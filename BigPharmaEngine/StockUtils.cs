using System.Collections.ObjectModel;

namespace BigPharmaEngine
{
    public class StockUtils
    {
        public static bool Satisfies_Criterion(MedicationModel medication, string criterion)
        {
            bool theresNoCriterion = Criterion_Is_Empty(criterion);
            bool descriptionContainsCrtierion = Property_Contains_Criterion(medication.Description, criterion);
            bool nameContainsCrtierion = Property_Contains_Criterion(medication.Name, criterion);

            if (theresNoCriterion || nameContainsCrtierion || descriptionContainsCrtierion) return true;
            else return false;
        }

        public static int Convert_Numeral(string priceText) => Int32.Parse(priceText);

        private static bool Criterion_Is_Empty(string criterion) => criterion.Length == 0;        

        private static bool Property_Contains_Criterion(string Property, string criterion)
            => Property.ToLower().Contains(criterion.ToLower());        

        public static MedicationModel? Find_Medication(int Id, Collection<MedicationModel> medications)
        {
            foreach (var medication in medications)            
                if (medication.Id == Id) 
                    return medication;             
            return null;
        }
    }
}
