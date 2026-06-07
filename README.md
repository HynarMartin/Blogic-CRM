## Uživatelská dokumentace

Aplikace slouží jako jednoduchý CRM systém pro evidenci smluv a osob, které se na nich podílejí.

### Hlavní funkce
* **Dashboard (Domovská stránka):** Rychlý přehled všech smluv s prokliky na detaily a rychlými akcemi pro správu klientů a poradců.
* **Správa entit (CRUD):** Plnohodnotné vytváření, zobrazení, úprava a mazání u Smluv, Klientů i Poradců.
* **Vyhledávání a filtrování:** Rychlé vyhledávání záznamů v seznamech podle klíčových údajů (evidenční číslo, příjmení, e-mail).
* **Export dat:** Možnost stáhnout aktuální data smluv, zákazníků a poradců přímo do CSV souboru stisknutím jediného tlačítka na dashboardu (podpora českého kódování).

### Spuštění aplikace
1. Naklonujte si repozitář do svého počítače.
2. Otevřete projekt v prostředí Visual Studio.
3. V souboru `appsettings.json` zkontrolujte Connection String na váš lokální MS SQL Server Express.
4. V Package Manager Console spusťte příkaz `Update-Database` pro vytvoření databáze a tabulek.
5. Spusťte aplikaci.

---

## Technická dokumentace

Aplikace je postavena na moderních technologiích od Microsoftu a dodržuje architekturu MVC (Model-View-Controller).

### Použité technologie
* **Back-end:** C#, ASP.NET Core MVC
* **Front-end:** HTML5, CSS3, Razor šablony, Bootstrap
* **Databáze:** MS SQL Server Express
* **ORM:** Entity Framework Core (Code-First přístup)

### Architektura a specifika řešení
* **Databázové vazby:** Aplikace využívá jak vazby **1:N** (jeden správce spravuje více smluv, jeden klient má více smluv), tak i složitější vazbu **M:N** (smlouva může mít více dalších účastníků/poradců a poradce může figurovat na více smlouvách).
* **Validace dat:** Kontrola logiky (např. návaznost dat uzavření, platnosti a ukončení u smlouvy) je řešena čistě na backendu implementací rozhraní `IValidatableObject` přímo v datovém modelu. Tím je zajištěna robustnost a ochrana proti neplatným datům i mimo uživatelské rozhraní.
* **Export do CSV:** Generování probíhá na serveru pomocí `StringBuilderu`. Výstupní soubor obsahuje BOM (Byte Order Mark) pro kódování UTF-8, což zajišťuje bezproblémové zobrazení české diakritiky v tabulkových procesorech (např. MS Excel). Jako oddělovač je využit středník (`;`).

---
**Autor:** Martin Hýnar  
