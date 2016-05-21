
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calculateur;

namespace WpfCalc
{
    public class CalculateurModel
    {
        private ListeFonction _listeFonction;   // Liste des fonctions
        private Variables _variables;           // Liste des variables
        private AnalyseurMath _analyseur;       // L'analyseur lexical et syntaxique
        private EvaluationVisitor _evaluateur;

        public CalculateurModel()
        {
            _analyseur = new AnalyseurMath();
            _listeFonction = new ListeFonction();
            _variables = new Variables();
            _variables.Ajouter("PI", Math.PI);
            _variables.Ajouter("E", Math.E);
            _evaluateur = new EvaluationVisitor(_variables, _listeFonction, TypeAngle.Degres);
        }

        /// <summary>
        /// Retour le résultat du calcul et la décomposition syntaxique
        /// </summary>
        /// <param name="expression">Expression à calculer</param>
        /// <param name="elt">Arbre syntaxique déduit de l'expression</param>
        /// <returns>L'évaluation de l'arbre syntaxique</returns>
        public ResultatCalcul CalculerExpression(string expression)
        {
            IElementCalcul elt;
            double resultat;
            try
            {
                elt = _analyseur.CreationArbre(expression);
            }
            catch (FormatException e1)
            {
                return new ResultatCalcul(0f, e1, null);
            }
            try
            {
                resultat = _evaluateur.Evaluer(elt);
            }
            catch (Exception e2)
            {
                return new ResultatCalcul(0f, e2, null) ;
            }
            return new ResultatCalcul(resultat, null, elt);
        }

        /// <summary>
        ///  Retourne la liste des variables
        /// </summary>
        public IVariables LesVariables
        {
            get
            {
                return _variables;
            }
        }

        /// <summary>
        /// Retourne la liste des fonctions
        /// </summary>
        public ListeFonction ListeFonctions
        {
            get
            {
                return _listeFonction;
            }
        }

        /// <summary>
        /// Type d'angle pour effectuer l'évaluation
        /// </summary>
        public TypeAngle Angle
        {
            get { return _evaluateur.Angle; }
            set { _evaluateur.Angle = value;}
        }

        /// <summary>
        /// Evènement appelé sur l'ajout d'une fonction
        /// </summary>
        public event EventHandler<FonctionEventArgs> OnAjoutFonction
        {
            add
            {
                _listeFonction.OnAjoutFonction += value;
            }
            remove
            {
                _listeFonction.OnAjoutFonction -= value;
            }
        }

        /// <summary>
        /// Evènenement appelé lors de la suppression d'une fonction
        /// </summary>
        public event EventHandler<FonctionEventArgs> OnSuppFonction
        {
            add
            {
                _listeFonction.OnSuppFonction += value;
            }
            remove
            {
                _listeFonction.OnSuppFonction -= value;
            }
        }

        /// <summary>
        /// Evènement appelé lors de l'ajout d'une variable
        /// </summary>
        public event EventHandler<VariableEventArgs> OnAjoutVariable
        {
            add
            {
                _variables.OnAjoutVariable += value;
            }
            remove
            {
                _variables.OnAjoutVariable -= value;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class ResultatCalcul
    {
        private double _resultat;
        private Exception _erreur;
        private IElementCalcul _arbre;

        public ResultatCalcul(double resultat, Exception erreur, IElementCalcul arbre)
        {
            _resultat = resultat;
            _erreur = erreur;
            _arbre = arbre;
        }

        public double Resultat
        {
            get
            {
                return _resultat;
            }
        }

        public Exception Erreur
        {
            get
            {
                return _erreur;
            }
        }

        public IElementCalcul Arbre
        {
            get
            {
                return _arbre;
            }
        }
    }
}
