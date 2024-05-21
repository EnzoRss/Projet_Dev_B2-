<?php
require_once($_SERVER["DOCUMENT_ROOT"] . "/API/Service/Service.php");
require_once($_SERVER["DOCUMENT_ROOT"] . "/API/DataBaseHandler.php");

class SelectData extends Service {

    function Trig() {
        $args = $this->GetArgs();
        $table = $args["table"];
        $col = $args["col"] ?? "*";
        $filter = isset($args["filter"]) ?json_decode($args["filter"],true) : "";
        $dbh = new DataBaseHandler();
        $res =$dbh->SelectData($table,$filter,$col);
        echo  json_encode($res);
    }

    static function EndPoint() {
        new SelectData();
    }
}