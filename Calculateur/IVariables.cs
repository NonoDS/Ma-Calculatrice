using System;
using System.Collections.Generic;
using System.Text;

namespace Calculateur
{
    public interface IVariables : IEnumerable<IVariable>
    {
        /// <summary>
        /// Ajoute une variable dans le gestionnaire de variables
        /// </summary>
        void Ajouter(string nomVariable, double valeur);
        
        /// <summary>
        /// Retourne la valeur d'une variable en fonction de son nom
        /// </summary>
        double ObtenirValeur(string nomVariable);

        /// <summary>
        /// Indique si la variable existe
        /// </summary>
        /// <param name="nomVariable"></param>
        /// <returns></returns>
        bool ContientVariable(string nomVariable);

        event EventHandler<VariableEventArgs> OnAjoutVariable;
    }
}
    