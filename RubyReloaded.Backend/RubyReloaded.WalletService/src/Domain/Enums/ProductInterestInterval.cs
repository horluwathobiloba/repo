using Newtonsoft.Json;

namespace RubyReloaded.WalletService.Domain.Enums
{
    public enum ProductInterestInterval
    {
        [JsonProperty("30 Days")]
        _30Days = 30,
        [JsonProperty("60 Days")]
        _60Days = 60,
        [JsonProperty("90 Days")]
        _90Days = 90,
        [JsonProperty("120 Days")]
        _120Days = 120,
        [JsonProperty("180 Days")]
        _180Days = 180,
        [JsonProperty("210 Days")]
        _210Days =210,
        [JsonProperty("240 Days")]
        _240Days = 240,
        [JsonProperty("270 Days")]
        _270Days = 270,
        [JsonProperty("300 Days")]
        _300Days = 300,
        [JsonProperty("330 Days")]
        _330Days = 330,
        [JsonProperty("365 Days")]
        _365Days = 365,
    }
}
