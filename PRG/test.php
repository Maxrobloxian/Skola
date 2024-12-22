<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
    <link rel="stylesheet" href="test.css">
</head>
<?php

isset($_GET["x"]) && isset($_GET["y"]) && ctype_digit($_GET["x"]) && ctype_digit($_GET["y"]) ? $player = ["x" => $_GET["x"], "y" => $_GET["y"]] : $player = ["x" => 0, "y" => 0];
isset($_GET["rotation"]) && ctype_digit($_GET["rotation"]) ? $rotation = $_GET["rotation"] : $rotation = 0;
isset($_POST["text"]) ? $text = $_POST["text"] : $text = "";

var_dump($player);

$text = preg_split("/\r\n|\n|\r/", $text);
var_dump($text);
echo count($text);

for ($i = 0; $i < count($text); $i++) {
    $code = explode(" ", $text[$i]);
    var_dump($code);
    if (strcasecmp($code[0], "poloz") == 0) { if (count($code) < 2) {continue;} if ($code[1] !== '') { Move($code[0], 0, $code[1]); continue; }}
    count($code) > 1 && $code[1] !== '' && ctype_digit($code[1]) ? Move($code[0], $code[1]) : Move($code[0]);
}

function Move(string $code, int $amount = 1, $placebleObj = NAN) {
    echo "<div class='player'>";
    switch ($code) {
        case "":
            break;
        case strcasecmp($code, "up") == 0:
            var_dump("up".$amount);
            break;
        case strcasecmp($code, "down") == 0:
            var_dump("down".$amount);
            break;
        case strcasecmp($code, "left") == 0:
            var_dump("left".$amount);
            break;
        case strcasecmp($code, "right") == 0:
            var_dump("right".$amount);
            break;
        case strcasecmp($code, "reset") == 0:
            var_dump("reset");
            break;
        case strcasecmp($code, "vlevokok") == 0:
            RotatePlayer(-$amount);
            break;
        case strcasecmp($code, "vpravokok") == 0:
            RotatePlayer($amount);
            break;
        case strcasecmp($code, "poloz") == 0:
            PlaceOnPlayer($placebleObj);
            break;
    }
    echo "</div>";
}

function PlaceOnPlayer($placebleObj) {
    global $gridData, $player;
    var_dump($gridData[] = $player["x"].";".$player["y"].";$placebleObj");
}