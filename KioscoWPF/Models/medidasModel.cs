using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace KioscoWPF
{
    public class medidasModel : Base.PropertyChangedBase
    {
        #region Private
        int _Tipo; string _Medida; bool _Activo;
        #endregion // Private

        #region Public
        public int Id { get; set; }

        public int Tipo { get => _Tipo; set { if (SetProperty(ref _Tipo, value)) { OnPropertyChanged(nameof(TipoShort)); OnPropertyChanged(nameof(fullMedida)); } } }

        public string Medida { get => _Medida; set { if (SetProperty(ref _Medida, value)) { OnPropertyChanged(nameof(fullMedida)); } } }

        public bool Activo { get => _Activo; set => SetProperty(ref _Activo, value); }
        #endregion // Public

        #region Navigation
        public virtual ICollection<productosModel> ProductosPerMedida { get; private set; } = new ObservableCollection<productosModel>();
        #endregion // Navigation

        #region NotMapped
        [NotMapped]
        public string TipoShort
        {
            get => Tipo switch
            {
                1 => "cm",
                2 => "cc",
                3 => "g",
                4 => "Kg",
                5 => "L",
                6 => "ml",
                7 => "u",
                8 => "Par",
                9 => "w",
                10 => "Kw",
                11 => "v",
                12 => "Talle",
                13 => "mg",
                14 => "cl",
                _ => "",
            };
            set
            {
                Tipo = value switch
                {
                    "cm" => 1,
                    "cc" => 2,
                    "g" => 3,
                    "Kg" => 4,
                    "L" => 5,
                    "ml" => 6,
                    "u" => 7,
                    "Par" => 8,
                    "w" => 9,
                    "Kw" => 10,
                    "v" => 11,
                    "Talle" => 12,
                    "mg" => 13,
                    "cl" => 14,
                    _ => 0,
                }; OnPropertyChanged();
            }
        }

        [NotMapped]
        public string fullMedida => Medida + TipoShort;
        #endregion // NotMapped
    }
}
