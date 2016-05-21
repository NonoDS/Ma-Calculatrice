using System;
using System.Collections.Generic;
using System.Text;

namespace Calculateur
{
    /// <summary>
    /// Evènement lié à une variable
    /// </summary>
    public class VariableEventArgs : EventArgs
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="variable">Variable</param>
        public VariableEventArgs(IVariable variable)
        {
            _variable = variable;
        }

        private IVariable _variable;

        public IVariable Variable
        {
            get { return _variable; }
        }
    }

    /// <summary>
    /// Liste de varaibles
    /// </summary>
    public class Variables : IVariables
    {   
        #region Constructeur
        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public Variables()
        {
            _listeVariables = new Dictionary<string, IVariable>();
        }
        #endregion

        #region IVariables Members
        /// <summary>
        /// Ajout d'une variable
        /// </summary>
        /// <param name="valeur"></param>
        public void Ajouter(IVariable valeur)
        {
            if(_listeVariables.ContainsKey(valeur.Nom))
            {
                _listeVariables[valeur.Nom] = valeur;
            }
            else
            {
                _listeVariables.Add(valeur.Nom, valeur);
            }
            if(OnAjoutVariable != null)
                OnAjoutVariable(this, new VariableEventArgs(valeur));
        }

        /// <summary>
        /// Retourne la variable
        /// </summary>
        /// <param name="nomVaraible">Nom de la variable</param>
        /// <returns></returns>
        public IVariable ObtenirVariable(string nomVariable)
        {
            if (_listeVariables.ContainsKey(nomVariable))
            {
                return _listeVariables[nomVariable];
            }
            else
            {
                throw new ArgumentOutOfRangeException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "La varialble {0} n'exite pas.", nomVariable));
            }
        }
        #endregion

        #region Attributs privés
        private Dictionary<string, IVariable> _listeVariables;
        #endregion

        #region IVariables Members

        /// <summary>
        /// Ajout d'une variable
        /// </summary>
        /// <param name="nomVariable">Nom de la variable</param>
        /// <param name="valeur">Valeur de la variable</param>
        public void Ajouter(string nomVariable, double valeur)
        {
            if (_listeVariables.ContainsKey(nomVariable))
            {
                IVariable var = _listeVariables[nomVariable];
                var.ModifierValeur(valeur);
            }
            else
            {
                Ajouter(new Variable(nomVariable, valeur));
            }
        }

        /// <summary>
        /// Obtient la valeur d'une variable
        /// </summary>
        /// <param name="nomVariable">Nom de la variable</param>
        /// <returns></returns>
        public double ObtenirValeur(string nomVariable)
        {
            return _listeVariables[nomVariable].ObtenirValeur();
        }

        /// <summary>
        /// Indique si la variable est connue
        /// </summary>
        /// <param name="nomVariable">Nom de la variable</param>
        /// <returns></returns>
        public bool ContientVariable(string nomVariable)
        {
            return _listeVariables.ContainsKey(nomVariable);
        }

        IEnumerator<IVariable> GetEnumerator()
        {
            return _listeVariables.Values.GetEnumerator();
        }

        public event EventHandler<VariableEventArgs> OnAjoutVariable;
        #endregion

        #region IEnumerable<IVariable> Membres

        IEnumerator<IVariable> IEnumerable<IVariable>.GetEnumerator()
        {
            return _listeVariables.Values.GetEnumerator();
        }
        #endregion

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
