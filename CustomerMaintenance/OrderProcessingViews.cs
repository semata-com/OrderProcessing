using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semata.DataStore.ObjectModel;
using Semata.DataStore.ObjectModel.Views;
using OrderProcessingDS;
using Semata.DataView;


namespace CustomerMaintenance
{
    public partial class OrderProcessingView : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        protected OrderProcessing dataStore_;
        
        ItemObjectViewBindingList<CustomerView> customer_ = null;
        ItemObjectViewBindingList<OrderView> order_ = null;
        ItemObjectViewBindingList<OrderLineView> orderLine_ = null;
        ItemObjectViewBindingList<ProductView> product_ = null;
        ItemObjectViewBindingList<ProductGroupView> productGroup_ = null;
        
        public OrderProcessingView(PropertyChangedEventDispatcher eventDispatcher)
        {
            dataStore_ = new OrderProcessing(eventDispatcher);
        }
        
        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, args);
            }
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
        
        public ItemObjectViewBindingList<CustomerView> CustomerItems
        {
            get
            {
                if (customer_ == null)
                {
                    customer_= ItemObjectViewBindingListFactory.Create(dataStore_.CustomerItems.GetItemObjectSet()
                                                                                      , (x) => { return new CustomerView(x, false, false); }
                                                                                      , false);
                }
                return customer_;
            }
        }
        public ItemObjectViewBindingList<OrderView> OrderItems
        {
            get
            {
                if (order_ == null)
                {
                    order_= ItemObjectViewBindingListFactory.Create(dataStore_.OrderItems.GetItemObjectSet()
                                                                                      , (x) => { return new OrderView(x, false, false); }
                                                                                      , false);
                }
                return order_;
            }
        }
        public ItemObjectViewBindingList<OrderLineView> OrderLineItems
        {
            get
            {
                if (orderLine_ == null)
                {
                    orderLine_= ItemObjectViewBindingListFactory.Create(dataStore_.OrderLineItems.GetItemObjectSet()
                                                                                      , (x) => { return new OrderLineView(x, false, false); }
                                                                                      , false);
                }
                return orderLine_;
            }
        }
        public ItemObjectViewBindingList<ProductView> ProductItems
        {
            get
            {
                if (product_ == null)
                {
                    product_= ItemObjectViewBindingListFactory.Create(dataStore_.ProductItems.GetItemObjectSet()
                                                                                      , (x) => { return new ProductView(x, false, false); }
                                                                                      , false);
                }
                return product_;
            }
        }
        public ItemObjectViewBindingList<ProductGroupView> ProductGroupItems
        {
            get
            {
                if (productGroup_ == null)
                {
                    productGroup_= ItemObjectViewBindingListFactory.Create(dataStore_.ProductGroupItems.GetItemObjectSet()
                                                                                      , (x) => { return new ProductGroupView(x, false, false); }
                                                                                      , false);
                }
                return productGroup_;
            }
        }
}

    public partial class CustomerView : ItemObjectView<OrderProcessingDS.Customer>
    {
    
        ValueProperty addressLine1_;
        ValueProperty addressLine2_;
        ValueProperty addressLine3_;
        ValueProperty code_;
        ValueProperty name_;
        ValueProperty postCode_;
        ValueProperty postTown_;
        ItemObjectViewBindingList<OrderView> has_ = null;
        internal CustomerView(OrderProcessingDS.Customer customer, bool usePropertyChanged, bool writeOnEndEdit) : base(customer, usePropertyChanged, writeOnEndEdit)
        {
        }
        
        internal CustomerView(CustomerView customerView, bool usePropertyChanged, bool writeOnEndEdit) : base(customerView, usePropertyChanged, writeOnEndEdit)
        {
        }
        
        
        public object AddressLine1
        {
            get
            {
                if (addressLine1_ == null)
                {
                    addressLine1_ = itemObject_.ValueProperties["AddressLine1"];
                }
                  return addressLine1_ .RawValue;
            }
            set
            {
                if (addressLine1_ == null)
                {
                    addressLine1_ = itemObject_.ValueProperties["AddressLine1"];
                }
                addressLine1_ .Value = value;
            }
        }
        
        public object AddressLine2
        {
            get
            {
                if (addressLine2_ == null)
                {
                    addressLine2_ = itemObject_.ValueProperties["AddressLine2"];
                }
                  return addressLine2_ .RawValue;
            }
            set
            {
                if (addressLine2_ == null)
                {
                    addressLine2_ = itemObject_.ValueProperties["AddressLine2"];
                }
                addressLine2_ .Value = value;
            }
        }
        
        public object AddressLine3
        {
            get
            {
                if (addressLine3_ == null)
                {
                    addressLine3_ = itemObject_.ValueProperties["AddressLine3"];
                }
                  return addressLine3_ .RawValue;
            }
            set
            {
                if (addressLine3_ == null)
                {
                    addressLine3_ = itemObject_.ValueProperties["AddressLine3"];
                }
                addressLine3_ .Value = value;
            }
        }
        
        public object Code
        {
            get
            {
                if (code_ == null)
                {
                    code_ = itemObject_.ValueProperties["Code"];
                }
                  return code_ .RawValue;
            }
            set
            {
                if (code_ == null)
                {
                    code_ = itemObject_.ValueProperties["Code"];
                }
                code_ .Value = value;
            }
        }
        
        public object Name
        {
            get
            {
                if (name_ == null)
                {
                    name_ = itemObject_.ValueProperties["Name"];
                }
                  return name_ .RawValue;
            }
            set
            {
                if (name_ == null)
                {
                    name_ = itemObject_.ValueProperties["Name"];
                }
                name_ .Value = value;
            }
        }
        
        public object PostCode
        {
            get
            {
                if (postCode_ == null)
                {
                    postCode_ = itemObject_.ValueProperties["PostCode"];
                }
                  return postCode_ .RawValue;
            }
            set
            {
                if (postCode_ == null)
                {
                    postCode_ = itemObject_.ValueProperties["PostCode"];
                }
                postCode_ .Value = value;
            }
        }
        
        public object PostTown
        {
            get
            {
                if (postTown_ == null)
                {
                    postTown_ = itemObject_.ValueProperties["PostTown"];
                }
                  return postTown_ .RawValue;
            }
            set
            {
                if (postTown_ == null)
                {
                    postTown_ = itemObject_.ValueProperties["PostTown"];
                }
                postTown_ .Value = value;
            }
        }
        public ItemObjectViewBindingList<OrderView> Has
        {
            get
            {
                if (has_ == null)
                {
                    has_= ItemObjectViewBindingListFactory.Create(this.ItemObject.Has.GetItemObjectSet()
                                                                                         , (x) => { return new OrderView(x, false, false); }
                                                                                         , false);
                }
                return has_;
            }
        }
    }
    
    public partial class OrderView : ItemObjectView<OrderProcessingDS.Order>
    {
    
        ValueProperty customerReference_;
        ValueProperty date_;
        ValueProperty orderNo_;
        ItemObjectViewBindingList<CustomerView> by_ = null;
        ItemObjectViewBindingList<OrderLineView> lines_ = null;
        internal OrderView(OrderProcessingDS.Order order, bool usePropertyChanged, bool writeOnEndEdit) : base(order, usePropertyChanged, writeOnEndEdit)
        {
        }
        
        internal OrderView(OrderView orderView, bool usePropertyChanged, bool writeOnEndEdit) : base(orderView, usePropertyChanged, writeOnEndEdit)
        {
        }
        
        
        public object CustomerReference
        {
            get
            {
                if (customerReference_ == null)
                {
                    customerReference_ = itemObject_.ValueProperties["CustomerReference"];
                }
                  return customerReference_ .RawValue;
            }
            set
            {
                if (customerReference_ == null)
                {
                    customerReference_ = itemObject_.ValueProperties["CustomerReference"];
                }
                customerReference_ .Value = value;
            }
        }
        
        public object Date
        {
            get
            {
                if (date_ == null)
                {
                    date_ = itemObject_.ValueProperties["Date"];
                }
                  return date_ .RawValue;
            }
            set
            {
                if (date_ == null)
                {
                    date_ = itemObject_.ValueProperties["Date"];
                }
                date_ .Value = value;
            }
        }
        
        public object OrderNo
        {
            get
            {
                if (orderNo_ == null)
                {
                    orderNo_ = itemObject_.ValueProperties["OrderNo"];
                }
                  return orderNo_ .RawValue;
            }
            set
            {
                if (orderNo_ == null)
                {
                    orderNo_ = itemObject_.ValueProperties["OrderNo"];
                }
                orderNo_ .Value = value;
            }
        }
        public ItemObjectViewBindingList<CustomerView> By
        {
            get
            {
                if (by_ == null)
                {
                    by_= ItemObjectViewBindingListFactory.Create(this.ItemObject.By.GetItemObjectSet()
                                                                                         , (x) => { return new CustomerView(x, false, false); }
                                                                                         , false);
                }
                return by_;
            }
        }
        public ItemObjectViewBindingList<OrderLineView> Lines
        {
            get
            {
                if (lines_ == null)
                {
                    lines_= ItemObjectViewBindingListFactory.Create(this.ItemObject.Lines.GetItemObjectSet()
                                                                                         , (x) => { return new OrderLineView(x, false, false); }
                                                                                         , false);
                }
                return lines_;
            }
        }
    }
    
    public partial class OrderLineView : ItemObjectView<OrderProcessingDS.OrderLine>
    {
    
        ValueProperty quantity_;
        ItemObjectViewBindingList<ProductView> for_ = null;
        ItemObjectViewBindingList<OrderView> on_ = null;
        internal OrderLineView(OrderProcessingDS.OrderLine orderLine, bool usePropertyChanged, bool writeOnEndEdit) : base(orderLine, usePropertyChanged, writeOnEndEdit)
        {
        }
        
        internal OrderLineView(OrderLineView orderLineView, bool usePropertyChanged, bool writeOnEndEdit) : base(orderLineView, usePropertyChanged, writeOnEndEdit)
        {
        }
        
        
        public object Quantity
        {
            get
            {
                if (quantity_ == null)
                {
                    quantity_ = itemObject_.ValueProperties["Quantity"];
                }
                  return quantity_ .RawValue;
            }
            set
            {
                if (quantity_ == null)
                {
                    quantity_ = itemObject_.ValueProperties["Quantity"];
                }
                quantity_ .Value = value;
            }
        }
        public ItemObjectViewBindingList<ProductView> For
        {
            get
            {
                if (for_ == null)
                {
                    for_= ItemObjectViewBindingListFactory.Create(this.ItemObject.For.GetItemObjectSet()
                                                                                         , (x) => { return new ProductView(x, false, false); }
                                                                                         , false);
                }
                return for_;
            }
        }
        public ItemObjectViewBindingList<OrderView> On
        {
            get
            {
                if (on_ == null)
                {
                    on_= ItemObjectViewBindingListFactory.Create(this.ItemObject.On.GetItemObjectSet()
                                                                                         , (x) => { return new OrderView(x, false, false); }
                                                                                         , false);
                }
                return on_;
            }
        }
    }
    
    public partial class ProductView : ItemObjectView<OrderProcessingDS.Product>
    {
    
        ValueProperty code_;
        ValueProperty description_;
        ValueProperty price_;
        ValueProperty stockLevel_;
        ItemObjectViewBindingList<ProductGroupView> group_ = null;
        ItemObjectViewBindingList<ProductGroupView> is_ = null;
        ItemObjectViewBindingList<OrderLineView> orderedOn_ = null;
        internal ProductView(OrderProcessingDS.Product product, bool usePropertyChanged, bool writeOnEndEdit) : base(product, usePropertyChanged, writeOnEndEdit)
        {
        }
        
        internal ProductView(ProductView productView, bool usePropertyChanged, bool writeOnEndEdit) : base(productView, usePropertyChanged, writeOnEndEdit)
        {
        }
        
        
        public object Code
        {
            get
            {
                if (code_ == null)
                {
                    code_ = itemObject_.ValueProperties["Code"];
                }
                  return code_ .RawValue;
            }
            set
            {
                if (code_ == null)
                {
                    code_ = itemObject_.ValueProperties["Code"];
                }
                code_ .Value = value;
            }
        }
        
        public object Description
        {
            get
            {
                if (description_ == null)
                {
                    description_ = itemObject_.ValueProperties["Description"];
                }
                  return description_ .RawValue;
            }
            set
            {
                if (description_ == null)
                {
                    description_ = itemObject_.ValueProperties["Description"];
                }
                description_ .Value = value;
            }
        }
        
        public object Price
        {
            get
            {
                if (price_ == null)
                {
                    price_ = itemObject_.ValueProperties["Price"];
                }
                  return price_ .RawValue;
            }
            set
            {
                if (price_ == null)
                {
                    price_ = itemObject_.ValueProperties["Price"];
                }
                price_ .Value = value;
            }
        }
        
        public object StockLevel
        {
            get
            {
                if (stockLevel_ == null)
                {
                    stockLevel_ = itemObject_.ValueProperties["StockLevel"];
                }
                  return stockLevel_ .RawValue;
            }
            set
            {
                if (stockLevel_ == null)
                {
                    stockLevel_ = itemObject_.ValueProperties["StockLevel"];
                }
                stockLevel_ .Value = value;
            }
        }
        public ItemObjectViewBindingList<ProductGroupView> Group
        {
            get
            {
                if (group_ == null)
                {
                    group_= ItemObjectViewBindingListFactory.Create(this.ItemObject.Group.GetItemObjectSet()
                                                                                         , (x) => { return new ProductGroupView(x, false, false); }
                                                                                         , false);
                }
                return group_;
            }
        }
        public ItemObjectViewBindingList<ProductGroupView> Is
        {
            get
            {
                if (is_ == null)
                {
                    is_= ItemObjectViewBindingListFactory.Create(this.ItemObject.Is.GetItemObjectSet()
                                                                                         , (x) => { return new ProductGroupView(x, false, false); }
                                                                                         , false);
                }
                return is_;
            }
        }
        public ItemObjectViewBindingList<OrderLineView> OrderedOn
        {
            get
            {
                if (orderedOn_ == null)
                {
                    orderedOn_= ItemObjectViewBindingListFactory.Create(this.ItemObject.OrderedOn.GetItemObjectSet()
                                                                                         , (x) => { return new OrderLineView(x, false, false); }
                                                                                         , false);
                }
                return orderedOn_;
            }
        }
    }
    
    public partial class ProductGroupView : ItemObjectView<OrderProcessingDS.ProductGroup>
    {
    
        ValueProperty description_;
        ValueProperty name_;
        ItemObjectViewBindingList<ProductView> contains_ = null;
        ItemObjectViewBindingList<ProductGroupView> parentGroup_ = null;
        internal ProductGroupView(OrderProcessingDS.ProductGroup productGroup, bool usePropertyChanged, bool writeOnEndEdit) : base(productGroup, usePropertyChanged, writeOnEndEdit)
        {
        }
        
        internal ProductGroupView(ProductGroupView productGroupView, bool usePropertyChanged, bool writeOnEndEdit) : base(productGroupView, usePropertyChanged, writeOnEndEdit)
        {
        }
        
        
        public object Description
        {
            get
            {
                if (description_ == null)
                {
                    description_ = itemObject_.ValueProperties["Description"];
                }
                  return description_ .RawValue;
            }
            set
            {
                if (description_ == null)
                {
                    description_ = itemObject_.ValueProperties["Description"];
                }
                description_ .Value = value;
            }
        }
        
        public object Name
        {
            get
            {
                if (name_ == null)
                {
                    name_ = itemObject_.ValueProperties["Name"];
                }
                  return name_ .RawValue;
            }
            set
            {
                if (name_ == null)
                {
                    name_ = itemObject_.ValueProperties["Name"];
                }
                name_ .Value = value;
            }
        }
        public ItemObjectViewBindingList<ProductView> Contains
        {
            get
            {
                if (contains_ == null)
                {
                    contains_= ItemObjectViewBindingListFactory.Create(this.ItemObject.Contains.GetItemObjectSet()
                                                                                         , (x) => { return new ProductView(x, false, false); }
                                                                                         , false);
                }
                return contains_;
            }
        }
        public ItemObjectViewBindingList<ProductGroupView> ParentGroup
        {
            get
            {
                if (parentGroup_ == null)
                {
                    parentGroup_= ItemObjectViewBindingListFactory.Create(this.ItemObject.ParentGroup.GetItemObjectSet()
                                                                                         , (x) => { return new ProductGroupView(x, false, false); }
                                                                                         , false);
                }
                return parentGroup_;
            }
        }
    }
    
}
