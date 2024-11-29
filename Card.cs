/*
 * Author: Kritika Kapri
 * Date: November 29, 2024
 * Description: Defines the Card class to represent individual playing cards
 *              with a suit and rank, including a custom string representation.
 */

using System;

namespace Advanced_DeckBuilder
{
    public class Card
    {
        public string Suit { get; set; }
        public string Rank { get; set; }

        // Constructor to initialize a card
        public Card(string suit, string rank)
        {
            Suit = suit;
            Rank = rank;
        }

        // Override ToString for easy display of card details
        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }
    }
}


