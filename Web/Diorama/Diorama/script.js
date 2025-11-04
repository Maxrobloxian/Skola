window.addEventListener("load", function() {
  const canvas = document.getElementById("cnvs");
  const ctx = canvas.getContext("2d");
  const htmlBody = document.getElementById("body");
  let bodyColor1R = 118;
  let bodyColor1G = 183;
  let bodyColor1B = 212;
  let bodyColor2R = 61;
  let bodyColor2G = 122;
  let bodyColor2B = 182;
  let dropBgColorR = 118;
  let dropBgColorG = 183;
  let dropBgColorB = 212;
  let dropBgColorA = .8;
  let dropFgColorR = 158;
  let dropFgColorG = 205;
  let dropFgColorB = 241;
  let dropFgColorA = 0.5;
  let dropCount = 0;
  let drops = [];
  let player = null;
  let keys = [];
  let squares = [];
  let circles = [];
  let triangles = [];
  let worldLength = 0;
  let worldBottom = 0;
  let appleSvg = document.getElementById("apple");
  let downArrow = document.getElementById("downArrow");
  let isArrowDown = true;
  let bushObject = null;
  let rightArrow = document.getElementById("rightArrow");
  let cameraArrow = true;
  let upArrow = document.getElementById("upArrow");
  let isArrowUp = true;
  let satellite = this.document.getElementById("satellite");
  let satelliteX = 0;
  let satelliteY = 0;
  
  class Drop {
    constructor() {
      const maxRadius = 100;
      const minRadius = 10;
      this.radius = Math.random() * maxRadius + minRadius;
      this.x = Math.random() * (canvas.width - 2 * this.radius) + this.radius;
      this.y = Math.random() * (canvas.height - 2 * this.radius) + this.radius;
      this.dx = Math.random() - 0.5;
      this.dy = Math.random() - 0.5;
    }
  
    move() {
      if (this.x + this.radius > canvas.width || this.x - this.radius < 0) {
        this.dx = -this.dx;
      }
      if (this.y + this.radius > canvas.height || this.y - this.radius < 0) {
        this.dy = -this.dy;
      }
      this.x += this.dx;
      this.y += this.dy;
    }
    drawBg(ctx) {
      ctx.beginPath();
      ctx.arc(this.x, this.y, this.radius, 0, 2 * Math.PI);
      ctx.fillStyle = `rgba(${dropBgColorR}, ${dropBgColorG}, ${dropBgColorB}, ${dropBgColorA})`;
      ctx.fill();
      ctx.closePath();
    }
    drawFg(ctx) {
      ctx.beginPath();
      ctx.arc(this.x, this.y, this.radius - this.radius * .07, 0, 2 * Math.PI);
      this.gradient = ctx.createRadialGradient(this.x, this.y, this.radius * 0.2, this.x, this.y, this.radius);
      this.gradient.addColorStop(0, `rgba(128, 175, 214, .5)`); //128, 175, 214
      this.gradient.addColorStop(1, `rgba(${dropFgColorR}, ${dropFgColorG}, ${dropFgColorB}, ${dropFgColorA})`);
      ctx.fillStyle = this.gradient;
      ctx.fill();
      ctx.closePath();
    }
  }
  
  class Player {
    constructor() {
      this.radius = 100;
      this.foreground = 90;
      this.x = 535;
      this.y = 749;
      this.dx = 0;
      this.dy = 0;
      this.movementSpeed = 2;
      this.pythagoreanSpeed = Math.sqrt(this.movementSpeed**2 + this.movementSpeed**2) / 2;
      this.isColiding = [];
    }
    move(direction) {
      this.dx = 0;
      this.dy = 0;
      
      if (direction === "right") { this.dx = this.movementSpeed;
      } else if (direction === "left") { this.dx = -this.movementSpeed;
      } else if (direction === "up") { this.dy = -this.movementSpeed;
      } else if (direction === "down") { this.dy = this.movementSpeed;
      } else if (direction === "right-top") { this.dx = this.pythagoreanSpeed; this.dy = -this.pythagoreanSpeed;
      } else if (direction === "right-down") { this.dx = this.pythagoreanSpeed; this.dy = this.pythagoreanSpeed;
      } else if (direction === "left-top") { this.dx = -this.pythagoreanSpeed; this.dy = -this.pythagoreanSpeed;
      } else if (direction === "left-down") { this.dx = -this.pythagoreanSpeed; this.dy = this.pythagoreanSpeed;
      }
  
      if (this.x + this.radius + this.dx > canvas.width || this.x + this.dx < 0) { this.dx = 0; }
      if (this.y + this.radius + this.dy > canvas.height || this.y + this.dy < 0) { this.dy = 0; }
  
      this.checkCollision();
      this.x += this.dx;
      this.y += this.dy;

      this.moveCamera();
    }
    checkCollision(){
      this.isColiding = [];
      squares.forEach(square => {
        if (square.isColider || typeof square.isColider === "string") {
          if (square.x < this.x + this.dx + this.radius && square.x + square.width > this.x + this.dx && square.y < this.y + this.dy + this.radius && square.y + square.height > this.y + this.dy) {
            if (typeof square.isColider === "string") this.isColiding.push(square.isColider);
            else { this.dx = 0; this.dy = 0; }
          }
        }
      });
    }
    moveCamera() {
      if (this.x > canvas.width - 300 && this.dx > 0 && squares[squares.length - 1].x > this.x || this.x < 200 && this.dx < 0 && squares[0].x < this.x) {
        squares.forEach(square => {square.x -= this.dx;});
        circles.forEach(circle => {circle.x -= this.dx;});
        triangles.forEach(triangle => {triangle.x1 -= this.dx; triangle.x2 -= this.dx; triangle.x3 -= this.dx;});
        player.x -= this.dx;
        satelliteX -= this.dx;
        cameraArrow = false;
      }
      if (this.y > canvas.height - 200 && this.dy > 0 && this.y > worldBottom - 200 || this.y < 100 && this.dy < 0) {
        squares.forEach(square => {square.y -= this.dy;});
        circles.forEach(circle => {circle.y -= this.dy;});
        triangles.forEach(triangle => {triangle.y1 -= this.dy; triangle.y2 -= this.dy; triangle.y3 -= this.dy;});
        player.y -= this.dy;
        satelliteY -= this.dy;
        if (this.dy < 0) { decreaseSkyColor(this.dy); }
        else if (this.dy > 0) { increaseSkyColor(this.dy); }
        isArrowUp = false;
      }
    }
    drawBg(ctx) {
      ctx.beginPath();
      ctx.fillStyle = `rgb(255,255,255)`;
      ctx.fillRect(this.x, this.y, this.radius, this.radius);
      ctx.closePath();
    }
    drawFg(ctx) {
      ctx.beginPath();
      ctx.fillStyle = `rgb(255,0,0)`;
      ctx.fillRect(this.x + 5, this.y + 5, this.foreground, this.foreground);
      ctx.closePath();
    }
  }
  
  class InputHandler {
    constructor() {
      window.addEventListener("keydown", e => {
        if (keys.indexOf(e.key) === -1 && e.key !== "Alt") {
          keys.push(e.key);
        }
      });
      window.addEventListener("keyup", e => {
        keys.splice(keys.indexOf(e.key), 1);
      });
    }
  }
  
  class Square {
    constructor(x, y, width, height, color, onPlayer = true, isColider) {
      this.x = x;
      this.y = y;
      this.width = width;
      this.height = height;
      this.color = color;
      this.onPlayer = onPlayer;
      this.isColider = isColider;
    }
  
    draw(){
      ctx.beginPath();
      ctx.fillStyle = this.color;
      ctx.fillRect(this.x, this.y, this.width, this.height);
      ctx.closePath();
    }
  }

  class Circle {
    constructor(x, y, radius, color, onPlayer = false) {
      this.x = x;
      this.y = y;
      this.radius = radius;
      this.color = color;
      this.onPlayer = onPlayer;
    }
    draw() {
      ctx.beginPath();
      ctx.arc(this.x, this.y, this.radius, 0, 2 * Math.PI);
      ctx.fillStyle = this.color;
      ctx.fill();
      ctx.closePath();
    }
  }

  class Triangle {
    constructor(x1, y1, x2, y2, x3, y3, color1, color2, onPlayer = false) {
      this.x1 = x1;
      this.y1 = y1;
      this.x2 = x2;
      this.y2 = y2;
      this.x3 = x3;
      this.y3 = y3;
      this.color1 = color1;
      this.color2 = color2;
      this.onPlayer = onPlayer;
      this.gradient = null;
    }
    draw() {
      ctx.beginPath();
      ctx.moveTo(this.x1, this.y1);
      ctx.lineTo(this.x2, this.y2);
      ctx.lineTo(this.x3, this.y3);
      this.gradient = ctx.createLinearGradient(this.x2,this.y2,this.x2,this.y1);
      this.gradient.addColorStop(0, this.color2); 
      this.gradient.addColorStop(0.4, this.color1);
      ctx.fillStyle = this.gradient;
      ctx.fill();
      ctx.closePath();
    }
  }
  
  function decreaseSkyColor(amount) {
    bodyColor1R += amount * 0.5;
    bodyColor1G += amount * 0.5;
    bodyColor1B += amount * 0.5;
    bodyColor2R += amount * 0.5;
    bodyColor2G += amount * 0.5;
    bodyColor2B += amount * 0.5;
    dropBgColorR += amount * 0.5;
    dropBgColorG += amount * 0.5;
    dropBgColorB += amount * 0.5;
    dropBgColorA += amount * 0.05;
    dropFgColorR += amount * 0.5;
    dropFgColorG += amount * 0.5;
    dropFgColorB += amount * 0.5;
    dropFgColorA += amount * 0.05;
    htmlBody.style.background = `linear-gradient(rgb(${bodyColor1R}, ${bodyColor1G}, ${bodyColor1B}), rgb(${bodyColor2R}, ${bodyColor2G}, ${bodyColor2B})) fixed`;
  }
  function increaseSkyColor(amount) {
    bodyColor1R += amount * 0.5;
    bodyColor1G += amount * 0.5;
    bodyColor1B += amount * 0.5;
    bodyColor2R += amount * 0.5;
    bodyColor2G += amount * 0.5;
    bodyColor2B += amount * 0.5;
    dropBgColorR += amount * 0.5;
    dropBgColorG += amount * 0.5;
    dropBgColorB += amount * 0.5;
    dropBgColorA += amount * 0.05;
    dropFgColorR += amount * 0.5;
    dropFgColorG += amount * 0.5;
    dropFgColorB += amount * 0.5;
    dropFgColorA += amount * 0.05;
    htmlBody.style.background = `linear-gradient(rgb(${bodyColor1R}, ${bodyColor1G}, ${bodyColor1B}), rgb(${bodyColor2R}, ${bodyColor2G}, ${bodyColor2B})) fixed`;
  }

  function resizeCanvas() {
    canvas.width = window.innerWidth;
    canvas.height = window.innerHeight;
    dropCount = Math.round((canvas.width * canvas.height) / 25000);
    drops = [];
    for (let i = 0; i < dropCount; i++) {
      drops.push(new Drop());
    }
  }
  window.onresize = resizeCanvas;
  resizeCanvas();
  satelliteX = canvas.width - 100;
  
  function animateCanvas() {
    ctx.clearRect(0, 0, canvas.width, canvas.height);
  
    drops.forEach((drop) => drop.drawBg(ctx));
    drops.forEach((drop) => drop.drawFg(ctx));
    drops.forEach((drop) => drop.move());

    drawSatellite();
  
    moveDirection();
  
    triangles.forEach((triangle) => {if (!triangle.onPlayer) triangle.draw()});
    squares.forEach((square) => {if (!square.onPlayer) square.draw()});
    circles.forEach((circle) => {if (!circle.onPlayer) circle.draw()});

    checkCollisions();

    player.drawBg(ctx);
    player.drawFg(ctx);

    triangles.forEach((triangle) => {if (triangle.onPlayer) triangle.draw()});
    circles.forEach((circle) => {if (circle.onPlayer) circle.draw()});
    squares.forEach((square) => {if (square.onPlayer) square.draw()});

    if (isArrowDown) { ctx.drawImage(downArrow, bushObject.x, bushObject.y - 100); }
    if (isArrowUp) { ctx.drawImage(upArrow, canvas.width / 2 - 25, 10); }
    if (cameraArrow) { ctx.drawImage(rightArrow, canvas.width - 200, 400); }

    requestAnimationFrame(animateCanvas);
  }
  
  function moveDirection() {
    if (keys.includes("d") && keys.includes("w") || keys.includes("D") && keys.includes("W") || keys.includes("ArrowRight") && keys.includes("ArrowUp")) {
      player.move("right-top");
    }else if (keys.includes("d") && keys.includes("s") || keys.includes("D") && keys.includes("S") || keys.includes("ArrowRight") && keys.includes("ArrowDown")) {
      player.move("right-down");
    }else if (keys.includes("a") && keys.includes("w") || keys.includes("A") && keys.includes("W") || keys.includes("ArrowLeft") && keys.includes("ArrowUp")) {
      player.move("left-top");
    }else if (keys.includes("a") && keys.includes("s") || keys.includes("A") && keys.includes("S") || keys.includes("ArrowLeft") && keys.includes("ArrowDown")) {
      player.move("left-down");
    }else if (keys.includes("d") || keys.includes("D") || keys.includes("ArrowRight")) {
      player.move("right");
    }else if (keys.includes("a") || keys.includes("A") || keys.includes("ArrowLeft")) {
      player.move("left");
    }else if (keys.includes("w") || keys.includes("W") || keys.includes("ArrowUp")) {
      player.move("up");
    }else if (keys.includes("s") || keys.includes("S") || keys.includes("ArrowDown")) {
      player.move("down");
    }
  }

  function checkCollisions() {
    if (player.isColiding.length > 0) {
      if  (player.isColiding == "tree") {
        squares.forEach(square => {
          if  (square.isColider === "tree") {
            ctx.drawImage(appleSvg, square.x + 100, square.y + 10);
            ctx.drawImage(appleSvg, square.x, square.y + 160);
            ctx.drawImage(appleSvg, square.x + 50, square.y + 120);
            ctx.drawImage(appleSvg, square.x + 50, square.y - 40);
            ctx.drawImage(appleSvg, square.x - 50, square.y + 110);
            ctx.drawImage(appleSvg, square.x + 130, square.y + 80);
            ctx.drawImage(appleSvg, square.x - 20, square.y + 25);
          }
        });
      }
      if (player.isColiding == "bush") {
        isArrowDown = false;
      }
    }
  }

  function drawSatellite() {
    satelliteX -= .1;
    if (satelliteX < -300) { satelliteX = canvas.width; }
    ctx.drawImage(satellite, satelliteX, satelliteY - 500, 300, 300);
  }

  new InputHandler();
  player = new Player();
  // World Start
  squares.push(new Square(200));

  // World Ground
  squares.push(new Square(0, 850, 2920, 100, `rgb(153,76,0)`, true, true));
  squares.push(new Square(0, 847, 1500, 20, `rgb(0, 255, 0)`));
  squares.push(new Square(1800, 847, 1120, 20, `rgba(0, 255, 0)`));

  squares.push(new Square(1500, 750, 300, 100, `rgb(153,76,0)`, true, true));
  squares.push(new Square(1500, 747, 100, 20, `rgb(0, 255, 0)`));
  squares.push(new Square(1700, 747, 100, 20, `rgb(0, 255, 0)`));

  squares.push(new Square(1600, 650, 100, 100, `rgb(153,76,0)`, true, true));
  squares.push(new Square(1600, 647, 100, 20, `rgb(0, 255, 0)`));

  // Bush 1
  circles.push(new Circle(300, 850, 50, `rgb(0, 200, 0)`));
  circles.push(new Circle(350, 830, 50, `rgb(0, 200, 0)`));
  circles.push(new Circle(400, 840, 50, `rgb(0, 200, 0)`));
  circles.push(new Circle(430, 840, 50, `rgb(0, 230, 0)`, true));
  circles.push(new Circle(330, 850, 50, `rgb(0, 230, 0)`, true));
  circles.push(new Circle(380, 860, 50, `rgb(0, 220, 0)`, true));
  squares.push(new Square(340, 800, 40, 50, `rgba(0,0,0,0)`, true, "bush"));
  bushObject = squares[squares.length - 1];


  // Tree 1
  squares.push(new Square(1100, 450, 100, 400, `rgb(143,66,0)`, false, "tree"));
  circles.push(new Circle(1150, 540, 150, `rgb(0, 190, 0)`));

  triangles.push(new Triangle(2050, 950, 2450, 300, 3000, 700,  `rgb(141, 148, 154)`, `rgb(176, 183, 189)`));
  triangles.push(new Triangle(2150, 950, 2800, -100, 3520, 950,  `rgb(151, 158, 164)`, `rgb(255, 255, 255)`));

  // World End
  squares.forEach((square) => {
    if (square.x + square.width > worldLength) worldLength = square.x + square.width;
    if (square.y + square.height > worldBottom) worldBottom = square.y + square.height;
  });
  squares.push(new Square(worldLength - 300));
  
  animateCanvas();
});