# SocketApp_InterstellarRadar
===========================================================
                PROGETTO: TERMINALE RADAR 


1. DESCRIZIONE
--------------
Il programma è un gioco di esplorazione spaziale composto da 
due parti che comunicano tra loro tramite Socket:
- SERVER: Gestisce la mappa, i movimenti e le statistiche.
- CLIENT (RADAR): La tua interfaccia grafica per pilotare.

2. COME AVVIARE IL PROGRAMMA
----------------------------
Per far funzionare il gioco correttamente, segui questo ordine:

1. Avvia prima il SERVER
2. Avvia il CLIENT (RADAR): Si aprirà la finestra del radar 
   con la griglia spaziale.

Nota: Entrambi i programmi devono essere in esecuzione 
contemporaneamente, ma in questo caso ho messo tramite le proprieta',
il multi avviamento di entrambe le AppConsole.

3. COMANDI DI GIOCO
-------------------
Usa i tasti della tastiera per muoverti nello spazio:
- [W] : Vai su (Nord)
- [S] : Vai giù (Sud)
- [A] : Vai a sinistra (Ovest)
- [D] : Vai a destra (Est)

Ogni movimento consuma 2 unità di ENERGIA. Se non premi tasti, 
il radar invia un comando "PING" per aggiornare solo la visuale 
senza consumare carburante.

4. LEGENDA RADAR
----------------
Sullo schermo vedrai diversi simboli:
- [▲] : Sei tu (al centro del radar).
- [O] : Asteroide (Ostacolo estetico, colore Rosso).
- [F] : Fuel/Carburante (Obiettivo, colore Giallo).
- [.] : Spazio vuoto.

5. OBIETTIVO
------------
Esplora la mappa per raccogliere le 3 TANICHE di carburante (F).
Ogni tanica raccolta:
- Aumenta il tuo punteggio (CARBURANTE preso).
- Ti ricarica di +30 energia.

Se l'energia arriva a 0%, non sara' più possibile muoversi

===========================================================