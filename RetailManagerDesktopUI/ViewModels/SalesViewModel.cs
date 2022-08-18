using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AutoMapper;
using Caliburn.Micro;
using RetailManagerDesktopUI.Library.Api;
using RetailManagerDesktopUI.Library.Helpers;
using RetailManagerDesktopUI.Library.Models;
using RetailManagerDesktopUI.Models;
using CartItemModel = RetailManagerDesktopUI.Library.Models.CartItemModel;

namespace RetailManagerDesktopUI.ViewModels
{
    public class SalesViewModel : Screen
    {
        private IProductEndpoint _productEndpoint;
        private ISaleEndpoint _saleEndpoint;
        private readonly IMapper _mapper;
        private readonly StatusInfoViewModel _statusInfo;
        private readonly IWindowManager _windowManager;
        IConfigHelper cfgHelper;
        public SalesViewModel(IProductEndpoint productEndpoint, IConfigHelper configHelper,
            ISaleEndpoint saleEndpoint,IMapper mapper,StatusInfoViewModel statusInfo,IWindowManager windowManager
        )
        {
            _productEndpoint = productEndpoint;
            _saleEndpoint = saleEndpoint;
            _mapper = mapper;
            _statusInfo = statusInfo;
            _windowManager = windowManager;
            cfgHelper = configHelper;
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadProducts();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System error";
                if (ex.Message == "Unauthorized")
                {
                    _statusInfo.UpdateMessage("Unauthorized access", "You dont have a permission");
                    await _windowManager.ShowDialogAsync(_statusInfo, null, settings);
                }
                else
                {
                    _statusInfo.UpdateMessage("Fatal error", ex.Message);
                    await _windowManager.ShowDialogAsync(_statusInfo, null, settings);
                }
                await TryCloseAsync();
            }
        }
        private async Task LoadProducts()
        {
            var productList = await _productEndpoint.GetAll();
            var products = _mapper.Map<List<ProductDisplayModel>>(productList);
            Products = new BindingList<ProductDisplayModel>(products);
        }



        private BindingList<ProductDisplayModel> _products;
        public BindingList<ProductDisplayModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        private ProductDisplayModel _selectedProduct;
        public ProductDisplayModel SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanAddToCart);
            }
        }

        private async Task resetSalesViewModel()
        {
            Cart = new BindingList<CartItemDisplayModel>();
            await LoadProducts();
            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
        }

        private CartItemDisplayModel _selectedCartItem;
        public CartItemDisplayModel SelectedCartItem
        {
            get => _selectedCartItem;
            set
            {
                _selectedCartItem = value;
                NotifyOfPropertyChange(() => SelectedProduct);
                NotifyOfPropertyChange(() => CanRemoveFromCart);
            }
        }
        private BindingList<CartItemDisplayModel> _cart = new BindingList<CartItemDisplayModel>();
        public BindingList<CartItemDisplayModel> Cart
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
                if (SelectedCartItem != null && SelectedCartItem?.QuantityInCart > 0)
                {
                    output = true;
                }
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
            await resetSalesViewModel();
        }

        public void AddToCart()
        {
            CartItemDisplayModel existingItemDisplay = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
            if (existingItemDisplay != null)
            {
                existingItemDisplay.QuantityInCart += ItemQuantity;

                //Костыль.Найти лучшее решение обновления корзины.
                //Cart.Remove(existingItemDisplay);
                //Cart.Add(existingItemDisplay);
            }
            else
            {
                CartItemDisplayModel itemDisplay = new CartItemDisplayModel()
                {
                    Product = SelectedProduct,
                    QuantityInCart = ItemQuantity
                };
                _cart.Add(itemDisplay);
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
            SelectedCartItem.Product.QuantityInStock += 1;
            if (SelectedCartItem.QuantityInCart > 1)
            {
                SelectedCartItem.QuantityInCart -= 1;
            }
            else
            {
                Cart.Remove(SelectedCartItem);
            }

            NotifyOfPropertyChange(() => SubTotal);
            NotifyOfPropertyChange(() => Tax);
            NotifyOfPropertyChange(() => Total);
            NotifyOfPropertyChange(() => CanCheckOut);
            NotifyOfPropertyChange(() => CanAddToCart);
        }
    }
}
