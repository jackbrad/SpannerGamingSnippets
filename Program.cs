
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


//create a player 
var p = new Player();
p.Name ="NoobSlayer";
p.Location = "Cleveland";
p.PictureURL = "https://giphy.com/gifs/L1VRSg6CslKVZoxWBu";
p.PlayerUUID = System.Guid.NewGuid().ToString();

//player profile property bag for random settings added at any time
p.Preferences = JsonConvert.SerializeObject(new { Value = 108, Welcome = "Hello", GameMode=1 });


//Send player to Spanner
var spanner = new SpannerHelper(connectionString, credentials);

//inserts the player into a table that aligns perfectly to the properties of the object
spanner.Store("Player",p);

var different = JsonConvert.DeserializeObject<Player>(spanner.Retrieve("Player", "PlayerUUID", p.PlayerUUID));

//Place object in cache 
rh.Cache(p.PlayerUUID,p);


//Get the Object from Cache
//check to see if it was there. 
var cache = rh.Retrieve(p.PlayerUUID);
Player cachePlayer;

if(cache.HasValue)
{
    cachePlayer = JsonConvert.DeserializeObject<Player>(cache);
}
else{
    //get from spanner
    cachePlayer = null;
}
//Print cached object 
Console.WriteLine(JsonConvert.SerializeObject(cachePlayer));