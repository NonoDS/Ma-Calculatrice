using System;
using System.Collections.Generic;
using System.Text;

namespace Calculateur
{
  public class AnalyseurMath
  {
    #region Constructeur
    public AnalyseurMath()
    {

    }

    static AnalyseurMath()
    {

    }

    #endregion

    private Tokeniser _tokensiner;
    private IEnumerator<Token> _enumeratorToken;
    /// <summary>
    /// Cr�ation de l'arbre permettant de calculer l'expression math�matique
    /// </summary>
    /// <returns></returns>
    public IElementCalcul CreationArbre(string expression)
    {
      _paramsFonctionCourante = null;      
      _paramsFonctionCourante = new Dictionary<string, string>(3);
      _tokensiner = new Tokeniser(expression);
      _enumeratorToken = _tokensiner.Tokens.GetEnumerator();
      LitTokenSuivant();
      if (_tokenCourant.TypeToken == ENTypeToken.ENFin) return null;
      return LitAffectationFonction();
    }

    /// <summary>
    /// Expression = somme | somme '=' somme
    /// </summary>
    /// <returns></returns>
    private IElementCalcul LitAffectationFonction()
    {
      IElementCalcul retour;
      int positionDebutFonction = _tokenCourant.PositionDebutToken; // token courant = nom de la fonction
      IElementCalcul premiereSomme = LitSomme();
      if (_tokenCourant.TypeToken == ENTypeToken.ENEgal) //D�finition d'une fonction
      {
        Fonction premierFonction = premiereSomme as Fonction;
        LitTokenSuivant();
        if (premierFonction == null)
        {
          throw new FormatException(Calculateur.ResourceManager.GetString("ErreurSyntaxe"));
        }
        //Tous les param�tres de la fonction doivent �tre des variables avec des noms diff�rents
        int i = 0;
        foreach (ElementCalculVariable elt in premiereSomme.ListeParametres)
        {
          _paramsFonctionCourante.Add(elt.NomVariable, i.ToString(System.Globalization.CultureInfo.InvariantCulture));
          i++;
        }
        retour = new ElementCalculAffFonc(premierFonction, LitSomme(), _tokensiner.Expression.Substring(positionDebutFonction, _tokenCourant.PositionDebutToken - positionDebutFonction));
      }
      else
        retour = premiereSomme;
      return retour;
    }

    /// <summary>
    /// Analyse d'une somme
    /// ---------------------------------------------------------
    /// |  Somme    ::= Prod '+' Somme | Prod '-' Somme | Prod
    /// | Analyse des additions et des soustractions
    /// ---------------------------------------------------------
    /// </summary>
    /// <returns></returns>
    private IElementCalcul LitSomme()
    {
      IElementCalcul eltGauche;
      IElementCalcul eltDroit;
      IElementCalcul retour = null;
      eltGauche = LitProd();
      if (_tokenCourant.TypeToken == ENTypeToken.ENPlus) // addition
      {
        LitTokenSuivant();
        eltDroit = LitSomme();
        retour = new ElementCalculFBinaire(eltGauche, new FonctionBinaire((double a, double b) => a + b), eltDroit);
      }
      else if (_tokenCourant.TypeToken == ENTypeToken.ENMoins)	// soustraction
      {
        LitTokenSuivant();
        eltDroit = LitSomme();
        retour = new ElementCalculFBinaire(eltGauche, new FonctionBinaire((double a, double b) => a - b), eltDroit);
      }
      else
      {
        retour = eltGauche;
      }
      return retour;
    }

    /// <summary>
    ///  Prod     ::= Elem '*' Prod | Elem '/' Prod | Elem  |
    ///  Analyse des multiplications et des divisions
    /// </summary>
    /// <returns></returns>
    private IElementCalcul LitProd()
    {
      IElementCalcul eltGauche;
      IElementCalcul eltDroit;
      IElementCalcul retour = null;

      eltGauche = LitElem();

      if (_tokenCourant.TypeToken == ENTypeToken.ENMultiplication)
      {
        LitTokenSuivant();
        eltDroit = LitProd();
        retour = new ElementCalculFBinaire(eltGauche, new FonctionBinaire((double a, double b) => a * b), eltDroit);
      }
      else if (_tokenCourant.TypeToken == ENTypeToken.ENDivision)
      {
        LitTokenSuivant();
        eltDroit = LitProd();
        retour = new ElementCalculFBinaire(eltGauche, new FonctionBinaire((double a, double b) => a / b), eltDroit);
      }
      else
      {
        return eltGauche;
      }
      return retour;
    }

