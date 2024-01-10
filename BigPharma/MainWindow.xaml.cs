using BigPharmaEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace BigPharma
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<MedicationModel> AllMedications { get; set; } = new();
        public ObservableCollection<MedicationModel> ShownMedications { get; set; } = new();
        public string PlaceholderText { get; set; }

        public MainWindow()
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

        private void AddMedication_Click(object sender, RoutedEventArgs e)
        {
            Clear_Warning_Labels();

            int price = Parse_Price(MedicationPrice.Text);
            string name = Parse_Name(MedicationName.Text);

            bool priceAndNameAreInvalid = price == -1 || name == "-1";
            if (priceAndNameAreInvalid) return;

            MedicationModel addedMedication = SQLiteDataAccess.SaveMedication
            (
                new MedicationModel()
                {
                    Name = name,
                    Price = price
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
                AllMedications.Remove(selectedMedication);
            }
        }

        private void ResetForm_Click(object sender, RoutedEventArgs e)
        {
            Clear_Form_Inputs();
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
                return Convert_Price(priceText);
            } catch (Exception)
            {
                PriceWarningLabel.Content = "Wrong price!";
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
        
        private int Convert_Price(string priceText)
        {
            return Int32.Parse(priceText);
        }

        private void Clear_Warning_Labels()
        {
            PriceWarningLabel.Content = "";
            NameWarningLabel.Content = "";
        }

        private void Clear_Form_Inputs()
        {
            MedicationName.Text = "";
            MedicationPrice.Text = "";
        }

        private void AddMedicationInternal(MedicationModel medication)
        {
            bool theresNoSearchBoxCriterion = SearchBox.Text.Length == 0;
            if (theresNoSearchBoxCriterion)
            {
                AllMedications.Add(medication);
                ShownMedications.Add(medication);
            }
        }
    }
}
