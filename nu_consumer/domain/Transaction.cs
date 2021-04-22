using Newtonsoft.Json;

public class Transaction {
    [JsonProperty("merchant")]
    public string Merchant {get;set;}

    [JsonProperty("amount")]
    public int Amount {get;set;}

    [JsonProperty("time")]
    public string Time {get;set;} 
}