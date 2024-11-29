/*
 * Author: Kritika Kapri
 * Date: November 29, 2024
 * Description: Implements a WPF application to manage a card deck. Features
 *              include adding custom cards, resetting, shuffling, dealing, 
 *              saving, and viewing the deck, with database integration.
 */
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace Advanced_DeckBuilder
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Server=Kritika;Database=DeckBuilderDB;Trusted_Connection=True;";

        public MainWindow()
        {
            InitializeComponent();
            LoadDeck();

        }
        /// <summary>
        ///  Load all cards from the database to display
        /// </summary>
        private void LoadDeck()
        {
            // You can now call the LoadDeckFromDatabase() method directly
            LoadDeckFromDatabase(null, null);
        }
        // Fetch and display the deck from the database
        private void LoadDeckFromDatabase(object sender, RoutedEventArgs e)
        {
            {
                DeckListView.Items.Clear();

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string selectQuery = "SELECT Suit, Rank FROM Cards";
                        using (SqlCommand command = new SqlCommand(selectQuery, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    string suit = reader["Suit"].ToString();
                                    string rank = reader["Rank"].ToString();
                                    DeckListView.Items.Add($"{rank} of {suit}");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading deck: {ex.Message}");
                }
            }
        }



        // Reset the deck to a standard 52-card deck
        private void ResetButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM Cards";
                    using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.ExecuteNonQuery();
                    }

                    string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
                    string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };

                    foreach (var suit in suits)
                    {
                        foreach (var rank in ranks)
                        {
                            string insertQuery = "INSERT INTO Cards (Suit, Rank) VALUES (@Suit, @Rank)";
                            using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@Suit", suit);
                                insertCommand.Parameters.AddWithValue("@Rank", rank);
                                insertCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }

                LoadDeck();
                MessageBox.Show("Deck has been reset to a standard 52-card deck.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error resetting deck: {ex.Message}");
            }
        }

        // Add a custom card to the deck
        private void AddCustomButtonClick(object sender, RoutedEventArgs e)
        {
            string suit = SuitTextBox.Text.Trim();
            string rank = RankTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(suit) || string.IsNullOrWhiteSpace(rank))
            {
                MessageBox.Show("Suit and Rank cannot be empty.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO Cards (Suit, Rank) VALUES (@Suit, @Rank)";
                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Suit", suit);
                        command.Parameters.AddWithValue("@Rank", rank);
                        command.ExecuteNonQuery();
                    }
                }

                LoadDeck();
                SuitTextBox.Clear();
                RankTextBox.Clear();
                MessageBox.Show("Custom card added successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding card: {ex.Message}");
            }
        }

        // Deal a specified number of cards randomly
        private void DealButtonClick(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(DrawTextBox.Text, out int numberOfCards) && numberOfCards > 0)
            {
                List<string> dealtCards = DealCards(numberOfCards);
                CardsDealtListView.Items.Clear();
                foreach (string card in dealtCards)
                {
                    CardsDealtListView.Items.Add(card);
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid number.");
            }
        }

        // Save the current deck to the database
        private void SaveDeckToDatabaseCommand(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Optionally clear the existing deck in the database to avoid duplicates
                    string deleteQuery = "DELETE FROM Cards";
                    using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
                    {
                        deleteCommand.ExecuteNonQuery();
                    }

                    // Save the current deck to the database
                    foreach (var card in DeckListView.Items)
                    {
                        string cardDetails = card.ToString(); // e.g., "Ace of Spades"
                        string[] cardParts = cardDetails.Split(new[] { " of " }, StringSplitOptions.None);
                        string rank = cardParts[0];
                        string suit = cardParts[1];

                        string insertQuery = "INSERT INTO Cards (Suit, Rank) VALUES (@Suit, @Rank)";
                        using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@Suit", suit);
                            insertCommand.Parameters.AddWithValue("@Rank", rank);
                            insertCommand.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Deck saved to database successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving deck to database: {ex.Message}");
            }
        }


        /// <summary>
        /// Retrieves a random set of cards from the database.
        /// </summary>
        private List<string> DealCards(int numberOfCards)
        {
            List<string> dealtCards = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string selectQuery = "SELECT TOP (@NumberOfCards) Suit, Rank FROM Cards ORDER BY NEWID()";
                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("@NumberOfCards", numberOfCards);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string suit = reader["Suit"].ToString();
                                string rank = reader["Rank"].ToString();
                                dealtCards.Add($"{rank} of {suit}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error dealing cards: {ex.Message}");
            }

            return dealtCards;
        }

        // Shuffle the deck in the database
        private void ShuffleButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string shuffleQuery = @"
                        WITH CTE AS (
                            SELECT *, NEWID() AS RandomOrder
                            FROM Cards
                        )
                        UPDATE Cards
                        SET Rank = CTE.Rank, Suit = CTE.Suit
                        FROM CTE
                        WHERE Cards.Id = CTE.Id";
                    using (SqlCommand command = new SqlCommand(shuffleQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
                LoadDeck();
                MessageBox.Show("Deck shuffled!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error shuffling deck: {ex.Message}");
            }
        }

        // Reload and display the current deck
        private void ViewDeckButtonClick(object sender, RoutedEventArgs e)
        {
            LoadDeck();
            MessageBox.Show("Deck reloaded and displayed.");
        }

        // Exit the application
        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Keyboard shortcut for resetting the deck
        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                ResetButtonClick(sender, e);
            }
        }

        // Display information about the application
        private void AboutCommand(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Advanced Deck Builder\nVersion 1.0\n© 2024 Kritika Kapri", "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Display help instructions
        private void HelpCommand(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Use this application to manage your deck of cards. Add, shuffle, deal, and reset cards.\n\nTo add a custom card, enter the suit and rank, then click 'Add Custom'.\nTo save or load your deck, use the options in the File menu.", "Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}

    

