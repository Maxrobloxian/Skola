<?php
include_once 'logic.php';
?>

<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
    <link rel="stylesheet" href="style.css">
</head>

<body>
    <div class="help">
        <h3>Příkazy:</h3>
        <p>Krok &ThinSpace;_</p>
        <p>Reset</p>
        <p>Vlevobok &ThinSpace;_</p>
        <p>Vpravobok &ThinSpace;_</p>
        <p>Poloz &ThinSpace;_ &ThickSpace; - cislo nebo pismeno -</p>
        <p style="background-color: rgb(150, 150, 150, .1); border-radius: 8px 8px 0 0;">Poloz color red </p>
        <p style="background-color: rgb(150, 150, 150, .1);">&ThickSpace; - vsechny barvy slovy z CSS -</p>
        <p style="background-color: rgb(150, 150, 150, .1);">&ThickSpace; - nebo RGB -</p>
        <p style="background-color: rgb(150, 150, 150, .1); text-align: center;">rgb(0,0,0)</p>
        <p style="background-color: rgb(150, 150, 150, .1); text-align: center;">red</p>
        <p style="background-color: rgb(150, 150, 150, .1); text-align: center;">green</p>
        <p style="background-color: rgb(150, 150, 150, .1); text-align: center;">blue</p>
        <p style="background-color: rgb(150, 150, 150, .1); text-align: center;">...</p>
        <p style="background-color: rgb(150, 150, 150, .1); border-radius: 0 0 8px 8px; text-align: center;">no #fff</p>
    </div>
    <div class="commands">
        <form action="<?php
        !isset($_GET["x"]) && !isset($_GET["y"]) ? 'logic.php' : 'logic.php?x=' . $_GET["x"] . '&y=' . $_GET["y"];
        ?>" method="POST">
            <textarea name="text" placeholder="Write code :D"></textarea>
            <button type="submit" name="submit">Run</button>
        </form>
        <?php if (isset($_GET["tests"])) echo "
        <form action='test.php?x=0&y=0' method='POST' class='test'>
            <textarea name='text' placeholder='Test code'></textarea>
            <button type='submit' name='submit'>Run</button>
        </form>";
        ?>
    </div>
    <div class="field">
        <?php
        isset($_GET["x"]) && isset($_GET["y"]) ? SetPlayer($_GET["x"], $_GET["y"]) : SetPlayer(0, 0);
        MakeGrid();
        ?>
    </div>
    <a href="index.php?tests=true" class="underScene"> Under the scene</a>
</body>

</html>