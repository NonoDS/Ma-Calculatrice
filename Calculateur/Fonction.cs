using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Calculateur
{
    /// <summary>
    /// Description d'une fonction
    /// </summary>
    public partial class Fonction : IElementCalcul
    {
        #region Constructeur
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="nomFonction">Nom de la fonction</param>
        /// <param name="parametres">Liste des paramètres de la fonction</param>
        public Fonction(string nomFonction, ICollection<IElementCalcul> parametres)
        {
            _nomFonction = nomFonction;
            _listeElements = parametres;
            _nBParametres = parametres.Count;
        }

        /// <summary>
        /// Factory de fonction
        /// </summary>
        /// <param name="nomFonction">Nom de la fonction trigonométrique</param>
        /// <param name="parametres">Liste des parmètres de la fonction</param>
        /// <returns></returns>
        public static IElementCalcul Factory(string nomFonction, ICollection<IElementCalcul> parametres)
        { 
            IElementCalcul elt;
            if ((elt = FonctionTrigo.ObtenirFonctionTrigo(nomFonction, parametres)) != null)
            {
                return elt;
            }
            return new Fonction(nomFonction, parametres);
        }

        #endregion

        #region Attributs privés
        private string _expression;
        private int _nBParametres;
        private string _nomFonction;
        private IElementCalcul _elementRacine;          //La définition de la fonction
        private ICollection<IElementCalcul> _listeElements;    //La liste des paramètres
        #endregion

        public override string ToString()
        {
            return _expression;
        }


        /// <summary>
        /// Nombre de paramètre de la fonction
        /// </summary>
        public int NombreParametres
        {
            get { return _nBParametres; }
        }

        /// <summary>
        /// Expression associée à la fonction
        /// </summary>
        public string Expression
        {
            get
            {
                return _expression;
            }
            set
            {
                _expression = value;
            }
        }

        /// <summary>
        /// Liste des paramètres de la fonction
        /// </summary>
        public ICollection<IElementCalcul> ListeParametres
        {
            get
            {
                return _listeElements;
           }
        }

        /// <summary>
        /// Nom de la fonction
        /// </summary>
        public string Nom
        {
            get
            {
                return _nomFonction;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public IElementCalcul ElementRacine
        {
            get 
            {
                return _elementRacine;
            }
            set
            {
                _elementRacine = value;
            }
        }

        /// <summary>
        /// Accept un visiteur
        /// </summary>
        /// <param name="visitor"></param>
        public void Accept(IVisiteurIElement visitor)
        {
           visitor.Visit(this);
        }
    }
}
