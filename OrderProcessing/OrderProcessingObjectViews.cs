using System;
using System.Collections.Generic;
using System.ComponentModel;
using Semata.DataStore.ObjectModel;
using Semata.DataStore.ObjectModel.Views;
using Semata.Lazy;
using Semata.EditableData;

namespace OrderProcessing
{
    public partial class OrderProcessingDataStoreView : INotifyPropertyChanged, INotifyStateChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler StateChanged;
        
        protected OrderProcessing.OrderProcessingDataStore dataStore_;
        
        LazyValue<ItemObjectViewList<Customer, CustomerView>> customerItems_;
        LazyValue<ItemObjectViewList<Order, OrderView>> orderItems_;
        LazyValue<ItemObjectViewList<OrderLine, OrderLineView>> orderLineItems_;
        LazyValue<ItemObjectViewList<Product, ProductView>> productItems_;
        LazyValue<ItemObjectViewList<ProductGroup, ProductGroupView>> productGroupItems_;
        
        partial void OnInitialize();
        
        public OrderProcessingDataStoreView(PropertyChangedEventDispatcher eventDispatcher)
        {
            dataStore_ = new OrderProcessing.OrderProcessingDataStore(eventDispatcher);
                customerItems_=
                    new LazyValue<ItemObjectViewList<Customer, CustomerView>>(() => 
                        {
                            var list = new ItemObjectViewList<Customer, CustomerView>
                                           (dataStore_.CustomerItems.GetItemObjectCollection()
                                            , (x) => new CustomerView(x, false, false));
                            list.ListChanged += (object sender, EventArgs e) => NotifyPropertyChanged(new PropertyChangedEventArgs("CustomerItems"));
                            return list;
                        });
                orderItems_=
                    new LazyValue<ItemObjectViewList<Order, OrderView>>(() => 
                        {
                            var list = new ItemObjectViewList<Order, OrderView>
                                           (dataStore_.OrderItems.GetItemObjectCollection()
                                            , (x) => new OrderView(x, false, false));
                            list.ListChanged += (object sender, EventArgs e) => NotifyPropertyChanged(new PropertyChangedEventArgs("OrderItems"));
                            return list;
                        });
                orderLineItems_=
                    new LazyValue<ItemObjectViewList<OrderLine, OrderLineView>>(() => 
                        {
                            var list = new ItemObjectViewList<OrderLine, OrderLineView>
                                           (dataStore_.OrderLineItems.GetItemObjectCollection()
                                            , (x) => new OrderLineView(x, false, false));
                            list.ListChanged += (object sender, EventArgs e) => NotifyPropertyChanged(new PropertyChangedEventArgs("OrderLineItems"));
                            return list;
                        });
                productItems_=
                    new LazyValue<ItemObjectViewList<Product, ProductView>>(() => 
                        {
                            var list = new ItemObjectViewList<Product, ProductView>
                                           (dataStore_.ProductItems.GetItemObjectCollection()
                                            , (x) => new ProductView(x, false, false));
                            list.ListChanged += (object sender, EventArgs e) => NotifyPropertyChanged(new PropertyChangedEventArgs("ProductItems"));
                            return list;
                        });
                productGroupItems_=
                    new LazyValue<ItemObjectViewList<ProductGroup, ProductGroupView>>(() => 
                        {
                            var list = new ItemObjectViewList<ProductGroup, ProductGroupView>
                                           (dataStore_.ProductGroupItems.GetItemObjectCollection()
                                            , (x) => new ProductGroupView(x, false, false));
                            list.ListChanged += (object sender, EventArgs e) => NotifyPropertyChanged(new PropertyChangedEventArgs("ProductGroupItems"));
                            return list;
                        });
        }
        
        protected virtual void NotifyStateChanged(EventArgs args)
        {
            StateChanged?.Invoke(this, args);
        }
        
