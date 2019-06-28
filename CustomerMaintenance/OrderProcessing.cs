using System;
using Semata.DataStore;
using Semata.DataStore.Util;
using Semata.DataStore.ObjectModel;

namespace CustomerMaintenance
{
    public class OrderProcessingDataStore : DataStoreObject<OrderProcessingDataStore>
    {
        
        string script_ = @"
Add ItemType Customer;
Add ItemType Order;
Add ItemType OrderLine;
Add ItemType Product;
Add ItemType ProductGroup;
Add AttributeType Customer.AddressLine1 String;
Add AttributeType Customer.AddressLine2 String;
Add AttributeType Customer.AddressLine3 String;
Add AttributeType Customer.Code String;
Add AttributeType Customer.Name String;
Add AttributeType Customer.PostCode String;
Add AttributeType Customer.PostTown String;

Add AttributeType Order.CustomerReference String;
Add AttributeType Order.Date Date;
Add AttributeType Order.OrderNo String;

Add AttributeType OrderLine.Quantity Integer;

Add AttributeType Product.Code String;
Add AttributeType Product.Description String;
Add AttributeType Product.Price Decimal;
Add AttributeType Product.StockLevel Integer;

Add AttributeType ProductGroup.Description String;
Add AttributeType ProductGroup.Name String;

Add AssociationType Order.Lines to OrderLine.On
{
  ""semata.datastore.objectmodel"" : 
  [
     ""property""
  ]
}
;

Add AssociationType Customer.Has to Order.By;

Add AssociationType Product.OrderedOn to OrderLine.For;

Add AssociationType ProductGroup.Contains to Product.Is;

Add AssociationType ProductGroup.SubGroups to Product.Group;
Add Associate ProductGroup.ParentGroup to ProductGroup.SubGroups;

";
        public ItemObjects<OrderProcessingDataStore, Customer> CustomerItems {get; private set;}
        public ItemObjects<OrderProcessingDataStore, Order> OrderItems {get; private set;}
        public ItemObjects<OrderProcessingDataStore, OrderLine> OrderLineItems {get; private set;}
        public ItemObjects<OrderProcessingDataStore, Product> ProductItems {get; private set;}
        public ItemObjects<OrderProcessingDataStore, ProductGroup> ProductGroupItems {get; private set;}
        
        public OrderProcessingDataStore(PropertyChangedEventDispatcher eventDispatcher) : base(eventDispatcher)
        {
        }
        
        public void Create(string path)
        {
            base.CreateInstance(path, "OrderProcessingDataStore");
            connection_.ExecuteCommands(script_);
            base.CloseInstance();
        }
        
