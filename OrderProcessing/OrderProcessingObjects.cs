using System;
using System.Collections.Generic;
using Semata.DataStore;
using Semata.DataStore.Util;
using Semata.DataStore.ObjectModel;

namespace OrderProcessing
{
    public sealed partial class OrderProcessingDataStore : DataStoreObject
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
Add AttributeType Customer.Code String
{
  ""semata.datastore.objectmodel"" : 
  [
     ""mandatory"",
     ""unique""
  ]
}
;
Add AttributeType Customer.Name String
{
  ""semata.datastore.objectmodel"" : 
  [
     ""mandatory""
  ]
}
;
Add AttributeType Customer.PostCode String;
Add AttributeType Customer.PostTown String;

Add AttributeType Order.CustomerReference String;
Add AttributeType Order.Date Date
{
  ""semata.datastore.objectmodel"" : 
  [
     ""mandatory""
  ]
}
;
Add AttributeType Order.OrderNo String
{
  ""semata.datastore.objectmodel"" : 
  [
     ""mandatory"",
     ""unique""
  ]
}
;

Add AttributeType OrderLine.Quantity Integer
{
  ""semata.datastore.objectmodel"" : 
  [
     ""mandatory""
  ]
}
;

Add AttributeType Product.Code String
{
  ""semata.datastore.objectmodel"" : 
  [
     ""mandatory"",
     ""unique""
  ]
}
;
Add AttributeType Product.Description String
{
  ""semata.datastore.objectmodel"" : 
  [
     ""mandatory""
  ]
}
;
Add AttributeType Product.Price Decimal;
Add AttributeType Product.StockLevel Integer;

Add AttributeType ProductGroup.Description String;
Add AttributeType ProductGroup.Name String
{
  ""semata.datastore.objectmodel"" : 
  [
     ""mandatory"",
     ""unique""
  ]
}
;

Add AssociationType Customer.Has
{
  ""semata.datastore.objectmodel"" : 
  [
     ""associates_prevent_delete""
  ]
}
to Order.By
{
  ""semata.datastore.objectmodel"" : 
  [
     ""property""
  ]
}
;

Add AssociationType Order.Lines
{
  ""semata.datastore.objectmodel"" : 
  [
     ""delete_associates""
  ]
}
to OrderLine.On
{
  ""semata.datastore.objectmodel"" : 
  [
     ""property""
  ]
}
;

Add AssociationType Product.OrderedOn
{
  ""semata.datastore.objectmodel"" : 
  [
     ""associates_prevent_delete""
  ]
}
to OrderLine.For
{
  ""semata.datastore.objectmodel"" : 
  [
     ""property"",
     ""mandatory(Product not Set)""
  ]
}
;

Add AssociationType ProductGroup.Contains to Product.Is
{
  ""semata.datastore.objectmodel"" : 
  [
     ""property""
  ]
}
;

Add AssociationType ProductGroup.SubGroups to Product.Group;
Add Associate ProductGroup.ParentGroup
{
  ""semata.datastore.objectmodel"" : 
  [
     ""property""
  ]
}
to ProductGroup.SubGroups;

";
        public ItemObjects<Customer> CustomerItems {get; private set;}
        public ItemObjects<Order> OrderItems {get; private set;}
        public ItemObjects<OrderLine> OrderLineItems {get; private set;}
        public ItemObjects<Product> ProductItems {get; private set;}
        public ItemObjects<ProductGroup> ProductGroupItems {get; private set;}
        
        public OrderProcessingDataStore(PropertyChangedEventDispatcher eventDispatcher) : base(eventDispatcher)
        {
        }
        
        partial void AfterCreate(string path);
        
        public void Create(string path)
        {
            base.CreateInstance(path, "OrderProcessingDataStore");
            connection_.ExecuteCommands(script_);
            base.CloseInstance();
            AfterCreate(path);
            Open(path);
        }
        
        partial void BeforeOpening(string path);
        
        public void Open(string path)
        {
            BeforeOpening(path);
            base.OpenInstance(path);
            try
            {
                DefineItemObjects();
            }
            catch (System.Exception exception)
            {
                base.CloseInstance();
                throw;
            }
        }
        
