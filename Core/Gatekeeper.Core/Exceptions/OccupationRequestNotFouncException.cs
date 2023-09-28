namespace Gatekeeper.Core.Exceptions;

public class OccupationRequestNotFouncException : InvalidOperationException {
    public OccupationRequestNotFouncException(string message) : base(message)
    {
        
    }
}