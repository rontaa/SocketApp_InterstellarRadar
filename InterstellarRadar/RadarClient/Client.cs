using System.Net.Sockets;
using System.Text;

Console.Title = "TERMINALE RADAR"; // Titolo della finestra console
Console.CursorVisible = false;      // Nasconde il trattino lampeggiante fastidioso

// Ciclo infinito per far andare il gioco
while (true)
{
    // Variabile comando: di base è "PING" (chiede solo dati) ma se premo un tasto cambia
    string comando = "PING";

    // KeyAvailable controlla se ho premuto un tasto senza fermare il radar mentre aspetta.
    if (Console.KeyAvailable)
    {
        var k = Console.ReadKey(true).Key; // Legge il tasto premuto (non lo scrive a schermo)
        comando = k.ToString();            // Trasforma il tasto in testo come W A S D (tasti per muovermi)
    }

    try
    {
        // Connessione e invio, uso 127.0.0.1 dato che sono in singleplayer quindi in locale
        using var client = new TcpClient("127.0.0.1", 8888);
        using var stream = client.GetStream(); // Ottiene il flusso per far viaggiare i dati

        byte[] msg = Encoding.UTF8.GetBytes(comando);
        stream.Write(msg, 0, msg.Length);

        // Ricezione risposta
        byte[] buffer = new byte[1024];
        int read = stream.Read(buffer, 0, buffer.Length);
        string risposta = Encoding.UTF8.GetString(buffer, 0, read);

        
        // Il messaggio arriva come tante lettere e numeri messi insieme e dato che il PC non capirebbe cosa è energia, cosa è carburante etc.
        // Lo dividiamo in 3 grandi scatole usando il separatore barra verticale '|' ( qui ho avuto bisogno di una mano)
        var sezioni = risposta.Split('|');

        // Estraiamo gli oggetti e le statistiche saltando i primi 2 caratteri di etichetta ("O:" e "S:")
        var oggRaw = sezioni[1].Substring(2).Split(';', StringSplitOptions.RemoveEmptyEntries);
        var stats = sezioni[2].Substring(2).Split(',');

        // Conversione delle statistiche da testo a numeri interi per poter fare calcoli
        int energia = int.Parse(stats[0]);
        int score = int.Parse(stats[1]);

        // Parte grafica
        //Faccio partire tutto dalle coordinate 0,0
        Console.SetCursorPosition(0, 0);

        Console.ForegroundColor = ConsoleColor.Cyan;
        // .PadRight(50) aggiunge spazi vuoti per cancellare scritte vecchie più lunghe
        Console.WriteLine($"=== SISTEMA RADAR ===  CARBURANTE: {score}/3".PadRight(50));

        Console.Write("ENERGIA: ");
        // Cambia colore in base all'energia: Verde se OK, Rosso se sotto il 20%
        Console.ForegroundColor = energia > 20 ? ConsoleColor.Green : ConsoleColor.Red;
        Console.WriteLine($"{energia}%   ".PadRight(40));

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("-------------------------------------------");

        // Griglia radar
        // Partiamo da Y=5 (alto) fino a Y=-5 (basso) per disegnare la mappa
        for (int y = 5; y >= -5; y--)
        {
            Console.Write("  "); // Margine sinistro per centrare il radar
            for (int x = -5; x <= 5; x++)
            {
                // Se siamo alle coordinate (0,0), siamo noi (centro del radar)
                if (x == 0 && y == 0)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("▲ ");
                }
                else
                {
                    // Controllo coordinate: Cerca se c'è un oggetto del server in questa casella (x,y)
                    string[] objTrovato = null;
                    foreach (var riga in oggRaw)
                    {
                        var dati = riga.Split(','); // Divide la riga in: [x, y, tipo]
                        // Confronta la posizione dell'oggetto con la casella che stiamo disegnando
                        if (int.Parse(dati[0]) == x && int.Parse(dati[1]) == y)
                        {
                            objTrovato = dati;
                            break; // Se lo trovi, ferma la ricerca per questa casella
                        }
                    }

                    // Disegno: In base a cosa abbiamo trovato, scegliamo il simbolo
                    if (objTrovato != null)
                    {
                        if (objTrovato[2] == "A") // Se è un Asteroide
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("O ");
                        }
                        else if (objTrovato[2] == "F") // Se è Fuel (Carburante)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("F ");
                        }
                    }
                    else // Se la casella è vuota
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write(". ");
                    }
                }
            }
            Console.WriteLine(); // Va a capo alla fine di ogni riga della griglia
        }
    }
    catch
    {
        // Catch vuoto per evitare crash se il server non risponde per un istante
    }
}