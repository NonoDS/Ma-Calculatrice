using System;
using System.Collections.Generic;
using System.Text;

namespace Calculateur
{
    class AnalyseurMath
    {
        #region Enum�res

        public enum EnCaract : byte
        {
            enELEMENT,
            enSEPARATEUR,
            enLETTRE,
            enCHIFFRE,
            enOPERANDE,
            enPARENTHESE,
            enVIRGULE,
            enSEPVARIABLE,
            enFIN
        }

        public enum EnTypeToken
        {
            enVARIABLE,
            enPARENTHESE,
            enNOMBRE,
            enOPERANDE
        }

        #endregion

        #region Fonctions
        private static double Adition(double valeur1, double valeur2)
        {
            return valeur1 + valeur2;
        }

        private static double Soustraction(double valeur1, double valeur2)
        {
            return valeur1 - valeur2;
        }

        private static double Multiplication(double valeur1, double valeur2)
        {
            return valeur1 * valeur2;
        }

        private static double Division(double valeur1, double valeur2)
        {
            return valeur1 / valeur2;
        }

        private static double Negatif(double valeur1)
        {
            return -valeur1;
        }
        
        #endregion
        #region Constructeur
        public AnalyseurMath()
        {
        
        }

        static AnalyseurMath()
        {
            InitListeTypeCaract();
        }

        #endregion

        /// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
        public IElementCalcul CreationArbre(string expression)
		{
            mParamsFonctionCourante = null;
            mPosCourant = 0 ;
            mPhraseCourante = expression + '\0';
            mParamsFonctionCourante = new Dictionary<string, string>(3);
			LitToken() ;
			if(mTokenCourant == "")
				return null ;
			return LitAffectationFonction() ;
		}

        /// <summary>
        /// Expression = somme | somme '=' somme
        /// </summary>
        /// <returns></returns>
        private IElementCalcul LitAffectationFonction()
        {
            IElementCalcul retour;
            IElementCalcul premiereSomme = LitSomme();
            if (mTokenCourant == "=") //D�finition d'une fonction
            {
                LitToken();
                if (!(premiereSomme is Fonction))
                {
                    throw new Exception("Erreur de syntaxe");
                }
                //Tous les param�tres de la fonction doivent �tre des variables avec des nom diff�rents
                int i = 0;
                foreach (ElementCalculVariable elt in (premiereSomme as Fonction).ListeParametres)
                {
                    mParamsFonctionCourante.Add(elt.NomVariable, i.ToString());
                    i++;
                }
                retour = new ElementCalculAffFonc(premiereSomme as Fonction, LitSomme());
            }
            else
                retour = premiereSomme;
            return retour;
        }


