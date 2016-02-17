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
        private Attribute<OrderProcessing, string> AddressLine1Attribute { get; set;}
        private Attribute<OrderProcessing, string> AddressLine2Attribute { get; set;}
        private Attribute<OrderProcessing, string> AddressLine3Attribute { get; set;}
        private Attribute<OrderProcessing, string> CodeAttribute { get; set;}
        private Attribute<OrderProcessing, string> NameAttribute { get; set;}
        private Attribute<OrderProcessing, string> PostCodeAttribute { get; set;}
        private Attribute<OrderProcessing, string> PostTownAttribute { get; set;}
        
        public Associations<OrderProcessing, Order, ObservableCollection<Order>> Has { get; private set;}
        
        internal Customer(ItemObjectInitializer<OrderProcessing> initializer) : base(initializer)
        {
            AddressLine1Attribute = new Attribute<OrderProcessing, string>(this, "Address Line 1", "AddressLine1");
            AddAttribute(AddressLine1Attribute);
            AddressLine2Attribute = new Attribute<OrderProcessing, string>(this, "Address Line 2", "AddressLine2");
            AddAttribute(AddressLine2Attribute);
            AddressLine3Attribute = new Attribute<OrderProcessing, string>(this, "Address Line 3", "AddressLine3");
            AddAttribute(AddressLine3Attribute);
            CodeAttribute = new Attribute<OrderProcessing, string>(this, "Code", "Code");
            AddAttribute(CodeAttribute);
            NameAttribute = new Attribute<OrderProcessing, string>(this, "Name", "Name");
            AddAttribute(NameAttribute);
            PostCodeAttribute = new Attribute<OrderProcessing, string>(this, "Post Code", "PostCode");
            AddAttribute(PostCodeAttribute);
            PostTownAttribute = new Attribute<OrderProcessing, string>(this, "Post Town", "PostTown");
            AddAttribute(PostTownAttribute);
            Has = new Associations<OrderProcessing, Order, ObservableCollection<Order>>(this, "Has", "Has");
        }
        
        public object AddressLine1
        {
            get
            {
                return AddressLine1Attribute.Value;
            }
            set
            {
                AddressLine1Attribute.Value = value;
            }
        }
        
        
        public object AddressLine2
        {
            get
            {
                return AddressLine2Attribute.Value;
            }
            set
            {
                AddressLine2Attribute.Value = value;
            }
        }
        
        
        public object AddressLine3
        {
            get
            {
                return AddressLine3Attribute.Value;
            }
            set
            {
                AddressLine3Attribute.Value = value;
            }
        }
        
        
        public object Code
        {
            get
            {
                return CodeAttribute.Value;
            }
            set
            {
                CodeAttribute.Value = value;
            }
        }
        
        
        public object Name
        {
            get
            {
                return NameAttribute.Value;
            }
            set
            {
                NameAttribute.Value = value;
            }
        }
        
        
        public object PostCode
        {
            get
            {
                return PostCodeAttribute.Value;
            }
            set
            {
                PostCodeAttribute.Value = value;
            }
        }
        
        
        public object PostTown
        {
            get
            {
                return PostTownAttribute.Value;
            }
            set
            {
                PostTownAttribute.Value = value;
            }
        }
        
    }
    
    public class Order : ItemObject<OrderProcessing>
    {
        private Attribute<OrderProcessing, string> CustomerReferenceAttribute { get; set;}
        private Attribute<OrderProcessing, DateTimeOffset> DateAttribute { get; set;}
        private Attribute<OrderProcessing, string> OrderNoAttribute { get; set;}
        
        public Associations<OrderProcessing, Customer, ObservableCollection<Customer>> By { get; private set;}
        public Associations<OrderProcessing, OrderLine, ObservableCollection<OrderLine>> Lines { get; private set;}
        
        internal Order(ItemObjectInitializer<OrderProcessing> initializer) : base(initializer)
        {
            CustomerReferenceAttribute = new Attribute<OrderProcessing, string>(this, "Customer Reference", "CustomerReference");
            AddAttribute(CustomerReferenceAttribute);
            DateAttribute = new Attribute<OrderProcessing, DateTimeOffset>(this, "Date", "Date");
            AddAttribute(DateAttribute);
            OrderNoAttribute = new Attribute<OrderProcessing, string>(this, "Order No", "OrderNo");
            AddAttribute(OrderNoAttribute);
            By = new Associations<OrderProcessing, Customer, ObservableCollection<Customer>>(this, "By", "By");
            Lines = new Associations<OrderProcessing, OrderLine, ObservableCollection<OrderLine>>(this, "Lines", "Lines");
        }
        
        public object CustomerReference
        {
            get
            {
                return CustomerReferenceAttribute.Value;
            }
            set
            {
                CustomerReferenceAttribute.Value = value;
            }
        }
        
        
        public object Date
        {
            get
            {
                return DateAttribute.Value;
            }
            set
            {
                DateAttribute.Value = value;
            }
        }
        
        
        public object OrderNo
        {
            get
            {
                return OrderNoAttribute.Value;
            }
            set
            {
                OrderNoAttribute.Value = value;
            }
        }
        
    }
    
    public class OrderLine : ItemObject<OrderProcessing>
    {
        private Attribute<OrderProcessing, int> QuantityAttribute { get; set;}
        
        public Associations<OrderProcessing, Product, ObservableCollection<Product>> For { get; private set;}
        public Associations<OrderProcessing, Order, ObservableCollection<Order>> On { get; private set;}
        
        internal OrderLine(ItemObjectInitializer<OrderProcessing> initializer) : base(initializer)
        {
            QuantityAttribute = new Attribute<OrderProcessing, int>(this, "Quantity", "Quantity");
            AddAttribute(QuantityAttribute);
            For = new Associations<OrderProcessing, Product, ObservableCollection<Product>>(this, "For", "For");
            On = new Associations<OrderProcessing, Order, ObservableCollection<Order>>(this, "On", "On");
        }
        
        public object Quantity
        {
            get
            {
                return QuantityAttribute.Value;
            }
            set
            {
                QuantityAttribute.Value = value;
            }
        }
        
    }
    
    public class Product : ItemObject<OrderProcessing>
    {
        private Attribute<OrderProcessing, string> CodeAttribute { get; set;}
        private Attribute<OrderProcessing, string> DescriptionAttribute { get; set;}
        private Attribute<OrderProcessing, decimal> PriceAttribute { get; set;}
        private Attribute<OrderProcessing, int> StockLevelAttribute { get; set;}
        
        public Associations<OrderProcessing, ProductGroup, ObservableCollection<ProductGroup>> Group { get; private set;}
        public Associations<OrderProcessing, OrderLine, ObservableCollection<OrderLine>> OrderedOn { get; private set;}
        
        internal Product(ItemObjectInitializer<OrderProcessing> initializer) : base(initializer)
        {
            CodeAttribute = new Attribute<OrderProcessing, string>(this, "Code", "Code");
            AddAttribute(CodeAttribute);
            DescriptionAttribute = new Attribute<OrderProcessing, string>(this, "Description", "Description");
            AddAttribute(DescriptionAttribute);
            PriceAttribute = new Attribute<OrderProcessing, decimal>(this, "Price", "Price");
            AddAttribute(PriceAttribute);
            StockLevelAttribute = new Attribute<OrderProcessing, int>(this, "Stock Level", "StockLevel");
            AddAttribute(StockLevelAttribute);
            Group = new Associations<OrderProcessing, ProductGroup, ObservableCollection<ProductGroup>>(this, "Group", "Group");
            OrderedOn = new Associations<OrderProcessing, OrderLine, ObservableCollection<OrderLine>>(this, "Ordered On", "OrderedOn");
        }
        
        public object Code
        {
            get
            {
                return CodeAttribute.Value;
            }
            set
            {
                CodeAttribute.Value = value;
            }
        }
        
        
        public object Description
        {
            get
            {
                return DescriptionAttribute.Value;
            }
            set
            {
                DescriptionAttribute.Value = value;
            }
        }
        
        
        public object Price
        {
            get
            {
                return PriceAttribute.Value;
            }
            set
            {
                PriceAttribute.Value = value;
            }
        }
        
        
        public object StockLevel
        {
            get
            {
                return StockLevelAttribute.Value;
            }
            set
            {
                StockLevelAttribute.Value = value;
            }
        }
        
    }
    
    public class ProductGroup : ItemObject<OrderProcessing>
    {
        private Attribute<OrderProcessing, string> DescriptionAttribute { get; set;}
        private Attribute<OrderProcessing, string> NameAttribute { get; set;}
        
        public Associations<OrderProcessing, ProductGroup, ObservableCollection<ProductGroup>> ParentGroup { get; private set;}
        public Associations<OrderProcessing, ItemObject<OrderProcessing>, ObservableCollection<ItemObject<OrderProcessing>>> SubGroups { get; private set;}
        
        internal ProductGroup(ItemObjectInitializer<OrderProcessing> initializer) : base(initializer)
        {
            DescriptionAttribute = new Attribute<OrderProcessing, string>(this, "Description", "Description");
            AddAttribute(DescriptionAttribute);
            NameAttribute = new Attribute<OrderProcessing, string>(this, "Name", "Name");
            AddAttribute(NameAttribute);
            ParentGroup = new Associations<OrderProcessing, ProductGroup, ObservableCollection<ProductGroup>>(this, "Parent Group", "ParentGroup");
            SubGroups = new Associations<OrderProcessing, ItemObject<OrderProcessing>, ObservableCollection<ItemObject<OrderProcessing>>>(this, "Sub Groups", "SubGroups");
        }
        
        public object Description
        {
            get
            {
                return DescriptionAttribute.Value;
            }
            set
            {
                DescriptionAttribute.Value = value;
            }
        }
        
        
        public object Name
        {
            get
            {
                return NameAttribute.Value;
            }
            set
            {
                NameAttribute.Value = value;
            }
        }
        
    }
    
}
