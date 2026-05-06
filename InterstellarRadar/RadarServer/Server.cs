using System.Net;
using System.Net.Sockets;
using System.Text;

Random rng = new Random(); // Generatore di numeri casuali per creare la mappa ogni volta diversa

int pX = 0, pY = 0; // Coordinate X e Y del giocatore (parte da 0,0)

int energia = 100; // Risorsa limitata, cala muovendosi, se arriva a 0 il gioco finisce

int tanichePrese = 0; // Contatore degli obiettivi raggiunti (punteggio del giocatore)

// Lista per gli asteroidi, rappresentano ostacoli che non vanno via nella mappa (è solo per estetica)
var asteroidi = new List<(int x, int y)>();