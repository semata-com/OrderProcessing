using System;
using System.Windows.Input;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semata.DataStore.ObjectModel;
using Semata.DataStore.ObjectModel.Views;
using Semata.DataView;
using Semata.Lazy;

namespace CustomerMaintenance
{
    public partial class OrderProcessingDataStoreView
    {
        LazyValue<ItemObjectViewList<CustomerView>> orderedCustomers_;
        LazyValue<SelectorDetailSource> editableCustomers_;

        partial void OnInitialize()
        {
            orderedCustomers_ =
                new LazyValue<ItemObjectViewList<CustomerView>>(() =>
                    new ItemObjectViewList<Customer, CustomerView>
                        (dataStore_.CustomerItems.GetItemObjectSet()
                         , z => z.OrderBy((y) => y.Code)
                         , (x) => new CustomerView(x, false, false)));

            editableCustomers_ =
                new LazyValue<SelectorDetailSource>(() =>
                    new SelectorDetailSource
                        (OrderedCustomers
                         , (x) => x is CustomerView ? new CustomerView(x as CustomerView, true, true) : null
                         , () => System.Windows.MessageBox.Show("Has been edited, Save Changes?", "Customers", System.Windows.MessageBoxButton.OKCancel) == System.Windows.MessageBoxResult.OK));
        }

        public ItemObjectViewList<CustomerView> OrderedCustomers => orderedCustomers_.Value;

        public SelectorDetailSource EditableCustomers => editableCustomers_.Value;

    }


}

