# Tjugoett

## Pågående arbete: lägga till spelare

Vi arbetar med user storyn: **Som spelare vill jag kunna lägga till 1–5 spelare med namn, så att vi kan spela flera tillsammans.**

### Hittills
- `Player` har en konstruktor som tar emot spelarens namn och skickar det vidare till basklassen `Participant`.
- `Participant` lagrar deltagarens namn och skapar en tom `Hand` för deltagaren.
- `ConsoleUi` frågar efter 1–5 spelare och deras namn, och `Program` anropar metoden för att samla in spelarna.
- `Hand` lagrar kort i en intern lista, gör korten tillgängliga för läsning via `Cards` och har metoden `AddCard` för att lägga till kort.

### Kvar att göra
- Använda spelarlistan i resten av spelet när spelrundan kopplas in.
- Lägga till handens poängräkning och kontroller för mjuk hand, tjugoett och sprucken hand.

Vi uppdaterar den här README:n efterhand som arbetet går framåt.