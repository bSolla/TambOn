using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Records
{

    public List<int> scores;
    public List<string> usernames;
    public List<int> mutators;
    public List<string> character;
    public Records(List<string> usernames, List<int> scores, List<int> mut,List<string> character)
    {
        this.usernames = usernames;
        this.scores = scores;
        this.mutators = mut;
        this.character = character;
    }
 
    public Records()
    {
        this.usernames = new List<string>();
        this.scores = new List<int>();
        this.mutators = new List<int>();
        this.character = new List<string>();
    }
}
