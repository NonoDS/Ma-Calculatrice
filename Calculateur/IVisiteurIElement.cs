using System;
using System.Collections.Generic;
using System.Text;

namespace Calculateur
{
    public interface IVisiteurIElement
    {
        void Visit(ElementCalculAffectVar elt);
        void Visit(ElementCalculAffFonc elt);
        void Visit(ElementCalculFBinaire elt);
        void Visit(ElementCalculFUnaire elt);
        void Visit(ElementCalculValeur elt);
        void Visit(ElementCalculVariable elt);
        void Visit(FonctionTrigo elt);
        void Visit(Fonction elt);
    }
}
