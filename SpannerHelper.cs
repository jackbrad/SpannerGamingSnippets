using System.Text;
using System.Reflection;
using Google.Cloud.Spanner.Data;
using Newtonsoft.Json;
using Grpc.Core;

public class SpannerHelper
{

    //hold the connection object and initializes when needed
    private Lazy<SpannerConnection> lazyConnection;
    public SpannerHelper(string connectionString, ChannelCredentials creds)
    {
        this.lazyConnection = CreateConnection(connectionString, creds);
    }
    //open connection object for use outside helper. 
    public SpannerConnection Connection
    {
        get
        {
            return lazyConnection.Value;
        }
    }

    private Lazy<SpannerConnection> CreateConnection(string connectionString, ChannelCredentials creds)
    {
        return new Lazy<SpannerConnection>(() =>
        {
            return new SpannerConnection(connectionString, creds);
        });
    }

    //Stored data items with a very simple ORM mapping based on properties of the object
    public void Store(string TableName, object item)
    {
        //use reflection to get a named value pair of the item being stored
        var props = item.GetType().GetProperties().Select(p => new { Name = p.Name, Value = FormatParamValue(p, item) });
        // Execute the SQL statement.
        var cmd = lazyConnection.Value.CreateDmlCommand($"INSERT INTO {TableName} ({String.Join(',', props.Select(pr => pr.Name))}) VALUES ({String.Join(',',props.Select(pr=>pr.Value))})");
        var ret = cmd.ExecuteNonQuery();
        Console.WriteLine($"DML Response {ret.ToString()}");
    }

    public string Retrieve(string TableName, String KeyFieldName, string Key)
    {
       var cmd = lazyConnection.Value.CreateSelectCommand($"SELECT TO_JSON(t) AS json FROM {TableName} as t where {KeyFieldName}='{Key}'");

        var reader = cmd.ExecuteReader();
        StringBuilder sb = new StringBuilder(1000);
        while(reader.Read()){
            sb.Append(reader.GetString(0).ToString());
        }
        return sb.ToString();
    }

    //helper function to clean up and insert Json where needed.
    private string FormatParamValue(PropertyInfo p, object item)
    {
        //check for json attributes
        var attri =p.GetCustomAttribute<JsonPropertyAttribute>();
        //json data
        if(null!=attri)
        {
            //add the function to insert Json in Spanner
            return $"TO_JSON('{p.GetValue(item)}')";
        }
        //add quotes
        if(p.PropertyType == typeof(String))
        {
            return $"'{p.GetValue(item).ToString()}'";
        }
        //ignore
        return p.GetValue(item).ToString();
    }
}