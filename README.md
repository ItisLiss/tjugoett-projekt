# Tjugoett

Ett konsolbaserat kortspel i C# där en till fem spelare ska spela tjugoett mot en givare. Programmet utvecklas gemensamt som en del av en kurs i programmering och objektorienterad utveckling.

## Struktur

- `Card`, `Suit` och `Rank` beskriver korten.
- `Deck` bygger och blandar kortleken.
- `Hand` håller en deltagares kort och räknar handens värde.
- `Participant` innehåller gemensamma egenskaper för `Player` och `Dealer`.
- `Round` och `Game` är avsedda att hantera spelrundan respektive spelets flöde.
- `ConsoleUi` hanterar konsolmeny och användarinmatning.

## Pågående arbete

En pågående user story är: **Som spelare vill jag kunna lägga till 1–5 spelare med namn, så att vi kan spela flera tillsammans.**

Spelarregistreringen frågar efter antal och namn, validerar att antalet är mellan 1 och 5 och att namnen inte är tomma. Spelarlistan visas när ett nytt spel startas. Själva spelrundan är ännu inte inkopplad.

Arbetet sker på feature-brancher och integreras till `dev` via pull requests och kodgranskning.
