using System;
using Newtonsoft.Json;


public class Player
{
    public string PlayerUUID { get; set; }
    public string Name { get; set; }
    public string PictureURL { get; set; }
    public string    Location { get; set; }
    
   [JsonProperty()]
   public string Preferences { get; set; }    
    
}

