namespace SharedKernel.Helpers.IBAN;

public class IbanHelper
{
    public IbanHelper(
        string countryCode
        , int length
        , string regexStructure
        , bool isEu924 = false
        , string sample = ""
    )
    {
        CountryCode = countryCode;
        Length = length;
        Regex = regexStructure;
        IsEu924 = isEu924;
        Sample = sample;
    }

    public int Length { get; }
    public string Regex { get; }
    private string CountryCode { get; }
    private bool IsEu924 { get; }
    private string Sample { get; }
}