using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using BigPharmaEngine;
using BigPharmaEngine.Models;
using BigPharmaEngine.Models.Extensions;

namespace BigPharma
{
    public partial class SaleManager : Window, INotifyPropertyChanged
    {
        public ObservableCollection<MedicationModel> AllMedications { get; set; } = new();
        public MedicationModel SelectedMedication { get; set; } = null;
        public ObservableCollection<OrderModel> AllOrders { get; set; } = new();
        public OrderModel SelectedOrder { get; set; } = null;

        private int quantityToOrder = 0;
        public int QuantityToOrder
        {
            get => quantityToOrder;
            set
            {
                SetField(ref quantityToOrder, value);
                OnPropertyChanged(nameof(newOrderButtonsAvailability));
            }
        }

        private bool newOrderButtonsAvailability = false;
        public bool NewOrderButtonsAvailability
        {
            get
            {
                if (SelectedMedication is null)
                {
                    return false;
                }
                
                return SelectedMedication.Quantity >= QuantityToOrder;
            }
            set => SetField(ref newOrderButtonsAvailability, value);
        }

        private bool finishOrderButtonsAvailability = false;

        public bool FinishOrderButtonsAvailability
        {
            get => SelectedOrder is not null; 
            set => SetField(ref finishOrderButtonsAvailability, value);
        }
        
        public SaleManager()
        {
            InitializeComponent();
            LoadMedicationList();
            LoadOrderList();
            MedicationClickedHandler = medication =>
            {
                SelectedMedication = medication;
                OnPropertyChanged(nameof(NewOrderButtonsAvailability));
            };
            OrderClickedHandler = order =>
            {
                SelectedOrder = order;
                OnPropertyChanged(nameof(FinishOrderButtonsAvailability));
                OnPropertyChanged(nameof(FinishOrderLabelContent));
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

        private void LoadOrderList()
        {
            AllOrders.Clear();
            var orders = SQLiteDataAccess.LoadOrders();
            foreach (var order in orders)
            {
                AllOrders.Add(order);
            }
        }

        private Action<MedicationModel> medicationClickedHandler;
        public Action<MedicationModel> MedicationClickedHandler { get => medicationClickedHandler; private set => SetField(ref medicationClickedHandler, value); }

        private Action<OrderModel> orderClickedHandler;
        public Action<OrderModel> OrderClickedHandler { get => orderClickedHandler; private set => SetField(ref orderClickedHandler, value); }
        public string FinishOrderLabelContent => SelectedOrder is null ? "" : $"Selected order: {SelectedOrder.Id}";

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
            var canceledOrder = SelectedOrder.CreatePatch(order =>
            {
                order.CompletionDate = DateTime.Now;
                order.Status = OrderStatus.Canceled;
            });
            SQLiteDataAccess.EditOrder(canceledOrder);
            
            LoadOrderList();
        }

        private void Complete_OnClick(object sender, RoutedEventArgs e)
        {
            var completedOrder = SelectedOrder.CreatePatch(order =>
            {
                order.CompletionDate = DateTime.Now;
                order.Status = OrderStatus.Finalized;
            });
            SQLiteDataAccess.EditOrder(completedOrder);
            
            LoadOrderList();
        }

        private void CreateOrder_OnClick(object sender, RoutedEventArgs e)
        {
            var updatedMedication =
                SelectedMedication.CreatePatch(medication => medication.Quantity -= quantityToOrder);

            try
            {
                SQLiteDataAccess.UpdateMedication(updatedMedication);
            }
            catch (Exception)
            {
                return;
            }
            
            var order = new OrderModel
            {
                MedicationId = SelectedMedication.Id,
                Price = SelectedMedication.Price * quantityToOrder,
                Quantity = quantityToOrder,
                CreationDate = DateTime.Now,
                CompletionDate = null
            };
            try
            {
                SQLiteDataAccess.SaveOrder(order);
            }
            catch (Exception)
            {
                SQLiteDataAccess.UpdateMedication(SelectedMedication);
                return;
            }
            LoadMedicationList();
            LoadOrderList();
        }

        /// <summary>
        /// INotifyPropertyChanged stuff
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            LoadMedicationList();
            LoadOrderList();
        }
    }
}
