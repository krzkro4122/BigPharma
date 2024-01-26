using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using BigPharmaEngine.Models;

namespace BigPharma.Components
{
    public partial class MedicationBrowserComponent
    {
        private static readonly DependencyProperty MedicationsDependencyProperty =
            DependencyProperty.Register(nameof(Medications),
                typeof(ObservableCollection<MedicationModel>),
                typeof(MedicationBrowserComponent));

        private static readonly DependencyProperty MedicationClickedDependencyProperty =
            DependencyProperty.Register(
                nameof(MedicationClicked),
                typeof(Action<MedicationModel>),   
                typeof(MedicationBrowserComponent));

        
        public ObservableCollection<MedicationModel> Medications
        {
            get => (ObservableCollection<MedicationModel>)GetValue(MedicationsDependencyProperty);
            set => SetValue(MedicationsDependencyProperty, value);
        }

        public Action<MedicationModel> MedicationClicked
        {
            get => (Action<MedicationModel>)GetValue(MedicationClickedDependencyProperty);
            set => SetValue(MedicationClickedDependencyProperty, value);
        }

        public MedicationBrowserComponent()
        {
            InitializeComponent();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateShownMedications(SearchBox.Text);
        }

        private void UpdateShownMedications(string criterion)
        {
            var view = CollectionViewSource.GetDefaultView(Medications);
            if (view != null)
            {
                view.Filter = item =>
                {
                    var medicationModel = item as MedicationModel;
                    var isFilterSet = criterion.Length != 0;
                    if (!isFilterSet)
                    {
                        return true;
                    }
                    var nameContainsCriterion = false;
                    var descriptionContainsCriterion = false;
                    if (medicationModel?.Description is not null)
                    {
                        descriptionContainsCriterion = medicationModel.Description.Contains(criterion);
                    }
                    if (medicationModel?.Name is not null)
                    {
                        nameContainsCriterion = medicationModel.Name.Contains(criterion);
                    }
                    return nameContainsCriterion ||
                           descriptionContainsCriterion;
                };
            }
        }

        private void MedicationsDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems is null || e.AddedItems.Count == 0)
            {
                return;
            }
            if(e.AddedItems[0] is not MedicationModel medication) return;
            MedicationClicked(medication);
        }
    }
}
