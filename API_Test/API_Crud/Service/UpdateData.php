<?php
require_once($_SERVER["DOCUMENT_ROOT"] . "/API/Service/Service.php");
require_once($_SERVER["DOCUMENT_ROOT"] . "/API/DataBaseHandler.php");

class UpdateData extends Service {

    function Trig() {
        $args = $this->GetArgs();
        $table = $args["table"];
        $data = $args["data"];
        $filter = isset($args["filter"]) ?json_decode($args["filter"],true) : "";
        $dbh = new DataBaseHandler();
        $dbh->UpdateData($table,$data,$filter);
    }

    static function EndPoint() {
        new UpdateData();
    }
}