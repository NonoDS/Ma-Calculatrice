using System;
using System.Collections.Generic;
using System.Text;

namespace Calculateur
{
    public class FonctionTrigo : IElementCalcul
    {
        #region Champs privés
        private FCTrigo _fcontionTrigo;
        private IElementCalcul _parametre;
        private static Dictionary<string, FCTrigo> _listeFonction = new Dictionary<string, FCTrigo>();
        #endregion

        public enum FCTrigo
        {
            /// <summary>
            /// Sinus
            /// </summary>
            sin,
            /// <summary>
            /// ArcCos
            /// </summary>
            asin,
            /// <summary>
            /// Cosinus
            /// </summary>
            cos,
            /// <summary>
            /// ArcCos
            /// </summary>
            acos,
            /// <summary>
            /// Tangante
            /// </summary>
            tan,
            /// <summary>
            /// ArcTangante
            /// </summary>
            atan,
            /// <summary>
            /// Sinus hyperbolique
            /// </summary>
            sinh,
            /// <summary>
            /// Cosinus hyperbolique
            /// </summary>
            cosh,
            /// <summary>
            /// Tangente hyperbolique
            /// </summary>
            tanh,
        }

        
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="fcTrigo"></param>
        /// <param name="parametre"></param>
        public FonctionTrigo(FCTrigo fcTrigo, IElementCalcul parametre)
        {
            _fcontionTrigo = fcTrigo;
            _parametre = parametre;
        }
        
        /// <summary>
        /// Nombre de paramètres de la fonction
        /// </summary>
        public int NombreParametres
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Retourne la fonction trigo si le nom de la fonction est une fonction trigo
        /// </summary>
        /// <param name="nomFonction">Nom de la fonction</param>
        /// <param name="parametres">Liste des paramètres</param>
        /// <returns></returns>
        public static FonctionTrigo ObtenirFonctionTrigo(string nomFonction, ICollection<IElementCalcul> parametres)
        {
            if(_listeFonction.ContainsKey(nomFonction))
            {
                if (parametres.Count != 1)
                    throw new ArgumentException(string.Format("La fonction {0} n'accepte qu'un seul paramètre."));
                IEnumerator<IElementCalcul> enume = parametres.GetEnumerator();
                enume.MoveNext();
                return new FonctionTrigo(_listeFonction[nomFonction], enume.Current);
            }
            return null;
        }

        /// <summary>
        /// Constructeur static
        /// </summary>
        static FonctionTrigo()
        {
            foreach (int i in Enum.GetValues(typeof(FCTrigo)))
            {
                _listeFonction.Add(((FCTrigo)i).ToString(), (FCTrigo)i);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public ICollection<IElementCalcul> ListeParametres
        {
            get
            {
                List<IElementCalcul> list =  new List<IElementCalcul>();
                list.Add(_parametre);
                return list;
            }
        }

        public IElementCalcul Parametre
        {
            get { return _parametre; }
        }

        public FCTrigo Fonction
        {
            get { return _fcontionTrigo; }
        }

     

        #region IElementCalcul Membres


        public void Accept(IVisiteurIElement visitor)
        {
            visitor.Visit(this);
        }

        #endregion
    }
    /// <summary>
    /// Les différents type d'angle
    /// </summary>
    public enum TypeAngle
    {
        Degres,
        Radians,
        Grades
    }
}
