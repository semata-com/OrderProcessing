using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semata.DataStore.ObjectModel;
using Semata.DataStore.ObjectModel.Views;
using Semata.DataView;

namespace CustomerMaintenance
{
    public partial class OrderProcessingView : INotifyPropertyChanged, INotifyStateChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler StateChanged;
        
        protected CustomerMaintenance.OrderProcessing dataStore_;
        
        LazyValue<ItemObjectViewList<CustomerView>> customerItems_;
        LazyValue<ItemObjectViewList<OrderView>> orderItems_;
        LazyValue<ItemObjectViewList<OrderLineView>> orderLineItems_;
        LazyValue<ItemObjectViewList<ProductView>> productItems_;
        LazyValue<ItemObjectViewList<ProductGroupView>> productGroupItems_;
        
        partial void OnInitialize();
        
        public OrderProcessingView(PropertyChangedEventDispatcher eventDispatcher)
        {
            dataStore_ = new CustomerMaintenance.OrderProcessing(eventDispatcher);
                customerItems_=
                    new LazyValue<ItemObjectViewList<CustomerView>>(() => 
                        new ItemObjectViewList<Customer, CustomerView>
                            (dataStore_.CustomerItems.GetItemObjectSet()
                            , (x) => new CustomerView(x, false, false)));
                orderItems_=
                    new LazyValue<ItemObjectViewList<OrderView>>(() => 
                        new ItemObjectViewList<Order, OrderView>
                            (dataStore_.OrderItems.GetItemObjectSet()
                            , (x) => new OrderView(x, false, false)));
                orderLineItems_=
                    new LazyValue<ItemObjectViewList<OrderLineView>>(() => 
                        new ItemObjectViewList<OrderLine, OrderLineView>
                            (dataStore_.OrderLineItems.GetItemObjectSet()
                            , (x) => new OrderLineView(x, false, false)));
                productItems_=
                    new LazyValue<ItemObjectViewList<ProductView>>(() => 
                        new ItemObjectViewList<Product, ProductView>
                            (dataStore_.ProductItems.GetItemObjectSet()
                            , (x) => new ProductView(x, false, false)));
                productGroupItems_=
                    new LazyValue<ItemObjectViewList<ProductGroupView>>(() => 
                        new ItemObjectViewList<ProductGroup, ProductGroupView>
                            (dataStore_.ProductGroupItems.GetItemObjectSet()
                            , (x) => new ProductGroupView(x, false, false)));
            OnInitialize();
        }
        
