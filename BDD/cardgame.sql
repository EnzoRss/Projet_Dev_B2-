-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1:3306
-- Généré le : dim. 26 mai 2024 à 20:33
-- Version du serveur : 8.0.31
-- Version de PHP : 8.0.26

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `cardgame`
--

-- --------------------------------------------------------

--
-- Structure de la table `cards`
--

DROP TABLE IF EXISTS `cards`;
CREATE TABLE IF NOT EXISTS `cards` (
  `Id_cards` int NOT NULL AUTO_INCREMENT,
  `card_name` varchar(50) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `pv` int DEFAULT NULL,
  `atk` int DEFAULT NULL,
  PRIMARY KEY (`Id_cards`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `cards`
--

INSERT INTO `cards` (`Id_cards`, `card_name`, `description`, `pv`, `atk`) VALUES
(1, 'Chevalier Vaillant', 'Un chevalier intrepide en quete de victoire', 10, 12),
(2, 'Archer Aguerri', 'Un archer adroit et rapide avec son arc', 8, 10),
(3, 'Lancier Courageux', 'Un lancier resolu et determine sur le champ de bataille', 12, 10),
(4, 'Fantassin Audacieux', 'Un fantassin robuste et intrepide au combat', 7, 13),
(5, 'Cavalier Temeraire', 'Un cavalier intrepide menant la charge', 9, 14),
(6, 'eclaireur Furtif', 'Un eclaireur habile et discret sur le champ de bataille', 11, 11),
(7, 'Bretteur Agile', 'Un bretteur vif et agile maniant son epee', 6, 15),
(8, 'Piquier Intrepide', 'Un piquier determine a defendre son territoire', 13, 9),
(9, 'Hallebardier Intrepide', 'Un hallebardier intrepide pret a en decoudre', 5, 12),
(10, 'Arbaletrier emerite', 'Un arbaletrier adroit et precis dans ses tirs', 14, 8),
(11, 'Sentinelle Vigilante', 'Une sentinelle vigilante gardant les remparts', 12, 7),
(12, 'ecuyer Devoue', 'Un ecuyer fidele et devoue a son seigneur', 14, 5),
(13, 'Archer de Reserve', 'Un archer de reserve protegeant les frontieres', 15, 4),
(14, 'Garde Courageux', 'Un garde courageux protegeant la citadelle', 11, 6),
(15, 'Defenseur Intrepide', 'Un defenseur intrepide pret a tout pour defendre', 13, 5),
(16, 'Lancier Endurci', 'Un lancier endurci au service de son roi', 10, 8),
(17, 'Capitaine Resolu', 'Un capitaine resolu menant ses troupes au combat', 12, 6),
(18, 'Bouclier Humain', 'Un bouclier humain protegeant ses camarades', 9, 9),
(19, 'Archer de Siege', 'Un archer de siege prenant position sur les remparts', 8, 10),
(20, 'Defenseur Valeureux', 'Un defenseur valeureux defendant sa patrie', 11, 7),
(21, 'Guerrier Polyvalent', 'Un guerrier polyvalent avec une formation equilibree', 9, 9),
(22, 'Combattant Versatile', 'Un combattant polyvalent capable de sadapter a toutes les situations', 10, 10),
(23, 'Brave Combattant', 'Un brave combattant avec des competences equilibrees', 11, 8),
(24, 'Chevalier Sans Peur', 'Un chevalier sans peur et sans reproche', 8, 11),
(25, 'Garde Determine', 'Un garde determine avec une force et une resistance equilibrees', 12, 7),
(26, 'Archer Agile', 'Un archer agile capable de tirer avec precision en mouvement', 7, 12),
(27, 'Hallebardier Intrepide', 'Un hallebardier intrepide pret a affronter n importe quelle menace', 13, 6),
(28, 'Soldat Resolu', 'Un soldat resolu avec des competences equilibrees', 6, 13),
(29, 'Lancier Polyvalent', 'Un lancier polyvalent capable de combattre a differentes distances', 14, 5),
(30, 'Fantassin Endurci', 'Un fantassin endurci capable de resister aux pires conditions', 5, 14);

-- --------------------------------------------------------

--
-- Structure de la table `decks`
--

DROP TABLE IF EXISTS `decks`;
CREATE TABLE IF NOT EXISTS `decks` (
  `Id_decks` int NOT NULL AUTO_INCREMENT,
  `name` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`Id_decks`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `decks`
--

INSERT INTO `decks` (`Id_decks`, `name`) VALUES
(1, 'Deck Attaque'),
(2, 'Deck Defense'),
(3, 'Deck equilibre');

-- --------------------------------------------------------

--
-- Structure de la table `decks_cards`
--

DROP TABLE IF EXISTS `decks_cards`;
CREATE TABLE IF NOT EXISTS `decks_cards` (
  `Id_cards` int NOT NULL,
  `Id_decks` int NOT NULL,
  PRIMARY KEY (`Id_cards`,`Id_decks`),
  KEY `Id_decks` (`Id_decks`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `decks_cards`
--

INSERT INTO `decks_cards` (`Id_cards`, `Id_decks`) VALUES
(1, 1),
(2, 1),
(3, 1),
(4, 1),
(5, 1),
(6, 1),
(7, 1),
(8, 1),
(9, 1),
(10, 1),
(11, 2),
(12, 2),
(13, 2),
(14, 2),
(15, 2),
(16, 2),
(17, 2),
(18, 2),
(19, 2),
(20, 2),
(21, 3),
(22, 3),
(23, 3),
(24, 3),
(25, 3),
(26, 3),
(27, 3),
(28, 3),
(29, 3),
(30, 3);

-- --------------------------------------------------------

--
-- Structure de la table `games_history`
--

DROP TABLE IF EXISTS `games_history`;
CREATE TABLE IF NOT EXISTS `games_history` (
  `Id_games_history` int NOT NULL AUTO_INCREMENT,
  PRIMARY KEY (`Id_games_history`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Structure de la table `matches`
--

DROP TABLE IF EXISTS `matches`;
CREATE TABLE IF NOT EXISTS `matches` (
  `Id_matches` int NOT NULL AUTO_INCREMENT,
  `player1_id` int DEFAULT NULL,
  `player2_id` int DEFAULT NULL,
  `winner_id` int DEFAULT NULL,
  `Id_games_history` int DEFAULT NULL,
  PRIMARY KEY (`Id_matches`),
  KEY `Id_games_history` (`Id_games_history`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Structure de la table `statistics`
--

DROP TABLE IF EXISTS `statistics`;
CREATE TABLE IF NOT EXISTS `statistics` (
  `Id_statistics` int NOT NULL AUTO_INCREMENT,
  `total_matchs_played` int DEFAULT NULL,
  `total_wins` int DEFAULT NULL,
  `total_looses` int DEFAULT NULL,
  `favorite_deck` varchar(50) DEFAULT NULL,
  `Id_users` int DEFAULT NULL,
  PRIMARY KEY (`Id_statistics`),
  KEY `Id_users` (`Id_users`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Structure de la table `users`
--

DROP TABLE IF EXISTS `users`;
CREATE TABLE IF NOT EXISTS `users` (
  `Id_users` int NOT NULL AUTO_INCREMENT,
  `username` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `Id_decks` int DEFAULT NULL,
  PRIMARY KEY (`Id_users`),
  UNIQUE KEY `username` (`username`),
  KEY `Id_decks` (`Id_decks`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `users`
--

INSERT INTO `users` (`Id_users`, `username`, `password`, `Id_decks`) VALUES
(1, 'eee', 'rrr', 2),
(3, 'yesy', 'test', 2),
(4, 'rerr', 'tttt', 3),
(5, 'enzo', 'enzo', 1),
(6, 'test', 'test', 2);

-- --------------------------------------------------------

--
-- Structure de la table `users_game_history`
--

DROP TABLE IF EXISTS `users_game_history`;
CREATE TABLE IF NOT EXISTS `users_game_history` (
  `Id_users` int NOT NULL,
  `Id_games_history` int NOT NULL,
  PRIMARY KEY (`Id_users`,`Id_games_history`),
  KEY `Id_games_history` (`Id_games_history`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------

--
-- Structure de la table `users_matches`
--

DROP TABLE IF EXISTS `users_matches`;
CREATE TABLE IF NOT EXISTS `users_matches` (
  `Id_users` int NOT NULL,
  `Id_matches` int NOT NULL,
  PRIMARY KEY (`Id_users`,`Id_matches`),
  KEY `Id_matches` (`Id_matches`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `decks_cards`
--
ALTER TABLE `decks_cards`
  ADD CONSTRAINT `decks_cards_ibfk_1` FOREIGN KEY (`Id_cards`) REFERENCES `cards` (`Id_cards`),
  ADD CONSTRAINT `decks_cards_ibfk_2` FOREIGN KEY (`Id_decks`) REFERENCES `decks` (`Id_decks`);

--
-- Contraintes pour la table `matches`
--
ALTER TABLE `matches`
  ADD CONSTRAINT `matches_ibfk_1` FOREIGN KEY (`Id_games_history`) REFERENCES `games_history` (`Id_games_history`);

--
-- Contraintes pour la table `statistics`
--
ALTER TABLE `statistics`
  ADD CONSTRAINT `statistics_ibfk_1` FOREIGN KEY (`Id_users`) REFERENCES `users` (`Id_users`);

--
-- Contraintes pour la table `users`
--
ALTER TABLE `users`
  ADD CONSTRAINT `users_ibfk_1` FOREIGN KEY (`Id_decks`) REFERENCES `decks` (`Id_decks`);

--
-- Contraintes pour la table `users_game_history`
--
ALTER TABLE `users_game_history`
  ADD CONSTRAINT `users_game_history_ibfk_1` FOREIGN KEY (`Id_users`) REFERENCES `users` (`Id_users`),
  ADD CONSTRAINT `users_game_history_ibfk_2` FOREIGN KEY (`Id_games_history`) REFERENCES `games_history` (`Id_games_history`);

--
-- Contraintes pour la table `users_matches`
--
ALTER TABLE `users_matches`
  ADD CONSTRAINT `users_matches_ibfk_1` FOREIGN KEY (`Id_users`) REFERENCES `users` (`Id_users`),
  ADD CONSTRAINT `users_matches_ibfk_2` FOREIGN KEY (`Id_matches`) REFERENCES `matches` (`Id_matches`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
