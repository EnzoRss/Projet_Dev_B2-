<?php
require_once ($_SERVER["DOCUMENT_ROOT"]."/API/Service/Credentials.php");
class Connexion {

    public PDO $dbh;

    public function __construct($name){
        $credential = new Credentials("phpadmin.json");
        $this->dbh = new  \PDO("mysql:host=localhost;dbname=".$name,$credential->username ,$credential->password);
    }

}

//Quel est l'enrte deux entre ton tableau issue du json et l'utilisation de la donnée ici.