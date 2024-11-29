/*
 * Author: Kritika Kapri
 * Date: November 29, 2024
 * Description: Defines the CustomDeck class, allowing users to create and
 *              populate a deck with custom cards.
 */

namespace Advanced_DeckBuilder
{
    public class CustomDeck : Deck
    {
        // Constructor for a custom deck (starts empty)
        public CustomDeck()
        {
        }

        // Add a custom card to the deck
        public void AddCustomCard(string suit, string rank)
        {
            if (string.IsNullOrWhiteSpace(suit) || string.IsNullOrWhiteSpace(rank))
                throw new ArgumentException("Suit and Rank cannot be empty.");

            AddCard(new Card(suit, rank));
        }
    }
}