        void DefineItemObjects()
        {
            
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
            OrderDefinition.AddAssociationProperty("By", "By", x => (x as Order).ByProperty);
            OrderDefinition.AddAssociation("Lines", "Lines", x => (x as Order).Lines);
            SetActivator("Order", (initializer) => new Order(initializer), OrderDefinition);
            
            //    OrderLine
            
            ItemObjectDefinition OrderLineDefinition = new ItemObjectDefinition();
            OrderLineDefinition.AddAttributeProperty("Quantity", x => (x as OrderLine).QuantityProperty);
            OrderLineDefinition.AddAssociationProperty("For", "For", x => (x as OrderLine).ForProperty);
            OrderLineDefinition.AddAssociationProperty("On", "On", x => (x as OrderLine).OnProperty);
            SetActivator("OrderLine", (initializer) => new OrderLine(initializer), OrderLineDefinition);
            
            //    Product
            
            ItemObjectDefinition ProductDefinition = new ItemObjectDefinition();
            ProductDefinition.AddAttributeProperty("Code", x => (x as Product).CodeProperty);
            ProductDefinition.AddAttributeProperty("Description", x => (x as Product).DescriptionProperty);
            ProductDefinition.AddAttributeProperty("Price", x => (x as Product).PriceProperty);
            ProductDefinition.AddAttributeProperty("StockLevel", x => (x as Product).StockLevelProperty);
            ProductDefinition.AddAssociationProperty("Is", "Is", x => (x as Product).IsProperty);
            ProductDefinition.AddAssociation("Group", "Group", x => (x as Product).Group);
            ProductDefinition.AddAssociation("OrderedOn", "OrderedOn", x => (x as Product).OrderedOn);
            SetActivator("Product", (initializer) => new Product(initializer), ProductDefinition);
            
            //    ProductGroup
            
            ItemObjectDefinition ProductGroupDefinition = new ItemObjectDefinition();
            ProductGroupDefinition.AddAttributeProperty("Description", x => (x as ProductGroup).DescriptionProperty);
            ProductGroupDefinition.AddAttributeProperty("Name", x => (x as ProductGroup).NameProperty);
            ProductGroupDefinition.AddAssociationProperty("ParentGroup", "ParentGroup", x => (x as ProductGroup).ParentGroupProperty);
            ProductGroupDefinition.AddAssociation("Contains", "Contains", x => (x as ProductGroup).Contains);
            ProductGroupDefinition.AddAssociation("SubGroups", "SubGroups", x => (x as ProductGroup).SubGroups);
            SetActivator("ProductGroup", (initializer) => new ProductGroup(initializer), ProductGroupDefinition);
            CustomerItems = new ItemObjects<Customer>(connection_.GetItemType("Customer"), this, "CustomerItems");
            OrderItems = new ItemObjects<Order>(connection_.GetItemType("Order"), this, "OrderItems");
            OrderLineItems = new ItemObjects<OrderLine>(connection_.GetItemType("OrderLine"), this, "OrderLineItems");
            ProductItems = new ItemObjects<Product>(connection_.GetItemType("Product"), this, "ProductItems");
            ProductGroupItems = new ItemObjects<ProductGroup>(connection_.GetItemType("ProductGroup"), this, "ProductGroupItems");
        }
        
