# Tjugoett

Ett konsolbaserat kortspel i C# där en till fem spelare ska spela tjugoett mot en givare. Programmet utvecklas gemensamt som en del av en kurs i programmering och objektorienterad utveckling.

## Struktur

- `Card`, `Suit` och `Rank` beskriver korten.
- `Deck` bygger kortleken, blandar den med Fisher–Yates, delar ut kort med `DealCard()` och har `PrepareForNextRound()` för att skapa och blanda en ny lek när färre än 15 kort återstår.
- `Hand` håller en deltagares kort och ansvarar för att räkna handens värde och handstatus.
- `Participant` innehåller gemensamma egenskaper för `Player` och `Dealer`.
- `Round` och `Game` är avsedda att hantera spelrundan respektive spelets flöde.
- `ConsoleUi` hanterar konsolmeny och användarinmatning.
- `CardRenderer` bygger en spelkortsmall som rader av text för konsolen.

## Status

Spelarna kan registreras med namn; antalet begränsas till 1–5. `Player` ärver från `Participant` och håller statistik för vinster, förluster och blackjacks. `RecordOutcome(Outcome)` uppdaterar statistiken: en vanlig vinst ökar `Wins`, en förlust ökar `Losses`, en blackjack ökar både `Wins` och `Blackjacks`, och oavgjort ändrar inga av räknarna.

`Hand` räknar handens värde och hanterar ess som 1 eller 11. Den innehåller även statuskontroller för mjuk hand, bust och blackjack samt `Clear()` för att tömma handen.

`Round` har grundläggande stöd för att ge en deltagare kort och kontrollera om handen gått över 21. Spelets fullständiga rundflöde är ännu inte färdigkopplat till `ConsoleUi`.

## Nästa steg

- Färdigställa `Round` med utdelning, spelarturer, givartur och utfallsberäkning.
- Koppla rundflödet till `Game` och `ConsoleUi`, inklusive visning av handvärden.
- Anropa `Deck.PrepareForNextRound()` mellan rundor.
- Koppla `CardRenderer` till konsolens kortvisning och prova utseendet.

Projektet byggdes efter ändringen av spelarstatistiken med 0 fel. Bygget rapporterade en varning om att `Dealer.HoleCardRevealed` inte används.

Arbetet sker på feature-brancher och integreras till `dev` via pull requests och kodgranskning.
