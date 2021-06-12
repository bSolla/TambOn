using System.Collections.Generic;


public class LobbyDB { 
    
public string lobbyName;
public List<string> players;
public List<string> playersReady;
public string currentSong;
public string hostPlayer;
public LobbyDB(string lobbyName, string currentSong, List<string> players, List<string> playersReady, string hostPlayer)
{
    this.lobbyName = lobbyName;
    this.currentSong = currentSong;
    this.players = players;
    this.playersReady = playersReady;
    this.hostPlayer = hostPlayer;
}
}
