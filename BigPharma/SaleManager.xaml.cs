using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using BigPharmaEngine;
using BigPharmaEngine.Models;
using BigPharmaEngine.Models.Extensions;

namespace BigPharma
{
    public sealed partial class SaleManager : INotifyPropertyChanged
    {
        public ObservableCollection<MedicationModel> AllMedications { get; set; } = new();
        private MedicationModel? SelectedMedication { get; set; }
        public ObservableCollection<OrderModel> AllOrders { get; set; } = new();
        private OrderModel? SelectedOrder { get; set; }

        private int quantityToOrder;
        public int QuantityToOrder
        {
            get => quantityToOrder;
            set
            {
                SetField(ref quantityToOrder, value);
                OnPropertyChanged(nameof(newOrderButtonsAvailability));
            }
        }

        private bool newOrderButtonsAvailability;
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

        private bool confirmOrderButtonsAvailability;
        public bool ConfirmOrderButtonsAvailability
        {
            get => SelectedOrder is not null; 
            set => SetField(ref confirmOrderButtonsAvailability, value);
        }

        private bool finishTransactionButtonAvailability;
        public bool FinishTransactionButtonAvailability
        {
            get => AllOrders.Any() && AllOrders.All(order => order.Status != OrderStatus.Created);
            set => SetField(ref finishTransactionButtonAvailability, value);
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
                OnPropertyChanged(nameof(ConfirmOrderButtonsAvailability));
                OnPropertyChanged(nameof(ConfirmOrderLabelContent));
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
            var orders = SQLiteDataAccess.LoadOrders()
                .Where(order => order.Status is OrderStatus.Canceled or OrderStatus.Confirmed or OrderStatus.Created);
            foreach (var order in orders)
            {
                AllOrders.Add(order);
            }
        }

        private Action<MedicationModel>? medicationClickedHandler;
        public Action<MedicationModel>? MedicationClickedHandler { get => medicationClickedHandler; private set => SetField(ref medicationClickedHandler, value); }

        private Action<OrderModel>? orderClickedHandler;
        public Action<OrderModel>? OrderClickedHandler { get => orderClickedHandler; private set => SetField(ref orderClickedHandler, value); }
        public string ConfirmOrderLabelContent => SelectedOrder is null ? "" : $"Selected order: {SelectedOrder.Id}";

        private string sumUpTransactionText = string.Empty;
        public string SumUpTransactionText
        {
            get => FinishTransactionButtonAvailability && AllOrders.Any(order => order.Status == OrderStatus.Confirmed) ? $"Sum: {AllOrders
                .Where(order => order.Status == OrderStatus.Confirmed)
                .Select(order => order.Price)
                .Aggregate((sum, val) => sum + val).ToString()} $" : string.Empty;
            set => SetField(ref sumUpTransactionText, value); 
        }

        private Visibility popupVisibility = Visibility.Collapsed;
        public Visibility PopupVisibility { get => popupVisibility; set => SetField(ref popupVisibility, value); }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (sender.GetType() == typeof(MainWindow)) return;
            this.Hide();
            e.Cancel = true;
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            if(SelectedOrder is null) return;
            if(SelectedOrder.Status == OrderStatus.Canceled) return;
            var medicationToUpdate = SQLiteDataAccess.LoadMedictaions()
                .FirstOrDefault(medication => medication.Id == SelectedOrder.MedicationId);
            if(medicationToUpdate is null) return;
            try
            {
                SQLiteDataAccess.UpdateMedication(medicationToUpdate.CreatePatch(medication => medication.Quantity += SelectedOrder.Quantity));
            }
            catch (Exception)
            {
                return;
            }
            var canceledOrder = SelectedOrder?.CreatePatch(order =>
            {
                order.Status = OrderStatus.Canceled;
            });
            if (canceledOrder is null) return; 
            SQLiteDataAccess.EditOrder(canceledOrder);
            LoadOrderList();
            LoadMedicationList();
            OnPropertyChanged(nameof(FinishTransactionButtonAvailability));
            OnPropertyChanged(nameof(SumUpTransactionText));
        }

        private void Confirm_OnClick(object sender, RoutedEventArgs e)
        {
            if(SelectedOrder is null) return;
            if(SelectedOrder.Status == OrderStatus.Confirmed) return;
            var medicationToUpdate = SQLiteDataAccess.LoadMedictaions()
                .FirstOrDefault(medication => medication.Id == SelectedOrder.MedicationId);
            if(medicationToUpdate is null) return;
            try
            {
                SQLiteDataAccess.UpdateMedication(medicationToUpdate.CreatePatch(medication => medication.Quantity -= SelectedOrder.Quantity));
            }
            catch (Exception)
            {
                return;
            }
            var completedOrder = SelectedOrder?.CreatePatch(order =>
            {
                order.Status = OrderStatus.Confirmed;
            });
            if(completedOrder is null) return;
            SQLiteDataAccess.EditOrder(completedOrder);
            LoadOrderList();
            LoadMedicationList();
            OnPropertyChanged(nameof(FinishTransactionButtonAvailability));
            OnPropertyChanged(nameof(SumUpTransactionText));
        }

        private void CreateOrder_OnClick(object sender, RoutedEventArgs e)
        {
            if(SelectedMedication is null) return;
            var order = new OrderModel
            {
                MedicationId = SelectedMedication.Id,
                Price = SelectedMedication.Price * quantityToOrder,
                Quantity = quantityToOrder
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
            OnPropertyChanged(nameof(FinishTransactionButtonAvailability));
            OnPropertyChanged(nameof(SumUpTransactionText));
        }

        private void FinishTransaction_OnClick(object sender, RoutedEventArgs e)
        {
            var transaction = new TransactionModel()
            {
                CompletionDate = DateTime.Now
            };
            var createdTransaction = SQLiteDataAccess.SaveTransaction(transaction);
            
            foreach (var orderModel in AllOrders.Where(order => order.Status is OrderStatus.Confirmed))
            {
                SQLiteDataAccess.EditOrder(orderModel.CreatePatch(order =>
                {
                    order.Status = OrderStatus.Completed;
                    order.TransactionId = createdTransaction.Id;
                }));
            }
            foreach (var orderModel in AllOrders.Where(order => order.Status is OrderStatus.Canceled))
            {
                SQLiteDataAccess.EditOrder(orderModel.CreatePatch(order =>
                {
                    order.Status = OrderStatus.Archived;
                    order.TransactionId = createdTransaction.Id;
                }));
            }
            AllOrders.Clear();
            ResetUi();
            PopupVisibility = Visibility.Visible;
        }

        private void ResetUi()
        {
            SelectedOrder = null;
            SelectedMedication = null;
            QuantityToOrder = 0;
            OnPropertyChanged(nameof(newOrderButtonsAvailability));
            OnPropertyChanged(nameof(ConfirmOrderButtonsAvailability));
            OnPropertyChanged(nameof(FinishTransactionButtonAvailability));
            OnPropertyChanged(nameof(SumUpTransactionText));
            OnPropertyChanged(nameof(ConfirmOrderLabelContent));
        }

        private void ClosePopup()
        {
            GreatPopup.IsOpen = false;
        }

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
            LoadOrderList();
        }
    }
}
