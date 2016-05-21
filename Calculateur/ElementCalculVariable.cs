using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Calculateur
{
    public class ElementCalculVariable : IElementCalcul
    {  
        #region Attributs privés
        private string _nomVariable;
        #endregion
        
        /// <summary>
        /// Création d'une variable
        /// </summary>
        /// <param name="nomVaraible"></param>
        public ElementCalculVariable(string nomVariable)
        {
            _nomVariable = nomVariable;
        }
        
        public int NombreParametres
        {
            get { return 0; }
        }

        /// <summary>
        /// Nom de la variable courante
        /// </summary>
        public string NomVariable
        {
            get
            {
                return _nomVariable;
            }
        }

        /// <summary>
        /// Liste des paramètres
        /// </summary>
        public ICollection<IElementCalcul> ListeParametres
        {
            get { return null; }
        }

        public void Accept(IVisiteurIElement visitor)
        {
            visitor.Visit(this);
        }
    }
}
