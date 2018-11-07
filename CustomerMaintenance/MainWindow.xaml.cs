using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Semata.DataStore.ObjectModel.Views;

namespace CustomerMaintenance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Customer customer_;
        OrderProcessingView orderProcessing_;
        string filterAttribute_ = "(No Filter)";
        string filterValue_ = "";

        public MainWindow()
        {
            InitializeComponent();
            orderProcessing_ = new OrderProcessingView(null);
            orderProcessing_.Open("..\\..\\Orderprocessing.ds");
            FilterAttributeComboBox.Items.Add("(No Filter)");
            FilterAttributeComboBox.Items.Add("Code");
            FilterAttributeComboBox.Items.Add("Name");
            FilterAttributeComboBox.Items.Add("Address Line 1");
            FilterAttributeComboBox.SelectedIndex = 0;
        }

        private bool IsChanged
        {
            get
            {
                return customer_ != null && (customer_.IsNew || customer_.IsChanged);
            }
        }

        public void SetControlsState(bool enable)
        {
            ApplyFilterButton.IsEnabled = enable && !IsChanged && ((string)FilterAttributeComboBox.SelectedValue != filterAttribute_ || FilterValueTextBox.Text != filterValue_);
            FilterValueTextBox.IsEnabled = enable && !IsChanged && (string)FilterAttributeComboBox.SelectedValue != "(No Filter)";
        }


        private async void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
        {
            SetControlsState(false);
            filterAttribute_ = FilterAttributeComboBox.Text;
            filterValue_ = FilterValueTextBox.Text;
//            await LoadCustomers();
            CustomerList.SelectedItem = customer_;
            SetControlsState(true);
        }



        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = orderProcessing_;
        }

        private void FilterAttributeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetControlsState(true);
        }

        private void FilterValueTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetControlsState(true);
        }



    }
}
