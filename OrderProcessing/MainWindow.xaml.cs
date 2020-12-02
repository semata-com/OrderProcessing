using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.IO;

namespace OrderProcessing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OrderProcessingDataStoreView orderProcessing_;

        public MainWindow()
        {
            InitializeComponent();
            orderProcessing_ = new OrderProcessingDataStoreView(null);
            string appCommonData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string orderProcessingPath = appCommonData + "\\Semata\\OrderProcessing\\OrderProcessing.ds";
            if (!(new FileInfo(orderProcessingPath)).Exists)
            {
                MessageBox.Show("OrderProcessing.ds not found. Have you run OrderProcessingSetup?", "DataStore not found");
                Close();
            }
            else
            {
                orderProcessing_.Open(orderProcessingPath);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = orderProcessing_;
        }

        private void OrderLines_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            OrderLinesProductCodeComboBox.ItemsSource = CollectionViewSource.GetDefaultView(orderProcessing_.ProductItems);
        }
    }
}
