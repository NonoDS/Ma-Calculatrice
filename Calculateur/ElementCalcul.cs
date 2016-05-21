using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Calculateur
{
    public interface IElementCalcul
    {
        /// <summary>
        /// Nombre de paramètres de l'opération
        /// </summary>
        int NombreParametres
        {
            get;    
        }

        /// <summary>
        /// Liste des paramètres de l'opération
        /// </summary>
        ICollection<IElementCalcul> ListeParametres
        {
            get;
        }

        /// <summary>
        /// Accept un visiteur
        /// </summary>
        /// <param name="visitor"></param>
        void Accept(IVisiteurIElement visitor);
    }
}
