using Poker_AI;

Cards cards = new Cards();

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