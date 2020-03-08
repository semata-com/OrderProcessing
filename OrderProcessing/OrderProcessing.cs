using System;
using System.Windows.Input;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semata.DataStore.ObjectModel;
using Semata.DataStore.ObjectModel.Views;
using Semata.DataView;
using Semata.Lazy;

namespace OrderProcessing
{

    public partial class OrderLine
    {
        partial void OnWriting()
        {
            For.StockLevel -= ((int?)QuantityProperty.Value ?? 0) - ((int?)QuantityProperty.StoredValue ?? 0);
            For.Write();
        }

        partial void OnDeleting()
        {
            For.StockLevel += ((int?)QuantityProperty.StoredValue ?? 0);
            For.Write();
        }
    }

    public partial class OrderProcessingDataStoreView
    {
        LazyValue<ItemObjectViewList<Customer, CustomerView>> orderedCustomers_;
        LazyValue<SelectorDetailSource> editableCustomers_;

        LazyValue<ItemObjectViewList<Product, ProductView>> orderedProducts_;
        LazyValue<SelectorDetailSource> editableProducts_;

        partial void OnInitialize()
        {
            orderedCustomers_ =
                new LazyValue<ItemObjectViewList<Customer, CustomerView>>(() =>
                    new ItemObjectViewList<Customer, CustomerView>
                        (dataStore_.CustomerItems.GetItemObjectSet()
                         , z => z.OrderBy((y) => y.Code)
                         , (x) => new CustomerView(x, false, false)));

            editableCustomers_ =
                new LazyValue<SelectorDetailSource>(() =>
                    new SelectorDetailSource
                        (OrderedCustomers
                         , (x) => x is CustomerView ? new CustomerView(x as CustomerView, true, true) : null
                         , () => System.Windows.MessageBox.Show("Has been edited, Save Changes?", "Customers", System.Windows.MessageBoxButton.OKCancel) == System.Windows.MessageBoxResult.OK
                         , false
                         , true));

            orderedProducts_ =
                new LazyValue<ItemObjectViewList<Product, ProductView>>(() =>
                    new ItemObjectViewList<Product, ProductView>
                        (dataStore_.ProductItems.GetItemObjectSet()
                         , z => z.OrderBy((y) => y.Code)
                         , (x) => new ProductView(x, false, false)));

            editableProducts_ =
                new LazyValue<SelectorDetailSource>(() =>
                    new SelectorDetailSource
                        (OrderedProducts
                         , (x) => x is ProductView ? new ProductView(x as ProductView, true, true) : null
                         , () => System.Windows.MessageBox.Show("Has been edited, Save Changes?", "Customers", System.Windows.MessageBoxButton.OKCancel) == System.Windows.MessageBoxResult.OK
                         , false
                         , true));
        }

        public ItemObjectViewList<Customer, CustomerView> OrderedCustomers => orderedCustomers_.Value;

        public SelectorDetailSource EditableCustomers => editableCustomers_.Value;

        public ItemObjectViewList<Product, ProductView> OrderedProducts => orderedProducts_.Value;

        public SelectorDetailSource EditableProducts => editableProducts_.Value;

    }

    public partial class CustomerView
    {
        LazyValue<ItemObjectViewList<Order, OrderView>> orderedOrders_;
        LazyValue<SelectorDetailSource> editableOrders_;

        partial void OnInitialize()
        {
            orderedOrders_ =
                new LazyValue<ItemObjectViewList<Order, OrderView>>(() =>
                    new ItemObjectViewList<Order, OrderView>
                        (Customer.Has.GetItemObjectSet()
                         , z => z.OrderBy((y) => y.Date)
                         , (x) => new OrderView(x, false, false)));

            editableOrders_ =
                new LazyValue<SelectorDetailSource>(() =>
                    new SelectorDetailSource
                        (OrderedOrders
                         , (x) => x is OrderView ? new OrderView(x as OrderView, true, true) : null
                         , () => System.Windows.MessageBox.Show("Has been edited, Save Changes?", "Orders", System.Windows.MessageBoxButton.OKCancel) == System.Windows.MessageBoxResult.OK
                         , false
                         , true));
        }

        public ItemObjectViewList<Order, OrderView> OrderedOrders => orderedOrders_.Value;

        public SelectorDetailSource EditableOrders => editableOrders_.Value;

    }

    public partial class OrderView
    {
        LazyValue<ItemObjectViewList<OrderLine, OrderLineView>> editableLines_;

        partial void OnInitialize()
        {
            editableLines_ =
                new LazyValue<ItemObjectViewList<OrderLine, OrderLineView>>(() =>
                    new ItemObjectViewList<OrderLine, OrderLineView>
                        (Order.Lines.GetItemObjectSet()
                         , z => z.OrderBy((y) => y.For.Code)
                         , (x) => new OrderLineView(x, true, true)));

        }

        public ItemObjectViewList<OrderLine, OrderLineView> EditableLines => editableLines_.Value;

    }

    public partial class OrderLineView
    {
        ProductView currentProduct;

        partial void OnInitialize()
        {
            PropertyChanged += OrderLineViewPropertyChanged;
            if (For != null)
            {
                currentProduct = For;
                currentProduct.PropertyChanged += CurrentProductPropertyChanged;
            }
        }

        private void CurrentProductPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Price")
            {
                NotifyPropertyChanged(new PropertyChangedEventArgs("Cost"));
            }
        }


        private void OrderLineViewPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Quantity")
            {
                NotifyPropertyChanged(new PropertyChangedEventArgs("Cost"));
            }
            else if (e.PropertyName == "For")
            {
                if (currentProduct != null)
                {
                    currentProduct.PropertyChanged -= CurrentProductPropertyChanged;
                }
                if (For != null)
                {
                    currentProduct = For;
                    currentProduct.PropertyChanged += CurrentProductPropertyChanged;
                }
                NotifyPropertyChanged(new PropertyChangedEventArgs("Cost"));
            }
        }

        public override bool Validate()
        {
            var result = base.Validate();
            if (result)
            {
                if (ItemObject.Quantity - ((int?)ItemObject.QuantityProperty.StoredValue ?? 0) > (ItemObject.For.StockLevel ?? 0))
                {
                    ItemObject.AddError("Quantity", "Quantity is grater than available stock");
                }
            }
            return result;
        }

        public decimal Cost => (ItemObject?.For?.Price ?? 0) * (ItemObject?.Quantity ?? 0);

    }
}

