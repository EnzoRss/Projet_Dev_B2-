<?php

require_once($_SERVER["DOCUMENT_ROOT"] . "/API/Service/Service.php");
require_once($_SERVER["DOCUMENT_ROOT"] . "/API/DataBaseHandler.php");

class SelectDataJoin extends Service {

    function Trig() {
        $args = $this->GetArgs();
        $table = json_decode($args["table"],true);
        $col = $args["col"] ?? "*";
        $key = json_decode($args["key"],true);
        $filter = isset($args["filter"]) ?json_decode($args["filter"],true) : "";
        $dbh = new DataBaseHandler();
        $ans = $dbh->SelectDataJoin($table, $col,$key,$filter);
        echo json_encode($ans,JSON_PRETTY_PRINT);

    }

    static function EndPoint() {
        new SelectDataJoin();
    }
}
