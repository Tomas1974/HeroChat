namespace infrastructure;

public class Utilities
{
    //private static readonly Uri Uri = new(Environment.GetEnvironmentVariable("pgconn")!);
    private static readonly Uri Uri = new("postgres://vjljzkim:UOo8Wly-t_Q5vfxaM3_wJyP9kzCu7rM4@snuffleupagus.db.elephantsql.com/vjljzkim");
    

    public static readonly string
        ProperlyFormattedConnectionString = string.Format(
            "Server={0};Database={1};User Id={2};Password={3};Port={4};Pooling=true;MaxPoolSize=3",
            Uri.Host,
            Uri.AbsolutePath.Trim('/'),
            Uri.UserInfo.Split(':')[0],
            Uri.UserInfo.Split(':')[1],
            Uri.Port > 0 ? Uri.Port : 5432);
}