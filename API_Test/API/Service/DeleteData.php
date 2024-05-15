<?php
require_once($_SERVER["DOCUMENT_ROOT"] . "/API/Service/Service.php");
require_once($_SERVER["DOCUMENT_ROOT"] . "/API/DataBaseHandler.php");

class DeleteData extends Service {

    function Trig() {
        $args = $this->GetArgs();
        $table = $args["table"];
        $filter = json_decode($args["filter"],true);
        $dbh = new DataBaseHandler();
        $dbh->DeleteData($table,$filter);
    }

    static function EndPoint() {
        new DeleteData();
    }
}

//Voici un résumé des problèmes de code PHP identifiés sur la page :
//
//Erreur d’Array Indéfini: Une erreur est survenue à cause d’une clé de tableau non définie filter dans le fichier DeleteData.php à la ligne 10.
//Erreur Fatale: Une erreur fatale de type TypeError s’est produite dans DataBaseHandler.php à la ligne 92, où la fonction count() attend un argument de type Countable|array, mais a reçu null.
//Pile d’Appels: Les erreurs se sont produites lors de l’exécution de la méthode DeleteData::EndPoint() appelée par index.php, et ont impliqué plusieurs fichiers et méthodes.
//Ces erreurs indiquent des problèmes dans le code qui nécessitent une attention immédiate pour éviter des dysfonctionnements dans l’application.