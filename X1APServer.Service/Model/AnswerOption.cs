namespace X1APServer.Service.Model
{
    public class AnswerOption
    {
        public int ID { get; set; }
        public string OptionText { get; set; }
        public string Value { get; set; }
        public int CodingBookIndex { get; set; }
        public string CodingBookTitle { get; set; }
        public bool HiddenFromBackend { get; set; }
    }
}
