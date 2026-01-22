# Beta 2.2
### Quick log:
- Added multithreading
- Player inventory
  - partial UI (hotbar only)
- Added block placement (alpha phase)
- Breaked/placed blocks are stored/removed in the inventory
- Added chunk border debug (for players as well)
- Added Chunk recycling
## Kinda full log:
- multithreading pro zmenseni lag spiku pri generovani novych chunku + pridani delaye mezi renderovani chunku pro zmenseni mnozstvi dat kdy se data vraci na main thread z side threadu
- player ma inventory (zatim jen pro bloky), muze vybirat (zatim jenom mezi 9 slotama, co jsou na hotbaru).
  - Inventory je delen (jen visualne, v realite je vsehno ukladano v array) na radky 1 radek je "hotbar". Itemy v hotbaru jsou rovnou akcesibilni pro playera (musi je mit v hotbaru aby fungovali)
- alfa faze plasovani blocku, zatim funguje sejne jako niceni ale misto niceni premeni block na block co je v inventari (duvod proc totak je, je jsem to udelal zatim jen pro testovani inventory systemu)
- niceni bloky se skladaji do inventory
- pri stisknuti "f3 + b" se objeby chunk bordery pro pripad ze nekdo hce videt konec/zacatek chunku
- predtim stary chunky byly zniceni a pak se pridaly novy (kdy se player pohybuje mezi chunky) coz je hodne CPU narozni, takze ted se pridaj jen na zacatek (dokud se dosahne cilene viditelnosti) .........
