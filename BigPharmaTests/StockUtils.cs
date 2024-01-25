using BigPharmaEngine;  
using NUnit.Framework;
using System.Collections.ObjectModel;
using BigPharmaEngine.Models;

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

        [Test]
        [TestCase(2, null)]
        public void Test_Find_Medication_Sad(int Id, string Name)
        {
            Assert.Multiple(() =>
            {
                var foundMedication = StockUtils.Find_Medication(Id, medications);

                Assert.That(foundMedication, Is.Null);
                Assert.That(foundMedication?.Name, Is.EqualTo(Name));
            });
        }

        [Test]
        [TestCase("string", "")]
        [TestCase("string", "s")]
        [TestCase("string", "i")]
        [TestCase("string", "g")]
        [TestCase("string", "str")]
        [TestCase("string", "ring")]
        [TestCase("string", "string")]
        public void Test_Property_Contains_Criterion_Happy(string property, string criterion)
        {
            Assert.That(StockUtils.Property_Contains_Criterion(property, criterion), Is.True);
        }

        [Test]
        [TestCase("string", "P")]
        [TestCase("string", "su")]
        [TestCase("string", "substring")]
        [TestCase("string", "kanapka")]
        [TestCase("string", @"\(._.)/")]
        [TestCase("string", "stringa")]
        public void Test_Property_Contains_Criterion_Sad(string property, string criterion)
        {
            Assert.That(StockUtils.Property_Contains_Criterion(property, criterion), Is.False);
        }

        [Test]
        [TestCase("")]
        public void Test_Criterion_Is_Empty_Happy(string criterion)
        {
            Assert.That(StockUtils.Criterion_Is_Empty(criterion), Is.True);
        }

        [Test]
        [TestCase("I am NOT empty, lol")]
        public void Test_Criterion_Is_Empty_Sad(string criterion)
        {
            Assert.That(StockUtils.Criterion_Is_Empty(criterion), Is.False);
        }

        [Test]
        [TestCase("1", 1)]
        [TestCase("-1", -1)]
        [TestCase("0", 0)]
        [TestCase("2147483647", 2147483647)]
        [TestCase("-2147483648", -2147483648)]
        public void Test_Convert_Numeral_Happy(string input, int output)
        {
            Assert.That(StockUtils.Convert_Numeral(input), Is.EqualTo(output));
        }

        [Test]
        [TestCase("2147483648")]
        [TestCase("-2147483649")]
        public void Test_Convert_Numeral_Sad(string input)
        {
            Assert.Throws<OverflowException>(() => StockUtils.Convert_Numeral(input));
        }

        [Test]
        [TestCase("T")]
        [TestCase("Tes")]
        [TestCase("Test0")]
        [TestCase("0")]
        [TestCase("st")]
        [TestCase("e")]
        [TestCase("")]
        public void Test_Satisfies_Criterion_Happy(string criterion)
        {
            Assert.That(StockUtils.Satisfies_Criterion(medications[0], criterion), Is.True);
        }

        [Test]
        [TestCase("Test0LOL")]
        [TestCase("Ziemniak")]
        [TestCase("Teft0")]
        [TestCase("Z")]
        [TestCase("1")]
        public void Test_Satisfies_Criterion_Sad(string criterion)
        {
            Assert.That(StockUtils.Satisfies_Criterion(medications[0], criterion), Is.False);
        }
    }
}