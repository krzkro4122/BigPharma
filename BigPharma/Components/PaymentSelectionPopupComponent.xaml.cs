using System;
using System.Windows;

namespace BigPharma.Components;

public partial class PaymentSelectionPopupComponent
{
    private static readonly DependencyProperty CashChosenHandlerDependencyProperty =
        DependencyProperty.Register(
            nameof(CashChosen),
            typeof(Action),   
            typeof(PaymentSelectionPopupComponent));
    
    private static readonly DependencyProperty CardChosenHandlerDependencyProperty =
        DependencyProperty.Register(
            nameof(CardChosen),
            typeof(Action),   
            typeof(PaymentSelectionPopupComponent));

    public Action CardChosen
    {
        get => (Action)GetValue(CardChosenHandlerDependencyProperty);
        set => SetValue(CardChosenHandlerDependencyProperty, value);
    }
    
    public Action CashChosen 
    {
        get => (Action)GetValue(CashChosenHandlerDependencyProperty);
        set => SetValue(CashChosenHandlerDependencyProperty, value);
    }
    
    public PaymentSelectionPopupComponent()
    {
        InitializeComponent();
    }

    private void CashChosen_OnClick(object sender, RoutedEventArgs e)
    {
        CashChosen();
    }

    private void CardChosen_OnClick(object sender, RoutedEventArgs e)
    {
        CashChosen();
    }
}