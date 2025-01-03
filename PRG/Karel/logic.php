<?php

isset($_GET["x"]) && isset($_GET["y"]) && ctype_digit($_GET["x"]) && ctype_digit($_GET["y"]) ? $player = ["x" => $_GET["x"], "y" => $_GET["y"]] : $player = ["x" => 0, "y" => 0];
isset($_GET["rotation"]) && ctype_digit($_GET["rotation"]) ? $rotation = (int)$_GET["rotation"] : $rotation = 90;
isset($_POST["text"]) ? $text = $_POST["text"] : $text = "";
isset($_GET["grid"]) ? $gridData = json_decode($_GET["grid"]) : $gridData = [];

if (isset($_POST["submit"])) {
    global $player, $text;
    DecodeText($text);
    header("location: index.php?x=".$player["x"]."&y=".$player["y"]."&rotation=".$rotation."&grid=".json_encode($gridData));
}

function MakeGrid() {
    global $gridData;
    $gridData == [] ? MakeGridNoData() : MakeGridLoop();
}

function MakeGridLoop() {
    global $player, $gridData;
    $checkLetter = false;
    $checkColor = false;
    for ($i = 0; $i < count($gridData); $i++) {
        $grid[] = explode(";", $gridData[$i]);
    }
    echo "<table>";
    for ($y = 0; $y < 9; $y++) {
        echo "<tr>";
        $player["y"] == $y ? $checkPlayer = true : $checkPlayer = false;
        foreach ($grid as $row) { if ($row[1] == $y) { $checkY = true; break; } $checkY = false; }
        for ($x = 0; $x < 9; $x++) {
            if ($checkY) { foreach ($grid as $row) {
                if ($row[0] == $x && $row[1] == $y) {
                    if ($row[2][0] == "@" && $row[2][1] == "`") { $checkColor = true; break; } 
                    $checkColor = false;
                    $checkLetter = true;
                    break;
                }
                $checkLetter = false;} }
            echo GenerateGridBlock($x, $checkPlayer, $checkColor, $checkLetter, $row[2]);
            $checkColor = false;
        } echo "</tr>";
    } echo "</table>";
}

function GenerateGridBlock($x, bool $checkPlayer = false, bool $checkColor = false, bool $checkLetter = false, string $letter = ""): string {
    global $player;
    $checkPlayer && $x == $player["x"] ? $checkPlayer = true : $checkPlayer = false;
    return "<td"
    .($checkPlayer ? " class='player".GetPlayerRotationClass()."'" : "")
    .($checkColor ? " style='background-color:".substr($letter, 2)."'>" : ">")
    .($checkLetter ? PlaceLetter($letter): "")."</td>";
}

function MakeGridNoData() {
    global $player;
    echo "<table>";
    for ($y = 0; $y < 9; $y++) {
        echo "<tr>";
        $player["y"] == $y ? $checkPlayer = true : $checkPlayer = false;
        for ($x = 0; $x < 9; $x++) {
            if ($checkPlayer && $x == $player["x"]) {
                echo "<td class='player".GetPlayerRotationClass()."'></td>";
            } else echo "<td></td>";
        } echo "</tr>";
    } echo "</table>";
}

function SetPlayer($x, $y) {
    global $player;
    $player = ["x" => $x, "y" => $y];
    $player["y"] = max(min($player["y"], 8), 0);
    $player["x"] = max(min($player["x"], 8), 0);
}

function Move(string $code, int $amount = 1, $placebleObj = NAN) {
    switch ($code) {
        case "":
            break;
        case strcasecmp($code, "krok") == 0:
            MovePlayer($amount);
            break;
        case strcasecmp($code, "reset") == 0:
            ResetPlayer();
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
}

function DecodeText(string $string) {
    global $text;
    $text = preg_split("/\r\n|\n|\r/", $string);

    for ($i = 0; $i < count($text); $i++) {
        $code = explode(" ", $text[$i]);
        if (strcasecmp($code[0], "poloz") == 0) { if (count($code) < 2) {continue;} if ($code[1] == "color") { Move($code[0], 0, "@`$code[2]"); continue;} if ($code[1] !== '') { Move($code[0], 0, $code[1]); continue; } }
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
    if ($gridData == []) { $gridData[] = $player["x"].";".$player["y"].";$placebleObj"; return; }
    foreach ($gridData as $key) {
        if ($key[0] !== $player["x"] && $key[1] !== $player["y"]) {
            $gridData[] = $player["x"].";".$player["y"].";$placebleObj";
            return;
        }
    }
}

function PlaceLetter(string $letter): string {
    return "<p>".strtoupper(substr($letter, 0, 1))."</p>";
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

function ResetPlayer() {
    global $gridData, $rotation;
    SetPlayer(0, 0);
    $gridData = [];
    $rotation = 90;
}