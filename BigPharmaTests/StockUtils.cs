using BigPharmaEngine;  
using NUnit.Framework;
using System.Collections.ObjectModel;

namespace BigPharmaTests
{
    public class Tests
    {
        Collection<MedicationModel> medications = new();

        [SetUp]
        public void Setup()
        {
            medications.Add(
                new MedicationModel
                {
                    Id = 0,
                    Price = 0,
                    Quantity = 0,
                    Name = "Test0",
                    Description = "Test0"
                }
            );
            medications.Add(
                new MedicationModel
                {
                    Id = 1,
                    Price = 1,
                    Quantity = 1,
                    Name = "Test1",
                    Description = "Test1"
                }
            );
        }

        [Test]
        [TestCase(0, "Test0")]
        [TestCase(1, "Test1")]
        public void Test_Find_Medication_Happy(int Id, string Name)
        {
            Assert.Multiple(() =>
            {
                var foundMedication = StockUtils.Find_Medication(Id, medications);

                Assert.That(foundMedication, Is.Not.Null);
                Assert.That(foundMedication?.Id, Is.EqualTo(Id));
                Assert.That(foundMedication?.Name, Is.EqualTo(Name));
            });
        }

    }
}