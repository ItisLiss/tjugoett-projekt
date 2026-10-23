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

Gry har byggt spelarregistreringen i `ConsoleUi`, där användaren anger 1–5 spelare och namn som inte får vara tomma. I `Deck` lade Gry till `DealCard`, som delar ut nästa kort och hanterar en tom lek, samt att leken blandas om när färre än 15 kort återstår. Gry har byggt `Card` med kortets färg (`Suit`), valör (`Rank`) och värde; klädda kort är värda 10 och ess har grundvärdet 11. `Card.ToString()` ger också en kort textrepresentation med färg- och valörsymbol. I `Hand` kapslas korten in i en privat lista som exponeras skrivskyddat. Handens värde räknas dynamiskt, så ett eller flera ess kan räknas om från 11 till 1 när handen annars skulle spricka. `Hand` har även `IsSoft`, `IsBust`, `IsBlackJack`, `AddCard` och `Clear`. I `Player` finns spelarstatistiken `Wins`, `Losses` och `Blackjacks` med `private set`, så att den bara kan ändras genom `RecordOutcome`. `GameStorage` sparar och läser in spelarprofiler i `players.txt` med `SavePlayers` och `LoadPlayers`, och profilerna läses in när programmet startar. Rader med fel antal fält, utan namn eller med ogiltig statistik hoppas över med `continue`, så att en enda trasig rad inte hindrar spelet från att starta och övriga profiler fortfarande kan väljas. Oavgjorda (`Pushes`) räknas under spelet men sparas medvetet inte i `players.txt`, eftersom statistiken som visas för spelaren är vinster, förluster, blackjack och marker. Gry har också delat upp `ConsoleUi` i två filer: `ConsoleUi-MainLoop.cs` med menyn och spelflödet och `ConsoleUi-Methods.cs` med hjälpmetoderna. För att skärmen ska rensas ordentligt även i VS Code-terminalen lade Gry till hjälpmetoden `ClearScreen()`, som används i stället för `Console.Clear()` i `ConsoleUi`. På Windows lämnade `Console.Clear()` kvar rester av korten i VS Code-terminalen, så `ClearScreen()` skickar i stället terminalkoder som tömmer skärmen och historiken ovanför och flyttar markören högst upp – samma sätt som `Console.Clear()` fungerar på Mac och Linux. Kortgrafiken och valet av sparad eller ny spelare beskrivs nedan.

#### Kortgrafik (Gry)

`CardRenderer` i `gry-grafik.cs` ritar varje kort som sju textrader. `Render(Card card)` väljer valör och färgsymbol med `switch`, där ess blir `A`, kung `♚`, dam `♛` och knekt `♞`, och färgerna blir `♥`, `♦`, `♣` och `♠`. Valören placeras i två hörn med `PadRight` och `PadLeft`. `RenderBack()` ritar kortets baksida och används för givarens dolda kort.

Renderaren returnerar bara textrader och skriver inget själv. Utskriften sker i `ConsoleUi`, där `PrintCardsSideBySide` gör om korten i en hand till textrader och `PrintCardRowsSideBySide` skriver ut dem bredvid varandra och färgar hjärter och ruter röda. Logiken för att lägga korten sida vid sida finns därmed på ett ställe och används för både spelarens hand och givarens kort.

Under en spelares tur ritas hela bordet av `DrawPlayerTurn(Player player, Dealer dealer)`, som rensar skärmen och visar spelarens namn, givarens synliga kort och dolda baksida samt spelarens hand. Metoden anropas när turen börjar och efter varje köpt kort. Tidigare skrevs den nya handen ut under den gamla, så att "Din hand" syntes två gånger; nu ritas skärmen om i stället.

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

### Tillval / Individuell del: Husregler från konfigurationsfil (Isac)

## Vad som har implementerats:
* **Inställningar i `Config`:**
  - `ShouldHitOnSoft` – om givaren köper på mjuk 17 eller stannar.
  - `DeckCount` – hur många kortlekar som spelas med.
  - `MinDeckCardAmount` – vid hur många kvarvarande kort leken blandas om.
  - `NaturalBlackjackWin` – om naturlig tjugoett vinner över en 21 byggd av tre eller fler kort.
