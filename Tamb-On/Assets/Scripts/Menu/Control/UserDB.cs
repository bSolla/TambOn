using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string username;
    public string email;
    public int level;
    public List<string> friends;
    public List<string> friendsRequest;
    public string status;
    public User(string username, string email,int level, List<string> fr,string st, List<string> frRequest)
    {
        this.username = username;
        this.email = email;
        this.level = level;
        this.friends = fr;
        this.status = st;
        this.friendsRequest = frRequest;
    }
}
