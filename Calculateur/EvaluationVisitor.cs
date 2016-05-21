using System;
using System.Collections.Generic;
using System.Text;

namespace Calculateur
{
    /// <summary>
    /// Le visiteur permettant de calculer une composition d'IElementCalcul
    /// </summary>
    public class EvaluationVisitor : IVisiteurIElement
    {
        #region Attribus Privés
        private IVariables _contexFonction;
        private IVariables _contexVariables;
        private ListeFonction _listeFonc;
        private double _valeurCourante;
        private TypeAngle _typeAngle;
        #endregion Attribus Privés

        /// <summary>
        /// Le constructeur
        /// </summary>
        /// <param name="contexVariables">Context des variables</param>
        /// <param name="listeFonc">Liste des fonctions</param>
        /// <param name="angle">Type d'angle pour les calcule trigonométriques</param>
        public EvaluationVisitor(IVariables contexVariables, ListeFonction listeFonc, TypeAngle angle)
        {
            _contexFonction = null;
            _contexVariables = contexVariables;
            _listeFonc = listeFonc;
            _typeAngle = angle;
        }

        /// <summary>
        /// Retourne le résultat de l'évaluation
        /// </summary>
        /// <param name="elt">L'élément à evaluer</param>
        /// <returns></returns>
        public double Evaluer(IElementCalcul elt)
        {
            elt.Accept(this);
            return _valeurCourante;
        }

        #region IVisiteurIElement Membres

        /// <summary>
        /// Visit d'un ElementCalculAffectVar
        /// </summary>
        /// <param name="elt"></param>
        void IVisiteurIElement.Visit(ElementCalculAffectVar elt)
        {
            elt.Valeur.Accept(this);
            _contexVariables.Ajouter(elt.Variable.NomVariable, _valeurCourante);
        }

        /// <summary>
        /// Visit d'un ElementCalculAffFonc
        /// </summary>
        /// <param name="elt"></param>
        void IVisiteurIElement.Visit(ElementCalculAffFonc elt)
        {
            elt.Fonction.ElementRacine = elt.ElementRacine;
            _listeFonc.Ajouter(elt.Fonction);
            _valeurCourante = 0;
        }

        /// <summary>
        /// Visit d'un ElementCalculFBinaire
        /// </summary>
        /// <param name="elt"></param>
        void IVisiteurIElement.Visit(ElementCalculFBinaire elt)
        {
            double valeurgauche, valeurDroit;
            elt.ElementGauche.Accept(this);
            valeurgauche = _valeurCourante;
            elt.ElementDroit.Accept(this);
            valeurDroit = _valeurCourante;
            _valeurCourante = elt.Fonction(valeurgauche, valeurDroit);
        }

        /// <summary>
        /// Visit d'un ElementCalculFUnaire
        /// </summary>
        /// <param name="elt"></param>
        void IVisiteurIElement.Visit(ElementCalculFUnaire elt)
        {
            double valeurElement;
            elt.Element.Accept(this);
            valeurElement = _valeurCourante;
            _valeurCourante = elt.Fonction(valeurElement);
        }

        /// <summary>
        /// Visit d'un ElementCalculValeur
        /// </summary>
        /// <param name="elt"></param>
        void IVisiteurIElement.Visit(ElementCalculValeur elt)
        {
            _valeurCourante = elt.Valeur;
        }