        public void Open(string path)
        {
            base.OpenInstance(path);
            
            //    Customer
            
            ItemObjectDefinition CustomerDefinition = new ItemObjectDefinition();
            CustomerDefinition.AddAttributeProperty("AddressLine1", x => (x as Customer).AddressLine1Property);
            CustomerDefinition.AddAttributeProperty("AddressLine2", x => (x as Customer).AddressLine2Property);
            CustomerDefinition.AddAttributeProperty("AddressLine3", x => (x as Customer).AddressLine3Property);
            CustomerDefinition.AddAttributeProperty("Code", x => (x as Customer).CodeProperty);
            CustomerDefinition.AddAttributeProperty("Name", x => (x as Customer).NameProperty);
            CustomerDefinition.AddAttributeProperty("PostCode", x => (x as Customer).PostCodeProperty);
            CustomerDefinition.AddAttributeProperty("PostTown", x => (x as Customer).PostTownProperty);
            CustomerDefinition.AddAssociation("Has", "Has", x => (x as Customer).Has);
            SetActivator("Customer", (initializer) => new Customer(initializer), CustomerDefinition);
            
            //    Order
            
            ItemObjectDefinition OrderDefinition = new ItemObjectDefinition();
            OrderDefinition.AddAttributeProperty("CustomerReference", x => (x as Order).CustomerReferenceProperty);
            OrderDefinition.AddAttributeProperty("Date", x => (x as Order).DateProperty);
            OrderDefinition.AddAttributeProperty("OrderNo", x => (x as Order).OrderNoProperty);
            OrderDefinition.AddAssociation("By", "By", x => (x as Order).By);
            OrderDefinition.AddAssociation("Lines", "Lines", x => (x as Order).Lines);
            SetActivator("Order", (initializer) => new Order(initializer), OrderDefinition);
            
            //    OrderLine
            
            ItemObjectDefinition OrderLineDefinition = new ItemObjectDefinition();
            OrderLineDefinition.AddAttributeProperty("Quantity", x => (x as OrderLine).QuantityProperty);
            OrderLineDefinition.AddAssociationProperty("On", "On", x => (x as OrderLine).OnProperty);
            OrderLineDefinition.AddAssociation("For", "For", x => (x as OrderLine).For);
            SetActivator("OrderLine", (initializer) => new OrderLine(initializer), OrderLineDefinition);
            
            //    Product
            
            ItemObjectDefinition ProductDefinition = new ItemObjectDefinition();
            ProductDefinition.AddAttributeProperty("Code", x => (x as Product).CodeProperty);
            ProductDefinition.AddAttributeProperty("Description", x => (x as Product).DescriptionProperty);
            ProductDefinition.AddAttributeProperty("Price", x => (x as Product).PriceProperty);
            ProductDefinition.AddAttributeProperty("StockLevel", x => (x as Product).StockLevelProperty);
            ProductDefinition.AddAssociation("Group", "Group", x => (x as Product).Group);
            ProductDefinition.AddAssociation("Is", "Is", x => (x as Product).Is);
            ProductDefinition.AddAssociation("OrderedOn", "OrderedOn", x => (x as Product).OrderedOn);
            SetActivator("Product", (initializer) => new Product(initializer), ProductDefinition);
            
            //    ProductGroup
            
            ItemObjectDefinition ProductGroupDefinition = new ItemObjectDefinition();
            ProductGroupDefinition.AddAttributeProperty("Description", x => (x as ProductGroup).DescriptionProperty);
            ProductGroupDefinition.AddAttributeProperty("Name", x => (x as ProductGroup).NameProperty);
            ProductGroupDefinition.AddAssociation("Contains", "Contains", x => (x as ProductGroup).Contains);
            ProductGroupDefinition.AddAssociation("ParentGroup", "ParentGroup", x => (x as ProductGroup).ParentGroup);
            ProductGroupDefinition.AddAssociation("SubGroups", "SubGroups", x => (x as ProductGroup).SubGroups);
            SetActivator("ProductGroup", (initializer) => new ProductGroup(initializer), ProductGroupDefinition);
            CustomerItems = new ItemObjects<OrderProcessingDataStore, Customer>(connection_.GetItemType("Customer"), this, "CustomerItems");
            OrderItems = new ItemObjects<OrderProcessingDataStore, Order>(connection_.GetItemType("Order"), this, "OrderItems");
            OrderLineItems = new ItemObjects<OrderProcessingDataStore, OrderLine>(connection_.GetItemType("OrderLine"), this, "OrderLineItems");
            ProductItems = new ItemObjects<OrderProcessingDataStore, Product>(connection_.GetItemType("Product"), this, "ProductItems");
            ProductGroupItems = new ItemObjects<OrderProcessingDataStore, ProductGroup>(connection_.GetItemType("ProductGroup"), this, "ProductGroupItems");
        }
        
        public void Close()
        {
            base.CloseInstance();
        }
        
    }
    
    public partial class Customer : ItemObject<OrderProcessingDataStore>
    {
        public AttributeProperty<OrderProcessingDataStore, string> AddressLine1Property { get; set;}
        public AttributeProperty<OrderProcessingDataStore, string> AddressLine2Property { get; set;}
        public AttributeProperty<OrderProcessingDataStore, string> AddressLine3Property { get; set;}
        public AttributeProperty<OrderProcessingDataStore, string> CodeProperty { get; set;}
        public AttributeProperty<OrderProcessingDataStore, string> NameProperty { get; set;}
        public AttributeProperty<OrderProcessingDataStore, string> PostCodeProperty { get; set;}
        public AttributeProperty<OrderProcessingDataStore, string> PostTownProperty { get; set;}
        
        
        public Association<OrderProcessingDataStore, Order> Has { get; private set;}
        
