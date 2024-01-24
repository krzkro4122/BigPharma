using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using BigPharmaEngine;

namespace BigPharma
{
    public partial class SaleManager : Window
    {
        public ObservableCollection<MedicationModel> AllMedications { get; set; } = new();
        public MedicationModel SelectedMedication { get; set; }
        public ObservableCollection<OrderModel> AllOrders { get; set; } = new();
        public OrderModel SelectedOrder { get; set; }

        public int QuantityToOrder { get; set; } = 0;
        
        public SaleManager()
        {
            InitializeComponent();
            LoadMedicationList();
            LoadOrderList();
            MedicationClickedHandler = medication => SelectedMedication = medication;
            OrderClickedHandler = order => SelectedOrder = order;
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
            var orders = SQLiteDataAccess.LoadOrders();
            foreach (var order in orders)
            {
                AllOrders.Add(order);
            }
        }

        public Action<MedicationModel> MedicationClickedHandler { get; private set; }

        public Action<OrderModel> OrderClickedHandler { get; private set; }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (sender.GetType() != typeof(MainWindow))
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Complete_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CreateOrder_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
