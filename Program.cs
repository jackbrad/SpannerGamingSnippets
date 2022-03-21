
using System;
using System.Threading.Tasks;

//added assemblies for code example
using Google.Cloud.Spanner.Data;
using Grpc.Core;
using Grpc.Auth;
using Google.Apis.Auth.OAuth2;


//setup connection string to spanner
string projectId = "bitfoon-dev";
string instanceId = "bitfoon-dev";
string databaseId = "bitfoon";
string credentialFile = "/Users/bradham/downloads/bitfoon-dev-34cce5fcde89.json";
string connectionString =$"Data Source=projects/{projectId}/instances/{instanceId}/databases/{databaseId}";

//lets auth with a credential file. 
GoogleCredential googleCredential = GoogleCredential
    .FromFile(credentialFile);
var credentials = googleCredential.ToChannelCredentials();

// Create connection to Cloud Spanner.
using (var connection = new SpannerConnection(connectionString,credentials))
{
    // Execute a simple SQL statement.
    var cmd = connection.CreateSelectCommand(@"SELECT ""Hello World"" as test");
    
    using (var reader = await cmd.ExecuteReaderAsync())
    {
        while (await reader.ReadAsync())
        {
            Console.WriteLine(
                reader.GetFieldValue<string>("test"));
        }
    }
}
