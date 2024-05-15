<?php

require_once ($_SERVER["DOCUMENT_ROOT"]. "/API/Service/Service.php");
require_once ($_SERVER["DOCUMENT_ROOT"]."/API/SecurityLib.php");
class Credentials  {
    public $username;
    public $password;

    function __construct($file) {
        $this->username =SecurityLib::GetCredential($file)->username;
        $this->password =SecurityLib::GetCredential($file)->password;

    }

}