        /// <summary>
        /// Visit d'un ElementCalculVariable
        /// </summary>
        /// <param name="elt"></param>
        void IVisiteurIElement.Visit(ElementCalculVariable elt)
        {
            if (_contexFonction != null && _contexFonction.ContientVariable(elt.NomVariable))
            {
                _valeurCourante = _contexFonction.ObtenirValeur(elt.NomVariable);
            }
            else if (_contexVariables.ContientVariable(elt.NomVariable))
            {
                _valeurCourante = _contexVariables.ObtenirValeur(elt.NomVariable);
            }
            else
            {
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.CurrentUICulture, Calculateur.ResourceManager.GetString("VariableIndefini"), elt.NomVariable));
            }
        }

        /// <summary>
        /// Visit d'un FonctionTrigo
        /// </summary>
        /// <param name="elt"></param>
        void IVisiteurIElement.Visit(FonctionTrigo elt)
        {
            double valeurParametre;
            elt.Parametre.Accept(this);
            valeurParametre = _valeurCourante;
            switch (elt.Fonction)
            {
                case FonctionTrigo.FCTrigo.sin:
                    _valeurCourante = Math.Sin(ConvertToRadians(valeurParametre));
                    break;
                case FonctionTrigo.FCTrigo.cos:
                    _valeurCourante = Math.Cos(ConvertToRadians(valeurParametre));
                    break;
                case FonctionTrigo.FCTrigo.acos:
                    _valeurCourante = ConvertToCurrentAngle(Math.Acos(valeurParametre));
                    break;
                case FonctionTrigo.FCTrigo.asin:
                    _valeurCourante = ConvertToCurrentAngle(Math.Asin(valeurParametre));
                    break;
                case FonctionTrigo.FCTrigo.tan:
                    _valeurCourante = Math.Tan(ConvertToRadians(valeurParametre));
                    break;
                case FonctionTrigo.FCTrigo.atan:
                    _valeurCourante = ConvertToCurrentAngle(Math.Atan(valeurParametre));
                    break;
                case FonctionTrigo.FCTrigo.sinh:
                    _valeurCourante = Math.Sinh(ConvertToRadians(valeurParametre));
                    break;
                case FonctionTrigo.FCTrigo.cosh:
                    _valeurCourante = Math.Cosh(ConvertToRadians(valeurParametre));
                    break;
                case FonctionTrigo.FCTrigo.tanh:
                    _valeurCourante = Math.Tanh(ConvertToRadians(valeurParametre));
                    break;
                default:
                    _valeurCourante = 0; // Pas possible
                    throw new ArgumentException(string.Format(System.Globalization.CultureInfo.InvariantCulture, Calculateur.ResourceManager.GetString("FonctionIndefini"), elt.ToString()));
            }
        }

        /// <summary>
        /// Visit d'un Fonction
        /// </summary>
        /// <param name="elt"></param>
        void IVisiteurIElement.Visit(Fonction elt)
        {
            IVariables ancienContext;
            IVariables nouveauContext;
            int i = 0;

            if (elt.ElementRacine == null)
            {
                nouveauContext = new Variables();
                foreach (IElementCalcul element in elt.ListeParametres)
                {
                    element.Accept(this);
                    nouveauContext.Ajouter(i.ToString(System.Globalization.CultureInfo.InvariantCulture), _valeurCourante);
                    i++;
                }
                //Recherche de la fonction dans le contexte de calcul
                Fonction fonction = _listeFonc.ObtenirFonction(elt.Nom);
                if (fonction == null)
                {
                    throw new ArgumentOutOfRangeException(string.Format(System.Globalization.CultureInfo.InvariantCulture, Calculateur.ResourceManager.GetString("FonctionIndefini"), elt.Nom));
                }
                if (elt.NombreParametres != fonction.NombreParametres)
                {
                    throw new ArgumentException(string.Format(System.Globalization.CultureInfo.InvariantCulture, Calculateur.ResourceManager.GetString("ParametreIndefini"), elt.Nom, fonction.NombreParametres));
                }
                ancienContext = _contexFonction;
                _contexFonction = nouveauContext;
                fonction.ElementRacine.Accept(this);
                _contexFonction = ancienContext;
            }
            else
            {
                elt.ElementRacine.Accept(this);
            }
        }

        /// <summary>
        /// Type d'angle pour les calculs trigonométriques
        /// </summary>
        public TypeAngle Angle
        {
            get
            {
                return _typeAngle;
            }
            set
            { 
                _typeAngle = value;
            }
        }

        #endregion
        /// <summary>
        /// Conversion de l'angle dans le type courant en radians
        /// </summary>
        /// <param name="angleRadian"></param>
        /// <returns></returns>
        private double ConvertToRadians(double angleTypeCourant)
        {
            switch (_typeAngle)
            {
                case TypeAngle.Degres:
                    return (angleTypeCourant / 180) * Math.PI;
                case TypeAngle.Grades:
                    return (angleTypeCourant / 200) * Math.PI;
                case TypeAngle.Radians:
                    return angleTypeCourant;
            }
            return 0.0;
        }

        /// <summary>
        /// Conversion de l'angle donné en paramètre dans l'angle courant
        /// </summary>
        /// <param name="angleRadian"></param>
        /// <returns></returns>
        private double ConvertToCurrentAngle(double angleRadian)
        {
            switch (_typeAngle)
            {
                case TypeAngle.Degres:
                    return (angleRadian / Math.PI) * 180;
                case TypeAngle.Grades:
                    return (angleRadian / Math.PI) * 200;
                case TypeAngle.Radians:
                    return angleRadian;
            }
            return 0.0;
        }
    }
}
