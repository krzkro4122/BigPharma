using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using BigPharmaEngine;

namespace BigPharma
{
    public partial class SaleManager : Window
    {
        public ObservableCollection<MedicationModel> AllMedications { get; set; } = new();
        public MedicationModel SelectedMedication { get; set; }
        public ObservableCollection<OrderModel> AllOrders { get; set; } = new();
        public OrderModel SelectedModel { get; set; }
        
        public SaleManager()
        {
            InitializeComponent();
            LoadMedicationList();
            LoadOrderList();
        }

        private void LoadMedicationList()
        {
            foreach (var medication in SQLiteDataAccess.LoadMedictaions())
            {
                AllMedications.Add(medication);
            }
        }

        private void LoadOrderList()
        {
            //TODO: Populate orders
        }

        public Action<MedicationModel> MedicationClickedHandler => medication =>
        {
            return;
        };

        public Action<OrderModel> OrderClickedHandler { get; }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (sender.GetType() != typeof(MainWindow))
            {
                this.Hide();
                e.Cancel = true;
            }
        }
    }
}