		//---------------------------------------------------------
		//|  Somme    ::= Prod '+' Somme | Prod '-' Somme | Prod
		//| Analyse des additions et des soustractions
		//---------------------------------------------------------
		private IElementCalcul LitSomme()
		{
			IElementCalcul eltGauche;
			IElementCalcul eltDroit;
            IElementCalcul retour = null;
			eltGauche = LitProd() ;
			if(mTokenCourant == "+") // addition
			{
				LitToken();
				eltDroit = LitSomme();
                retour = new ElementCalculFBinaire(eltGauche, new fonctionBinaire(Adition), eltDroit);
			}
            else if (mTokenCourant == "-")	// soustraction
            {
                LitToken();
                eltDroit = LitSomme();
                retour = new ElementCalculFBinaire(eltGauche, new fonctionBinaire(Soustraction), eltDroit);
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
    
			if (mTokenCourant == "*")
			{
				LitToken();
				eltDroit = LitProd();
                retour = new ElementCalculFBinaire(eltGauche, new fonctionBinaire(Multiplication), eltDroit);
			}
            else if (mTokenCourant == "/")
            {
                LitToken();
                eltDroit = LitProd();
                retour = new ElementCalculFBinaire(eltGauche, new fonctionBinaire(Division), eltDroit);
            }
            else
            {
                return eltGauche;
            }
			return retour;
		}

		// -------------------------------------------------------
		// | Elem  ::= variable | constante | '(' Somme ')' | Fonction '(' Parametres ')'
		// | Analyse des variable, contantes et parenth�ses
		// -------------------------------------------------------
		private IElementCalcul LitElem()
		{
			IElementCalcul retour;
			if (mTokenCourant == "(")
			{
				LitToken();
				retour = LitSomme();
				if (mTokenCourant != ")") 
				{						
					// Erreure de synthaxe
					throw (new Exception("El�ment attendu � la position " + (mPosCourant - mTokenCourant.Length) + " : )"));
				}
				LitToken();
			}
			else if(mTokenCourant == "+") // opr�rateur unaire
			{
				LitToken();
				retour = LitElem();
                
			}
            else if (mTokenCourant == "-") // opr�rateur unaire
            {
                LitToken();
                retour = new ElementCalculFUnaire(new fonctionUnaire(Negatif), LitElem());
            }
            else
            {
                string tokenSuivant = LitTokenSuivant();
                if (tokenSuivant == "(") //C'est une fonction
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
        /// <param name="nomFonction"></param>
        private IElementCalcul LitFonction()
        {
            string nomFonction = mTokenCourant;
            List<IElementCalcul> listParametres;
            LitToken(); // "("
            //Lecture de la liste des param�tres
            listParametres = LitParametresFonction();
            return new Fonction(nomFonction, listParametres);
        }

        /// <summary>
        /// Parametre := somme | somme , Parametre
        /// Retourne la liste des param�tres de la fonction
        /// </summary>
        /// <returns></returns>
        List<IElementCalcul> LitParametresFonction()
        {
            List<IElementCalcul> listeElement = new List<IElementCalcul>();
            if (mTokenCourant != "(")
            {
                throw new Exception("Carat�re Attendu '('.");
            }
            LitToken();
            do
            {
                listeElement.Add(LitSomme());
                if (mTokenCourant == ",")
                    LitToken();
                else
                    break;


            }
            while(true);

            if (mTokenCourant != ")")
            {
                throw new Exception("Carat�re Attendu ')'.");
            }
            LitToken();

            return listeElement;
        }

		// ---------------------------------------------------------------------------
		// |     Analyse lexicale : �valuation d'une variable ou d'une constante     |
		// ---------------------------------------------------------------------------
		private IElementCalcul LitValeur()
		{
			IElementCalcul retour;
		    EnCaract typCaractCourant = LitCaractere(mTokenCourant[0]);
			if (typCaractCourant == EnCaract.enCHIFFRE)
			{
				// C'est un nombre
				retour = new ElementCalculValeur(mTokenCourant); // convertion en float
				LitToken();
			}
			else if(typCaractCourant == EnCaract.enLETTRE)//gestion des variables
			{
				retour = LitVariable();
			}
			else
			{
				// Erreur de syntaxe
				throw (new Exception("Erreur de syntaxe")) ;
			}
			return retour;
		}

		/// <summary>
		/// Lecteur d'une variable
		/// </summary>
		/// <returns></returns>
		private IElementCalcul LitVariable()
		{
			string nomVariable = mTokenCourant;
			IElementCalcul retour;
			LitToken();
			if(mTokenCourant == "=")
			{
				LitToken();
				retour = LitSomme();
                retour = new ElementCalculAffectVar(new ElementCalculVariable(nomVariable), retour);
			}
			else
			{
                if (mParamsFonctionCourante.ContainsKey(nomVariable)) //C'est le param�tre de la fonction en court de d�finition
                {
                    retour = new ElementCalculVariable(mParamsFonctionCourante[nomVariable]);
                }
                else //C'est une variable
                {
                    retour = new ElementCalculVariable(nomVariable);
                }
			}
			return retour ;
		}

		//---------------------------------------------------
		//| S�paration de la phrase en tokens
		//| Passe les s�parateurs et lit le token suivant,
		//| par rapport � la position actuel : m_posCourant
		//| Le token lut est stock� dans m_tokenCourant
		//-----------------------------------------
		#region  Analyse Lexicale
		private void LitToken()
		{
			int debut;
			EnCaract typeCaractCourant;
			// on passe les separateurs
			Avancer(EnCaract.enSEPARATEUR);
			typeCaractCourant = LitCaractere(mPhraseCourante[mPosCourant]);
			debut = mPosCourant;
    
            switch(typeCaractCourant)
            {
                case EnCaract.enCHIFFRE: //C'est un nombre
				LitTokenNombre();
				return;

                case EnCaract.enOPERANDE:// si c'est un op�rande
				LitTokenOperande();
				return;
			
                case EnCaract.enPARENTHESE: // si c'est une parenthese
				mTokenCourant = mPhraseCourante[mPosCourant].ToString();
				mPosCourant ++;
				return;

                case EnCaract.enFIN: //Fin de la phrase
				mTokenCourant = " ";
				return;

                case EnCaract.enSEPVARIABLE: //C'est un s�parateur de param�tre de fonction
                mTokenCourant = mPhraseCourante[mPosCourant].ToString();
                mPosCourant++;
                return;

                default:// sinon c'est une fonction ou une variable			
			    LitTokenFonctionVariable();
                break;
            }
	    }

        /// <summary>
        /// Lit le token suivant sans modifier
        /// </summary>
        /// <returns></returns>
        private string LitTokenSuivant()
        {
            int positionCourante = mPosCourant;
            string token = mTokenCourant;
            string retour;
            LitToken();
            retour = mTokenCourant;
            mTokenCourant = token;
            mPosCourant = positionCourante;
            return retour;
        }

		//--------------------------------------------------------------------------------
		//| Renvoie le type de caract�re : s�parateur operande   parenthese ou caract�re |
		//|                                SEPARATEUR, OPERANDE, PARANTHESE,    ELEMENT  |
		//--------------------------------------------------------------------------------
		private static EnCaract LitCaractere(char unChar)
		{
			return (EnCaract)mTypeChar[unChar] ;
		}

		/// <summary>
		/// Lecture d'un token de type nombre dans la phrase courante
		/// </summary>
		/// <returns></returns>
		private void LitTokenNombre()
		{			
			int debut = mPosCourant ;
			EnCaract typeCarat ;
			Avancer(EnCaract.enCHIFFRE) ;

			//Lecture de la virgule
			typeCarat = LitCaractere(mPhraseCourante[mPosCourant]) ;
			if(typeCarat == EnCaract.enVIRGULE)
			{
				mPosCourant ++ ;
				//Lecture des chiffres d�cimaux
				Avancer(EnCaract.enCHIFFRE) ;
			}
			
			//Lecture de l'exposant
			if(mPhraseCourante[mPosCourant].ToString().ToLower() == "e")
			{
				mPosCourant ++ ;
				Avancer(EnCaract.enCHIFFRE) ;
			}
			mTokenCourant = mPhraseCourante.Substring(debut, mPosCourant - debut) ;
			
			//Transformation du point en virgule
			mTokenCourant = mTokenCourant.Replace('.', ',') ;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="typeCarat"></param>
		private void Avancer(EnCaract typeCarat)
		{
			EnCaract typeCaractCourant = LitCaractere(mPhraseCourante[mPosCourant]) ;
			while(LitCaractere(mPhraseCourante[mPosCourant]) == typeCarat)
			{
				mPosCourant ++ ;
				typeCaractCourant = LitCaractere(mPhraseCourante[mPosCourant]) ;
			}
		}


		/// <summary>
		/// Lecture d'un token de type op�rande
		/// </summary>
		private void LitTokenOperande()
		{
			mTokenCourant = mPhraseCourante[mPosCourant++].ToString() ;	
		}

		/// <summary>
		/// Lecture d'un variable ou d'une fonction
		/// </summary>
		private void LitTokenFonctionVariable()
		{
			int debut = mPosCourant;
			Avancer(EnCaract.enLETTRE);
			mTokenCourant = mPhraseCourante.Substring(debut, mPosCourant - debut);
		}
		#endregion


        /// <summary>
        /// Initialisation de la liste des types de caract�res
        /// </summary>
        private static void InitListeTypeCaract()
        {
            mTypeChar = new char[256];
            for (int i = 0; i < 256; i++)
            {
                mTypeChar[i] = (char)EnCaract.enELEMENT;
            }

            for (int i = 'a'; i <= 'z'; i++)
            {
                mTypeChar[i] = (char)EnCaract.enLETTRE;
            }

            for (int i = 'A'; i <= 'Z'; i++)
            {
                mTypeChar[i] = (char)EnCaract.enLETTRE;
            }

            for (int i = '0'; i <= '9'; i++)
            {
                mTypeChar[i] = (char)EnCaract.enCHIFFRE;
            }
            mTypeChar['\t'] = (char)EnCaract.enSEPARATEUR;
            mTypeChar[' '] = (char)EnCaract.enSEPARATEUR;

            mTypeChar['+'] = (char)EnCaract.enOPERANDE;
            mTypeChar['-'] = (char)EnCaract.enOPERANDE;
            mTypeChar['*'] = (char)EnCaract.enOPERANDE;
            mTypeChar['/'] = (char)EnCaract.enOPERANDE;
            mTypeChar['='] = (char)EnCaract.enOPERANDE;
            mTypeChar['('] = (char)EnCaract.enPARENTHESE;
            mTypeChar[')'] = (char)EnCaract.enPARENTHESE;
            mTypeChar[','] = (char)EnCaract.enSEPVARIABLE;
            mTypeChar['.'] = (char)EnCaract.enVIRGULE;
            mTypeChar['\0'] = (char)EnCaract.enFIN;
        }

        #region Attributs priv�s
        private string mPhraseCourante; // la phrase en court de traitement
		private string mTokenCourant;   // le token courant
		private int mPosCourant;		  // la position courant dans la phrase
		private static char [] mTypeChar;
		public string mMessageErreur;
        private Dictionary<string, string> mParamsFonctionCourante;
        #endregion
		//private EnTypeToken m_typeTokenCourant;
    }
}
