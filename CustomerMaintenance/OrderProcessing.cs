using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Semata.DataStore;
using Semata.DataStore.ObjectModel;
using System.ComponentModel;

namespace CustomerMaintenance
{
    public class OrderProcessing : DataStoreObject<OrderProcessing>
    {
        
        public Items<OrderProcessing, Customer, ObservableCollection<Customer>> CustomerItems {get; private set;}
        public Items<OrderProcessing, Order, ObservableCollection<Order>> OrderItems {get; private set;}
        public Items<OrderProcessing, OrderLine, ObservableCollection<OrderLine>> OrderLineItems {get; private set;}
        public Items<OrderProcessing, Product, ObservableCollection<Product>> ProductItems {get; private set;}
        public Items<OrderProcessing, ProductGroup, ObservableCollection<ProductGroup>> ProductGroupItems {get; private set;}
        
        public void Open(string path)
        {
            base.OpenInstance(path);
            SetActivator("Customer", (initializer) => new Customer(initializer));
            SetActivator("Order", (initializer) => new Order(initializer));
            SetActivator("Order Line", (initializer) => new OrderLine(initializer));
            SetActivator("Product", (initializer) => new Product(initializer));
            SetActivator("Product Group", (initializer) => new ProductGroup(initializer));
            CustomerItems = new Items<OrderProcessing, Customer, ObservableCollection<Customer>>(connection_.GetItemType("Customer"), this);
            OrderItems = new Items<OrderProcessing, Order, ObservableCollection<Order>>(connection_.GetItemType("Order"), this);
            OrderLineItems = new Items<OrderProcessing, OrderLine, ObservableCollection<OrderLine>>(connection_.GetItemType("Order Line"), this);
            ProductItems = new Items<OrderProcessing, Product, ObservableCollection<Product>>(connection_.GetItemType("Product"), this);
            ProductGroupItems = new Items<OrderProcessing, ProductGroup, ObservableCollection<ProductGroup>>(connection_.GetItemType("Product Group"), this);
        }
        
        public void Close(string path)
        {
            base.CloseInstance();
        }
        
    }
    
    public class Customer : ItemObject<OrderProcessing>
    {
        public Attribute<OrderProcessing, string> AddressLine1 { get; private set;}
        public Attribute<OrderProcessing, string> AddressLine2 { get; private set;}
        public Attribute<OrderProcessing, string> AddressLine3 { get; private set;}
        public Attribute<OrderProcessing, string> Code { get; private set;}
        public Attribute<OrderProcessing, string> Name { get; private set;}
        public Attribute<OrderProcessing, string> PostCode { get; private set;}
        public Attribute<OrderProcessing, string> PostTown { get; private set;}
        
        public Associations<OrderProcessing, Order, ObservableCollection<Order>> Has { get; private set;}
        
        internal Customer(ItemObjectInitializer<OrderProcessing> initializer) : base(initializer)
        {
            AddressLine1 = new Attribute<OrderProcessing, string>(this, "Address Line 1", "AddressLine1");
            AddAttribute(AddressLine1);
            AddressLine2 = new Attribute<OrderProcessing, string>(this, "Address Line 2", "AddressLine2");
            AddAttribute(AddressLine2);
            AddressLine3 = new Attribute<OrderProcessing, string>(this, "Address Line 3", "AddressLine3");
            AddAttribute(AddressLine3);
            Code = new Attribute<OrderProcessing, string>(this, "Code", "Code");
            AddAttribute(Code);
            Name = new Attribute<OrderProcessing, string>(this, "Name", "Name");
            AddAttribute(Name);
            PostCode = new Attribute<OrderProcessing, string>(this, "Post Code", "PostCode");
            AddAttribute(PostCode);
            PostTown = new Attribute<OrderProcessing, string>(this, "Post Town", "PostTown");
            AddAttribute(PostTown);
            Has = new Associations<OrderProcessing, Order, ObservableCollection<Order>>(this, "Has");
        }
        
    }
    
    public class Order : ItemObject<OrderProcessing>
    {
        public Attribute<OrderProcessing, string> CustomerReference { get; private set;}
        public Attribute<OrderProcessing, DateTimeOffset> Date { get; private set;}
        public Attribute<OrderProcessing, string> OrderNo { get; private set;}
        
