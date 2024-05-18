<?php
require_once($_SERVER["DOCUMENT_ROOT"] . "/API/Service/Service.php");
require_once($_SERVER["DOCUMENT_ROOT"] . "/API/DataBaseHandler.php");

class CreateData extends Service {

    function Trig() {
        $args = $this->GetArgs();
        $table = $args["table"];
        $data = json_decode($args["data"],true);
        var_dump($data);
        $dbh = new DataBaseHandler();
        $res = $dbh->CreateData($table,$data);
        var_dump($res);

    }

    static function EndPoint() {
        new CreateData();
    }
}