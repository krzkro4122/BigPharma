using BigPharmaEngine;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using BigPharmaEngine.Models;

namespace BigPharma
{
    public partial class Summaries
    {
        public Summaries()
        {
            InitializeComponent();
            UpdateFields();
        }

        private void UpdateFields()
        {
            MostPopularMedication();
            UpdateFieldsWithHighestStock();
            TotalOrderValue();
            HighestOrderValue();
        }
        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (sender.GetType() != typeof(MainWindow))
            {
                this.Hide();
                e.Cancel = true;
            }
        }
        private void MostPopularMedication()
        {
            try
            {
                // Get the medication with the highest order count
                string mostPopular = SQLiteDataAccess.GetMostOrderedMedicationName();

                // Format the string
                string displayText = $"Ależ ma branie ten  {mostPopular}";

                // Update the TextBlock with the formatted string
                Field1Value.Text = displayText;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log or display an error message)
                Debug.WriteLine("Error updating fields with highest stock: " + ex.Message);
            }
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
        private void TotalOrderValue()
        {
            try
            {
               
                int value = SQLiteDataAccess.GetTotalOrderValue();

                // Format the string
                string displayText = $"BigPharma has {value} zł in our bank account, stonks!";

                // Update the TextBlock with the formatted string
                Field3Value.Text = displayText;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log or display an error message)
                Debug.WriteLine("Error updating fields with highest stock: " + ex.Message);
            }
        }
        private void HighestOrderValue()
        {
            try
            {
                // Get the value of the highest order
                decimal highestOrderValue = SQLiteDataAccess.GetHighestOrderValue();

                // Format the string
                string displayText = $"Ktoś się obkupił! Wziął leków za : {highestOrderValue} zł";

                // Update the TextBlock with the formatted string
                Field4Value.Text = displayText;
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
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateFields();
        }
    }
}
