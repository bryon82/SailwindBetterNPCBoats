using HarmonyLib;

namespace BetterNPCBoats
{
    internal static class Extensions
    {
        public static T GetPrivateField<T>(this object obj, string field)
        {
            return (T)Traverse.Create(obj).Field(field).GetValue();
        }

        public static void SetPrivateField(this object obj, string field, object value)
        {
            Traverse.Create(obj).Field(field).SetValue(value);
        }
    }
}
