<?php


abstract class Service{

    public function __construct(){
        $this->Trig();
    }

    public function GetArgSort(){
        return  json_decode($_GET["arr"],true);
    }

    public function GetArgs(): array {
        return $_GET;
    }

    abstract function Trig();

}


