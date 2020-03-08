using System;
using System.IO;
using Semata.DataStore;

namespace OrderProcessing
{
    using ValueType = Semata.DataStore.ValueType;

    class Program
    {
        static string annotationPrefix = @"
{
  ""semata.datastore.objectmodel"" : 
  [
     ";

        static string annotationSuffix = @"
  ]
}
";
        static string FormatAnnotations(string annotations)
        {
            var formattedAnnotations = "\"" + annotations.Replace(",", "\",\n     \"") + "\"";
            return annotationPrefix + formattedAnnotations + annotationSuffix;
        }

        static string orderProcessingPath = "..\\..\\..\\..\\OrderProcessing\\OrderProcessing.ds";

        static void CreateDataStore(string path)
        {
            /// Create DataStore
            Connection orderProcessing = Instance.Create(path, "OrderProcessing");

            ItemType customerType = orderProcessing.AddItemType("Customer","", "");
            customerType.AddAttributeType("Code", "", FormatAnnotations("mandatory, unique"), ValueType.String);
            customerType.AddAttributeType("Name", "", FormatAnnotations("mandatory"), ValueType.String);
            customerType.AddAttributeType("AddressLine1", "", "", ValueType.String);
            customerType.AddAttributeType("AddressLine2", "", "", ValueType.String);
            customerType.AddAttributeType("AddressLine3", "", "", ValueType.String);
            customerType.AddAttributeType("PostTown", "", "", ValueType.String);
            customerType.AddAttributeType("PostCode", "", "", ValueType.String);

            ItemType productGroupType = orderProcessing.AddItemType("ProductGroup", "", "");
            productGroupType.AddAttributeType("Name", "", FormatAnnotations("mandatory, unique"), ValueType.String);
            productGroupType.AddAttributeType("Description", "", "", ValueType.String);

            /// Create Item Type
            ItemType productType = orderProcessing.AddItemType("Product", "", "");
            AttributeType productCodeAttribute = productType.AddAttributeType("Code", "", FormatAnnotations("mandatory, unique"), ValueType.String);
            /// Create Attribute Type
            productType.AddAttributeType("Description", "", FormatAnnotations("mandatory"), ValueType.String);
            productType.AddAttributeType("StockLevel", "", "", ValueType.Integer);
            productType.AddAttributeType("Price", "", "", ValueType.Decimal);

            ItemType orderType = orderProcessing.AddItemType("Order", "", "");
            orderType.AddAttributeType("OrderNo", "", FormatAnnotations("mandatory, unique"), ValueType.String);
            orderType.AddAttributeType("Date", "", FormatAnnotations("mandatory"), ValueType.Date);
            orderType.AddAttributeType("CustomerReference", "", "", ValueType.String);

            ItemType orderLineType = orderProcessing.AddItemType("OrderLine", "", "");
            orderLineType.AddAttributeType("Quantity", "", FormatAnnotations("mandatory"), ValueType.Integer);

            customerType.AddAssociationType("Has", "", FormatAnnotations("associates_prevent_delete"), orderType, "By", "", FormatAnnotations("property"));
            orderType.AddAssociationType("Lines", "", FormatAnnotations("delete_associates"), orderLineType, "On", "", FormatAnnotations("property"));
            /// Create Association Type
            productType.AddAssociationType("OrderedOn", "", FormatAnnotations("associates_prevent_delete"), orderLineType, "For", "", FormatAnnotations("property"));
            productGroupType.AddAssociationType( "Contains", "", "", productType, "Is", "", FormatAnnotations("property"));

            AssociationTypePair groupTypes = productGroupType.AddAssociationType("SubGroups", "", "", productGroupType, "ParentGroup", "", FormatAnnotations("property"));
            groupTypes.AssociationType.AddAssociate(productType, "Group", "", "");
            orderProcessing.Close();
        }

