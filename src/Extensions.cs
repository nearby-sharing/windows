namespace NearShare.Windows;

static class Extensions
{
    public static T Ref<T>(this T obj, out T reference)
    {
        reference = obj;
        return obj;
    }
}
