namespace NearShare;

static class Extensions
{
    public static T Ref<T>(this T obj, out T reference)
    {
        reference = obj;
        return obj;
    }
}
