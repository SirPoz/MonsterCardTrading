using MonsterCardTrading.DAL;
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

        private DatabaseHandler db;

        public CardHandler()
        {
            db = new DatabaseHandler();
        }

        public void createPackage(User user, Stack stack)
        {
            //check token?
            if(user.Username != "admin")
            {
                throw new ResponseException("Provided user is not \"admin\"", 403);
            }

            //create package id

            int packageid = db.getMaxPackageId() + 1;


            //check
           
            for(int i = 0; i < stack.Cards.Count; i++)
            {
                stack.Cards[i] = initCardFromPackage(stack.Cards[i], packageid);
            }
            


            //add cards to cards and stacks
            
             db.AddPackage(stack);
            
        }

       

        public List<Card> aquirePackage(User user)
        {
            
            

            
            List<Card> cards = db.DrawPackage(user);
            

            //return packages
            return cards;

        }

        public Card getCard(string id)
        {
            try
            {
                Card card = db.GetCard(id);
                return card;
            }
            catch
            {
                return null;
            }
           

        }

        public List<Card> getCards(User user)
        {
            List<Card> cards = db.GetCards(user);        
            return cards; 
            
        }

        public List<Card> getDeck(User user)
        {
            List<Card> cards = db.GetDeck(user);
            return cards;
        }

        public void setDeck(User user, string[] cards)
        {
            db.SetDeck(user, cards);
        }

        private Card initCardFromPackage(Card card, int packageid)
        {
            if(getCard(card.Id) != null)
            {
                throw new ResponseException("At least one card in the packages already exists", 409);
            }



            card.packageid = packageid;
            
            string name = card.Name;

            int count = 0;
            for (int i = 0; i < name.Length; i++)
            {
                if (char.IsUpper(name[i])) count++;
            }
            if(count == 1)
            {
                card.Element = Element.Normal;
                
                if(MonsterCardTrading.Model.Species.TryParse(name, out Species species))
                {
                    card.Type = species;
                }
                else
                {
                    throw new Exception("Could not define species: " + name);
                }
            }
            else
            {
                string element = "";
                string species = "";
                for (int i = 0; i < name.Length; i++)
                {
                    if (char.IsUpper(name[i]))
                    {
                        count--;
                        if (count == 0)
                        {
                            element = name.Substring(0, i);
                            species = name.Substring(i);
                            //exception because Regular = Normal
                            if(element == "Regular") { element = "Normal"; }
                        }
                    }
                   
                }

                if (MonsterCardTrading.Model.Species.TryParse(species, out Species parseSpecies) && MonsterCardTrading.Model.Element.TryParse(element, out Element parseElement))
                {
                    card.Element = parseElement;
                    card.Type = parseSpecies;
                }
                else
                {
                    throw new Exception("Could not parse: " + species + " " + element);
                }

            }
            return card;

        }
    }
}
