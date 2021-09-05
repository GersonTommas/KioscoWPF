using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace KioscoWPF.Base
{
    public class ViewModelBase : PropertyChangedBase
    {
        #region Windows
        public Views.helperSelectorView gHelperSelectorView;
        public Views.helperCantidadView vHelperCantidad;
        public Views.helperPrecioView vHelperPrecio;
        public Views.helperIngresoPreciosView gHelperIngresos;

        public VentanaPrincipal vPrincipal;
        public VentanaLogIn vLogIn;


        public bool gOpenAddConsumo() { Views.addConsumoView vTemp = new Views.addConsumoView(); return vTemp.ShowDialog().Value; }

        public bool gOpenAddConteoCaja() { Views.addConteoCajaView vTemp = new Views.addConteoCajaView(); return vTemp.ShowDialog().Value; }
        public bool gOpenAddConteoCaja(bool sentIsSalida) { Views.addConteoCajaView vTemp = new Views.addConteoCajaView(sentIsSalida); return vTemp.ShowDialog().Value; }

        public bool gOpenAddConversion(productosModel sentProducto) { Views.addConversionView vTemp = new Views.addConversionView(sentProducto); return vTemp.ShowDialog().Value; }

        public bool gOpenAddDeudor() { Views.addDeudorView vTemp = new Views.addDeudorView(); return vTemp.ShowDialog().Value; }
        public bool gOpenEditDeudor(deudoresModel sentDeudor) { Views.addDeudorView vTemp = new Views.addDeudorView(sentDeudor); return vTemp.ShowDialog().Value; }

        public bool gOpenAddIngreso() { Views.addIngresoView vTemp = new Views.addIngresoView(); return vTemp.ShowDialog().Value; }

        public bool gOpenAddMedida() { Views.addMedidaView vTemp = new Views.addMedidaView(); return vTemp.ShowDialog().Value; }
        public bool gOpenEditMedida(medidasModel sentMedida) { Views.addMedidaView vTemp = new Views.addMedidaView(sentMedida); return vTemp.ShowDialog().Value; }

        public bool gOpenAddProducto() { Views.addProductoView vTemp = new Views.addProductoView(); return vTemp.ShowDialog().Value; }
        public bool gOpenAddProducto(string sentCodigo) { Views.addProductoView vTemp = new Views.addProductoView(sentCodigo); return vTemp.ShowDialog().Value; }
        public bool gOpenEditProducto(productosModel sentProducto) { Views.addProductoView vTemp = new Views.addProductoView(sentProducto); return vTemp.ShowDialog().Value; }

        public bool gOpenAddProveedor() { Views.addProveedorView vTemp = new Views.addProveedorView(); return vTemp.ShowDialog().Value; }
        public bool gOpenEditProveedor(proveedoresModel sentProveedor) { Views.addProveedorView vTemp = new Views.addProveedorView(sentProveedor); return vTemp.ShowDialog().Value; }

        public bool gOpenAddRetiroCaja() { Views.addRetiroCajaView vTemp = new Views.addRetiroCajaView(); return vTemp.ShowDialog().Value; }

        public bool gOpenAddSacado(usuariosModel sentUsuario) { Views.addSacadoView vTemp = new Views.addSacadoView(sentUsuario); return vTemp.ShowDialog().Value; }

        public bool gOpenAddTag() { Views.addTagView vTemp = new Views.addTagView(); return vTemp.ShowDialog().Value; }
        public bool gOpenEditTag(tagsModel sentTag) { Views.addTagView vTemp = new Views.addTagView(sentTag); return vTemp.ShowDialog().Value; }

        public bool gOpenAddUsuario() { Views.addUsuarioView vTemp = new Views.addUsuarioView(); return vTemp.ShowDialog().Value; }
        public bool gOpenEditUsuario(usuariosModel sentUsuario) { Views.addUsuarioView vTemp = new Views.addUsuarioView(sentUsuario); return vTemp.ShowDialog().Value; }

        public bool gOpenAddVenta() { Views.addVentaView vTemp = new Views.addVentaView(); return vTemp.ShowDialog().Value; }


        public bool gOpenDetallesProducto(productosModel sentProducto) { Views.detallesProductoView vTemp = new Views.detallesProductoView(sentProducto); return vTemp.ShowDialog().Value; }


        public bool gOpenModificarStock(productosModel sentProducto) { Views.helperModificarStockView vTemp = new Views.helperModificarStockView(sentProducto); return vTemp.ShowDialog().Value; }
        #endregion // Windows

        #region Initialize
        public Window thisWindow;

        public virtual void setInitialize(Window sentWindow)
        {
            thisWindow = sentWindow;
        }
        #endregion //Initialize


        public virtual void aCommandAgregarQuitarProducto(object sentParameter = null) { }
        public virtual void aCommandUnSoloProducto(object sentParameter = null) { }
        public bool bolIsOnlyOne { get; set; }

        public Command bComUnSoloProducto => new Command((object parameter) => aCommandUnSoloProducto(parameter), (object parameter) => bolIsOnlyOne);

        public Command bComSelectorAgregarQuitar => new Command((object parameter) => aCommandAgregarQuitarProducto(parameter));

        public virtual Command bComPrevControl => new Command((object parameter) => { Variables.nextTarget(parameter, true); });
        public virtual Command bComNextControl => new Command((object parameter) => { Variables.nextTarget(parameter); });
        public virtual Command bComCancelar => new Command((object parameter) => { thisWindow.Close(); });
    }
}
