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

Konsolflödet kan ta emot 1–5 mänskliga spelare, låta dem välja sparade profiler eller skapa nya, dela ut kort, låta spelaren dra eller stanna, låta givaren dra automatiskt tills handen når minst 17 och visa resultatet. `CardRenderer` ritar dynamiska kortframsidor och kortbaksidor. `ConsoleUi` visar spelarnas händer och givarens synliga/dolda kort sida vid sida och färgar hjärter och ruter röda.

`Player` håller antal vinster, förluster, oavgjorda rundor och blackjacks samt spelarens marker. `ConsoleUi` registrerar rundans utfall och uppdaterar statistiken och markörerna genom `RecordOutcome`.

`GameStorage` sparar spelarprofiler som semikolonseparerade rader med namn, vinster, förluster, blackjacks och marker. `LoadPlayers` läser in profilerna och avvisar rader med ogiltiga fält eller värden. `ConsoleUi` laddar profilerna vid start, låter användaren välja en sparad eller ny profil och sparar profilerna efter varje runda.

### Grys del

Gry har byggt spelarregistreringen i `ConsoleUi`, där användaren anger 1–5 spelare och namn som inte får vara tomma. I `Deck` lade Gry till `DealCard`, som delar ut nästa kort och hanterar en tom lek, samt att leken blandas om när färre än 15 kort återstår. Gry har byggt `Card` med kortets färg (`Suit`), valör (`Rank`) och värde; klädda kort är värda 10 och ess har grundvärdet 11. `Card.ToString()` ger också en kort textrepresentation med färg- och valörsymbol. I `Hand` kapslas korten in i en privat lista som exponeras skrivskyddat. Handens värde räknas dynamiskt, så ett eller flera ess kan räknas om från 11 till 1 när handen annars skulle spricka. `Hand` har även `IsSoft`, `IsBust`, `IsBlackJack`, `AddCard` och `Clear`. I `Player` finns spelarstatistiken `Wins`, `Losses` och `Blackjacks` med `private set`, så att den bara kan ändras genom `RecordOutcome`. `GameStorage` sparar och läser in spelarprofiler i `players.txt` med `SavePlayers` och `LoadPlayers`, och profilerna läses in när programmet startar. Gry har också delat upp `ConsoleUi` i två filer: `ConsoleUi-MainLoop.cs` med menyn och spelflödet och `ConsoleUi-Methods.cs` med hjälpmetoderna. Kortgrafiken och valet av sparad eller ny spelare beskrivs nedan.

#### Kortgrafik (Gry)

`CardRenderer` i `gry-grafik.cs` ritar varje kort som sju textrader. `Render(Card card)` väljer valör och färgsymbol med `switch`, där ess blir `A`, kung `♚`, dam `♛` och knekt `♞`, och färgerna blir `♥`, `♦`, `♣` och `♠`. Valören placeras i två hörn med `PadRight` och `PadLeft`. `RenderBack()` ritar kortets baksida och används för givarens dolda kort.

Renderaren returnerar bara textrader och skriver inget själv. Utskriften sker i `ConsoleUi`, där `PrintCardsSideBySide` gör om korten i en hand till textrader och `PrintCardRowsSideBySide` skriver ut dem bredvid varandra och färgar hjärter och ruter röda. Logiken för att lägga korten sida vid sida finns därmed på ett ställe och används för både spelarens hand och givarens kort.

#### Välja sparad eller ny spelare (Gry)

Gry har byggt stödet för att varje spelare kan välja mellan en sparad profil och en ny. `AddPlayers` anropar `ChooseOrCreatePlayer(int playerNumber, List<Player> playersInGame)` för varje mänsklig spelare och lägger till den `Player` som metoden returnerar. Metoderna är inte `static`, eftersom de behöver läsa instansfältet `_savedPlayers`.

`ChooseOrCreatePlayer` börjar med att bygga listan `available` med de sparade spelare som inte redan är valda i spelet, så att samma person inte kan väljas två gånger. Är listan tom skapas en ny spelare direkt. Annars visas en meny med *1. Välj befintlig spelare* och *2. Skapa ny spelare*, som upprepas tills valet är giltigt. Väljer man en befintlig spelare visar `SelectExistingPlayer(List<Player> available)` spelarna numrerade med vinster, förluster, blackjack och marker. Metoden returnerar samma objekt som ligger i `_savedPlayers` och inte en kopia, så att statistiken uppdateras på den sparade profilen.

Nya spelare skapas i `CreateNewPlayer(int playerNumber)`, som läser in namnet, skapar en `Player` och lägger till den i `_savedPlayers`. `ReadPlayerName` avvisar tomma namn, namn med `;`, som skulle förstöra formatet i `players.txt`, och namn som redan finns. Kontrollen av befintliga namn sker i `NameExists(string name)` och skiljer inte på stora och små bokstäver, så att ingen profil skrivs över.

Efter varje runda anropas `_gameStorage.SavePlayers(_savedPlayers)` i `StartNewGame`, så att nya spelare, statistik och marker sparas. Hela `_savedPlayers` sparas och inte bara spelarna i det pågående spelet, eftersom `SavePlayers` skriver över hela filen. Datorspelare läggs aldrig till i `_savedPlayers` och sparas därför inte.

## Nästa steg

- Bestäm om `CardRenderer` ska byggas vidare för att visa valör och färg från det mottagna kortet och kopplas till `ConsoleUi`.

- Låta användaren välja mellan sparade spelare och nya spelare (påbörjat av Gry, se ovan).
- Registrera rundans utfall med `Player.RecordOutcome` och spara profilerna när statistik ändras och när spelet avslutas.
- Koppla `IPlayerStrategy`/`SimplePlayerStrategy` till spelarförloppet om strategispelare ska ingå.
- Anropa `Deck.PrepareForNextRound()` när en ny runda startas.
- Samordna ansvaret mellan `Game` och det spelflöde som för närvarande ligger i `ConsoleUi`.
- Bygga och prova lagring, spelarstatistik och spelrundans gränsfall.

Projektet integrerar gemensamt arbete via feature-brancher och pull requests till `dev`.
