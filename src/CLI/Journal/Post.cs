public record Post
{
    public Post(string date, string description, decimal amount, string account, string comment)
    {
        Date = DateTime.Parse(date);
        Description = description;
        Amount = amount;
        Account = account;
        Comment = comment;
    }

    public DateTime Date { get; }
    public string Description { get; }
    public decimal Amount { get; }
    public string Account { get; }
    public string Comment { get; }
}