using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace KioscoWPF.ViewModels
{
    class detallesProductoViewModel : Base.ViewModelBase
    {
        #region Initialize
        public void setInitialize(Window sentWindow, productosModel sentProducto)
        {
            base.setInitialize(sentWindow);

            _selectedProducto = sentProducto;
            OnPropertyChanged(nameof(selectedProducto));
        }
        #endregion // Initialize



        #region Variables
        productosModel _selectedProducto;
        public productosModel selectedProducto => _selectedProducto;
        #endregion // Variables
    }
}
