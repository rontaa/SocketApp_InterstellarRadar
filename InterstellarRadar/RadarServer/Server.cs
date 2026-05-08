using System.Net;
using System.Net.Sockets;
using System.Text;

Random rng = new Random(); // Generatore di numeri casuali per creare la mappa ogni volta diversa

int pX = 0, pY = 0;        // Coordinate X e Y del giocatore (parte dal centro 0,0)

int energia = 100;         // Carburante: cala muovendosi, se finisce non ci si muove più

int tanichePrese = 0;      // Il nostro punteggio: quante taniche abbiamo raccolto

// --- CREAZIONE MAPPA ---
// Lista per gli asteroidi (ostacoli fissi sulla mappa)
var asteroidi = new List<(int x, int y)>();
for (int i = 0; i < 8; i++)
    asteroidi.Add((rng.Next(-10, 11), rng.Next(-10, 11))); // Crea 8 asteroidi a caso tra -10 e 10

// Lista per le taniche (i nostri obiettivi da raccogliere)
var taniche = new List<(int x, int y)>();
for (int i = 0; i < 3; i++)
    taniche.Add((rng.Next(-10, 11), rng.Next(-10, 11))); // Crea 3 taniche a caso sulla mappa

// --- AVVIO SERVER ---
TcpListener server = new TcpListener(IPAddress.Any, 8888); // Apre il server sulla porta 8888
server.Start();
Console.WriteLine("SERVER AVVIATO E IN ASCOLTO...");

while (true) // Ciclo infinito: il server deve rispondere sempre a ogni comando
{
    using var client = server.AcceptTcpClient(); // Aspetta che il radar si connetta
    using var stream = client.GetStream();        // Apre il tunnel per mandare i dati

    byte[] buffer = new byte[1024];
    int bytesRead = stream.Read(buffer, 0, buffer.Length);
    string comando = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim().ToUpper();

    // --- GESTIONE MOVIMENTO ---
    if (energia > 0) // Ci muoviamo solo se abbiamo ancora carburante
    {
        if (comando == "W") pY++; // Vai su
        if (comando == "S") pY--; // Vai giù
        if (comando == "A") pX--; // Vai a sinistra
        if (comando == "D") pX++; // Vai a destra

        // Se abbiamo premuto un tasto di movimento (non un PING), togliamo energia
        if (comando != "PING") energia -= 2;
    }

    // --- LOGICA DI RACCOLTA ---
    // Controlliamo una per una tutte le taniche sulla mappa
    for (int i = 0; i < taniche.Count; i++)
    {
        // Se la mia posizione è uguale a quella della tanica numero "i"
        if (taniche[i].x == pX && taniche[i].y == pY)
        {
            tanichePrese++;      // Aumenta il punteggio
            taniche.RemoveAt(i); // Cancella la tanica dalla mappa (l'abbiamo presa!)
            energia += 30;       // Bonus carburante

            if (energia > 100) energia = 100; // L'energia non può superare il 100%
            break; // Abbiamo già trovato cosa c'era in questa casella, smettiamo di cercare
        }
    }

    // --- COSTRUZIONE DEL MESSAGGIO (IL PACCO DA MANDARE AL RADAR) ---
    StringBuilder sb = new StringBuilder();

    // Dati del giocatore (P: posizione)
    sb.Append($"P:{pX},{pY}|O:");

    // Filtro Radar (O: oggetti vicini)
    // Controlliamo quali asteroidi sono nel raggio di 5 quadratini da noi
    foreach (var a in asteroidi)
    {
        int relX = a.x - pX; // Calcola quanto è lontano l'asteroide da noi (X)
        int relY = a.y - pY; // Calcola quanto è lontano l'asteroide da noi (Y)

        if (Math.Abs(relX) <= 5 && Math.Abs(relY) <= 5) // Se è vicino (max 5 caselle)
            sb.Append($"{relX},{relY},A;"); // Lo aggiungiamo alla lista come Asteroide (A)
    }

    // Facciamo la stessa cosa per le taniche rimaste
    foreach (var t in taniche)
    {
        int relX = t.x - pX;
        int relY = t.y - pY;
        if (Math.Abs(relX) <= 5 && Math.Abs(relY) <= 5)
            sb.Append($"{relX},{relY},F;"); // La aggiungiamo come Carburante (F)
    }

    // Statistiche finali (S: energia e punti)
    sb.Append($"|S:{energia},{tanichePrese}");

    // --- INVIO FINALE ---
    byte[] res = Encoding.UTF8.GetBytes(sb.ToString()); // Trasforma tutto il testo in byte
    stream.Write(res, 0, res.Length); // Spedisce il pacchetto al radar
}