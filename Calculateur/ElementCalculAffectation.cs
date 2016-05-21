using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Calculateur
{
    public class ElementCalculAffectVar : IElementCalcul
    {
        #region Attributs privés
        ElementCalculVariable _variable;
        IElementCalcul _valeur;
        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        public ElementCalculAffectVar(ElementCalculVariable variable, IElementCalcul valeur)
        {
            if (variable == null)
                throw new ArgumentException(Calculateur.ResourceManager.GetString("variableNull"));
            _variable = variable;
            _valeur = valeur;
        }

        /// <summary>
        /// Nombre de paramètre de l'affectation -> 2
        /// </summary>
        public int NombreParametres
        {
            get { return 2; }
        }

        /// <summary>
        /// La partie droite de l'affectation
        /// </summary>
        public IElementCalcul Valeur
        {
            get { return _valeur; }
        }

        /// <summary>
        /// La partie gauche de l'affectation
        /// </summary>
        public ElementCalculVariable Variable
        {
            get
            {
                return _variable;
            }
        }

        /// <summary>
        /// La partie droit et gauche de l'affectation
        /// </summary>
        public ICollection<IElementCalcul> ListeParametres
        {
            get
            {
                List<IElementCalcul> elt = new List<IElementCalcul>(2);
                elt.Add(_variable);
                elt.Add(_valeur);
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
