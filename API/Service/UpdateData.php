<?php
require_once($_SERVER["DOCUMENT_ROOT"] . "/API/Service/Service.php");
require_once($_SERVER["DOCUMENT_ROOT"] . "/API/DataBaseHandler.php");

class UpdateData extends Service {

    function Trig() {
        $args = $this->GetArgs();
        $table = $args["table"];
        $data = json_decode($args["data"],true);
        $filter = isset($args["filter"]) ?json_decode($args["filter"],true) : "";
        $dbh = new DataBaseHandler();
        $ans =$dbh->UpdateData($table,$data,$filter);
        var_dump($ans);
    }

    static function EndPoint() {
        new UpdateData();
    }
}