        static void ExampleProcessing(Connection dataStore)
        {
            ItemType productType = dataStore.GetItemType("Product");
            ItemType orderType = dataStore.GetItemType("Order");
            ItemType orderLineType = dataStore.GetItemType("OrderLine");

            AttributeType productCodeAttribute = productType.GetAttributeType("Code");

            /// Create Item
            Item product = productType.CreateItem();

            product.SetAttribute(productCodeAttribute, "P0001");
            /// Set Attribute
            product.SetAttribute("Code", "P0001");
            /// Get Attribute
            string code = (string)product.GetAttribute("Code");

            Item order = orderType.CreateItem();
            order.SetAttribute("OrderNo", "N001");
            Item orderLine = orderLineType.CreateItem();
            orderLine.SetAttribute("Quantity",  4);

            /// Add Association
            orderLine.AddAssociation("For", product);
            /// Remove Association
            orderLine.RemoveAssociation("For", product);

            orderLine.AddAssociation("For", product);
            orderLine.AddAssociation("On", order);

            Iterator<Item> productIterator = orderLine.GetAssociations("For");
            product = productIterator.GetCurrent();
            product = orderLine.GetAssociations("For").GetCurrent();

            /// Get Associations
            Iterator<Item> orderLineIterator = product.GetAssociations("OrderedOn");
            /// Get Associations
            while (!orderLineIterator.GetDone())
            /// Get Associations
            {
                /// Get Associations
                Console.WriteLine("Order:" + orderLineIterator.GetCurrent().GetAssociations("On").GetCurrent().GetAttribute("OrderNo"));
                /// Get Associations
                orderLineIterator.Next();
            /// Get Associations
            }

            productType.GetAttributeType("StockLevel").GetItemsByValue(0);

            /// Delete Item
            product.Delete();

        }


        static Item CreateCustomer(Connection orderProcessing
                            , string code
                            , string name
                            , string addressLine1
                            , string addressLine2
                            , string addressLine3
                            , string postTown
                            , string postCode)
        {
            Item customer = orderProcessing.GetItemType("Customer").CreateItem();
            customer.SetAttribute("Code", code);
            customer.SetAttribute("Name", name);
            customer.SetAttribute("AddressLine1", addressLine1);
            customer.SetAttribute("AddressLine2", addressLine2);
            customer.SetAttribute("AddressLine3", addressLine3);
            customer.SetAttribute("PostTown", postTown);
            customer.SetAttribute("PostCode",postCode);
            return customer;
        }

        static Item CreateProductGroup(Connection orderProcessing, Item parentGroup, string name, string description)
        {
            Item productGroup = orderProcessing.GetItemType("ProductGroup").CreateItem();
            productGroup.SetAttribute("Name", name);
            productGroup.SetAttribute("Description", description);
            if (parentGroup != null)
            {
                productGroup.AddAssociation("ParentGroup", parentGroup);
            }
            return productGroup;
        }

        static Item CreateProduct(Connection orderProcessing, string code, string description, int stockLevel, decimal price, Item productGroup)
        {
            Item product = orderProcessing.GetItemType("Product").CreateItem();
            product.SetAttribute("Code", code);
            product.SetAttribute("Description", description);
            product.SetAttribute("StockLevel", stockLevel);
            product.SetAttribute("Price", price);
            if (productGroup != null)
            {
                product.AddAssociation("Is", productGroup);
            }
            return product;
        }

        static Item CreateOrder(Connection orderProcessing, Item customer, string orderNo, DateTime date, string customerReference)
        {
            Item order = orderProcessing.GetItemType("Order").CreateItem();
            order.SetAttribute("OrderNo", orderNo);
            order.SetAttribute("Date", date);
            order.SetAttribute("CustomerReference", customerReference);
            order.AddAssociation("By", customer);
            return order;
        }

        static Item CreateOrderLine(Connection orderProcessing, Item order, Item product, int quantity)
        {
            Item orderLine = orderProcessing.GetItemType("OrderLine").CreateItem();
            orderLine.SetAttribute("Quantity", quantity);
            orderLine.AddAssociation("For", product);
            orderLine.AddAssociation("On", order);
            return orderLine;
        }

        class ProductQuantity
        {
            public string ProductCode { get; private set; }
            public int Quantity { get; private set; }
            public ProductQuantity(string productCode, int quantity)
            {
                ProductCode = productCode;
                Quantity = quantity;
            }
        };

        static Item GetCustomer(Connection orderProcessing, string customerCode)
        {
            Iterator<Item> items = orderProcessing.GetItemType("Customer").GetAttributeType("Code").GetItemsByValue(customerCode);
            if (items.GetDone())
            {
                throw new System.Exception("Customer not found");
            }
            Item customer = items.GetCurrent();
            items.Next();
            if (!items.GetDone())
            {
                throw new System.Exception("More than one Customer found");
            }
            return customer;
        }

        static Item GetProduct(Connection orderProcessing, string productCode)
        {
            Iterator<Item> items = orderProcessing.GetItemType("Product").GetAttributeType("Code").GetItemsByValue(productCode);
            if (items.GetDone())
            {
                throw new System.Exception("Product not found");
            }
            Item product = items.GetCurrent();
            items.Next();
            if (!items.GetDone())
            {
                throw new System.Exception("More than one Product found");
            }
            return product;
        }

