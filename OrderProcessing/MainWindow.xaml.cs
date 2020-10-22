using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Semata.DataView;

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
            orderProcessing_.Open(orderProcessingPath);
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
