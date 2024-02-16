-- Table des utilisateurs
CREATE TABLE Users (
    UserID INT PRIMARY KEY,
    Username VARCHAR(255) UNIQUE,
    PasswordHash VARCHAR(255),
    Email VARCHAR(255),
);

-- Table des parties
CREATE TABLE Matches (
    MatchID INT PRIMARY KEY,
    Status VARCHAR(50),
    -- Autres champs spécifiques à la partie
);

-- Table des invitations
CREATE TABLE Invitations (
    InvitationID INT PRIMARY KEY,
    SenderID INT,
    ReceiverID INT,
    Status VARCHAR(50),
    FOREIGN KEY (SenderID) REFERENCES Users(UserID),
    FOREIGN KEY (ReceiverID) REFERENCES Users(UserID)
);

-- Table des connexions récentes
CREATE TABLE RecentConnections (
    ConnectionID INT PRIMARY KEY,
    UserID INT,
    LastConnection DATETIME,
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);

-- Table des paramètres du jeu
CREATE TABLE GameSettings (
    SettingID INT PRIMARY KEY,
    -- Ajoutez ici les paramètres spécifiques à la partie
);

-- Table des historiques
CREATE TABLE GameHistory (
    HistoryID INT PRIMARY KEY,
    MatchID INT,
    -- Ajoutez ici les événements de la partie
    FOREIGN KEY (MatchID) REFERENCES Matches(MatchID)
);