        internal Customer(ItemObjectInitializer<OrderProcessingDataStore> initializer) : base(initializer)
        {
            AddressLine1Property = new AttributeProperty<OrderProcessingDataStore, string>(this, "AddressLine1", "AddressLine1", false, false, (x) => {OnAddressLine1Changed(ref x); return x;}, (x) => {OnAddressLine1Writing(ref x); return x;});
            AddressLine2Property = new AttributeProperty<OrderProcessingDataStore, string>(this, "AddressLine2", "AddressLine2", false, false, (x) => {OnAddressLine2Changed(ref x); return x;}, (x) => {OnAddressLine2Writing(ref x); return x;});
            AddressLine3Property = new AttributeProperty<OrderProcessingDataStore, string>(this, "AddressLine3", "AddressLine3", false, false, (x) => {OnAddressLine3Changed(ref x); return x;}, (x) => {OnAddressLine3Writing(ref x); return x;});
            CodeProperty = new AttributeProperty<OrderProcessingDataStore, string>(this, "Code", "Code", false, false, (x) => {OnCodeChanged(ref x); return x;}, (x) => {OnCodeWriting(ref x); return x;});
            NameProperty = new AttributeProperty<OrderProcessingDataStore, string>(this, "Name", "Name", false, false, (x) => {OnNameChanged(ref x); return x;}, (x) => {OnNameWriting(ref x); return x;});
            PostCodeProperty = new AttributeProperty<OrderProcessingDataStore, string>(this, "PostCode", "PostCode", false, false, (x) => {OnPostCodeChanged(ref x); return x;}, (x) => {OnPostCodeWriting(ref x); return x;});
            PostTownProperty = new AttributeProperty<OrderProcessingDataStore, string>(this, "PostTown", "PostTown", false, false, (x) => {OnPostTownChanged(ref x); return x;}, (x) => {OnPostTownWriting(ref x); return x;});
            Has = new Association<OrderProcessingDataStore, Order>(this, "Has", "Has", "");
        }
        partial void OnAddressLine1Changed(ref object value);
        partial void OnAddressLine1Writing(ref object value);
        partial void OnAddressLine2Changed(ref object value);
        partial void OnAddressLine2Writing(ref object value);
        partial void OnAddressLine3Changed(ref object value);
        partial void OnAddressLine3Writing(ref object value);
        partial void OnCodeChanged(ref object value);
        partial void OnCodeWriting(ref object value);
        partial void OnNameChanged(ref object value);
        partial void OnNameWriting(ref object value);
        partial void OnPostCodeChanged(ref object value);
        partial void OnPostCodeWriting(ref object value);
        partial void OnPostTownChanged(ref object value);
        partial void OnPostTownWriting(ref object value);
        partial void OnValidate();
        partial void OnCanDelete();
        partial void OnDeleting();
        partial void OnCreated();
        partial void OnWriting();
        partial void OnWritten();
        
        protected override void OnItemObjectValidate()
        {
            OnValidate();
        }
        
        protected override void OnItemObjectCanDelete()
        {
            OnCanDelete();
        }
        
        protected override void OnItemObjectDeleting()
        {
            OnDeleting();
        }
        
        protected override void OnItemObjectCreated()
        {
            OnCreated();
        }
        
        protected override void OnItemObjectWriting()
        {
            OnWriting();
        }
        
        protected override void OnItemObjectWritten()
        {
            OnWritten();
        }
        
        
        public string AddressLine1
        {
            get => (string)AddressLine1Property.Value;
            set => AddressLine1Property.Value = value;
        }
        
        public string AddressLine2
        {
            get => (string)AddressLine2Property.Value;
            set => AddressLine2Property.Value = value;
        }
        
        public string AddressLine3
        {
            get => (string)AddressLine3Property.Value;
            set => AddressLine3Property.Value = value;
        }
        
