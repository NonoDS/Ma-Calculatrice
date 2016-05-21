using System;
using System.Collections.Generic;
using System.Text;

namespace Calculateur
{
    /// <summary>
    /// Evenement lié à une fonction
    /// </summary>
    public class FonctionEventArgs : EventArgs
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="fonction">La fonction</param>
        public FonctionEventArgs(Fonction fonction)
        {
            _fonction = fonction;
        }

        private Fonction _fonction;

        /// <summary>
        /// La fonction qui a généré l'évènement
        /// </summary>
        public Fonction Fonction
        {
            get { return _fonction; }            
        }
    }

    /// <summary>
    /// Une liste de fonctions
    /// </summary>
    public class ListeFonction : IEnumerable<Fonction>
    {
        private Dictionary<string, Fonction> _listeFonctions;
        
        /// <summary>
        /// Constructeur
        /// </summary>
        public ListeFonction()
        {
            _listeFonctions = new Dictionary<string, Fonction>();
        }

        /// <summary>
        /// Retourne 
        /// </summary>
        /// <param name="nomFonction">Nom de la fonction</param>
        /// <returns></returns>
        public Fonction ObtenirFonction(string nomFonction)
        {
            if (_listeFonctions.ContainsKey(nomFonction))
            {
                return _listeFonctions[nomFonction];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Ajout de la fonction dans la liste de fonctions
        /// </summary>
        /// <param name="nomFonction">Nom de la fonction</param>
        /// <param name="fonction">La fonction</param>
        public void Ajouter(string nomFonction, Fonction fonction)
        {
            if (_listeFonctions.ContainsKey(nomFonction))
            {
                if (OnSuppFonction != null)
                    OnSuppFonction(this, new FonctionEventArgs(_listeFonctions[nomFonction]));
                _listeFonctions[nomFonction] = fonction;
            }
            else
                _listeFonctions.Add(nomFonction, fonction);
            if(OnAjoutFonction != null)
                OnAjoutFonction(this, new FonctionEventArgs(fonction));
        }

        /// <summary>
        /// Ajout d'une nouvelle fonction
        /// </summary>
        /// <param name="fonction">la fonction a ajouter</param>
        public void Ajouter(Fonction fonction)
        {
            if (fonction == null) throw new ArgumentException(Calculateur.ResourceManager.GetString("FonctionNull"));
            Ajouter(fonction.Nom, fonction);
        }

        public IEnumerator<Fonction> GetEnumerator()
        {
            return _listeFonctions.Values.GetEnumerator();
        }

        public event EventHandler<FonctionEventArgs> OnAjoutFonction;
        public event EventHandler<FonctionEventArgs> OnSuppFonction;

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _listeFonctions.GetEnumerator();
        }

        #endregion
    }
}
