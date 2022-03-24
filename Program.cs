
using System;
using System.Threading.Tasks;

//added assemblies for code example
using Google.Cloud.Spanner.Data;
using Grpc.Core;
using Grpc.Auth;
using Google.Apis.Auth.OAuth2;

using Newtonsoft.Json;


using SpannerGamingSnippets;




//setup connection string to spanner
string projectId = "bitfoon-dev";
string instanceId = "bitfoon-dev";
string databaseId = "bitfoon";
string credentialFile = "/Users/bradham/downloads/bitfoon-dev-34cce5fcde89.json";
string connectionString =$"Data Source=projects/{projectId}/instances/{instanceId}/databases/{databaseId}";

//lets auth with a credential file. 
GoogleCredential googleCredential = GoogleCredential.FromFile(credentialFile);
var credentials = googleCredential.ToChannelCredentials();

//redis server endpoint
string endpoint = "localhost";
var rh = new SpannerGamingSnippets.Relper("endpoint");

//create a player 
var p = new Player();
p.Name ="NewbSlayer";
p.Location = "Cleveland";
p.PictureURL = "https://giphy.com/gifs/L1VRSg6CslKVZoxWBu";
p.PlayerUUID = Convert.ToString(Guid.NewGuid);
p.Preferences = JsonConvert.SerializeObject(new { Value = 108, Welcome = "Hello", GameMode=1 });









