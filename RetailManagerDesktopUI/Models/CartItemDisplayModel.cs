using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RetailManagerDesktopUI.Annotations;

namespace RetailManagerDesktopUI.Models
{
    public class CartItemDisplayModel : INotifyPropertyChanged
    {
        public ProductDisplayModel Product { get; set; }
        private int qtityInCart;

        public int QuantityInCart
        {
            get { return qtityInCart; }
            set
            {
                qtityInCart = value;
                OnPropertyChanged(nameof(QuantityInCart));
                OnPropertyChanged(nameof(DisplayInfo));
            }
        }

        public string DisplayInfo
        {
            get => $"{Product.ProductName}({QuantityInCart})";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
