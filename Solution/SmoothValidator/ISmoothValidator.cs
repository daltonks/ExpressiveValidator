using System.Collections.Generic;

namespace SmoothValidator
{
    public interface ISmoothValidator<in TObject, TError>
    {
        bool Validate(TObject obj);
        bool Validate(TObject obj, out List<TError> errors);
    }
}
