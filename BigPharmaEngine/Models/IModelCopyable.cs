namespace BigPharmaEngine.Models;

public interface IModelCopyable<out T>
{
    T Copy();
}