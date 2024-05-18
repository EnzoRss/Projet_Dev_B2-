<?php

require_once($_SERVER["DOCUMENT_ROOT"] . "/API/Service/Service.php");
require_once($_SERVER["DOCUMENT_ROOT"] . "/API/DataBaseHandler.php");

class SelectDataJoin extends Service {

    function Trig() {
        $args = $this->GetArgs();
        $table = $args["table"];
        $col = $args["col"] ?? "*";
        $key = $args["key"];
        $filter = isset($args["filter"]) ?json_decode($args["filter"],true) : "";
        $dbh = new DataBaseHandler();
        $ans = $dbh->SelectDataJoin($table, $col,$key,$filter);
        print_r($ans);
    }

    static function EndPoint() {
        new SelectDataJoin();
    }
}
