using System;
using System.Collections.Generic;
using System.Text;

namespace Calculateur
{
    /// <summary>
    /// Décrit une variable
    /// </summary>
    public class Variable : IVariable
    {
        #region Attributs privés
        private string _nomVariable;
        private double _valeur;
        #endregion

        #region Constructeur
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="nomVariable">Nom de la variable</param>
        /// <param name="valeur">valeur de la variable</param>
        public Variable(string nomVariable, double valeur)
        {
            _nomVariable = nomVariable;
            _valeur = valeur;
        }
        #endregion

        /// <summary>
        /// Retourne la valeur de la variable
        /// </summary>
        /// <returns></returns>
        public double ObtenirValeur()
        {
            return _valeur;
        }

        /// <summary>
        /// Nom de la variable
        /// </summary>
        public string Nom
        {
            get
            {
                return _nomVariable;
            }
        }

        /// <summary>
        /// Modifier la valeur de la variable
        /// </summary>
        /// <param name="valeur">Valeur de la variable</param>
        public void ModifierValeur(double valeur)
        {
            if (_valeur != valeur)
            {
                _valeur = valeur;
                if (OnValeurChange != null)
                {
                    OnValeurChange(this, EventArgs.Empty);
                }
            }
        }

        public event EventHandler<EventArgs> OnValeurChange;

        public override string ToString()
        {
            return _nomVariable + " = " + _valeur.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
