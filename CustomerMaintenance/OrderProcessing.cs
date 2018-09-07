﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Semata.DataStore;
using Semata.DataStore.ObjectModel;
using System.ComponentModel;

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
        
        public void Open(string path)
        {
            base.OpenInstance(path);
            SetActivator("Customer", (initializer) => new Customer(initializer));
            SetActivator("Order", (initializer) => new Order(initializer));
            SetActivator("OrderLine", (initializer) => new OrderLine(initializer));
            SetActivator("Product", (initializer) => new Product(initializer));
            SetActivator("ProductGroup", (initializer) => new ProductGroup(initializer));
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
        private AttributeProperty<OrderProcessing, string> AddressLine1Property { get; set;}
        private AttributeProperty<OrderProcessing, string> AddressLine2Property { get; set;}
        private AttributeProperty<OrderProcessing, string> AddressLine3Property { get; set;}
        private AttributeProperty<OrderProcessing, string> CodeProperty { get; set;}
        private AttributeProperty<OrderProcessing, string> NameProperty { get; set;}
        private AttributeProperty<OrderProcessing, string> PostCodeProperty { get; set;}
        private AttributeProperty<OrderProcessing, string> PostTownProperty { get; set;}
        
        
        public Association<OrderProcessing, Order> Has { get; private set;}
        
        internal Customer(ItemObjectInitializer<OrderProcessing> initializer) : base(initializer)
        {
            AddressLine1Property = new AttributeProperty<OrderProcessing, string>(this, "AddressLine1", "AddressLine1", false, false, (x) => OnAddressLine1Changed(x), (x) => {return OnAddressLine1Writing(x);});
            AddAttributeProperty(AddressLine1Property);
            AddressLine2Property = new AttributeProperty<OrderProcessing, string>(this, "AddressLine2", "AddressLine2", false, false, (x) => OnAddressLine2Changed(x), (x) => {return OnAddressLine2Writing(x);});
            AddAttributeProperty(AddressLine2Property);
            AddressLine3Property = new AttributeProperty<OrderProcessing, string>(this, "AddressLine3", "AddressLine3", false, false, (x) => OnAddressLine3Changed(x), (x) => {return OnAddressLine3Writing(x);});
            AddAttributeProperty(AddressLine3Property);
            CodeProperty = new AttributeProperty<OrderProcessing, string>(this, "Code", "Code", false, false, (x) => OnCodeChanged(x), (x) => {return OnCodeWriting(x);});
            AddAttributeProperty(CodeProperty);
            NameProperty = new AttributeProperty<OrderProcessing, string>(this, "Name", "Name", false, false, (x) => OnNameChanged(x), (x) => {return OnNameWriting(x);});
            AddAttributeProperty(NameProperty);
            PostCodeProperty = new AttributeProperty<OrderProcessing, string>(this, "PostCode", "PostCode", false, false, (x) => OnPostCodeChanged(x), (x) => {return OnPostCodeWriting(x);});
            AddAttributeProperty(PostCodeProperty);
            PostTownProperty = new AttributeProperty<OrderProcessing, string>(this, "PostTown", "PostTown", false, false, (x) => OnPostTownChanged(x), (x) => {return OnPostTownWriting(x);});
            AddAttributeProperty(PostTownProperty);
            Has = new Association<OrderProcessing, Order>(this, "Has", "Has", "");
            AddAssociation("Has", Has);
        }
        partial void AddressLine1Changed(object value);
        partial void AddressLine1Writing(ref object value);
        partial void AddressLine2Changed(object value);
        partial void AddressLine2Writing(ref object value);
        partial void AddressLine3Changed(object value);
        partial void AddressLine3Writing(ref object value);
        partial void CodeChanged(object value);
        partial void CodeWriting(ref object value);
        partial void NameChanged(object value);
        partial void NameWriting(ref object value);
        partial void PostCodeChanged(object value);
        partial void PostCodeWriting(ref object value);
        partial void PostTownChanged(object value);
        partial void PostTownWriting(ref object value);
        partial void Validate();
        partial void CanDelete(ItemObjectDeleteResult result);
        partial void Deleting(ItemObjectDeleteResult result);
        partial void Created();
        partial void Writing();
        partial void Written();
        
        protected void OnAddressLine1Changed(object value)
        {
            AddressLine1Changed(value);
        }
        
        protected object OnAddressLine1Writing(object value)
        {
            AddressLine1Writing(ref value);
            return value;
        }
        
        protected void OnAddressLine2Changed(object value)
        {
            AddressLine2Changed(value);
        }
        
        protected object OnAddressLine2Writing(object value)
        {
            AddressLine2Writing(ref value);
            return value;
        }
        
        protected void OnAddressLine3Changed(object value)
        {
            AddressLine3Changed(value);
        }
        
        protected object OnAddressLine3Writing(object value)
        {
            AddressLine3Writing(ref value);
            return value;
        }
        
        protected void OnCodeChanged(object value)
        {
            CodeChanged(value);
        }
        
        protected object OnCodeWriting(object value)
        {
            CodeWriting(ref value);
            return value;
        }
        
        protected void OnNameChanged(object value)
        {
            NameChanged(value);
        }
        
        protected object OnNameWriting(object value)
        {
            NameWriting(ref value);
            return value;
        }
        
        protected void OnPostCodeChanged(object value)
        {
            PostCodeChanged(value);
        }
        
        protected object OnPostCodeWriting(object value)
        {
            PostCodeWriting(ref value);
            return value;
        }
        
        protected void OnPostTownChanged(object value)
        {
            PostTownChanged(value);
        }
        
        protected object OnPostTownWriting(object value)
        {
            PostTownWriting(ref value);
            return value;
        }
        
        protected override void OnItemObjectValidate()
        {
            Validate();
        }
        
        protected override void OnItemObjectCanDelete(ItemObjectDeleteResult result)
        {
            CanDelete(result);
        }
        
        protected override void OnItemObjectDeleting(ItemObjectDeleteResult result)
        {
            Deleting(result);
        }
        
        protected override void OnItemObjectCreated()
        {
            Created();
        }
        
        protected override void OnItemObjectWriting()
        {
            Writing();
        }
        
        protected override void OnItemObjectWritten()
        {
            Written();
        }
        
        
        public string AddressLine1
        {
            get
            {
                return (string)AddressLine1Property.Value;
            }
            set
            {
                AddressLine1Property.Value = value;
            }
        }
        
        public string AddressLine2
        {
            get
            {
                return (string)AddressLine2Property.Value;
            }
            set
            {
                AddressLine2Property.Value = value;
            }
        }
        
        public string AddressLine3
        {
            get
            {
                return (string)AddressLine3Property.Value;
            }
            set
            {
                AddressLine3Property.Value = value;
            }
        }
        
        public string Code
        {
            get
            {
                return (string)CodeProperty.Value;
            }
            set
            {
                CodeProperty.Value = value;
            }
        }
        
        public string Name
        {
            get
            {
                return (string)NameProperty.Value;
            }
            set
            {
                NameProperty.Value = value;
            }
        }
        
        public string PostCode
        {
            get
            {
                return (string)PostCodeProperty.Value;
            }
            set
            {
                PostCodeProperty.Value = value;
            }
        }
        
        public string PostTown
        {
            get
            {
                return (string)PostTownProperty.Value;
            }
            set
            {
                PostTownProperty.Value = value;
            }
        }
    }
    
    public partial class Order : ItemObject<OrderProcessing>
    {
        private AttributeProperty<OrderProcessing, string> CustomerReferenceProperty { get; set;}
        private AttributeProperty<OrderProcessing, DateTime> DateProperty { get; set;}
        private AttributeProperty<OrderProcessing, string> OrderNoProperty { get; set;}
        
        
        public Association<OrderProcessing, Customer> By { get; private set;}
        public Association<OrderProcessing, OrderLine> Lines { get; private set;}
        
        internal Order(ItemObjectInitializer<OrderProcessing> initializer) : base(initializer)
        {
            CustomerReferenceProperty = new AttributeProperty<OrderProcessing, string>(this, "CustomerReference", "CustomerReference", false, false, (x) => OnCustomerReferenceChanged(x), (x) => {return OnCustomerReferenceWriting(x);});
            AddAttributeProperty(CustomerReferenceProperty);
            DateProperty = new AttributeProperty<OrderProcessing, DateTime>(this, "Date", "Date", false, false, (x) => OnDateChanged(x), (x) => {return OnDateWriting(x);});
            AddAttributeProperty(DateProperty);
            OrderNoProperty = new AttributeProperty<OrderProcessing, string>(this, "OrderNo", "OrderNo", false, false, (x) => OnOrderNoChanged(x), (x) => {return OnOrderNoWriting(x);});
            AddAttributeProperty(OrderNoProperty);
            By = new Association<OrderProcessing, Customer>(this, "By", "By", "");
            AddAssociation("By", By);
            Lines = new Association<OrderProcessing, OrderLine>(this, "Lines", "Lines", "");
            AddAssociation("Lines", Lines);
        }
        partial void CustomerReferenceChanged(object value);
        partial void CustomerReferenceWriting(ref object value);
        partial void DateChanged(object value);
        partial void DateWriting(ref object value);
        partial void OrderNoChanged(object value);
        partial void OrderNoWriting(ref object value);
        partial void Validate();
        partial void CanDelete(ItemObjectDeleteResult result);
        partial void Deleting(ItemObjectDeleteResult result);
        partial void Created();
        partial void Writing();
        partial void Written();
        
        protected void OnCustomerReferenceChanged(object value)
        {
            CustomerReferenceChanged(value);
        }
        
        protected object OnCustomerReferenceWriting(object value)
        {
            CustomerReferenceWriting(ref value);
            return value;
        }
        
        protected void OnDateChanged(object value)
        {
            DateChanged(value);
        }
        
        protected object OnDateWriting(object value)
        {
            DateWriting(ref value);
            return value;
        }
        
        protected void OnOrderNoChanged(object value)
        {
            OrderNoChanged(value);
        }
        
        protected object OnOrderNoWriting(object value)
        {
            OrderNoWriting(ref value);
            return value;
        }
        
        protected override void OnItemObjectValidate()
        {
            Validate();
        }
        
        protected override void OnItemObjectCanDelete(ItemObjectDeleteResult result)
        {
            CanDelete(result);
        }
        
        protected override void OnItemObjectDeleting(ItemObjectDeleteResult result)
        {
            Deleting(result);
        }
        
        protected override void OnItemObjectCreated()
        {
            Created();
        }
        
        protected override void OnItemObjectWriting()
        {
            Writing();
        }
        
        protected override void OnItemObjectWritten()
        {
            Written();
        }
        
        
        public string CustomerReference
        {
            get
            {
                return (string)CustomerReferenceProperty.Value;
            }
            set
            {
                CustomerReferenceProperty.Value = value;
            }
        }
        
        public DateTime? Date
        {
            get
            {
                return (DateTime?)DateProperty.Value;
            }
            set
            {
                DateProperty.Value = value;
            }
        }
        
        public string OrderNo
        {
            get
            {
                return (string)OrderNoProperty.Value;
            }
            set
            {
                OrderNoProperty.Value = value;
            }
        }
    }
    
    public partial class OrderLine : ItemObject<OrderProcessing>
    {
        private AttributeProperty<OrderProcessing, int> QuantityProperty { get; set;}
        
        
        public Association<OrderProcessing, Product> For { get; private set;}
        public Association<OrderProcessing, Order> On { get; private set;}
        
        internal OrderLine(ItemObjectInitializer<OrderProcessing> initializer) : base(initializer)
        {
            QuantityProperty = new AttributeProperty<OrderProcessing, int>(this, "Quantity", "Quantity", false, false, (x) => OnQuantityChanged(x), (x) => {return OnQuantityWriting(x);});
            AddAttributeProperty(QuantityProperty);
            For = new Association<OrderProcessing, Product>(this, "For", "For", "");
            AddAssociation("For", For);
            On = new Association<OrderProcessing, Order>(this, "On", "On", "");
            AddAssociation("On", On);
        }
        partial void QuantityChanged(object value);
        partial void QuantityWriting(ref object value);
        partial void Validate();
        partial void CanDelete(ItemObjectDeleteResult result);
        partial void Deleting(ItemObjectDeleteResult result);
        partial void Created();
        partial void Writing();
        partial void Written();
        
        protected void OnQuantityChanged(object value)
        {
            QuantityChanged(value);
        }
        
        protected object OnQuantityWriting(object value)
        {
            QuantityWriting(ref value);
            return value;
        }
        
        protected override void OnItemObjectValidate()
        {
            Validate();
        }
        
        protected override void OnItemObjectCanDelete(ItemObjectDeleteResult result)
        {
            CanDelete(result);
        }
        
        protected override void OnItemObjectDeleting(ItemObjectDeleteResult result)
        {
            Deleting(result);
        }
        
        protected override void OnItemObjectCreated()
        {
            Created();
        }
        
        protected override void OnItemObjectWriting()
        {
            Writing();
        }
        
        protected override void OnItemObjectWritten()
        {
            Written();
        }
        
        
        public int? Quantity
        {
            get
            {
                return (int?)QuantityProperty.Value;
            }
            set
            {
                QuantityProperty.Value = value;
            }
        }
    }
    
    public partial class Product : ItemObject<OrderProcessing>
    {
        private AttributeProperty<OrderProcessing, string> CodeProperty { get; set;}
        private AttributeProperty<OrderProcessing, string> DescriptionProperty { get; set;}
        private AttributeProperty<OrderProcessing, decimal> PriceProperty { get; set;}
        private AttributeProperty<OrderProcessing, int> StockLevelProperty { get; set;}
        
        
        public Association<OrderProcessing, ProductGroup> Group { get; private set;}
        public Association<OrderProcessing, ProductGroup> Is { get; private set;}
        public Association<OrderProcessing, OrderLine> OrderedOn { get; private set;}
        
        internal Product(ItemObjectInitializer<OrderProcessing> initializer) : base(initializer)
        {
            CodeProperty = new AttributeProperty<OrderProcessing, string>(this, "Code", "Code", false, false, (x) => OnCodeChanged(x), (x) => {return OnCodeWriting(x);});
            AddAttributeProperty(CodeProperty);
            DescriptionProperty = new AttributeProperty<OrderProcessing, string>(this, "Description", "Description", false, false, (x) => OnDescriptionChanged(x), (x) => {return OnDescriptionWriting(x);});
            AddAttributeProperty(DescriptionProperty);
            PriceProperty = new AttributeProperty<OrderProcessing, decimal>(this, "Price", "Price", false, false, (x) => OnPriceChanged(x), (x) => {return OnPriceWriting(x);});
            AddAttributeProperty(PriceProperty);
            StockLevelProperty = new AttributeProperty<OrderProcessing, int>(this, "StockLevel", "StockLevel", false, false, (x) => OnStockLevelChanged(x), (x) => {return OnStockLevelWriting(x);});
            AddAttributeProperty(StockLevelProperty);
            Group = new Association<OrderProcessing, ProductGroup>(this, "Group", "Group", "");
            AddAssociation("Group", Group);
            Is = new Association<OrderProcessing, ProductGroup>(this, "Is", "Is", "");
            AddAssociation("Is", Is);
            OrderedOn = new Association<OrderProcessing, OrderLine>(this, "OrderedOn", "OrderedOn", "");
            AddAssociation("OrderedOn", OrderedOn);
        }
        partial void CodeChanged(object value);
        partial void CodeWriting(ref object value);
        partial void DescriptionChanged(object value);
        partial void DescriptionWriting(ref object value);
        partial void PriceChanged(object value);
        partial void PriceWriting(ref object value);
        partial void StockLevelChanged(object value);
        partial void StockLevelWriting(ref object value);
        partial void Validate();
        partial void CanDelete(ItemObjectDeleteResult result);
        partial void Deleting(ItemObjectDeleteResult result);
        partial void Created();
        partial void Writing();
        partial void Written();
        
        protected void OnCodeChanged(object value)
        {
            CodeChanged(value);
        }
        
        protected object OnCodeWriting(object value)
        {
            CodeWriting(ref value);
            return value;
        }
        
        protected void OnDescriptionChanged(object value)
        {
            DescriptionChanged(value);
        }
        
        protected object OnDescriptionWriting(object value)
        {
            DescriptionWriting(ref value);
            return value;
        }
        
        protected void OnPriceChanged(object value)
        {
            PriceChanged(value);
        }
        
        protected object OnPriceWriting(object value)
        {
            PriceWriting(ref value);
            return value;
        }
        
        protected void OnStockLevelChanged(object value)
        {
            StockLevelChanged(value);
        }
        
        protected object OnStockLevelWriting(object value)
        {
            StockLevelWriting(ref value);
            return value;
        }
        
        protected override void OnItemObjectValidate()
        {
            Validate();
        }
        
        protected override void OnItemObjectCanDelete(ItemObjectDeleteResult result)
        {
            CanDelete(result);
        }
        
        protected override void OnItemObjectDeleting(ItemObjectDeleteResult result)
        {
            Deleting(result);
        }
        
        protected override void OnItemObjectCreated()
        {
            Created();
        }
        
        protected override void OnItemObjectWriting()
        {
            Writing();
        }
        
        protected override void OnItemObjectWritten()
        {
            Written();
        }
        
        
        public string Code
        {
            get
            {
                return (string)CodeProperty.Value;
            }
            set
            {
                CodeProperty.Value = value;
            }
        }
        
        public string Description
        {
            get
            {
                return (string)DescriptionProperty.Value;
            }
            set
            {
                DescriptionProperty.Value = value;
            }
        }
        
        public decimal? Price
        {
            get
            {
                return (decimal?)PriceProperty.Value;
            }
            set
            {
                PriceProperty.Value = value;
            }
        }
        
        public int? StockLevel
        {
            get
            {
                return (int?)StockLevelProperty.Value;
            }
            set
            {
                StockLevelProperty.Value = value;
            }
        }
    }
    
    public partial class ProductGroup : ItemObject<OrderProcessing>
    {
        private AttributeProperty<OrderProcessing, string> DescriptionProperty { get; set;}
        private AttributeProperty<OrderProcessing, string> NameProperty { get; set;}
        
        
        public Association<OrderProcessing, Product> Contains { get; private set;}
        public Association<OrderProcessing, ProductGroup> ParentGroup { get; private set;}
        public Association<OrderProcessing, ItemObject<OrderProcessing>> SubGroups { get; private set;}
        
        internal ProductGroup(ItemObjectInitializer<OrderProcessing> initializer) : base(initializer)
        {
            DescriptionProperty = new AttributeProperty<OrderProcessing, string>(this, "Description", "Description", false, false, (x) => OnDescriptionChanged(x), (x) => {return OnDescriptionWriting(x);});
            AddAttributeProperty(DescriptionProperty);
            NameProperty = new AttributeProperty<OrderProcessing, string>(this, "Name", "Name", false, false, (x) => OnNameChanged(x), (x) => {return OnNameWriting(x);});
            AddAttributeProperty(NameProperty);
            Contains = new Association<OrderProcessing, Product>(this, "Contains", "Contains", "");
            AddAssociation("Contains", Contains);
            ParentGroup = new Association<OrderProcessing, ProductGroup>(this, "ParentGroup", "ParentGroup", "");
            AddAssociation("ParentGroup", ParentGroup);
            SubGroups = new Association<OrderProcessing, ItemObject<OrderProcessing>>(this, "SubGroups", "SubGroups", "");
            AddAssociation("SubGroups", SubGroups);
        }
        partial void DescriptionChanged(object value);
        partial void DescriptionWriting(ref object value);
        partial void NameChanged(object value);
        partial void NameWriting(ref object value);
        partial void Validate();
        partial void CanDelete(ItemObjectDeleteResult result);
        partial void Deleting(ItemObjectDeleteResult result);
        partial void Created();
        partial void Writing();
        partial void Written();
        
        protected void OnDescriptionChanged(object value)
        {
            DescriptionChanged(value);
        }
        
        protected object OnDescriptionWriting(object value)
        {
            DescriptionWriting(ref value);
            return value;
        }
        
        protected void OnNameChanged(object value)
        {
            NameChanged(value);
        }
        
        protected object OnNameWriting(object value)
        {
            NameWriting(ref value);
            return value;
        }
        
        protected override void OnItemObjectValidate()
        {
            Validate();
        }
        
        protected override void OnItemObjectCanDelete(ItemObjectDeleteResult result)
        {
            CanDelete(result);
        }
        
        protected override void OnItemObjectDeleting(ItemObjectDeleteResult result)
        {
            Deleting(result);
        }
        
        protected override void OnItemObjectCreated()
        {
            Created();
        }
        
        protected override void OnItemObjectWriting()
        {
            Writing();
        }
        
        protected override void OnItemObjectWritten()
        {
            Written();
        }
        
        
        public string Description
        {
            get
            {
                return (string)DescriptionProperty.Value;
            }
            set
            {
                DescriptionProperty.Value = value;
            }
        }
        
        public string Name
        {
            get
            {
                return (string)NameProperty.Value;
            }
            set
            {
                NameProperty.Value = value;
            }
        }
    }
    
}
