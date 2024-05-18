<?php

class SecurityLib {
    public static function GetCredential($name) {
        $path = $_SERVER["DOCUMENT_ROOT"]."/../Credentials/" . $name;
        return json_decode(file_get_contents($path));
    }

}