* **Fillagring:**
  - Inställningarna sparas i textfilen `config.txt` med `SaveConfig()` och läses in med `LoadConfig()`. Filen läses in automatiskt av en statisk konstruktor första gången `Config` används.
  - Saknas filen skapas en ny med standardvärden och användaren får ett meddelande – programmet kraschar inte.
  - Är filen trasig behålls standardvärdena och användaren får ett meddelande.
  - För låga värden höjs till minsta tillåtna värde (`MinDeckCount` och `MinCardCount`) med `Math.Max`, och användaren får ett meddelande.
  - Den privata klassen `ConfigData` används för att flytta värdena till och från filen, eftersom `JsonSerializer` inte kan spara statiska fält.
* **Användning i spelet:**
  - `Round.PlayDealerTurn()` låter givaren köpa på mjuk 17 när `ShouldHitOnSoft` är på.
  - `Deck` bygger leken med `DeckCount` lekar, och `PrepareForNextRound()` blandar om när färre än `MinDeckCardAmount` kort återstår.
  - `Round.DetermineWinner()` använder `NaturalBlackjackWin` när en naturlig tjugoett möter en 21 byggd av tre eller fler kort.
* **Inställningsmeny:**
  - `ConsoleUi.Settings()` visar de aktuella inställningarna med `Config.PrintConfig()` och låter användaren ändra dem, spara till fil eller ladda från fil.
  - Hjälpmetoderna `ReadYesNo()` och `ReadNumber()` frågar tills användaren anger ett giltigt svar.

## Tillval / Individuell del: Marker- och insatssystem (Uppgift 4)
**Ansvarig:** Lisette

### Vad som har implementerats:
* **Ekonomi och saldon:** 
  - Lagt till stöd för markersaldo (`Chips`) och aktuella insatser (`CurrentBet`) för varje spelare.
  - Implementerat en validerad insatsfas (`PlaceBet()`) i början av varje runda där marker dras från spelarens saldo. Både mänskliga spelare och datorspelare (bots) kan placera giltiga insatser baserade på sina tillgängliga tillgångar.
* **Utbetalningslogik:**
  - Standardvinst betalar **1:1** (`WinBet()` – ger tillbaka insatsen plus lika mycket i vinst).
  - Blackjack betalar **3:2** (`BlackjackPayout()` – ger tillbaka insatsen plus 1.5x insatsen i vinst).
  - Oavgjort / Push (`PushBet()` – återlämnar insatsen oavkortat till saldot).
  - Förlust (`LoseBet()` – nollställer aktuell insats då markerna drogs vid rundans start).
* **Integrering & Saldostyrning:**
  - Kopplat samman resultatberäkningen i `Round` och `ConsoleUi` med spelarnas `RecordOutcome()` så att utbetalningar och statistik registreras konsekvent.
  - Automatisk hantering för att ta bort spelare som får 0 marker ("panka") inför nästa runda.
  - Förberett och anpassat datakonstruktorer för persistering av markersaldon via `GameStorage`.

## Sarwars del - Datorspelare och strategier
Jag har gjort med att lägga till datorspelarna i vårt Blackjack-spel. Målet var att datorspelarna automatiskt ska kunna bestämma om de ska köpa kort eller stanna.
## IPlayerStrategy
Jag skapade interfacet IPlayerStrategy med metoden ChooseMove(Hand hand, Card dealerUpCard). Det fungerar som en kontrakt och mina strategier klasser ska följa.
## SimplePlayerStrategy
Jag skapade en enkel strategi där datorspelaren bara kontrollerar sina egna poäng. Under 17 poäng den väljer att köpa kort.
17 eller över poäng då den stannar.
Den här strategin tar inte hänsyn till givarens kort.
## BasicPlayerStrategy
Jag skapade även en lite mer avancerad strategi som använder olika regler och strategitabeller.
Datorspelaren kontrollerar sin egen poäng och kan även ser givarens synliga kort för att köpa kort eller stanna.
Den strategin kan se om handen är mjuk eller hårt därefter bestämmer att köpa eller stanna.
Jag har kopplat strategierna till datorspelarna så att datorspelare och mänskliga kan spela vid samma bort.
När jag testade under ett spel att datorspelarna kan fatta egna beslut utifrån sin stategin.
  Markdown
