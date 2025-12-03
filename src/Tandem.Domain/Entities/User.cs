namespace Tandem.Domain.Entities;

public class User
{
    public Guid UserId { get; private set; }
    public string FirstName { get; private set; }
    public string? MiddleName { get; private set; }
    public string LastName { get; private set; }
    public string PhoneNumber { get; private set; }
    public string EmailAddress { get; private set; }

    private User()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        PhoneNumber = string.Empty;
        EmailAddress = string.Empty;
    }

    public User(
        string firstName,
        string? middleName,
        string lastName,
        string phoneNumber,
        string emailAddress)
    {
        UserId = Guid.NewGuid();
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        MiddleName = middleName;
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
        EmailAddress = emailAddress ?? throw new ArgumentNullException(nameof(emailAddress));
    }

    public void Update(
        string firstName,
        string? middleName,
        string lastName,
        string phoneNumber)
    {
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        MiddleName = middleName;
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        PhoneNumber = phoneNumber ?? throw new ArgumentNullException(nameof(phoneNumber));
    }

    public string GetFullName()
    {
        var nameParts = new List<string> { FirstName };
        
        if (!string.IsNullOrWhiteSpace(MiddleName))
        {
            nameParts.Add(MiddleName);
        }
        
        nameParts.Add(LastName);
        
        return string.Join(" ", nameParts);
    }
}

