using System.Net.Sockets;
using System.Text;

Console.Title = "TERMINALE RADAR"; // Titolo della finestra console
Console.CursorVisible = false;      // Nasconde il trattino lampeggiante fastidioso

//Ciclo infinito per far andare il gioco
while (true)
{
    // Variabile comando: di base è "PING" (chiede solo dati) ma se premo un tasto cambia
    string comando = "PING";
 
    // KeyAvailable controlla se ho premuto un tasto senza fermare il radar mentre aspetta.
    if (Console.KeyAvailable)
    {
        var k = Console.ReadKey(true).Key; // Legge il tasto premuto (true fa in modo che non lo scrive a schermo)
        comando = k.ToString();            // Trasforma il tasto in testo ( per esempio i tasti usati per muoversi " W A S D " )
    }


}