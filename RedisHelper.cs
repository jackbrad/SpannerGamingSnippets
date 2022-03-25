using StackExchange.Redis;
using Google.Cloud.Spanner.Data;
using Newtonsoft.Json;


public class Relper
{    private  Lazy<ConnectionMultiplexer> lazyConnection;

    public Relper(string RedisServerEndpoint){
        
        
        this.lazyConnection = CreateConnection(RedisServerEndpoint);

    }
    public  ConnectionMultiplexer Connection
    {
        get
        {
            return lazyConnection.Value;
        }
    }

    private  Lazy<ConnectionMultiplexer> CreateConnection(string endpoint)
    {
        return new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect(endpoint);
        });
    }

    public void Cache(string key, object item)
    {
        this.Connection.GetDatabase().StringSet(new RedisKey(key),JsonConvert.SerializeObject(item));

    }
    public RedisValue Retrieve(string key)
    {
        
        return this.Connection.GetDatabase().StringGet(new RedisKey(key),CommandFlags.None);
        
    }

}
