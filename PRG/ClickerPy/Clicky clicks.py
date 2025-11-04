import pygame, math

pygame.init()

clock = pygame.time.Clock()
autoClickPower = 0
coins = 0
coinsPerClick = 1
deltaTime = 0
mineCooldown = 0
coinsPerSecond = 0
maxLayer = 0

circles = []
squares = []
buttons = []
texts = []
forcedTexts = []
onClickText = []
coinsPerSecondList = []
shelf = []
box = []

gameDisplay = pygame.display.set_mode((800, 600))
pygame.display.set_caption("Clicky clicks")

class Circle:
    def __init__(self, color, position, radius, name = None):
        self.color = color
        self.position = position
        self.radius = radius
        self.name = name

    def draw(self):
        self.drawing = pygame.draw.circle(gameDisplay, self.color, self.position[:2], self.radius)

    def IsColliding(self):
        mousePos = pygame.mouse.get_pos()
        return math.sqrt((mousePos[0] - self.position[0])**2 + (mousePos[1] - self.position[1])**2) <= self.radius

class Square:
    def __init__(self, color, position, size, name = None, isButton = bool):
        self.color = color
        self.position = position
        self.size = size
        self.name = name
        if (isButton) and forButton(name) == None:
            buttons.append(Button(self.name))

    def draw(self):
        self.drawing = pygame.draw.rect(gameDisplay, self.color, (self.position[0], self.position[1], self.size[0], self.size[1]))

    def IsColliding(self):
        if self.drawing.collidepoint(pygame.mouse.get_pos()): return True

class Text:
    def __init__(self, text, color, position, size, transparency = 255, name = None):
        self.text = text
        self.color = color
        self.position = position
        self.size = size
        self.transparency = transparency
        self.name = name

    def draw(self):
        tText = self.text
        font = pygame.font.Font('freesansbold.ttf', self.size)
        tText = font.render(tText, True, self.color)
        tText.set_alpha(self.transparency)
        textRect = tText.get_rect()
        textRect.center = (self.position[0], self.position[1])
        gameDisplay.blit(tText, textRect)

class Button:
    def __init__(self, name):
        self.name = name
        self.price = 0
        self.lvl = 0
        self.isOpen = False
    
    def SetPrice(self, price):
        self.price = price

    def SetState(self, state):
        self.isOpen = state

    def IncreaseLevel(self, amount):
        self.lvl += amount

    def IncreasePrice(self, times):
        self.price = round(self.price * times, 0)

class OnClickText:
    def __init__(self, mousePos):
        self.x = mousePos[0]
        self.y = mousePos[1]
        self.transparency = 255
        onClickText.append(self)

def OnClickTextAnimation():
    for text in onClickText:
        Text(str(f'{coinsPerClick}') + " coins", (0, 0, 0), (text.x, text.y, 0), 20, text.transparency).draw()
        text.y -= .1 * deltaTime
        text.transparency -= .6 * deltaTime
        if text.transparency <= 0:
            onClickText.remove(text)
    
class CoinsPerSecond:
    def __init__(self, amount, time):
        self.amount = amount
        self.time = time

def Autominer():
    global mineCooldown
    if mineCooldown <= 0:
        TouchCoins(autoClickPower)
        mineCooldown = 250
    else:
        mineCooldown -= deltaTime

def IncreaseAutoClick(times, byAmount = 0):
    global autoClickPower
    if times == 0:
        autoClickPower += byAmount
    else:
        if autoClickPower == 0:
            autoClickPower = 1
        else:
            autoClickPower *= times

def IncreaseCoinsPerClick(times, byAmount = 0):
    global coinsPerClick
    if times == 0: coinsPerClick = round(coinsPerClick + byAmount, 2)
    else: coinsPerClick = round(coinsPerClick * times, 2)

def TouchCoins(amount):
    global coins
    coins += amount
    CoinsPerSecondCalculator(amount)

def CoinsPerSecondCalculator(amount):
    global coinsPerSecond
    if amount > 0:
        coinsPerSecondList.append(CoinsPerSecond(amount, 1000))
        coinsPerSecond = 0
        for i in coinsPerSecondList:
            coinsPerSecond += i.amount
    for i in coinsPerSecondList:
            if i.time <= 0:
                coinsPerSecond -= i.amount
                coinsPerSecondList.pop(coinsPerSecondList.index(i))
            else:
                i.time -= deltaTime

def forCircle(name):
    for circle in circles:
        if circle.name == name:
            return circle

def forSquare(name):
    for square in squares:
        if square.name == name:
            return square

def forButton(name):
    for button in buttons:
        if button.name == name:
            return button

