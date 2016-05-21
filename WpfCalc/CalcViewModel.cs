using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Calculateur;
using System.ComponentModel;
using System.Windows;

namespace WpfCalc
{
    /// <summary>
    /// 
    /// </summary>
    public class CalcViewModel : INotifyPropertyChanged
    {
        private CalculateurModel _calculateur;
        private ObservableCollection<ViewModelVariable> _variables;
        private ObservableCollection<Fonction> _fonctions;
        private ObservableCollection<FonctionResultat> _expressions;
        private string _expression;
        private string _messageErreur;
        private ICommand _executeCalcul;
        private bool _erreurSyntaxe;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="calc"></param>
        public CalcViewModel(CalculateurModel calc)
        {
            _calculateur = calc;
            _variables = new ObservableCollection<ViewModelVariable>();
            foreach (IVariable v in _calculateur.LesVariables)
            {
                _variables.Add(new ViewModelVariable(v, new CommandeIVariable(TraiterCommandeIVariable)));
            }
            _calculateur.LesVariables.OnAjoutVariable +=
                new EventHandler<VariableEventArgs>((object o, VariableEventArgs v) => _variables.Add(new ViewModelVariable(v.Variable, new CommandeIVariable(TraiterCommandeIVariable))));
            _fonctions = new ObservableCollection<Fonction>(_calculateur.ListeFonctions);
            _calculateur.ListeFonctions.OnAjoutFonction += new EventHandler<FonctionEventArgs>((object o, FonctionEventArgs f) => _fonctions.Add(f.Fonction));
            _calculateur.ListeFonctions.OnSuppFonction += new EventHandler<FonctionEventArgs>((object o, FonctionEventArgs f) => _fonctions.Remove(f.Fonction));
            _expressions = new ObservableCollection<FonctionResultat>();
            _erreurSyntaxe = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        private void TraiterCommandeIVariable(ViewModelVariable v)
        {
            Expression += v.Nom;
        }

        /// <summary>
        /// La commande à appler pour lancer le calcul de l'expression "La propriété Expression"
        /// </summary>
        public ICommand ExecuteCalcul
        {
            get
            {
                if (_executeCalcul == null)
                    _executeCalcul = new CalculCommand(() => CalculExpression(Expression));
                return _executeCalcul;
            }
        }

        /// <summary>
        /// Indique s'il y a une erreur de syntaxe
        /// </summary>
        public bool ErreurSyntaxe
        {
            get
            {
                return _erreurSyntaxe;
            }
            set 
            {
                if (_erreurSyntaxe != value)
                {
                    _erreurSyntaxe = value;
                    OnPropertyChanged("ErreurSyntaxe");
                }
            }
        }

        /// <summary>
        /// Calcul de l'expression
        /// </summary>
        /// <param name="expression">Expression à évaluer</param>
        private void CalculExpression(string expression)
        {
            ResultatCalcul resultat;
            resultat = _calculateur.CalculerExpression(expression);
            if (resultat.Erreur == null) // Pas d'erreur retourné à l'évaluation
            {
                _expressions.Insert(0, new FonctionResultat { Expression = expression, Resultat = resultat.Resultat });
                ErreurSyntaxe = false;
                Expression = "";
            }
            else
            {
                _messageErreur = resultat.Erreur.Message;
                OnPropertyChanged("MessageErreur");
                ErreurSyntaxe = true;
            }
        }

        /// <summary>
        /// Expression
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
                OnPropertyChanged("Expression");
            }
        }

        /// <summary>
        /// Le message d'erreur
        /// </summary>
        public string MessageErreur
        {
            get
            {
                return _messageErreur;
            }
        }

        /// <summary>
        /// Liste des expression déjà évaluées
        /// </summary>
        public ObservableCollection<FonctionResultat> Expressions
        {
            get
            {
                return _expressions;
            }
        }

        /// <summary>
        /// Liste des fonctions
        /// </summary>
        public ObservableCollection<Fonction> Fonctions
        {
            get
            {
                return _fonctions;
            }
        }

        /// <summary>
        /// Liste des variables
        /// </summary>
        public ObservableCollection<ViewModelVariable> Variables
        {
            get
            {
                return _variables;
            }
        }

        /// <summary>
        /// Type d'angle pour effectuer des calculs trigonométrique
        /// </summary>
        public TypeAngle Angle
        {
            get
            {
                return _calculateur.Angle;
            }
            set
            {
                _calculateur.Angle = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }        
    }

    /// <summary>
    /// Représent une fonction avec le résultat de l'évaluation
    /// </summary>
    public class FonctionResultat
    {
        /// <summary>
        /// Résultat de l'évaluation
        /// </summary>
        public double Resultat { get; set; }
        
        /// <summary>
        /// Expression utilisée pour effectuer l'évaluation
        /// </summary>
        public string Expression { get; set; }
    }
}
