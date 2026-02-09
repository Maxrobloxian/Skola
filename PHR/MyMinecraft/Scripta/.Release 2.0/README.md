# Release 2.0
### Quick log:
- Massive terrain generation overhaul
- Added biomes
  - and biome layers
  - and domain warping
- Added player avare world generation
- Revrote chunk loading
- Multiple code optimizations/relocations/fixes/...
## Kinda full log:
- Takze, udelal jsem temporarniho playera (kulicka v neby :D)
- Svet se tvori kolem playera, pokud se player hohne tak se generuje novy teren, teren co je daleko se smaze
- Dotedka se teren generoval od 0,0 k severu a na vychod, ted, se dokaze generovat kdekoliv a do jakehokoliv smeru
- Udelal jsem nejake calculacky abych mohl zpracovat lokaci playera k terenu
- Teren se ted generuje podle biomu (jsem udelal sistem pro prumerne jednoducy spusob vytvareni biomu)
- Biomy maji k sobe přiřazení layery ty se deli na treba: "voda, vzduch, podzemi, ...", taky hodne modularni a muzu jednoduse pridat nove
- Taky jsem vylepsil formu terenu pomoci domain warping (je to celkem zlozita vec na to abych rek co to dela, nejak to posouva/tvori teren)
- A hromada backendu byla presmerovana/prejmenovana(protoze jsem se v tom ztracel)/prehazena abych mohl propojit dohromady data z ruznych skriptu
## Photo
Beta 1.3 | Release 2.0
:---:|:---:
![](https://github.com/Maxrobloxian/GitImages/blob/main/MyMinecraft/Beta1.3.png) | ![](https://github.com/Maxrobloxian/GitImages/blob/main/MyMinecraft/Release2.0.png)
## Planny pro Release 3.0 -> Hratelnost
- optimizace
- multithreading
- player (konecne budu moct hodit po sevete 😅)
- interakce
- stromy :D
- a jine
