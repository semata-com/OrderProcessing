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
        string filterAttribute_ = "Name";
        string filterValue_ = "";

        public MainWindow()
        {
            InitializeComponent();
            orderProcessing_ = new OrderProcessing();
            orderProcessing_.Open("..\\..\\Orderprocessing.ds");
            SetControlsState();
            FilterAttribute.Items.Add("Code");
            FilterAttribute.Items.Add("Name");
            FilterAttribute.Items.Add("Address Line 1");
            FilterAttribute.SelectedIndex = 1;
        }

        private bool IsChanged
        {
            get
            {
                return customer_ != null && (customer_.IsNew || customer_.IsChanged);
            }
        }

        private void SetTextBoxState(TextBox textBox)
        {
            textBox.IsEnabled = customer_ != null;
            if (textBox.IsEnabled)
            {
                textBox.BorderBrush = Brushes.Black;
            }
            else
            {
                textBox.BorderBrush = Brushes.Gray;
            }
        }

        public void SetControlsState()
        {
            ApplyFilterButton.IsEnabled = ((string)FilterAttribute.SelectedValue != filterAttribute_ || FilterValue.Text != filterValue_);
            DeleteButton.IsEnabled = CustomerList.SelectedItem != null && !IsChanged;
            NewButton.IsEnabled = !IsChanged;
            SaveButton.IsEnabled = IsChanged;
            CancelButton.IsEnabled = SaveButton.IsEnabled;
            SetTextBoxState(CodeTextBox);
            SetTextBoxState(NameTextBox);
            SetTextBoxState(AddressLine1TextBox);
            SetTextBoxState(AddressLine2TextBox);
            SetTextBoxState(AddressLine3TextBox);
            SetTextBoxState(PostCodeTextBox);
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
            SetControlsState();
        }

        private async Task LoadCustomers()
        {
            CustomerList.ItemsSource = await Task.Run(() => orderProcessing_.CustomerItems.GetItemsByPrefix(filterAttribute_, filterValue_));
            CustomerList.SelectedItem = customer_;
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
            filterAttribute_ = FilterAttribute.Text;
            filterValue_ = FilterValue.Text;
            await LoadCustomers();
        }

        private async void NewButton_Click(object sender, RoutedEventArgs e)
        {
            await NewCustomer();
            SetControlsState();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await SaveCustomer();
            SetControlsState();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Customer selectedCustomer = (Customer)CustomerList.SelectedItem;
            if (MessageBox.Show("Are you sure you wish to delete " + selectedCustomer.Name.Value + "?", "Customer", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                await Task.Run(() => selectedCustomer.Delete());
                SetCustomer(null);
                await LoadCustomers();
                SetControlsState();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            customer_.CancelEdits();
            if (customer_.IsNew)
            {
                SetCustomer(null);
            }
            SetControlsState();
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
            SetControlsState();
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            SetCustomer(null);
            await LoadCustomers();
            SetControlsState();
        }

        private void FilterAttribute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetControlsState();
        }

        private void FilterValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetControlsState();
        }



    }
}
