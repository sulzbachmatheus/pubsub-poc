using Newtonsoft.Json;

public class Account {
    [JsonProperty("active-card")]
    public bool ActiveCard {get;set;}
    
    [JsonProperty("available-limit")]
    public int AvailableLimit {get;set;}
}