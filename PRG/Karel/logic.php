<?php

isset($_GET["x"]) && isset($_GET["y"]) && ctype_digit($_GET["x"]) && ctype_digit($_GET["y"]) ? $player = ["x" => $_GET["x"], "y" => $_GET["y"]] : $player = ["x" => 0, "y" => 0];
isset($_GET["rotation"]) && ctype_digit($_GET["rotation"]) ? $rotation = (int)$_GET["rotation"] : $rotation = 90;
isset($_POST["text"]) ? $text = $_POST["text"] : $text = "";
$gridData = [];

if (isset($_POST["submit"])) {
    global $player, $text;
    DecodeText($text);
    header("location: index.php?x=".$player["x"]."&y=".$player["y"]."&rotation=".$rotation."&grid=".json_encode($gridData));
}

function MakeGrid() {
    global $player;
    echo "<table>";
    for ($y = 0; $y < 9; $y++) {
        echo "<tr>";
        for ($x = 0; $x < 9; $x++) {
            if ($player["x"] == $x && $player["y"] == $y) { echo "<td name='x:$x y:$y' class='player".GetPlayerRotationClass()."'><p>A</p></td>"; }
            else { echo "<td name='x:$x y:$y'>".PlaceLetter("a")."</td>"; }
        } echo "</tr>";
    } echo "</table>";
}

function CheckGridData($x, $y): string {
    global $player;
    if ($player["x"] == $x && $player["y"] == $y) { return " class='player".GetPlayerRotationClass()."'"; }
    else { return ""; }
}

function SetPlayer($x, $y) {
    global $player;
    $player = ["x" => $x, "y" => $y];
}

function Move(string $code, int $amount = 1, $placebleObj = NAN) {
    global $player;
    switch ($code) {
        case "":
            break;
        case strcasecmp($code, "up") == 0:
            $player["y"] -= $amount;
            break;
        case strcasecmp($code, "down") == 0:
            $player["y"] += $amount;
            break;
        case strcasecmp($code, "left") == 0:
            $player["x"] -= $amount;
            break;
        case strcasecmp($code, "right") == 0:
            $player["x"] += $amount;
            break;
        case strcasecmp($code, "reset") == 0:
            $player = ["x" => 0, "y" => 0];
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
    $player["y"] = max(min($player["y"], 8), 0);
    $player["x"] = max(min($player["x"], 8), 0);
}

function DecodeText(string $string) {
    global $text;
    $text = preg_split("/\r\n|\n|\r/", $string);

    for ($i = 0; $i < count($text); $i++) {
        $code = explode(" ", $text[$i]);
        if (strcasecmp($code[0], "poloz") == 0) { if (count($code) < 2) {continue;} if ($code[1] !== '') { Move($code[0], 0, $code[1]); continue; }}
        count($code) > 1 && $code[1] !== '' && ctype_digit($code[1]) ? Move($code[0], $code[1]) : Move($code[0]);
    }
}

function GetPlayerRotationClass(): string {
    global $rotation;
    return match ($rotation) {
        0 || 360 => "Up",
        90 => "Right",
        180 => "Down",
        270 => "Left",
        default => "Up"
    };
}

function RotatePlayer(int $amount) {
    global $rotation;
    $rotation += 90 * $amount;
    while ($rotation < 0 || $rotation > 360) {
        if ($rotation < 0) { $rotation = 360 + $rotation; }
        if ($rotation > 360) { $rotation -= 360; }
    }
}

function PlaceOnPlayer($placebleObj) {
    global $gridData, $player;
    $gridData[] = $player["x"].";".$player["y"].";$placebleObj";
}

function PlaceLetter(string $letter): string {
    return "<p>".strtoupper($letter)."</p>";
}
