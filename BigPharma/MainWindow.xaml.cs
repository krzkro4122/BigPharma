using BigPharmaEngine;
using System;
using System.Collections.Generic;
using System.Windows;

namespace BigPharma
{
    public partial class MainWindow : Window
    {
        public List<MedicationModel> medications { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();

            LoadMedicationList();
        }

        private void LoadMedicationList()
        {
            medications = SQLiteDataAccess.LoadMedictaions();
            MedicationsDataGrid.ItemsSource = medications;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string priceText = MedicationPrice.Text;

            if (priceText == "")
            {
                return;
            }

            MedicationModel medication = new()
            {
                Name = MedicationName.Text,
                Price = Int32.Parse(priceText)
            };

            SQLiteDataAccess.SaveMedication(medication);

            MedicationName.Text = "";
            MedicationPrice.Text = "";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LoadMedicationList();
        }
    }
}