        static void CreateCustomerOrder(Connection orderProcessing
                                        , string customerCode
                                        , string orderNo
                                        , DateTime date
                                        , string customerReference
                                        , ProductQuantity[] lines)
        {
            Item customer = GetCustomer(orderProcessing, customerCode);
            Item order = CreateOrder(orderProcessing, customer, orderNo, date, customerReference);
            foreach (ProductQuantity productQuantity in lines)
            {
                Item product = GetProduct(orderProcessing, productQuantity.ProductCode);
                CreateOrderLine(orderProcessing, order,  product, productQuantity.Quantity);
            }
        }

        static void PopulateDataStore(Connection orderProcessing)
        {
            Item JH001 = CreateCustomer(orderProcessing, "JH001", "Jones Hardware", "28 High Street", "", "", "Bristol", "BS54 2XZ");
            Item HD001 = CreateCustomer(orderProcessing, "HD001", "Hamptons DIY", "3 Portland Avenue", "", "", "Bath", "BA42 7UV");
            Item LHI01 = CreateCustomer(orderProcessing, "LH001", "Lansdown Home Improvements", "11 London Road", "", "", "Keynsham", "BA23 9AH");

            Item fixings = CreateProductGroup(orderProcessing, null, "Fixings","");
            Item screws = CreateProductGroup(orderProcessing, fixings, "Screws","");
            Item countersunkScrews = CreateProductGroup(orderProcessing, screws, "Countersunk Screws","");
            Item roundheadScrews = CreateProductGroup(orderProcessing, screws, "RoundHead Screws","");
            Item nails = CreateProductGroup(orderProcessing, fixings, "Nails", "");

            CreateProduct(orderProcessing,"10002", "Countersunk Screws 3 x 12mm Pack of 200", 6, 1.66m, countersunkScrews);
            CreateProduct(orderProcessing,"10003", "Countersunk Screws 3 x 16mm Pack of 200", 19, 2.50m, countersunkScrews);
            CreateProduct(orderProcessing,"10004", "Countersunk Screws 3 x 20mm Pack of 200", 2, 2.60m, countersunkScrews);
            CreateProduct(orderProcessing,"10005", "Countersunk Screws 3 x 25mm Pack of 200", 5, 2.90m, countersunkScrews);
            CreateProduct(orderProcessing,"10006", "Countersunk Screws 3 x 30mm Pack of 200", 8, 3.26m, countersunkScrews);
            CreateProduct(orderProcessing,"10007", "Countersunk Screws 3.5 x 12mm Pack of 200", 11, 2.60m, countersunkScrews);
            CreateProduct(orderProcessing,"10008", "Countersunk Screws 3.5 x 16mm Pack of 200", 14, 2.86m, countersunkScrews);
            CreateProduct(orderProcessing,"10009", "Countersunk Screws 3.5 x 20mm Pack of 200", 17, 3.01m, countersunkScrews);
            CreateProduct(orderProcessing,"10010", "Countersunk Screws 3.5 x 25mm Pack of 200", 0, 3.43m, countersunkScrews);
            CreateProduct(orderProcessing,"10011", "Countersunk Screws 3.5 x 30mm Pack of 200", 3, 3.74m, countersunkScrews);
            CreateProduct(orderProcessing,"10012", "Countersunk Screws 3.5 x 40mm Pack of 200", 6, 4.39m, countersunkScrews);
            CreateProduct(orderProcessing,"10013", "Countersunk Screws 4 x 16mm Pack of 200", 9, 3.24m, countersunkScrews);
            CreateProduct(orderProcessing,"10014", "Countersunk Screws 4 x 20mm Pack of 200", 12, 3.48m, countersunkScrews);
            CreateProduct(orderProcessing,"10015", "Countersunk Screws 4 x 25mm Pack of 200", 4, 3.78m, countersunkScrews);
            CreateProduct(orderProcessing,"10016", "Countersunk Screws 4 x 30mm Pack of 200", 7, 4.38m, countersunkScrews);
            CreateProduct(orderProcessing,"10017", "Countersunk Screws 4 x 35mm Pack of 200", 10, 4.75m, countersunkScrews);
            CreateProduct(orderProcessing,"10018", "Countersunk Screws 4 x 40mm Pack of 200", 13, 5.29m, countersunkScrews);
            CreateProduct(orderProcessing,"10019", "Countersunk Screws 4 x 45mm Pack of 200", 16, 5.66m, countersunkScrews);
            CreateProduct(orderProcessing,"10020", "Countersunk Screws 4.5 x 25mm Pack of 200", 19, 4.65m, countersunkScrews);
            CreateProduct(orderProcessing,"10021", "Countersunk Screws 4 x 50mm Pack of 200", 2, 6.20m, countersunkScrews);
            CreateProduct(orderProcessing,"10022", "Countersunk Screws 4.5 x 30mm Pack of 200", 5, 5.20m, countersunkScrews);
            CreateProduct(orderProcessing,"10023", "Countersunk Screws 4.5 x 40mm Pack of 200", 8, 6.31m, countersunkScrews);
            CreateProduct(orderProcessing,"10024", "Countersunk Screws 4.5 x 45mm Pack of 200", 11, 6.96m, countersunkScrews);
            CreateProduct(orderProcessing,"10025", "Countersunk Screws 4.5 x 50mm Pack of 200", 14, 7.59m, countersunkScrews);
            CreateProduct(orderProcessing,"10027", "Countersunk Screws 5 x 30mm Pack of 200", 19, 6.15m, countersunkScrews);
            CreateProduct(orderProcessing,"10028", "Countersunk Screws 5 x 40mm Pack of 200", 2, 7.78m, countersunkScrews);
            CreateProduct(orderProcessing,"10029", "Countersunk Screws 5 x 45mm Pack of 200", 15, 8.34m, countersunkScrews);
            CreateProduct(orderProcessing,"10030", "Countersunk Screws 5 x 50mm Pack of 200", 18, 9.14m, countersunkScrews);
            CreateProduct(orderProcessing,"20002", "Round Head Screws 3.5 x 25mm Pack of 200", 6, 1.35m, roundheadScrews);
            CreateProduct(orderProcessing,"20004", "Round Head Screws 4 x 30mm Pack of 200", 14, 1.69m, roundheadScrews);
            CreateProduct(orderProcessing,"20005", "Round Head Screws 4 x 40mm Pack of 200", 7, 1.86m, roundheadScrews);
            CreateProduct(orderProcessing,"20007", "Round Head Screws 4 x 50mm Pack of 200", 14, 2.13m, roundheadScrews);
            CreateProduct(orderProcessing,"20009", "Round Head Screws 5 x 50mm Pack of 200", 11, 3.55m, roundheadScrews);
            CreateProduct(orderProcessing,"20010", "Round Head Screws 5 x 60mm Pack of 100", 4, 2.20m, roundheadScrews);
            CreateProduct(orderProcessing,"20012", "Round Head Screws 5 x 80mm Pack of 100", 1, 2.89m, roundheadScrews);
            CreateProduct(orderProcessing,"20013", "Round Head Screws 6 x 100mm Pack of 100", 13, 5.08m, roundheadScrews);

            CreateProduct(orderProcessing,"30002", "Nails 3.75 x 75mm 1kg Pack ", 6, 3.87m, nails);
            CreateProduct(orderProcessing,"30003", "Nails 4.50 x 100mm 1kg Pack ", 9, 3.87m, nails);
            CreateProduct(orderProcessing,"30004", "Nails 5.60 x 125mm 1kg Pack ", 12, 3.87m, nails);
            CreateProduct(orderProcessing,"30005", "Nails 6.00 x 150mm 1kg Pack ", 15, 3.87m, nails);

            CreateCustomerOrder(orderProcessing
                                , "JH001"
                                , "001"
                                , new DateTime(2014, 5, 13)
                                , "JH-123"
                                , new ProductQuantity[] { new ProductQuantity("10002", 2)
                                                          , new ProductQuantity("20004", 3)  } );
            CreateCustomerOrder(orderProcessing
                                , "JH001"
                                , "002"
                                , new DateTime(2014, 5, 14)
                                , "JH-456"
                                , new ProductQuantity[] { new ProductQuantity("10003", 5)
                                                          , new ProductQuantity("20007", 5) 
                                                          , new ProductQuantity("30002", 10) });
            CreateCustomerOrder(orderProcessing
                                , "HD001"
                                , "003"
                                , new DateTime(2014, 5, 15)
                                , "HD/5/15"
                                , new ProductQuantity[] { new ProductQuantity("10004", 7)});
        }

        static void Main(string[] args)
        {
            try
            {
                if (File.Exists(orderProcessingPath))
                {
                    File.Delete(orderProcessingPath);
                }

                CreateDataStore(orderProcessingPath);
                /// Open DataStore
                Connection orderProcessing = Instance.Open(orderProcessingPath);
                PopulateDataStore(orderProcessing);
                ExampleProcessing(orderProcessing);

            }
            catch (System.Exception exception)
            {
                Console.WriteLine(exception.Message);
                Console.ReadKey();
            }
        }
    }
}
