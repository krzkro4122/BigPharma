using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using BigPharmaEngine;

namespace BigPharma.Components;

public partial class OrderBrowserComponent : UserControl
{
    private static readonly DependencyProperty medicationsDependencyProperty =
        DependencyProperty.Register(nameof(Orders),
            typeof(ObservableCollection<MedicationModel>),
            typeof(MedicationBrowserComponent));

    private static readonly DependencyProperty medicationClickedDependencyProperty =
        DependencyProperty.Register(
            nameof(OrderClicked),
            typeof(Action<MedicationModel>),   
            typeof(MedicationBrowserComponent));

    public ObservableCollection<OrderModel> Orders
    {
        get => (ObservableCollection<OrderModel>)GetValue(medicationsDependencyProperty);
        set => SetValue(medicationsDependencyProperty, value);
    }
    
    public Action<OrderModel> OrderClicked
    {
        get => (Action<OrderModel>)GetValue(medicationClickedDependencyProperty);
        set => SetValue(medicationClickedDependencyProperty, value);
    }
    
    public OrderBrowserComponent()
    {
        InitializeComponent();
    }

    private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        var view = CollectionViewSource.GetDefaultView(Orders);
        if (view != null)
        {
            view.Filter = item =>
            {
                var orderModel = item as OrderModel;
                var isFilterSet = SearchBox.Text.Length != 0;
                if (!isFilterSet)
                {
                    return true;
                }
                    
                var idContainsCriterion = orderModel != null && orderModel.Id.ToString().Contains(SearchBox.Text);

                return idContainsCriterion;
            };
        }
    }

    private void OrdersDataGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.AddedItems is null || e.AddedItems.Count == 0)
        {
            return;
        }
        var order = e.AddedItems[0] as OrderModel;
        OrderClicked(order);
    }
}