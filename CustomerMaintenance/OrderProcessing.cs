using System;
using Semata.DataStore;
using Semata.DataStore.ObjectModel;

namespace CustomerMaintenance
{
    public class OrderProcessing : DataStoreObject<OrderProcessing>
    {
        
        public ItemObjects<OrderProcessing, Customer> CustomerItems {get; private set;}
        public ItemObjects<OrderProcessing, Order> OrderItems {get; private set;}
        public ItemObjects<OrderProcessing, OrderLine> OrderLineItems {get; private set;}
        public ItemObjects<OrderProcessing, Product> ProductItems {get; private set;}
        public ItemObjects<OrderProcessing, ProductGroup> ProductGroupItems {get; private set;}
        
        public OrderProcessing(PropertyChangedEventDispatcher eventDispatcher) : base(eventDispatcher)
        {
        }
        
        public void Create(string path)
        {
            base.CreateInstance(path, "OrderProcessing");
            ItemType itemType;
            
            //    Customer
            
            itemType = connection_.AddItemType("Customer", "", "");
            itemType.AddAttributeType("AddressLine1", "", "", Semata.DataStore.ValueType.String);
            itemType.AddAttributeType("AddressLine2", "", "", Semata.DataStore.ValueType.String);
            itemType.AddAttributeType("AddressLine3", "", "", Semata.DataStore.ValueType.String);
            itemType.AddAttributeType("Code", "", "", Semata.DataStore.ValueType.String);
            itemType.AddAttributeType("Name", "", "", Semata.DataStore.ValueType.String);
            itemType.AddAttributeType("PostCode", "", "", Semata.DataStore.ValueType.String);
            itemType.AddAttributeType("PostTown", "", "", Semata.DataStore.ValueType.String);
            
            //    Order
            
            itemType = connection_.AddItemType("Order", "", "");
            itemType.AddAttributeType("CustomerReference", "", "", Semata.DataStore.ValueType.String);
            itemType.AddAttributeType("Date", "", "", Semata.DataStore.ValueType.Date);
            itemType.AddAttributeType("OrderNo", "", "", Semata.DataStore.ValueType.String);
            
            //    OrderLine
            
            itemType = connection_.AddItemType("OrderLine", "", "");
            itemType.AddAttributeType("Quantity", "", "", Semata.DataStore.ValueType.Integer);
            
            //    Product
            
            itemType = connection_.AddItemType("Product", "", "");
            itemType.AddAttributeType("Code", "", "", Semata.DataStore.ValueType.String);
            itemType.AddAttributeType("Description", "", "", Semata.DataStore.ValueType.String);
            itemType.AddAttributeType("Price", "", "", Semata.DataStore.ValueType.Decimal);
            itemType.AddAttributeType("StockLevel", "", "", Semata.DataStore.ValueType.Integer);
            
            //    ProductGroup
            
            itemType = connection_.AddItemType("ProductGroup", "", "");
            itemType.AddAttributeType("Description", "", "", Semata.DataStore.ValueType.String);
            itemType.AddAttributeType("Name", "", "", Semata.DataStore.ValueType.String);
            
            ItemType leftItemType;
            ItemType rightItemType;
            AssociationTypePair pair;
            leftItemType = connection_.GetItemType("Customer");
            rightItemType = connection_.GetItemType("Order");
            pair = leftItemType.AddAssociationType("Has"
                                                   ,""
                                                   ,""
                                                   , rightItemType
                                                   ,"By"
                                                   ,""
                                                   ,"");
            leftItemType = connection_.GetItemType("Order");
            rightItemType = connection_.GetItemType("OrderLine");
            pair = leftItemType.AddAssociationType("Lines"
                                                   ,""
                                                   ,""
                                                   , rightItemType
                                                   ,"On"
                                                   ,""
                                                   ,"[property]");
            leftItemType = connection_.GetItemType("Product");
            rightItemType = connection_.GetItemType("OrderLine");
            pair = leftItemType.AddAssociationType("OrderedOn"
                                                   ,""
                                                   ,""
                                                   , rightItemType
                                                   ,"For"
                                                   ,""
                                                   ,"");
            leftItemType = connection_.GetItemType("ProductGroup");
            rightItemType = connection_.GetItemType("Product");
            pair = leftItemType.AddAssociationType("Contains"
                                                   ,""
                                                   ,""
                                                   , rightItemType
                                                   ,"Is"
                                                   ,""
                                                   ,"");
            leftItemType = connection_.GetItemType("ProductGroup");
            rightItemType = connection_.GetItemType("Product");
            pair = leftItemType.AddAssociationType("SubGroups"
                                                   ,""
                                                   ,""
                                                   , rightItemType
                                                   ,"Group"
                                                   ,""
                                                   ,"");
            rightItemType = connection_.GetItemType("ProductGroup");
            pair.AssociationType.AddAssociate(rightItemType
                                              ,"ParentGroup"
                                              ,""
                                              ,"");
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
            CustomerItems = new ItemObjects<OrderProcessing, Customer>(connection_.GetItemType("Customer"), this, "CustomerItems");
            OrderItems = new ItemObjects<OrderProcessing, Order>(connection_.GetItemType("Order"), this, "OrderItems");
            OrderLineItems = new ItemObjects<OrderProcessing, OrderLine>(connection_.GetItemType("OrderLine"), this, "OrderLineItems");
            ProductItems = new ItemObjects<OrderProcessing, Product>(connection_.GetItemType("Product"), this, "ProductItems");
            ProductGroupItems = new ItemObjects<OrderProcessing, ProductGroup>(connection_.GetItemType("ProductGroup"), this, "ProductGroupItems");
        }
        
