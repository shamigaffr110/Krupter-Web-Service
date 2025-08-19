
public class BillValidator
{
    public string ValidateUnitsConsumed(int units)
    {
        if (units < 0)
        {
            return "Given units is invalid";
        }
        return string.Empty;
    }
}
