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
            EnableDisable();
        }

        private bool IsChanged
        {
            get
            {
                return customer_ != null && (customer_.IsNew || customer_.IsChanged);
            }
        }

        public void EnableDisable()
        {
            DeleteButton.IsEnabled = CustomerList.SelectedItem != null && !IsChanged;
            NewButton.IsEnabled = !IsChanged;
            SaveButton.IsEnabled = IsChanged;
            CancelButton.IsEnabled = SaveButton.IsEnabled;
            CodeTextBox.IsEnabled = customer_ != null;
            NameTextBox.IsEnabled = customer_ != null;
            AddressLine1TextBox.IsEnabled = customer_ != null;
            AddressLine2TextBox.IsEnabled = customer_ != null;
            AddressLine3TextBox.IsEnabled = customer_ != null;
            PostCodeTextBox.IsEnabled = customer_ != null;
        }

        private void SetCustomer(Customer customer)
        {
            customer_ = customer;
            this.DataContext = customer_;
            if (customer != null)
            {
                customer_.PropertyChanged += CustomerChanged;
            }
        }

        private async Task<bool> CheckAndSetCustomer(Customer customer)
        {
            if (IsChanged
                && MessageBox.Show("Has been edited, Save Changes?", "Customer", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                await SaveCustomer();
            }
            bool customerSet = false;
            if (!IsChanged)
            {
                SetCustomer(customer);
                customerSet = true;
            }
            return customerSet;
        }

        private void CustomerChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            EnableDisable();
        }

        private async Task SaveCustomer()
        {
            await Task.Run(() => customer_.Write());
            CustomerList.ItemsSource = await Task.Run(() => orderProcessing_.CustomerItems.GetItems());
            CustomerList.SelectedItem = customer_;
        }

        private async Task NewCustomer()
        {
            Customer customer = await Task.Run(() => orderProcessing_.CustomerItems.Create());
            if (await CheckAndSetCustomer(customer))
            {
                CustomerList.SelectedItem = null;
            }
        }

        private void FilterValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (FilterValue.Text != "")
            {
                CustomerList.Items.Filter = x => ((string)((Customer)x).Name.Value).StartsWith(FilterValue.Text);
            }
            else
            {
                CustomerList.Items.Filter = null;
            }
        }

        private async void NewButton_Click(object sender, RoutedEventArgs e)
        {
            await NewCustomer();
            EnableDisable();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await SaveCustomer();
            EnableDisable();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            customer_.CancelEdits();
            if (customer_.IsNew)
            {
                SetCustomer(null);
            }
            EnableDisable();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Customer selectedCustomer = (Customer)CustomerList.SelectedItem;
            if (MessageBox.Show("Are you sure you wish to delete " + selectedCustomer.Name.Value +"?", "Customer", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await Task.Run(() => selectedCustomer.Delete());
                SetCustomer(null);
                CustomerList.ItemsSource = await Task.Run(() => orderProcessing_.CustomerItems.GetItems());
                EnableDisable();
            }
        }

        private async void CustomerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Customer selectedCustomer = (Customer)CustomerList.SelectedItem;
            if (selectedCustomer != null && selectedCustomer != customer_)
            {
                if (!(await CheckAndSetCustomer(selectedCustomer)))
                {
                    CustomerList.SelectedItem = e.RemovedItems.Count > 0 ? e.RemovedItems[0] : null;
                }
            }
            EnableDisable();
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            SetCustomer(null);
            CustomerList.ItemsSource = await Task.Run(() => orderProcessing_.CustomerItems.GetItems());
            EnableDisable();
        }

    }
}