## Klassdiagram
![Klassdiagram over 21](//www.plantuml.com/plantuml/png/hLPRLnit47xVNw53psbC2WwqwQLxKax0170BX6BIF8lTmLRHIZwwOEZblsT6Omkn-F0K7hIZcT4ttrmayzu7wKBiTELlbPbB9plHQEdzVWXENSS0uyQQYVe7n4xXqbbZcvcp7OWRgJtqJuj_Au5_1xAvdJeRJJkosZhnUQO2iCMw5Xnh3pIw9Us-StPHAl-lgbVYWucThm1NCOag9EmJGDeUXKCbaKnBuaZ7Q_fEvh85ZsVvpAKqj-dCrSAAtyJt9CqS0Ce_e7naeqFnBOdgZhG_4eQwH-adak0E3Ce_e_n1JMS1vLzGFbU6d7ybkEImkuFYxyYQv5sK_up01WhvXp9JaYdaVWCaliqKp-mTS5egKCWWJPkD7sDeRCVspyhWUcezuESY-babk59_vsgDJG1t8nkiW5g9OdoXvGEuIN0omFGXONqdHZEBI1HyCCE8jJZ1TKkq83NsHZhKqBfP9u8CFx8En1jnQ3kf3EfRqBxYdfCl8wS-rMcbRMf1dLRQ9iU1povRmcNxPd8uXl2Nr14665ePnhYoo1wBI3gV92J4aOatHzuM3MBxMfmg7zvHd3sEoDhn9SZsez4FFNCAzXXglsqJnQOimPrLBMDfa6vGQiR-8Ff0gcjhzLALsvBrI5TU8tNP1C6yBwGBgb5pQSAoSEUoWvmVAKvoOt3xJNbYO0fVhjibD80p_0BvSrWGLgQVIyW3mYnep7mk1usRXs9FSrVkSwQ5RZHJyt8RdGCJ3Y0ywbR3MFUdCrbPrSznlM6Blkknyfa6CFX0TZOQvf2xnVcYjUXfaTI5VB2nj741-lvq9_iwR8TrxVB-3Yn_Qx5Kb9uJ29CWWnyiLfNV4fhBpfivXioRwxBWf6XojP_rzH2apdtbpM2JOyvirEs9oeDR7cfkNxnFv91XuisDXY9nZaFEfAGlLyt1N3euikuSxiEbZUl6SAAwkOQlz5env87qglbgxz-nx02RFLMD-5OSQRjOHadvB76FbtTmtxIZ6LOglGxpnvjRbzSOkrT6oZD9ZBbVLz6P0PEg1QkG9EBY3jUdXG_WEcK09zfWf-SfxJg1yQaIvVbBokbnv_diglUCi9SiFbWdfr0dxmblKdZQhyOYI5VS1fwtnHcO-Bpf1Smozet28P1XLZw5fTkWxy11JwricQYdA2LnfY3lO6LVcrv1xPNuD1PlQ6ezc223icRrEsUDjne-gMMxpwGofzRESsBbauFZpjYzNZpfcl9aP0FDu0k-5vXjw3_m9UjBq8cin_ayX3cO5aoZm5VbZysx_xQtb-3FzJmSLTLxIj4u7Awze6izrzsxgkAzsDZT4AynrCRES8XYxtn9ARlYbty6QNa2y7g37nbQIdV-_wElvNz3AhevQr5eole6fjSpuRUS_ufVWE5mJvJqQq5FYmPVs2as6g97RHuciYH0jBGy47qh8zG2xg690G-_n-x6Jdy1)

