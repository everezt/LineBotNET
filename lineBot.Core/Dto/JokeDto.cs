namespace lineBot.Core.Dto
{
    public class JokeDto
    {
        public bool Error { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Joke { get; set; }
        public JokeFlagsDto Flags { get; set; }
        public int Id { get; set; }
        public string Lang { get; set; }
    }

    public class JokeFlagsDto
    {
        public bool Nsfw { get; set; }
        public bool Religious { get; set; }
        public bool Political { get; set; }
        public bool Racist { get; set; }
        public bool Sexist { get; set; }
    }
}
