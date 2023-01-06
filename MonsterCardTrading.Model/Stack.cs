using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterCardTrading.Model
{
    public class Stack
    {
        public List<Card> Cards { get; private set; }
        public User Owner { get; private set; }
        public int DeckSize { get; private set; }

        public List<Card> Deck { get; private set; }

        public Stack (User owner, List<Card> cards,  List<Card> deck)
        {
            Cards = cards;
            Owner = owner;
            Deck = deck;
            DeckSize = Deck.Count;
        }
    }
}
