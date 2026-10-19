# Tjugoett

Ett konsolbaserat kortspel i C# där en till fem spelare spelar tjugoett mot en givare. Projektet utvecklas gemensamt som en del av en kurs i programmering och objektorienterad utveckling.

## Struktur

- `Card`, `Suit` och `Rank` beskriver spelkort.
- `Deck` skapar, blandar och delar ut kort. `PrepareForNextRound()` förbereder en ny blandad lek när färre än 15 kort återstår.
- `Hand` håller korten och beräknar handvärde, mjuk hand, bust och blackjack.
- `Participant` innehåller gemensamma egenskaper och beteenden för spelare och givare.
- `Player` innehåller spelarstatistik och `RecordOutcome(Outcome)` för att registrera ett utfall.
- `Round` hanterar kort till deltagare, givarens tur och jämförelsen av spelarens och givarens händer.
- `ConsoleUi` hanterar meny, inmatning, kortvisning och det aktuella spelflödet.
- `GameStorage` kan läsa och skriva spelarprofiler i textfilen `players.txt`.
- `IPlayerStrategy` och `SimplePlayerStrategy` definierar en enkel strategi, men strategin används ännu inte i det aktuella konsolflödet.

## Aktuell status

Konsolflödet kan ta emot 1–5 nya spelare, dela ut kort, låta spelaren dra eller stanna, låta givaren dra automatiskt tills handen når minst 17 och visa resultatet. Kort och handvärden visas i konsolen. `CardRenderer` finns som separat kort-renderare med en fast exempelmall för ess i hjärter. Den tar emot ett `Card`-objekt men använder det inte ännu; `ConsoleUi` använder sin egen `PrintCard`-metod.

`Player` håller antal vinster, förluster och blackjacks. Blackjack räknas i `RecordOutcome` både som vinst och som blackjack. Resultatvisningen i `ConsoleUi` använder dock för närvarande `DetermineWinner`-text; den anropar inte `RecordOutcome`, så statistiken uppdateras ännu inte av spelet.

`GameStorage` kan serialisera spelarprofiler som semikolonseparerade rader med namn, vinster, förluster och blackjacks. `LoadPlayers` återställer profilerna och avvisar rader med ogiltiga fält eller värden. `ConsoleUi` läser in profiler vid start, men erbjuder ännu inte val av sparade spelare och anropar inte `SavePlayers`. Sparad statistik bevaras därför inte genom det vanliga spelflödet ännu.

## Nästa steg

- Bestäm om `CardRenderer` ska byggas vidare för att visa valör och färg från det mottagna kortet och kopplas till `ConsoleUi`.

- Låta användaren välja mellan sparade spelare och nya spelare.
- Registrera rundans utfall med `Player.RecordOutcome` och spara profilerna när statistik ändras och när spelet avslutas.
- Hantera semikolon i spelarnamn, eftersom tecknet används som fältavgränsare i `players.txt`.
- Koppla `IPlayerStrategy`/`SimplePlayerStrategy` till spelarförloppet om strategispelare ska ingå.
- Anropa `Deck.PrepareForNextRound()` när en ny runda startas.
- Samordna ansvaret mellan `Game` och det spelflöde som för närvarande ligger i `ConsoleUi`.
- Bygga och prova lagring, spelarstatistik och spelrundans gränsfall.

Projektet integrerar gemensamt arbete via feature-brancher och pull requests till `dev`.
