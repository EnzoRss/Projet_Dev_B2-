<?php

class Utils
{
    function __construct()
    {

    }
    Public function ArrayKeyOnly($data){
        $realdata = [];
        foreach ($data as $cle => $valeur) {
            if (is_string($cle)) {
                $realdata[$cle] = $valeur;
            }
        }

        return $realdata;
    }

    Public function ArrayKeyOnly2D($data){
        $realdata = [];
        foreach ($data as $array) {
            // Initialiser un nouveau tableau pour chaque array
            $newArray = [];

            // Filtrer uniquement les clés associatives pour cet array
            foreach ($array as $cle => $valeur) {
                if (is_string($cle)) {
                    $newArray[$cle] = $valeur;
                }
            }

            // Ajouter le nouvel array au tableau de résultats
            $realdata[] = $newArray;
        }
        return $realdata;
    }
}
