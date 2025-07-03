namespace EcommerceWeb.Services;

public class GuidServices : IGuidServices
{
    private readonly Guid _guid = Guid.NewGuid();

    public string GetGuid()
    {
        return _guid.ToString();
    }
}