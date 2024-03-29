﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RetailManagerDesktopUI.Annotations;

namespace RetailManagerDesktopUI.Models
{
    public class ProductDisplayModel : INotifyPropertyChanged
    {
        private int qtityInStock;
        public int Id { get; set; }
        public string ProductName { get; set; }
        public decimal RetailPrice { get; set; }
        public string Description { get; set; }
        public int QuantityInStock
        {
            get { return qtityInStock; }
            set
            {
                qtityInStock = value;
                OnPropertyChanged(nameof(QuantityInStock));
            }
        }
        public bool IsTaxable { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
