<?php
require_once($_SERVER["DOCUMENT_ROOT"] . "/API/Service/Service.php");
require_once($_SERVER["DOCUMENT_ROOT"] . "/API/DataBaseHandler.php");

class DeleteData extends Service {

    function Trig() {
        $args = $this->GetArgs();
        $table = $args["table"];
        $filter = json_decode($args["filter"],true);
        $dbh = new DataBaseHandler();
        $ans = $dbh->DeleteData($table,$filter);
        print_r($ans);
    }

    static function EndPoint() {
        new DeleteData();
    }
}
