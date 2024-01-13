using BigPharmaEngine;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace BigPharma
{
    public partial class StockManager : Window
    {
        public ObservableCollection<MedicationModel> AllMedications { get; set; } = new();
        public ObservableCollection<MedicationModel> ShownMedications { get; set; } = new();
        public string PlaceholderText { get; set; }

        #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public StockManager()
        #pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            InitializeComponent();
            LoadMedicationList();
        }

        private void LoadMedicationList()
        {
            foreach (var medication in SQLiteDataAccess.LoadMedictaions())
            {
                AddMedicationInternal(medication);
            }
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (sender.GetType() != typeof(MainWindow)) 
            { 
                this.Hide();
                e.Cancel = true;
            }
        }

        private void AddMedication_Click(object sender, RoutedEventArgs e)
        {
            Clear_Warning_Labels();

            int price = Parse_Price(MedicationPrice.Text);
            int quantity = Parse_Quantity(MedicationQuantity.Text);
            string name = Parse_Name(MedicationName.Text);
            string description = Parse_Name(MedicationDescription.Text);

            bool priceAndNameAreInvalid = price == -1 || name == "-1";
            if (priceAndNameAreInvalid) return;

            MedicationModel addedMedication = SQLiteDataAccess.SaveMedication
            (
                new MedicationModel()
                {
                    Name = name,
                    Price = price,
                    Description = description,
                    Quantity = quantity,
                }
            );

            AddMedicationInternal(addedMedication);

            Clear_Form_Inputs();
        }

        private void DeleteMedication_Click(object sender, RoutedEventArgs e)
        {
            MedicationModel selectedMedication = (MedicationModel)MedicationsDataGrid.SelectedItem;
            if (selectedMedication != null)
            {
                // Not atomic, whatever
                SQLiteDataAccess.DeleteMedication(selectedMedication);
                DeleteMedicationInternal(selectedMedication);
            }
        }

        private void ResetForm_Click(object sender, RoutedEventArgs e)
        {
            Clear_Form_Inputs();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShownMedications.Clear();

            string criterion = SearchBox.Text;
            bool theresNoCriterion = criterion.Length == 0;

            foreach (var medication in AllMedications)
            {
                bool containsCrtierion = medication.Name.ToLower().Contains(criterion.ToLower());
                if (theresNoCriterion || containsCrtierion)
                {
                    ShownMedications.Add(medication);
                }
            }
        }

        private int Parse_Price(string priceText)
        {
            if (string.IsNullOrEmpty(priceText))
            {
                PriceWarningLabel.Content = "Empty price!";
                return -1;
            }

            try
            {
                return Convert_Numeral(priceText);
            } catch (Exception)
            {
                PriceWarningLabel.Content = "Wrong price!";
                return -1;
            }
        }

        private int Parse_Quantity(string priceText)
        {
            if (string.IsNullOrEmpty(priceText))
            {
                QuantityWarningLabel.Content = "Empty price!";
                return -1;
            }

            try
            {
                return Convert_Numeral(priceText);
            }
            catch (Exception)
            {
                QuantityWarningLabel.Content = "Wrong price!";
                return -1;
            }
        }

        private string Parse_Name(string nameText)
        {
            if (string.IsNullOrEmpty(nameText))
            {
                NameWarningLabel.Content = "Empty name!";
                return "-1";
            }

            return nameText;
        }

        private int Convert_Numeral(string priceText)
        {
            return Int32.Parse(priceText);
        }

        private void Clear_Warning_Labels()
        {
            PriceWarningLabel.Content = "";
            NameWarningLabel.Content = "";
            QuantityWarningLabel.Content = "";
            DescriptionWarningLabel.Content = "";
        }

        private void Clear_Form_Inputs()
        {
            MedicationName.Text = "";
            MedicationPrice.Text = "";
            MedicationQuantity.Text = "";
            MedicationDescription.Text = "";
        }

        private void AddMedicationInternal(MedicationModel medication)
        {
            string criterion = SearchBox.Text;
            bool theresNoSearchBoxCriterion = criterion.Length == 0;
            bool satisfiesCrtierion = medication.Name.ToLower().Contains(criterion.ToLower());

            if (theresNoSearchBoxCriterion || satisfiesCrtierion)
            {
                AllMedications.Add(medication);
                ShownMedications.Add(medication);
            } else if (!satisfiesCrtierion)
            {
                AllMedications.Add(medication);
            }            
        }

        private void DeleteMedicationInternal(MedicationModel medication)
        {
            AllMedications.Remove(medication);
            ShownMedications.Remove(medication);
        }
    }
}
