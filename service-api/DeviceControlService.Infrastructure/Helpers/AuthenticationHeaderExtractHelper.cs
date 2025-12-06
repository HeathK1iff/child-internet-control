using System.Net.Http.Headers;

namespace DeviceControlService.Infrastructure.Helpers;

public static class AuthenticationHeaderExtractHelper
{
    const string RealmKey = "X-NDM-Realm";
    const string RealmChallengeKey = "X-NDM-Challenge";
    public static string ExtractRealm(HttpResponseHeaders pairs) =>
        pairs.TryGetValues(RealmKey, out var props) ? props.First() : string.Empty;

    public static string ExtractRealmChallenge(HttpResponseHeaders pairs) =>
        pairs.TryGetValues(RealmChallengeKey, out var props) ? props.First() : string.Empty;
}

