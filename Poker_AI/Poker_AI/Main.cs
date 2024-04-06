using Poker_AI;
using System.Diagnostics.Contracts;


//Játékmenet:
//10$/20$ játék
//200$ tőke
//alap tétek megadása (kötelező, hogy mindenképp kelljen játszani)
//először a JÁTÉKOS a "kis vak", 5$ beszálló
//AI kártyái alapján tartja/emel
//ha AI emel, JÁTÉKOS dönthet a megadás/eldobás/emelés közül (max 3 emelés lehetséges, utána megadás/eldobás)
//ha AI nem emel =>
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
    Thread.Sleep(2000);
    Console.Clear();

    //first dealing
    game.Burning(cards);
    for (int i = 0; i < 3; i++)
    {
        tableCards.Add(game.Dealing(cards));
        if (i >= 1)
        {
            human.Hand.Add(game.Dealing(cards));
            ai.Hand.Add(game.Dealing(cards));
        }
    }

    Console.WriteLine("Kezdő osztás:");
    foreach (var item in tableCards)
    {
        Console.WriteLine(item);
    }
    Console.WriteLine("\nOsztott lapok:");
    foreach (var item in human.Hand)    
    {
        Console.WriteLine(item);
    }

    
}