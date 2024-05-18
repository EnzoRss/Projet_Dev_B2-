<?php

require_once($_SERVER["DOCUMENT_ROOT"] . "/API/Connexion.php");

class DataBaseHandler
{
    public $Connexion;

    function __construct()
    {
        $this->Connexion = new  Connexion("dblib");
    }

    function SelectData($table, $filter, $col)
    {

        $acces = $this->Connexion->dbh;
        if ($filter == "") {
            $sql = $acces->prepare("select $col from $table");
        } elseif (count($filter) > 1) {
            $queryString = '';
            foreach ($filter as $key => $value) {
                $queryString .= "$key=$value AND ";
            }
            // Supprimer le " AND " final
            $queryString = rtrim($queryString, ' AND ');
            $sql = $acces->prepare("select $col from $table WHERE  $queryString");
        } else {
            $queryString = '';
            foreach ($filter as $key => $value) {
                $queryString .= "$key=$value";
            }
            $sql = $acces->prepare("select $col from $table WHERE $queryString");
        }
        var_dump($sql);
        $ans = $sql->execute();
        $personne = $sql->fetchAll();
        print_r($personne);
        $personne = json_decode($personne);
        return $personne;
    }

    function CreateData($table, $data)
    {
        $col = "`" . implode('`,`', array_keys($data)) . "`";
        $values = "'" . implode("','", array_values($data)) . "'";
        $acces = $this->Connexion->dbh;
        $sql = "INSERT into $table ($col) values ($values);";
        try {
            $req = $acces->prepare($sql);
            $req->execute();
        } catch (PDOException $e) {
            var_dump($e);
        }
    }



    function UpdateData($table, $data,$filter)
    {
        foreach ($data as $key => $value){
            $dataString .= `.$key.`."= '".$value ."',";
        }
        $data = rtrim($dataString, ',');
        $acces = $this->Connexion->dbh;
        $queryString = '';
        if (count($filter) > 1) {
            foreach ($filter as $key => $value) {
                $queryString .= "$key=$value AND ";
            }
            // Supprimer le " AND " final
            $queryString = rtrim($queryString, ' AND ');
            $sql = "UPDATE $table SET $dataString WHERE $filter";
        } else {
            foreach ($filter as $key => $value) {
                $queryString .= "$key=$value";
            }
            $sql = "UPDATE $table SET $dataString WHERE $filter;";
        }

            $req = $acces->prepare($sql);
            $req->execute();

    }


//    function DeleteData($table, $filter)
//    {
//
//        $acces = $this->Connexion->dbh;
//        $queryString = '';
//        if (count($filter) > 1) {
//            foreach ($filter as $key => $value) {
//                $queryString .= "$key=$value AND ";
//            }
//            // Supprimer le " AND " final
//            $queryString = rtrim($queryString, ' AND ');
//            $sql = "DELETE FROM $table WHERE $queryString;";
//        } else {
//            foreach ($filter as $key => $value) {
//                $queryString .= "$key=$value";
//            }
//            $sql = "DELETE FROM $table WHERE $queryString;";
//        }
//            $req = $acces->prepare($sql);
//            $req->execute();
//    }

    function DeleteData($table, $filter)
    {
        $acces = $this->Connexion->dbh;
        $queryString = '';

        // Vérifiez si $filter est un tableau avant d'utiliser count()
        if (is_array($filter) && count($filter) > 1) {
            foreach ($filter as $key => $value) {
                $queryString .= "$key=$value AND ";
            }
            // Supprimer le " AND " final
            $queryString = rtrim($queryString, ' AND ');
            $sql = "DELETE FROM $table WHERE $queryString;";
        } else {
            foreach ($filter as $key => $value) {
                $queryString .= "$key=$value";
            }
            $sql = "DELETE FROM $table WHERE $queryString;";
        }
        $req = $acces->prepare($sql);
        $req->execute();
    }


}