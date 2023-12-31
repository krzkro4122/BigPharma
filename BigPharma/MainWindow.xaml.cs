﻿using BigPharmaEngine;
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
        public ObservableCollection<MedicationModel> medications { get; set; } = new();    

        public MainWindow()
        {
            InitializeComponent();
            LoadMedicationList();
        }

        private void LoadMedicationList()
        {
            foreach (var medication in SQLiteDataAccess.LoadMedictaions())
            {
                medications.Add(medication);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Clear_Warning_Labels();

            int price = Parse_Price(MedicationPrice.Text);
            string name = Parse_Name(MedicationName.Text);

            bool priceAndNameAreInvalid = price == -1 || name == "-1";
            if (priceAndNameAreInvalid) return;
            
            medications.Add(
                SQLiteDataAccess.SaveMedication
                (
                    new MedicationModel()
                    {
                        Name = name,
                        Price = price
                    }
                )
            );

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

        private void DeleteMedication_Click(object sender, RoutedEventArgs e)
        {
            MedicationModel selectedMedication = (MedicationModel) MedicationsDataGrid.SelectedItem;
            if (selectedMedication != null)
            {                
                // Not atomic, whatever
                SQLiteDataAccess.DeleteMedication(selectedMedication);
                medications.Remove(selectedMedication);
            }
        }
    }
}
