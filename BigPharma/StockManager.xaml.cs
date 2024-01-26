using BigPharmaEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using BigPharmaEngine.Models;

namespace BigPharma
{
    public sealed partial class StockManager : INotifyPropertyChanged
    {
        private MedicationModel? selectedMedication;
        public MedicationModel? SelectedMedication
        {
            get => selectedMedication; 
            private set => SetField(ref selectedMedication, value);
        }
        public ObservableCollection<MedicationModel> AllMedications { get; set; } = new();

        public StockManager()
        {
            InitializeComponent();
            LoadMedicationList();
            SelectionChangedHandler = medication =>
            {
                SelectedMedication = medication.Copy();
            };
        }

        private void LoadMedicationList()
        {
            AllMedications.Clear();

            foreach (var medication in SQLiteDataAccess.LoadMedictaions())
            {
                AllMedications.Add(medication);
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
            string? name = Handle_Name(MedicationName.Text);
            string description = MedicationDescription.Text;

            bool anyOfTheFieldsIsInvalid = 
                price is null || 
                quantity is null || 
                name is null;

            if (anyOfTheFieldsIsInvalid) return;

            AddMedicationInternal(
                new MedicationModel
                {
                    Name = name!,
                    Price = (int) price! ,
                    Description = description,
                    Quantity = (int) quantity!,
                }
            );

            Clear_Form_Inputs();
        }

        private void Handle_Delete_Medication()
        {
            Clear_Form_Inputs();
            DeleteMedicationInternal(SelectedMedication);
            LoadMedicationList();
        }
        
        private void Handle_Medication_Update()
        {
            try
            {
                ValidateInputSources();
            }
            catch (Exception)
            {
                return;
            }
            UpdateMedicationInternal(SelectedMedication);
            LoadMedicationList();
            Clear_Form_Inputs();
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

        private string? Handle_Name(string text)
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
            var addedMedication = SQLiteDataAccess.SaveMedication
            (
                new MedicationModel
                {
                    Name = medication.Name,
                    Price = medication.Price,
                    Description = medication.Description,
                    Quantity = medication.Quantity,
                }
            );
            AllMedications.Add(addedMedication);
        }

        private static void UpdateMedicationInternal(MedicationModel? patch)
        {
            if(patch is null) return;
            SQLiteDataAccess.UpdateMedication(patch);
        }
        
        private void ValidateInputSources()
        {
            var nameBinding = UpdateNameTextBox.GetBindingExpression(TextBox.TextProperty);
            var descriptionBinding = UpdateDescriptionTextBox.GetBindingExpression(TextBox.TextProperty);
            var priceBinding = UpdatePriceTextBox.GetBindingExpression(TextBox.TextProperty);
            var quantityBinding = UpdateQuantityTextBox.GetBindingExpression(TextBox.TextProperty);                
            
            nameBinding?.UpdateSource();
            descriptionBinding?.UpdateSource();
            priceBinding?.UpdateSource();
            quantityBinding?.UpdateSource();
            
            if (nameBinding?.ValidationErrors is not null || descriptionBinding?.ValidationErrors is not null || 
                priceBinding?.ValidationErrors is not null || quantityBinding?.ValidationErrors is not null)
            {
                throw new Exception("Validation not valid.");
            }
        }

        
        private void DeleteMedicationInternal(MedicationModel? medication)
        {
            if (medication == null) return;
            SQLiteDataAccess.DeleteMedication(medication);
            SelectedMedication = null;
        }

        private Action<MedicationModel>? selectionChangedHandler;
        public Action<MedicationModel>? SelectionChangedHandler { get => selectionChangedHandler; set => SetField(ref selectionChangedHandler, value); }
        
        /// <summary>
        /// INotifyPropertyChanged stuff
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return;
            field = value;
            OnPropertyChanged(propertyName);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            LoadMedicationList();
        }
    }
}
