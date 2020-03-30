namespace Skybrud.Umbraco.Search.Models.Options
{
    public enum JoinTypeEnum
    {
        Or,
        And
    }

    public static class EnumTranslator
    {
        public static string JoinTypeToString(this JoinTypeEnum joinType)
        {
            return joinType == JoinTypeEnum.And ? " AND " : " OR ";
        }
    }
}
