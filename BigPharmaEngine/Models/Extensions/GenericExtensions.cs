namespace BigPharmaEngine.Models.Extensions;

public static class GenericExtensions
{
    public static T CreatePatch<T>(this T model, Action<T> where) where T: IModelCopyable<T>
    {
        var copy = model.Copy();
        where(copy);
        return copy;
    }
}