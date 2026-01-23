# Error Pages Project: "System Core Breach"

Tento repozitář obsahuje sadu vlastních chybových stránek (401, 403, 404) navržených pro webové portály. Design využívá estetiku "Cyberpunk/Terminal".

## 🎨 Volba designu a UX/UI

### Koncept
Místo tradičního omluvného tónu volíme technický, mírně "gamifikovaný" přístup. Chyba není prezentována jako selhání, ale jako systémový stav (Signal Lost, Access Denied).

### UI Prvky
* **Monospace Font:** Evokuje terminál a kód.
* **Barvy:**
    * **404 (Cyan/Modrá):**
    * **403 (Červená):**
    * **401 (Žlutá/Oranžová):**
* **Glitch Efekt:** Animace na čísle chyby udržuje stránku "živou" a naznačuje, že portál je dynamický systém, ne statický papír.
* **CRT Scanlines:**

### UX (Uživatelská zkušenost)
* **Jasná akce:** Tlačítko "Návrat do bezpečí" je nejvýraznějším interaktivním prvkem.
* **Odlehčení:** Texty jsou stručné, netechnický uživatel pochopí podstatu, technický uživatel ocení styl.

---

## 🛠️ Funkčnost a Implementace

Stránky jsou navrženy jako **Standalone HTML** (vše v jednom souboru). To zajišťuje, že se stránka načte správně i v případě, že selže načítání externích CSS nebo JS souborů z vašeho hlavního serveru.

### Struktura
Jediný soubor `error.html` lze použít pro všechny tři stavy změnou CSS třídy na wrapperu `.container`:
* `.type-404`
* `.type-403`
* `.type-401`

---

## ⚙️ Nastavení Webového Serveru

Aby se tyto stránky zobrazovaly automaticky při chybě, je nutné upravit konfiguraci serveru.

### Apache (.htaccess)
V kořenovém adresáři webu vytvořte nebo upravte soubor `.htaccess`.

Vytvořte 3 kopie HTML souboru (`401.html`, `403.html`, `404.html`) s příslušnými texty a třídami.

```apache
ErrorDocument 401 /errors/401.html
ErrorDocument 403 /errors/403.html
ErrorDocument 404 /errors/404.html
