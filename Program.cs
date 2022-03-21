
using Google.Cloud.Spanner.Data;
using Grpc.Core;
using Grpc.Auth;
using Google.Apis.Auth.OAuth2;

using System;
using System.Threading.Tasks;


string projectId = "bitfoon-dev";
string instanceId = "bitfoon-dev";
string databaseId = "bitfoon";
string credentialFile = "/Users/bradham/downloads/bitfoon-dev-34cce5fcde89.json";
string connectionString =$"Data Source=projects/{projectId}/instances/{instanceId}/databases/{databaseId}";

var builder = new SpannerConnectionStringBuilder();
builder.CredentialFile = credentialFile;
builder.ConnectionString = connectionString;

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
