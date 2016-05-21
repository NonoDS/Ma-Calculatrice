using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Resources;
using System.Reflection;
using System.Text;

namespace Calculateur
{
    /// <summary>
    /// Affectation d'une fonction � une nouvelle expression
    /// </summary>
    public class ElementCalculAffFonc : IElementCalcul
    {
        #region Attributs priv�s
        private Fonction _fonction;
        private IElementCalcul _elementRacine;

        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="fonction">La fonction a affecter</param>
        /// <param name="elementRacine">D�tail de la fonction</param>
        /// <param name="expression">Expression qui d�crit la fonction</param>
        public ElementCalculAffFonc(Fonction fonction, IElementCalcul elementRacine, string expression)
        {
            _fonction = fonction;
            _fonction.Expression = expression;
            _elementRacine = elementRacine;
        }

        /// <summary>
        /// D�tail de la fonction
        /// </summary>
        public IElementCalcul ElementRacine
        {
            get { return _elementRacine; }
        }

        /// <summary>
        /// Une affectation a deux param�tres
        /// </summary>
        public int NombreParametres
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// La fonction
        /// </summary>
        public Fonction Fonction
        {
            get { return _fonction; }
        }

        /// <summary>
        /// Liste des param�tres
        /// </summary>
        public ICollection<IElementCalcul> ListeParametres
        {
            get
            {
                List<IElementCalcul> elt = new List<IElementCalcul>(2);
                elt.Add(_fonction);
                elt.Add(_elementRacine);
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
