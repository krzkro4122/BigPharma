using BigPharmaEngine;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace BigPharma
{
    public partial class Summaries : Window
    {
        public Summaries()
        {
            InitializeComponent();
            UpdateFieldsWithHighestStock();
        }


        private void OnClosing(object sender, CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
        private void UpdateFieldsWithHighestStock()
        {
            try
            {
                // Get the medication with the highest stock
                MedicationModel medicationWithHighestStock = SQLiteDataAccess.GetMedicationWithHighestStock();

                // Format the string
                string displayText = $"Most Popular Medication: {medicationWithHighestStock.Name}, we have {medicationWithHighestStock.Quantity} units for the measly price of {medicationWithHighestStock.Price:C} per unit";

                // Update the TextBlock with the formatted string
                Field2Value.Text = displayText;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log or display an error message)
                Debug.WriteLine("Error updating fields with highest stock: " + ex.Message);
            }
        }
        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {


            // Close the window
            Close();
        }
    }
}
