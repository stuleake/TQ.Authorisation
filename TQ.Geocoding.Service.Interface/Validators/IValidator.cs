namespace TQ.Geocoding.Service.Interface.Validators
{
    public interface IValidator
    {
        bool IsValid { get; }
        string ErrorMessage { get; }
        void Validate();
    }
}