        public string Code
        {
            get => (string)CodeProperty.Value;
            set => CodeProperty.Value = value;
        }
        
        public string Name
        {
            get => (string)NameProperty.Value;
            set => NameProperty.Value = value;
        }
        
        public string PostCode
        {
            get => (string)PostCodeProperty.Value;
            set => PostCodeProperty.Value = value;
        }
        
        public string PostTown
        {
            get => (string)PostTownProperty.Value;
            set => PostTownProperty.Value = value;
        }
    }
    
    public partial class Order : ItemObject<OrderProcessingDataStore>
    {
        public AttributeProperty<OrderProcessingDataStore, string> CustomerReferenceProperty { get; set;}
        public AttributeProperty<OrderProcessingDataStore, DateTime> DateProperty { get; set;}
        public AttributeProperty<OrderProcessingDataStore, string> OrderNoProperty { get; set;}
        
        
        public Association<OrderProcessingDataStore, Customer> By { get; private set;}
        public Association<OrderProcessingDataStore, OrderLine> Lines { get; private set;}
        
        internal Order(ItemObjectInitializer<OrderProcessingDataStore> initializer) : base(initializer)
        {
            CustomerReferenceProperty = new AttributeProperty<OrderProcessingDataStore, string>(this, "CustomerReference", "CustomerReference", false, false, (x) => {OnCustomerReferenceChanged(ref x); return x;}, (x) => {OnCustomerReferenceWriting(ref x); return x;});
            DateProperty = new AttributeProperty<OrderProcessingDataStore, DateTime>(this, "Date", "Date", false, false, (x) => {OnDateChanged(ref x); return x;}, (x) => {OnDateWriting(ref x); return x;});
            OrderNoProperty = new AttributeProperty<OrderProcessingDataStore, string>(this, "OrderNo", "OrderNo", false, false, (x) => {OnOrderNoChanged(ref x); return x;}, (x) => {OnOrderNoWriting(ref x); return x;});
            By = new Association<OrderProcessingDataStore, Customer>(this, "By", "By", "");
            Lines = new Association<OrderProcessingDataStore, OrderLine>(this, "Lines", "Lines", "On");
        }
        partial void OnCustomerReferenceChanged(ref object value);
        partial void OnCustomerReferenceWriting(ref object value);
        partial void OnDateChanged(ref object value);
        partial void OnDateWriting(ref object value);
        partial void OnOrderNoChanged(ref object value);
        partial void OnOrderNoWriting(ref object value);
        partial void OnValidate();
        partial void OnCanDelete();
        partial void OnDeleting();
        partial void OnCreated();
        partial void OnWriting();
        partial void OnWritten();
        
        protected override void OnItemObjectValidate()
        {
            OnValidate();
        }
        
        protected override void OnItemObjectCanDelete()
        {
            OnCanDelete();
        }
        
        protected override void OnItemObjectDeleting()
        {
            OnDeleting();
        }
        
        protected override void OnItemObjectCreated()
        {
            OnCreated();
        }
        
        protected override void OnItemObjectWriting()
        {
            OnWriting();
        }
        
        protected override void OnItemObjectWritten()
        {
            OnWritten();
        }
        
        
        public string CustomerReference
        {
            get => (string)CustomerReferenceProperty.Value;
            set => CustomerReferenceProperty.Value = value;
        }
        
        public DateTime? Date
        {
            get => (DateTime?)DateProperty.Value;
            set => DateProperty.Value = value;
        }
        
        public string OrderNo
        {
            get => (string)OrderNoProperty.Value;
            set => OrderNoProperty.Value = value;
        }
    }
    
    public partial class OrderLine : ItemObject<OrderProcessingDataStore>
    {
        public AttributeProperty<OrderProcessingDataStore, int> QuantityProperty { get; set;}
        
        public AssociationProperty<OrderProcessingDataStore> OnProperty { get; set;}
        
        public Association<OrderProcessingDataStore, Product> For { get; private set;}
        