        public void Close()
        {
            base.CloseInstance();
        }
        
    }
    
    public partial class Customer : ItemObject<OrderProcessing>
    {
        public AttributeProperty<OrderProcessing, string> AddressLine1Property { get; set;}
        public AttributeProperty<OrderProcessing, string> AddressLine2Property { get; set;}
        public AttributeProperty<OrderProcessing, string> AddressLine3Property { get; set;}
        public AttributeProperty<OrderProcessing, string> CodeProperty { get; set;}
        public AttributeProperty<OrderProcessing, string> NameProperty { get; set;}
        public AttributeProperty<OrderProcessing, string> PostCodeProperty { get; set;}
        public AttributeProperty<OrderProcessing, string> PostTownProperty { get; set;}
        
        
        public Association<OrderProcessing, Order> Has { get; private set;}
        
        internal Customer(ItemObjectInitializer<OrderProcessing> initializer) : base(initializer)
        {
            AddressLine1Property = new AttributeProperty<OrderProcessing, string>(this, "AddressLine1", "AddressLine1", false, false, (x) => AddressLine1Changed(x), (x) => {return AddressLine1Writing(x);});
            AddressLine2Property = new AttributeProperty<OrderProcessing, string>(this, "AddressLine2", "AddressLine2", false, false, (x) => AddressLine2Changed(x), (x) => {return AddressLine2Writing(x);});
            AddressLine3Property = new AttributeProperty<OrderProcessing, string>(this, "AddressLine3", "AddressLine3", false, false, (x) => AddressLine3Changed(x), (x) => {return AddressLine3Writing(x);});
            CodeProperty = new AttributeProperty<OrderProcessing, string>(this, "Code", "Code", false, false, (x) => CodeChanged(x), (x) => {return CodeWriting(x);});
            NameProperty = new AttributeProperty<OrderProcessing, string>(this, "Name", "Name", false, false, (x) => NameChanged(x), (x) => {return NameWriting(x);});
            PostCodeProperty = new AttributeProperty<OrderProcessing, string>(this, "PostCode", "PostCode", false, false, (x) => PostCodeChanged(x), (x) => {return PostCodeWriting(x);});
            PostTownProperty = new AttributeProperty<OrderProcessing, string>(this, "PostTown", "PostTown", false, false, (x) => PostTownChanged(x), (x) => {return PostTownWriting(x);});
            Has = new Association<OrderProcessing, Order>(this, "Has", "Has", "");
        }
        partial void OnAddressLine1Changed(object value);
        partial void OnAddressLine1Writing(ref object value);
        partial void OnAddressLine2Changed(object value);
        partial void OnAddressLine2Writing(ref object value);
        partial void OnAddressLine3Changed(object value);
        partial void OnAddressLine3Writing(ref object value);
        partial void OnCodeChanged(object value);
        partial void OnCodeWriting(ref object value);
        partial void OnNameChanged(object value);
        partial void OnNameWriting(ref object value);
        partial void OnPostCodeChanged(object value);
        partial void OnPostCodeWriting(ref object value);
        partial void OnPostTownChanged(object value);
        partial void OnPostTownWriting(ref object value);
        partial void OnValidate();
        partial void OnCanDelete(ItemObjectDeleteResult result);
        partial void OnDeleting(ItemObjectDeleteResult result);
        partial void OnCreated();
        partial void OnWriting();
        partial void OnWritten();
        
