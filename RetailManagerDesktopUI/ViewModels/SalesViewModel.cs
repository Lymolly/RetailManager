using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using RetailManagerDesktopUI.Library.Api;
using RetailManagerDesktopUI.Library.Models;

namespace RetailManagerDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private IProductEndpoint _productEndpoint;
        public SalesViewModel(IProductEndpoint productEndpoint)
        {
            _productEndpoint = productEndpoint;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            await LoadProducts();
        }
        private async Task LoadProducts()
        {
            var products = await _productEndpoint.GetAll();
            Products = new BindingList<ProductModel>(products);
        }
        
        
        
        private BindingList<ProductModel> _products;
        public BindingList<ProductModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductModel _selectedProduct;
        public ProductModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
            }
        }
        private BindingList<CartItemModel> _cart = new BindingList<CartItemModel>();
        public BindingList<CartItemModel> Cart
        {
            get => _cart;
            set
            {
                _cart = value;
                NotifyOfPropertyChange(() => Cart);
            }
        }
        private int _itemQtity = 1;
        public int ItemQuantity
        {
            get { return _itemQtity; }
            set
            {
                _itemQtity = value;
                NotifyOfPropertyChange(() => ItemQuantity);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        public string SubTotal
        {
            get
            {
                decimal subTotal = 0;
                foreach (var item in Cart)
                {
                    subTotal += item.Product.RetailPrice * item.QuantityInCart;
                }

                return subTotal.ToString("C");
            }
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
                if (ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity)
                {
                    output = true;
                }
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
            CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if (existingItem != null)
            {
                existingItem.QuantityInCart += ItemQuantity;

                //Костыль.Найти лучшее решение обновления корзины.
                Cart.Remove(existingItem);
                Cart.Add(existingItem);
            }
            else
            {
                CartItemModel item = new CartItemModel()
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                _cart.Add(item);
            }
            SelectedProduct.QuantityInStock -= ItemQuantity;
            ItemQuantity = 1;
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => existingItem.DisplayInfo);
        }

        public void RemoveFromCart()
        {
            NotifyOfPropertyChange(() => SubTotal);
        }
    }
}
