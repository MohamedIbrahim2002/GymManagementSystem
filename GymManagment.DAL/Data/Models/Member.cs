namespace GymManagment.DAL.Data.Models
{
    public class Member : GymUser
    {
        public string? Photo { get; set; }
        // join date = created at
        public HealthRecord HealthRecord { get; set; } = default!;
        public ICollection<Booking> Bookings { get; set; } = default!;
        public ICollection<Membership> Memberships { get; set; } = default!;
    }
}
