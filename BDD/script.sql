CREATE TABLE users(
   Id_users INT AUTO_INCREMENT,
   username VARCHAR(255) NOT NULL,
   password VARCHAR(255) NOT NULL,
   PRIMARY KEY(Id_users),
   UNIQUE(username)
);

CREATE TABLE statistics(
   Id_statistics INT AUTO_INCREMENT,
   total_matchs_played INT,
   total_wins INT,
   total_looses INT,
   favorite_deck VARCHAR(50),
   Id_users INT,
   PRIMARY KEY(Id_statistics),
   FOREIGN KEY(Id_users) REFERENCES users(Id_users)
);

CREATE TABLE games_history(
   Id_games_history INT AUTO_INCREMENT,
   PRIMARY KEY(Id_games_history)
);

CREATE TABLE cards(
   Id_cards INT AUTO_INCREMENT,
   card_name VARCHAR(50),
   description VARCHAR(255),
   pv INT,
   damages INT,
   PRIMARY KEY(Id_cards)
);

CREATE TABLE decks(
   Id_decks INT AUTO_INCREMENT,
   name VARCHAR(50),
   PRIMARY KEY(Id_decks)
);


CREATE TABLE matches(
   Id_matches INT AUTO_INCREMENT,
   player1_id INT,
   player2_id INT,
   winner_id INT,
   Id_games_history INT,
   PRIMARY KEY(Id_matches),
   FOREIGN KEY(Id_games_history) REFERENCES games_history(Id_games_history)
);

CREATE TABLE users_matches(
   Id_users INT,
   Id_matches INT,
   PRIMARY KEY(Id_users, Id_matches),
   FOREIGN KEY(Id_users) REFERENCES users(Id_users),
   FOREIGN KEY(Id_matches) REFERENCES matches(Id_matches)
);

CREATE TABLE users_game_history(
   Id_users INT,
   Id_games_history INT,
   PRIMARY KEY(Id_users, Id_games_history),
   FOREIGN KEY(Id_users) REFERENCES users(Id_users),
   FOREIGN KEY(Id_games_history) REFERENCES games_history(Id_games_history)
);

CREATE TABLE users_decks(
   Id_users INT,
   Id_decks INT,
   PRIMARY KEY(Id_users, Id_decks),
   FOREIGN KEY(Id_users) REFERENCES users(Id_users),
   FOREIGN KEY(Id_decks) REFERENCES decks(Id_decks)
);

CREATE TABLE decks_cards(
   Id_cards INT,
   Id_decks INT,
   PRIMARY KEY(Id_cards, Id_decks),
   FOREIGN KEY(Id_cards) REFERENCES cards(Id_cards),
   FOREIGN KEY(Id_decks) REFERENCES decks(Id_decks)
);

CREATE TABLE cards_stats_cards(
   Id_cards INT,
   Id_stats_card INT,
   PRIMARY KEY(Id_cards, Id_stats_card),
   FOREIGN KEY(Id_cards) REFERENCES cards(Id_cards),
   FOREIGN KEY(Id_stats_card) REFERENCES stats_card(Id_stats_card)
);
