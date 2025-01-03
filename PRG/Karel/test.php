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
isset($_GET["rotation"]) && ctype_digit($_GET["rotation"]) ? $rotation = (int)$_GET["rotation"] : $rotation = 90;
isset($_POST["text"]) ? $text = $_POST["text"] : $text = "";
isset($_GET["grid"]) ? $gridData = json_decode($_GET["grid"]) : $gridData = [];

echo "player";
var_dump($player);

echo "<div class='box'>text start";
$text = preg_split("/\r\n|\n|\r/", $text);
var_dump($text);
echo "text row count: ". count($text);

for ($i = 0; $i < count($text); $i++) {
    $code = explode(" ", $text[$i]);
    var_dump($code);
    if (strcasecmp($code[0], "poloz") == 0) { if (count($code) < 2) {continue;} if ($code[1] !== '') { Move($code[0], 0, $code[1]); continue; }}
    count($code) > 1 && $code[1] !== '' && ctype_digit($code[1]) ? Move($code[0], $code[1]) : Move($code[0]);
}
echo "text end</div>";

// from 259 loops to 133 loops      :D
echo "grid";
var_dump($gridData);
MakeGrid();
//// End of line ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------

function MakeGrid() {
    global $player, $gridData;
    if ($gridData == []) {
        echo "<table>";
        for ($y = 0; $y < 9; $y++) {
            echo "<tr>";
            $player["y"] == $y ? $checkPlayer = true : $checkPlayer = false;
            for ($x = 0; $x < 9; $x++) {
                if ($checkPlayer && $x == $player["x"]) {
                    echo "<td class='player'></td>";
                } else echo "<td></td>";
            } echo "</tr>";
        } echo "</table>";
    }
    else {
        $checkLetter = false;
        $numOfLoops = 0;
        for ($i = 0; $i < count($gridData); $i++) {
            $grid[] = explode(";", $gridData[$i]);
        }
        echo "<table>";
        for ($y = 0; $y < 9; $y++) {
            echo "<tr>";
            $player["y"] == $y ? $checkPlayer = true : $checkPlayer = false;
            foreach ($grid as $row) { $numOfLoops++; if ($row[1] == $y) { $checkY = true; break; } else { $checkY = false;} }
            for ($x = 0; $x < 9; $x++) {
                $numOfLoops++;
                if ($checkY) { foreach ($grid as $row) { $numOfLoops++; if ($row[0] == $x && $row[1] == $y) { $checkLetter = true; break; } else { $checkLetter = false;} } }
                echo GenerateGridBlock($x,$y,$checkPlayer, false, $checkLetter);
            } echo "</tr>"; echo $numOfLoops."\n";
        } echo "</table>";
        echo $numOfLoops;
    }
}

function Move(string $code, int $amount = 1, $placebleObj = NAN) {
    global $player;
    echo "<div class='player'>";
    switch ($code) {
        case "":
            break;
        case strcasecmp($code, "krok") == 0:
            MovePlayer($amount);
            var_dump("krok $amount", $player);
            break;
        case strcasecmp($code, "reset") == 0:
            var_dump("reset");
            break;
        case strcasecmp($code, "vlevobok") == 0:
            RotatePlayer(-$amount);
            break;
        case strcasecmp($code, "vpravobok") == 0:
            RotatePlayer($amount);
            break;
        case strcasecmp($code, "poloz") == 0:
            PlaceOnPlayer($placebleObj);
            break;
    }
    echo "</div>";
}

function MovePlayer($amount) {
    global $player, $rotation;
    switch ($rotation) {
        case 0:
            $player["y"] -= $amount;
            break;
        case 90:
            $player["x"] += $amount;
            break;
        case 180:
            $player["y"] += $amount;
            break;
        case 270:
            $player["x"] -= $amount;
            break;
        case 360:
            $player["y"] -= $amount;
            break;
    }
    $player["y"] = max(min($player["y"], 8), 0);
    $player["x"] = max(min($player["x"], 8), 0);
}

function GenerateGridBlock($x, $y, bool $checkPlayer = false, bool $checkColor = false, bool $checkLetter = false): string {
    global $player, $gridData;
    $checkPlayer && $x == $player["x"] ? $checkPlayer = true : $checkPlayer = false;
    return "<td"
    .($checkPlayer ? " class='player'" : "")
    .($checkColor ? " style='background-color: blue;'>" : ">")
    .($checkLetter ? "A": "")."</td>";
}

function PlaceOnPlayer($placebleObj) {
    global $gridData, $player;
    var_dump($gridData[] = $player["x"].";".$player["y"].";$placebleObj");
}