        internal OrderLine(ItemObjectInitializer<OrderProcessingDataStore> initializer) : base(initializer)
        {
            QuantityProperty = new AttributeProperty<OrderProcessingDataStore, int>(this, "Quantity", "Quantity", false, false, (x) => {OnQuantityChanged(ref x); return x;}, (x) => {OnQuantityWriting(ref x); return x;});
            OnProperty = new AssociationProperty<OrderProcessingDataStore>(this, "On", "On", false, false, (x) => {OnOnChanged(ref x); return x;}, (x) => {OnOnWriting(ref x); return x;});
            For = new Association<OrderProcessingDataStore, Product>(this, "For", "For", "");
        }
        partial void OnQuantityChanged(ref object value);
        partial void OnQuantityWriting(ref object value);
        partial void OnOnChanged(ref object value);
        partial void OnOnWriting(ref object value);
        partial void OnValidate();
        partial void OnCanDelete();
        partial void OnDeleting();
        partial void OnCreated();
        partial void OnWriting();
        partial void OnWritten();
        
        protected override void OnItemObjectValidate()
        {
            OnValidate();
        }
        
        protected override void OnItemObjectCanDelete()
        {
            OnCanDelete();
        }
        
        protected override void OnItemObjectDeleting()
        {
            OnDeleting();
        }
        
        protected override void OnItemObjectCreated()
        {
            OnCreated();
        }
        
        protected override void OnItemObjectWriting()
        {
            OnWriting();
        }
        
        protected override void OnItemObjectWritten()
        {
            OnWritten();
        }
        
        
        public int? Quantity
        {
            get => (int?)QuantityProperty.Value;
            set => QuantityProperty.Value = value;
        }
        
        public Order On
        {
            get => (Order)OnProperty.Value;
            set => OnProperty.Value = value;
        }
    }
    
    public partial class Product : ItemObject<OrderProcessingDataStore>
    {
        public AttributeProperty<OrderProcessingDataStore, string> CodeProperty { get; set;}
        public AttributeProperty<OrderProcessingDataStore, string> DescriptionProperty { get; set;}
        public AttributeProperty<OrderProcessingDataStore, decimal> PriceProperty { get; set;}
        public AttributeProperty<OrderProcessingDataStore, int> StockLevelProperty { get; set;}
        
        
        public Association<OrderProcessingDataStore, ProductGroup> Group { get; private set;}
        public Association<OrderProcessingDataStore, ProductGroup> Is { get; private set;}
        public Association<OrderProcessingDataStore, OrderLine> OrderedOn { get; private set;}
        
        internal Product(ItemObjectInitializer<OrderProcessingDataStore> initializer) : base(initializer)
        {
            CodeProperty = new AttributeProperty<OrderProcessingDataStore, string>(this, "Code", "Code", false, false, (x) => {OnCodeChanged(ref x); return x;}, (x) => {OnCodeWriting(ref x); return x;});
            DescriptionProperty = new AttributeProperty<OrderProcessingDataStore, string>(this, "Description", "Description", false, false, (x) => {OnDescriptionChanged(ref x); return x;}, (x) => {OnDescriptionWriting(ref x); return x;});
            PriceProperty = new AttributeProperty<OrderProcessingDataStore, decimal>(this, "Price", "Price", false, false, (x) => {OnPriceChanged(ref x); return x;}, (x) => {OnPriceWriting(ref x); return x;});
            StockLevelProperty = new AttributeProperty<OrderProcessingDataStore, int>(this, "StockLevel", "StockLevel", false, false, (x) => {OnStockLevelChanged(ref x); return x;}, (x) => {OnStockLevelWriting(ref x); return x;});
            Group = new Association<OrderProcessingDataStore, ProductGroup>(this, "Group", "Group", "");
            Is = new Association<OrderProcessingDataStore, ProductGroup>(this, "Is", "Is", "");
            OrderedOn = new Association<OrderProcessingDataStore, OrderLine>(this, "OrderedOn", "OrderedOn", "");
        }
        partial void OnCodeChanged(ref object value);
        partial void OnCodeWriting(ref object value);
        partial void OnDescriptionChanged(ref object value);
        partial void OnDescriptionWriting(ref object value);
        partial void OnPriceChanged(ref object value);
        partial void OnPriceWriting(ref object value);
        partial void OnStockLevelChanged(ref object value);
        partial void OnStockLevelWriting(ref object value);
        partial void OnValidate();
        partial void OnCanDelete();
        partial void OnDeleting();
        partial void OnCreated();
        partial void OnWriting();
        partial void OnWritten();
        
