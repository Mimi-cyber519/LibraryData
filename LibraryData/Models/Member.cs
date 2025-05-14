namespace LibraryData.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNum { get; set; }

        public ICollection<Loan> Loans { get; set; }
    }
}
