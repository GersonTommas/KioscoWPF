using System;
using System.Collections.Generic;
using System.Text;

namespace KioscoWPF.Base
{
    public class ModelBase : PropertyChangedBase
    {
        public int Id { get; set; }

        public virtual void updateModel()
        {

        }
    }
}
