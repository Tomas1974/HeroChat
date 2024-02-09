namespace infrastructure;

public class Utilities
{
    private static readonly Uri Uri = new(Environment.GetEnvironmentVariable("pgconn")!);
    // private static readonly Uri Uri = new("postgres://vjljzkim:nR8wiIfT6gXEt91RBU48uSG1HDUlDpHH@snuffleupagus.db.elephantsql.com/vjljzkim");
    

    public static readonly string
        ProperlyFormattedConnectionString = string.Format(
            "Server={0};Database={1};User Id={2};Password={3};Port={4};Pooling=true;MaxPoolSize=3",
            Uri.Host,
            Uri.AbsolutePath.Trim('/'),
            Uri.UserInfo.Split(':')[0],
            Uri.UserInfo.Split(':')[1],
            Uri.Port > 0 ? Uri.Port : 5432);
}