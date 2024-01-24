using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using BigPharmaEngine;

namespace BigPharma.Components
{
    public partial class MedicationBrowserComponent : UserControl
    {
        private static readonly DependencyProperty medicationsDependencyProperty =
            DependencyProperty.Register(nameof(Medications),
                typeof(ObservableCollection<MedicationModel>),
                typeof(MedicationBrowserComponent));

        private static readonly DependencyProperty medicationClickedDependencyProperty =
            DependencyProperty.Register(
                nameof(MedicationClicked),
                typeof(Action<MedicationModel>),   
                typeof(MedicationBrowserComponent));

        
        public ObservableCollection<MedicationModel> Medications
        {
            get => (ObservableCollection<MedicationModel>)GetValue(medicationsDependencyProperty);
            set => SetValue(medicationsDependencyProperty, value);
        }

        public Action<MedicationModel> MedicationClicked
        {
            get => (Action<MedicationModel>)GetValue(medicationClickedDependencyProperty);
            set => SetValue(medicationClickedDependencyProperty, value);
        }

        public string SearchBarText { get; set; }

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
                    
                    var nameContainsCriterion = medicationModel?.Description?.Contains(criterion) ?? false;
                    var descriptionContainsCriterion = medicationModel?.Name?.Contains(criterion) ?? false;
                    
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
            var medication = e.AddedItems[0] as MedicationModel;
            MedicationClicked(medication);
        }
    }
}
