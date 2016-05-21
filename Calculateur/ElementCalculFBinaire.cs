using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Calculateur
{
    public delegate double FonctionBinaire(double valeur1, double valeur2);

    /// <summary>
    /// fonction avec deux paramètre (+, -, *, /)
    /// </summary>
    public class ElementCalculFBinaire : IElementCalcul
    {
        #region Attributs privés
        private IElementCalcul _elementGauche;
        private IElementCalcul _elementDroit;
        private FonctionBinaire _fonction;
        #endregion

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="elementGauche">Premier paramètre de la fonction</param>
        /// <param name="fonction">La fonction</param>
        /// <param name="elementDroit">Deuxième paramètre de la fonction</param>
        public ElementCalculFBinaire(IElementCalcul elementGauche, FonctionBinaire fonction, IElementCalcul elementDroit)
        {
            _elementDroit = elementDroit;
            _elementGauche = elementGauche;
            _fonction = fonction;
        }
       
        /// <summary>
        /// Element gauche de la fonction
        /// </summary>
        public IElementCalcul ElementGauche
        { 
            get
            {
                return _elementGauche;
            }
        }

        /// <summary>
        /// Element droit de la fonction
        /// </summary>
        public IElementCalcul ElementDroit
        {
            get { return _elementDroit; }
        }

        /// <summary>
        /// La fonction
        /// </summary>
        public FonctionBinaire Fonction
        {
            get { return _fonction; }
        }

        /// <summary>
        /// 2 paramètres
        /// </summary>
        public int NombreParametres
        {
            get { return 2; }
        }

        /// <summary>
        /// L'élément froit et l'élément gauche
        /// </summary>
        public ICollection<IElementCalcul> ListeParametres
        {
            get
            {
                List<IElementCalcul> elt = new List<IElementCalcul>(2);
                elt.Add(_elementGauche);
                elt.Add(_elementDroit);
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
