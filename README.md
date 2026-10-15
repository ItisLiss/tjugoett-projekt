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

## Pågående arbete

En pågående user story är: **Som spelare vill jag kunna lägga till 1–5 spelare med namn, så att vi kan spela flera tillsammans.**

Spelarregistreringen frågar efter antal och namn, validerar att antalet är mellan 1 och 5 och att namnen inte är tomma. Spelarlistan visas när ett nytt spel startas. Själva spelrundan är ännu inte inkopplad.

Vi arbetar också med user storyn: **Som spelare vill jag se min hands värde, även när esset räknas som 1 eller 11, så att jag vet var jag står.** `Hand` håller korten och räknar ut `Value` med ess som 11 eller 1 efter behov. Egenskaper för mjuk hand, sprucken hand och naturlig tjugoett samt `Clear()` är under arbete. Värdet visas ännu inte under en spelrunda eftersom kortutdelningen och rundflödet inte är färdigkopplade. Hand-implementationen behöver fortfarande syntaxkontrolleras och testas.

Nästa steg är att bygga vidare på `Round` och koppla ihop kortutdelning, spelarens val och visning av handens värde via `ConsoleUi`.

`Deck.PrepareForNextRound()` finns nu, men behöver anropas mellan rundorna för att kontrollen av återstående kort ska användas i spelet.

Kortgrafikens `CardRenderer.Render(Card)` returnerar kortet som rader med dynamisk valör och färgsymbol. Renderaren behöver fortfarande kopplas till `ConsoleUi` och testas visuellt i konsolen.

Arbetet sker på feature-brancher och integreras till `dev` via pull requests och kodgranskning.
