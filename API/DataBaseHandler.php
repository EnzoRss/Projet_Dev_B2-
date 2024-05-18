<?php

require_once($_SERVER["DOCUMENT_ROOT"] . "/API/Connexion.php");

class DataBaseHandler
{
    public $Connexion;

    function __construct()
    {
        $this->Connexion = new  Connexion("CardGame");
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

    Function SelectDataJoin($table,$col,$key,$filter) {
        $acces = $this->Connexion->dbh;
        $table1  = $table[0];
        $table2 = $table[1];
        $key1 = $key[0];
        $key2 = $key[1];
        $queryString = '';
        foreach ($filter as $key => $value) {
            $queryString .= "$key=$value";
        }
        $sql = $acces->prepare("select $col from $table1 join $table2 on $key1 = $key2 WHERE $queryString");
        print_r($sql);
        $sql->execute();
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
        print($sql);
        try {
            $req = $acces->prepare($sql);
            return $req->execute();
        } catch (PDOException $e) {
            var_dump($e);
        }
    }



    function UpdateData($table, $data,$filter)
    {
        $dataString = "";
        foreach ($data as $key => $value){
            //print_r($key);
            $dataString .= "`".$key."`= '".$value ."',";
        }
        $dataString = rtrim($dataString, ',');
        $acces = $this->Connexion->dbh;
        $queryString = '';
        if (count($filter) > 1) {
            foreach ($filter as $key => $value) {
                $queryString .= "$key=$value AND ";
            }
            // Supprimer le " AND " final
            $queryString = rtrim($queryString, ' AND ');
            $sql = "UPDATE $table SET $dataString WHERE $queryString";
        } else {

            foreach ($filter as $key => $value) {
                if (gettype($value) == "string")
                $queryString .= "$key= '".$value ."'";
                else {
                    $queryString .= "$key= $value";
                }
            }
            $sql = "UPDATE $table SET $dataString WHERE $queryString;";
        }
            print_r($sql);
            $req = $acces->prepare($sql);
            return $req->execute();

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
                $queryString .= "$key= '".$value ."'";
            }
            $sql = "DELETE FROM $table WHERE $queryString;";
        }
        $req = $acces->prepare($sql);
        return $req->execute();
    }


}