    /// <summary>
    /// -------------------------------------------------------
    /// | Elem  ::= variable | constante | '(' Somme ')' | Fonction '(' Parametres ')'
    /// | Analyse des variable, contantes et parenth�ses
    /// -------------------------------------------------------
    /// </summary>
    /// <returns></returns>
    private IElementCalcul LitElem()
    {
      IElementCalcul retour;
      if (_tokenCourant.TypeToken == ENTypeToken.ENOuvertureParenthese)
      {
        LitTokenSuivant();
        retour = LitSomme();
        if (_tokenCourant.TypeToken != ENTypeToken.ENFermetureParenthese)
        {
          // Erreur de synthaxe
          throw (new FormatException(string.Format(System.Globalization.CultureInfo.CurrentUICulture, 
            Calculateur.ResourceManager.GetString("EltAttPos"), 
            (_tokenCourant.PositionDebutToken), ")")));
        }
        LitTokenSuivant();
      }
      else if (_tokenCourant.TypeToken == ENTypeToken.ENPlus) // opr�rateur unaire
      {
        LitTokenSuivant();
        retour = LitElem();

      }
      else if (_tokenCourant.TypeToken == ENTypeToken.ENMoins) // opr�rateur unaire
      {
        LitTokenSuivant();
        retour = new ElementCalculFUnaire(new FonctionUnaire((double a) => -a), LitElem());
      }
      else
      {
        Token tokenSuivant = LitTokenSuivant(false);
        if (tokenSuivant.TypeToken == ENTypeToken.ENOuvertureParenthese) //C'est une fonction
        {
          retour = LitFonction();

        }
        else //Variable ou d�finition d'une nouvelle fonction
        {
          retour = LitValeur();
        }
      }
      return retour;
    }

    /// <summary>
    /// Lecture de la fonction
    /// </summary>
    /// <param name="nomFonction">Nom de la fonction</param>
    private IElementCalcul LitFonction()
    {
      string nomFonction = _tokenCourant.Text;
      List<IElementCalcul> listParametres;
      LitTokenSuivant(); // "("
      //Lecture de la liste des param�tres
      listParametres = LitParametresFonction();
      return Fonction.Factory(nomFonction, listParametres);
    }

    /// <summary>
    /// Parametre := somme | somme , Parametre
    /// Retourne la liste des param�tres de la fonction
    /// </summary>
    /// <returns></returns>
    List<IElementCalcul> LitParametresFonction()
    {
      List<IElementCalcul> listeElement = new List<IElementCalcul>();
      if (_tokenCourant.TypeToken != ENTypeToken.ENOuvertureParenthese)
      {
        throw (new FormatException(string.Format(System.Globalization.CultureInfo.CurrentUICulture, Calculateur.ResourceManager.GetString("EltAttPos"), (_tokenCourant.PositionDebutToken), '(')));
      }
      LitTokenSuivant();
      do
      {
        listeElement.Add(LitSomme());
        if (_tokenCourant.TypeToken == ENTypeToken.ENSeparateurParametres)
          LitTokenSuivant();
        else
          break;
      }
      while (true);

      if (_tokenCourant.TypeToken != ENTypeToken.ENFermetureParenthese)
      {
        throw (new FormatException(string.Format(System.Globalization.CultureInfo.CurrentUICulture, Calculateur.ResourceManager.GetString("EltAttPos"), (_tokenCourant.PositionDebutToken), ')')));
      }
      LitTokenSuivant();
      return listeElement;
    }

    /// <summary>
    /// ---------------------------------------------------------------------------
    /// |     Analyse lexicale : �valuation d'une variable ou d'une constante     |
    /// ---------------------------------------------------------------------------
    /// </summary>
    /// <returns></returns>
    private IElementCalcul LitValeur()
    {
      IElementCalcul retour;
      if (_tokenCourant.TypeToken == ENTypeToken.ENNombre)
      {
        // C'est un nombre
        retour = new ElementCalculValeur(_tokenCourant.Text); // convertion en float
        LitTokenSuivant();
      }
      else if (_tokenCourant.TypeToken == ENTypeToken.ENIdentifiant)//gestion des variables
      {
        retour = LitVariable();
      }
      else
      {
        // Erreur de syntaxe
        throw (new FormatException(Calculateur.ResourceManager.GetString("ErreurSyntaxe")));
      }
      return retour;
    }

    /// <summary>
    /// Lecteur d'une variable
    /// </summary>
    /// <returns></returns>
    private IElementCalcul LitVariable()
    {
      Token nomVariable = _tokenCourant;
      IElementCalcul retour;
      LitTokenSuivant();
      if (_tokenCourant.TypeToken == ENTypeToken.ENEgal)
      {
        LitTokenSuivant();
        retour = LitSomme();
        retour = new ElementCalculAffectVar(new ElementCalculVariable(nomVariable.Text), retour);
      }
      else
      {
        if (_paramsFonctionCourante.ContainsKey(nomVariable.Text)) //C'est le param�tre de la fonction en court de d�finition
        {
          retour = new ElementCalculVariable(_paramsFonctionCourante[nomVariable.Text]);
        }
        else //C'est une variable
        {
          retour = new ElementCalculVariable(nomVariable.Text);
        }
      }
      return retour;
    }

    private Token LitTokenSuivant(bool depiler)
    {
      if (depiler)
      {
        if (_tokenSuivant != null)
        {
          _tokenCourant = _tokenSuivant;
          _tokenSuivant = null;
        }
        else
        {
          if (!_enumeratorToken.MoveNext())
            throw new ApplicationException();
          _tokenCourant = _enumeratorToken.Current;
        }
      }
      else 
      {
        if (_tokenSuivant != null)
          return _tokenSuivant;
        if (!_enumeratorToken.MoveNext())
          throw new ApplicationException();
        _tokenSuivant = _enumeratorToken.Current;
        return _tokenSuivant;
      }
      return _tokenCourant;
    }

    private Token LitTokenSuivant()
    {
      return LitTokenSuivant(true);
    }

    #region Attributs priv�s
    private Token _tokenCourant;
    private Token _tokenSuivant;
    
    private Dictionary<string, string> _paramsFonctionCourante;
    #endregion
  }
}