        protected void AddressLine1Changed(object value)
        {
            OnAddressLine1Changed(value);
        }
        
        protected object AddressLine1Writing(object value)
        {
            OnAddressLine1Writing(ref value);
            return value;
        }
        
        protected void AddressLine2Changed(object value)
        {
            OnAddressLine2Changed(value);
        }
        
        protected object AddressLine2Writing(object value)
        {
            OnAddressLine2Writing(ref value);
            return value;
        }
        
        protected void AddressLine3Changed(object value)
        {
            OnAddressLine3Changed(value);
        }
        
        protected object AddressLine3Writing(object value)
        {
            OnAddressLine3Writing(ref value);
            return value;
        }
        
        protected void CodeChanged(object value)
        {
            OnCodeChanged(value);
        }
        
        protected object CodeWriting(object value)
        {
            OnCodeWriting(ref value);
            return value;
        }
        
        protected void NameChanged(object value)
        {
            OnNameChanged(value);
        }
        
        protected object NameWriting(object value)
        {
            OnNameWriting(ref value);
            return value;
        }
        
        protected void PostCodeChanged(object value)
        {
            OnPostCodeChanged(value);
        }
        
        protected object PostCodeWriting(object value)
        {
            OnPostCodeWriting(ref value);
            return value;
        }
        
        protected void PostTownChanged(object value)
        {
            OnPostTownChanged(value);
        }
        
        protected object PostTownWriting(object value)
        {
            OnPostTownWriting(ref value);
            return value;
        }
        
        protected override void OnItemObjectValidate()
        {
            OnValidate();
        }
        
        protected override void OnItemObjectCanDelete(ItemObjectDeleteResult result)
        {
            OnCanDelete(result);
        }
        