        public Associations<OrderProcessing, Customer, ObservableCollection<Customer>> By { get; private set;}
        public Associations<OrderProcessing, OrderLine, ObservableCollection<OrderLine>> Lines { get; private set;}
        
        internal Order(ItemObjectInitializer<OrderProcessing> initializer) : base(initializer)
        {
            CustomerReference = new Attribute<OrderProcessing, string>(this, "Customer Reference", "CustomerReference");
            AddAttribute(CustomerReference);
            Date = new Attribute<OrderProcessing, DateTimeOffset>(this, "Date", "Date");
            AddAttribute(Date);
            OrderNo = new Attribute<OrderProcessing, string>(this, "Order No", "OrderNo");
            AddAttribute(OrderNo);
            By = new Associations<OrderProcessing, Customer, ObservableCollection<Customer>>(this, "By");
            Lines = new Associations<OrderProcessing, OrderLine, ObservableCollection<OrderLine>>(this, "Lines");
        }
        
    }
    
    public class OrderLine : ItemObject<OrderProcessing>
    {
        public Attribute<OrderProcessing, int> Quantity { get; private set;}
        
        public Associations<OrderProcessing, Product, ObservableCollection<Product>> For { get; private set;}
        public Associations<OrderProcessing, Order, ObservableCollection<Order>> On { get; private set;}
        
        internal OrderLine(ItemObjectInitializer<OrderProcessing> initializer) : base(initializer)
        {
            Quantity = new Attribute<OrderProcessing, int>(this, "Quantity", "Quantity");
            AddAttribute(Quantity);
            For = new Associations<OrderProcessing, Product, ObservableCollection<Product>>(this, "For");
            On = new Associations<OrderProcessing, Order, ObservableCollection<Order>>(this, "On");
        }
        
    }
    
    public class Product : ItemObject<OrderProcessing>
    {
        public Attribute<OrderProcessing, string> Code { get; private set;}
        public Attribute<OrderProcessing, string> Description { get; private set;}
        public Attribute<OrderProcessing, decimal> Price { get; private set;}
        public Attribute<OrderProcessing, int> StockLevel { get; private set;}
        
        public Associations<OrderProcessing, ProductGroup, ObservableCollection<ProductGroup>> Group { get; private set;}
        public Associations<OrderProcessing, OrderLine, ObservableCollection<OrderLine>> OrderedOn { get; private set;}
        
        internal Product(ItemObjectInitializer<OrderProcessing> initializer) : base(initializer)
        {
            Code = new Attribute<OrderProcessing, string>(this, "Code", "Code");
            AddAttribute(Code);
            Description = new Attribute<OrderProcessing, string>(this, "Description", "Description");
            AddAttribute(Description);
            Price = new Attribute<OrderProcessing, decimal>(this, "Price", "Price");
            AddAttribute(Price);
            StockLevel = new Attribute<OrderProcessing, int>(this, "Stock Level", "StockLevel");
            AddAttribute(StockLevel);
            Group = new Associations<OrderProcessing, ProductGroup, ObservableCollection<ProductGroup>>(this, "Group");
            OrderedOn = new Associations<OrderProcessing, OrderLine, ObservableCollection<OrderLine>>(this, "Ordered On");
        }
        
    }
    
    public class ProductGroup : ItemObject<OrderProcessing>
    {
        public Attribute<OrderProcessing, string> Description { get; private set;}
        public Attribute<OrderProcessing, string> Name { get; private set;}
        
        public Associations<OrderProcessing, ProductGroup, ObservableCollection<ProductGroup>> ParentGroup { get; private set;}
        public Associations<OrderProcessing, ItemObject<OrderProcessing>, ObservableCollection<ItemObject<OrderProcessing>>> SubGroups { get; private set;}
        
        internal ProductGroup(ItemObjectInitializer<OrderProcessing> initializer) : base(initializer)
        {
            Description = new Attribute<OrderProcessing, string>(this, "Description", "Description");
            AddAttribute(Description);
            Name = new Attribute<OrderProcessing, string>(this, "Name", "Name");
            AddAttribute(Name);
            ParentGroup = new Associations<OrderProcessing, ProductGroup, ObservableCollection<ProductGroup>>(this, "Parent Group");
            SubGroups = new Associations<OrderProcessing, ItemObject<OrderProcessing>, ObservableCollection<ItemObject<OrderProcessing>>>(this, "Sub Groups");
        }
        
    }
    
}
