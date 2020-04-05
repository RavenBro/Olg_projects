try:
    BLACK = (0, 0, 0)
    WHITE = (255, 255, 255)
    BLUE = (0, 0, 255)
    GREEN = (0, 255, 0)
    RED = (255, 0, 0)
    import math
    import pygame
    import traceback
    import random
    import copy

    #Глобалки для мира
    size = (800,800)
    window = pygame.display.set_mode(size)
    pygame.display.set_caption("My Game")
    screen = pygame.Surface(size)
    gamescr = pygame.Surface((800,800))
    clock = pygame.time.Clock()
    ImLib = {
        'man' : pygame.image.load('man.png'),
        'block' : pygame.image.load('blocks.png'),
        'door' : pygame.image.load('door.png'),
        'key'  : pygame.image.load('key.png'),
        'lock'  : pygame.image.load('lock.png'),
        'blood' : pygame.image.load('blo.png'),
        'text' : pygame.image.load('text.png'),
        'in' : pygame.image.load('in.png')
    }   

    #Игровые глобалки
    map = [[1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1],
           [0,1,0,0,0,2,2,0,2,2,0,0,0,0,1,0],
           [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],
           [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0],
           [0,0,0,0,1,1,1,1,1,1,1,0,0,0,0,0],
           [0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0],
           [0,1,0,0,0,0,0,0,0,0,0,0,0,2,1,0],
           [0,1,0,0,0,0,0,0,0,0,0,0,1,1,1,0],
           [0,1,0,0,0,2,2,0,0,0,0,0,0,0,2,0],
           [0,1,0,1,1,1,1,1,1,0,0,0,0,0,0,0],
           [0,1,0,0,0,0,0,0,0,1,0,0,0,0,0,1],
           [0,1,0,0,0,0,0,0,0,0,1,0,0,0,0,0],
           [0,1,0,0,0,0,0,0,0,0,0,0,0,0,1,0],
           [0,1,1,0,0,0,0,0,0,0,0,0,0,1,1,0],
           [0,1,1,1,0,2,2,2,2,0,1,1,2,1,1,1],
           [0,0,0,0,1,1,1,1,1,1,1,1,1,0,0,1], ]
    downs = []
    rotl = False
    sav = []
    printscr = pygame.Surface((800,800))
    oldic = 0 
    start = pygame.Surface((800,800))
    finish = pygame.Surface((800,800))
    counter = 0

    def mirror(mtx):
        res = []
        for i in range(len(mtx)):
            a = mtx[i]
            a.reverse()
            res.append(a)
        res.reverse()
        return res

    def flo(who):
        return [[round((who.y+100)/50),math.floor((who.x+40)/50)],
        [round((who.y+100)/50),math.floor((who.x+50)/50)],
        [round((who.y+100)/50),math.floor((who.x+60)/50)]]

    def wall(m,side,shift):
        
        if(side == 'right'):
            return([math.floor((m.x+100-shift)/50),math.floor((m.y+10)/50),math.floor((m.y+50)/50),math.floor((m.y+99)/50)])
        else:
            return([math.floor((m.x+shift)/50),math.floor((m.y+10)/50),math.floor((m.y+50)/50),math.floor((m.y+99)/50)])
    def bumpDetect(m,self,shift):
        t = wall(man,'right',shift)
        t2 = wall(man,'left',shift)
        if((self.side == 'right' and (m[t[1]][t[0]] != math.fabs(1-self.color) or m[t[2]][t[0]] != math.fabs(1-self.color) or m[t[3]][t[0]] != math.fabs(1-self.color))) or
            (self.side == 'left' and (m[t2[1]][t2[0]] != math.fabs(1-self.color) or m[t2[2]][t2[0]] != math.fabs(1-self.color) or m[t2[3]][t2[0]] != math.fabs(1-self.color))) or
            (self.x >= 650+shift and self.side == 'right') or (self.x <= 50-shift and self.side == 'left')):
            return True
        else:
            return False

    def up(m):
        return [math.floor(m.y/50),math.floor((m.x+40)/50),math.floor((m.x+60)/50)]

    class Door:
        def __init__(self,x,y,keys):
            self.x = x
            self.y = y
            self.key = keys
        def draw(self):
            
            a = [ImLib['door'],ImLib['key'],ImLib['lock'].subsurface(0,0,50,50),ImLib['lock'].subsurface(50,0,50,50),]
            x1 = self.x
            y1 = self.y
            v = self.key
            global change
            if((man.state == 'reverse' and ((man.color == 0 and man.scaler < 100+change) or (man.color == 1 and man.scaler > 100))) or (man.color == 0 and man.state != 'reverse')):
                x1 = 800 - x1 - 50
                y1 = 800 - y1 - 100
                
                v = [800 - self.key[0] - 50,800-self.key[1] - 50] 
                a = [pygame.transform.flip(a[i],0,1) for i in range(len(a))]
            global e
            global v
            e = (x1,y1)
            gamescr.blit(a[0],e)
            if(self.key[0] >= 0):
                gamescr.blit(a[1],v)
                gamescr.blit(a[2],(e[0]+5,e[1]+20))
            else:
                gamescr.blit(a[3],(e[0],e[1]+20))


    class Man:
        def __init__(self,x,y):
            self.x = x
            self.y = y
            self.color = 1
            self.animat = 0
            self.animloop = 36
            self.scaler = 0
            self.an = 0
            self.oldcol = 0
            self.state = ''
            self.side = ''
            self.speed = 2

            self.fallsp = 0.13
            self.maxsp = 8
            self.fulljumpsp = 3.5
            self.jumpsp = 1.5
            self.upsp = -5

            self.speedX = self.speed
            self.speedY = 0
            
        def draw(self):
            whattodraw = ImLib['man'].subsurface(math.floor(self.animat/(self.animloop/6))*100,0,100,100)
            
            #Проверка на ключ и дверь
            if((man.x - door.key[0] < 20 and man.x-door.key[0] > -70) and (man.y - door.key[1] < 50 and man.y-door.key[1] > -100)):
                door.key[0] -= 1000
            if(math.fabs(man.x+25-door.x)<15 and math.fabs(man.y-door.y)<10 and door.key[0] < 0):
                #А ЗДЕСЬ ВЫРЕЗАТЬ КОСТЫЛЬ!
                gamescr.blit(ImLib['in'],(250,250))


            if(self.state == 'stay' or self.state == 'go' or self.state == ''):
                self.state = ''
                for i in downs:
                    if(i == 100):
                        self.state = 'go'
                        self.side = 'right'
                    elif(i == 97):
                        self.state = 'go'
                        self.side = 'left'
                    elif(i == 119):
                        n = up(man)
                        if(map[n[0]-1][n[1]] != self.color and map[n[0]-1][n[2]] != self.color):
                            self.state = 'jump'
                            self.speedX = self.fulljumpsp
                            self.speedY = self.upsp
                        else:
                            self.state = 'stay'
                    elif(i == 115):
                        k = flo(self)
                        k2 = flo(Man(self.x,self.y+50))
                        if(map[k[0][0]][k[0][1]] == self.color and map[k[1][0]][k[1][1]] == self.color and 
                            map[k[2][0]][k[2][1]] == self.color and map[k2[0][0]][k2[0][1]] == self.color and 
                            map[k2[1][0]][k2[1][1]] == self.color and map[k2[2][0]][k2[2][1]] == self.color):
                            self.state = 'reverse'
                            global sav
                            self.oldcol = self.color
                            sav = [man.x+100,man.y+200]
                        else:
                            self.state = 'stay'

                if(self.state != 'reverse' and self.state != 'jump' and (downs.count(100)+downs.count(97)+downs.count(115)+downs.count(119) == 0 or bumpDetect(map,man,24))):
                    self.state = 'stay'

                k = flo(man)
                
                l = math.fabs(1-self.color)
                if(map[k[1][0]][k[1][1]] ==l and map[k[0][0]][k[0][1]] == l and map[k[2][0]][k[2][1]] == l):
                    self.state = 'jump'
                    self.speedX = self.jumpsp

                

                if(self.state == 'go'):
                    self.animat += 1
                    if (self.side == 'right'):
                        self.x+=self.speedX                    
                    elif(self.side == 'left'):
                        self.x-=self.speedX              
                    
                    if(self.animat >= self.animloop):
                        self.animat = 0

                elif(self.state == 'stay'):
                    whattodraw = ImLib['man'].subsurface(1600-900*self.color,0,100,100)



            elif(self.state == 'jump'):
                whattodraw = ImLib['man'].subsurface(600,0,100,100)
                self.speedY += self.fallsp

                if(self.speedY > self.maxsp):
                    self.speedY = self.maxsp
                #Проверка на столкновение со стеной
                if(bumpDetect(map,man,35)):
                    #print('начало вызова')
                    self.speedX = 0
                if(self.speedX == 0 and self.speedY < 0 and bumpDetect(map,man,35) == False):
                    self.speedX = self.fulljumpsp*0.5
                #Проверка на потолок
                k = up(self)
                if((map[k[0]][k[1]] == self.color or map[k[0]][k[2]] == self.color) and self.speedY < 0):
                    self.speedY = -0.1 * self.speedY

                if(self.side == 'left'):
                    self.x -= self.speedX
                else:
                    self.x += self.speedX
                self.y += self.speedY

                #Проверка на остановку
                if(math.fabs((self.y % 50) - 50) < 7):
                    oldy = self.y
                    self.y = round(self.y/50)*50
                    k = flo(man)
                    l = self.color
                    if((map[k[1][0]][k[1][1]]==l or map[k[0][0]][k[0][1]]==l or map[k[2][0]][k[2][1]]==l) and self.speedY>0):
                        self.state = 'go'
                        self.speedY = 0
                        self.speedX = self.speed
                    else:
                        self.y = oldy
            #Проверка на СМЕРТЬ
            if(self.state != 'die'):
                f = flo(man)
                r = up(man)
                if(map[f[0][0]][f[0][1]] == 2 and map[f[1][0]][f[1][1]] == 2 and map[f[2][0]][f[2][1]] == 2):
                    man.state = 'die'
                    self.scaler = 0
                    oldic = 0 
                    self.x += 50
                    self.y += 75
                elif(map[r[0]][r[1]] == 2 and map[r[0]][r[2]] == 2):
                    man.state = 'die'
                    self.scaler = 0
                    oldic = 0 
                    self.x += 50
                    self.y += 65

            if(self.color == 0):
                if(self.state == 'stay'):
                    whattodraw = ImLib['man'].subsurface(1600,0,100,100)
                elif(self.state == 'jump'):
                    whattodraw = ImLib['man'].subsurface(1500,0,100,100)
                elif(self.state == 'go'):
                    whattodraw = ImLib['man'].subsurface(math.floor(self.animat/(self.animloop/6)+9)*100,0,100,100)

            if(self.state == 'reverse'):
                global change
                change = 4
                whattodraw = ImLib['man'].subsurface(1600-self.color*900,0,100,100)
                if(self.scaler<=100-change):
                    whattodraw = pygame.transform.scale(whattodraw,(100,100-self.scaler))                    
                    self.y+=change
                    self.scaler+=change
                elif(self.scaler<=200):
                    whattodraw = pygame.transform.flip(whattodraw,0,1)
                    self.color = math.fabs(1 - self.oldcol)                    
                    whattodraw = pygame.transform.scale(whattodraw,(100,self.scaler-100))  
                    self.scaler += change
                else:
                    whattodraw = pygame.transform.flip(whattodraw,0,1)
                    global rotl 
                    rotl = True

            if(self.state == 'die'): 
                change = 15
                whattodraw = ImLib['blood']
                if(self.scaler >= 200 and self.scaler <= 200 + change*80*2):
                    global oldic   
                    whattodraw = pygame.transform.scale(whattodraw,(oldic,oldic))
                    self.scaler += change
                elif(self.scaler >= 200 + change*80):
                    global finish
                    global start

                    whattodraw = pygame.transform.scale(whattodraw,(oldic,oldic))
                    self.scaler = 0
                    oldic = 0
                    a()  
                    door.draw()    
                    if(man.side == 'left'):             
                        whattodraw = pygame.transform.flip(whattodraw,1,0)

                    gamescr.blit(whattodraw,(man.x,man.y))
                    start = copy.copy(gamescr)
                    if(self.color == 0):
                        man.x = 800 - stX
                        man.y = 800 - stY
                        global map
                        map = mirror(map)
                        self.color = 1

                    man.x = stX
                    man.y = stY
                    man.state = 'stay'
                    if(door.key[0]<0):
                        door.key[0]+= 1000
                    a()
                    man.draw()                   
                    finish = copy.copy(gamescr)
                    global gamescr
                    gamescr = copy.copy(start)
                    self.state = 'exchange'

                else:                    
                    whattodraw = pygame.transform.scale(whattodraw,(self.scaler,self.scaler))
                    self.x-=change/2
                    self.y-=change/2
                    self.scaler += change
                    if(self.scaler >= 200):
                        self.x -= change/2-0.5
                        self.y-= change/2-0.5
                    oldic = self.scaler
                


            if(self.side == 'left'):
                whattodraw = pygame.transform.flip(whattodraw,1,0)
            if(self.state != 'exchange'):
                gamescr.blit(whattodraw,(self.x,self.y)) 
                door.draw()



    man = Man(80,520)
    man.state = 'menu'
    stX = man.x
    stY = man.y
    door = Door(350,100,[150,400])

    mousek = []

    while(True):
        global downs    
        done = True
        while done:
            #ОБРАБОТКА СОБЫТИЙ
            for e in pygame.event.get():
                if e.type == pygame.QUIT:
                    pygame.quit()
                elif (e.type == pygame.KEYDOWN):
                    downs.append(e.key)
                elif (e.type == pygame.KEYUP):
                    downs.remove(e.key) 
                elif (e.type == pygame.MOUSEBUTTONDOWN):
                    global mousek
                    mousek = e.pos





            screen.fill(WHITE)
            window.blit(screen,(0,0))
            #ОТРИСОВКА МАПЫ
            def a():
                        gamescr.fill(WHITE)
                        curblock = [pygame.Surface((50,50)),pygame.Surface((50,50)),pygame.Surface((50,50))]
                        curblock[0].fill(WHITE)
                        curblock[1].fill(BLACK)
                        curblock[2].fill((60,60,60))
                        for i in range(len(map)):
                            for j in range(len(map[i])):
                                currentblock = pygame.Surface((50,50))
                                if(i == 0 or i == 15 or j == 0 or j == 15):
                                    currentblock = curblock[2]
                                else:
                                    if(map[i][j] == 0):
                                        currentblock = curblock[0]
                                    elif(map[i][j] == 1):
                                        currentblock = curblock[1]
                                    elif(map[i][j] == 2):   
                                        if(map[i-1][j] == 0):
                                            currentblock = ImLib['block'].subsurface(0,0,50,50)
                                        else:
                                            currentblock = pygame.transform.flip(ImLib['block'].subsurface(0,0,50,50),0,1)
                                gamescr.blit(currentblock,(j*50,i*50))

            if(rotl == False and man.state != 'menu'):
                if(man.state != 'exchange'):
                    
                    a()                  
                        
                    if(man.state == 'reverse' and man.scaler >= 190):
                        k = man.scaler
                        man.scaler = 199
                        man.draw()
                        printscr.blit(gamescr,(0,0))
                        man.scaler = k

                    man.draw()
                    window.blit(gamescr,(size[0]/2-400,size[1]/2-400))
                
                else:
                    man.state = 'exchange'
                    ch = 5
                    a()
                    if(counter<=255-ch):                
                        v = start
                        v.set_alpha(255-counter)
                        window.blit(v,(0,0))
                        counter += ch
                    else:
                        v = finish
                        v.set_alpha(counter-255)
                        window.blit(v,(0,0))
                        counter+=ch
                        if(counter>=610):
                            counter = 0
                            man.state = 'go'
                            a()
                    #window.blit(finish,(0,0))

            elif(rotl == True and man.state != 'menu'):
                man.an += 4
                re = pygame.Surface([800,800])
                re.fill(WHITE)
                re.blit(printscr,(1,1))
                img = pygame.transform.rotate(re,man.an)
                hz = img.get_rect(center = (400,400))
                
                
                window.blit(img,hz)
                if(man.an>=180):
                    man.an = 0
                    rotl = False
                    man.scaler = 0
                    map = mirror(map)
                    man.x = 800 - sav[0]
                    man.y = 800 - sav[1]
                    man.state = 'stay'
                    man.color = math.fabs(1 - man.oldcol)
                    
            else:
                gamescr.fill((0,0,0))
                cover = pygame.Surface((700,700))
                cover.fill((150,150,150))
                gamescr.blit(cover,(50,50))
                gamescr.blit(pygame.transform.scale(ImLib['in'],(300,300)),(250,100))
                button = (150,500)
                if(button[0] < pygame.mouse.get_pos()[0] and button[0] + 500 > pygame.mouse.get_pos()[0] and button[1] < pygame.mouse.get_pos()[1] and button[1] + 200 > pygame.mouse.get_pos()[0]):
                    gamescr.blit(ImLib['text'].subsurface(500,0,500,200),button)
                    if(mousek != []):
                        mousek = []
                        start = copy.copy(gamescr)
                        if(man.color == 0):
                            man.x = 800 - stX
                            man.y = 800 - stY
                            global map
                            map = mirror(map)
                            man.color = 1

                        man.x = stX
                        man.y = stY
                        man.state = 'stay'
                        if(door.key[0]<0):
                            door.key[0]+= 1000
                        a()
                        man.draw()                   
                        finish = copy.copy(gamescr)
                        global gamescr
                        gamescr = copy.copy(start)
                        man.state = 'exchange'
                else:
                    gamescr.blit(ImLib['text'].subsurface(0,0,500,200),button)
                window.blit(gamescr,(0,0))
            clock.tick(80)
            pygame.display.flip()
            

except: 
    traceback.print_exc()
    input()
