
//base assemblies
using System;
using System.Threading.Tasks;
using System.Text.Json;

//added assemblies used in this example
using Grpc.Core;
using Grpc.Auth;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;

//setup connection string to spanner
string projectId = "bitfoon-dev";
string instanceId = "bitfoon-dev";
string databaseId = "bitfoon";
string credentialFile = "/Users/bradham/downloads/bitfoon-dev-34cce5fcde89.json";
string connectionString =$"Data Source=projects/{projectId}/instances/{instanceId}/databases/{databaseId}";

//lets auth with a credential file. 
GoogleCredential googleCredential = GoogleCredential.FromFile(credentialFile);
var credentials = googleCredential.ToChannelCredentials();

//redis or memstore server endpoint setup
string endpoint = "localhost";
var rh = new Relper(endpoint);

//spanner helper for ORM to Spanner
var spanner = new SpannerHelper(connectionString, credentials);


//Scenario 1 brand new player 
//create a player 
var p = new Player();
p.Name ="NoobSlayer";
p.Location = "Cleveland";
p.PictureURL = "https://giphy.com/gifs/L1VRSg6CslKVZoxWBu";
p.PlayerUUID = System.Guid.NewGuid().ToString();
//player profile property bag for random settings added at any time
p.Preferences = JsonConvert.SerializeObject(new { Value = 108, Welcome = "Hello", GameMode=1 });


//inserts the player into a table that aligns perfectly to the properties of the object
spanner.Store("Player",p);
//Also Add Player to cache
rh.Cache(p.PlayerUUID,p);
//Player now cached . 
Console.WriteLine("New Player:" + JsonConvert.SerializeObject(p).ToString());


//Scenario 2 returning player Object may be in Cache... It may not. 
//Try get the Object from Cache
var cache = rh.Retrieve(p.PlayerUUID);
Player cachePlayer;

//check to see if player was in cache
if(cache.HasValue)
{
    cachePlayer = JsonConvert.DeserializeObject<Player>(cache);
}
else{
    //get from spanner
    cachePlayer = JsonConvert.DeserializeObject<Player>(spanner.Retrieve("Player", "PlayerUUID", p.PlayerUUID));

}

//Print cached object 
Console.WriteLine("Cached Player:" + JsonConvert.SerializeObject(cachePlayer));