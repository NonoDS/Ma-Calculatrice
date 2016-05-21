using System;
using System.Collections.Generic;
using System.Text;

namespace Calculateur
{
    public interface IFonction
    {
        int Expression
        {
            get;
            set;
        }

        double Calculer(IVariables contexVariables);
    }
}
