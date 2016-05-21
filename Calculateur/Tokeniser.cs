using System;
using System.Collections.Generic;
using System.Text;

namespace Calculateur
{
    /// <summary>
  /// Les type de lexens
  /// </summary>
  public enum ENCaract
  {
    ENElement,
    ENSeparateur,
    ENLettre,
    ENChiffre,
    ENOperande,
    ENParenthese,
    ENVirgule,
    ENSepVariable,
    ENFin
  }

  /// <summary>
  /// Les types de token
  /// </summary>
  public enum ENTypeToken
  {
    ENVariable,
    ENParenthese,    
    ENOperande,
    ENEgal,
    ENPlus,
    ENMoins,
    ENMultiplication,
    ENDivision,
    ENOuvertureParenthese,
    ENFermetureParenthese,
    ENSeparateurParametres,
    ENIdentifiant,
    ENNombre,
    ENFin,
  }
  public class Token
  {
    public string Text;
    public ENTypeToken TypeToken;
    public int PositionDebutToken;

    public Token(string text, ENTypeToken typeToken, int positionDebutToken)
    {
      Text = text;
      TypeToken = typeToken;
      PositionDebutToken = positionDebutToken;
    }
  }
  public class Tokeniser
  {
    public Tokeniser(string expression)
    {
      _posCourant = 0;
      _phraseCourante = expression + '\0';

    }

    static Tokeniser()
    {

      InitListeTypeCaract();
    }

    public IEnumerable<Token> Tokens
    {
      get 
      {
        do
        {
          LitToken();
          yield return new Token(_tokenCourant, _typeTokenCourant, _positionDebutTokenCourant);
        } while (_typeTokenCourant != ENTypeToken.ENFin);
        yield return new Token("", ENTypeToken.ENFin, _positionDebutTokenCourant);
      }
    }

    public string Expression
    {
      get 
      {
        return _phraseCourante;
      }
    }

    /// <summary>
    /// Initialisation de la liste des types de caractères
    /// </summary>
    private static void InitListeTypeCaract()
    {
      _typeChar = new ENCaract[256];
      for (int i = 0; i < 256; i++)
      {
        _typeChar[i] = ENCaract.ENElement;
      }

      for (int i = 'a'; i <= 'z'; i++)
      {
        _typeChar[i] = ENCaract.ENLettre;
      }

      for (int i = 'A'; i <= 'Z'; i++)
      {
        _typeChar[i] = ENCaract.ENLettre;
      }

      for (int i = '0'; i <= '9'; i++)
      {
        _typeChar[i] = ENCaract.ENChiffre;
      }
      _typeChar['\t'] = ENCaract.ENSeparateur;
      _typeChar[' '] = ENCaract.ENSeparateur;

      _typeChar['+'] = ENCaract.ENOperande;
      _typeChar['-'] = ENCaract.ENOperande;
      _typeChar['*'] = ENCaract.ENOperande;
      _typeChar['/'] = ENCaract.ENOperande;
      _typeChar['='] = ENCaract.ENOperande;
      _typeChar['('] = ENCaract.ENParenthese;
      _typeChar[')'] = ENCaract.ENParenthese;
      _typeChar[','] = ENCaract.ENSepVariable;
      _typeChar['.'] = ENCaract.ENVirgule;
      _typeChar['\0'] = ENCaract.ENFin;
    }

    /// <summary>
    /// Lit le token suivant sans modifier
    /// </summary>
    /// <returns></returns>
    private string LitTokenSuivant()
    {
      int positionCourante = _posCourant;
      string token = _tokenCourant;
      string retour;
      LitToken();
      retour = _tokenCourant;
      _tokenCourant = token;
      _posCourant = positionCourante;
      return retour;
    }

    /// <summary>
    /// --------------------------------------------------------------------------------
    /// | Renvoie le type de caractère : séparateur operande   parenthese ou caractère |
    /// |                                SEPARATEUR, OPERANDE, PARANTHESE,    ELEMENT  |
    /// --------------------------------------------------------------------------------
    /// </summary>
    /// <param name="unChar"></param>
    /// <returns></returns>
    private static ENCaract LitCaractere(char unChar)
    {
      return (ENCaract)_typeChar[unChar];
    }

