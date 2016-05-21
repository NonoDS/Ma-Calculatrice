using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Calculateur;
using System.ComponentModel;

namespace WpfCalc
{
    public delegate void CommandeIVariable(ViewModelVariable v);
    /// <summary>
    /// View model d'une variable
    /// </summary>
    public class ViewModelVariable : ICommand, INotifyPropertyChanged
    {
        #region Attributs privés
        private IVariable _variable;
        private CommandeIVariable _commande;
        #endregion Attributs privés

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="v"></param>
        /// <param name="executeCommande"></param>
        public ViewModelVariable(IVariable v, CommandeIVariable executeCommande)
        {
            _commande = executeCommande;
            _variable = v;
            _variable.OnValeurChange += (object sender, EventArgs e) => NotifyPropertyChanged("Valeur");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }  

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _commande(this);
        }

        /// <summary>
        /// Nom de la variable
        /// </summary>
        public string Nom
        {
            get
            {
                return _variable.Nom;
            }
        }

        /// <summary>
        /// Valeur associé à la variable
        /// </summary>
        public double Valeur
        {
            get
            {
                return _variable.ObtenirValeur();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
