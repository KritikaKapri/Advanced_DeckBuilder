/*
 * Author: Kritika Kapri
 * Date: November 29, 2024
 * Description: Defines the StandardDeck class for creating a standard 52-card deck.
 *              Includes functionality to initialize and populate the deck with all
 *              standard suits and ranks.
 */
namespace Advanced_DeckBuilder
{
    public class StandardDeck : Deck
    {
        // Constructor to initialize the standard deck
        public StandardDeck()
        {
            InitializeStandardDeck();
        }

        // Populate the deck with 52 standard cards
        private void InitializeStandardDeck()
        {
            string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
            string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };

            foreach (var suit in suits)
            {
                foreach (var rank in ranks)
                {
                    AddCard(new Card(suit, rank));
                }
            }
        }
    }
}

