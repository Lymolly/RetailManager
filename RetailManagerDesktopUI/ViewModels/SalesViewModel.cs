using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using RetailManagerDesktopUI.Library.Api;
using RetailManagerDesktopUI.Library.Helpers;
using RetailManagerDesktopUI.Library.Models;

namespace RetailManagerDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private IProductEndpoint _productEndpoint;
        private ISaleEndpoint _saleEndpoint;
        IConfigHelper cfgHelper;
        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper, ISaleEndpoint saleEndpoint)
        {
            _productEndpoint = productEndpoint;
            _saleEndpoint = saleEndpoint;
            cfgHelper = configHelper;
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
                decimal subTotal = CalculateSubTotal();
                return subTotal.ToString("C");
            }
        }

        private decimal CalculateSubTotal()
        {
            decimal subTotal = 0;
            foreach (var item in Cart)
            {
                subTotal += item.Product.RetailPrice * item.QuantityInCart;
            }

            return subTotal;
        }
        public string Total
        {
            get
            {
                var result = CalculateSubTotal() + CalculateTax();
                return result.ToString("C");
            }
        }
        public string Tax
        {
            get
            {
                decimal taxAmount = CalculateTax();
                return taxAmount.ToString("C");
            }
        }

        private decimal CalculateTax()
        {
            decimal taxAmount = 0;
            decimal taxRate = cfgHelper.GetTaxRate() / 100;

            taxAmount = Cart
                .Where(i => i.Product.IsTaxable)
                .Sum(x => x.Product.RetailPrice * x.QuantityInCart * taxRate);

            //foreach (var item in Cart)
            //{
            //    if (item.Product.IsTaxable)
            //    {
            //        taxAmount += (item.Product.RetailPrice * item.QuantityInCart * taxRate);
            //    }
            //}
            return taxAmount;
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
                if (Cart.Count > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        public async Task CheckOut()
        {
            //create sale model and post to api
            SaleModel saleModel = new SaleModel();
            foreach (var item in Cart)
            {
                saleModel.SaleDetails.Add(new SaleDetailModel
                {
                    ProductId = item.Product.Id,
                    Quantity = item.QuantityInCart
                });
            }

            await _saleEndpoint.PostSale(saleModel);
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
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        public void RemoveFromCart()
        {
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }
    }
}