    /// <summary>
    /// Lecture d'un token de type nombre dans la phrase courante
    /// </summary>
    /// <returns></returns>
    private void LitTokenNombre()
    {
      int debut = _posCourant;
      _typeTokenCourant = ENTypeToken.ENNombre;
      ENCaract typeCarat;
      Avancer(ENCaract.ENChiffre);

      //Lecture de la virgule
      typeCarat = LitCaractere(_phraseCourante[_posCourant]);
      if (typeCarat == ENCaract.ENVirgule)
      {
        _posCourant++;
        //Lecture des chiffres décimaux
        Avancer(ENCaract.ENChiffre);
      }

      //Lecture de l'exposant
      if (char.ToLower(_phraseCourante[_posCourant], System.Globalization.CultureInfo.InvariantCulture) == 'e')
      {
        _posCourant++;
        Avancer(ENCaract.ENChiffre);
      }
      _tokenCourant = _phraseCourante.Substring(debut, _posCourant - debut);

      //Transformation du point en virgule
      _tokenCourant = _tokenCourant.Replace(',', '.');
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="typeCarat"></param>
    private void Avancer(ENCaract typeCarat)
    {
      ENCaract typeCaractCourant = LitCaractere(_phraseCourante[_posCourant]);
      while (LitCaractere(_phraseCourante[_posCourant]) == typeCarat)
      {
        _posCourant++;
        typeCaractCourant = LitCaractere(_phraseCourante[_posCourant]);
      }
    }


    /// <summary>
    /// Lecture d'un token de type opérande
    /// </summary>
    private void LitTokenOperande()
    {
      _tokenCourant = _phraseCourante[_posCourant++].ToString();
      switch(_tokenCourant[0])
      {
        case '-':
          _typeTokenCourant = ENTypeToken.ENMoins;
          break;
          case '+':
          _typeTokenCourant = ENTypeToken.ENPlus;
          break;
          case '*':
          _typeTokenCourant = ENTypeToken.ENMultiplication;
          break;
          case '/':
          _typeTokenCourant = ENTypeToken.ENDivision;
          break;
          case '=':
          _typeTokenCourant = ENTypeToken.ENEgal;
          break;
        default : throw new ApplicationException();
      }
    }

    /// <summary>
    /// Lecture d'un variable ou d'une fonction
    /// </summary>
    private void LitTokenFonctionVariable()
    {
      int debut = _posCourant;
      Avancer(ENCaract.ENLettre);
      _tokenCourant = _phraseCourante.Substring(debut, _posCourant - debut);
      _typeTokenCourant = ENTypeToken.ENIdentifiant;
    }

    private void LitToken()
    {
      int debut;
      ENCaract typeCaractCourant;
      // on passe les separateurs
      Avancer(ENCaract.ENSeparateur);
      _positionDebutTokenCourant = _posCourant;
      typeCaractCourant = LitCaractere(_phraseCourante[_posCourant]);
      debut = _posCourant;

      switch (typeCaractCourant)
      {
        case ENCaract.ENChiffre: //C'est un nombre
          LitTokenNombre();
          return;

        case ENCaract.ENOperande:// si c'est un opérande
          LitTokenOperande();
          return;

        case ENCaract.ENParenthese: // si c'est une parenthese
          _tokenCourant = _phraseCourante[_posCourant].ToString();
          if(_tokenCourant == "(")
            _typeTokenCourant = ENTypeToken.ENOuvertureParenthese;
          else
            _typeTokenCourant = ENTypeToken.ENFermetureParenthese;
          _posCourant++;
          return;

        case ENCaract.ENFin: //Fin de la phrase
          _tokenCourant = " ";
          _typeTokenCourant = ENTypeToken.ENFin;
          return;

        case ENCaract.ENSepVariable: //C'est un séparateur de paramètre de fonction
          _typeTokenCourant = ENTypeToken.ENSeparateurParametres;
          _tokenCourant = _phraseCourante[_posCourant].ToString();
          _posCourant++;
          return;

        default:// sinon c'est une fonction ou une variable
          LitTokenFonctionVariable();
          break;
      }
    }
    private string _phraseCourante; // la phrase en cours de traitement
    private string _tokenCourant;   // le token courant
    private ENTypeToken _typeTokenCourant;
    private int _posCourant;		  // la position courant dans la phrase
    private static ENCaract[] _typeChar;
    private int _positionDebutTokenCourant;
  }
}
