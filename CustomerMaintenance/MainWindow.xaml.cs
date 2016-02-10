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

namespace CustomerMaintenance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Customer> customerList_;
        Customer customer_;
        OrderProcessing orderProcessing_;
        string filterAttribute_ = "(No Filter)";
        string filterValue_ = "";

        public MainWindow()
        {
            InitializeComponent();
            orderProcessing_ = new OrderProcessing();
            orderProcessing_.Open("..\\..\\Orderprocessing.ds");
            SetControlsState(true);
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

        private void SetTextBoxState(bool enable, TextBox textBox)
        {
            textBox.IsEnabled = enable && customer_ != null;
            if (textBox.IsEnabled)
            {
                textBox.BorderBrush = Brushes.Black;
            }
            else
            {
                textBox.BorderBrush = Brushes.Gray;
            }
        }

        public void SetControlsState(bool enable)
        {
            ApplyFilterButton.IsEnabled = enable && !IsChanged && ((string)FilterAttributeComboBox.SelectedValue != filterAttribute_ || FilterValueTextBox.Text != filterValue_);
            FilterValueTextBox.IsEnabled = enable && !IsChanged && (string)FilterAttributeComboBox.SelectedValue != "(No Filter)";
            DeleteButton.IsEnabled = enable && CustomerList.SelectedItem != null && !IsChanged;
            NewButton.IsEnabled = enable && !IsChanged;
            SaveButton.IsEnabled = enable && IsChanged;
            CancelButton.IsEnabled = SaveButton.IsEnabled;
            SetTextBoxState(enable, CodeTextBox);
            SetTextBoxState(enable, NameTextBox);
            SetTextBoxState(enable, AddressLine1TextBox);
            SetTextBoxState(enable, AddressLine2TextBox);
            SetTextBoxState(enable, AddressLine3TextBox);
            SetTextBoxState(enable, PostCodeTextBox);
            Cursor = (enable ? Cursors.Arrow : Cursors.Wait);
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
            SetControlsState(true);
        }

        private async Task LoadCustomers()
        {
            if (filterAttribute_ == "(No Filter)")
            {
                customerList_ = await Task.Run(() => orderProcessing_.CustomerItems.GetItems());
            }
            else
            {
                customerList_ = await Task.Run(() => orderProcessing_.CustomerItems.GetItemsByPrefix(filterAttribute_, filterValue_));
            }
            CustomerList.ItemsSource = customerList_;
        }

        private async Task SaveCustomer()
        {
            await Task.Run(() => customer_.Write());
            await LoadCustomers();
        }

        private async Task NewCustomer()
        {
            Customer customer = await Task.Run(() => orderProcessing_.CustomerItems.Create());
            if (await CheckAndSetCustomer(customer))
            {
                CustomerList.SelectedItem = null;
            }
        }

        private async void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
        {
            SetControlsState(false);
            filterAttribute_ = FilterAttributeComboBox.Text;
            filterValue_ = FilterValueTextBox.Text;
            await LoadCustomers();
            CustomerList.SelectedItem = customer_;
            SetControlsState(true);
        }

        private async void NewButton_Click(object sender, RoutedEventArgs e)
        {
            SetControlsState(false);
            await NewCustomer();
            SetControlsState(true);
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SetControlsState(false);
            await SaveCustomer();
            CustomerList.SelectedItem = customer_;
            SetControlsState(true);
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Customer selectedCustomer = (Customer)CustomerList.SelectedItem;
            if (MessageBox.Show("Are you sure you wish to delete " + selectedCustomer.Name + "?", "Customer", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                SetControlsState(false);
                await Task.Run(() => selectedCustomer.Delete());
                customerList_.Remove(customer_);
                SetCustomer(null);
                //await LoadCustomers();
                SetControlsState(true);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            customer_.CancelEdits();
            if (customer_.IsNew)
            {
                SetCustomer(null);
            }
            SetControlsState(true);
        }


        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
            SetControlsState(true);
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            SetControlsState(false);
            SetCustomer(null);
            await LoadCustomers();
            SetControlsState(true);
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