        protected virtual void NotifyPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
            NotifyStateChanged(new EventArgs());
        }
        
        public void Open(string path)
        {
            dataStore_.Open(path);
            OnInitialize();
        }
        
        public void Close()
        {
            dataStore_.Close();
        }
        
        public ItemObjectViewList<Customer, CustomerView> CustomerItems => customerItems_.Value;
        
        public ItemObjectViewList<Order, OrderView> OrderItems => orderItems_.Value;
        
        public ItemObjectViewList<OrderLine, OrderLineView> OrderLineItems => orderLineItems_.Value;
        
        public ItemObjectViewList<Product, ProductView> ProductItems => productItems_.Value;
        
        public ItemObjectViewList<ProductGroup, ProductGroupView> ProductGroupItems => productGroupItems_.Value;
        
}

    public partial class CustomerView : ItemObjectView<OrderProcessing.Customer>
    {
    
        LazyProperty<object> addressLine1_;
        LazyProperty<object> addressLine2_;
        LazyProperty<object> addressLine3_;
        LazyProperty<object> code_;
        LazyProperty<object> name_;
        LazyProperty<object> postCode_;
        LazyProperty<object> postTown_;
        LazyValue<ItemObjectViewList<Order, OrderView>> has_;
        
        partial void OnInitialize();
        
        protected override void InitializeValues()
        {
            addressLine1_=
                new LazyProperty<object>(this
                    , "AddressLine1"
                    , () => Customer.AddressLine1Property.RawValue
                    , (x) => Customer.AddressLine1Property.Value = x
                    , (x) => x != Customer.AddressLine1Property.RawValue
                    , (x) => NotifyPropertyChanged(new PropertyChangedEventArgs("AddressLine1")));
        
            addressLine2_=
                new LazyProperty<object>(this
                    , "AddressLine2"
                    , () => Customer.AddressLine2Property.RawValue
                    , (x) => Customer.AddressLine2Property.Value = x
                    , (x) => x != Customer.AddressLine2Property.RawValue
                    , (x) => NotifyPropertyChanged(new PropertyChangedEventArgs("AddressLine2")));
        
            addressLine3_=
                new LazyProperty<object>(this
                    , "AddressLine3"
                    , () => Customer.AddressLine3Property.RawValue
                    , (x) => Customer.AddressLine3Property.Value = x
                    , (x) => x != Customer.AddressLine3Property.RawValue
                    , (x) => NotifyPropertyChanged(new PropertyChangedEventArgs("AddressLine3")));
        
            code_=
                new LazyProperty<object>(this
                    , "Code"
                    , () => Customer.CodeProperty.RawValue
                    , (x) => Customer.CodeProperty.Value = x
                    , (x) => x != Customer.CodeProperty.RawValue
                    , (x) => NotifyPropertyChanged(new PropertyChangedEventArgs("Code")));
        
            name_=
                new LazyProperty<object>(this
                    , "Name"
                    , () => Customer.NameProperty.RawValue
                    , (x) => Customer.NameProperty.Value = x
                    , (x) => x != Customer.NameProperty.RawValue
                    , (x) => NotifyPropertyChanged(new PropertyChangedEventArgs("Name")));
        
            postCode_=
                new LazyProperty<object>(this
                    , "PostCode"
                    , () => Customer.PostCodeProperty.RawValue
                    , (x) => Customer.PostCodeProperty.Value = x
                    , (x) => x != Customer.PostCodeProperty.RawValue
                    , (x) => NotifyPropertyChanged(new PropertyChangedEventArgs("PostCode")));
        
            postTown_=
                new LazyProperty<object>(this
                    , "PostTown"
                    , () => Customer.PostTownProperty.RawValue
                    , (x) => Customer.PostTownProperty.Value = x
                    , (x) => x != Customer.PostTownProperty.RawValue
                    , (x) => NotifyPropertyChanged(new PropertyChangedEventArgs("PostTown")));
        
            has_=
                new LazyValue<ItemObjectViewList<Order, OrderView>>(() =>
                    {
                        var list = new ItemObjectViewList<Order, OrderView>
                                       (Customer.Has.GetItemObjectCollection(null, null, null)
                                        , (x) => new OrderView(x, false, false));
                        list.ListChanged += (object sender, EventArgs e) => NotifyPropertyChanged(new PropertyChangedEventArgs("Has"));
                        return list;
                    });
            OnInitialize();
        }
        
        public CustomerView() : base()
        {
        }
        
        internal CustomerView(OrderProcessing.Customer customer, bool usePropertyChanged, bool writeOnEndEdit) : base(customer, usePropertyChanged, writeOnEndEdit)
        {
        }
        
        internal CustomerView(CustomerView customerView, bool usePropertyChanged, bool writeOnEndEdit) : base(customerView, usePropertyChanged, writeOnEndEdit)
        {
        }
        
        public Customer Customer => ItemObject;
        
        
        public object AddressLine1
        {
            get => addressLine1_.Value;
            set => addressLine1_.Value = value;
        }
        
        public object AddressLine2
        {
            get => addressLine2_.Value;
            set => addressLine2_.Value = value;
        }
        
        public object AddressLine3
        {
            get => addressLine3_.Value;
            set => addressLine3_.Value = value;
        }
        
        public object Code
        {
            get => code_.Value;
            set => code_.Value = value;
        }
        
        public object Name
        {
            get => name_.Value;
            set => name_.Value = value;
        }
        
        public object PostCode
        {
            get => postCode_.Value;
            set => postCode_.Value = value;
        }
        
        public object PostTown
        {
            get => postTown_.Value;
            set => postTown_.Value = value;
        }
        
        public ItemObjectViewList<Order, OrderView> Has => has_.Value;
    }
    
    public partial class OrderView : ItemObjectView<OrderProcessing.Order>
    {
    
        LazyProperty<object> customerReference_;
        LazyProperty<object> date_;
        LazyProperty<object> orderNo_;
        LazyProperty<CustomerView> by_;
        LazyValue<ItemObjectViewList<OrderLine, OrderLineView>> lines_;
        
        partial void OnInitialize();
        
        protected override void InitializeValues()
        {
            customerReference_=
                new LazyProperty<object>(this
                    , "CustomerReference"
                    , () => Order.CustomerReferenceProperty.RawValue
                    , (x) => Order.CustomerReferenceProperty.Value = x
                    , (x) => x != Order.CustomerReferenceProperty.RawValue
                    , (x) => NotifyPropertyChanged(new PropertyChangedEventArgs("CustomerReference")));
        
            date_=
                new LazyProperty<object>(this
                    , "Date"
                    , () => Order.DateProperty.RawValue
                    , (x) => Order.DateProperty.Value = x
                    , (x) => x != Order.DateProperty.RawValue
                    , (x) => NotifyPropertyChanged(new PropertyChangedEventArgs("Date")));
        
            orderNo_=
                new LazyProperty<object>(this
                    , "OrderNo"
                    , () => Order.OrderNoProperty.RawValue
                    , (x) => Order.OrderNoProperty.Value = x
                    , (x) => x != Order.OrderNoProperty.RawValue
                    , (x) => NotifyPropertyChanged(new PropertyChangedEventArgs("OrderNo")));
        
            by_=
                new LazyProperty<CustomerView>(this, "By", () =>
                    {
                      return Order.By == null ? null : new CustomerView(Order.By, usePropertyChanged_, writeOnEndEdit_);
                    }
                    , (x) => Order.By = x?.ItemObject
                    , (x) => x?.ItemObject != Order.By
                    , (x) => NotifyPropertyChanged(new PropertyChangedEventArgs("By")));
        
            lines_=
                new LazyValue<ItemObjectViewList<OrderLine, OrderLineView>>(() =>
                    {
                        var list = new ItemObjectViewList<OrderLine, OrderLineView>
                                       (Order.Lines.GetItemObjectCollection(null, null, null)
                                        , (x) => new OrderLineView(x, false, false));
                        list.ListChanged += (object sender, EventArgs e) => NotifyPropertyChanged(new PropertyChangedEventArgs("Lines"));
                        return list;
                    });
            OnInitialize();
        }
        
        public OrderView() : base()
        {
        }
        
        internal OrderView(OrderProcessing.Order order, bool usePropertyChanged, bool writeOnEndEdit) : base(order, usePropertyChanged, writeOnEndEdit)
        {
        }
        
        internal OrderView(OrderView orderView, bool usePropertyChanged, bool writeOnEndEdit) : base(orderView, usePropertyChanged, writeOnEndEdit)
        {
        }
        
        public Order Order => ItemObject;
        
        
        public object CustomerReference
        {
            get => customerReference_.Value;
            set => customerReference_.Value = value;
        }
        
        public object Date
        {
            get => date_.Value;
            set => date_.Value = value;
        }
        
        public object OrderNo
        {
            get => orderNo_.Value;
            set => orderNo_.Value = value;
        }
        
        public CustomerView By
        {
            get => by_.Value;
            set => by_.Value = value;
        }
        
        public ItemObjectViewList<OrderLine, OrderLineView> Lines => lines_.Value;
    }
    
    public partial class OrderLineView : ItemObjectView<OrderProcessing.OrderLine>
    {
    
        LazyProperty<object> quantity_;
        LazyProperty<ProductView> for_;
        LazyProperty<OrderView> on_;
        
        partial void OnInitialize();
        
        protected override void InitializeValues()
        {
            quantity_=
                new LazyProperty<object>(this
                    , "Quantity"
                    , () => OrderLine.QuantityProperty.RawValue
                    , (x) => OrderLine.QuantityProperty.Value = x
                    , (x) => x != OrderLine.QuantityProperty.RawValue
                    , (x) => NotifyPropertyChanged(new PropertyChangedEventArgs("Quantity")));
        
            for_=
                new LazyProperty<ProductView>(this, "For", () =>
                    {
                      return OrderLine.For == null ? null : new ProductView(OrderLine.For, usePropertyChanged_, writeOnEndEdit_);
                    }
                    , (x) => OrderLine.For = x?.ItemObject
                    , (x) => x?.ItemObject != OrderLine.For
                    , (x) => NotifyPropertyChanged(new PropertyChangedEventArgs("For")));
        
            on_=
                new LazyProperty<OrderView>(this, "On", () =>
                    {
                      return OrderLine.On == null ? null : new OrderView(OrderLine.On, usePropertyChanged_, writeOnEndEdit_);
                    }
                    , (x) => OrderLine.On = x?.ItemObject
                    , (x) => x?.ItemObject != OrderLine.On
                    , (x) => NotifyPropertyChanged(new PropertyChangedEventArgs("On")));
        
            OnInitialize();
        }
        
        public OrderLineView() : base()
        {
        }
        
        internal OrderLineView(OrderProcessing.OrderLine orderLine, bool usePropertyChanged, bool writeOnEndEdit) : base(orderLine, usePropertyChanged, writeOnEndEdit)
        {
        }
        
        internal OrderLineView(OrderLineView orderLineView, bool usePropertyChanged, bool writeOnEndEdit) : base(orderLineView, usePropertyChanged, writeOnEndEdit)
        {
        }
        
        public OrderLine OrderLine => ItemObject;
        
        
        public object Quantity
        {
            get => quantity_.Value;
            set => quantity_.Value = value;
        }
        
        public ProductView For
        {
            get => for_.Value;
            set => for_.Value = value;
        }
        
        public OrderView On
        {
            get => on_.Value;
            set => on_.Value = value;
        }
    }
    
    public partial class ProductView : ItemObjectView<OrderProcessing.Product>
    {
    
        LazyProperty<object> code_;
        LazyProperty<object> description_;
        LazyProperty<object> price_;
        LazyProperty<object> stockLevel_;
        LazyProperty<ProductGroupView> is_;
        LazyValue<ItemObjectViewList<ProductGroup, ProductGroupView>> group_;
        LazyValue<ItemObjectViewList<OrderLine, OrderLineView>> orderedOn_;
        
        partial void OnInitialize();
        
        protected override void InitializeValues()
        {
            code_=
                new LazyProperty<object>(this
                    , "Code"
                    , () => Product.CodeProperty.RawValue
                    , (x) => Product.CodeProperty.Value = x
                    , (x) => x != Product.CodeProperty.RawValue
                    , (x) => NotifyPropertyChanged(new PropertyChangedEventArgs("Code")));
        
            description_=
                new LazyProperty<object>(this
                    , "Description"
                    , () => Product.DescriptionProperty.RawValue
                    , (x) => Product.DescriptionProperty.Value = x
                    , (x) => x != Product.DescriptionProperty.RawValue
                    , (x) => NotifyPropertyChanged(new PropertyChangedEventArgs("Description")));
        
            price_=
                new LazyProperty<object>(this
                    , "Price"
                    , () => Product.PriceProperty.RawValue
                    , (x) => Product.PriceProperty.Value = x
                    , (x) => x != Product.PriceProperty.RawValue
                    , (x) => NotifyPropertyChanged(new PropertyChangedEventArgs("Price")));
        
            stockLevel_=
                new LazyProperty<object>(this
                    , "StockLevel"
                    , () => Product.StockLevelProperty.RawValue
                    , (x) => Product.StockLevelProperty.Value = x
                    , (x) => x != Product.StockLevelProperty.RawValue
                    , (x) => NotifyPropertyChanged(new PropertyChangedEventArgs("StockLevel")));
        
            is_=
                new LazyProperty<ProductGroupView>(this, "Is", () =>
                    {
                      return Product.Is == null ? null : new ProductGroupView(Product.Is, usePropertyChanged_, writeOnEndEdit_);
                    }
                    , (x) => Product.Is = x?.ItemObject
                    , (x) => x?.ItemObject != Product.Is
                    , (x) => NotifyPropertyChanged(new PropertyChangedEventArgs("Is")));
        
            group_=
                new LazyValue<ItemObjectViewList<ProductGroup, ProductGroupView>>(() =>
                    {
                        var list = new ItemObjectViewList<ProductGroup, ProductGroupView>
                                       (Product.Group.GetItemObjectCollection(null, null, null)
                                        , (x) => new ProductGroupView(x, false, false));
                        list.ListChanged += (object sender, EventArgs e) => NotifyPropertyChanged(new PropertyChangedEventArgs("Group"));
                        return list;
                    });
            orderedOn_=
                new LazyValue<ItemObjectViewList<OrderLine, OrderLineView>>(() =>
                    {
                        var list = new ItemObjectViewList<OrderLine, OrderLineView>
                                       (Product.OrderedOn.GetItemObjectCollection(null, null, null)
                                        , (x) => new OrderLineView(x, false, false));
                        list.ListChanged += (object sender, EventArgs e) => NotifyPropertyChanged(new PropertyChangedEventArgs("OrderedOn"));
                        return list;
                    });
            OnInitialize();
        }
        
        public ProductView() : base()
        {
        }
        
        internal ProductView(OrderProcessing.Product product, bool usePropertyChanged, bool writeOnEndEdit) : base(product, usePropertyChanged, writeOnEndEdit)
        {
        }
        
        internal ProductView(ProductView productView, bool usePropertyChanged, bool writeOnEndEdit) : base(productView, usePropertyChanged, writeOnEndEdit)
        {
        }
        
        public Product Product => ItemObject;
        
        
        public object Code
        {
            get => code_.Value;
            set => code_.Value = value;
        }
        
        public object Description
        {
            get => description_.Value;
            set => description_.Value = value;
        }
        
        public object Price
        {
            get => price_.Value;
            set => price_.Value = value;
        }
        
        public object StockLevel
        {
            get => stockLevel_.Value;
            set => stockLevel_.Value = value;
        }
        
        public ProductGroupView Is
        {
            get => is_.Value;
            set => is_.Value = value;
        }
        
        public ItemObjectViewList<ProductGroup, ProductGroupView> Group => group_.Value;
        
        public ItemObjectViewList<OrderLine, OrderLineView> OrderedOn => orderedOn_.Value;
    }
    
    public partial class ProductGroupView : ItemObjectView<OrderProcessing.ProductGroup>
    {
    
        LazyProperty<object> description_;
        LazyProperty<object> name_;
        LazyProperty<ProductGroupView> parentGroup_;
        LazyValue<ItemObjectViewList<Product, ProductView>> contains_;
        
        partial void OnInitialize();
        
        protected override void InitializeValues()
        {
            description_=
                new LazyProperty<object>(this
                    , "Description"
                    , () => ProductGroup.DescriptionProperty.RawValue
                    , (x) => ProductGroup.DescriptionProperty.Value = x
                    , (x) => x != ProductGroup.DescriptionProperty.RawValue
                    , (x) => NotifyPropertyChanged(new PropertyChangedEventArgs("Description")));
        
            name_=
                new LazyProperty<object>(this
                    , "Name"
                    , () => ProductGroup.NameProperty.RawValue
                    , (x) => ProductGroup.NameProperty.Value = x
                    , (x) => x != ProductGroup.NameProperty.RawValue
                    , (x) => NotifyPropertyChanged(new PropertyChangedEventArgs("Name")));
        
            parentGroup_=
                new LazyProperty<ProductGroupView>(this, "ParentGroup", () =>
                    {
                      return ProductGroup.ParentGroup == null ? null : new ProductGroupView(ProductGroup.ParentGroup, usePropertyChanged_, writeOnEndEdit_);
                    }
                    , (x) => ProductGroup.ParentGroup = x?.ItemObject
                    , (x) => x?.ItemObject != ProductGroup.ParentGroup
                    , (x) => NotifyPropertyChanged(new PropertyChangedEventArgs("ParentGroup")));
        
            contains_=
                new LazyValue<ItemObjectViewList<Product, ProductView>>(() =>
                    {
                        var list = new ItemObjectViewList<Product, ProductView>
                                       (ProductGroup.Contains.GetItemObjectCollection(null, null, null)
                                        , (x) => new ProductView(x, false, false));
                        list.ListChanged += (object sender, EventArgs e) => NotifyPropertyChanged(new PropertyChangedEventArgs("Contains"));
                        return list;
                    });
            OnInitialize();
        }
        
        public ProductGroupView() : base()
        {
        }
        
        internal ProductGroupView(OrderProcessing.ProductGroup productGroup, bool usePropertyChanged, bool writeOnEndEdit) : base(productGroup, usePropertyChanged, writeOnEndEdit)
        {
        }
        
        internal ProductGroupView(ProductGroupView productGroupView, bool usePropertyChanged, bool writeOnEndEdit) : base(productGroupView, usePropertyChanged, writeOnEndEdit)
        {
        }
        
        public ProductGroup ProductGroup => ItemObject;
        
        
        public object Description
        {
            get => description_.Value;
            set => description_.Value = value;
        }
        
        public object Name
        {
            get => name_.Value;
            set => name_.Value = value;
        }
        
        public ProductGroupView ParentGroup
        {
            get => parentGroup_.Value;
            set => parentGroup_.Value = value;
        }
        
        public ItemObjectViewList<Product, ProductView> Contains => contains_.Value;
    }
    
}
