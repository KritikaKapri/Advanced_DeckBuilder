/*
 * Author: Kritika Kapri
 * Date: November 29, 2024
 * Description: Defines the Card class to represent individual playing cards
 *              with a suit and rank, including a custom string representation.
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace Advanced_DeckBuilder
{
    public class Deck
    {
        protected List<Card> Cards;

        // Constructor to initialize an empty deck
        public Deck()
        {
            Cards = new List<Card>();
        }

        // Add a card to the deck
        public void AddCard(Card card)
        {
            Cards.Add(card);
        }

        // Shuffle the deck
        public void Shuffle()
        {
            Random random = new Random();
            Cards = Cards.OrderBy(c => random.Next()).ToList();
        }

        // Deal a specified number of cards
        public List<Card> DealCards(int count)
        {
            if (count > Cards.Count)
                throw new InvalidOperationException("Not enough cards in the deck to deal.");

            List<Card> dealtCards = Cards.Take(count).ToList();
            Cards.RemoveRange(0, count);
            return dealtCards;
        }

        // Get the number of remaining cards in the deck
        public int RemainingCards()
        {
            return Cards.Count;
        }
    }
}


