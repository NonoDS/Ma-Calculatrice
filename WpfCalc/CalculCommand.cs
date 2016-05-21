using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace WpfCalc
{
    public class CalculCommand : ICommand
    {
        private Action _executeAction;

        public CalculCommand(Action executeAction)
        {
            _executeAction = executeAction;
        }
        // Résumé :
        //     Se produit lorsque des modifications influent sur l'exécution de la commande.
        event EventHandler CanExecuteChanged
        {
            add {}
            remove{}
        }

        // Résumé :
        //     Définit la méthode qui détermine si la commande peut s'exécuter dans son
        //     état actuel.
        //
        // Paramètres :
        //   parameter:
        //     Données utilisées par la commande. Si la commande ne requiert pas que les
        //     données soient passées, cet objet peut avoir la valeur null.
        //
        // Retourne :
        //     true si cette commande peut être exécutée ; sinon false.
        bool ICommand.CanExecute(object parameter)
        {
            return true;
        }
        //
        // Résumé :
        //     Définit la méthode à appeler lorsque la commande est appelée.
        //
        // Paramètres :
        //   parameter:
        //     Données utilisées par la commande. Si la commande ne requiert pas que les
        //     données soient passées, cet objet peut avoir la valeur null.
       public void Execute(object parameter)
        {
            _executeAction();
        }

        #region ICommand Membres

        event EventHandler ICommand.CanExecuteChanged
        {
            add { }
            remove { }
        }

        //void ICommand.Execute(object parameter)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }
}
