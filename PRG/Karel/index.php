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
    <div class="commands">
        <form action="<?php
        !isset($_GET["x"]) && !isset($_GET["y"]) ? 'logic.php' : 'logic.php?x=' . $_GET["x"] . '&y=' . $_GET["y"];
        ?>" method="POST">
            <textarea name="text" placeholder="Write code :D"></textarea>
            <button type="submit" name="submit">Run</button>
        </form>
        <form action="test.php" method="POST" class="test">
            <textarea name="text" placeholder="Write code :D"></textarea>
            <button type="submit" name="submit">Run</button>
        </form>
    </div>
    <div class="field">
        <?php
        isset($_GET["x"]) && isset($_GET["y"]) ? SetPlayer($_GET["x"], $_GET["y"]) : SetPlayer(0, 0);
        MakeGrid();
        ?>
    </div>
</body>

</html>
