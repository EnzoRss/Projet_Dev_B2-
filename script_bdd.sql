-- Table des utilisateurs
users (
    id INT PRIMARY KEY AUTO_INCREMENT,
    user_type_id INT > user_types.id,
    parse_id VARCHAR(255),
    email VARCHAR(255),
    password VARCHAR(255),
    logged_in BOOLEAN,
    token_facebook VARCHAR(255),
    token_twitter VARCHAR(255),
    user_token VARCHAR(255),
    token_expiration DATETIME
);

-- Table des parties
matches (
    id INT PRIMARY KEY AUTO_INCREMENT,
    status VARCHAR(50) NOT NULL,
    player_id INT > users.id
);

-- Table des invitations
invitations (
    id INT PRIMARY KEY AUTO_INCREMENT,
    sender_id INT > users.id,
    receiver_id INT > users.id,
    status VARCHAR(50) NOT NULL
);

-- Table des statistiques
statistics (
    id INT PRIMARY KEY AUTO_INCREMENT,
    player_id INT > users.id,
    wins INT,
    losses INT
);

-- Table des connexions récentes
recent_connections (
    id INT PRIMARY KEY AUTO_INCREMENT,
    player_id INT > users.id,
    last_connection TIMESTAMP
);

-- Table des paramètres du jeu
game_settings (
    id INT PRIMARY KEY AUTO_INCREMENT,
    max_players INT,
    time_limit INT
);

-- Table des historiques
game_history (
    id INT PRIMARY KEY AUTO_INCREMENT,
    match_id INT > matches.id,
    event_description VARCHAR(255)
);


-- Table des cartes
cards (
    id INT PRIMARY KEY AUTO_INCREMENT,
    name VARCHAR(255),
    type VARCHAR(50),
    attack INT,
    defense INT,
    cost INT
);

-- Table des decks
decks (
    id INT PRIMARY KEY AUTO_INCREMENT,
    user_id INT > users.id,
    name VARCHAR(255),
    description TEXT
);

-- Table intermédiaire pour la relation entre decks et cartes (pour gérer les cartes dans un deck)
deck_cards (
    id INT PRIMARY KEY AUTO_INCREMENT,
    deck_id INT > decks.id,
    card_id INT > cards.id,
    quantity INT
);
