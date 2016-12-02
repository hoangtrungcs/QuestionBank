namespace EnglishQuestion.Entity
{
    public class B1B2ConfigValue : BaseEntity
    {
        public string Key { get { return Get<string>(); } set { Set(value); } }
        public string Value { get { return Get<string>(); } set { Set(value); } }
    }
}
