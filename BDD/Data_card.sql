-- Creation des decks
INSERT INTO decks (name) VALUES ('Deck Attaque'), ('Deck Defense'), ('Deck equilibre');

-- Recuperation des IDs des decks
SET @deckAttaqueId = LAST_INSERT_ID();
SET @deckDefenseId = LAST_INSERT_ID() + 1;
SET @deckEquilibreId = LAST_INSERT_ID() + 2;

-- Creation des cartes pour le Deck Attaque
INSERT INTO cards (card_name, description, pv, atk) VALUES 
    ('Chevalier Vaillant', 'Un chevalier intrepide en quete de victoire', 10, 12),
    ('Archer Aguerri', 'Un archer adroit et rapide avec son arc', 8, 10),
    ('Lancier Courageux', 'Un lancier resolu et determine sur le champ de bataille', 12, 10),
    ('Fantassin Audacieux', 'Un fantassin robuste et intrepide au combat', 7, 13),
    ('Cavalier Temeraire', 'Un cavalier intrepide menant la charge', 9, 14),
    ('eclaireur Furtif', 'Un eclaireur habile et discret sur le champ de bataille', 11, 11),
    ('Bretteur Agile', 'Un bretteur vif et agile maniant son epee', 6, 15),
    ('Piquier Intrepide', 'Un piquier determine a defendre son territoire', 13, 9),
    ('Hallebardier Intrepide', 'Un hallebardier intrepide pret a en decoudre', 5, 12),
    ('Arbaletrier emerite', 'Un arbaletrier adroit et precis dans ses tirs', 14, 8);

-- Creation des cartes pour le Deck Defense
INSERT INTO cards (card_name, description, pv, atk) VALUES 
    ('Sentinelle Vigilante', 'Une sentinelle vigilante gardant les remparts', 12, 7),
    ('ecuyer Devoue', 'Un ecuyer fidele et devoue a son seigneur', 14, 5),
    ('Archer de Reserve', 'Un archer de reserve protegeant les frontieres', 15, 4),
    ('Garde Courageux', 'Un garde courageux protegeant la citadelle', 11, 6),
    ('Defenseur Intrepide', 'Un defenseur intrepide pret a tout pour defendre', 13, 5),
    ('Lancier Endurci', 'Un lancier endurci au service de son roi', 10, 8),
    ('Capitaine Resolu', 'Un capitaine resolu menant ses troupes au combat', 12, 6),
    ('Bouclier Humain', 'Un bouclier humain protegeant ses camarades', 9, 9),
    ('Archer de Siege', 'Un archer de siege prenant position sur les remparts', 8, 10),
    ('Defenseur Valeureux', 'Un defenseur valeureux defendant sa patrie', 11, 7);

-- Creation des cartes pour le Deck equilibre
INSERT INTO cards (card_name, description, pv,atk) VALUES 
    ('Guerrier Polyvalent', 'Un guerrier polyvalent avec une formation equilibree', 9, 9),
    ('Combattant Versatile', 'Un combattant polyvalent capable de sadapter a toutes les situations', 10, 10),
    ('Brave Combattant', 'Un brave combattant avec des competences equilibrees', 11, 8),
    ('Chevalier Sans Peur', 'Un chevalier sans peur et sans reproche', 8, 11),
    ('Garde Determine', 'Un garde determine avec une force et une resistance equilibrees', 12, 7),
    ('Archer Agile', 'Un archer agile capable de tirer avec precision en mouvement', 7, 12),
    ('Hallebardier Intrepide', 'Un hallebardier intrepide pret a affronter n importe quelle menace', 13, 6),
    ('Soldat Resolu', 'Un soldat resolu avec des competences equilibrees', 6, 13),
    ('Lancier Polyvalent', 'Un lancier polyvalent capable de combattre a differentes distances', 14, 5),
    ('Fantassin Endurci', 'Un fantassin endurci capable de resister aux pires conditions', 5, 14);

-- Liaison des cartes aux decks correspondants
-- Liaison des cartes aux decks correspondants
INSERT INTO decks_cards (id_decks, id_cards)
SELECT @deckAttaqueId, id_cards FROM cards WHERE id_cards BETWEEN 1 AND 10;

INSERT INTO decks_cards (id_decks, id_cards)
SELECT @deckDefenseId, id_cards FROM cards WHERE id_cards BETWEEN 11 AND 20;

INSERT INTO decks_cards (id_decks, id_cards)
SELECT @deckEquilibreId, id_cards FROM cards WHERE id_cards BETWEEN 21 AND 30;

