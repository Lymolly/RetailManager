using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace RetailManagerDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private BindingList<string> _products;
        private BindingList<string> _cart;
        private string _itemQtity;
        public BindingList<string> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public BindingList<string> Cart { get; set; }
        public string ItemQuantity
        {
            get { return _itemQtity; }
            set
            {
                _itemQtity = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }

        public string SubTotal
        {
            //TODO Calulate 
            get => "$0.00";
        }
        public string Total
        {
            //TODO Calulate 
            get => "$0.00";
        }
        public string Tax
        {
            //TODO Calulate 
            get => "$0.00";
        }

        public bool CanAddToCart
        {
            get
            {
                bool output = false;
                return output;
            }
        }
        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;
                return output;
            }
        }
        public bool CanCheckOut
        {
            get
            {
                bool output = false;
                //Make sure something in the cart
                return output;
            }
        }

        public void CheckOut()
        {

        }

        public void AddToCart()
        {

        }

        public void RemoveFromCart()
        {

        }
    }
}
