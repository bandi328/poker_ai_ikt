using Poker_AI;
using System.Diagnostics.Contracts;


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

Game game = new Game();
Human human = new Human();
AI ai = new AI();

bool decision = true;

while (decision)
{
    human.Hand = new List<string>{ };
    ai.Hand = new List<string>{ };
    Cards cards = new Cards();
    List<string> tableCards = new List<string> { };

    //beszállás
    Console.WriteLine($"Játékos tőke: {human.Money}");
    Console.WriteLine($"AI tőke: {ai.Money}");
    Console.WriteLine();
    Console.WriteLine("Játék: 5$\nKiszállás: 0$");
    int bet;
    do
    {
        bet = human.Bet(human.Money);
    } while (bet == 5 && bet == 0);
    decision = game.FirstBet(bet, human, ai);
    if (decision == false)
    {
        break;
    }
    Thread.Sleep(2000);
    Console.Clear();


    game.Burning(cards);
    //osztás
    for (int i = 0; i < 3; i++)
    {
        tableCards.Add(game.Dealing(cards));
        if (i >= 1)
        {
            human.Hand.Add(game.Dealing(cards));
            ai.Hand.Add(game.Dealing(cards));
        }
    }//

    Console.WriteLine($"Játékos tőke: {human.Money}");
    Console.WriteLine($"AI tőke: {ai.Money}");
    Console.WriteLine();
    
    Console.WriteLine("\nOsztott lapok:");
    game.HandListing(human);
    //1. licit
    int[] bidding = game.BiddingRound(human, ai);
    
    if (bidding[0] == 0)
    {
        Console.WriteLine($"AI nyert: {ai.Hand[0]}{ai.Hand[1]}");
    }

    //flop
    Console.WriteLine("Kezdő osztás:");
    foreach (var item in tableCards)
    {
        Console.WriteLine(item);
    }

    
    //2. licit


    //turn


    //3. licit


    //river


    //4., utolsó licit 
}