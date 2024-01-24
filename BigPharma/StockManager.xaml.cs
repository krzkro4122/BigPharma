using BigPharmaEngine;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BigPharma
{
    public partial class StockManager : Window
    {
        public ObservableCollection<MedicationModel> AllMedications { get; set; } = new();
        public ObservableCollection<MedicationModel> ShownMedications { get; set; } = new();

        public StockManager()
        {
            InitializeComponent();
            LoadMedicationList();
        }

        private void LoadMedicationList()
        {
            foreach (var medication in SQLiteDataAccess.LoadMedictaions())
            {
                AllMedications.Add(medication);
                ShownMedications.Add(medication);
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
            => Handle_Add_Medication();

        private void DeleteMedication_Click(object sender, RoutedEventArgs e)
            => Handle_Delete_Medication();

        private void UpdateMedication_Click(object sender, RoutedEventArgs e)
            => Handle_Medication_Update();

        private void ResetForm_Click(object sender, RoutedEventArgs e) 
            => Clear_Form_Inputs();

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e) 
            => Filter_Shown_Medications();

        private void UpdateIdTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string content = UpdateIdTextBox.Text;

            if (string.IsNullOrEmpty(content))
            {
                DisableUpdateInputs();
            } else
            {
                EnableUpdateInputs();
            }
        }

        private void Handle_Add_Medication()
        {
            Clear_Warning_Labels();

            int? price = Handle_Price(MedicationPrice.Text);
            int? quantity = Handle_Quantity(MedicationQuantity.Text);
            string name = Handle_Name(MedicationName.Text);
            string description = MedicationDescription.Text;

            bool AnyOfTheFieldsIsInvalid = 
                price == null || 
                quantity == null || 
                name == null;

            if (AnyOfTheFieldsIsInvalid) return;

            AddMedicationInternal(
                new MedicationModel()
                {
                    Name = name,
                    Price = (int) price,
                    Description = description,
                    Quantity = (int) quantity,
                }
            );

            Clear_Form_Inputs();
        }

        private void Handle_Delete_Medication()
        {
            Clear_Form_Inputs();
            DeleteMedicationInternal((MedicationModel)MedicationsDataGrid.SelectedItem);
        }

        private void Handle_Medication_Update()
        {
            try
            {
                int Id = StockUtils.Convert_Numeral(UpdateIdTextBox.Text);
            } catch (FormatException)
            { 
                return;
            }

            var medication = AllMedications.Select(m => m.Id).First();


            Update_Input_Sources();
            Clear_Form_Inputs();
        }

        private void Filter_Shown_Medications()
        {
            ShownMedications.Clear();
            string criterion = SearchBox.Text;

            foreach (var medication in AllMedications)            
                if (StockUtils.Satisfies_Criterion(medication, criterion)) ShownMedications.Add(medication);                            
        }

        private int? Handle_Price(string text)
        {            
            try
            {
                return StockUtils.Convert_Numeral(text);
            } catch (Exception)
            {
                if (string.IsNullOrEmpty(text)) PriceWarningLabel.Content = "Empty price!";
                else PriceWarningLabel.Content = "Wrong format!";
                return null;
            }
        }

        private int? Handle_Quantity(string text)
        {            
            try
            {
                return StockUtils.Convert_Numeral(text);
            }
            catch (Exception)
            {
                if (string.IsNullOrEmpty(text)) QuantityWarningLabel.Content = "Empty quantity!";
                else QuantityWarningLabel.Content = "Wrong format!";
                return null;
            }
        }

        private string Handle_Name(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                NameWarningLabel.Content = "Empty name!";
                return null;
            }
            return text;
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

        private void Update_Input_Sources()
        {
            BindingExpression nameBinding = UpdateNameTextBox.GetBindingExpression(TextBox.TextProperty);
            BindingExpression descriptionBinding = UpdateDescriptionTextBox.GetBindingExpression(TextBox.TextProperty);
            BindingExpression priceBinding = UpdatePriceTextBox.GetBindingExpression(TextBox.TextProperty);
            BindingExpression quantityBinding = UpdateQuantityTextBox.GetBindingExpression(TextBox.TextProperty);                

            nameBinding.UpdateSource();
            descriptionBinding.UpdateSource();
            priceBinding.UpdateSource();
            quantityBinding.UpdateSource();
        }

        private void EnableUpdateInputs()
        {
            UpdateNameTextBox.IsEnabled = true;
            UpdateDescriptionTextBox.IsEnabled = true;
            UpdatePriceTextBox.IsEnabled = true;
            UpdateQuantityTextBox.IsEnabled = true;
        }

        private void DisableUpdateInputs()
        {
            UpdateNameTextBox.IsEnabled = false;
            UpdateDescriptionTextBox.IsEnabled = false;
            UpdatePriceTextBox.IsEnabled = false;
            UpdateQuantityTextBox.IsEnabled = false;
        }

        private void AddMedicationInternal(MedicationModel medication)
        {
            MedicationModel addedMedication = SQLiteDataAccess.SaveMedication
            (
                new MedicationModel()
                {
                    Name = medication.Name,
                    Price = medication.Price,
                    Description = medication.Description,
                    Quantity = medication.Quantity,
                }
            );
            AllMedications.Add(addedMedication);

            string criterion = SearchBox.Text;
            if (StockUtils.Satisfies_Criterion(medication, criterion)) ShownMedications.Add(addedMedication);
        }

        private void DeleteMedicationInternal(MedicationModel medication)
        {
            if (medication != null)
            {
                SQLiteDataAccess.DeleteMedication(medication);
                AllMedications.Remove(medication);
                ShownMedications.Remove(medication);
            }
        }
    }
}