        public void Close()
        {
            base.CloseInstance();
        }
        
    }
    
    public sealed partial class Customer : ItemObject
    {
        public AttributeProperty<string> AddressLine1Property { get; set;}
        public AttributeProperty<string> AddressLine2Property { get; set;}
        public AttributeProperty<string> AddressLine3Property { get; set;}
        public AttributeProperty<string> CodeProperty { get; set;}
        public AttributeProperty<string> NameProperty { get; set;}
        public AttributeProperty<string> PostCodeProperty { get; set;}
        public AttributeProperty<string> PostTownProperty { get; set;}
        
        
        public Association<Order> Has { get; private set;}
        
        internal Customer(ItemObjectInitializer initializer) : base(initializer)
        {
            AddressLine1Property = new AttributeProperty<string>(this, "AddressLine1", "AddressLine1", false, false, (x) => {OnAddressLine1Changed(ref x); return x;}, (x) => {OnAddressLine1Writing(ref x); return x;});
            AddressLine2Property = new AttributeProperty<string>(this, "AddressLine2", "AddressLine2", false, false, (x) => {OnAddressLine2Changed(ref x); return x;}, (x) => {OnAddressLine2Writing(ref x); return x;});
            AddressLine3Property = new AttributeProperty<string>(this, "AddressLine3", "AddressLine3", false, false, (x) => {OnAddressLine3Changed(ref x); return x;}, (x) => {OnAddressLine3Writing(ref x); return x;});
            CodeProperty = new AttributeProperty<string>(this, "Code", "Code", false, false, (x) => {OnCodeChanged(ref x); return x;}, (x) => {OnCodeWriting(ref x); return x;});
            NameProperty = new AttributeProperty<string>(this, "Name", "Name", false, false, (x) => {OnNameChanged(ref x); return x;}, (x) => {OnNameWriting(ref x); return x;});
            PostCodeProperty = new AttributeProperty<string>(this, "PostCode", "PostCode", false, false, (x) => {OnPostCodeChanged(ref x); return x;}, (x) => {OnPostCodeWriting(ref x); return x;});
            PostTownProperty = new AttributeProperty<string>(this, "PostTown", "PostTown", false, false, (x) => {OnPostTownChanged(ref x); return x;}, (x) => {OnPostTownWriting(ref x); return x;});
            Has = new Association<Order>(this, "Has", "Has", "By");
            OnInitialize();
        }
        
        public new OrderProcessingDataStore DataStore => (OrderProcessingDataStore)base.DataStore;
        
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
        partial void OnInitialize();
        partial void OnValidate();
        partial void OnCanDelete(List<string> errorMessages);
        partial void OnDeleting();
        partial void OnCreated();
        partial void OnWriting();
        partial void OnWritten();
        
        protected override void OnItemObjectValidate()
        {
            CodeProperty.CheckValueSet("");
            CodeProperty.CheckValueUnique("");
            NameProperty.CheckValueSet("");
            OnValidate();
        }
        
        protected override void OnItemObjectCanDelete(List<string> errorMessages)
        {
            Has.CheckNoAssociatesExist(errorMessages, "");
            OnCanDelete(errorMessages);
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
    
    public sealed partial class Order : ItemObject
    {
        public AttributeProperty<string> CustomerReferenceProperty { get; set;}
        public AttributeProperty<System.DateTime> DateProperty { get; set;}
        public AttributeProperty<string> OrderNoProperty { get; set;}
        
        public AssociationProperty<Customer> ByProperty { get; set;}
        
        public Association<OrderLine> Lines { get; private set;}
        
        internal Order(ItemObjectInitializer initializer) : base(initializer)
        {
            CustomerReferenceProperty = new AttributeProperty<string>(this, "CustomerReference", "CustomerReference", false, false, (x) => {OnCustomerReferenceChanged(ref x); return x;}, (x) => {OnCustomerReferenceWriting(ref x); return x;});
            DateProperty = new AttributeProperty<System.DateTime>(this, "Date", "Date", false, false, (x) => {OnDateChanged(ref x); return x;}, (x) => {OnDateWriting(ref x); return x;});
            OrderNoProperty = new AttributeProperty<string>(this, "OrderNo", "OrderNo", false, false, (x) => {OnOrderNoChanged(ref x); return x;}, (x) => {OnOrderNoWriting(ref x); return x;});
            ByProperty = new AssociationProperty<Customer>(this, "By", "By", false, false, (x) => {OnByChanged(ref x); return x;}, (x) => {OnByWriting(ref x); return x;}, false);
            Lines = new Association<OrderLine>(this, "Lines", "Lines", "On");
            OnInitialize();
        }
        
        public new OrderProcessingDataStore DataStore => (OrderProcessingDataStore)base.DataStore;
        
        partial void OnCustomerReferenceChanged(ref object value);
        partial void OnCustomerReferenceWriting(ref object value);
        partial void OnDateChanged(ref object value);
        partial void OnDateWriting(ref object value);
        partial void OnOrderNoChanged(ref object value);
        partial void OnOrderNoWriting(ref object value);
        partial void OnByChanged(ref object value);
        partial void OnByWriting(ref object value);
        partial void OnInitialize();
        partial void OnValidate();
        partial void OnCanDelete(List<string> errorMessages);
        partial void OnDeleting();
        partial void OnCreated();
        partial void OnWriting();
        partial void OnWritten();
        
        protected override void OnItemObjectValidate()
        {
            DateProperty.CheckValueSet("");
            OrderNoProperty.CheckValueSet("");
            OrderNoProperty.CheckValueUnique("");
            OnValidate();
        }
        
        protected override void OnItemObjectCanDelete(List<string> errorMessages)
        {
            Lines.CanDeleteAssociates(errorMessages, "");
            OnCanDelete(errorMessages);
        }
        
        protected override void OnItemObjectDeleting()
        {
            Lines.DeleteAssociates();
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
        
        public System.DateTime? Date
        {
            get => (System.DateTime?)DateProperty.Value;
            set => DateProperty.Value = value;
        }
        
        public string OrderNo
        {
            get => (string)OrderNoProperty.Value;
            set => OrderNoProperty.Value = value;
        }
        
        public Customer By
        {
            get => (Customer)ByProperty.Value;
            set => ByProperty.Value = value;
        }
    }
    
    public sealed partial class OrderLine : ItemObject
    {
        public AttributeProperty<int> QuantityProperty { get; set;}
        
        public AssociationProperty<Product> ForProperty { get; set;}
        public AssociationProperty<Order> OnProperty { get; set;}
        
        
        internal OrderLine(ItemObjectInitializer initializer) : base(initializer)
        {
            QuantityProperty = new AttributeProperty<int>(this, "Quantity", "Quantity", false, false, (x) => {OnQuantityChanged(ref x); return x;}, (x) => {OnQuantityWriting(ref x); return x;});
            ForProperty = new AssociationProperty<Product>(this, "For", "For", false, false, (x) => {OnForChanged(ref x); return x;}, (x) => {OnForWriting(ref x); return x;}, false);
            OnProperty = new AssociationProperty<Order>(this, "On", "On", false, false, (x) => {OnOnChanged(ref x); return x;}, (x) => {OnOnWriting(ref x); return x;}, false);
            OnInitialize();
        }
        
        public new OrderProcessingDataStore DataStore => (OrderProcessingDataStore)base.DataStore;
        
        partial void OnQuantityChanged(ref object value);
        partial void OnQuantityWriting(ref object value);
        partial void OnForChanged(ref object value);
        partial void OnForWriting(ref object value);
        partial void OnOnChanged(ref object value);
        partial void OnOnWriting(ref object value);
        partial void OnInitialize();
        partial void OnValidate();
        partial void OnCanDelete(List<string> errorMessages);
        partial void OnDeleting();
        partial void OnCreated();
        partial void OnWriting();
        partial void OnWritten();
        
        protected override void OnItemObjectValidate()
        {
            QuantityProperty.CheckValueSet("");
            ForProperty.CheckValueSet("Product not Set");
            OnValidate();
        }
        
        protected override void OnItemObjectCanDelete(List<string> errorMessages)
        {
            OnCanDelete(errorMessages);
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
        
        public Product For
        {
            get => (Product)ForProperty.Value;
            set => ForProperty.Value = value;
        }
        
        public Order On
        {
            get => (Order)OnProperty.Value;
            set => OnProperty.Value = value;
        }
    }
    
    public sealed partial class Product : ItemObject
    {
        public AttributeProperty<string> CodeProperty { get; set;}
        public AttributeProperty<string> DescriptionProperty { get; set;}
        public AttributeProperty<decimal> PriceProperty { get; set;}
        public AttributeProperty<int> StockLevelProperty { get; set;}
        
        public AssociationProperty<ProductGroup> IsProperty { get; set;}
        
        public Association<ProductGroup> Group { get; private set;}
        public Association<OrderLine> OrderedOn { get; private set;}
        
        internal Product(ItemObjectInitializer initializer) : base(initializer)
        {
            CodeProperty = new AttributeProperty<string>(this, "Code", "Code", false, false, (x) => {OnCodeChanged(ref x); return x;}, (x) => {OnCodeWriting(ref x); return x;});
            DescriptionProperty = new AttributeProperty<string>(this, "Description", "Description", false, false, (x) => {OnDescriptionChanged(ref x); return x;}, (x) => {OnDescriptionWriting(ref x); return x;});
            PriceProperty = new AttributeProperty<decimal>(this, "Price", "Price", false, false, (x) => {OnPriceChanged(ref x); return x;}, (x) => {OnPriceWriting(ref x); return x;});
            StockLevelProperty = new AttributeProperty<int>(this, "StockLevel", "StockLevel", false, false, (x) => {OnStockLevelChanged(ref x); return x;}, (x) => {OnStockLevelWriting(ref x); return x;});
            IsProperty = new AssociationProperty<ProductGroup>(this, "Is", "Is", false, false, (x) => {OnIsChanged(ref x); return x;}, (x) => {OnIsWriting(ref x); return x;}, false);
            Group = new Association<ProductGroup>(this, "Group", "Group", "");
            OrderedOn = new Association<OrderLine>(this, "OrderedOn", "OrderedOn", "For");
            OnInitialize();
        }
        
        public new OrderProcessingDataStore DataStore => (OrderProcessingDataStore)base.DataStore;
        
        partial void OnCodeChanged(ref object value);
        partial void OnCodeWriting(ref object value);
        partial void OnDescriptionChanged(ref object value);
        partial void OnDescriptionWriting(ref object value);
        partial void OnPriceChanged(ref object value);
        partial void OnPriceWriting(ref object value);
        partial void OnStockLevelChanged(ref object value);
        partial void OnStockLevelWriting(ref object value);
        partial void OnIsChanged(ref object value);
        partial void OnIsWriting(ref object value);
        partial void OnInitialize();
        partial void OnValidate();
        partial void OnCanDelete(List<string> errorMessages);
        partial void OnDeleting();
        partial void OnCreated();
        partial void OnWriting();
        partial void OnWritten();
        
        protected override void OnItemObjectValidate()
        {
            CodeProperty.CheckValueSet("");
            CodeProperty.CheckValueUnique("");
            DescriptionProperty.CheckValueSet("");
            OnValidate();
        }
        
        protected override void OnItemObjectCanDelete(List<string> errorMessages)
        {
            OrderedOn.CheckNoAssociatesExist(errorMessages, "");
            OnCanDelete(errorMessages);
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
        
        public ProductGroup Is
        {
            get => (ProductGroup)IsProperty.Value;
            set => IsProperty.Value = value;
        }
    }
    
    public sealed partial class ProductGroup : ItemObject
    {
        public AttributeProperty<string> DescriptionProperty { get; set;}
        public AttributeProperty<string> NameProperty { get; set;}
        
        public AssociationProperty<ProductGroup> ParentGroupProperty { get; set;}
        
        public Association<Product> Contains { get; private set;}
        public Association<ItemObject> SubGroups { get; private set;}
        
        internal ProductGroup(ItemObjectInitializer initializer) : base(initializer)
        {
            DescriptionProperty = new AttributeProperty<string>(this, "Description", "Description", false, false, (x) => {OnDescriptionChanged(ref x); return x;}, (x) => {OnDescriptionWriting(ref x); return x;});
            NameProperty = new AttributeProperty<string>(this, "Name", "Name", false, false, (x) => {OnNameChanged(ref x); return x;}, (x) => {OnNameWriting(ref x); return x;});
            ParentGroupProperty = new AssociationProperty<ProductGroup>(this, "ParentGroup", "ParentGroup", false, false, (x) => {OnParentGroupChanged(ref x); return x;}, (x) => {OnParentGroupWriting(ref x); return x;}, false);
            Contains = new Association<Product>(this, "Contains", "Contains", "Is");
            SubGroups = new Association<ItemObject>(this, "SubGroups", "SubGroups", "");
            OnInitialize();
        }
        
        public new OrderProcessingDataStore DataStore => (OrderProcessingDataStore)base.DataStore;
        
        partial void OnDescriptionChanged(ref object value);
        partial void OnDescriptionWriting(ref object value);
        partial void OnNameChanged(ref object value);
        partial void OnNameWriting(ref object value);
        partial void OnParentGroupChanged(ref object value);
        partial void OnParentGroupWriting(ref object value);
        partial void OnInitialize();
        partial void OnValidate();
        partial void OnCanDelete(List<string> errorMessages);
        partial void OnDeleting();
        partial void OnCreated();
        partial void OnWriting();
        partial void OnWritten();
        
        protected override void OnItemObjectValidate()
        {
            NameProperty.CheckValueSet("");
            NameProperty.CheckValueUnique("");
            OnValidate();
        }
        
        protected override void OnItemObjectCanDelete(List<string> errorMessages)
        {
            OnCanDelete(errorMessages);
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
        
        public ProductGroup ParentGroup
        {
            get => (ProductGroup)ParentGroupProperty.Value;
            set => ParentGroupProperty.Value = value;
        }
    }
    
}
