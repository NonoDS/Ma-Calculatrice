using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Calculateur
{
    public delegate double FonctionUnaire(double valeur1);

    /// <summary>
    /// Fonction Unaire
    /// </summary>
    public class ElementCalculFUnaire : IElementCalcul
    {
        #region Attributs privés
        private FonctionUnaire _fonction;
        private IElementCalcul _element;
        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="fonction">La fonction</param>
        /// <param name="element">L'element auquel appliquer la fonction</param>
        public ElementCalculFUnaire(FonctionUnaire fonction, IElementCalcul element)
        {
            _fonction = fonction;
            _element = element;
        }

        /// <summary>
        /// La fonction
        /// </summary>
        public FonctionUnaire Fonction
        {
            get { return _fonction; }
        }

        /// <summary>
        /// L'élémentà appliquer la fonction
        /// </summary>
        public IElementCalcul Element
        {
            get { return _element; }
        }

        /// <summary>
        /// Une fonction unaire n'a qu'un paramètre
        /// </summary>
        public int NombreParametres
        {
            get { return 1; }
        }

        /// <summary>
        /// Le paramètre
        /// </summary>
        public ICollection<IElementCalcul> ListeParametres
        {
            get
            {
                List<IElementCalcul> elt = new List<IElementCalcul>(1);
                elt.Add(_element);
                return elt;
            }
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
