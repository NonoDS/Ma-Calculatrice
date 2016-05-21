using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Calculateur;

namespace CalculatriceFR2
{
    public class CalculateurIhm
    {
        #region Attributs privées
        private ListeFonction _listeFonction;   // Liste des fonctions
        private Variables _variables;           // Liste des variables
        private AnalyseurMath _analyseur;       // L'analyseur lexical et syntaxique
        private EvaluationVisitor _evaluateur;
        #endregion

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public CalculateurIhm()
        {
            _analyseur = new AnalyseurMath();
            _listeFonction = new ListeFonction();
            _variables = new Variables();
            _evaluateur = new EvaluationVisitor(_variables, _listeFonction, TypeAngle.Degres);
        }

        /// <summary>
        /// Retourne le résultat du calcul
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public double CalcluerExpression(string expression)
        {
            IElementCalcul eltCalcul = _analyseur.CreationArbre(expression);
            return _evaluateur.Evaluer(eltCalcul);            
        }

        /// <summary>
        /// Retour le résultat du calcul et la décomposition hiérarchique
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="elt"></param>
        /// <returns></returns>
        public double CalculerExpression(string expression, out IElementCalcul elt)
        {
            elt = _analyseur.CreationArbre(expression);
            return _evaluateur.Evaluer(elt);
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

        public TypeAngle Angle
        {
            get
            {
                return _evaluateur.Angle;
            }
            set
            {
                _evaluateur.Angle = value;
            }
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
}