def Draw(layer = 0):
    global maxLayer, texts
    for i in circles, squares, texts:
        for j in i:
            if j.position[2] == layer: j.draw()
            if j.position[2] > maxLayer: maxLayer = j.position[2]
    if layer < maxLayer or layer == 0:
        layer += 1
        Draw(layer)

def massDelete(items = list):
    if type(items) == tuple:
        for i in reversed(circles), reversed(squares):
            for j in i:
                if j.name in items:
                    try: circles.remove(j)
                    except: squares.remove(j)

def textMassDelete(items = list):
    for i in reversed(forcedTexts):
        if i.name in items: forcedTexts.remove(i)
                
circles.append(Circle((108, 77, 59), (400, 260, 0), 160, "cookie"))
circles.append(Circle((205, 205, 205), (20, 120, 1), 13, "shop"))
circles.append(Circle((205, 205, 205), (780, 120, 1), 13, "shop2"))

squares.append(Square((0, 100, 250), (50, 500, 0), (200, 50), "but1", True))
squares.append(Square((0, 155, 255), (55, 505, 0), (190, 40)))
squares.append(Square((0, 100, 250), (550, 500, 0), (200, 50), "but2", True))
squares.append(Square((0, 155, 255), (555, 505, 0), (190, 40)))
squares.append(Square((0, 155, 225), (0, 100, 0), (40, 40), "shop", True))
squares.append(Square((0, 155, 225), (760, 100, 0), (40, 40), "shop2", True))

forButton("but1").SetPrice(50)
forButton("but2").SetPrice(50)