        protected override void OnItemObjectDeleting(ItemObjectDeleteResult result)
        {
            OnDeleting(result);
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
    
    public partial class Order : ItemObject<OrderProcessing>
    {
        public AttributeProperty<OrderProcessing, string> CustomerReferenceProperty { get; set;}
        public AttributeProperty<OrderProcessing, DateTime> DateProperty { get; set;}
        public AttributeProperty<OrderProcessing, string> OrderNoProperty { get; set;}
        
        
        public Association<OrderProcessing, Customer> By { get; private set;}
        public Association<OrderProcessing, OrderLine> Lines { get; private set;}
        
        internal Order(ItemObjectInitializer<OrderProcessing> initializer) : base(initializer)
        {
            CustomerReferenceProperty = new AttributeProperty<OrderProcessing, string>(this, "CustomerReference", "CustomerReference", false, false, (x) => CustomerReferenceChanged(x), (x) => {return CustomerReferenceWriting(x);});
            DateProperty = new AttributeProperty<OrderProcessing, DateTime>(this, "Date", "Date", false, false, (x) => DateChanged(x), (x) => {return DateWriting(x);});
            OrderNoProperty = new AttributeProperty<OrderProcessing, string>(this, "OrderNo", "OrderNo", false, false, (x) => OrderNoChanged(x), (x) => {return OrderNoWriting(x);});
            By = new Association<OrderProcessing, Customer>(this, "By", "By", "");
            Lines = new Association<OrderProcessing, OrderLine>(this, "Lines", "Lines", "On");
        }
        partial void OnCustomerReferenceChanged(object value);
        partial void OnCustomerReferenceWriting(ref object value);
        partial void OnDateChanged(object value);
        partial void OnDateWriting(ref object value);
        partial void OnOrderNoChanged(object value);
        partial void OnOrderNoWriting(ref object value);
        partial void OnValidate();
        partial void OnCanDelete(ItemObjectDeleteResult result);
        partial void OnDeleting(ItemObjectDeleteResult result);
        partial void OnCreated();
        partial void OnWriting();
        partial void OnWritten();
        
        protected void CustomerReferenceChanged(object value)
        {
            OnCustomerReferenceChanged(value);
        }
        
        protected object CustomerReferenceWriting(object value)
        {
            OnCustomerReferenceWriting(ref value);
            return value;
        }
        
        protected void DateChanged(object value)
        {
            OnDateChanged(value);
        }
        
        protected object DateWriting(object value)
        {
            OnDateWriting(ref value);
            return value;
        }
        
        protected void OrderNoChanged(object value)
        {
            OnOrderNoChanged(value);
        }
        
        protected object OrderNoWriting(object value)
        {
            OnOrderNoWriting(ref value);
            return value;
        }
        
        protected override void OnItemObjectValidate()
        {
            OnValidate();
        }
        
        protected override void OnItemObjectCanDelete(ItemObjectDeleteResult result)
        {
            OnCanDelete(result);
        }
        
        protected override void OnItemObjectDeleting(ItemObjectDeleteResult result)
        {
            OnDeleting(result);
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
    
    public partial class OrderLine : ItemObject<OrderProcessing>
    {
        public AttributeProperty<OrderProcessing, int> QuantityProperty { get; set;}
        
        public AssociationProperty<OrderProcessing> OnProperty { get; set;}
        
        public Association<OrderProcessing, Product> For { get; private set;}
        
        internal OrderLine(ItemObjectInitializer<OrderProcessing> initializer) : base(initializer)
        {
            QuantityProperty = new AttributeProperty<OrderProcessing, int>(this, "Quantity", "Quantity", false, false, (x) => QuantityChanged(x), (x) => {return QuantityWriting(x);});
            OnProperty = new AssociationProperty<OrderProcessing>(this, "On", "On", false, false, (x) => OnChanged(x), (x) => {return OnWriting(x);});
            For = new Association<OrderProcessing, Product>(this, "For", "For", "");
        }
        partial void OnQuantityChanged(object value);
        partial void OnQuantityWriting(ref object value);
        partial void OnOnChanged(object value);
        partial void OnOnWriting(ref object value);
        partial void OnValidate();
        partial void OnCanDelete(ItemObjectDeleteResult result);
        partial void OnDeleting(ItemObjectDeleteResult result);
        partial void OnCreated();
        partial void OnWriting();
        partial void OnWritten();
        
        protected void QuantityChanged(object value)
        {
            OnQuantityChanged(value);
        }
        
        protected object QuantityWriting(object value)
        {
            OnQuantityWriting(ref value);
            return value;
        }
        
        protected void OnChanged(object value)
        {
            OnOnChanged(value);
        }
        
        protected object OnWriting(object value)
        {
            OnOnWriting(ref value);
            return value;
        }
        
        protected override void OnItemObjectValidate()
        {
            OnValidate();
        }
        
        protected override void OnItemObjectCanDelete(ItemObjectDeleteResult result)
        {
            OnCanDelete(result);
        }
        
        protected override void OnItemObjectDeleting(ItemObjectDeleteResult result)
        {
            OnDeleting(result);
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
    
    public partial class Product : ItemObject<OrderProcessing>
    {
        public AttributeProperty<OrderProcessing, string> CodeProperty { get; set;}
        public AttributeProperty<OrderProcessing, string> DescriptionProperty { get; set;}
        public AttributeProperty<OrderProcessing, decimal> PriceProperty { get; set;}
        public AttributeProperty<OrderProcessing, int> StockLevelProperty { get; set;}
        
        
        public Association<OrderProcessing, ProductGroup> Group { get; private set;}
        public Association<OrderProcessing, ProductGroup> Is { get; private set;}
        public Association<OrderProcessing, OrderLine> OrderedOn { get; private set;}
        
        internal Product(ItemObjectInitializer<OrderProcessing> initializer) : base(initializer)
        {
            CodeProperty = new AttributeProperty<OrderProcessing, string>(this, "Code", "Code", false, false, (x) => CodeChanged(x), (x) => {return CodeWriting(x);});
            DescriptionProperty = new AttributeProperty<OrderProcessing, string>(this, "Description", "Description", false, false, (x) => DescriptionChanged(x), (x) => {return DescriptionWriting(x);});
            PriceProperty = new AttributeProperty<OrderProcessing, decimal>(this, "Price", "Price", false, false, (x) => PriceChanged(x), (x) => {return PriceWriting(x);});
            StockLevelProperty = new AttributeProperty<OrderProcessing, int>(this, "StockLevel", "StockLevel", false, false, (x) => StockLevelChanged(x), (x) => {return StockLevelWriting(x);});
            Group = new Association<OrderProcessing, ProductGroup>(this, "Group", "Group", "");
            Is = new Association<OrderProcessing, ProductGroup>(this, "Is", "Is", "");
            OrderedOn = new Association<OrderProcessing, OrderLine>(this, "OrderedOn", "OrderedOn", "");
        }
        partial void OnCodeChanged(object value);
        partial void OnCodeWriting(ref object value);
        partial void OnDescriptionChanged(object value);
        partial void OnDescriptionWriting(ref object value);
        partial void OnPriceChanged(object value);
        partial void OnPriceWriting(ref object value);
        partial void OnStockLevelChanged(object value);
        partial void OnStockLevelWriting(ref object value);
        partial void OnValidate();
        partial void OnCanDelete(ItemObjectDeleteResult result);
        partial void OnDeleting(ItemObjectDeleteResult result);
        partial void OnCreated();
        partial void OnWriting();
        partial void OnWritten();
        
        protected void CodeChanged(object value)
        {
            OnCodeChanged(value);
        }
        
        protected object CodeWriting(object value)
        {
            OnCodeWriting(ref value);
            return value;
        }
        
        protected void DescriptionChanged(object value)
        {
            OnDescriptionChanged(value);
        }
        
        protected object DescriptionWriting(object value)
        {
            OnDescriptionWriting(ref value);
            return value;
        }
        
        protected void PriceChanged(object value)
        {
            OnPriceChanged(value);
        }
        
        protected object PriceWriting(object value)
        {
            OnPriceWriting(ref value);
            return value;
        }
        
        protected void StockLevelChanged(object value)
        {
            OnStockLevelChanged(value);
        }
        
        protected object StockLevelWriting(object value)
        {
            OnStockLevelWriting(ref value);
            return value;
        }
        
        protected override void OnItemObjectValidate()
        {
            OnValidate();
        }
        
        protected override void OnItemObjectCanDelete(ItemObjectDeleteResult result)
        {
            OnCanDelete(result);
        }
        
        protected override void OnItemObjectDeleting(ItemObjectDeleteResult result)
        {
            OnDeleting(result);
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
    
    public partial class ProductGroup : ItemObject<OrderProcessing>
    {
        public AttributeProperty<OrderProcessing, string> DescriptionProperty { get; set;}
        public AttributeProperty<OrderProcessing, string> NameProperty { get; set;}
        
        
        public Association<OrderProcessing, Product> Contains { get; private set;}
        public Association<OrderProcessing, ProductGroup> ParentGroup { get; private set;}
        public Association<OrderProcessing, ItemObject<OrderProcessing>> SubGroups { get; private set;}
        
        internal ProductGroup(ItemObjectInitializer<OrderProcessing> initializer) : base(initializer)
        {
            DescriptionProperty = new AttributeProperty<OrderProcessing, string>(this, "Description", "Description", false, false, (x) => DescriptionChanged(x), (x) => {return DescriptionWriting(x);});
            NameProperty = new AttributeProperty<OrderProcessing, string>(this, "Name", "Name", false, false, (x) => NameChanged(x), (x) => {return NameWriting(x);});
            Contains = new Association<OrderProcessing, Product>(this, "Contains", "Contains", "");
            ParentGroup = new Association<OrderProcessing, ProductGroup>(this, "ParentGroup", "ParentGroup", "");
            SubGroups = new Association<OrderProcessing, ItemObject<OrderProcessing>>(this, "SubGroups", "SubGroups", "");
        }
        partial void OnDescriptionChanged(object value);
        partial void OnDescriptionWriting(ref object value);
        partial void OnNameChanged(object value);
        partial void OnNameWriting(ref object value);
        partial void OnValidate();
        partial void OnCanDelete(ItemObjectDeleteResult result);
        partial void OnDeleting(ItemObjectDeleteResult result);
        partial void OnCreated();
        partial void OnWriting();
        partial void OnWritten();
        
        protected void DescriptionChanged(object value)
        {
            OnDescriptionChanged(value);
        }
        
        protected object DescriptionWriting(object value)
        {
            OnDescriptionWriting(ref value);
            return value;
        }
        
        protected void NameChanged(object value)
        {
            OnNameChanged(value);
        }
        
        protected object NameWriting(object value)
        {
            OnNameWriting(ref value);
            return value;
        }
        
        protected override void OnItemObjectValidate()
        {
            OnValidate();
        }
        
        protected override void OnItemObjectCanDelete(ItemObjectDeleteResult result)
        {
            OnCanDelete(result);
        }
        
        protected override void OnItemObjectDeleting(ItemObjectDeleteResult result)
        {
            OnDeleting(result);
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
