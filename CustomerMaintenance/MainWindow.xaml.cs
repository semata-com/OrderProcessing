using System;
using System.Collections.Generic;
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

namespace CustomerMaintenance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Customer customer_;
        OrderProcessing orderProcessing_;

        public MainWindow()
        {
            InitializeComponent();
            orderProcessing_ = new OrderProcessing();
            orderProcessing_.Open("..\\..\\Orderprocessing.ds");
        }

        public void EnableDisable()
        {
            NewButton.IsEnabled = !customer_.Changed;
            SaveButton.IsEnabled = customer_.Changed;
            CancelButton.IsEnabled = customer_.Changed;
        }

        private bool SetCustomer(Customer customer)
        {
            bool customerSet = false;
            if (customer_ == null
                || !customer_.Changed
                || MessageBox.Show("Has been edited", "Customer", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                if (customer_ != null)
                {
                    customer_.CancelEdits();
                }
                customer_ = customer;
                this.DataContext = customer_;
                customer_.PropertyChanged += CustomerChanged;
                customerSet = true;
            }
            return customerSet;
        }

        private void CustomerChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            EnableDisable();
        }

        private async Task NewCustomer()
        {
            Customer customer = await Task.Run(() => orderProcessing_.CustomerItems.Create());
            if (SetCustomer(customer))
            {
                CustomerList.SelectedItem = null;
            }
        }

        private void CustomerFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CustomerFilter.Text != "")
            {
                CustomerList.Items.Filter = x => ((Customer)x).Name.Value.StartsWith(CustomerFilter.Text);
            }
            else
            {
                CustomerList.Items.Filter = null;
            }
        }

        private async void NewButton_Click(object sender, RoutedEventArgs e)
        {
            await NewCustomer();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => customer_.Write());
            CustomerList.ItemsSource = await Task.Run(() => orderProcessing_.CustomerItems.GetItems());
            CustomerList.SelectedItem = customer_;
            EnableDisable();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            customer_.CancelEdits();
            EnableDisable();
        }

        private async void DeleteCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            Customer selectedCustomer = (Customer)CustomerList.SelectedItem;
            if (selectedCustomer != null)
            {
                await Task.Run(() =>
                {
                    selectedCustomer.Delete();
                    CustomerList.ItemsSource = orderProcessing_.CustomerItems.GetItems();
                });
                await NewCustomer();
            }
            EnableDisable();
        }

        private void CustomerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Customer selectedCustomer = (Customer)CustomerList.SelectedItem;
            if (selectedCustomer != null && selectedCustomer != customer_)
            {
                if (!SetCustomer(selectedCustomer))
                {
                    CustomerList.SelectedItem = e.RemovedItems.Count > 0 ? e.RemovedItems[0] : null;
                }
            }
            EnableDisable();
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            await NewCustomer();
            CustomerList.ItemsSource = await Task.Run(() => orderProcessing_.CustomerItems.GetItems());
            EnableDisable();
        }

    }
}
