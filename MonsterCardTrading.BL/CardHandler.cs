using MonsterCardTrading.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.BL
{
    public class CardHandler
    {
        public void createPackage(List<Card> cards)
        {
            //check token?

            //check cards

            //add cards to cards and stacks
        }

        public Card getCard(string id)
        {

        }

        public void aquirePackage(User user)
        {
            //check money of User

            //check available packages

            //add cards to users stack

        }

        public List<Card> getCards(User user)
        {
            return null;
        }

        public List<Card> getDeck(User user, bool plain = false)
        {
            return null;
        }

        public void setDeck(User user, List<Card> cards)
        {
            //check amount of cards

            //check if cards belong to user

            //save deck in stacks
        }
    }
}
