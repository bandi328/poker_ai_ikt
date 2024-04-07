using Poker_AI;
using System.Diagnostics.Contracts;
Console.OutputEncoding = System.Text.Encoding.UTF8;

//Játékmenet:
//10$/20$ játék
//200$ tőke
//alap tétek megadása (kötelező, hogy mindenképp kelljen játszani)
//először a JÁTÉKOS a "kis vak", 5$ beszálló
//AI megadja a dupláját

//első licit

//DEALER oszt 3 lapot (1-et éget előtte)

//kör
//"kis vak" tétet rak/skippel/eldob
//"nagy vak" tart/emel/eldob

//ha bármelyik is az AI: lapok alapján tétet rak/tart/skippel/eldob

//3 emelés vagy végleges megadés után új lap 1 égetéssel, még egy kör jön

//3 kör után lapok mutatása, pénz jóváírása, új kör.
//amíg valaki nem nyer, vagy amíg a játékos úgy nem dönt megy a játék


Human human = new Human();
AI ai = new AI();
Game game = new Game(human, ai);
bool decision = true;

while (decision)
{
    Console.Clear();
    human.Hand = new List<string>{ };
    ai.Hand = new List<string>{ };
    Cards cards = new Cards();
    List<string> tableCards = new List<string> { };
    for (int i = 0; i < 5; i++)
    {
        tableCards.Add("?");
    }
    //beszállás
    Console.WriteLine($"Játékos tőke: {human.Money}");
    Console.WriteLine($"AI tőke: {ai.Money}");
    Console.WriteLine();
    Console.WriteLine("Játék: 5$\nKiszállás: 0$");

    //int bet = human.Bet(human.Money, 5, false);
    decision = game.FirstBet();
    if (decision == false)
    {
        break;
    }
    Thread.Sleep(2000);
    Console.Clear();

    //osztás

    for (int i = 0; i < 2; i++)
    {
        human.Hand.Add(game.Dealing(cards));
        ai.Hand.Add(game.Dealing(cards));
    }

    //1. licit
    if(InitiateBidding(tableCards))
        break;


    Console.WriteLine(
    new HandCheck().CheckHand(new List<string> { ai.Hand[0], ai.Hand[1] })
    );
    Thread.Sleep(5000);

    //flop
    game.Burning(cards);
    for (int i = 0; i < 3; i++)
    {
        tableCards[i] = game.Dealing(cards);
    }//


    //2. licit
    if (InitiateBidding(tableCards))
        break;
    Console.WriteLine(new HandCheck().CheckHand(new List<string> { ai.Hand[0], ai.Hand[1], tableCards[0], tableCards[1], tableCards[2] }));


    //turn
    game.Burning(cards);
    tableCards[3] = game.Dealing(cards);

    //3. licit
    if (InitiateBidding(tableCards))
        break;
    Console.WriteLine(new HandCheck().CheckHand(new List<string> { ai.Hand[0], ai.Hand[1], tableCards[0], tableCards[1], tableCards[2], tableCards[3] }));


    //river
    game.Burning(cards);
    tableCards[4] = game.Dealing(cards);

    //4., utolsó licit 
    if (InitiateBidding(tableCards))
        break;
    Console.WriteLine(new HandCheck().CheckHand(new List<string> { ai.Hand[0], ai.Hand[1], tableCards[0], tableCards[1], tableCards[2], tableCards[3], tableCards[4] }));
}

bool InitiateBidding(List<string> tableCards)
{
    Console.Clear();
    Console.WriteLine($"Játékos tőke: {human.Money}$");
    Console.WriteLine($"AI tőke: {ai.Money}$");
    Console.WriteLine($"Nyeremény: {game.Pot}$");
    Console.WriteLine($"Tét: {game.bet}$\n");
    Console.WriteLine("Leosztás:");
    foreach (var item in tableCards)
    {
        Console.WriteLine(item);
    }
    Console.WriteLine("\nOsztott lapok:");
    game.HandListing(human);

    bool isFolding = game.BiddingRound();
    if (isFolding)
    {
        ai.Money += game.Pot;
        return true;
    }
    return false;
}