        protected virtual void OnStateChanged(object sender, EventArgs args)
        {
            StateChanged?.Invoke(this, args);
        }
        
        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            OnStateChanged(sender, new EventArgs());
            PropertyChanged?.Invoke(this, args);
        }
        
        public void Open(string path)
        {
            dataStore_.PropertyWritten += OnPropertyChanged;
            dataStore_.Open(path);
        }
        
        public void Close(string path)
        {
            dataStore_.Close();
        }
        
        public ItemObjectViewList<CustomerView> CustomerItems => customerItems_.Value;
        
        public ItemObjectViewList<OrderView> OrderItems => orderItems_.Value;
        
        public ItemObjectViewList<OrderLineView> OrderLineItems => orderLineItems_.Value;
        
        public ItemObjectViewList<ProductView> ProductItems => productItems_.Value;
        
        public ItemObjectViewList<ProductGroupView> ProductGroupItems => productGroupItems_.Value;
        
}

    public partial class CustomerView : ItemObjectView<CustomerMaintenance.Customer>
    {
    
        LazyValue<IReadOnlyList<OrderView>> has_;
        
        partial void OnInitialize();
        
        private void Initialize()
        {
            has_=
                new LazyValue<IReadOnlyList<OrderView>>(() =>
                    new ItemObjectViewReadOnlyList<Order, OrderView>
                        (this.ItemObject.Has.GetItems()
                         , (x) => { return new OrderView(x, false, false); }));
            OnInitialize();
        }
        
        public CustomerView() : base()
        {
            Initialize();
        }
        
        internal CustomerView(CustomerMaintenance.Customer customer, bool usePropertyChanged, bool writeOnEndEdit) : base(customer, usePropertyChanged, writeOnEndEdit)
        {
            Initialize();
        }
        
        internal CustomerView(CustomerView customerView, bool usePropertyChanged, bool writeOnEndEdit) : base(customerView, usePropertyChanged, writeOnEndEdit)
        {
            Initialize();
        }
        
        
        public object AddressLine1
        {
            get => itemObject_.AddressLine1Property.RawValue;
            set => itemObject_.AddressLine1Property.Value = value;
        }
        
        public object AddressLine2
        {
            get => itemObject_.AddressLine2Property.RawValue;
            set => itemObject_.AddressLine2Property.Value = value;
        }
        
        public object AddressLine3
        {
            get => itemObject_.AddressLine3Property.RawValue;
            set => itemObject_.AddressLine3Property.Value = value;
        }
        
        public object Code
        {
            get => itemObject_.CodeProperty.RawValue;
            set => itemObject_.CodeProperty.Value = value;
        }
        
        public object Name
        {
            get => itemObject_.NameProperty.RawValue;
            set => itemObject_.NameProperty.Value = value;
        }
        
        public object PostCode
        {
            get => itemObject_.PostCodeProperty.RawValue;
            set => itemObject_.PostCodeProperty.Value = value;
        }
        
        public object PostTown
        {
            get => itemObject_.PostTownProperty.RawValue;
            set => itemObject_.PostTownProperty.Value = value;
        }
        
        public IReadOnlyList<OrderView> Has => has_.Value;
    }
    
    public partial class OrderView : ItemObjectView<CustomerMaintenance.Order>
    {
    
        LazyValue<IReadOnlyList<CustomerView>> by_;
        LazyValue<IReadOnlyList<OrderLineView>> lines_;
        
        partial void OnInitialize();
        
        private void Initialize()
        {
            by_=
                new LazyValue<IReadOnlyList<CustomerView>>(() =>
                    new ItemObjectViewReadOnlyList<Customer, CustomerView>
                        (this.ItemObject.By.GetItems()
                         , (x) => { return new CustomerView(x, false, false); }));
            lines_=
                new LazyValue<IReadOnlyList<OrderLineView>>(() =>
                    new ItemObjectViewReadOnlyList<OrderLine, OrderLineView>
                        (this.ItemObject.Lines.GetItems()
                         , (x) => { return new OrderLineView(x, false, false); }));
            OnInitialize();
        }
        
        public OrderView() : base()
        {
            Initialize();
        }
        
        internal OrderView(CustomerMaintenance.Order order, bool usePropertyChanged, bool writeOnEndEdit) : base(order, usePropertyChanged, writeOnEndEdit)
        {
            Initialize();
        }
        
        internal OrderView(OrderView orderView, bool usePropertyChanged, bool writeOnEndEdit) : base(orderView, usePropertyChanged, writeOnEndEdit)
        {
            Initialize();
        }
        
        
        public object CustomerReference
        {
            get => itemObject_.CustomerReferenceProperty.RawValue;
            set => itemObject_.CustomerReferenceProperty.Value = value;
        }
        
        public object Date
        {
            get => itemObject_.DateProperty.RawValue;
            set => itemObject_.DateProperty.Value = value;
        }
        
        public object OrderNo
        {
            get => itemObject_.OrderNoProperty.RawValue;
            set => itemObject_.OrderNoProperty.Value = value;
        }
        
        public IReadOnlyList<CustomerView> By => by_.Value;
        
        public IReadOnlyList<OrderLineView> Lines => lines_.Value;
    }
    
    public partial class OrderLineView : ItemObjectView<CustomerMaintenance.OrderLine>
    {
    
        LazyValue<IReadOnlyList<ProductView>> for_;
        
        partial void OnInitialize();
        
        private void Initialize()
        {
            for_=
                new LazyValue<IReadOnlyList<ProductView>>(() =>
                    new ItemObjectViewReadOnlyList<Product, ProductView>
                        (this.ItemObject.For.GetItems()
                         , (x) => { return new ProductView(x, false, false); }));
            OnInitialize();
        }
        
        public OrderLineView() : base()
        {
            Initialize();
        }
        
        internal OrderLineView(CustomerMaintenance.OrderLine orderLine, bool usePropertyChanged, bool writeOnEndEdit) : base(orderLine, usePropertyChanged, writeOnEndEdit)
        {
            Initialize();
        }
        
        internal OrderLineView(OrderLineView orderLineView, bool usePropertyChanged, bool writeOnEndEdit) : base(orderLineView, usePropertyChanged, writeOnEndEdit)
        {
            Initialize();
        }
        
        
        public object Quantity
        {
            get => itemObject_.QuantityProperty.RawValue;
            set => itemObject_.QuantityProperty.Value = value;
        }
        
        public OrderView On
        {
            get
            {
                return itemObject_.On == null ? null : new OrderView(itemObject_.On, false, false);
            }
            set
            {
                itemObject_.On = value == null ? null : value.ItemObject;
            }
        }
        
        public IReadOnlyList<ProductView> For => for_.Value;
    }
    
    public partial class ProductView : ItemObjectView<CustomerMaintenance.Product>
    {
    
        LazyValue<IReadOnlyList<ProductGroupView>> group_;
        LazyValue<IReadOnlyList<ProductGroupView>> is_;
        LazyValue<IReadOnlyList<OrderLineView>> orderedOn_;
        
        partial void OnInitialize();
        
        private void Initialize()
        {
            group_=
                new LazyValue<IReadOnlyList<ProductGroupView>>(() =>
                    new ItemObjectViewReadOnlyList<ProductGroup, ProductGroupView>
                        (this.ItemObject.Group.GetItems()
                         , (x) => { return new ProductGroupView(x, false, false); }));
            is_=
                new LazyValue<IReadOnlyList<ProductGroupView>>(() =>
                    new ItemObjectViewReadOnlyList<ProductGroup, ProductGroupView>
                        (this.ItemObject.Is.GetItems()
                         , (x) => { return new ProductGroupView(x, false, false); }));
            orderedOn_=
                new LazyValue<IReadOnlyList<OrderLineView>>(() =>
                    new ItemObjectViewReadOnlyList<OrderLine, OrderLineView>
                        (this.ItemObject.OrderedOn.GetItems()
                         , (x) => { return new OrderLineView(x, false, false); }));
            OnInitialize();
        }
        
        public ProductView() : base()
        {
            Initialize();
        }
        
        internal ProductView(CustomerMaintenance.Product product, bool usePropertyChanged, bool writeOnEndEdit) : base(product, usePropertyChanged, writeOnEndEdit)
        {
            Initialize();
        }
        
        internal ProductView(ProductView productView, bool usePropertyChanged, bool writeOnEndEdit) : base(productView, usePropertyChanged, writeOnEndEdit)
        {
            Initialize();
        }
        
        
        public object Code
        {
            get => itemObject_.CodeProperty.RawValue;
            set => itemObject_.CodeProperty.Value = value;
        }
        
        public object Description
        {
            get => itemObject_.DescriptionProperty.RawValue;
            set => itemObject_.DescriptionProperty.Value = value;
        }
        
        public object Price
        {
            get => itemObject_.PriceProperty.RawValue;
            set => itemObject_.PriceProperty.Value = value;
        }
        
        public object StockLevel
        {
            get => itemObject_.StockLevelProperty.RawValue;
            set => itemObject_.StockLevelProperty.Value = value;
        }
        
        public IReadOnlyList<ProductGroupView> Group => group_.Value;
        
        public IReadOnlyList<ProductGroupView> Is => is_.Value;
        
        public IReadOnlyList<OrderLineView> OrderedOn => orderedOn_.Value;
    }
    
    public partial class ProductGroupView : ItemObjectView<CustomerMaintenance.ProductGroup>
    {
    
        LazyValue<IReadOnlyList<ProductView>> contains_;
        LazyValue<IReadOnlyList<ProductGroupView>> parentGroup_;
        
        partial void OnInitialize();
        
        private void Initialize()
        {
            contains_=
                new LazyValue<IReadOnlyList<ProductView>>(() =>
                    new ItemObjectViewReadOnlyList<Product, ProductView>
                        (this.ItemObject.Contains.GetItems()
                         , (x) => { return new ProductView(x, false, false); }));
            parentGroup_=
                new LazyValue<IReadOnlyList<ProductGroupView>>(() =>
                    new ItemObjectViewReadOnlyList<ProductGroup, ProductGroupView>
                        (this.ItemObject.ParentGroup.GetItems()
                         , (x) => { return new ProductGroupView(x, false, false); }));
            OnInitialize();
        }
        
        public ProductGroupView() : base()
        {
            Initialize();
        }
        
        internal ProductGroupView(CustomerMaintenance.ProductGroup productGroup, bool usePropertyChanged, bool writeOnEndEdit) : base(productGroup, usePropertyChanged, writeOnEndEdit)
        {
            Initialize();
        }
        
        internal ProductGroupView(ProductGroupView productGroupView, bool usePropertyChanged, bool writeOnEndEdit) : base(productGroupView, usePropertyChanged, writeOnEndEdit)
        {
            Initialize();
        }
        
        
        public object Description
        {
            get => itemObject_.DescriptionProperty.RawValue;
            set => itemObject_.DescriptionProperty.Value = value;
        }
        
        public object Name
        {
            get => itemObject_.NameProperty.RawValue;
            set => itemObject_.NameProperty.Value = value;
        }
        
        public IReadOnlyList<ProductView> Contains => contains_.Value;
        
        public IReadOnlyList<ProductGroupView> ParentGroup => parentGroup_.Value;
    }
    
}
