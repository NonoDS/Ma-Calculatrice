using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Calculateur
{
    /// <summary>
    /// Une valeur
    /// </summary>
    public class ElementCalculValeur : IElementCalcul
    {
        #region Attributs privés
        private double _valeur;
        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="valeur">La valeur</param>
        public ElementCalculValeur(double valeur)
        {
            _valeur = valeur;
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="valeur"></param>
        public ElementCalculValeur(string valeur)
        {
            _valeur = Convert.ToDouble(valeur, System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Aucun paramètre
        /// </summary>
        public int NombreParametres
        {
            get
            {
                return 0;
            }        
        }

        /// <summary>
        /// Aucun paramètre
        /// </summary>
        public ICollection<IElementCalcul> ListeParametres
        {
            get { return null; }
        }

        /// <summary>
        /// La valeur
        /// </summary>
        public double Valeur
        {
            get { return _valeur; }
        }

        /// <summary>
        /// Accept un visiteur
        /// </summary>
        /// <param name="visitor">Le visiteur</param>
        public void Accept(IVisiteurIElement visitor)
        {
            visitor.Visit(this);
        }
    }
}