        protected override void OnItemObjectValidate()
        {
            OnValidate();
        }
        
        protected override void OnItemObjectCanDelete()
        {
            OnCanDelete();
        }
        
        protected override void OnItemObjectDeleting()
        {
            OnDeleting();
        }
        
        protected override void OnItemObjectCreated()
        {
            OnCreated();
        }
        
        protected override void OnItemObjectWriting()
        {
            OnWriting();
        }
        
        protected override void OnItemObjectWritten()
        {
            OnWritten();
        }
        
        
        public string Code
        {
            get => (string)CodeProperty.Value;
            set => CodeProperty.Value = value;
        }
        
        public string Description
        {
            get => (string)DescriptionProperty.Value;
            set => DescriptionProperty.Value = value;
        }
        
        public decimal? Price
        {
            get => (decimal?)PriceProperty.Value;
            set => PriceProperty.Value = value;
        }
        
        public int? StockLevel
        {
            get => (int?)StockLevelProperty.Value;
            set => StockLevelProperty.Value = value;
        }
    }
    
    public partial class ProductGroup : ItemObject<OrderProcessingDataStore>
    {
        public AttributeProperty<OrderProcessingDataStore, string> DescriptionProperty { get; set;}
        public AttributeProperty<OrderProcessingDataStore, string> NameProperty { get; set;}
        
        
        public Association<OrderProcessingDataStore, Product> Contains { get; private set;}
        public Association<OrderProcessingDataStore, ProductGroup> ParentGroup { get; private set;}
        public Association<OrderProcessingDataStore, ItemObject<OrderProcessingDataStore>> SubGroups { get; private set;}
        
        internal ProductGroup(ItemObjectInitializer<OrderProcessingDataStore> initializer) : base(initializer)
        {
            DescriptionProperty = new AttributeProperty<OrderProcessingDataStore, string>(this, "Description", "Description", false, false, (x) => {OnDescriptionChanged(ref x); return x;}, (x) => {OnDescriptionWriting(ref x); return x;});
            NameProperty = new AttributeProperty<OrderProcessingDataStore, string>(this, "Name", "Name", false, false, (x) => {OnNameChanged(ref x); return x;}, (x) => {OnNameWriting(ref x); return x;});
            Contains = new Association<OrderProcessingDataStore, Product>(this, "Contains", "Contains", "");
            ParentGroup = new Association<OrderProcessingDataStore, ProductGroup>(this, "ParentGroup", "ParentGroup", "");
            SubGroups = new Association<OrderProcessingDataStore, ItemObject<OrderProcessingDataStore>>(this, "SubGroups", "SubGroups", "");
        }
        partial void OnDescriptionChanged(ref object value);
        partial void OnDescriptionWriting(ref object value);
        partial void OnNameChanged(ref object value);
        partial void OnNameWriting(ref object value);
        partial void OnValidate();
        partial void OnCanDelete();
        partial void OnDeleting();
        partial void OnCreated();
        partial void OnWriting();
        partial void OnWritten();
        
        protected override void OnItemObjectValidate()
        {
            OnValidate();
        }
        
        protected override void OnItemObjectCanDelete()
        {
            OnCanDelete();
        }
        
        protected override void OnItemObjectDeleting()
        {
            OnDeleting();
        }
        
        protected override void OnItemObjectCreated()
        {
            OnCreated();
        }
        
        protected override void OnItemObjectWriting()
        {
            OnWriting();
        }
        
        protected override void OnItemObjectWritten()
        {
            OnWritten();
        }
        
        
        public string Description
        {
            get => (string)DescriptionProperty.Value;
            set => DescriptionProperty.Value = value;
        }
        
        public string Name
        {
            get => (string)NameProperty.Value;
            set => NameProperty.Value = value;
        }
    }
    
}