def main_loop():
    global coinsPerClick, coins, deltaTime, texts
    coinsPerClick = 1
    coins = 10
    forButton("but1").SetState(True)
    forButton("but2").SetState(True)
    while True:
        Autominer()
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                return False

            if event.type == pygame.MOUSEBUTTONDOWN:
                for circle in circles:
                    if circle.name == "cookie" and circle.IsColliding():
                        TouchCoins(coinsPerClick)
                        mousePos = pygame.mouse.get_pos()
                        OnClickText((mousePos[0], mousePos[1]))

                for square in squares:
                    if square.name == "but1" and square.IsColliding() and coins >= forButton("but1").price and forButton("but1").isOpen:
                        button = forButton("but1")
                        TouchCoins(-button.price)
                        button.IncreasePrice(1.5)
                        button.IncreaseLevel(1)
                        IncreaseCoinsPerClick(1.4)

                    if square.name == "but2" and square.IsColliding() and coins >= forButton("but2").price and forButton("but2").isOpen:
                        button = forButton("but2")
                        TouchCoins(-button.price)
                        button.IncreasePrice(1.5)
                        button.IncreaseLevel(1)
                        IncreaseAutoClick(1.3)

                    if square.name == "shop" and square.IsColliding():
                        if forButton("shop").isOpen == False:
                            square.position = (250, 100, 0)
                            forCircle("shop").position = (270, 120, 1)
                            squares.append(Square((50, 50, 50), (0, 0, 1), (250, 600), "shopBackground"))
                            squares.append(Square((180, 180, 180), (25, 50, 1), (200, 50), "clickUp1", True))
                            forcedTexts.append(Text(str(f'+{2:.2f}') + " per click", (240, 240, 240), (125, 77.5, 1), 20, 255, "clickUp1Text"))
                            __but = forButton("clickUp1")
                            if __but.price == 0: __but.SetPrice(50)
                            forcedTexts.append(Text(str(f'({__but.price:.2f}') + " coins)", (255, 255, 255), (125, 110, 1), 16, 128, "clickUpPrice1Text"))

                            squares.append(Square((210, 132, 50), (25, 150, 1), (200, 50), "clickUp2", True))
                            forcedTexts.append(Text(str(f'+{5:.2f}') + " per click", (240, 240, 240), (125, 177.5, 1), 20, 255, "clickUp2Text"))
                            __but = forButton("clickUp2")
                            if __but.price == 0: __but.SetPrice(100)
                            forcedTexts.append(Text(str(f'({__but.price:.2f}') + " coins)", (255, 255, 255), (125, 210, 1), 16, 128, "clickUpPrice2Text"))

                            squares.append(Square((0, 190, 0), (25, 250, 1), (200, 50), "clickUp3", True))
                            forcedTexts.append(Text(str(f'+{10:.2f}') + " per click", (240, 240, 240), (125, 277.5, 1), 20, 255, "clickUp3Text"))
                            __but = forButton("clickUp3")
                            if __but.price == 0: __but.SetPrice(250)
                            forcedTexts.append(Text(str(f'({__but.price:.2f}') + " coins)", (255, 255, 255), (125, 310, 1), 16, 128, "clickUpPrice3Text"))

                            squares.append(Square((0, 190, 255), (25, 350, 1), (200, 50), "clickUp4", True))
                            forcedTexts.append(Text(str(f'+{25:.2f}') + " per click", (240, 240, 240), (125, 377.5, 1), 20, 255, "clickUp4Text"))
                            __but = forButton("clickUp4")
                            if __but.price == 0: __but.SetPrice(500)
                            forcedTexts.append(Text(str(f'({__but.price:.2f}') + " coins)", (255, 255, 255), (125, 410, 1), 16, 128, "clickUpPrice4Text"))

                            squares.append(Square((255, 255, 0), (25, 450, 1), (200, 50), "clickUp5", True))
                            forcedTexts.append(Text(str(f'+{50:.2f}') + " per click", (10, 10, 10), (125, 477.5, 1), 20, 255, "clickUp5Text"))
                            __but = forButton("clickUp5")
                            if __but.price == 0: __but.SetPrice(1000)
                            forcedTexts.append(Text(str(f'({__but.price:.2f}') + " coins)", (255, 255, 255), (125, 510, 1), 16, 128, "clickUpPrice5Text"))

                            forButton("shop").SetState(True)
                            forButton("but1").SetState(False)
                        ############################################################
                        elif forButton("shop").isOpen:
                            square.position = (0, 100, 0)
                            forCircle("shop").position = (20, 120, 1)
                            massDelete(("shopBackground", "clickUp1", "clickUp2", "clickUp3", "clickUp4", "clickUp5"))
                            textMassDelete(("clickUp1Text", "clickUp2Text", "clickUp3Text", "clickUp4Text", "clickUp5Text",
                                            "clickUpPrice1Text", "clickUpPrice2Text", "clickUpPrice3Text", "clickUpPrice4Text", "clickUpPrice5Text"))

                            forButton("shop").SetState(False)
                            forButton("but1").SetState(True)

                    if square.name == "shop2" and square.IsColliding():
                        if forButton("shop2").isOpen == False:
                            square.position = (510, 100, 0)
                            forCircle("shop2").position = (530, 120, 1)
                            squares.append(Square((50, 50, 50), (550, 0, 1), (250, 600), "shop2Background"))
                            squares.append(Square((180, 180, 180), (575, 50, 1), (200, 50), "mineUp1", True))
                            forcedTexts.append(Text(str(f'+{1:.2f}') + " per cycle", (240, 240, 240), (675, 77.5, 1), 20, 255, "mineUp1Text"))
                            __but = forButton("mineUp1")
                            if __but.price == 0: __but.SetPrice(100)
                            forcedTexts.append(Text(str(f'({__but.price:.2f}') + " coins)", (255, 255, 255), (675, 110, 1), 16, 128, "mineUpPrice1Text"))

                            squares.append(Square((210, 132, 50), (575, 150, 1), (200, 50), "mineUp2", True))
                            forcedTexts.append(Text(str(f'+{2.5:.2f}') + " per cycle", (240, 240, 240), (675, 177.5, 1), 20, 255, "mineUp2Text"))
                            __but = forButton("mineUp2")
                            if __but.price == 0: __but.SetPrice(250)
                            forcedTexts.append(Text(str(f'({__but.price:.2f}') + " coins)", (255, 255, 255), (675, 210, 1), 16, 128, "mineUpPrice2Text"))
                            
                            squares.append(Square((0, 190, 0), (575, 250, 1), (200, 50), "mineUp3", True))
                            forcedTexts.append(Text(str(f'+{5:.2f}') + " per cycle", (240, 240, 240), (675, 277.5, 1), 20, 255, "mineUp3Text"))
                            __but = forButton("mineUp3")
                            if __but.price == 0: __but.SetPrice(500)
                            forcedTexts.append(Text(str(f'({__but.price:.2f}') + " coins)", (255, 255, 255), (675, 310, 1), 16, 128, "mineUpPrice3Text"))
                            
                            squares.append(Square((0, 190, 255), (575, 350, 1), (200, 50), "mineUp4", True))
                            forcedTexts.append(Text(str(f'+{10:.2f}') + " per cycle", (240, 240, 240), (675, 377.5, 1), 20, 255, "mineUp4Text"))
                            __but = forButton("mineUp4")
                            if __but.price == 0: __but.SetPrice(1000)
                            forcedTexts.append(Text(str(f'({__but.price:.2f}') + " coins)", (255, 255, 255), (675, 410, 1), 16, 128, "mineUpPrice4Text"))
                            
                            squares.append(Square((255, 255, 0), (575, 450, 1), (200, 50), "mineUp5", True))
                            forcedTexts.append(Text(str(f'+{20:.2f}') + " per cycle", (0, 0, 0), (675, 477.5, 1), 20, 255, "mineUp5Text"))
                            __but = forButton("mineUp5")
                            if __but.price == 0: __but.SetPrice(2000)
                            forcedTexts.append(Text(str(f'({__but.price:.2f}') + " coins)", (255, 255, 255), (675, 510, 1), 16, 128, "mineUpPrice5Text"))

                            forButton("shop2").SetState(True)
                            forButton("but2").SetState(False)
                        ############################################################
                        elif forButton("shop2").isOpen:
                            square.position = (760, 100, 0)
                            forCircle("shop2").position = (780, 120, 1)
                            massDelete(("shop2Background", "mineUp1", "mineUp2", "mineUp3", "mineUp4", "mineUp5"))
                            textMassDelete(("mineUp1Text", "mineUp2Text", "mineUp3Text", "mineUp4Text", "mineUp5Text",
                                            "mineUpPrice1Text", "mineUpPrice2Text", "mineUpPrice3Text", "mineUpPrice4Text", "mineUpPrice5Text"))

                            forButton("shop2").SetState(False)
                            forButton("but2").SetState(True)
                    
                    try:
                        if forButton("shop").isOpen:
                            if square.name == "clickUp1" and square.IsColliding() and coins >= forButton("clickUp1").price:
                                button = forButton("clickUp1")
                                TouchCoins(-button.price)
                                button.IncreasePrice(1.8)
                                button.IncreaseLevel(1)
                                IncreaseCoinsPerClick(0, 2)
                                for i in forcedTexts:
                                    if i.name == "clickUpPrice1Text":
                                        forcedTexts.remove(i)
                                        forcedTexts.append(Text(str(f'({button.price:.2f}') + " coins)", (255, 255, 255), (125, 110, 1), 16, 128, "clickUpPrice1Text"))

                            if square.name == "clickUp2" and square.IsColliding() and coins >= forButton("clickUp2").price:
                                button = forButton("clickUp2")
                                TouchCoins(-button.price)
                                button.IncreasePrice(1.8)
                                button.IncreaseLevel(1)
                                IncreaseCoinsPerClick(0, 5)
                                for i in forcedTexts:
                                    if i.name == "clickUpPrice2Text":
                                        forcedTexts.remove(i)
                                        forcedTexts.append(Text(str(f'({button.price:.2f}') + " coins)", (255, 255, 255), (125, 210, 1), 16, 128, "clickUpPrice2Text"))

                            if square.name == "clickUp3" and square.IsColliding() and coins >= forButton("clickUp3").price:
                                button = forButton("clickUp3")
                                TouchCoins(-button.price)
                                button.IncreasePrice(1.8)
                                button.IncreaseLevel(1)
                                IncreaseCoinsPerClick(0, 10)
                                for i in forcedTexts:
                                    if i.name == "clickUpPrice3Text":
                                        forcedTexts.remove(i)
                                        forcedTexts.append(Text(str(f'({button.price:.2f}') + " coins)", (255, 255, 255), (125, 310, 1), 16, 128, "clickUpPrice3Text"))

                            if square.name == "clickUp4" and square.IsColliding() and coins >= forButton("clickUp4").price:
                                button = forButton("clickUp4")
                                TouchCoins(-button.price)
                                button.IncreasePrice(1.8)
                                button.IncreaseLevel(1)
                                IncreaseCoinsPerClick(0, 25)
                                for i in forcedTexts:
                                    if i.name == "clickUpPrice4Text":
                                        forcedTexts.remove(i)
                                        forcedTexts.append(Text(str(f'({button.price:.2f}') + " coins)", (255, 255, 255), (125, 410, 1), 16, 128, "clickUpPrice4Text"))

                            if square.name == "clickUp5" and square.IsColliding() and coins >= forButton("clickUp5").price:
                                button = forButton("clickUp5")
                                TouchCoins(-button.price)
                                button.IncreasePrice(1.8)
                                button.IncreaseLevel(1)
                                IncreaseCoinsPerClick(0, 50)
                                for i in forcedTexts:
                                    if i.name == "clickUpPrice5Text":
                                        forcedTexts.remove(i)
                                        forcedTexts.append(Text(str(f'({button.price:.2f}') + " coins)", (255, 255, 255), (125, 510, 1), 16, 128, "clickUpPrice5Text"))
                    except: pass

                    try:
                        if forButton("shop2").isOpen:
                            if square.name == "mineUp1" and square.IsColliding() and coins >= forButton("mineUp1").price:
                                button = forButton("mineUp1")
                                TouchCoins(-button.price)
                                button.IncreasePrice(1.8)
                                button.IncreaseLevel(1)
                                IncreaseAutoClick(0, 1)
                                for i in forcedTexts:
                                    if i.name == "mineUpPrice1Text":
                                        forcedTexts.remove(i)
                                        forcedTexts.append(Text(str(f'({button.price:.2f}') + " coins)", (255, 255, 255), (675, 110, 1), 16, 128, "mineUpPrice1Text"))

                            if square.name == "mineUp2" and square.IsColliding() and coins >= forButton("mineUp2").price:
                                button = forButton("mineUp2")
                                TouchCoins(-button.price)
                                button.IncreasePrice(1.8)
                                button.IncreaseLevel(1)
                                IncreaseAutoClick(0, 2.5)
                                for i in forcedTexts:
                                    if i.name == "mineUpPrice2Text":
                                        forcedTexts.remove(i)
                                        forcedTexts.append(Text(str(f'({button.price:.2f}') + " coins)", (255, 255, 255), (675, 210, 1), 16, 128, "mineUpPrice2Text"))

                            if square.name == "mineUp3" and square.IsColliding() and coins >= forButton("mineUp3").price:
                                button = forButton("mineUp3")
                                TouchCoins(-button.price)
                                button.IncreasePrice(1.8)
                                button.IncreaseLevel(1)
                                IncreaseAutoClick(0, 5)
                                for i in forcedTexts:
                                    if i.name == "mineUpPrice3Text":
                                        forcedTexts.remove(i)
                                        forcedTexts.append(Text(str(f'({button.price:.2f}') + " coins)", (255, 255, 255), (675, 310, 1), 16, 128, "mineUpPrice3Text"))
                            
                            if square.name == "mineUp4" and square.IsColliding() and coins >= forButton("mineUp4").price:
                                button = forButton("mineUp4")
                                TouchCoins(-button.price)
                                button.IncreasePrice(1.8)
                                button.IncreaseLevel(1)
                                IncreaseAutoClick(0, 10)
                                for i in forcedTexts:
                                    if i.name == "mineUpPrice4Text":
                                        forcedTexts.remove(i)
                                        forcedTexts.append(Text(str(f'({button.price:.2f}') + " coins)", (255, 255, 255), (675, 410, 1), 16, 128, "mineUpPrice4Text"))

                            if square.name == "mineUp5" and square.IsColliding() and coins >= forButton("mineUp5").price:
                                button = forButton("mineUp5")
                                TouchCoins(-button.price)
                                button.IncreasePrice(1.8)
                                button.IncreaseLevel(1)
                                IncreaseAutoClick(0, 20)
                                for i in forcedTexts:
                                    if i.name == "mineUpPrice5Text":
                                        forcedTexts.remove(i)
                                        forcedTexts.append(Text(str(f'({button.price:.2f}') + " coins)", (255, 255, 255), (675, 510, 1), 16, 128, "mineUpPrice5Text"))
                    except: pass

            if event.type == pygame.KEYDOWN:
                if event.key == pygame.K_ESCAPE:
                    return False

        CoinsPerSecondCalculator(0)

        # Draw
        gameDisplay.fill((173, 216, 230))

        texts.clear()

        if forcedTexts != []:
            for i in forcedTexts:
                texts.append(i)

        texts.append(Text(str(f'{coins:.2f}') + " coins", (0, 0, 0), (400, 50, 0), 20))
        texts.append(Text(str(f'{coinsPerSecond:.2f}') + " coins/s", (100, 100, 100), (400, 65, 0), 15, 175))

        texts.append(Text("upgrade clicker " + str(forButton("but1").lvl), (0, 0, 0), (150, 480, 0), 20))
        texts.append(Text(str(forButton("but1").price), (0, 0, 0), (150, 525, 0), 20))
        texts.append(Text("per click: " + str(f'{coinsPerClick:.2f}'), (0, 0, 0), (150, 570, 0), 20))

        texts.append(Text("buy auto miner " + str(forButton("but2").lvl), (0, 0, 0), (650, 480, 0), 20))
        texts.append(Text(str(forButton("but2").price), (0, 0, 0), (650, 525, 0), 20))
        texts.append(Text("per cycle: " + str(f'{autoClickPower:.2f}'), (0, 0, 0), (650, 570, 0), 20))

        Draw()

        OnClickTextAnimation()

        pygame.display.update()
        deltaTime = clock.tick(60)

main_loop()
pygame.quit()
quit()