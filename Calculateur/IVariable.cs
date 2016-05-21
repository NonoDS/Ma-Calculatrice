using System;
using System.Collections.Generic;
using System.Text;

namespace Calculateur
{
    /// <summary>
    /// Description d'une variable
    /// </summary>
    public interface IVariable
    {
        /// <summary>
        /// Nom de la variable
        /// </summary>
        string Nom
        {
            get;
        }
    
        /// <summary>
        /// Retourne la valeur associé à la variable
        /// </summary>
        /// <returns></returns>
        double ObtenirValeur();

        event EventHandler<EventArgs> OnValeurChange;

        /// <summary>
        /// Modifie la valeur associé à la variable
        /// </summary>
        /// <param name="valeur"></param>
        void ModifierValeur(double valeur);
    }
}
